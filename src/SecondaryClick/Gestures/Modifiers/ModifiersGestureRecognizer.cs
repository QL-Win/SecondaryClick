using System.WindowsInput;
using System.MouseKeyHook;
using System.WindowsInput.WinApi;
using System.MouseKeyHook.WinApi;

namespace SecondaryClick.Gestures.Modifiers;

/// <summary>
/// Gesture recognizer that implements modifier-key based secondary click gestures.
/// Allows users to trigger a right-click by holding modifier keys (Alt, Ctrl, Shift) and clicking the left mouse button.
/// Uses a low-level mouse/keyboard hook to intercept and process user input.
/// </summary>
public sealed partial class ModifiersGestureRecognizer : IGestureRecognizer
{
    /// <summary>
    /// Simulates keyboard and mouse input events.
    /// </summary>
    private readonly InputSimulator _simulator = new();

    /// <summary>
    /// Low-level keyboard and mouse hook for global input capture.
    /// </summary>
    private IKeyboardMouseEvents? _hook;

    /// <summary>
    /// Flag indicating whether a right mouse button down event has been injected by this recognizer.
    /// </summary>
    private bool _rightDownInjected;

    /// <summary>
    /// Flag to prevent re-entrant input processing of injected events.
    /// </summary>
    private bool _suppressInjected;

    /// <summary>
    /// Flag indicating whether this recognizer is currently enabled.
    /// </summary>
    private bool _enabled;

    /// <summary>
    /// Flag indicating whether the Alt key should be suppressed from appearing as a key event.
    /// </summary>
    private bool _suppressAltKey;

    /// <summary>
    /// The modifier keys that must be held to activate the gesture (default: Control).
    /// </summary>
    private GestureModifiers _activationModifiers = GestureModifiers.Control;

    /// <summary>
    /// Gets or sets the modifier keys that must be held to activate the secondary click gesture.
    /// </summary>
    public GestureModifiers ActivationModifiers
    {
        get => _activationModifiers;
        set
        {
            _activationModifiers = value;

            // Clear Alt suppression if Alt is no longer a required modifier
            if (!_activationModifiers.HasFlag(GestureModifiers.Alt))
            {
                _suppressAltKey = false;
            }
        }
    }

    /// <summary>
    /// Enables or disables the modifier gesture recognizer.
    /// When enabled, it installs the global hook to monitor user input.
    /// When disabled, it releases the hook and cleans up state.
    /// </summary>
    /// <param name=\"enabled\">True to enable the recognizer, false to disable.</param>
    public void SetEnabled(bool enabled)
    {
        if (_enabled == enabled)
            return;

        _enabled = enabled;

        if (enabled)
        {
            EnsureHook();
        }
        else
        {
            ReleaseHook();
        }
    }

    /// <summary>
    /// Disposes the gesture recognizer and releases the hook.
    /// </summary>
    public void Dispose()
    {
        ReleaseHook();
    }

    /// <summary>
    /// Ensures the global keyboard and mouse hook is installed and configured.
    /// </summary>
    private void EnsureHook()
    {
        if (_hook is not null)
            return;

        _hook = Hook.GlobalEvents();
        _hook.MouseDownExt += OnMouseDown;
        _hook.MouseUpExt += OnMouseUp;
        _hook.MouseMove += OnMouseMove;
        _hook.KeyDown += OnKeyDown;
        _hook.KeyUp += OnKeyUp;
    }

    /// <summary>
    /// Releases and uninstalls the global keyboard and mouse hook.
    /// </summary>
    private void ReleaseHook()
    {
        if (_hook is null)
            return;

        _hook.MouseDownExt -= OnMouseDown;
        _hook.MouseUpExt -= OnMouseUp;
        _hook.MouseMove -= OnMouseMove;
        _hook.KeyDown -= OnKeyDown;
        _hook.KeyUp -= OnKeyUp;
        _hook.Dispose();
        _hook = null;
        _rightDownInjected = false;
        _suppressInjected = false;
        _suppressAltKey = false;
    }

    /// <summary>
    /// Handles mouse down events. When modifier keys are pressed and left mouse button is clicked,
    /// injects a right mouse button down event instead.
    /// </summary>
    private void OnMouseDown(object? sender, MouseEventExtArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        GestureModifiers pressedModifiers = GetPressedActivationModifiers();
        if (pressedModifiers == GestureModifiers.None)
            return;

        if (e.Button == MouseButtons.Left)
        {
            _rightDownInjected = true;
            _suppressAltKey = pressedModifiers.HasFlag(GestureModifiers.Alt);
            QueueInjected(() => _simulator.Mouse.RightButtonDown());
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles mouse up events. When a right button up injection is pending, releases it.
    /// </summary>
    private void OnMouseUp(object? sender, MouseEventExtArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        if (e.Button == MouseButtons.Left && _rightDownInjected)
        {
            _rightDownInjected = false;
            QueueInjected(() => _simulator.Mouse.RightButtonUp());
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles mouse move events. If the mouse moves while right button is injected but modifiers are released,
    /// ends the injection.
    /// </summary>
    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        if (_rightDownInjected && !IsActivationModifierPressed())
        {
            QueueInjected(() => _simulator.Mouse.RightButtonUp());
            _rightDownInjected = false;
        }
    }

    /// <summary>
    /// Handles key up events. Suppresses Alt key events if necessary and manages the injected right click state.
    /// </summary>
    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        // Suppress Alt key if it was used to trigger the gesture
        if (IsAltKey(e.KeyCode))
        {
            if (_suppressAltKey)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                InjectAltKeyUp(e.KeyCode);
            }

            _suppressAltKey = false;
        }

        // End the injected right click if modifiers are released
        if (_rightDownInjected && !IsActivationModifierPressed())
        {
            QueueInjected(() => _simulator.Mouse.RightButtonUp());
            _rightDownInjected = false;
        }
    }

    /// <summary>
    /// Handles key down events. Suppresses Alt key down events if they are part of the gesture activation.
    /// </summary>
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        // Suppress Alt key down events if Alt is triggering the gesture
        if (_suppressAltKey && IsAltKey(e.KeyCode))
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    /// <summary>
    /// Determines if any activation modifier key is currently pressed.
    /// </summary>
    /// <returns>True if any configured modifier is pressed, false otherwise.</returns>
    private bool IsActivationModifierPressed()
    {
        return GetPressedActivationModifiers() != GestureModifiers.None;
    }

    /// <summary>
    /// Gets the currently pressed activation modifier keys.
    /// </summary>
    /// <returns>A combination of flags representing the pressed modifiers.</returns>
    private GestureModifiers GetPressedActivationModifiers()
    {
        GestureModifiers modifiers = GestureModifiers.None;

        if (_activationModifiers.HasFlag(GestureModifiers.Alt) && IsAltPressed())
        {
            modifiers |= GestureModifiers.Alt;
        }

        if (_activationModifiers.HasFlag(GestureModifiers.Control) && IsControlPressed())
        {
            modifiers |= GestureModifiers.Control;
        }

        if (_activationModifiers.HasFlag(GestureModifiers.Shift) && IsShiftPressed())
        {
            modifiers |= GestureModifiers.Shift;
        }

        return modifiers;
    }

    /// <summary>
    /// Checks if the Alt key (or either Alt key variant) is physically pressed.
    /// </summary>
    /// <returns>True if Alt is pressed, false otherwise.</returns>
    private bool IsAltPressed()
    {
        // Query physical key state to avoid sticky modifier state.
        const int VK_MENU = 0x12;
        const int VK_LMENU = 0xA4;
        const int VK_RMENU = 0xA5;
        return IsKeyDown(VK_MENU) || IsKeyDown(VK_LMENU) || IsKeyDown(VK_RMENU);
    }

    /// <summary>
    /// Checks if the Control key (or either Ctrl key variant) is physically pressed.
    /// </summary>
    /// <returns>True if Control is pressed, false otherwise.</returns>
    private bool IsControlPressed()
    {
        const int VK_CONTROL = 0x11;
        const int VK_LCONTROL = 0xA2;
        const int VK_RCONTROL = 0xA3;
        return IsKeyDown(VK_CONTROL) || IsKeyDown(VK_LCONTROL) || IsKeyDown(VK_RCONTROL);
    }

    /// <summary>
    /// Checks if the Shift key (or either Shift key variant) is physically pressed.
    /// </summary>
    /// <returns>True if Shift is pressed, false otherwise.</returns>
    private bool IsShiftPressed()
    {
        const int VK_SHIFT = 0x10;
        const int VK_LSHIFT = 0xA0;
        const int VK_RSHIFT = 0xA1;
        return IsKeyDown(VK_SHIFT) || IsKeyDown(VK_LSHIFT) || IsKeyDown(VK_RSHIFT);
    }

    /// <summary>
    /// Determines if a key code represents an Alt key.
    /// </summary>
    /// <param name=\"keyCode\">The key code to check.</param>
    /// <returns>True if the key is Alt or any Alt variant, false otherwise.</returns>
    private static bool IsAltKey(Keys keyCode)
    {
        return keyCode == Keys.Alt || keyCode == Keys.Menu || keyCode == Keys.LMenu || keyCode == Keys.RMenu;
    }

    /// <summary>
    /// Checks if a virtual key is currently in the down state using GetAsyncKeyState.
    /// </summary>
    /// <param name=\"virtualKey\">The virtual key code to check.</param>
    /// <returns>True if the key is down, false otherwise.</returns>
    private static bool IsKeyDown(int virtualKey)
    {
        return (WinApi.User32.GetAsyncKeyState(virtualKey) & 0x8000) != 0;
    }

    /// <summary>
    /// Injects an Alt key-up event to clear the modifier state in Windows.
    /// This is necessary because we suppressed the original Alt key-up event.
    /// </summary>
    /// <param name=\"keyCode\">The specific Alt key variant that was pressed.</param>
    private void InjectAltKeyUp(Keys keyCode)
    {
        // Replay the suppressed Alt key-up so Windows clears the modifier state.
        User32.VK vk = keyCode switch
        {
            Keys.LMenu => User32.VK.VK_LMENU,
            Keys.RMenu => User32.VK.VK_RMENU,
            _ => User32.VK.VK_MENU,
        };

        QueueInjected(() => _simulator.Keyboard.KeyUp(vk));
    }

    /// <summary>
    /// Queues an input action to be executed on a thread pool thread.
    /// Sets the suppress flag to prevent re-entrant input processing of the injected action.
    /// </summary>
    /// <param name=\"action\">The input action to execute.</param>
    private void QueueInjected(Action action)
    {
        // Run SendInput off the hook thread to avoid blocking mouse messages and to ignore re-entrant injections.
        ThreadPool.QueueUserWorkItem(_ =>
        {
            _suppressInjected = true;
            try
            {
                action();
            }
            finally
            {
                _suppressInjected = false;
            }
        });
    }
}

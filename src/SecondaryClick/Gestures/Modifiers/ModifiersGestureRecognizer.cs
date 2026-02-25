using System.WindowsInput;
using System.MouseKeyHook;
using System.WindowsInput.WinApi;
using System.MouseKeyHook.WinApi;

namespace SecondaryClick.Gestures.Modifiers;

public sealed class ModifiersGestureRecognizer : IGestureRecognizer
{
    private readonly InputSimulator _simulator = new();
    private IKeyboardMouseEvents? _hook;
    private bool _rightDownInjected;
    private bool _suppressInjected;
    private bool _enabled;
    private bool _suppressAltKey;
    private GestureModifiers _activationModifiers = GestureModifiers.Alt;

    public GestureModifiers ActivationModifiers
    {
        get => _activationModifiers;
        set
        {
            _activationModifiers = value;

            if (!_activationModifiers.HasFlag(GestureModifiers.Alt))
            {
                _suppressAltKey = false;
            }
        }
    }

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

    public void Dispose()
    {
        ReleaseHook();
    }

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

    private void OnMouseDown(object? sender, MouseEventExtArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        var pressedModifiers = GetPressedActivationModifiers();
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

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

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

        if (_rightDownInjected && !IsActivationModifierPressed())
        {
            QueueInjected(() => _simulator.Mouse.RightButtonUp());
            _rightDownInjected = false;
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!_enabled || _suppressInjected)
            return;

        if (_suppressAltKey && IsAltKey(e.KeyCode))
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    private bool IsActivationModifierPressed()
    {
        return GetPressedActivationModifiers() != GestureModifiers.None;
    }

    private GestureModifiers GetPressedActivationModifiers()
    {
        var modifiers = GestureModifiers.None;

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

    private bool IsAltPressed()
    {
        // Query physical key state to avoid sticky modifier state.
        const int VK_MENU = 0x12;
        const int VK_LMENU = 0xA4;
        const int VK_RMENU = 0xA5;
        return IsKeyDown(VK_MENU) || IsKeyDown(VK_LMENU) || IsKeyDown(VK_RMENU);
    }

    private bool IsControlPressed()
    {
        const int VK_CONTROL = 0x11;
        const int VK_LCONTROL = 0xA2;
        const int VK_RCONTROL = 0xA3;
        return IsKeyDown(VK_CONTROL) || IsKeyDown(VK_LCONTROL) || IsKeyDown(VK_RCONTROL);
    }

    private bool IsShiftPressed()
    {
        const int VK_SHIFT = 0x10;
        const int VK_LSHIFT = 0xA0;
        const int VK_RSHIFT = 0xA1;
        return IsKeyDown(VK_SHIFT) || IsKeyDown(VK_LSHIFT) || IsKeyDown(VK_RSHIFT);
    }

    private static bool IsAltKey(Keys keyCode)
    {
        return keyCode == Keys.Alt || keyCode == Keys.Menu || keyCode == Keys.LMenu || keyCode == Keys.RMenu;
    }

    private static bool IsKeyDown(int virtualKey)
    {
        return (WinApi.User32.GetAsyncKeyState(virtualKey) & 0x8000) != 0;
    }

    private void InjectAltKeyUp(Keys keyCode)
    {
        // Replay the suppressed Alt key-up so Windows clears the modifier state.
        var vk = keyCode switch
        {
            Keys.LMenu => User32.VK.VK_LMENU,
            Keys.RMenu => User32.VK.VK_RMENU,
            _ => User32.VK.VK_MENU,
        };

        QueueInjected(() => _simulator.Keyboard.KeyUp(vk));
    }

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

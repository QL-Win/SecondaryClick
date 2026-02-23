using System.WindowsInput.WinApi;

namespace System.WindowsInput;

/// <summary>
/// The service contract for a keyboard simulator for the Windows platform.
/// </summary>
public interface IKeyboardSimulator
{
    /// <summary>
    /// Gets the <see cref="IMouseSimulator"/> instance for simulating Mouse input.
    /// </summary>
    public IMouseSimulator Mouse { get; }

    /// <summary>
    /// Simulates a key down gesture.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyDown(User32.VK keyCode);

    /// <summary>
    /// Simulates a key down gesture with extended key option.
    /// </summary>
    /// <param name="isExtendedKey">Whether the key is extended.</param>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyDown(bool? isExtendedKey, User32.VK keyCode);

    /// <summary>
    /// Simulates a key press gesture.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyPress(User32.VK keyCode);

    /// <summary>
    /// Simulates a key press gesture with extended key option.
    /// </summary>
    /// <param name="isExtendedKey">Whether the key is extended.</param>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyPress(bool? isExtendedKey, User32.VK keyCode);

    /// <summary>
    /// Simulates a key press gesture for multiple keys.
    /// </summary>
    /// <param name="keyCodes">The key codes.</param>
    public IKeyboardSimulator KeyPress(params User32.VK[] keyCodes);

    /// <summary>
    /// Simulates a key press gesture for multiple keys with extended key option.
    /// </summary>
    /// <param name="isExtendedKey">Whether the key is extended.</param>
    /// <param name="keyCodes">The key codes.</param>
    public IKeyboardSimulator KeyPress(bool? isExtendedKey, params User32.VK[] keyCodes);

    /// <summary>
    /// Simulates a key up gesture.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyUp(User32.VK keyCode);

    /// <summary>
    /// Simulates a key up gesture with extended key option.
    /// </summary>
    /// <param name="isExtendedKey">Whether the key is extended.</param>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator KeyUp(bool? isExtendedKey, User32.VK keyCode);

    /// <summary>
    /// Simulates a modified keystroke gesture.
    /// </summary>
    /// <param name="modifierKeyCodes">The modifier key codes.</param>
    /// <param name="keyCodes">The key codes.</param>
    public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<User32.VK> modifierKeyCodes, IEnumerable<User32.VK> keyCodes);

    /// <summary>
    /// Simulates a modified keystroke gesture.
    /// </summary>
    /// <param name="modifierKeyCodes">The modifier key codes.</param>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<User32.VK> modifierKeyCodes, User32.VK keyCode);

    /// <summary>
    /// Simulates a modified keystroke gesture.
    /// </summary>
    /// <param name="modifierKey">The modifier key code.</param>
    /// <param name="keyCodes">The key codes.</param>
    public IKeyboardSimulator ModifiedKeyStroke(User32.VK modifierKey, IEnumerable<User32.VK> keyCodes);

    /// <summary>
    /// Simulates a modified keystroke gesture.
    /// </summary>
    /// <param name="modifierKeyCode">The modifier key code.</param>
    /// <param name="keyCode">The key code.</param>
    public IKeyboardSimulator ModifiedKeyStroke(User32.VK modifierKeyCode, User32.VK keyCode);

    /// <summary>
    /// Simulates text entry.
    /// </summary>
    /// <param name="text">The text to enter.</param>
    public IKeyboardSimulator TextEntry(string text);

    /// <summary>
    /// Simulates character entry.
    /// </summary>
    /// <param name="character">The character to enter.</param>
    public IKeyboardSimulator TextEntry(char character);

    /// <summary>
    /// Sleeps for the specified timeout in milliseconds.
    /// </summary>
    /// <param name="millsecondsTimeout">The timeout in milliseconds.</param>
    public IKeyboardSimulator Sleep(int millsecondsTimeout);

    /// <summary>
    /// Sleeps for the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    public IKeyboardSimulator Sleep(TimeSpan timeout);
}

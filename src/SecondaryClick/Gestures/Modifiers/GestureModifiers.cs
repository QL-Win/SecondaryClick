namespace SecondaryClick.Gestures.Modifiers;

/// <summary>
/// Represents modifier keys that can be used to activate a secondary click.
/// Supports bitwise combination of multiple modifiers.
/// </summary>
[Flags]
public enum GestureModifiers
{
    /// <summary>
    /// No modifier key.
    /// </summary>
    None = 0,

    /// <summary>
    /// The Alt key. (macOS: Option)
    /// </summary>
    Alt = 1,

    /// <summary>
    /// The Control (Ctrl) key. (macOS: Command)
    /// </summary>
    Control = 2,

    /// <summary>
    /// The Shift key. (macOS: Shift)
    /// </summary>
    Shift = 4,
}

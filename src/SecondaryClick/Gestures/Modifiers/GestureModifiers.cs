namespace SecondaryClick.Gestures.Modifiers;

/// <summary>
/// Represents modifier keys that can be used to activate a secondary click gesture.
/// Supports bitwise combination of multiple modifiers using the [Flags] attribute.
/// For example: <c>GestureModifiers.Alt | GestureModifiers.Shift</c> to require both Alt and Shift.
/// </summary>
[Flags]
public enum GestureModifiers
{
    /// <summary>
    /// No modifier key is required.
    /// </summary>
    None = 0,

    /// <summary>
    /// The Alt key (also called Menu key). Equivalent to Option key on macOS.
    /// Used as one of the modifier combination options for triggering secondary click.
    /// </summary>
    Alt = 1,

    /// <summary>
    /// The Control (Ctrl) key. Equivalent to Command key on macOS.
    /// Used as one of the modifier combination options for triggering secondary click.
    /// </summary>
    Control = 2,

    /// <summary>
    /// The Shift key.
    /// Used as one of the modifier combination options for triggering secondary click.
    /// </summary>
    Shift = 4,
}

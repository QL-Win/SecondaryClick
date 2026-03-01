using SecondaryClick.Gestures.Modifiers;
using SecondaryClick.Gestures.Touchpads;

namespace SecondaryClick.Gestures;

/// <summary>
/// Container class that holds all gesture recognizers used by the application.
/// Manages the lifecycle of recognizers for touchpad gestures and keyboard modifier-based gestures.
/// </summary>
public sealed class RecognizerHolder : IDisposable
{
    /// <summary>
    /// Gets the gesture recognizer for keyboard modifiers (Alt, Shift, Control + Mouse Click).
    /// </summary>
    public ModifiersGestureRecognizer ModifiersRecognizer { get; private set; } = new();
    
    /// <summary>
    /// Gets the gesture recognizer for touchpad-specific gestures (two-finger tap, right-click zone).
    /// </summary>
    public TouchpadGestureRecognizer TouchpadRecognizer { get; private set; } = new();

    /// <summary>
    /// Disposes all recognizers and releases their resources.
    /// </summary>
    public void Dispose()
    {
        ModifiersRecognizer.Dispose();
        TouchpadRecognizer.Dispose();
    }
}

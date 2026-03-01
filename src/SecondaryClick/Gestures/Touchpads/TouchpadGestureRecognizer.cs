namespace SecondaryClick.Gestures.Touchpads;

/// <summary>
/// Recognizer for precision touchpad-specific gestures.
/// Handles two-finger tap (as right-click) and right-click zone (corner click) features.
/// </summary>
public sealed class TouchpadGestureRecognizer : IGestureRecognizer
{
    /// <summary>
    /// Gets or sets whether two-finger tapping on the touchpad triggers a right-click.
    /// </summary>
    public bool IsTwoFingerTap
    {
        get => PrecisionTouchpadRegistrySettings.GetTwoFingerTapRightClickEnabled() ?? false;
        set => PrecisionTouchpadSPISettings.SetTwoFingerTapRightClickEnabled(value);
    }

    /// <summary>
    /// Gets or sets whether clicking in the bottom-right corner of the touchpad triggers a right-click.
    /// </summary>
    public bool IsRightClickZone
    {
        get => PrecisionTouchpadRegistrySettings.GetRightClickZoneEnabled() ?? false;
        set => PrecisionTouchpadSPISettings.SetRightClickZoneEnabled(value);
    }

    /// <summary>
    /// Disposes the gesture recognizer (nothing to dispose for this implementation).
    /// </summary>
    public void Dispose()
    {
        // Nothing to dispose.
    }
}

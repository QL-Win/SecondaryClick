namespace SecondaryClick.Gestures.Touchpads;

public sealed class TouchpadGestureRecognizer : IGestureRecognizer
{
    public bool IsTwoFingerTap
    {
        get => PrecisionTouchpadSettings.GetTwoFingerTapRightClickEnabled() ?? false;
        set => PrecisionTouchpadSettings.SetTwoFingerTapRightClickEnabled(value);
    }

    public bool IsRightClickZone
    {
        get => PrecisionTouchpadSettings.GetRightClickZoneEnabled() ?? false;
        set => PrecisionTouchpadSettings.SetRightClickZoneEnabled(value);
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }
}

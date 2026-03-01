namespace SecondaryClick.Gestures.Touchpads;

public sealed class TouchpadGestureRecognizer : IGestureRecognizer
{
    public bool IsTwoFingerTap
    {
        get => PrecisionTouchpadRegistrySettings.GetTwoFingerTapRightClickEnabled() ?? false;
        set => PrecisionTouchpadSPISettings.SetTwoFingerTapRightClickEnabled(value);
    }

    public bool IsRightClickZone
    {
        get => PrecisionTouchpadRegistrySettings.GetRightClickZoneEnabled() ?? false;
        set => PrecisionTouchpadSPISettings.SetRightClickZoneEnabled(value);
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }
}

using Microsoft.Win32;

namespace SecondaryClick.Gestures.Touchpads;

internal static class PrecisionTouchpadSettings
{
    private const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

    /// <summary>
    /// Read: whether two-finger tap is treated as right-click.
    /// </summary>
    public static bool? GetTwoFingerTapRightClickEnabled()
        => ReadBoolValue("TwoFingerTapEnabled");

    /// <summary>
    /// Set: treat two-finger tap as right-click.
    /// </summary>
    public static bool SetTwoFingerTapRightClickEnabled(bool enabled)
        => WriteBoolValue("TwoFingerTapEnabled", enabled);

    /// <summary>
    /// Read: whether pressing the bottom-right corner is treated as right-click.
    /// </summary>
    public static bool? GetRightClickZoneEnabled()
        => ReadBoolValue("RightClickZoneEnabled");

    /// <summary>
    /// Set: treat pressing the bottom-right corner as right-click.
    /// </summary>
    public static bool SetRightClickZoneEnabled(bool enabled)
        => WriteBoolValue("RightClickZoneEnabled", enabled);

    private static bool? ReadBoolValue(string valueName)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(KeyPath, writable: false);
        if (key == null)
            return null;

        object? value = key.GetValue(valueName);
        if (value == null)
            return null;

        // Windows uses REG_DWORD here.
        // 1 / 0xFFFFFFFF = true
        if (value is int intValue)
            return intValue != 0;

        return null;
    }

    private static bool WriteBoolValue(string valueName, bool enabled)
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.CreateSubKey(KeyPath);
            if (key == null)
                return false;

            // Writing 1 is enough; no need to write 0xFFFFFFFF.
            key.SetValue(
                valueName,
                enabled ? 1 : 0,
                RegistryValueKind.DWord);

            return true;
        }
        catch
        {
            return false;
        }
    }
}

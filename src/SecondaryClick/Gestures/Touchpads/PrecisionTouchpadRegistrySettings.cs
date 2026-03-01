using Microsoft.Win32;

namespace SecondaryClick.Gestures.Touchpads;

/// <summary>
/// https://learn.microsoft.com/en-us/windows-hardware/design/component-guidelines/touchpad-tuning-guidelines
/// </summary>
internal static class PrecisionTouchpadRegistrySettings
{
    private const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

    public static bool IsReadable
    {
        get
        {
            Version version = Environment.OSVersion.Version;

            // Check if the current OS is Windows 10 version 1511 (Build 10586) or later.
            return version.Major == 10 && version.Minor == 0 && version.Build >= 10586;
        }
    }

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
        if (value is int intValue)
            return (uint)intValue == 0xFFFFFFFF;

        return null;
    }

    private static bool WriteBoolValue(string valueName, bool enabled)
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.CreateSubKey(KeyPath);
            if (key == null)
                return false;

            key.SetValue(
                valueName,
                enabled ? unchecked((int)0xFFFFFFFF) : 0,
                RegistryValueKind.DWord);

            return true;
        }
        catch
        {
            return false;
        }
    }
}

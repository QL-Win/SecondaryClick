using Microsoft.Win32;

namespace SecondaryClick.Gestures.Touchpads;

/// <summary>
/// Provides registry-based access to precision touchpad settings on Windows.
/// References: https://learn.microsoft.com/en-us/windows-hardware/design/component-guidelines/touchpad-tuning-guidelines
/// This implementation reads touchpad configuration from the Windows registry.
/// </summary>
internal static class PrecisionTouchpadRegistrySettings
{
    /// <summary>
    /// Registry path for precision touchpad settings.
    /// </summary>
    private const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

    /// <summary>
    /// Gets a value indicating whether the current OS supports reading precision touchpad registry settings.
    /// Requires Windows 10 version 1511 (Build 10586) or later.
    /// </summary>
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
    /// Reads whether two-finger tap is treated as right-click from the registry.
    /// </summary>
    /// <returns>A nullable boolean indicating if two-finger tap right-click is enabled, or null if not available.</returns>
    public static bool? GetTwoFingerTapRightClickEnabled()
        => ReadBoolValue("TwoFingerTapEnabled");

    /// <summary>
    /// Sets whether two-finger tap should be treated as right-click in the registry.
    /// </summary>
    /// <param name="enabled">True to enable two-finger tap as right-click, false to disable.</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public static bool SetTwoFingerTapRightClickEnabled(bool enabled)
        => WriteBoolValue("TwoFingerTapEnabled", enabled);

    /// <summary>
    /// Reads whether pressing the bottom-right corner is treated as right-click from the registry.
    /// </summary>
    /// <returns>A nullable boolean indicating if right-click zone is enabled, or null if not available.</returns>
    public static bool? GetRightClickZoneEnabled()
        => ReadBoolValue("RightClickZoneEnabled");

    /// <summary>
    /// Sets whether pressing the bottom-right corner should be treated as right-click in the registry.
    /// </summary>
    /// <param name="enabled">True to enable right-click zone, false to disable.</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public static bool SetRightClickZoneEnabled(bool enabled)
        => WriteBoolValue("RightClickZoneEnabled", enabled);

    /// <summary>
    /// Reads a boolean value from the precision touchpad registry settings.
    /// </summary>
    /// <param name="valueName">The registry value name to read.</param>
    /// <returns>A nullable boolean, or null if the value is not found or not accessible.</returns>
    private static bool? ReadBoolValue(string valueName)
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(KeyPath, writable: false);
        if (key == null)
            return null;

        object? value = key.GetValue(valueName);
        if (value == null)
            return null;

        // Windows uses REG_DWORD where 0xFFFFFFFF = true, 0 = false
        if (value is int intValue)
            return (uint)intValue == 0xFFFFFFFF;

        return null;
    }

    /// <summary>
    /// Writes a boolean value to the precision touchpad registry settings.
    /// </summary>
    /// <param name="valueName">The registry value name to write.</param>
    /// <param name="enabled">The boolean value to write (0xFFFFFFFF for true, 0 for false).</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
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

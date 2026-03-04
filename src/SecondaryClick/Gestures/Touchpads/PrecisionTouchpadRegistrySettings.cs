using SecondaryClick.WinApi;

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
        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            KeyPath,
            0,
            Advapi32.KEY_QUERY_VALUE,
            out IntPtr keyHandle);

        if (openResult != Win32Error.ERROR_SUCCESS)
            return null;

        try
        {
            uint valueType;
            uint dataSize = 0;

            int queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                valueName,
                0,
                out valueType,
                null,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS)
                return null;

            if (valueType != Advapi32.REG_DWORD || dataSize < sizeof(int))
                return null;

            byte[] data = new byte[dataSize];
            queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                valueName,
                0,
                out valueType,
                data,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS)
                return null;

            int intValue = BitConverter.ToInt32(data, 0);
            return unchecked((uint)intValue) == 0xFFFFFFFF;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    /// <summary>
    /// Writes a boolean value to the precision touchpad registry settings.
    /// </summary>
    /// <param name="valueName">The registry value name to write.</param>
    /// <param name="enabled">The boolean value to write (0xFFFFFFFF for true, 0 for false).</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    private static bool WriteBoolValue(string valueName, bool enabled)
    {
        int createResult = Advapi32.RegCreateKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            KeyPath,
            0,
            null,
            0,
            Advapi32.KEY_SET_VALUE,
            IntPtr.Zero,
            out IntPtr keyHandle,
            out _);

        if (createResult != Win32Error.ERROR_SUCCESS)
            return false;

        try
        {
            byte[] data = BitConverter.GetBytes(enabled ? unchecked((int)0xFFFFFFFF) : 0);

            int setResult = Advapi32.RegSetValueEx(
                keyHandle,
                valueName,
                0,
                Advapi32.REG_DWORD,
                data,
                data.Length);

            return setResult == Win32Error.ERROR_SUCCESS;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }
}

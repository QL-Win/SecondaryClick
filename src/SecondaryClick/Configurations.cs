using SecondaryClick.WinApi;
using System.Text;

namespace SecondaryClick;

/// <summary>
/// Manages application configuration settings with persistent storage.
/// Stores modifier key configurations (Alt, Shift, Control) used for gesture recognition.
/// </summary>
internal static class Configurations
{
    /// <summary>
    /// Configuration for Alt key modifier activation. Defaults to false.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersAlt { get; } = new(nameof(ModifiersAlt), false);

    /// <summary>
    /// Configuration for Shift key modifier activation. Defaults to false.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersShift { get; } = new(nameof(ModifiersShift), false);

    /// <summary>
    /// Configuration for Control key modifier activation. Defaults to true.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersControl { get; } = new(nameof(ModifiersControl), true);

    /// <summary>
    /// Configuration for tray icon visibility. True means tray icon is hidden.
    /// </summary>
    public static ConfigurationDefinition<bool> HideTrayIcon { get; } = new(nameof(HideTrayIcon), false);
}

/// <summary>
/// Registry-backed configuration definition.
/// </summary>
/// <typeparam name="T">Configuration value type.</typeparam>
internal sealed class ConfigurationDefinition<T>(string name, T defaultValue)
{
    private const string RegistryPath = @"Software\SecondaryClick";

    private readonly string _name = name;
    private readonly T _defaultValue = defaultValue;

    public T Get()
    {
        if (!TryReadRegistryValue(out uint valueType, out byte[] data))
        {
            return _defaultValue;
        }

        try
        {
            if (typeof(T) == typeof(bool))
            {
                bool converted = ConvertToBool(valueType, data);

                return (T)(object)converted;
            }

            if (typeof(T) == typeof(string))
            {
                string stringValue = DecodeString(data);
                return (T)(object)stringValue;
            }

            if (valueType == Advapi32.REG_DWORD && data.Length >= sizeof(int))
            {
                int intValue = BitConverter.ToInt32(data, 0);
                return (T)Convert.ChangeType(intValue, typeof(T));
            }

            string valueAsString = DecodeString(data);
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
        catch
        {
            return _defaultValue;
        }
    }

    public void Set(T value)
    {
        int createResult = Advapi32.RegCreateKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            RegistryPath,
            0,
            null,
            0,
            Advapi32.KEY_SET_VALUE,
            IntPtr.Zero,
            out IntPtr keyHandle,
            out _);

        if (createResult != Win32Error.ERROR_SUCCESS)
            return;

        try
        {
            if (typeof(T) == typeof(bool))
            {
                bool boolValue = (bool)(object)value!;
                byte[] data = BitConverter.GetBytes(boolValue ? 1 : 0);
                _ = Advapi32.RegSetValueEx(
                    keyHandle,
                    _name,
                    0,
                    Advapi32.REG_DWORD,
                    data,
                    data.Length);
                return;
            }

            string stringValue = value?.ToString() ?? string.Empty;
            int dataSize = (stringValue.Length + 1) * sizeof(char);
            _ = Advapi32.RegSetValueEx(
                keyHandle,
                _name,
                0,
                Advapi32.REG_SZ,
                stringValue,
                dataSize);
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    private bool TryReadRegistryValue(out uint valueType, out byte[] data)
    {
        valueType = 0;
        data = [];

        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            RegistryPath,
            0,
            Advapi32.KEY_QUERY_VALUE,
            out IntPtr keyHandle);

        if (openResult != Win32Error.ERROR_SUCCESS)
            return false;

        try
        {
            uint dataSize = 0;
            int queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                _name,
                0,
                out valueType,
                null,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS || dataSize == 0)
                return false;

            data = new byte[dataSize];
            queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                _name,
                0,
                out valueType,
                data,
                ref dataSize);

            return queryResult == Win32Error.ERROR_SUCCESS;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    private static bool ConvertToBool(uint valueType, byte[] data)
    {
        if (valueType == Advapi32.REG_DWORD && data.Length >= sizeof(int))
        {
            return BitConverter.ToInt32(data, 0) != 0;
        }

        string text = DecodeString(data);
        if (bool.TryParse(text, out bool parsedBool))
            return parsedBool;

        if (int.TryParse(text, out int parsedInt))
            return parsedInt != 0;

        return false;
    }

    private static string DecodeString(byte[] data)
    {
        return Encoding.Unicode.GetString(data).TrimEnd('\0');
    }
}

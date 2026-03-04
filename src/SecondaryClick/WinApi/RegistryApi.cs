using System.Text;

namespace SecondaryClick.WinApi;

/// <summary>
/// Provides high-level registry read/write helpers built on top of Advapi32.
/// </summary>
internal static class RegistryApi
{
    /// <summary>
    /// Determines whether a registry value type is DWORD.
    /// </summary>
    /// <param name="valueType">The registry value type code.</param>
    /// <returns>True if the value type is REG_DWORD; otherwise false.</returns>
    public static bool IsDwordType(uint valueType)
        => valueType == Advapi32.REG_DWORD;

    /// <summary>
    /// Reads raw registry value data from HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to read.</param>
    /// <param name="valueType">When successful, receives the registry value type.</param>
    /// <param name="data">When successful, receives the raw value bytes.</param>
    /// <returns>True if the value is read successfully; otherwise false.</returns>
    public static bool TryReadRawCurrentUser(string subKeyPath, string valueName, out uint valueType, out byte[] data)
    {
        valueType = 0;
        data = [];

        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            subKeyPath,
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
                valueName,
                0,
                out valueType,
                null,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS)
                return false;

            if (dataSize == 0)
            {
                data = [];
                return true;
            }

            data = new byte[dataSize];
            queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                valueName,
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

    /// <summary>
    /// Reads a DWORD registry value from HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to read.</param>
    /// <param name="value">When successful, receives the DWORD value.</param>
    /// <returns>True if a valid DWORD value is read; otherwise false.</returns>
    public static bool TryReadDwordCurrentUser(string subKeyPath, string valueName, out int value)
    {
        value = 0;
        if (!TryReadRawCurrentUser(subKeyPath, valueName, out uint valueType, out byte[] data))
            return false;

        if (!IsDwordType(valueType) || data.Length < sizeof(int))
            return false;

        value = BitConverter.ToInt32(data, 0);
        return true;
    }

    /// <summary>
    /// Reads a string registry value (REG_SZ or REG_EXPAND_SZ) from HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to read.</param>
    /// <param name="value">When successful, receives the decoded string value.</param>
    /// <returns>True if a supported string value is read; otherwise false.</returns>
    public static bool TryReadStringCurrentUser(string subKeyPath, string valueName, out string value)
    {
        value = string.Empty;
        if (!TryReadRawCurrentUser(subKeyPath, valueName, out uint valueType, out byte[] data))
            return false;

        if (valueType != Advapi32.REG_SZ && valueType != Advapi32.REG_EXPAND_SZ)
            return false;

        value = DecodeUnicodeString(data);
        return true;
    }

    /// <summary>
    /// Writes a DWORD registry value to HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to write.</param>
    /// <param name="value">The DWORD value to write.</param>
    /// <returns>True if the value is written successfully; otherwise false.</returns>
    public static bool SetDwordCurrentUser(string subKeyPath, string valueName, int value)
    {
        int createResult = Advapi32.RegCreateKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            subKeyPath,
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
            byte[] data = BitConverter.GetBytes(value);
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

    /// <summary>
    /// Writes a string registry value (REG_SZ) to HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to write.</param>
    /// <param name="value">The string value to write.</param>
    /// <returns>True if the value is written successfully; otherwise false.</returns>
    public static bool SetStringCurrentUser(string subKeyPath, string valueName, string value)
    {
        int createResult = Advapi32.RegCreateKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            subKeyPath,
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
            int dataSize = (value.Length + 1) * sizeof(char);
            int setResult = Advapi32.RegSetValueEx(
                keyHandle,
                valueName,
                0,
                Advapi32.REG_SZ,
                value,
                dataSize);

            return setResult == Win32Error.ERROR_SUCCESS;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    /// <summary>
    /// Deletes a registry value from HKEY_CURRENT_USER.
    /// </summary>
    /// <param name="subKeyPath">The subkey path under HKEY_CURRENT_USER.</param>
    /// <param name="valueName">The value name to delete.</param>
    /// <returns>True if deletion succeeds or the value does not exist; otherwise false.</returns>
    public static bool DeleteValueCurrentUser(string subKeyPath, string valueName)
    {
        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            subKeyPath,
            0,
            Advapi32.KEY_SET_VALUE,
            out IntPtr keyHandle);

        if (openResult == Win32Error.ERROR_FILE_NOT_FOUND)
            return true;

        if (openResult != Win32Error.ERROR_SUCCESS)
            return false;

        try
        {
            int deleteResult = Advapi32.RegDeleteValue(keyHandle, valueName);
            return deleteResult == Win32Error.ERROR_SUCCESS
                || deleteResult == Win32Error.ERROR_FILE_NOT_FOUND;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    /// <summary>
    /// Decodes UTF-16 (Unicode) registry bytes to a managed string and trims trailing null terminators.
    /// </summary>
    /// <param name="data">The UTF-16 encoded byte array.</param>
    /// <returns>The decoded string value.</returns>
    public static string DecodeUnicodeString(byte[] data)
        => Encoding.Unicode.GetString(data).TrimEnd('\0');
}
using System.Runtime.InteropServices;

namespace SecondaryClick.WinApi;

/// <summary>
/// Provides P/Invoke declarations for Advapi32.dll registry functions.
/// </summary>
internal static class Advapi32
{
    /// <summary>
    /// Predefined handle to the HKEY_CURRENT_USER root key.
    /// </summary>
    public static readonly nuint HKEY_CURRENT_USER = 0x80000001u;

    /// <summary>
    /// Access right for querying key value data.
    /// </summary>
    public const int KEY_QUERY_VALUE = 0x0001;

    /// <summary>
    /// Access right for setting key value data.
    /// </summary>
    public const int KEY_SET_VALUE = 0x0002;

    /// <summary>
    /// Registry value type for a null-terminated Unicode string.
    /// </summary>
    public const uint REG_SZ = 1;

    /// <summary>
    /// Registry value type for an expandable null-terminated Unicode string.
    /// </summary>
    public const uint REG_EXPAND_SZ = 2;

    /// <summary>
    /// Registry value type for a 32-bit number.
    /// </summary>
    public const uint REG_DWORD = 4;

    /// <summary>
    /// Registry value type for a 64-bit number.
    /// </summary>
    public const uint REG_QWORD = 11;

    /// <summary>
    /// Opens the specified registry key.
    /// </summary>
    /// <param name="hKey">A handle to an open registry key (for example, HKEY_CURRENT_USER).</param>
    /// <param name="lpSubKey">The name of the registry subkey to open.</param>
    /// <param name="ulOptions">Reserved; must be zero.</param>
    /// <param name="samDesired">The access rights requested for the key.</param>
    /// <param name="phkResult">When successful, receives a handle to the opened key.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegOpenKeyEx(
        nuint hKey,
        string lpSubKey,
        int ulOptions,
        int samDesired,
        out nint phkResult);

    /// <summary>
    /// Creates or opens the specified registry key.
    /// </summary>
    /// <param name="hKey">A handle to an open registry key (for example, HKEY_CURRENT_USER).</param>
    /// <param name="lpSubKey">The name of the registry subkey to create or open.</param>
    /// <param name="Reserved">Reserved; must be zero.</param>
    /// <param name="lpClass">The user-defined class type for this key. Typically null.</param>
    /// <param name="dwOptions">Special options for key creation. Typically zero.</param>
    /// <param name="samDesired">The access rights requested for the key.</param>
    /// <param name="lpSecurityAttributes">Security descriptor pointer. Typically 0.</param>
    /// <param name="phkResult">When successful, receives a handle to the opened or created key.</param>
    /// <param name="lpdwDisposition">Receives status indicating whether key was created or opened.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegCreateKeyEx(
        nuint hKey,
        string lpSubKey,
        int Reserved,
        string? lpClass,
        uint dwOptions,
        int samDesired,
        nint lpSecurityAttributes,
        out nint phkResult,
        out uint lpdwDisposition);

    /// <summary>
    /// Sets the data and type of a named value under an open registry key (string overload).
    /// </summary>
    /// <param name="hKey">A handle to an open registry key.</param>
    /// <param name="lpValueName">The name of the value to set.</param>
    /// <param name="Reserved">Reserved; must be zero.</param>
    /// <param name="dwType">The registry value type.</param>
    /// <param name="lpData">The string data to store.</param>
    /// <param name="cbData">The size of data in bytes, including null terminator for strings.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegSetValueEx(
        nint hKey,
        string lpValueName,
        int Reserved,
        uint dwType,
        string lpData,
        int cbData);

    /// <summary>
    /// Sets the data and type of a named value under an open registry key (byte array overload).
    /// </summary>
    /// <param name="hKey">A handle to an open registry key.</param>
    /// <param name="lpValueName">The name of the value to set.</param>
    /// <param name="Reserved">Reserved; must be zero.</param>
    /// <param name="dwType">The registry value type.</param>
    /// <param name="lpData">The raw byte data to store.</param>
    /// <param name="cbData">The size of data in bytes.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern int RegSetValueEx(
        nint hKey,
        string lpValueName,
        int Reserved,
        uint dwType,
        byte[] lpData,
        int cbData);

    /// <summary>
    /// Retrieves the type and data for a specified registry value.
    /// </summary>
    /// <param name="hKey">A handle to an open registry key.</param>
    /// <param name="lpValueName">The name of the value to query.</param>
    /// <param name="lpReserved">Reserved; must be zero.</param>
    /// <param name="lpType">When successful, receives the registry value type.</param>
    /// <param name="lpData">A buffer that receives value data, or null to query required size.</param>
    /// <param name="lpcbData">On input, buffer size; on output, required or actual size in bytes.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegQueryValueEx(
        nint hKey,
        string lpValueName,
        int lpReserved,
        out uint lpType,
        byte[]? lpData,
        ref uint lpcbData);

    /// <summary>
    /// Removes a named value from the specified registry key.
    /// </summary>
    /// <param name="hKey">A handle to an open registry key.</param>
    /// <param name="lpValueName">The name of the value to delete.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegDeleteValue(
        nint hKey,
        string lpValueName);

    /// <summary>
    /// Closes a handle to the specified registry key.
    /// </summary>
    /// <param name="hKey">A handle to an open registry key.</param>
    /// <returns>Win32 error code. ERROR_SUCCESS indicates success.</returns>
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern int RegCloseKey(nint hKey);
}

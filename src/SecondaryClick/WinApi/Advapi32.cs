using System.Runtime.InteropServices;

namespace SecondaryClick.WinApi;

/// <summary>
/// Provides P/Invoke declarations for Advapi32.dll registry functions.
/// </summary>
internal static class Advapi32
{
    public static readonly UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001u;

    public const int KEY_QUERY_VALUE = 0x0001;
    public const int KEY_SET_VALUE = 0x0002;

    public const uint REG_SZ = 1;
    public const uint REG_EXPAND_SZ = 2;

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegOpenKeyEx(
        UIntPtr hKey,
        string lpSubKey,
        int ulOptions,
        int samDesired,
        out IntPtr phkResult);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegCreateKeyEx(
        UIntPtr hKey,
        string lpSubKey,
        int Reserved,
        string? lpClass,
        uint dwOptions,
        int samDesired,
        IntPtr lpSecurityAttributes,
        out IntPtr phkResult,
        out uint lpdwDisposition);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegSetValueEx(
        IntPtr hKey,
        string lpValueName,
        int Reserved,
        uint dwType,
        string lpData,
        int cbData);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegQueryValueEx(
        IntPtr hKey,
        string lpValueName,
        int lpReserved,
        out uint lpType,
        byte[]? lpData,
        ref uint lpcbData);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int RegDeleteValue(
        IntPtr hKey,
        string lpValueName);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern int RegCloseKey(IntPtr hKey);
}
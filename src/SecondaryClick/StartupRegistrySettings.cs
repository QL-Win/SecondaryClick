using SecondaryClick.WinApi;
using System.Diagnostics;
using System.Text;

namespace SecondaryClick;

/// <summary>
/// Provides startup registration management through HKCU Run registry values.
/// </summary>
internal static class StartupRegistrySettings
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string StartupValueName = "SecondaryClick";

    public static bool IsEnabled()
        => TryGetStartupCommand(out _);

    public static bool SetEnabled(bool enabled)
    {
        return enabled
            ? SetStartupCommand(BuildStartupCommand())
            : DeleteStartupCommand();
    }

    private static string BuildStartupCommand()
    {
        string executablePath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
        return $"\"{executablePath}\"";
    }

    private static bool TryGetStartupCommand(out string? startupCommand)
    {
        startupCommand = null;

        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            RunKeyPath,
            0,
            Advapi32.KEY_QUERY_VALUE,
            out IntPtr keyHandle);

        if (openResult != Win32Error.ERROR_SUCCESS)
            return false;

        try
        {
            uint type;
            uint dataSize = 0;

            int queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                StartupValueName,
                0,
                out type,
                null,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS)
                return false;

            if (type != Advapi32.REG_SZ && type != Advapi32.REG_EXPAND_SZ)
                return false;

            if (dataSize == 0)
                return false;

            byte[] data = new byte[dataSize];

            queryResult = Advapi32.RegQueryValueEx(
                keyHandle,
                StartupValueName,
                0,
                out type,
                data,
                ref dataSize);

            if (queryResult != Win32Error.ERROR_SUCCESS)
                return false;

            startupCommand = Encoding.Unicode.GetString(data).TrimEnd('\0');
            return !string.IsNullOrWhiteSpace(startupCommand);
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    private static bool SetStartupCommand(string startupCommand)
    {
        int createResult = Advapi32.RegCreateKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            RunKeyPath,
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
            int dataSize = (startupCommand.Length + 1) * sizeof(char);

            int setResult = Advapi32.RegSetValueEx(
                keyHandle,
                StartupValueName,
                0,
                Advapi32.REG_SZ,
                startupCommand,
                dataSize);

            return setResult == Win32Error.ERROR_SUCCESS;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }

    private static bool DeleteStartupCommand()
    {
        int openResult = Advapi32.RegOpenKeyEx(
            Advapi32.HKEY_CURRENT_USER,
            RunKeyPath,
            0,
            Advapi32.KEY_SET_VALUE,
            out IntPtr keyHandle);

        if (openResult == Win32Error.ERROR_FILE_NOT_FOUND)
            return true;

        if (openResult != Win32Error.ERROR_SUCCESS)
            return false;

        try
        {
            int deleteResult = Advapi32.RegDeleteValue(keyHandle, StartupValueName);
            return deleteResult == Win32Error.ERROR_SUCCESS
                || deleteResult == Win32Error.ERROR_FILE_NOT_FOUND;
        }
        finally
        {
            _ = Advapi32.RegCloseKey(keyHandle);
        }
    }
}
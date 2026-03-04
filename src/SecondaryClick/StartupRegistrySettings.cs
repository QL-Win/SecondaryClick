using SecondaryClick.WinApi;
using System.Diagnostics;

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

        if (!RegistryApi.TryReadStringCurrentUser(RunKeyPath, StartupValueName, out string command))
            return false;

        startupCommand = command;
        return !string.IsNullOrWhiteSpace(startupCommand);
    }

    private static bool SetStartupCommand(string startupCommand)
    {
        return RegistryApi.SetStringCurrentUser(RunKeyPath, StartupValueName, startupCommand);
    }

    private static bool DeleteStartupCommand()
    {
        return RegistryApi.DeleteValueCurrentUser(RunKeyPath, StartupValueName);
    }
}
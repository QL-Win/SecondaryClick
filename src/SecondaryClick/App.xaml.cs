using System.Diagnostics;
using System.NativeTray;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace SecondaryClick;

public partial class App : Application
{
    private Mutex? _isRunning;

    static App()
    {
        // Explicitly set to PerMonitor to avoid being overridden by the system
        Debug.WriteLine(DpiAware.SetProcessDpiAwareness() ? "DPI Awareness applied successfully" : "DPI Awareness manual setup failed.");
    }

    public App()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!EnsureFirstInstance())
        {
            Shutdown();
            return;
        }

        TrayIconManager.Start();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        TrayIconManager.GetInstance().Dispose();
        _isRunning?.Dispose();
        base.OnExit(e);
    }

    private bool EnsureFirstInstance()
    {
        _isRunning = new Mutex(true, "SecondaryClick.App.Mutex", out bool isFirst);

        if (isFirst)
            return true;

        // Second instance: duplicate
        MessageBox.Show("另一個 SecondaryClick 程式正在執行", "SecondaryClick",
            MessageBoxButton.OK, MessageBoxImage.Information);

        return false;
    }
}

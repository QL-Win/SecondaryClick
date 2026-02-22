using System.Diagnostics;
using System.Windows;
using Vanara.PInvoke;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace SecondaryClick;

public partial class App : Application
{
    private Mutex? _isRunning;

    static App()
    {
        // Explicitly set to PerMonitor to avoid being overridden by the system
        if (SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE) is HRESULT result)
        {
            Debug.WriteLine(
                result == 0 ?
                "DPI Awareness applied successfully" :
                $"DPI Awareness manual setup failed. Error Code: {result}"
            );
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!EnsureFirstInstance())
        {
            Shutdown();
            return;
        }

        TrayIconManager.GetInstance();
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

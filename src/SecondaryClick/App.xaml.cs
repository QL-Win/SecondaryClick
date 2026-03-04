using System.Diagnostics;
using System.NativeTray;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace SecondaryClick;

/// <summary>
/// The main application class that initializes the SecondaryClick application.
/// Manages the application lifecycle, configuration, and ensures only one instance runs at a time.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Mutex to ensure only one instance of the application is running.
    /// </summary>
    private Mutex? _isRunning;

    /// <summary>
    /// Static constructor to initialize DPI awareness settings.
    /// Ensures the application properly handles high-resolution displays.
    /// </summary>
    static App()
    {
        // Explicitly set to PerMonitor to avoid being overridden by the system
        Debug.WriteLine(DpiAware.SetProcessDpiAwareness() ? "DPI Awareness applied successfully" : "DPI Awareness manual setup failed.");
    }

    /// <summary>
    /// Default constructor that initializes the application configuration.
    /// Sets up the YAML configuration file in the My Documents folder.
    /// </summary>
    public App()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    /// <summary>
    /// Handles the startup event of the application.
    /// Ensures only one instance is running and initializes the tray icon manager.
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        if (!EnsureFirstInstance())
        {
            Shutdown();
            return;
        }

        TrayIconManager.Start();
    }

    /// <summary>
    /// Handles the exit event of the application.
    /// Releases resources including the tray icon and mutex.
    /// </summary>
    protected override void OnExit(ExitEventArgs e)
    {
        TrayIconManager.GetInstance().Dispose();
        _isRunning?.Dispose();
        base.OnExit(e);
    }

    /// <summary>
    /// Ensures that only one instance of the application is running.
    /// Shows a message if another instance is already active.
    /// </summary>
    /// <returns>True if this is the first instance, false otherwise.</returns>
    private bool EnsureFirstInstance()
    {
        _isRunning = new Mutex(true, "SecondaryClick.App.Mutex", out bool isFirst);

        if (isFirst)
            return true;

        // Second instance is detected - notify the user
        MessageBox.Show(SH.AnotherInstanceRunning, "SecondaryClick",
            MessageBoxButton.OK, MessageBoxImage.Information);

        return false;
    }
}

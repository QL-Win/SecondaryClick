using System.Diagnostics;
using System.Drawing;
using System.NativeTray;
using System.Reflection;
using Vanara.PInvoke;
using Application = System.Windows.Application;

namespace SecondaryClick;

internal sealed partial class TrayIconManager : IDisposable
{
    private static TrayIconManager? _instance;

    private readonly TrayIconHost _icon;
    private readonly TrayMenuItem _altAsRightMenuItem;
    private readonly SecondaryClickHandler _altHandler;

    private TrayIconManager()
    {
        _altHandler = new SecondaryClickHandler();
        _altHandler.SetEnabled(true);

        _altAsRightMenuItem = new TrayMenuItem
        {
            Header = "辅助点按",
            IsChecked = true,
            Command = ToggleAltAsRightClickMode,
        };

        _icon = new TrayIconHost
        {
            ToolTipText = "Secondary Click",
            ThemeMode = TrayThemeMode.System,
            Icon = Icon.ExtractAssociatedIcon(Process.GetCurrentProcess().MainModule?.FileName!)!.Handle,
            Menu =
            [
                new TrayMenuItem
                {
                    Header = $"v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}{(IsRunningAsUWP() ? " (UWP)" : string.Empty)}",
                    IsEnabled = false,
                },
                new TraySeparator(),
                _altAsRightMenuItem,
                new TraySeparator(),
                new TrayMenuItem
                {
                    Header = "退出程式",
                    Command = static _ => Application.Current.Shutdown(),
                }
            ],
        };
    }

    public void Dispose()
    {
        _altHandler.Dispose();
        _icon.IsVisible = false;
    }

    public static void ShowNotification(string title, string content, bool isError = false, int timeout = 5000,
        Action? clickEvent = null,
        Action? closeEvent = null)
    {
        var icon = GetInstance()._icon;
        icon.ShowBalloonTip(timeout, title, content, isError ? TrayToolTipIcon.Error : TrayToolTipIcon.Info);
        icon.BalloonTipClicked += OnIconOnBalloonTipClicked;
        icon.BalloonTipClosed += OnIconOnBalloonTipClosed;

        void OnIconOnBalloonTipClicked(object sender, EventArgs e)
        {
            clickEvent?.Invoke();
            icon.BalloonTipClicked -= OnIconOnBalloonTipClicked;
        }

        void OnIconOnBalloonTipClosed(object sender, EventArgs e)
        {
            closeEvent?.Invoke();
            icon.BalloonTipClosed -= OnIconOnBalloonTipClosed;
        }
    }

    public static TrayIconManager GetInstance()
    {
        return _instance ??= new TrayIconManager();
    }

    private void ToggleAltAsRightClickMode(object? _)
    {
        var newValue = !_altAsRightMenuItem.IsChecked;
        _altAsRightMenuItem.IsChecked = newValue;
        _altHandler.SetEnabled(newValue);
    }

    private static bool IsRunningAsUWP()
    {
        if (Environment.OSVersion.Version < new Version(6, 2)) // Windows 8
            return false;

        try
        {
            uint len = 0;
            Win32Error r = Kernel32.GetCurrentPackageFullName(ref len, null);

            return r == Win32Error.ERROR_INSUFFICIENT_BUFFER;
        }
        catch (EntryPointNotFoundException)
        {
            return false;
        }
    }
}

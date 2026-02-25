using System.Windows;

namespace SecondaryClick;

/// <summary>
/// This application has no main window/no settings window.
/// This window is only used to speed up the creation of trayicon's right-click menu and is destroyed immediately after creation.
/// </summary>
public partial class TrayIconWindow : Window, IDisposable
{
    public TrayIconWindow()
    {
        Title = "TrayIconWindow";
        Width = 0;
        Height = 0;
        AllowsTransparency = true;
        Opacity = 0;
        ShowInTaskbar = false;
        WindowStyle = WindowStyle.None;
    }

    public void Dispose()
    {
        Close();
    }
}

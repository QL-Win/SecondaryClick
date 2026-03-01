using SecondaryClick.Gestures;
using SecondaryClick.WinApi;
using System.Diagnostics;
using System.Drawing;
using System.NativeTray;
using System.Reflection;
using System.Windows;

namespace SecondaryClick;

internal sealed partial class TrayIconManager : IDisposable
{
    private static TrayIconManager? _instance;
    private readonly TrayIconHost _icon;
    private readonly RecognizerHolder _recognizerHolder;

    private TrayIconManager()
    {
        _recognizerHolder = new();
        //_recognizerHolder.ModifiersRecognizer.SetEnabled(true);

        _icon = new TrayIconHost
        {
            ToolTipText = $"SecondaryClick v{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}",
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
                new TrayMenuItem
                {
                    Tag = "Touchpads",
                    Header = "触控辅助",
                    IsChecked = true,
                    Menu =
                    [
                        new TrayMenuItem
                        {
                            Tag = "TwoFingerTap",
                            Header = "双指点按或轻点",
                            IsChecked = _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap,
                            Command = _ =>
                            {
                                _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap
                                    = !_recognizerHolder.TouchpadRecognizer.IsTwoFingerTap;
                            },
                        },
                        new TrayMenuItem
                        {
                            Tag = "RightClickZone",
                            Header = "点按右下角",
                            IsChecked = _recognizerHolder.TouchpadRecognizer.IsRightClickZone,
                            Command = _ =>
                            {
                                _recognizerHolder.TouchpadRecognizer.IsRightClickZone
                                    = !_recognizerHolder.TouchpadRecognizer.IsRightClickZone;
                            },
                        },
                    ],
                },
                new TrayMenuItem
                {
                    Tag = "Modifiers",
                    Header = "键盘辅助",
                    IsChecked = false,
                    Menu =
                    [
                        new TrayMenuItem
                        {
                            Tag = "ModifiersOff",
                            Header = "关",
                            IsChecked = false,
                        },
                        new TrayMenuItem
                        {
                            Tag = "ModifiersAlt",
                            Header = "Alt 键",
                            IsChecked = true,
                        },
                        new TrayMenuItem
                        {
                            Tag = "ModifiersControl",
                            Header = "Control 键",
                            IsChecked = false,
                        },
                        new TrayMenuItem
                        {
                            Tag = "ModifiersShift",
                            Header = "Shift 键",
                            IsChecked = false,
                        },
                    ]
                },
                new TraySeparator(),
                new TrayMenuItem
                {
                    Tag = "Exit",
                    Header = "退出程式",
                    Command = static _ => Application.Current.Shutdown(),
                }
            ],
        };
    }

    public void Dispose()
    {
        _recognizerHolder.Dispose();
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

    public static void Start()
    {
        using TrayIconWindow window = new();
        window.Show();
        _ = GetInstance();
    }

    private static bool IsRunningAsUWP()
    {
        if (Environment.OSVersion.Version < new Version(6, 2)) // Windows 8
            return false;

        try
        {
            uint len = 0;
            uint r = Kernel32.GetCurrentPackageFullName(ref len, null);

            return r == Win32Error.ERROR_INSUFFICIENT_BUFFER;
        }
        catch (EntryPointNotFoundException)
        {
            return false;
        }
    }
}

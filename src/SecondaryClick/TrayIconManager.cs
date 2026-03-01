using Fischless.Configuration;
using SecondaryClick.Gestures;
using SecondaryClick.Gestures.Touchpads;
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

        if (Configurations.ModifiersAlt.Get())
        {
            _recognizerHolder.ModifiersRecognizer.ActivationModifiers
                |= Gestures.Modifiers.GestureModifiers.Alt;
        }
        if (Configurations.ModifiersControl.Get())
        {
            _recognizerHolder.ModifiersRecognizer.ActivationModifiers
                |= Gestures.Modifiers.GestureModifiers.Control;
        }
        if (Configurations.ModifiersShift.Get())
        {
            _recognizerHolder.ModifiersRecognizer.ActivationModifiers
                |= Gestures.Modifiers.GestureModifiers.Shift;
        }
        _recognizerHolder.ModifiersRecognizer.SetEnabled(
            Configurations.ModifiersAlt.Get()
                | Configurations.ModifiersControl.Get()
                | Configurations.ModifiersShift.Get()
        );

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
                    Tag = nameof(SH.Touchpads),
                    Header = SH.Touchpads,
                    IsChecked = true,
                    Menu =
                    [
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.TwoFingerTap),
                            Header = SH.TwoFingerTap,
                            IsChecked = _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap,
                            IsEnabled = PrecisionTouchpadSPISettings.IsWritable,
                            IsVisible = PrecisionTouchpadRegistrySettings.IsReadable,
                            Command = _ =>
                            {
                                if (PrecisionTouchpadSPISettings.IsWritable)
                                {
                                    _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap
                                        = !_recognizerHolder.TouchpadRecognizer.IsTwoFingerTap;
                                }
                            },
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.RightClickZone),
                            Header = SH.RightClickZone,
                            IsChecked = _recognizerHolder.TouchpadRecognizer.IsRightClickZone,
                            IsEnabled = PrecisionTouchpadSPISettings.IsWritable,
                            IsVisible = PrecisionTouchpadRegistrySettings.IsReadable,
                            Command = _ =>
                            {
                                if (PrecisionTouchpadSPISettings.IsWritable)
                                {
                                    _recognizerHolder.TouchpadRecognizer.IsRightClickZone
                                        = !_recognizerHolder.TouchpadRecognizer.IsRightClickZone;
                                }
                            },
                        },
                    ],
                },
                new TrayMenuItem
                {
                    Tag = nameof(SH.Modifiers),
                    Header = SH.Modifiers,
                    IsChecked = false,
                    Menu =
                    [
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersOff),
                            Header = SH.ModifiersOff,
                            IsChecked = false,
                            Command = static _ =>
                            {
                                Configurations.ModifiersAlt.Set(false);
                                Configurations.ModifiersControl.Set(false);
                                Configurations.ModifiersShift.Set(false);
                                ConfigurationManager.Save();

                                GetInstance()._recognizerHolder.ModifiersRecognizer.SetEnabled(false);
                            },
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersAlt),
                            Header = SH.ModifiersAlt,
                            IsChecked = true,
                            Command = static _ =>
                            {
                                Configurations.ModifiersAlt.Set(!Configurations.ModifiersAlt.Get());
                                ConfigurationManager.Save();

                                GetInstance()._recognizerHolder.ModifiersRecognizer.ActivationModifiers
                                    |= Gestures.Modifiers.GestureModifiers.Alt;
                                GetInstance()._recognizerHolder.ModifiersRecognizer.SetEnabled(true);
                            },
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersControl),
                            Header = SH.ModifiersControl,
                            IsChecked = false,
                            Command = static _ =>
                            {
                                Configurations.ModifiersControl.Set(!Configurations.ModifiersControl.Get());
                                ConfigurationManager.Save();

                                GetInstance()._recognizerHolder.ModifiersRecognizer.ActivationModifiers
                                    |= Gestures.Modifiers.GestureModifiers.Control;
                                GetInstance()._recognizerHolder.ModifiersRecognizer.SetEnabled(true);
                            },
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersShift),
                            Header = SH.ModifiersShift,
                            IsChecked = false,
                            Command = static _ =>
                            {
                                Configurations.ModifiersShift.Set(!Configurations.ModifiersShift.Get());
                                ConfigurationManager.Save();

                                GetInstance()._recognizerHolder.ModifiersRecognizer.ActivationModifiers
                                    |= Gestures.Modifiers.GestureModifiers.Shift;
                                GetInstance()._recognizerHolder.ModifiersRecognizer.SetEnabled(true);
                            },
                        },
                    ]
                },
                new TraySeparator(),
                new TrayMenuItem
                {
                    Tag = nameof(SH.Exit),
                    Header = SH.Exit,
                    Command = static _ => Application.Current.Shutdown(),
                }
            ],
        };

        _icon.RightClick += (_, _) =>
        {
            FindMenuItemByTag(_icon.Menu, nameof(SH.TwoFingerTap))?.IsChecked =
                _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap;

            FindMenuItemByTag(_icon.Menu, nameof(SH.RightClickZone))?.IsChecked =
                _recognizerHolder.TouchpadRecognizer.IsRightClickZone;

            FindMenuItemByTag(_icon.Menu, nameof(SH.ModifiersOff))?.IsChecked =
                !Configurations.ModifiersAlt.Get()
                    && !Configurations.ModifiersShift.Get()
                    && !Configurations.ModifiersControl.Get();

            FindMenuItemByTag(_icon.Menu, nameof(SH.ModifiersAlt))?.IsChecked =
                Configurations.ModifiersAlt.Get();

            FindMenuItemByTag(_icon.Menu, nameof(SH.ModifiersControl))?.IsChecked =
                Configurations.ModifiersControl.Get();

            FindMenuItemByTag(_icon.Menu, nameof(SH.ModifiersShift))?.IsChecked =
                Configurations.ModifiersShift.Get();
        };
    }

    private static TrayMenuItem? FindMenuItemByTag(TrayMenu? menu, string tag)
    {
        if (menu == null)
            return null;

        foreach (ITrayMenuItemBase item in menu.Items)
        {
            if (item is not TrayMenuItem menuItem)
                continue;

            if (menuItem.Tag is string menuItemTag
                && string.Equals(menuItemTag, tag, StringComparison.Ordinal))
                return menuItem;

            TrayMenuItem? childMatch = FindMenuItemByTag(menuItem.Menu, tag);
            if (childMatch != null)
                return childMatch;
        }

        return null;
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

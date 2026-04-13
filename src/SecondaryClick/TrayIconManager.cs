using SecondaryClick.Gestures;
using SecondaryClick.Gestures.Modifiers;
using SecondaryClick.Gestures.Touchpads;
using SecondaryClick.WinApi;
using System.Diagnostics;
using System.Drawing;
using System.NativeTray;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace SecondaryClick;

/// <summary>
/// Manages the system tray icon and its associated menu for the SecondaryClick application.
/// Handles gesture recognition configuration, user interactions with the tray menu, and application lifecycle.
/// </summary>
internal sealed partial class TrayIconManager : IDisposable
{
    /// <summary>
    /// Singleton instance of the TrayIconManager.
    /// </summary>
    private static TrayIconManager? _instance;

    /// <summary>
    /// The system tray icon host that manages the tray icon UI.
    /// </summary>
    private readonly TrayIconHost _icon;

    /// <summary>
    /// Container for gesture recognizers (touchpad and modifiers).
    /// </summary>
    private readonly RecognizerHolder _recognizerHolder;

    /// <summary>
    /// Watches registry-backed tray icon visibility configuration changes.
    /// </summary>
    private readonly DispatcherTimer _trayIconVisibilityWatcher;

    /// <summary>
    /// Last applied hidden-state of the tray icon.
    /// </summary>
    private bool _isTrayIconHidden;

    /// <summary>
    /// Initializes a new instance of the TrayIconManager class (private constructor for singleton pattern).
    /// Sets up gesture recognizers and initializes the tray icon menu with all available options.
    /// </summary>
    private TrayIconManager()
    {
        _recognizerHolder = new();
        _isTrayIconHidden = Configurations.HideTrayIcon.Get();

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
                            Command = new TrayCommand(_ =>
                            {
                                if (PrecisionTouchpadSPISettings.IsWritable)
                                {
                                    _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap
                                        = !_recognizerHolder.TouchpadRecognizer.IsTwoFingerTap;
                                }
                            }),
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.RightClickZone),
                            Header = SH.RightClickZone,
                            IsChecked = _recognizerHolder.TouchpadRecognizer.IsRightClickZone,
                            IsEnabled = PrecisionTouchpadSPISettings.IsWritable,
                            IsVisible = PrecisionTouchpadRegistrySettings.IsReadable,
                            Command = new TrayCommand(_ =>
                            {
                                if (PrecisionTouchpadSPISettings.IsWritable)
                                {
                                    _recognizerHolder.TouchpadRecognizer.IsRightClickZone
                                        = !_recognizerHolder.TouchpadRecognizer.IsRightClickZone;
                                }
                            }),
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
                            Command = new TrayCommand(_ =>
                            {
                                Configurations.ModifiersAlt.Set(false);
                                Configurations.ModifiersControl.Set(false);
                                Configurations.ModifiersShift.Set(false);
                                ApplyModifierConfiguration();
                            }),
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersAlt),
                            Header = SH.ModifiersAlt,
                            IsChecked = Configurations.ModifiersAlt.Get(),
                            Command = new TrayCommand(_ =>
                            {
                                Configurations.ModifiersAlt.Set(!Configurations.ModifiersAlt.Get());
                                ApplyModifierConfiguration();
                            }),
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersControl),
                            Header = SH.ModifiersControl,
                            IsChecked = Configurations.ModifiersControl.Get(),
                            Command = new TrayCommand(_ =>
                            {
                                Configurations.ModifiersControl.Set(!Configurations.ModifiersControl.Get());
                                ApplyModifierConfiguration();
                            }),
                        },
                        new TrayMenuItem
                        {
                            Tag = nameof(SH.ModifiersShift),
                            Header = SH.ModifiersShift,
                            IsChecked = Configurations.ModifiersShift.Get(),
                            Command = new TrayCommand(_ =>
                            {
                                Configurations.ModifiersShift.Set(!Configurations.ModifiersShift.Get());
                                ApplyModifierConfiguration();
                            }),
                        },
                    ]
                },
                new TraySeparator(),
                new TrayMenuItem
                {
                    Tag = nameof(SH.HideTrayIcon),
                    Header = SH.HideTrayIcon,
                    IsChecked = Configurations.HideTrayIcon.Get(),
                    Command = new TrayCommand(static _ =>
                    {
                        bool nextState = !Configurations.HideTrayIcon.Get();
                        Configurations.HideTrayIcon.Set(nextState);
                        GetInstance().ApplyTrayIconVisibilityFromConfiguration();
                    }),
                },
                new TrayMenuItem
                {
                    Tag = nameof(SH.StartWithWindows),
                    Header = SH.StartWithWindows,
                    IsChecked = StartupRegistrySettings.IsEnabled(),
                    Command = new TrayCommand(static _ =>
                    {
                        bool nextState = !StartupRegistrySettings.IsEnabled();
                        if (StartupRegistrySettings.SetEnabled(nextState))
                        {
                            FindMenuItemByTag(GetInstance()._icon.Menu, nameof(SH.StartWithWindows))!.IsChecked
                                = StartupRegistrySettings.IsEnabled();
                        }
                    }),
                },
                new TrayMenuItem
                {
                    Tag = nameof(SH.Exit),
                    Header = SH.Exit,
                    Command = new TrayCommand(static _ => Application.Current.Shutdown()),
                }
            ],
        };

        _icon.RightClick += (_, _) =>
        {
            if (PrecisionTouchpadRegistrySettings.IsReadable)
            {
                FindMenuItemByTag(_icon.Menu, nameof(SH.TwoFingerTap))?.IsChecked =
                    _recognizerHolder.TouchpadRecognizer.IsTwoFingerTap;

                FindMenuItemByTag(_icon.Menu, nameof(SH.RightClickZone))?.IsChecked =
                    _recognizerHolder.TouchpadRecognizer.IsRightClickZone;
            }

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

            FindMenuItemByTag(_icon.Menu, nameof(SH.HideTrayIcon))?.IsChecked =
                Configurations.HideTrayIcon.Get();

            FindMenuItemByTag(_icon.Menu, nameof(SH.StartWithWindows))?.IsChecked =
                StartupRegistrySettings.IsEnabled();
        };

        _icon.IsVisible = !_isTrayIconHidden;

        _trayIconVisibilityWatcher = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1),
        };
        _trayIconVisibilityWatcher.Tick += OnTrayIconVisibilityWatcherTick;
        _trayIconVisibilityWatcher.Start();

        ApplyModifierConfiguration();
    }

    private void ApplyTrayIconVisibilityFromConfiguration()
    {
        _isTrayIconHidden = Configurations.HideTrayIcon.Get();
        _icon.IsVisible = !_isTrayIconHidden;
    }

    private void ApplyModifierConfiguration()
    {
        GestureModifiers activationModifiers = GestureModifiers.None;

        if (Configurations.ModifiersAlt.Get())
        {
            activationModifiers |= GestureModifiers.Alt;
        }

        if (Configurations.ModifiersControl.Get())
        {
            activationModifiers |= GestureModifiers.Control;
        }

        if (Configurations.ModifiersShift.Get())
        {
            activationModifiers |= GestureModifiers.Shift;
        }

        _recognizerHolder.ModifiersRecognizer.ActivationModifiers = activationModifiers;
        _recognizerHolder.ModifiersRecognizer.SetEnabled(enabled: activationModifiers != GestureModifiers.None);
    }

    private void OnTrayIconVisibilityWatcherTick(object? sender, EventArgs e)
    {
        bool isHiddenInRegistry = Configurations.HideTrayIcon.Get();
        if (isHiddenInRegistry == _isTrayIconHidden)
            return;

        _isTrayIconHidden = isHiddenInRegistry;
        _icon.IsVisible = !_isTrayIconHidden;
    }

    private static TrayMenuItem? FindMenuItemByTag(TrayMenu? menu, string tag)
    {
        // Recursively search for a menu item with the specified tag
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

    /// <summary>
    /// Disposes the tray icon manager and releases all resources.
    /// </summary>
    public void Dispose()
    {
        _trayIconVisibilityWatcher.Stop();
        _trayIconVisibilityWatcher.Tick -= OnTrayIconVisibilityWatcherTick;
        _recognizerHolder.Dispose();
        _icon.IsVisible = false;
    }

    /// <summary>
    /// Displays a balloon tip notification in the system tray.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="content">The content/message of the notification.</param>
    /// <param name="isError">If true, shows an error icon; otherwise shows an informational icon.</param>
    /// <param name="timeout">The time in milliseconds before the notification automatically closes.</param>
    /// <param name="clickEvent">Optional callback when the notification is clicked.</param>
    /// <param name="closeEvent">Optional callback when the notification is closed.</param>
    public static void ShowNotification(string title, string content, bool isError = false, int timeout = 5000,
        Action? clickEvent = null,
        Action? closeEvent = null)
    {
        TrayIconHost icon = GetInstance()._icon;
        icon.ShowBalloonTip(timeout, title, content, isError ? TrayToolTipIcon.Error : TrayToolTipIcon.Info);
        icon.BalloonTipClicked += OnIconOnBalloonTipClicked;
        icon.BalloonTipClosed += OnIconOnBalloonTipClosed;

        // Handle notification click event
        void OnIconOnBalloonTipClicked(object sender, EventArgs e)
        {
            clickEvent?.Invoke();
            icon.BalloonTipClicked -= OnIconOnBalloonTipClicked;
        }

        // Handle notification close event
        void OnIconOnBalloonTipClosed(object sender, EventArgs e)
        {
            closeEvent?.Invoke();
            icon.BalloonTipClosed -= OnIconOnBalloonTipClosed;
        }
    }

    /// <summary>
    /// Gets the singleton instance of the TrayIconManager.
    /// Creates a new instance if it doesn't exist yet.
    /// </summary>
    /// <returns>The singleton TrayIconManager instance.</returns>
    public static TrayIconManager GetInstance()
    {
        return _instance ??= new TrayIconManager();
    }

    /// <summary>
    /// Starts the tray icon manager and displays the tray icon in the system tray.
    /// </summary>
    public static void Start()
    {
        using TrayIconWindow window = new();
        window.Show();
        _ = GetInstance();
    }

    /// <summary>
    /// Determines if the application is running as a UWP (Universal Windows Platform) app.
    /// </summary>
    /// <returns>True if running as UWP, false otherwise.</returns>
    private static bool IsRunningAsUWP()
    {
        if (Environment.OSVersion.Version < new Version(6, 2)) // Windows 8
            return false;

        try
        {
            uint len = 0;
            uint r = Kernel32.GetCurrentPackageFullName(ref len, null);

            // ERROR_INSUFFICIENT_BUFFER indicates the app is packaged (UWP)
            return r == Win32Error.ERROR_INSUFFICIENT_BUFFER;
        }
        catch (EntryPointNotFoundException)
        {
            return false;
        }
    }
}

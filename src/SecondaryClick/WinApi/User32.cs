using System.Runtime.InteropServices;

namespace SecondaryClick.WinApi;

internal static class User32
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref SPI.TOUCHPAD_PARAMETERS_V1 pvParam, uint fWinIni);

    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-touchpad_parameters_v1
    /// </summary>
    internal static class SPI
    {
        public const uint TOUCHPAD_PARAMETERS_VERSION_1 = 1;

        public const uint SPI_GETTOUCHPADPARAMETERS = 0x00AE;
        public const uint SPI_SETTOUCHPADPARAMETERS = 0x00AF;

        public const uint SPIF_UPDATEINIFILE = 0x0001;
        public const uint SPIF_SENDWININICHANGE = 0x0002;

        public enum TOUCHPAD_SENSITIVITY_LEVEL : uint
        {
            Lowest = 0,
            Low,
            Medium,
            High,
            Highest,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOUCHPAD_PARAMETERS_V1
        {
            public uint versionNumber;
            public uint maxSupportedContacts;
            public uint legacyTouchpadFeatures;
            private uint flags1;
            private uint flags2;

            public TOUCHPAD_SENSITIVITY_LEVEL sensitivityLevel;
            public uint cursorSpeed;
            public uint feedbackIntensity;
            public uint clickForceSensitivity;
            public uint rightClickZoneWidth;
            public uint rightClickZoneHeight;

            public bool TouchpadPresent
            {
                readonly get => GetFlag(flags1, 0);
                set => SetFlag(ref flags1, 0, value);
            }

            public bool LegacyTouchpadPresent
            {
                readonly get => GetFlag(flags1, 1);
                set => SetFlag(ref flags1, 1, value);
            }

            public bool ExternalMousePresent
            {
                readonly get => GetFlag(flags1, 2);
                set => SetFlag(ref flags1, 2, value);
            }

            public bool TouchpadEnabled
            {
                readonly get => GetFlag(flags1, 3);
                set => SetFlag(ref flags1, 3, value);
            }

            public bool TouchpadActive
            {
                readonly get => GetFlag(flags1, 4);
                set => SetFlag(ref flags1, 4, value);
            }

            public bool FeedbackSupported
            {
                readonly get => GetFlag(flags1, 5);
                set => SetFlag(ref flags1, 5, value);
            }

            public bool ClickForceSupported
            {
                readonly get => GetFlag(flags1, 6);
                set => SetFlag(ref flags1, 6, value);
            }

            public bool AllowActiveWhenMousePresent
            {
                readonly get => GetFlag(flags1, 32);
                set => SetFlag(ref flags2, 0, value);
            }

            public bool FeedbackEnabled
            {
                readonly get => GetFlag(flags2, 1);
                set => SetFlag(ref flags2, 1, value);
            }

            public bool TapEnabled
            {
                readonly get => GetFlag(flags2, 2);
                set => SetFlag(ref flags2, 2, value);
            }

            public bool TapAndDragEnabled
            {
                readonly get => GetFlag(flags2, 3);
                set => SetFlag(ref flags2, 3, value);
            }

            public bool TwoFingerTapEnabled
            {
                readonly get => GetFlag(flags2, 4);
                set => SetFlag(ref flags2, 4, value);
            }

            public bool RightClickZoneEnabled
            {
                readonly get => GetFlag(flags2, 5);
                set => SetFlag(ref flags2, 5, value);
            }

            public bool MouseAccelSettingHonored
            {
                readonly get => GetFlag(flags2, 6);
                set => SetFlag(ref flags2, 6, value);
            }

            public bool PanEnabled
            {
                readonly get => GetFlag(flags2, 7);
                set => SetFlag(ref flags2, 7, value);
            }

            public bool ZoomEnabled
            {
                readonly get => GetFlag(flags2, 8);
                set => SetFlag(ref flags2, 8, value);
            }

            public bool ScrollDirectionReversed
            {
                readonly get => GetFlag(flags2, 9);
                set => SetFlag(ref flags2, 9, value);
            }

            public static bool GetFlag(uint value, int bit)
                => (value & (1u << bit)) != 0;

            public static void SetFlag(ref uint value, int bit, bool enabled)
            {
                if (enabled)
                    value |= (1u << bit);
                else
                    value &= ~(1u << bit);
            }
        }
    }
}

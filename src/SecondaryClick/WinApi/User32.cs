using System.Runtime.InteropServices;

namespace SecondaryClick.WinApi;

/// <summary>
/// Provides P/Invoke declarations for User32.dll functions and structures.
/// Includes touchpad parameter management and keyboard state queries.
/// </summary>
internal static class User32
{
    /// <summary>
    /// Determines whether a key is up or down at the time the function is called.
    /// Useful for querying the async state of keys independent of message queues.
    /// </summary>
    /// <param name="vKey">The virtual-key code of the key being tested.</param>
    /// <returns>The key state. If the high-order bit is 1, the key is down; if 0, the key is up.</returns>
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    /// <summary>
    /// Retrieves or sets the values of system metrics and system configuration settings.
    /// Used here for touchpad parameter management.
    /// </summary>
    /// <param name="uiAction">The system metric or configuration setting to query or set.</param>
    /// <param name="uiParam">The parameter whose meaning depends on the uiAction parameter.</param>
    /// <param name="pvParam">A pointer to a value whose meaning depends on uiAction. Pass a TOUCHPAD_PARAMETERS_V1 structure.</param>
    /// <param name="fWinIni">How the function should handle the system setting. Can include update and notification flags.</param>
    /// <returns>True if successful, false otherwise.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref SPI.TOUCHPAD_PARAMETERS_V1 pvParam, uint fWinIni);

    /// <summary>
    /// Contains constants and structures related to SystemParametersInfo touchpad operations.
    /// </summary>
    internal static class SPI
    {
        /// <summary>
        /// Touchpad parameters version 1. Used in TOUCHPAD_PARAMETERS_V1 structure.
        /// </summary>
        public const uint TOUCHPAD_PARAMETERS_VERSION_1 = 1;

        /// <summary>
        /// SystemParametersInfo action code to retrieve touchpad parameters.
        /// </summary>
        public const uint SPI_GETTOUCHPADPARAMETERS = 0x00AE;
        
        /// <summary>
        /// SystemParametersInfo action code to set touchpad parameters.
        /// </summary>
        public const uint SPI_SETTOUCHPADPARAMETERS = 0x00AF;

        /// <summary>
        /// Update the user profile with the new settings.
        /// </summary>
        public const uint SPIF_UPDATEINIFILE = 0x0001;
        
        /// <summary>
        /// Broadcast a WM_SETTINGCHANGE message after updating the settings.
        /// </summary>
        public const uint SPIF_SENDWININICHANGE = 0x0002;

        /// <summary>
        /// Enumerates the sensitivity levels for touchpad cursor speed.
        /// </summary>
        public enum TOUCHPAD_SENSITIVITY_LEVEL : uint
        {
            /// <summary>Lowest sensitivity setting (slowest).</summary>
            Lowest = 0,
            
            /// <summary>Low sensitivity setting.</summary>
            Low,
            
            /// <summary>Medium sensitivity setting (default).</summary>
            Medium,
            
            /// <summary>High sensitivity setting.</summary>
            High,
            
            /// <summary>Highest sensitivity setting (fastest).</summary>
            Highest,
        }

        /// <summary>
        /// Provides detailed configuration parameters for precision touchpads.
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-touchpad_parameters_v1
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TOUCHPAD_PARAMETERS_V1
        {
            /// <summary>Version number of this structure (should be TOUCHPAD_PARAMETERS_VERSION_1).</summary>
            public uint versionNumber;
            
            /// <summary>Maximum number of simultaneous touch contacts the hardware supports.</summary>
            public uint maxSupportedContacts;
            
            /// <summary>Legacy touchpad feature flags.</summary>
            public uint legacyTouchpadFeatures;
            
            /// <summary>Internal flags field 1 (used for bit-packed boolean properties).</summary>
            private uint flags1;
            
            /// <summary>Internal flags field 2 (used for bit-packed boolean properties).</summary>
            private uint flags2;

            /// <summary>Touchpad cursor sensitivity level.</summary>
            public TOUCHPAD_SENSITIVITY_LEVEL sensitivityLevel;
            
            /// <summary>Cursor speed setting.</summary>
            public uint cursorSpeed;
            
            /// <summary>Haptic feedback intensity.</summary>
            public uint feedbackIntensity;
            
            /// <summary>Force sensitivity for physical clicks.</summary>
            public uint clickForceSensitivity;
            
            /// <summary>Width of the right-click zone in pixels.</summary>
            public uint rightClickZoneWidth;
            
            /// <summary>Height of the right-click zone in pixels.</summary>
            public uint rightClickZoneHeight;

            /// <summary>Gets or sets whether a precision touchpad is present.</summary>
            public bool TouchpadPresent
            {
                readonly get => GetFlag(flags1, 0);
                set => SetFlag(ref flags1, 0, value);
            }

            /// <summary>Gets or sets whether a legacy touchpad is present.</summary>
            public bool LegacyTouchpadPresent
            {
                readonly get => GetFlag(flags1, 1);
                set => SetFlag(ref flags1, 1, value);
            }

            /// <summary>Gets or sets whether an external mouse is present.</summary>
            public bool ExternalMousePresent
            {
                readonly get => GetFlag(flags1, 2);
                set => SetFlag(ref flags1, 2, value);
            }

            /// <summary>Gets or sets whether the touchpad is enabled.</summary>
            public bool TouchpadEnabled
            {
                readonly get => GetFlag(flags1, 3);
                set => SetFlag(ref flags1, 3, value);
            }

            /// <summary>Gets or sets whether the touchpad is actively reporting input.</summary>
            public bool TouchpadActive
            {
                readonly get => GetFlag(flags1, 4);
                set => SetFlag(ref flags1, 4, value);
            }

            /// <summary>Gets or sets whether haptic feedback is supported.</summary>
            public bool FeedbackSupported
            {
                readonly get => GetFlag(flags1, 5);
                set => SetFlag(ref flags1, 5, value);
            }

            /// <summary>Gets or sets whether click force sensing is supported.</summary>
            public bool ClickForceSupported
            {
                readonly get => GetFlag(flags1, 6);
                set => SetFlag(ref flags1, 6, value);
            }

            /// <summary>Gets or sets whether the touchpad remains active when an external mouse is connected.</summary>
            public bool AllowActiveWhenMousePresent
            {
                readonly get => GetFlag(flags1, 32);
                set => SetFlag(ref flags2, 0, value);
            }

            /// <summary>Gets or sets whether haptic feedback is enabled.</summary>
            public bool FeedbackEnabled
            {
                readonly get => GetFlag(flags2, 1);
                set => SetFlag(ref flags2, 1, value);
            }

            /// <summary>Gets or sets whether tap-to-click is enabled.</summary>
            public bool TapEnabled
            {
                readonly get => GetFlag(flags2, 2);
                set => SetFlag(ref flags2, 2, value);
            }

            /// <summary>Gets or sets whether tap-and-drag is enabled.</summary>
            public bool TapAndDragEnabled
            {
                readonly get => GetFlag(flags2, 3);
                set => SetFlag(ref flags2, 3, value);
            }

            /// <summary>Gets or sets whether two-finger tap is treated as a right-click.</summary>
            public bool TwoFingerTapEnabled
            {
                readonly get => GetFlag(flags2, 4);
                set => SetFlag(ref flags2, 4, value);
            }

            /// <summary>Gets or sets whether the bottom-right corner click zone is enabled as a right-click.</summary>
            public bool RightClickZoneEnabled
            {
                readonly get => GetFlag(flags2, 5);
                set => SetFlag(ref flags2, 5, value);
            }

            /// <summary>Gets or sets whether the system mouse acceleration setting is honored.</summary>
            public bool MouseAccelSettingHonored
            {
                readonly get => GetFlag(flags2, 6);
                set => SetFlag(ref flags2, 6, value);
            }

            /// <summary>Gets or sets whether pan (scroll by dragging) is enabled.</summary>
            public bool PanEnabled
            {
                readonly get => GetFlag(flags2, 7);
                set => SetFlag(ref flags2, 7, value);
            }

            /// <summary>Gets or sets whether zoom pinch gesture is enabled.</summary>
            public bool ZoomEnabled
            {
                readonly get => GetFlag(flags2, 8);
                set => SetFlag(ref flags2, 8, value);
            }

            /// <summary>Gets or sets whether scroll direction is reversed (natural scrolling).</summary>
            public bool ScrollDirectionReversed
            {
                readonly get => GetFlag(flags2, 9);
                set => SetFlag(ref flags2, 9, value);
            }

            /// <summary>
            /// Gets a single bit flag from a 32-bit value.
            /// </summary>
            /// <param name="value">The 32-bit value to extract the flag from.</param>
            /// <param name="bit">The bit position (0-31) of the flag.</param>
            /// <returns>True if the bit is set, false otherwise.</returns>
            public static bool GetFlag(uint value, int bit)
                => (value & (1u << bit)) != 0;

            /// <summary>
            /// Sets or clears a single bit flag in a 32-bit value.
            /// </summary>
            /// <param name="value">The 32-bit value to modify.</param>
            /// <param name="bit">The bit position (0-31) of the flag to set or clear.</param>
            /// <param name="enabled">True to set the bit, false to clear it.</param>
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

using SecondaryClick.WinApi;
using System.Runtime.InteropServices;
using static SecondaryClick.WinApi.User32.SPI;

namespace SecondaryClick.Gestures.Touchpads;

/// <summary>
/// Provides SPI (System Parameters Info) based access to precision touchpad settings on Windows 11.
/// This is the newer method for configuring touchpad parameters introduced in Windows 11 (Build 26027+).
/// </summary>
internal class PrecisionTouchpadSPISettings
{
    /// <summary>
    /// Gets a value indicating whether the current OS supports writing precision touchpad settings via SPI.
    /// Requires Windows 11 (Build 26027 or later).
    /// </summary>
    public static bool IsWritable
    {
        get
        {
            Version version = Environment.OSVersion.Version;

            // Check if the current OS is Windows 11 (build 26027 or later).
            return version.Major == 10 && version.Minor == 0 && version.Build >= 26027;
        }
    }

    /// <summary>
    /// Sets whether two-finger tapping on the touchpad triggers a right-click via SPI.
    /// </summary>
    /// <param name="enabled">True to enable two-finger tap as right-click, false to disable.</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public static bool SetTwoFingerTapRightClickEnabled(bool enabled)
    {
        if (!TryGetParameters(out TOUCHPAD_PARAMETERS_V1 p))
            return false;

        p.TwoFingerTapEnabled = enabled;
        return SetParameters(ref p);
    }

    /// <summary>
    /// Sets whether clicking in the bottom-right corner of the touchpad triggers a right-click via SPI.
    /// </summary>
    /// <param name="enabled">True to enable right-click zone, false to disable.</param>
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    public static bool SetRightClickZoneEnabled(bool enabled)
    {
        if (!TryGetParameters(out TOUCHPAD_PARAMETERS_V1 p))
            return false;

        p.RightClickZoneEnabled = enabled;
        return SetParameters(ref p);
    }

    /// <summary>
    /// Retrieves the current touchpad parameters via SPI.
    /// </summary>
    /// <param name="p">The touchpad parameters structure to populate.</param>
    /// <returns>True if the parameters were retrieved successfully, false otherwise.</returns>
    private static bool TryGetParameters(out TOUCHPAD_PARAMETERS_V1 p)
    {
        p = default;
        p.versionNumber = TOUCHPAD_PARAMETERS_VERSION_1;

        return User32.SystemParametersInfo(
            SPI_GETTOUCHPADPARAMETERS,
            (uint)Marshal.SizeOf<TOUCHPAD_PARAMETERS_V1>(),
            ref p,
            0);
    }

    /// <summary>
    /// Applies the updated touchpad parameters via SPI.
    /// </summary>
    /// <param name="p">The touchpad parameters structure to apply.</param>
    /// <returns>True if the parameters were set successfully, false otherwise.</returns>
    private static bool SetParameters(ref TOUCHPAD_PARAMETERS_V1 p)
    {
        return User32.SystemParametersInfo(
            SPI_SETTOUCHPADPARAMETERS,
            (uint)Marshal.SizeOf<TOUCHPAD_PARAMETERS_V1>(),
            ref p,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }
}

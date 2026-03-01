using SecondaryClick.WinApi;
using System.Runtime.InteropServices;
using static SecondaryClick.WinApi.User32.SPI;

namespace SecondaryClick.Gestures.Touchpads;

internal class PrecisionTouchpadSPISettings
{
    public static bool IsWritable
    {
        get
        {
            Version version = Environment.OSVersion.Version;

            // Check if the current OS is Windows 11 (build 26027 or later).
            return version.Major == 10 && version.Minor == 0 && version.Build >= 26027;
        }
    }

    public static bool SetTwoFingerTapRightClickEnabled(bool enabled)
    {
        if (!TryGetParameters(out var p))
            return false;

        p.TwoFingerTapEnabled = enabled;
        return SetParameters(ref p);
    }

    public static bool SetRightClickZoneEnabled(bool enabled)
    {
        if (!TryGetParameters(out var p))
            return false;

        p.RightClickZoneEnabled = enabled;
        return SetParameters(ref p);
    }

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

    private static bool SetParameters(ref TOUCHPAD_PARAMETERS_V1 p)
    {
        return User32.SystemParametersInfo(
            SPI_SETTOUCHPADPARAMETERS,
            (uint)Marshal.SizeOf<TOUCHPAD_PARAMETERS_V1>(),
            ref p,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }
}

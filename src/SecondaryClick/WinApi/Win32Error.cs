namespace SecondaryClick.WinApi;

/// <summary>
/// Contains Win32 error code constants used by the SecondaryClick application.
/// </summary>
public partial struct Win32Error
{
    /// <summary>
    /// Error code indicating that the data area passed to a system call is too small.
    /// When returned from GetCurrentPackageFullName, it indicates the app is running as a UWP package.
    /// </summary>
    public const uint ERROR_INSUFFICIENT_BUFFER = 0x0000007A;
}

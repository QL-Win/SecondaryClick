using System.Runtime.InteropServices;
using System.Text;

namespace SecondaryClick.WinApi;

/// <summary>
/// Provides P/Invoke declarations for Kernel32.dll functions.
/// </summary>
internal static partial class Kernel32
{
    /// <summary>
    /// Retrieves the full package name and version of the calling process.
    /// Used to detect if the application is running as a UWP (Universal Windows Platform) app.
    /// </summary>
    /// <param name="packageFullNameLength">The size of the packageFullName buffer in characters. Returns the required size if the buffer is too small.</param>
    /// <param name="packageFullName">A buffer that receives the package full name string. Pass null to get the required length.</param>
    /// <returns>An error code: ERROR_INSUFFICIENT_BUFFER indicates the app is packaged (UWP).</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public extern static uint GetCurrentPackageFullName(ref uint packageFullNameLength, StringBuilder? packageFullName);
}

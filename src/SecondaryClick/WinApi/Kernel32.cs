using System.Runtime.InteropServices;
using System.Text;

namespace SecondaryClick.WinApi;

internal static partial class Kernel32
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public extern static uint GetCurrentPackageFullName(ref uint packageFullNameLength, StringBuilder? packageFullName);
}

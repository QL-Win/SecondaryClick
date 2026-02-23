using System.Runtime.InteropServices;

namespace SecondaryClick.WinApi;

internal static class User32
{

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);
}

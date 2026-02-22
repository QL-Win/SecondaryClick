using Microsoft.Win32.SafeHandles;

namespace System.MouseKeyHook.WinApi;

internal class HookProcedureHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    //private static bool _closing;

    static HookProcedureHandle()
    {
        //Application.ApplicationExit += (sender, e) => { HookProcedureHandle._closing = true; };
    }

    public HookProcedureHandle()
        : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
        //NOTE Calling Unhook during processexit causes deley
        var ret = HookNativeMethods.UnhookWindowsHookEx(handle);
        if (ret != 0)
        {
            base.Dispose();
            return true;
        }
        else
            return true;
    }
}

using System;

namespace System.MouseKeyHook.WinApi;

internal struct CallbackData
{
    public CallbackData(IntPtr wParam, IntPtr lParam, int mSwapButton = 0)
    {
        WParam = wParam;
        LParam = lParam;
        MSwapButton = mSwapButton;
    }

    public IntPtr WParam { get; }

    public IntPtr LParam { get; }

    public int MSwapButton { get; set; }
}

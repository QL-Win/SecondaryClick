using System;
using System.MouseKeyHook.WinApi;

namespace System.MouseKeyHook.Implementation;

internal abstract class BaseListener : IDisposable
{
    protected BaseListener(Subscribe subscribe)
    {
        Handle = subscribe(Callback);
    }

    protected HookResult Handle { get; set; }

    public void Dispose()
    {
        Handle.Dispose();
    }

    protected abstract bool Callback(CallbackData data);
}

using System.WindowsInput.WinApi;

namespace System.WindowsInput;

internal interface IInputMessageDispatcher
{
    public void DispatchInput(User32.INPUT[] inputs);
}

using System.WindowsInput.WinApi;

namespace System.WindowsInput;

/// <summary>
/// The contract for a service that dispatches input messages for the Windows Platform.
/// </summary>
internal interface IInputMessageDispatcher
{
    /// <summary>
    /// Dispatches the specified input messages.
    /// </summary>
    /// <param name="inputs">The input messages to dispatch.</param>
    public void DispatchInput(User32.INPUT[] inputs);
}

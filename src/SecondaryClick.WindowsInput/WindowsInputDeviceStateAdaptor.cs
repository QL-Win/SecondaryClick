using System.WindowsInput.WinApi;

namespace System.WindowsInput;

/// <summary>
/// Implementation of <see cref="IInputDeviceStateAdaptor"/> for determining the state of input devices for Windows platform.
/// </summary>
public class WindowsInputDeviceStateAdaptor : IInputDeviceStateAdaptor
{
    /// <inheritdoc/>
    public bool IsKeyDown(User32.VK keyCode)
    {
        short keyState = User32.GetKeyState((ushort)keyCode);
        return keyState < 0;
    }

    /// <inheritdoc/>
    public bool IsKeyUp(User32.VK keyCode)
    {
        return !IsKeyDown(keyCode);
    }

    /// <inheritdoc/>
    public bool IsHardwareKeyDown(User32.VK keyCode)
    {
        short asyncKeyState = User32.GetAsyncKeyState((ushort)keyCode);
        return asyncKeyState < 0;
    }

    /// <inheritdoc/>
    public bool IsHardwareKeyUp(User32.VK keyCode)
    {
        return !IsHardwareKeyDown(keyCode);
    }

    /// <inheritdoc/>
    public bool IsTogglingKeyInEffect(User32.VK keyCode)
    {
        short keyState = User32.GetKeyState((ushort)keyCode);
        return (keyState & 1) == 1;
    }
}

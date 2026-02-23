using System.WindowsInput.WinApi;

namespace System.WindowsInput;

/// <summary>
/// The contract for a service that determines the state of input devices for the Windows Platform.
/// </summary>
public interface IInputDeviceStateAdaptor
{
    /// <summary>
    /// Determines whether the specified key is down.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns>True if the key is down.</returns>
    public bool IsKeyDown(User32.VK keyCode);

    /// <summary>
    /// Determines whether the specified key is up.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns>True if the key is up.</returns>
    public bool IsKeyUp(User32.VK keyCode);

    /// <summary>
    /// Determines whether the specified hardware key is down.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns>True if the hardware key is down.</returns>
    public bool IsHardwareKeyDown(User32.VK keyCode);

    /// <summary>
    /// Determines whether the specified hardware key is up.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns>True if the hardware key is up.</returns>
    public bool IsHardwareKeyUp(User32.VK keyCode);

    /// <summary>
    /// Determines whether the specified toggling key is in effect.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns>True if the toggling key is in effect.</returns>
    public bool IsTogglingKeyInEffect(User32.VK keyCode);
}

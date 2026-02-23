namespace System.MouseKeyHook.WinApi;

/// <summary>
/// Represents the method that will handle the event.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">A <see cref="KeyPressEventArgs"/> that contains the event data.</param>
public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);

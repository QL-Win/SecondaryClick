namespace System.MouseKeyHook.WinApi;

/// <summary>
/// Represents the method that will handle the MouseDown, MouseUp, or MouseMove event of a form, control, or other component.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
public delegate void MouseEventHandler(object sender, MouseEventArgs e);

namespace System.WindowsInput;

/// <summary>
/// The service contract for a mouse simulator for the Windows platform.
/// </summary>
public interface IMouseSimulator
{
    /// <summary>
    /// Gets the <see cref="IKeyboardSimulator"/> instance for simulating Keyboard input.
    /// </summary>
    public IKeyboardSimulator Keyboard { get; }

    /// <summary>
    /// Simulates mouse movement by the specified distance measured as a delta from the current mouse location in pixels.
    /// </summary>
    /// <param name="pixelDeltaX">The distance in pixels to move the mouse horizontally.</param>
    /// <param name="pixelDeltaY">The distance in pixels to move the mouse vertically.</param>
    public IMouseSimulator MoveMouseBy(int pixelDeltaX, int pixelDeltaY);

    /// <summary>
    /// Simulates mouse movement to the specified location on the primary display device.
    /// </summary>
    /// <param name="absoluteX">The destination's absolute X-coordinate on the primary display device where 0 is the extreme left hand side of the display device and 65535 is the extreme right hand side of the display device.</param>
    /// <param name="absoluteY">The destination's absolute Y-coordinate on the primary display device where 0 is the top of the display device and 65535 is the bottom of the display device.</param>
    public IMouseSimulator MoveMouseTo(double absoluteX, double absoluteY);

    /// <summary>
    /// Simulates mouse movement to the specified location on the Virtual Desktop which includes all active displays.
    /// </summary>
    /// <param name="absoluteX">The destination's absolute X-coordinate on the virtual desktop where 0 is the left hand side of the virtual desktop and 65535 is the extreme right hand side of the virtual desktop.</param>
    /// <param name="absoluteY">The destination's absolute Y-coordinate on the virtual desktop where 0 is the top of the virtual desktop and 65535 is the bottom of the virtual desktop.</param>
    public IMouseSimulator MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY);

    /// <summary>
    /// Simulates a mouse left button down gesture.
    /// </summary>
    public IMouseSimulator LeftButtonDown();

    /// <summary>
    /// Simulates a mouse left button up gesture.
    /// </summary>
    public IMouseSimulator LeftButtonUp();

    /// <summary>
    /// Simulates a mouse left button click gesture.
    /// </summary>
    public IMouseSimulator LeftButtonClick();

    /// <summary>
    /// Simulates a mouse left button double click gesture.
    /// </summary>
    public IMouseSimulator LeftButtonDoubleClick();

    /// <summary>
    /// Simulates a mouse middle button down gesture.
    /// </summary>
    public IMouseSimulator MiddleButtonDown();

    /// <summary>
    /// Simulates a mouse middle button up gesture.
    /// </summary>
    public IMouseSimulator MiddleButtonUp();

    /// <summary>
    /// Simulates a mouse middle button click gesture.
    /// </summary>
    public IMouseSimulator MiddleButtonClick();

    /// <summary>
    /// Simulates a mouse middle button double click gesture.
    /// </summary>
    public IMouseSimulator MiddleButtonDoubleClick();

    /// <summary>
    /// Simulates a mouse right button down gesture.
    /// </summary>
    public IMouseSimulator RightButtonDown();

    /// <summary>
    /// Simulates a mouse right button up gesture.
    /// </summary>
    public IMouseSimulator RightButtonUp();

    /// <summary>
    /// Simulates a mouse right button click gesture.
    /// </summary>
    public IMouseSimulator RightButtonClick();

    /// <summary>
    /// Simulates a mouse right button double click gesture.
    /// </summary>
    public IMouseSimulator RightButtonDoubleClick();

    /// <summary>
    /// Simulates a mouse X button down gesture.
    /// </summary>
    /// <param name="buttonId">The X button id.</param>
    public IMouseSimulator XButtonDown(int buttonId);

    /// <summary>
    /// Simulates a mouse X button up gesture.
    /// </summary>
    /// <param name="buttonId">The X button id.</param>
    public IMouseSimulator XButtonUp(int buttonId);

    /// <summary>
    /// Simulates a mouse X button click gesture.
    /// </summary>
    /// <param name="buttonId">The X button id.</param>
    public IMouseSimulator XButtonClick(int buttonId);

    /// <summary>
    /// Simulates a mouse X button double click gesture.
    /// </summary>
    /// <param name="buttonId">The X button id.</param>
    public IMouseSimulator XButtonDoubleClick(int buttonId);

    /// <summary>
    /// Simulates a vertical mouse wheel scroll gesture.
    /// </summary>
    /// <param name="scrollAmountInClicks">The amount to scroll in clicks.</param>
    public IMouseSimulator VerticalScroll(int scrollAmountInClicks);

    /// <summary>
    /// Simulates a horizontal mouse wheel scroll gesture.
    /// </summary>
    /// <param name="scrollAmountInClicks">The amount to scroll in clicks.</param>
    public IMouseSimulator HorizontalScroll(int scrollAmountInClicks);

    /// <summary>
    /// Sleeps for the specified timeout in milliseconds.
    /// </summary>
    /// <param name="millsecondsTimeout">The timeout in milliseconds.</param>
    public IMouseSimulator Sleep(int millsecondsTimeout);

    /// <summary>
    /// Sleeps for the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    public IMouseSimulator Sleep(TimeSpan timeout);
}

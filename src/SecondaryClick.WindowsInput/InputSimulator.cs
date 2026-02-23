namespace System.WindowsInput;

/// <summary>
/// Implementation of <see cref="IInputSimulator"/> for simulating Keyboard and Mouse input and Hardware Input Device state detection for the Windows Platform.
/// </summary>
public class InputSimulator : IInputSimulator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputSimulator"/> class with custom simulators.
    /// </summary>
    public InputSimulator(IKeyboardSimulator keyboardSimulator, IMouseSimulator mouseSimulator, IInputDeviceStateAdaptor inputDeviceStateAdaptor)
    {
        _keyboardSimulator = keyboardSimulator;
        _mouseSimulator = mouseSimulator;
        _inputDeviceState = inputDeviceStateAdaptor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputSimulator"/> class with default simulators.
    /// </summary>
    public InputSimulator()
    {
        _keyboardSimulator = new KeyboardSimulator(this);
        _mouseSimulator = new MouseSimulator(this);
        _inputDeviceState = new WindowsInputDeviceStateAdaptor();
    }

    /// <inheritdoc/>
    public IKeyboardSimulator Keyboard => _keyboardSimulator;

    /// <inheritdoc/>
    public IMouseSimulator Mouse => _mouseSimulator;

    /// <inheritdoc/>
    public IInputDeviceStateAdaptor InputDeviceState => _inputDeviceState;

    private readonly IKeyboardSimulator _keyboardSimulator;

    private readonly IMouseSimulator _mouseSimulator;

    private readonly IInputDeviceStateAdaptor _inputDeviceState;
}

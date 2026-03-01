using SecondaryClick.Gestures.Modifiers;
using SecondaryClick.Gestures.Touchpads;

namespace SecondaryClick.Gestures;

public sealed class RecognizerHolder : IDisposable
{
    public ModifiersGestureRecognizer ModifiersRecognizer { get; private set; } = new();
    public TouchpadGestureRecognizer TouchpadRecognizer { get; private set; } = new();

    public void Dispose()
    {
        ModifiersRecognizer.Dispose();
        TouchpadRecognizer.Dispose();
    }
}

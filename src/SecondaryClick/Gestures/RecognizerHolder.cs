using SecondaryClick.Gestures.Modifiers;

namespace SecondaryClick.Gestures;

public sealed class RecognizerHolder : IDisposable
{
    public ModifiersGestureRecognizer ModifiersRecognizer { get; private set; } = new();

    public RecognizerHolder()
    {
        ModifiersRecognizer.SetEnabled(true);
    }

    public void Dispose()
    {
        ModifiersRecognizer.Dispose();
    }
}

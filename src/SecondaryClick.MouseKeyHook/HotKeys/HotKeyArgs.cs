namespace System.MouseKeyHook.HotKeys;

/// <summary>
/// The event arguments passed when a HotKeySet's OnHotKeysDownHold event is triggered.
/// </summary>
public sealed class HotKeyArgs : EventArgs
{
    /// <summary>
    /// Creates an instance of the HotKeyArgs.
    /// <param name="triggeredAt">Time when the event was triggered</param>
    /// </summary>
    public HotKeyArgs(DateTime triggeredAt)
    {
        Time = triggeredAt;
    }

    /// <summary>
    /// Time when the event was triggered
    /// </summary>
    public DateTime Time { get; }
}

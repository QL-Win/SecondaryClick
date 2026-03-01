using Fischless.Configuration;
using System.Reflection;

namespace SecondaryClick;

/// <summary>
/// Manages application configuration settings with persistent storage.
/// Stores modifier key configurations (Alt, Shift, Control) used for gesture recognition.
/// </summary>
[Obfuscation]
public static class Configurations
{
    /// <summary>
    /// Configuration for Alt key modifier activation. Defaults to false.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersAlt { get; } = new(nameof(ModifiersAlt), false);

    /// <summary>
    /// Configuration for Shift key modifier activation. Defaults to false.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersShift { get; } = new(nameof(ModifiersShift), false);

    /// <summary>
    /// Configuration for Control key modifier activation. Defaults to true.
    /// </summary>
    public static ConfigurationDefinition<bool> ModifiersControl { get; } = new(nameof(ModifiersControl), true);
}

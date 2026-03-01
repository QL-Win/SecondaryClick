using Fischless.Configuration;
using System.Reflection;

namespace SecondaryClick;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<bool> ModifiersAlt { get; } = new(nameof(ModifiersAlt), false);

    public static ConfigurationDefinition<bool> ModifiersShift { get; } = new(nameof(ModifiersShift), false);

    public static ConfigurationDefinition<bool> ModifiersControl { get; } = new(nameof(ModifiersControl), true);
}

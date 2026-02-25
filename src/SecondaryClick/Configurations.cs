using Fischless.Configuration;
using System.Reflection;

namespace SecondaryClick;

[Obfuscation]
public static class Configurations
{
    public static ConfigurationDefinition<bool> ModifiersAlt { get; } = new(nameof(ModifiersAlt), true);

    public static ConfigurationDefinition<bool> ModifiersShift { get; } = new(nameof(ModifiersShift), true);

    public static ConfigurationDefinition<bool> ModifiersCtrl { get; } = new(nameof(ModifiersCtrl), true);
}

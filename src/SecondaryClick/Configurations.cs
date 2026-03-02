using Microsoft.Win32;

namespace SecondaryClick;

/// <summary>
/// Manages application configuration settings with persistent storage.
/// Stores modifier key configurations (Alt, Shift, Control) used for gesture recognition.
/// </summary>
internal static class Configurations
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

/// <summary>
/// Registry-backed configuration definition.
/// </summary>
/// <typeparam name="T">Configuration value type.</typeparam>
internal sealed class ConfigurationDefinition<T>(string name, T defaultValue)
{
    private const string RegistryPath = @"Software\SecondaryClick";

    private readonly string _name = name;
    private readonly T _defaultValue = defaultValue;

    public T Get()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, writable: false);
        if (key?.GetValue(_name) is not object value)
        {
            return _defaultValue;
        }

        try
        {
            if (typeof(T) == typeof(bool))
            {
                bool converted = value switch
                {
                    int intValue => intValue != 0,
                    long longValue => longValue != 0,
                    string stringValue when bool.TryParse(stringValue, out bool parsedBool) => parsedBool,
                    string stringValue when int.TryParse(stringValue, out int parsedInt) => parsedInt != 0,
                    _ => Convert.ToInt32(value) != 0,
                };

                return (T)(object)converted;
            }

            if (value is T typedValue)
            {
                return typedValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return _defaultValue;
        }
    }

    public void Set(T value)
    {
        using RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath);

        if (typeof(T) == typeof(bool))
        {
            bool boolValue = (bool)(object)value!;
            key.SetValue(_name, boolValue ? 1 : 0, RegistryValueKind.DWord);
            return;
        }

        key.SetValue(_name, value?.ToString() ?? string.Empty, RegistryValueKind.String);
    }
}

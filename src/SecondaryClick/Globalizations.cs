using System.Globalization;

namespace SecondaryClick;

/// <summary>
/// Provides localized string resources for the SecondaryClick application.
/// Supports multiple languages including Simplified Chinese, Traditional Chinese, Japanese, and English.
/// </summary>
internal static class Globalizations
{
    /// <summary>
    /// Provides access to localized strings (SH = String Helper).
    /// </summary>
    public static class SH
    {
        /// <summary>
        /// Gets the localized string for "Touchpads" feature.
        /// </summary>
        public static string Touchpads => GetResources(nameof(Touchpads));

        /// <summary>
        /// Gets the localized string for "Two Finger Tap" gesture.
        /// </summary>
        public static string TwoFingerTap => GetResources(nameof(TwoFingerTap));

        /// <summary>
        /// Gets the localized string for "Right Click Zone" gesture.
        /// </summary>
        public static string RightClickZone => GetResources(nameof(RightClickZone));

        /// <summary>
        /// Gets the localized string for "Keyboard Modifiers" feature.
        /// </summary>
        public static string Modifiers => GetResources(nameof(Modifiers));

        /// <summary>
        /// Gets the localized string for "Modifiers Off".
        /// </summary>
        public static string ModifiersOff => GetResources(nameof(ModifiersOff));

        /// <summary>
        /// Gets the localized string for "Alt Key" modifier.
        /// </summary>
        public static string ModifiersAlt => GetResources(nameof(ModifiersAlt));

        /// <summary>
        /// Gets the localized string for "Control Key" modifier.
        /// </summary>
        public static string ModifiersControl => GetResources(nameof(ModifiersControl));

        /// <summary>
        /// Gets the localized string for "Shift Key" modifier.
        /// </summary>
        public static string ModifiersShift => GetResources(nameof(ModifiersShift));

        /// <summary>
        /// Gets the localized string for "Exit" action.
        /// </summary>
        public static string Exit => GetResources(nameof(Exit));

        /// <summary>
        /// Retrieves the localized string for the given resource name based on the current UI culture.
        /// </summary>
        /// <param name=\"name\">The resource name to retrieve.</param>
        /// <returns>The localized string value, or the original name if no translation is found.</returns>
        private static string GetResources(string name)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            // Determine the appropriate localization based on the current UI culture
            if (culture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            {
                if (culture.Name == "zh")
                {
                    return ReturnHans(name);
                }
                else
                {
                    CultureInfo current = culture;
                    while (current.Name != "zh-Hant"
                        && current.Name != "zh-Hans"
                        && current.Parent != null
                        && current.Parent != CultureInfo.InvariantCulture)
                    {
                        current = current.Parent;
                    }

                    if (current.Name == "zh-Hant")
                    {
                        return ReturnHant(name);
                    }
                    else
                    {
                        return ReturnHans(name);
                    }
                }

                // Return Simplified Chinese strings
                static string ReturnHans(string name)
                {
                    return name switch
                    {
                        nameof(Touchpads) => "触控辅助",
                        nameof(TwoFingerTap) => "双指点按或轻点",
                        nameof(RightClickZone) => "点按右下角",
                        nameof(Modifiers) => "键盘辅助",
                        nameof(ModifiersOff) => "关",
                        nameof(ModifiersAlt) => "Alt 键",
                        nameof(ModifiersControl) => "Control 键",
                        nameof(ModifiersShift) => "Shift 键",
                        nameof(Exit) => "退出",
                        _ => name,
                    };
                }

                // Return Traditional Chinese strings
                static string ReturnHant(string name)
                {
                    return name switch
                    {
                        nameof(Touchpads) => "觸控輔助",
                        nameof(TwoFingerTap) => "雙指點按或輕點",
                        nameof(RightClickZone) => "點按右下角",
                        nameof(Modifiers) => "鍵盤輔助",
                        nameof(ModifiersOff) => "關",
                        nameof(ModifiersAlt) => "Alt 鍵",
                        nameof(ModifiersControl) => "Control 鍵",
                        nameof(ModifiersShift) => "Shift 鍵",
                        nameof(Exit) => "退出",
                        _ => name,
                    };
                }
            }
            // Return Japanese strings
            else if (culture.Name.StartsWith("ja", StringComparison.OrdinalIgnoreCase))
            {
                return name switch
                {
                    nameof(Touchpads) => "タッチパッドのアクセシビリティ",
                    nameof(TwoFingerTap) => "2 本指でクリック",
                    nameof(RightClickZone) => "右下隅をクリック",
                    nameof(Modifiers) => "キーボードのアクセシビリティ",
                    nameof(ModifiersOff) => "オフ",
                    nameof(ModifiersAlt) => "Alt キー",
                    nameof(ModifiersControl) => "Control キー",
                    nameof(ModifiersShift) => "Shift キー",
                    nameof(Exit) => "終了",
                    _ => name,
                };
            }
            else
            {
                return name switch
                {
                    nameof(Touchpads) => "Touchpad Accessibility",
                    nameof(TwoFingerTap) => "Tap with two fingers",
                    nameof(RightClickZone) => "Click in bottom right corner",
                    nameof(Modifiers) => "Keyboard Accessibility",
                    nameof(ModifiersOff) => "Off",
                    nameof(ModifiersAlt) => "Alt Key",
                    nameof(ModifiersControl) => "Control Key",
                    nameof(ModifiersShift) => "Shift Key",
                    nameof(Exit) => "Exit",
                    _ => name,
                };
            }
        }
    }
}

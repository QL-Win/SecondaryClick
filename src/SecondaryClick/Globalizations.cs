using System.Globalization;

namespace SecondaryClick;

internal static class Globalizations
{
    public static class SH
    {
        public static string Touchpads => GetResources(nameof(Touchpads));
        public static string TwoFingerTap => GetResources(nameof(TwoFingerTap));
        public static string RightClickZone => GetResources(nameof(RightClickZone));
        public static string Modifiers => GetResources(nameof(Modifiers));
        public static string ModifiersOff => GetResources(nameof(ModifiersOff));
        public static string ModifiersAlt => GetResources(nameof(ModifiersAlt));
        public static string ModifiersControl => GetResources(nameof(ModifiersControl));
        public static string ModifiersShift => GetResources(nameof(ModifiersShift));
        public static string Exit => GetResources(nameof(Exit));

        private static string GetResources(string name)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            if (culture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            {
                if (culture.Name == "zh")
                {
                    ReturnHans(name);
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
                        ReturnHant(name);
                    }
                    else
                    {
                        ReturnHans(name);
                    }
                }

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

            return name;
        }
    }
}

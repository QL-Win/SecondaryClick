using System.Globalization;

namespace SecondaryClick;

/// <summary>
/// Provides localized string resources for the SecondaryClick application.
/// Supports multiple languages.
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
        /// Gets the localized string for "Hide tray icon" option.
        /// </summary>
        public static string HideTrayIcon => GetResources(nameof(HideTrayIcon));

        /// <summary>
        /// Gets the localized string for "Start with Windows" option.
        /// </summary>
        public static string StartWithWindows => GetResources(nameof(StartWithWindows));

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
            string cultureName = culture.Name.ToLowerInvariant();

            // Chinese (Simplified and Traditional)
            if (cultureName.StartsWith("zh"))
            {
                if (cultureName == "zh")
                {
                    return ReturnSimplifiedChinese(name);
                }

                CultureInfo current = culture;
                while (current.Name != "zh-Hant"
                    && current.Name != "zh-Hans"
                    && current.Parent != null
                    && current.Parent != CultureInfo.InvariantCulture)
                {
                    current = current.Parent;
                }

                return current.Name == "zh-Hant"
                    ? ReturnTraditionalChinese(name)
                        : ReturnSimplifiedChinese(name);
            }

            // Language-specific returns
            return cultureName switch
            {
                // Arabic
                string c when c.StartsWith("ar") => ReturnArabic(name),
                // Hungarian
                string c when c.StartsWith("hu-HU") => ReturnHungarian(name),
                // Slovak
                string c when c.StartsWith("sk") => ReturnSlovak(name),
                // Indonesian
                string c when c.StartsWith("id-ID") => ReturnIndonesian(name),
                // Korean
                string c when c.StartsWith("ko") => ReturnKorean(name),
                // Catalan
                string c when c.StartsWith("ca") => ReturnCatalan(name),
                // German
                string c when c.StartsWith("de") => ReturnGerman(name),
                // Spanish
                string c when c.StartsWith("es") => ReturnSpanish(name),
                // French
                string c when c.StartsWith("fr") => ReturnFrench(name),
                // Japanese
                string c when c.StartsWith("ja") => ReturnJapanese(name),
                // Italian
                string c when c.StartsWith("it") => ReturnItalian(name),
                // Norwegian
                string c when c.StartsWith("nb-NO") => ReturnNorwegian(name),
                // Dutch
                string c when c.StartsWith("nl-NL") => ReturnDutch(name),
                // Polish
                string c when c.StartsWith("pl") => ReturnPolish(name),
                // Portuguese Brazil
                string c when c.StartsWith("pt-BR") => ReturnPortugueseBrazil(name),
                // Portuguese Portugal
                string c when c.StartsWith("pt-PT") => ReturnPortuguesePortugal(name),
                // Russian
                string c when c.StartsWith("ru-RU") => ReturnRussian(name),
                // Turkish
                string c when c.StartsWith("tr-TR") => ReturnTurkish(name),
                // Ukrainian
                string c when c.StartsWith("uk-UA") => ReturnUkrainian(name),
                // Vietnamese
                string c when c.StartsWith("vi") => ReturnVietnamese(name),
                // Marathi
                string c when c.StartsWith("mr") => ReturnMarathi(name),
                // Hindi
                string c when c.StartsWith("hi") => ReturnHindi(name),
                // Hebrew
                string c when c.StartsWith("he") => ReturnHebrew(name),
                // Greek
                string c when c.StartsWith("el") => ReturnGreek(name),
                // Swedish
                string c when c.StartsWith("sv") => ReturnSwedish(name),
                // Romanian
                string c when c.StartsWith("ro") => ReturnRomanian(name),
                // Default to English
                _ => ReturnEnglish(name),
            };
        }

        private static string ReturnSimplifiedChinese(string name) => name switch
        {
            nameof(Touchpads) => "触控辅助",
            nameof(TwoFingerTap) => "双指点按或轻点",
            nameof(RightClickZone) => "点按右下角",
            nameof(Modifiers) => "键盘辅助",
            nameof(ModifiersOff) => "关",
            nameof(ModifiersAlt) => "Alt 键",
            nameof(ModifiersControl) => "Control 键",
            nameof(ModifiersShift) => "Shift 键",
            nameof(HideTrayIcon) => "隐藏托盘图标",
            nameof(StartWithWindows) => "开机启动",
            nameof(Exit) => "退出",
            _ => name,
        };

        private static string ReturnTraditionalChinese(string name) => name switch
        {
            nameof(Touchpads) => "觸控輔助",
            nameof(TwoFingerTap) => "雙指點按或輕點",
            nameof(RightClickZone) => "點按右下角",
            nameof(Modifiers) => "鍵盤輔助",
            nameof(ModifiersOff) => "關",
            nameof(ModifiersAlt) => "Alt 鍵",
            nameof(ModifiersControl) => "Control 鍵",
            nameof(ModifiersShift) => "Shift 鍵",
            nameof(HideTrayIcon) => "隱藏托盤圖示",
            nameof(StartWithWindows) => "開機啟動",
            nameof(Exit) => "退出",
            _ => name,
        };

        private static string ReturnJapanese(string name) => name switch
        {
            nameof(Touchpads) => "タッチパッド",
            nameof(TwoFingerTap) => "2 本指タップ",
            nameof(RightClickZone) => "右下をクリック",
            nameof(Modifiers) => "キーボード",
            nameof(ModifiersOff) => "オフ",
            nameof(ModifiersAlt) => "Alt キー",
            nameof(ModifiersControl) => "Control キー",
            nameof(ModifiersShift) => "Shift キー",
            nameof(HideTrayIcon) => "トレイアイコンを隠す",
            nameof(StartWithWindows) => "起動時に実行",
            nameof(Exit) => "終了",
            _ => name,
        };

        private static string ReturnEnglish(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Two-finger tap",
            nameof(RightClickZone) => "Bottom-right click",
            nameof(Modifiers) => "Keyboard",
            nameof(ModifiersOff) => "Off",
            nameof(ModifiersAlt) => "Alt Key",
            nameof(ModifiersControl) => "Control Key",
            nameof(ModifiersShift) => "Shift Key",
            nameof(HideTrayIcon) => "Hide tray icon",
            nameof(StartWithWindows) => "Start with Windows",
            nameof(Exit) => "Exit",
            _ => name,
        };

        private static string ReturnArabic(string name) => name switch
        {
            nameof(Touchpads) => "لوحة اللمس",
            nameof(TwoFingerTap) => "نقر بإصبعين",
            nameof(RightClickZone) => "نقر أسفل اليمين",
            nameof(Modifiers) => "لوحة المفاتيح",
            nameof(ModifiersOff) => "إيقاف",
            nameof(ModifiersAlt) => "مفتاح Alt",
            nameof(ModifiersControl) => "مفتاح Control",
            nameof(ModifiersShift) => "مفتاح Shift",
            nameof(HideTrayIcon) => "إخفاء أيقونة العلبة",
            nameof(StartWithWindows) => "التشغيل مع Windows",
            nameof(Exit) => "خروج",
            _ => name,
        };

        private static string ReturnHungarian(string name) => name switch
        {
            nameof(Touchpads) => "Érintőpad",
            nameof(TwoFingerTap) => "Kétujjas koppintás",
            nameof(RightClickZone) => "Kattintás jobb alsó sarokban",
            nameof(Modifiers) => "Billentyűzet",
            nameof(ModifiersOff) => "Kikapcsolás",
            nameof(ModifiersAlt) => "Alt billentyű",
            nameof(ModifiersControl) => "Ctrl billentyű",
            nameof(ModifiersShift) => "Shift billentyű",
            nameof(HideTrayIcon) => "Tálcaikon elrejtése",
            nameof(StartWithWindows) => "Indítás a Windowszal",
            nameof(Exit) => "Kilépés",
            _ => name,
        };

        private static string ReturnSlovak(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Klepnutie dvoma prstami",
            nameof(RightClickZone) => "Klik v pravom dolnom rohu",
            nameof(Modifiers) => "Klávesnica",
            nameof(ModifiersOff) => "Vypnuté",
            nameof(ModifiersAlt) => "Kláves Alt",
            nameof(ModifiersControl) => "Kláves Control",
            nameof(ModifiersShift) => "Kláves Shift",
            nameof(HideTrayIcon) => "Skryť ikonu na paneli",
            nameof(StartWithWindows) => "Spustiť pri štarte Windowsu",
            nameof(Exit) => "Ukončenie",
            _ => name,
        };

        private static string ReturnIndonesian(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Ketuk dua jari",
            nameof(RightClickZone) => "Klik di sudut kanan bawah",
            nameof(Modifiers) => "Keyboard",
            nameof(ModifiersOff) => "Nonaktif",
            nameof(ModifiersAlt) => "Tombol Alt",
            nameof(ModifiersControl) => "Tombol Control",
            nameof(ModifiersShift) => "Tombol Shift",
            nameof(HideTrayIcon) => "Sembunyikan ikon tray",
            nameof(StartWithWindows) => "Mulai bersama Windows",
            nameof(Exit) => "Keluar",
            _ => name,
        };

        private static string ReturnKorean(string name) => name switch
        {
            nameof(Touchpads) => "터치패드",
            nameof(TwoFingerTap) => "두 손가락 탭",
            nameof(RightClickZone) => "오른쪽 아래 클릭",
            nameof(Modifiers) => "키보드",
            nameof(ModifiersOff) => "끄기",
            nameof(ModifiersAlt) => "Alt 키",
            nameof(ModifiersControl) => "Control 키",
            nameof(ModifiersShift) => "Shift 키",
            nameof(HideTrayIcon) => "트레이 아이콘 숨기기",
            nameof(StartWithWindows) => "Windows 시작 시 실행",
            nameof(Exit) => "종료",
            _ => name,
        };

        private static string ReturnCatalan(string name) => name switch
        {
            nameof(Touchpads) => "Placa tàctil",
            nameof(TwoFingerTap) => "Toc amb dos dits",
            nameof(RightClickZone) => "Clic a la cantonada inferior dreta",
            nameof(Modifiers) => "Teclat",
            nameof(ModifiersOff) => "Desactivat",
            nameof(ModifiersAlt) => "Tecla Alt",
            nameof(ModifiersControl) => "Tecla Control",
            nameof(ModifiersShift) => "Tecla Shift",
            nameof(HideTrayIcon) => "Amaga la icona de la safata",
            nameof(StartWithWindows) => "Inicia amb Windows",
            nameof(Exit) => "Sortir",
            _ => name,
        };

        private static string ReturnGerman(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Tippen mit zwei Fingern",
            nameof(RightClickZone) => "Klick unten rechts",
            nameof(Modifiers) => "Tastatur",
            nameof(ModifiersOff) => "Aus",
            nameof(ModifiersAlt) => "Alt-Taste",
            nameof(ModifiersControl) => "Strg-Taste",
            nameof(ModifiersShift) => "Umschalt-Taste",
            nameof(HideTrayIcon) => "Taskleistensymbol ausblenden",
            nameof(StartWithWindows) => "Mit Windows starten",
            nameof(Exit) => "Beenden",
            _ => name,
        };

        private static string ReturnSpanish(string name) => name switch
        {
            nameof(Touchpads) => "Panel táctil",
            nameof(TwoFingerTap) => "Tocar con dos dedos",
            nameof(RightClickZone) => "Clic en la esquina inferior derecha",
            nameof(Modifiers) => "Teclado",
            nameof(ModifiersOff) => "Desactivado",
            nameof(ModifiersAlt) => "Tecla Alt",
            nameof(ModifiersControl) => "Tecla Control",
            nameof(ModifiersShift) => "Tecla Mayús",
            nameof(HideTrayIcon) => "Ocultar icono de la bandeja",
            nameof(StartWithWindows) => "Iniciar con Windows",
            nameof(Exit) => "Salir",
            _ => name,
        };

        private static string ReturnFrench(string name) => name switch
        {
            nameof(Touchpads) => "Pavé tactile",
            nameof(TwoFingerTap) => "Tapoter à deux doigts",
            nameof(RightClickZone) => "Clic en bas à droite",
            nameof(Modifiers) => "Clavier",
            nameof(ModifiersOff) => "Désactivé",
            nameof(ModifiersAlt) => "Touche Alt",
            nameof(ModifiersControl) => "Touche Ctrl",
            nameof(ModifiersShift) => "Touche Maj",
            nameof(HideTrayIcon) => "Masquer l'icône de la zone de notification",
            nameof(StartWithWindows) => "Démarrer avec Windows",
            nameof(Exit) => "Quitter",
            _ => name,
        };

        private static string ReturnItalian(string name) => name switch
        {
            nameof(Touchpads) => "Trackpad",
            nameof(TwoFingerTap) => "Tocco con due dita",
            nameof(RightClickZone) => "Clic in basso a destra",
            nameof(Modifiers) => "Tastiera",
            nameof(ModifiersOff) => "Disattivato",
            nameof(ModifiersAlt) => "Tasto Alt",
            nameof(ModifiersControl) => "Tasto Ctrl",
            nameof(ModifiersShift) => "Tasto Maiusc",
            nameof(HideTrayIcon) => "Nascondi icona nell'area di notifica",
            nameof(StartWithWindows) => "Avvia con Windows",
            nameof(Exit) => "Esci",
            _ => name,
        };

        private static string ReturnNorwegian(string name) => name switch
        {
            nameof(Touchpads) => "Styreplate",
            nameof(TwoFingerTap) => "Trykk med to fingre",
            nameof(RightClickZone) => "Klikk nederst til høyre",
            nameof(Modifiers) => "Tastatur",
            nameof(ModifiersOff) => "Av",
            nameof(ModifiersAlt) => "Alt-tasten",
            nameof(ModifiersControl) => "Ctrl-tasten",
            nameof(ModifiersShift) => "Shift-tasten",
            nameof(HideTrayIcon) => "Skjul systemstatusikon",
            nameof(StartWithWindows) => "Start med Windows",
            nameof(Exit) => "Avslutt",
            _ => name,
        };

        private static string ReturnDutch(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Tikken met twee vingers",
            nameof(RightClickZone) => "Klik rechtsonder",
            nameof(Modifiers) => "Toetsenbord",
            nameof(ModifiersOff) => "Uit",
            nameof(ModifiersAlt) => "Alt-toets",
            nameof(ModifiersControl) => "Ctrl-toets",
            nameof(ModifiersShift) => "Shift-toets",
            nameof(HideTrayIcon) => "Verberg systeemvakpictogram",
            nameof(StartWithWindows) => "Start met Windows",
            nameof(Exit) => "Afsluiten",
            _ => name,
        };

        private static string ReturnPolish(string name) => name switch
        {
            nameof(Touchpads) => "Trackpad",
            nameof(TwoFingerTap) => "Dotknięcie dwoma palcami",
            nameof(RightClickZone) => "Klik w prawym dolnym rogu",
            nameof(Modifiers) => "Klawiatura",
            nameof(ModifiersOff) => "Wyłączono",
            nameof(ModifiersAlt) => "Klawisz Alt",
            nameof(ModifiersControl) => "Klawisz Ctrl",
            nameof(ModifiersShift) => "Klawisz Shift",
            nameof(HideTrayIcon) => "Ukryj ikonę zasobnika",
            nameof(StartWithWindows) => "Uruchamiaj z systemem Windows",
            nameof(Exit) => "Wyjście",
            _ => name,
        };

        private static string ReturnPortugueseBrazil(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Tocar com dois dedos",
            nameof(RightClickZone) => "Clicar no canto inferior direito",
            nameof(Modifiers) => "Teclado",
            nameof(ModifiersOff) => "Desativado",
            nameof(ModifiersAlt) => "Tecla Alt",
            nameof(ModifiersControl) => "Tecla Ctrl",
            nameof(ModifiersShift) => "Tecla Shift",
            nameof(HideTrayIcon) => "Ocultar ícone da bandeja",
            nameof(StartWithWindows) => "Iniciar com o Windows",
            nameof(Exit) => "Sair",
            _ => name,
        };

        private static string ReturnPortuguesePortugal(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Toque com dois dedos",
            nameof(RightClickZone) => "Clique no canto inferior direito",
            nameof(Modifiers) => "Teclado",
            nameof(ModifiersOff) => "Desativado",
            nameof(ModifiersAlt) => "Tecla Alt",
            nameof(ModifiersControl) => "Tecla Ctrl",
            nameof(ModifiersShift) => "Tecla Shift",
            nameof(HideTrayIcon) => "Ocultar ícone da bandeja",
            nameof(StartWithWindows) => "Iniciar com o Windows",
            nameof(Exit) => "Sair",
            _ => name,
        };

        private static string ReturnRussian(string name) => name switch
        {
            nameof(Touchpads) => "Тачпад",
            nameof(TwoFingerTap) => "Касание двумя пальцами",
            nameof(RightClickZone) => "Клик в правом нижнем углу",
            nameof(Modifiers) => "Клавиатура",
            nameof(ModifiersOff) => "Отключено",
            nameof(ModifiersAlt) => "Клавиша Alt",
            nameof(ModifiersControl) => "Клавиша Ctrl",
            nameof(ModifiersShift) => "Клавиша Shift",
            nameof(HideTrayIcon) => "Скрыть значок в трее",
            nameof(StartWithWindows) => "Запускать вместе с Windows",
            nameof(Exit) => "Выход",
            _ => name,
        };

        private static string ReturnTurkish(string name) => name switch
        {
            nameof(Touchpads) => "Dokunmatik yüzey",
            nameof(TwoFingerTap) => "İki parmakla dokun",
            nameof(RightClickZone) => "Sağ alt köşeye tıkla",
            nameof(Modifiers) => "Klavye",
            nameof(ModifiersOff) => "Kapalı",
            nameof(ModifiersAlt) => "Alt Tuşu",
            nameof(ModifiersControl) => "Ctrl Tuşu",
            nameof(ModifiersShift) => "Shift Tuşu",
            nameof(HideTrayIcon) => "Tepsi simgesini gizle",
            nameof(StartWithWindows) => "Windows ile başlat",
            nameof(Exit) => "Çıkış",
            _ => name,
        };

        private static string ReturnUkrainian(string name) => name switch
        {
            nameof(Touchpads) => "Тачпад",
            nameof(TwoFingerTap) => "Дотик двома пальцями",
            nameof(RightClickZone) => "Клік у правому нижньому куті",
            nameof(Modifiers) => "Клавіатура",
            nameof(ModifiersOff) => "Відключено",
            nameof(ModifiersAlt) => "Клавіша Alt",
            nameof(ModifiersControl) => "Клавіша Ctrl",
            nameof(ModifiersShift) => "Клавіша Shift",
            nameof(HideTrayIcon) => "Приховати піктограму в треї",
            nameof(StartWithWindows) => "Запускати разом із Windows",
            nameof(Exit) => "Вихід",
            _ => name,
        };

        private static string ReturnVietnamese(string name) => name switch
        {
            nameof(Touchpads) => "Bàn di chuột",
            nameof(TwoFingerTap) => "Chạm hai ngón",
            nameof(RightClickZone) => "Nhấp góc phải dưới",
            nameof(Modifiers) => "Bàn phím",
            nameof(ModifiersOff) => "Tắt",
            nameof(ModifiersAlt) => "Phím Alt",
            nameof(ModifiersControl) => "Phím Ctrl",
            nameof(ModifiersShift) => "Phím Shift",
            nameof(HideTrayIcon) => "Ẩn biểu tượng khay hệ thống",
            nameof(StartWithWindows) => "Khởi động cùng Windows",
            nameof(Exit) => "Thoát",
            _ => name,
        };

        private static string ReturnMarathi(string name) => name switch
        {
            nameof(Touchpads) => "टचपॅड",
            nameof(TwoFingerTap) => "दोन बोटांनी टॅप",
            nameof(RightClickZone) => "उजव्या खालच्या कोपऱ्यात क्लिक",
            nameof(Modifiers) => "कीबोर्ड",
            nameof(ModifiersOff) => "बंद",
            nameof(ModifiersAlt) => "Alt की",
            nameof(ModifiersControl) => "Control की",
            nameof(ModifiersShift) => "Shift की",
            nameof(HideTrayIcon) => "ट्रे चिन्ह लपवा",
            nameof(StartWithWindows) => "Windows सोबत सुरू करा",
            nameof(Exit) => "बाहेर पडा",
            _ => name,
        };

        private static string ReturnHindi(string name) => name switch
        {
            nameof(Touchpads) => "टचपैड",
            nameof(TwoFingerTap) => "दो उंगलियों से टैप",
            nameof(RightClickZone) => "दाएं निचले कोने में क्लिक",
            nameof(Modifiers) => "कीबोर्ड",
            nameof(ModifiersOff) => "बंद",
            nameof(ModifiersAlt) => "Alt कुंजी",
            nameof(ModifiersControl) => "Control कुंजी",
            nameof(ModifiersShift) => "Shift कुंजी",
            nameof(HideTrayIcon) => "ट्रे आइकन छिपाएं",
            nameof(StartWithWindows) => "Windows के साथ शुरू करें",
            nameof(Exit) => "बाहर निकलें",
            _ => name,
        };

        private static string ReturnHebrew(string name) => name switch
        {
            nameof(Touchpads) => "משטח מגע",
            nameof(TwoFingerTap) => "הקשה בשתי אצבעות",
            nameof(RightClickZone) => "לחיצה בפינה הימנית התחתונה",
            nameof(Modifiers) => "מקלדת",
            nameof(ModifiersOff) => "כבוי",
            nameof(ModifiersAlt) => "מקש Alt",
            nameof(ModifiersControl) => "מקש Ctrl",
            nameof(ModifiersShift) => "מקש Shift",
            nameof(HideTrayIcon) => "הסתר סמל מגש",
            nameof(StartWithWindows) => "הפעל עם Windows",
            nameof(Exit) => "יציאה",
            _ => name,
        };

        private static string ReturnGreek(string name) => name switch
        {
            nameof(Touchpads) => "Επιφάνεια αφής",
            nameof(TwoFingerTap) => "Άγγιγμα με δύο δάχτυλα",
            nameof(RightClickZone) => "Κλικ κάτω δεξιά",
            nameof(Modifiers) => "Πληκτρολόγιο",
            nameof(ModifiersOff) => "Ανενεργό",
            nameof(ModifiersAlt) => "Πλήκτρο Alt",
            nameof(ModifiersControl) => "Πλήκτρο Ctrl",
            nameof(ModifiersShift) => "Πλήκτρο Shift",
            nameof(HideTrayIcon) => "Απόκρυψη εικονιδίου δίσκου",
            nameof(StartWithWindows) => "Εκκίνηση με τα Windows",
            nameof(Exit) => "Έξοδος",
            _ => name,
        };

        private static string ReturnSwedish(string name) => name switch
        {
            nameof(Touchpads) => "Styrplatta",
            nameof(TwoFingerTap) => "Tryck med två fingrar",
            nameof(RightClickZone) => "Klicka nere till höger",
            nameof(Modifiers) => "Tangentbord",
            nameof(ModifiersOff) => "Av",
            nameof(ModifiersAlt) => "Alt-tangenten",
            nameof(ModifiersControl) => "Ctrl-tangenten",
            nameof(ModifiersShift) => "Shift-tangenten",
            nameof(HideTrayIcon) => "Dölj aktivitetsfältsikon",
            nameof(StartWithWindows) => "Starta med Windows",
            nameof(Exit) => "Avsluta",
            _ => name,
        };

        private static string ReturnRomanian(string name) => name switch
        {
            nameof(Touchpads) => "Touchpad",
            nameof(TwoFingerTap) => "Atingere cu două degete",
            nameof(RightClickZone) => "Clic în colțul din dreapta jos",
            nameof(Modifiers) => "Tastatură",
            nameof(ModifiersOff) => "Dezactivat",
            nameof(ModifiersAlt) => "Tasta Alt",
            nameof(ModifiersControl) => "Tasta Control",
            nameof(ModifiersShift) => "Tasta Shift",
            nameof(HideTrayIcon) => "Ascunde pictograma din tavă",
            nameof(StartWithWindows) => "Pornește odată cu Windows",
            nameof(Exit) => "Ieșire",
            _ => name,
        };
    }
}

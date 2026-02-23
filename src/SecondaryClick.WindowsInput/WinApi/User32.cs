using System.Runtime.InteropServices;

namespace System.WindowsInput.WinApi;

public static class User32
{
    /// <summary>
    /// Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on,
    /// off—alternating each time the key is pressed).
    /// </summary>
    /// <param name="nVirtKey">
    /// <para>Type: <c>int</c></para>
    /// <para>
    /// A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set
    /// to the ASCII value of that character. For other keys, it must be a virtual-key code.
    /// </para>
    /// <para>
    /// If a non-English keyboard layout is used, virtual keys with values in the range ASCII A through Z and 0 through 9 are used to
    /// specify most of the character keys. For example, for the German keyboard layout, the virtual key of value ASCII O (0x4F) refers
    /// to the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
    /// </para>
    /// </param>
    /// <returns>
    /// <para>Type: <c>SHORT</c></para>
    /// <para>The return value specifies the status of the specified virtual key, as follows:</para>
    /// <list type="bullet">
    /// <item>
    /// <term>If the high-order bit is 1, the key is down; otherwise, it is up.</term>
    /// </item>
    /// <item>
    /// <term>
    /// If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off
    /// and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is
    /// toggled, and off when the key is untoggled.
    /// </term>
    /// </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// <para>
    /// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not
    /// reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information.
    /// </para>
    /// <para>
    /// An application calls <c>GetKeyState</c> in response to a keyboard-input message. This function retrieves the state of the key
    /// when the input message was generated.
    /// </para>
    /// <para>To retrieve state information for all the virtual keys, use the GetKeyboardState function.</para>
    /// <para>
    /// An application can use the virtual key code constants <c>VK_SHIFT</c>, <c>VK_CONTROL</c>, and <c>VK_MENU</c> as values for the
    /// nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An
    /// application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and
    /// right instances of those keys:
    /// </para>
    /// <para>
    /// <c>VK_LSHIFT</c><c>VK_RSHIFT</c><c>VK_LCONTROL</c><c>VK_RCONTROL</c><c>VK_LMENU</c><c>VK_RMENU</c> These left- and
    /// right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState,
    /// GetAsyncKeyState, <c>GetKeyState</c>, and MapVirtualKey functions.
    /// </para>
    /// <para>Examples</para>
    /// <para>For an example, see Displaying Keyboard Input.</para>
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getkeystate SHORT GetKeyState( int nVirtKey );
    [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
    public static extern short GetKeyState(EnumRebase<VK, int> nVirtKey);

    /// <summary>
    /// Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call
    /// to <c>GetAsyncKeyState</c>.
    /// </summary>
    /// <param name="vKey">
    /// <para>Type: <c>int</c></para>
    /// <para>The virtual-key code. For more information, see Virtual Key Codes.</para>
    /// <para>You can use left- and right-distinguishing constants to specify certain keys. See the Remarks section for further information.</para>
    /// </param>
    /// <returns>
    /// <para>Type: <c>SHORT</c></para>
    /// <para>
    /// If the function succeeds, the return value specifies whether the key was pressed since the last call to <c>GetAsyncKeyState</c>,
    /// and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant
    /// bit is set, the key was pressed after the previous call to <c>GetAsyncKeyState</c>. However, you should not rely on this last
    /// behavior; for more information, see the Remarks.
    /// </para>
    /// <para>The return value is zero for the following cases:</para>
    /// <list type="bullet">
    /// <item>
    /// <term>The current desktop is not the active desktop</term>
    /// </item>
    /// <item>
    /// <term>The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.</term>
    /// </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// <para>
    /// The <c>GetAsyncKeyState</c> function works with mouse buttons. However, it checks on the state of the physical mouse buttons,
    /// not on the logical mouse buttons that the physical buttons are mapped to. For example, the call
    /// <c>GetAsyncKeyState</c>(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is
    /// mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to
    /// logical mouse buttons by calling .
    /// </para>
    /// <para>which returns TRUE if the mouse buttons have been swapped.</para>
    /// <para>
    /// Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to
    /// the pre-emptive multitasking nature of Windows, another application can call <c>GetAsyncKeyState</c> and receive the "recently
    /// pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for
    /// compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
    /// </para>
    /// <para>
    /// You can use the virtual-key code constants <c>VK_SHIFT</c>, <c>VK_CONTROL</c>, and <c>VK_MENU</c> as values for the vKey
    /// parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right.
    /// </para>
    /// <para>
    /// You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of
    /// those keys.
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Code</term>
    /// <term>Meaning</term>
    /// </listheader>
    /// <item>
    /// <term>VK_LSHIFT</term>
    /// <term>Left-shift key.</term>
    /// </item>
    /// <item>
    /// <term>VK_RSHIFT</term>
    /// <term>Right-shift key.</term>
    /// </item>
    /// <item>
    /// <term>VK_LCONTROL</term>
    /// <term>Left-control key.</term>
    /// </item>
    /// <item>
    /// <term>VK_RCONTROL</term>
    /// <term>Right-control key.</term>
    /// </item>
    /// <item>
    /// <term>VK_LMENU</term>
    /// <term>Left-menu key.</term>
    /// </item>
    /// <item>
    /// <term>VK_RMENU</term>
    /// <term>Right-menu key.</term>
    /// </item>
    /// </list>
    /// <para>
    /// These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState,
    /// <c>GetAsyncKeyState</c>, GetKeyState, and MapVirtualKey functions.
    /// </para>
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getasynckeystate SHORT GetAsyncKeyState( int vKey );
    [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
    public static extern short GetAsyncKeyState(EnumRebase<VK, int> vKey);

    /// <summary>Synthesizes keystrokes, mouse motions, and button clicks.</summary>
    /// <param name="cInputs">
    /// <para>Type: <c>UINT</c></para>
    /// <para>The number of structures in the pInputs array.</para>
    /// </param>
    /// <param name="pInputs">
    /// <para>Type: <c>LPINPUT</c></para>
    /// <para>An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</para>
    /// </param>
    /// <param name="cbSize">
    /// <para>Type: <c>int</c></para>
    /// <para>The size, in bytes, of an INPUT structure. If cbSize is not the size of an <c>INPUT</c> structure, the function fails.</para>
    /// </param>
    /// <returns>
    /// <para>Type: <c>UINT</c></para>
    /// <para>
    /// The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function
    /// returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.
    /// </para>
    /// <para>
    /// This function fails when it is blocked by UIPI. Note that neither GetLastError nor the return value will indicate the failure
    /// was caused by UIPI blocking.
    /// </para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or
    /// lesser integrity level.
    /// </para>
    /// <para>
    /// The <c>SendInput</c> function inserts the events in the INPUT structures serially into the keyboard or mouse input stream. These
    /// events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse)
    /// or by calls to keybd_event, mouse_event, or other calls to <c>SendInput</c>.
    /// </para>
    /// <para>
    /// This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might
    /// interfere with the events that this function generates. To avoid this problem, check the keyboard's state with the
    /// GetAsyncKeyState function and correct as necessary.
    /// </para>
    /// <para>
    /// Because the touch keyboard uses the surrogate macros defined in winnls.h to send input to the system, a listener on the keyboard
    /// event hook must decode input originating from the touch keyboard. For more information, see Surrogates and Supplementary Characters.
    /// </para>
    /// <para>
    /// An accessibility application can use <c>SendInput</c> to inject keystrokes corresponding to application launch shortcut keys
    /// that are handled by the shell. This functionality is not guaranteed to work for other types of applications.
    /// </para>
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendinput UINT SendInput( UINT cInputs, LPINPUT pInputs,
    // int cbSize );
    [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern uint SendInput(uint cInputs, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] INPUT[] pInputs, int cbSize);

    /// <summary>
    /// <para>
    /// Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code.
    /// </para>
    /// <para>To specify a handle to the keyboard layout to use for translating the specified code, use the MapVirtualKeyEx function.</para>
    /// </summary>
    /// <param name="uCode">
    /// <para>Type: <c>UINT</c></para>
    /// <para>The virtual key code or scan code for a key. How this value is interpreted depends on the value of the uMapType parameter.</para>
    /// </param>
    /// <param name="uMapType">
    /// <para>Type: <c>UINT</c></para>
    /// <para>The translation to be performed. The value of this parameter depends on the value of the uCode parameter.</para>
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term>
    /// <term>Meaning</term>
    /// </listheader>
    /// <item>
    /// <term>MAPVK_VK_TO_CHAR 2</term>
    /// <term>
    /// uCode is a virtual-key code and is translated into an unshifted character value in the low-order word of the return value. Dead
    /// keys (diacritics) are indicated by setting the top bit of the return value. If there is no translation, the function returns 0.
    /// </term>
    /// </item>
    /// <item>
    /// <term>MAPVK_VK_TO_VSC 0</term>
    /// <term>
    /// uCode is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not distinguish between
    /// left- and right-hand keys, the left-hand scan code is returned. If there is no translation, the function returns 0.
    /// </term>
    /// </item>
    /// <item>
    /// <term>MAPVK_VSC_TO_VK 1</term>
    /// <term>
    /// uCode is a scan code and is translated into a virtual-key code that does not distinguish between left- and right-hand keys. If
    /// there is no translation, the function returns 0.
    /// </term>
    /// </item>
    /// <item>
    /// <term>MAPVK_VSC_TO_VK_EX 3</term>
    /// <term>
    /// uCode is a scan code and is translated into a virtual-key code that distinguishes between left- and right-hand keys. If there is
    /// no translation, the function returns 0.
    /// </term>
    /// </item>
    /// </list>
    /// </param>
    /// <returns>
    /// <para>Type: <c>UINT</c></para>
    /// <para>
    /// The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType.
    /// If there is no translation, the return value is zero.
    /// </para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// An application can use <c>MapVirtualKey</c> to translate scan codes to the virtual-key code constants <c>VK_SHIFT</c>,
    /// <c>VK_CONTROL</c>, and <c>VK_MENU</c>, and vice versa. These translations do not distinguish between the left and right
    /// instances of the SHIFT, CTRL, or ALT keys.
    /// </para>
    /// <para>
    /// An application can get the scan code corresponding to the left or right instance of one of these keys by calling
    /// <c>MapVirtualKey</c> with uCode set to one of the following virtual-key code constants.
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <term><c>VK_LSHIFT</c></term>
    /// </item>
    /// <item>
    /// <term><c>VK_RSHIFT</c></term>
    /// </item>
    /// <item>
    /// <term><c>VK_LCONTROL</c></term>
    /// </item>
    /// <item>
    /// <term><c>VK_RCONTROL</c></term>
    /// </item>
    /// <item>
    /// <term><c>VK_LMENU</c></term>
    /// </item>
    /// <item>
    /// <term><c>VK_RMENU</c></term>
    /// </item>
    /// </list>
    /// <para>
    /// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState,
    /// SetKeyboardState, GetAsyncKeyState, GetKeyState, and <c>MapVirtualKey</c> functions.
    /// </para>
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-mapvirtualkeya UINT MapVirtualKeyA( UINT uCode, UINT
    // uMapType );
    [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto)]
    public static extern uint MapVirtualKey(uint uCode, MAPVK uMapType);

    /// <summary>Contains information about a simulated message generated by an input device other than a keyboard or mouse.</summary>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-taghardwareinput typedef struct tagHARDWAREINPUT { DWORD
    // uMsg; WORD wParamL; WORD wParamH; } HARDWAREINPUT, *PHARDWAREINPUT, *LPHARDWAREINPUT;
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>The message generated by the input hardware.</para>
        /// </summary>
        public uint uMsg;

        /// <summary>
        /// <para>Type: <c>WORD</c></para>
        /// <para>The low-order word of the lParam parameter for <c>uMsg</c>.</para>
        /// </summary>
        public ushort wParamL;

        /// <summary>
        /// <para>Type: <c>WORD</c></para>
        /// <para>The high-order word of the lParam parameter for <c>uMsg</c>.</para>
        /// </summary>
        public ushort wParamH;
    }

    /// <summary>
    /// Used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    /// <remarks>
    /// <c>INPUT_KEYBOARD</c> supports nonkeyboard input methods, such as handwriting recognition or voice recognition, as if it were
    /// text input by using the <c>KEYEVENTF_UNICODE</c> flag. For more information, see the remarks section of KEYBDINPUT.
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-taginput typedef struct tagINPUT { DWORD type; union {
    // MOUSEINPUT mi; KEYBDINPUT ki; HARDWAREINPUT hi; } DUMMYUNIONNAME; } INPUT, *PINPUT, *LPINPUT;
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>The type of the input event. This member can be one of the following values.</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>INPUT_MOUSE 0</term>
        /// <term>The event is a mouse event. Use the mi structure of the union.</term>
        /// </item>
        /// <item>
        /// <term>INPUT_KEYBOARD 1</term>
        /// <term>The event is a keyboard event. Use the ki structure of the union.</term>
        /// </item>
        /// <item>
        /// <term>INPUT_HARDWARE 2</term>
        /// <term>The event is a hardware event. Use the hi structure of the union.</term>
        /// </item>
        /// </list>
        /// </summary>
        public INPUTTYPE type;

        private UNION union;

        /// <summary>
        /// <para>Type: <c>MOUSEINPUT</c></para>
        /// <para>The information about a simulated mouse event.</para>
        /// </summary>
        public MOUSEINPUT mi { readonly get => union.mi; set => union.mi = value; }

        /// <summary>
        /// <para>Type: <c>KEYBDINPUT</c></para>
        /// <para>The information about a simulated keyboard event.</para>
        /// </summary>
        public KEYBDINPUT ki { readonly get => union.ki; set => union.ki = value; }

        /// <summary>
        /// <para>Type: <c>HARDWAREINPUT</c></para>
        /// <para>The information about a simulated hardware event.</para>
        /// </summary>
        public HARDWAREINPUT hi { readonly get => union.hi; set => union.hi = value; }

        [StructLayout(LayoutKind.Explicit)]
        private struct UNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        /// <summary>Initializes a new instance of the <see cref="INPUT"/> struct for keyboard input.</summary>
        /// <param name="keyFlags">Specifies various aspects of a keystroke.</param>
        /// <param name="vkOrScan">
        /// If KEYEVENTF_SCANCODE, the value represents a hardware scan code for the key, otherwise, this value represents a virtual key-code.
        /// </param>
        public INPUT(KEYEVENTF keyFlags, ushort vkOrScan)
        {
            type = INPUTTYPE.INPUT_KEYBOARD;
            union = new UNION { ki = new KEYBDINPUT { dwFlags = keyFlags, wVk = (keyFlags & KEYEVENTF.KEYEVENTF_SCANCODE) == 0 ? vkOrScan : (ushort)0, wScan = (keyFlags & KEYEVENTF.KEYEVENTF_SCANCODE) != 0 ? vkOrScan : (ushort)0 } };
        }

        /// <summary>Initializes a new instance of the <see cref="INPUT"/> struct for mouse input.</summary>
        /// <param name="keyFlags">A set of bit flags that specify various aspects of mouse motion and button clicks.</param>
        /// <param name="mouseData">
        /// <para>
        /// If <c>dwFlags</c> contains <c>MOUSEEVENTF_WHEEL</c>, then <c>mouseData</c> specifies the amount of wheel movement. A
        /// positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel
        /// was rotated backward, toward the user. One wheel click is defined as <c>WHEEL_DELTA</c>, which is 120.
        /// </para>
        /// <para>
        /// Windows Vista: If dwFlags contains <c>MOUSEEVENTF_HWHEEL</c>, then dwData specifies the amount of wheel movement. A positive
        /// value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.
        /// One wheel click is defined as <c>WHEEL_DELTA</c>, which is 120.
        /// </para>
        /// <para>
        /// If <c>dwFlags</c> does not contain <c>MOUSEEVENTF_WHEEL</c>, <c>MOUSEEVENTF_XDOWN</c>, or <c>MOUSEEVENTF_XUP</c>, then
        /// <c>mouseData</c> should be zero.
        /// </para>
        /// <para>
        /// If <c>dwFlags</c> contains <c>MOUSEEVENTF_XDOWN</c> or <c>MOUSEEVENTF_XUP</c>, then <c>mouseData</c> specifies which X
        /// buttons were pressed or released. This value may be any combination of the following flags.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>XBUTTON1 0x0001</term>
        /// <term>Set if the first X button is pressed or released.</term>
        /// </item>
        /// <item>
        /// <term>XBUTTON2 0x0002</term>
        /// <term>Set if the second X button is pressed or released.</term>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="dx">
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value
        /// of the <c>dwFlags</c> member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the
        /// number of pixels moved.
        /// </param>
        /// <param name="dy">
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value
        /// of the <c>dwFlags</c> member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the
        /// number of pixels moved.
        /// </param>
        public INPUT(MOUSEEVENTF keyFlags, int mouseData = 0, int dx = 0, int dy = 0)
        {
            type = INPUTTYPE.INPUT_MOUSE;
            union = new UNION { mi = new MOUSEINPUT { dwFlags = keyFlags, dx = dx, dy = dy, mouseData = mouseData } };
        }
    }

    /// <summary>Type for <see cref="INPUT"/> structure.</summary>
    public enum INPUTTYPE
    {
        /// <summary>The event is a mouse event. Use the mi structure of the union.</summary>
        INPUT_MOUSE = 0,

        /// <summary>The event is a keyboard event. Use the ki structure of the union.</summary>
        INPUT_KEYBOARD = 1,

        /// <summary>The event is a hardware event. Use the hi structure of the union.</summary>
        INPUT_HARDWARE = 2,
    }

    //
    // Summary:
    //     Contains information about a simulated keyboard event.
    //
    // Remarks:
    //     INPUT_KEYBOARD supports nonkeyboard-input methods—such as handwriting recognition
    //     or voice recognition—as if it were text input by using the KEYEVENTF_UNICODE
    //     flag. If KEYEVENTF_UNICODE is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP
    //     message to the foreground thread's message queue with wParam equal to VK_PACKET.
    //     Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage
    //     posts a WM_CHAR message with the Unicode character originally specified by wScan.
    //     This Unicode character will automatically be converted to the appropriate ANSI
    //     value if it is posted to an ANSI window.
    //
    //     Set the KEYEVENTF_SCANCODE flag to define keyboard input in terms of the scan
    //     code. This is useful to simulate a physical keystroke regardless of which keyboard
    //     is currently being used. The virtual key value of a key may alter depending on
    //     the current keyboard layout or what other keys were pressed, but the scan code
    //     will always be the same.
    public struct KEYBDINPUT
    {
        //
        // Summary:
        //     Type: WORD
        //
        //     A virtual-key code. The code must be a value in the range 1 to 254. If the dwFlags
        //     member specifies KEYEVENTF_UNICODE, wVk must be 0.
        public EnumRebase<VK, ushort> wVk;

        //
        // Summary:
        //     Type: WORD
        //
        //     A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan
        //     specifies a Unicode character which is to be sent to the foreground application.
        public ushort wScan;

        //
        // Summary:
        //     Type: DWORD
        //
        //     Specifies various aspects of a keystroke. This member can be certain combinations
        //     of the following values.
        //
        //     Value – Meaning –
        //     KEYEVENTF_EXTENDEDKEY 0x0001 – If specified, the scan code was preceded by a
        //     prefix byte that has the value 0xE0 (224). –
        //     KEYEVENTF_KEYUP 0x0002 – If specified, the key is being released. If not specified,
        //     the key is being pressed. –
        //     KEYEVENTF_SCANCODE 0x0008 – If specified, wScan identifies the key and wVk is
        //     ignored. –
        //     KEYEVENTF_UNICODE 0x0004 – If specified, the system synthesizes a VK_PACKET keystroke.
        //     The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP
        //     flag. For more information, see the Remarks section. –
        public KEYEVENTF dwFlags;

        //
        // Summary:
        //     Type: DWORD
        //
        //     The time stamp for the event, in milliseconds. If this parameter is zero, the
        //     system will provide its own time stamp.
        public uint time;

        //
        // Summary:
        //     Type: ULONG_PTR
        //
        //     An additional value associated with the keystroke. Use the GetMessageExtraInfo
        //     function to obtain this information.
        public IntPtr dwExtraInfo;
    }

    /// <summary>Controls various aspects of function operation of <see cref="keybd_event"/>.</summary>
    [Flags]
    public enum KEYEVENTF
    {
        /// <summary>If specified, the scan code was preceded by a prefix byte having the value 0xE0 (224).</summary>
        KEYEVENTF_EXTENDEDKEY = 0x0001,

        /// <summary>If specified, the key is being released. If not specified, the key is being depressed.</summary>
        KEYEVENTF_KEYUP = 0x0002,

        /// <summary>
        /// If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined
        /// with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section.
        /// </summary>
        KEYEVENTF_UNICODE = 0x0004,

        /// <summary>If specified, wScan identifies the key and wVk is ignored.</summary>
        KEYEVENTF_SCANCODE = 0x0008,
    }

    /// <summary>Controls various aspects of mouse motion and button clicking.</summary>
    [Flags]
    public enum MOUSEEVENTF
    {
        /// <summary>
        /// The dx and dy parameters contain normalized absolute coordinates. If not set, those parameters contain relative data: the
        /// change in position since the last reported position. This flag can be set, or not set, regardless of what kind of mouse or
        /// mouse-like device, if any, is connected to the system. For further information about relative mouse motion, see the following
        /// Remarks section.
        /// </summary>
        MOUSEEVENTF_ABSOLUTE = 0x8000,

        /// <summary>The left button is down.</summary>
        MOUSEEVENTF_LEFTDOWN = 0x0002,

        /// <summary>The left button is up.</summary>
        MOUSEEVENTF_LEFTUP = 0x0004,

        /// <summary>The middle button is down.</summary>
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,

        /// <summary>The middle button is up.</summary>
        MOUSEEVENTF_MIDDLEUP = 0x0040,

        /// <summary>Movement occurred.</summary>
        MOUSEEVENTF_MOVE = 0x0001,

        /// <summary>The right button is down.</summary>
        MOUSEEVENTF_RIGHTDOWN = 0x0008,

        /// <summary>The right button is up.</summary>
        MOUSEEVENTF_RIGHTUP = 0x0010,

        /// <summary>The wheel has been moved, if the mouse has a wheel. The amount of movement is specified in dwData</summary>
        MOUSEEVENTF_WHEEL = 0x0800,

        /// <summary>An X button was pressed.</summary>
        MOUSEEVENTF_XDOWN = 0x0080,

        /// <summary>An X button was released.</summary>
        MOUSEEVENTF_XUP = 0x0100,

        /// <summary>The wheel button is tilted.</summary>
        MOUSEEVENTF_HWHEEL = 0x01000,

        /// <summary>Do not coalesce mouse moves.</summary>
        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,

        /// <summary>Map to entire virtual desktop</summary>
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
    }

    /// <summary>Contains information about a simulated mouse event.</summary>
    /// <remarks>
    /// <para>
    /// If the mouse has moved, indicated by <c>MOUSEEVENTF_MOVE</c>, <c>dx</c> and <c>dy</c> specify information about that movement.
    /// The information is specified as absolute or relative integer values.
    /// </para>
    /// <para>
    /// If <c>MOUSEEVENTF_ABSOLUTE</c> value is specified, <c>dx</c> and <c>dy</c> contain normalized absolute coordinates between 0 and
    /// 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of
    /// the display surface; coordinate (65535,65535) maps onto the lower-right corner. In a multimonitor system, the coordinates map to
    /// the primary monitor.
    /// </para>
    /// <para>If <c>MOUSEEVENTF_VIRTUALDESK</c> is specified, the coordinates map to the entire virtual desktop.</para>
    /// <para>
    /// If the <c>MOUSEEVENTF_ABSOLUTE</c> value is not specified, <c>dx</c> and <c>dy</c> specify movement relative to the previous
    /// mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse
    /// moved left (or up).
    /// </para>
    /// <para>
    /// Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three
    /// values with the <c>Pointer Speed</c> slider of the Control Panel's <c>Mouse Properties</c> sheet. You can obtain and set these
    /// values using the SystemParametersInfo function.
    /// </para>
    /// <para>
    /// The system applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis is
    /// greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance. If the specified
    /// distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the
    /// system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply
    /// specified relative mouse movement along the x or y axis by up to four times.
    /// </para>
    /// </remarks>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagmouseinput typedef struct tagMOUSEINPUT { LONG dx;
    // LONG dy; DWORD mouseData; DWORD dwFlags; DWORD time; ULONG_PTR dwExtraInfo; } MOUSEINPUT, *PMOUSEINPUT, *LPMOUSEINPUT;
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        /// <summary>
        /// <para>Type: <c>LONG</c></para>
        /// <para>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value
        /// of the <c>dwFlags</c> member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the
        /// number of pixels moved.
        /// </para>
        /// </summary>
        public int dx;

        /// <summary>
        /// <para>Type: <c>LONG</c></para>
        /// <para>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value
        /// of the <c>dwFlags</c> member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the
        /// number of pixels moved.
        /// </para>
        /// </summary>
        public int dy;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// If <c>dwFlags</c> contains <c>MOUSEEVENTF_WHEEL</c>, then <c>mouseData</c> specifies the amount of wheel movement. A
        /// positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel
        /// was rotated backward, toward the user. One wheel click is defined as <c>WHEEL_DELTA</c>, which is 120.
        /// </para>
        /// <para>
        /// Windows Vista: If dwFlags contains <c>MOUSEEVENTF_HWHEEL</c>, then dwData specifies the amount of wheel movement. A positive
        /// value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.
        /// One wheel click is defined as <c>WHEEL_DELTA</c>, which is 120.
        /// </para>
        /// <para>
        /// If <c>dwFlags</c> does not contain <c>MOUSEEVENTF_WHEEL</c>, <c>MOUSEEVENTF_XDOWN</c>, or <c>MOUSEEVENTF_XUP</c>, then
        /// <c>mouseData</c> should be zero.
        /// </para>
        /// <para>
        /// If <c>dwFlags</c> contains <c>MOUSEEVENTF_XDOWN</c> or <c>MOUSEEVENTF_XUP</c>, then <c>mouseData</c> specifies which X
        /// buttons were pressed or released. This value may be any combination of the following flags.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>XBUTTON1 0x0001</term>
        /// <term>Set if the first X button is pressed or released.</term>
        /// </item>
        /// <item>
        /// <term>XBUTTON2 0x0002</term>
        /// <term>Set if the second X button is pressed or released.</term>
        /// </item>
        /// </list>
        /// </summary>
        public int mouseData;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any
        /// reasonable combination of the following values.
        /// </para>
        /// <para>
        /// The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions. For example,
        /// if the left mouse button is pressed and held down, <c>MOUSEEVENTF_LEFTDOWN</c> is set when the left button is first pressed,
        /// but not for subsequent motions. Similarly, <c>MOUSEEVENTF_LEFTUP</c> is set only when the button is first released.
        /// </para>
        /// <para>
        /// You cannot specify both the <c>MOUSEEVENTF_WHEEL</c> flag and either <c>MOUSEEVENTF_XDOWN</c> or <c>MOUSEEVENTF_XUP</c>
        /// flags simultaneously in the <c>dwFlags</c> parameter, because they both require use of the <c>mouseData</c> field.
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>MOUSEEVENTF_ABSOLUTE 0x8000</term>
        /// <term>
        /// The dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the
        /// change in position since the last reported position). This flag can be set, or not set, regardless of what kind of mouse or
        /// other pointing device, if any, is connected to the system. For further information about relative mouse motion, see the
        /// following Remarks section.
        /// </term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_HWHEEL 0x01000</term>
        /// <term>
        /// The wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData. Windows
        /// XP/2000: This value is not supported.
        /// </term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_MOVE 0x0001</term>
        /// <term>Movement occurred.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_MOVE_NOCOALESCE 0x2000</term>
        /// <term>
        /// The WM_MOUSEMOVE messages will not be coalesced. The default behavior is to coalesce WM_MOUSEMOVE messages. Windows XP/2000:
        /// This value is not supported.
        /// </term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_LEFTDOWN 0x0002</term>
        /// <term>The left button was pressed.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_LEFTUP 0x0004</term>
        /// <term>The left button was released.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_RIGHTDOWN 0x0008</term>
        /// <term>The right button was pressed.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_RIGHTUP 0x0010</term>
        /// <term>The right button was released.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_MIDDLEDOWN 0x0020</term>
        /// <term>The middle button was pressed.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_MIDDLEUP 0x0040</term>
        /// <term>The middle button was released.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_VIRTUALDESK 0x4000</term>
        /// <term>Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_WHEEL 0x0800</term>
        /// <term>The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_XDOWN 0x0080</term>
        /// <term>An X button was pressed.</term>
        /// </item>
        /// <item>
        /// <term>MOUSEEVENTF_XUP 0x0100</term>
        /// <term>An X button was released.</term>
        /// </item>
        /// </list>
        /// </summary>
        public MOUSEEVENTF dwFlags;

        /// <summary>
        /// <para>Type: <c>DWORD</c></para>
        /// <para>The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.</para>
        /// </summary>
        public uint time;

        /// <summary>
        /// <para>Type: <c>ULONG_PTR</c></para>
        /// <para>
        /// An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// </para>
        /// </summary>
        public IntPtr dwExtraInfo;
    }

    /// <summary>The translation to be performed in <see cref="MapVirtualKey"/>.</summary>
    public enum MAPVK
    {
        /// <summary>
        /// uCode is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not distinguish
        /// between left- and right-hand keys, the left-hand scan code is returned. If there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_VSC = 0,

        /// <summary>
        /// uCode is a scan code and is translated into a virtual-key code that does not distinguish between left- and right-hand keys.
        /// If there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK = 1,

        /// <summary>
        /// uCode is a virtual-key code and is translated into an unshifted character value in the low-order word of the return value.
        /// Dead keys (diacritics) are indicated by setting the top bit of the return value. If there is no translation, the function
        /// returns 0.
        /// </summary>
        MAPVK_VK_TO_CHAR = 2,

        /// <summary>
        /// uCode is a scan code and is translated into a virtual-key code that distinguishes between left- and right-hand keys. If
        /// there is no translation, the function returns 0.
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 3,

        /// <summary>
        /// The uCode parameter is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not
        /// distinguish between left- and right-hand keys, the left-hand scan code is returned. If the scan code is an extended scan
        /// code, the high byte of the uCode value can contain either 0xe0 or 0xe1 to specify the extended scan code. If there is no
        /// translation, the function returns 0.
        /// </summary>
        MAPVK_VK_TO_VSC_EX = 4,
    }

    /// <summary>
    /// The following table shows the symbolic constant names, hexadecimal values, and mouse or keyboard equivalents for the virtual-key
    /// codes used by the system. The codes are listed in numeric order.
    /// </summary>
    // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    public enum VK : byte
    {
        /// <summary>Left mouse button</summary>
        VK_LBUTTON = 0x01,

        /// <summary>Right mouse button</summary>
        VK_RBUTTON = 0x02,

        /// <summary>Control-break processing</summary>
        VK_CANCEL = 0x03,

        /// <summary>Middle mouse button (three-button mouse)</summary>
        VK_MBUTTON = 0x04,

        /// <summary>X1 mouse button</summary>
        VK_XBUTTON1 = 0x05,

        /// <summary>X2 mouse button</summary>
        VK_XBUTTON2 = 0x06,

        /// <summary>BACKSPACE key</summary>
        VK_BACK = 0x08,

        /// <summary>TAB key</summary>
        VK_TAB = 0x09,

        /// <summary>CLEAR key</summary>
        VK_CLEAR = 0x0C,

        /// <summary>ENTER key</summary>
        VK_RETURN = 0x0D,

        /// <summary>SHIFT key</summary>
        VK_SHIFT = 0x10,

        /// <summary>CTRL key</summary>
        VK_CONTROL = 0x11,

        /// <summary>ALT key</summary>
        VK_MENU = 0x12,

        /// <summary>PAUSE key</summary>
        VK_PAUSE = 0x13,

        /// <summary>CAPS LOCK key</summary>
        VK_CAPITAL = 0x14,

        /// <summary>IME Kana mode</summary>
        VK_KANA = 0x15,

        /// <summary>IME Hanguel mode (maintained for compatibility; use VK_HANGUL)</summary>
        VK_HANGUEL = 0x15,

        /// <summary>IME Hangul mode</summary>
        VK_HANGUL = 0x15,

        /// <summary>IME On</summary>
        VK_IME_ON = 0x16,

        /// <summary>IME Junja mode</summary>
        VK_JUNJA = 0x17,

        /// <summary>IME final mode</summary>
        VK_FINAL = 0x18,

        /// <summary>IME Hanja mode</summary>
        VK_HANJA = 0x19,

        /// <summary>IME Kanji mode</summary>
        VK_KANJI = 0x19,

        /// <summary>IME Off</summary>
        VK_IME_OFF = 0x1A,

        /// <summary>ESC key</summary>
        VK_ESCAPE = 0x1B,

        /// <summary>IME convert</summary>
        VK_CONVERT = 0x1C,

        /// <summary>IME nonconvert</summary>
        VK_NONCONVERT = 0x1D,

        /// <summary>IME accept</summary>
        VK_ACCEPT = 0x1E,

        /// <summary>IME mode change request</summary>
        VK_MODECHANGE = 0x1F,

        /// <summary>SPACEBAR</summary>
        VK_SPACE = 0x20,

        /// <summary>PAGE UP key</summary>
        VK_PRIOR = 0x21,

        /// <summary>PAGE DOWN key</summary>
        VK_NEXT = 0x22,

        /// <summary>END key</summary>
        VK_END = 0x23,

        /// <summary>HOME key</summary>
        VK_HOME = 0x24,

        /// <summary>LEFT ARROW key</summary>
        VK_LEFT = 0x25,

        /// <summary>UP ARROW key</summary>
        VK_UP = 0x26,

        /// <summary>RIGHT ARROW key</summary>
        VK_RIGHT = 0x27,

        /// <summary>DOWN ARROW key</summary>
        VK_DOWN = 0x28,

        /// <summary>SELECT key</summary>
        VK_SELECT = 0x29,

        /// <summary>PRINT key</summary>
        VK_PRINT = 0x2A,

        /// <summary>EXECUTE key</summary>
        VK_EXECUTE = 0x2B,

        /// <summary>PRINT SCREEN key</summary>
        VK_SNAPSHOT = 0x2C,

        /// <summary>INS key</summary>
        VK_INSERT = 0x2D,

        /// <summary>DEL key</summary>
        VK_DELETE = 0x2E,

        /// <summary>HELP key</summary>
        VK_HELP = 0x2F,

        /// <summary>0 key</summary>
        VK_0 = 0x30,

        /// <summary>1 key</summary>
        VK_1 = 0x31,

        /// <summary>2 key</summary>
        VK_2 = 0x32,

        /// <summary>3 key</summary>
        VK_3 = 0x33,

        /// <summary>4 key</summary>
        VK_4 = 0x34,

        /// <summary>5 key</summary>
        VK_5 = 0x35,

        /// <summary>6 key</summary>
        VK_6 = 0x36,

        /// <summary>7 key</summary>
        VK_7 = 0x37,

        /// <summary>8 key</summary>
        VK_8 = 0x38,

        /// <summary>9 key</summary>
        VK_9 = 0x39,

        /// <summary>A key</summary>
        VK_A = 0x41,

        /// <summary>B key</summary>
        VK_B = 0x42,

        /// <summary>C key</summary>
        VK_C = 0x43,

        /// <summary>D key</summary>
        VK_D = 0x44,

        /// <summary>E key</summary>
        VK_E = 0x45,

        /// <summary>F key</summary>
        VK_F = 0x46,

        /// <summary>G key</summary>
        VK_G = 0x47,

        /// <summary>H key</summary>
        VK_H = 0x48,

        /// <summary>I key</summary>
        VK_I = 0x49,

        /// <summary>J key</summary>
        VK_J = 0x4A,

        /// <summary>K key</summary>
        VK_K = 0x4B,

        /// <summary>L key</summary>
        VK_L = 0x4C,

        /// <summary>M key</summary>
        VK_M = 0x4D,

        /// <summary>N key</summary>
        VK_N = 0x4E,

        /// <summary>O key</summary>
        VK_O = 0x4F,

        /// <summary>P key</summary>
        VK_P = 0x50,

        /// <summary>Q key</summary>
        VK_Q = 0x51,

        /// <summary>R key</summary>
        VK_R = 0x52,

        /// <summary>S key</summary>
        VK_S = 0x53,

        /// <summary>T key</summary>
        VK_T = 0x54,

        /// <summary>U key</summary>
        VK_U = 0x55,

        /// <summary>V key</summary>
        VK_V = 0x56,

        /// <summary>W key</summary>
        VK_W = 0x57,

        /// <summary>X key</summary>
        VK_X = 0x58,

        /// <summary>Y key</summary>
        VK_Y = 0x59,

        /// <summary>Z key</summary>
        VK_Z = 0x5A,

        /// <summary>Left Windows key (Natural keyboard)</summary>
        VK_LWIN = 0x5B,

        /// <summary>Right Windows key (Natural keyboard)</summary>
        VK_RWIN = 0x5C,

        /// <summary>Applications key (Natural keyboard)</summary>
        VK_APPS = 0x5D,

        /// <summary>Computer Sleep key</summary>
        VK_SLEEP = 0x5F,

        /// <summary>Numeric keypad 0 key</summary>
        VK_NUMPAD0 = 0x60,

        /// <summary>Numeric keypad 1 key</summary>
        VK_NUMPAD1 = 0x61,

        /// <summary>Numeric keypad 2 key</summary>
        VK_NUMPAD2 = 0x62,

        /// <summary>Numeric keypad 3 key</summary>
        VK_NUMPAD3 = 0x63,

        /// <summary>Numeric keypad 4 key</summary>
        VK_NUMPAD4 = 0x64,

        /// <summary>Numeric keypad 5 key</summary>
        VK_NUMPAD5 = 0x65,

        /// <summary>Numeric keypad 6 key</summary>
        VK_NUMPAD6 = 0x66,

        /// <summary>Numeric keypad 7 key</summary>
        VK_NUMPAD7 = 0x67,

        /// <summary>Numeric keypad 8 key</summary>
        VK_NUMPAD8 = 0x68,

        /// <summary>Numeric keypad 9 key</summary>
        VK_NUMPAD9 = 0x69,

        /// <summary>Multiply key</summary>
        VK_MULTIPLY = 0x6A,

        /// <summary>Add key</summary>
        VK_ADD = 0x6B,

        /// <summary>Separator key</summary>
        VK_SEPARATOR = 0x6C,

        /// <summary>Subtract key</summary>
        VK_SUBTRACT = 0x6D,

        /// <summary>Decimal key</summary>
        VK_DECIMAL = 0x6E,

        /// <summary>Divide key</summary>
        VK_DIVIDE = 0x6F,

        /// <summary>F1 key</summary>
        VK_F1 = 0x70,

        /// <summary>F2 key</summary>
        VK_F2 = 0x71,

        /// <summary>F3 key</summary>
        VK_F3 = 0x72,

        /// <summary>F4 key</summary>
        VK_F4 = 0x73,

        /// <summary>F5 key</summary>
        VK_F5 = 0x74,

        /// <summary>F6 key</summary>
        VK_F6 = 0x75,

        /// <summary>F7 key</summary>
        VK_F7 = 0x76,

        /// <summary>F8 key</summary>
        VK_F8 = 0x77,

        /// <summary>F9 key</summary>
        VK_F9 = 0x78,

        /// <summary>F10 key</summary>
        VK_F10 = 0x79,

        /// <summary>F11 key</summary>
        VK_F11 = 0x7A,

        /// <summary>F12 key</summary>
        VK_F12 = 0x7B,

        /// <summary>F13 key</summary>
        VK_F13 = 0x7C,

        /// <summary>F14 key</summary>
        VK_F14 = 0x7D,

        /// <summary>F15 key</summary>
        VK_F15 = 0x7E,

        /// <summary>F16 key</summary>
        VK_F16 = 0x7F,

        /// <summary>F17 key</summary>
        VK_F17 = 0x80,

        /// <summary>F18 key</summary>
        VK_F18 = 0x81,

        /// <summary>F19 key</summary>
        VK_F19 = 0x82,

        /// <summary>F20 key</summary>
        VK_F20 = 0x83,

        /// <summary>F21 key</summary>
        VK_F21 = 0x84,

        /// <summary>F22 key</summary>
        VK_F22 = 0x85,

        /// <summary>F23 key</summary>
        VK_F23 = 0x86,

        /// <summary>F24 key</summary>
        VK_F24 = 0x87,

        /// <summary>NUM LOCK key</summary>
        VK_NUMLOCK = 0x90,

        /// <summary>SCROLL LOCK key</summary>
        VK_SCROLL = 0x91,

        /// <summary>NEC '=' key on numpad</summary>
        VK_OEM_NEC_EQUAL = 0x92,

        /// <summary>Fujitsu 'Dictionary' key</summary>
        VK_OEM_FJ_JISHO = 0x92,

        /// <summary>Fujitsu 'Unregister word' key</summary>
        VK_OEM_FJ_MASSHOU = 0x93,

        /// <summary>Fujitsu 'Register word' key</summary>
        VK_OEM_FJ_TOUROKU = 0x94,

        /// <summary>Fujitsu 'Left OYAYUBI' key</summary>
        VK_OEM_FJ_LOYA = 0x95,

        /// <summary>Fujitsu 'Right OYAYUBI' key</summary>
        VK_OEM_FJ_ROYA = 0x96,

        /// <summary>Left SHIFT key</summary>
        VK_LSHIFT = 0xA0,

        /// <summary>Right SHIFT key</summary>
        VK_RSHIFT = 0xA1,

        /// <summary>Left CONTROL key</summary>
        VK_LCONTROL = 0xA2,

        /// <summary>Right CONTROL key</summary>
        VK_RCONTROL = 0xA3,

        /// <summary>Left MENU key</summary>
        VK_LMENU = 0xA4,

        /// <summary>Right MENU key</summary>
        VK_RMENU = 0xA5,

        /// <summary>Browser Back key</summary>
        VK_BROWSER_BACK = 0xA6,

        /// <summary>Browser Forward key</summary>
        VK_BROWSER_FORWARD = 0xA7,

        /// <summary>Browser Refresh key</summary>
        VK_BROWSER_REFRESH = 0xA8,

        /// <summary>Browser Stop key</summary>
        VK_BROWSER_STOP = 0xA9,

        /// <summary>Browser Search key</summary>
        VK_BROWSER_SEARCH = 0xAA,

        /// <summary>Browser Favorites key</summary>
        VK_BROWSER_FAVORITES = 0xAB,

        /// <summary>Browser Start and Home key</summary>
        VK_BROWSER_HOME = 0xAC,

        /// <summary>Volume Mute key</summary>
        VK_VOLUME_MUTE = 0xAD,

        /// <summary>Volume Down key</summary>
        VK_VOLUME_DOWN = 0xAE,

        /// <summary>Volume Up key</summary>
        VK_VOLUME_UP = 0xAF,

        /// <summary>Next Track key</summary>
        VK_MEDIA_NEXT_TRACK = 0xB0,

        /// <summary>Previous Track key</summary>
        VK_MEDIA_PREV_TRACK = 0xB1,

        /// <summary>Stop Media key</summary>
        VK_MEDIA_STOP = 0xB2,

        /// <summary>Play/Pause Media key</summary>
        VK_MEDIA_PLAY_PAUSE = 0xB3,

        /// <summary>Start Mail key</summary>
        VK_LAUNCH_MAIL = 0xB4,

        /// <summary>Select Media key</summary>
        VK_LAUNCH_MEDIA_SELECT = 0xB5,

        /// <summary>Start Application 1 key</summary>
        VK_LAUNCH_APP1 = 0xB6,

        /// <summary>Start Application 2 key</summary>
        VK_LAUNCH_APP2 = 0xB7,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the ';:' key///<summary>For any country/region, the '+' key</summary></para>
        /// </summary>
        VK_OEM_1 = 0xBA,

        /// <summary>For any country/region, the '+' key</summary>
        VK_OEM_PLUS = 0xBB,

        /// <summary>For any country/region, the ',' key</summary>
        VK_OEM_COMMA = 0xBC,

        /// <summary>For any country/region, the '-' key</summary>
        VK_OEM_MINUS = 0xBD,

        /// <summary>For any country/region, the '.' key</summary>
        VK_OEM_PERIOD = 0xBE,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the '/?' key</para>
        /// </summary>
        VK_OEM_2 = 0xBF,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the '`~' key</para>
        /// </summary>
        VK_OEM_3 = 0xC0,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_A = 0xC3,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_B = 0xC4,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_X = 0xC5,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_Y = 0xC6,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_SHOULDER = 0xC7,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_SHOULDER = 0xC8,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_TRIGGER = 0xC9,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_TRIGGER = 0xCA,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_DPAD_UP = 0xCB,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_DPAD_DOWN = 0xCC,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_DPAD_LEFT = 0xCD,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_DPAD_RIGHT = 0xCE,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_MENU = 0xCF,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_VIEW = 0xD0,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON = 0xD1,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 0xD2,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_THUMBSTICK_UP = 0xD3,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_THUMBSTICK_DOWN = 0xD4,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT = 0xD5,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_LEFT_THUMBSTICK_LEFT = 0xD6,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_THUMBSTICK_UP = 0xD7,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN = 0xD8,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 0xD9,

        /// <summary>Reserved</summary>
        VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT = 0xDA,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the '[{' key</para>
        /// </summary>
        VK_OEM_4 = 0xDB,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the '\|' key</para>
        /// </summary>
        VK_OEM_5 = 0xDC,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the ']}' key</para>
        /// </summary>
        VK_OEM_6 = 0xDD,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// <para>For the US standard keyboard, the 'single-quote/double-quote' key</para>
        /// </summary>
        VK_OEM_7 = 0xDE,

        /// <summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
        VK_OEM_8 = 0xDF,

        /// <summary>'AX' key on Japanese AX kbd</summary>
        VK_OEM_AX = 0xE1,

        /// <summary>Either the angle bracket key or the backslash key on the RT 102-key keyboard</summary>
        VK_OEM_102 = 0xE2,

        /// <summary>IME PROCESS key</summary>
        VK_PROCESSKEY = 0xE5,

        /// <summary>OEM specific</summary>
        VK_ICO_CLEAR = 0xE6,

        /// <summary>
        /// Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value
        /// used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        /// </summary>
        VK_PACKET = 0xE7,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_RESET = 0xE9,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_JUMP = 0xEA,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_PA1 = 0xEB,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_PA2 = 0xEC,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_PA3 = 0xED,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_WSCTRL = 0xEE,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_CUSEL = 0xEF,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_ATTN = 0xF0,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_FINISH = 0xF1,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_COPY = 0xF2,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_AUTO = 0xF3,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_ENLW = 0xF4,

        /// <summary>Nokia/Ericsson definition</summary>
        VK_OEM_BACKTAB = 0xF5,

        /// <summary>Attn key</summary>
        VK_ATTN = 0xF6,

        /// <summary>CrSel key</summary>
        VK_CRSEL = 0xF7,

        /// <summary>ExSel key</summary>
        VK_EXSEL = 0xF8,

        /// <summary>Erase EOF key</summary>
        VK_EREOF = 0xF9,

        /// <summary>Play key</summary>
        VK_PLAY = 0xFA,

        /// <summary>Zoom key</summary>
        VK_ZOOM = 0xFB,

        /// <summary>Reserved</summary>
        VK_NONAME = 0xFC,

        /// <summary>PA1 key</summary>
        VK_PA1 = 0xFD,

        /// <summary>Clear key</summary>
        VK_OEM_CLEAR = 0xFE,
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    /// </summary>
    public enum VK2
    {
        /// <summary>
        ///  ENTER key
        /// </summary>
        VK_ENTER = User32.VK.VK_RETURN,

        /// <summary>
        ///  The Unassigned code: The Num ENTER key.
        /// </summary>
        VK_NUMPAD_ENTER = 0x0E,
    }
}

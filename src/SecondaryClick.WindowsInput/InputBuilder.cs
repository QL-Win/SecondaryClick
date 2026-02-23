using System.Collections;
using System.WindowsInput.WinApi;

namespace System.WindowsInput;

/// <summary>
/// A helper class for building a list of <see cref="User32.INPUT"/> messages ready to be sent to the native Windows API.
/// </summary>
public class InputBuilder : IEnumerable<User32.INPUT>, IEnumerable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputBuilder"/> class.
    /// </summary>
    public InputBuilder()
    {
        _inputList = [];
    }

    /// <summary>
    /// Returns the list of <see cref="User32.INPUT"/> messages as an array.
    /// </summary>
    /// <returns>The array of <see cref="User32.INPUT"/> messages.</returns>
    public User32.INPUT[] ToArray()
    {
        return [.. _inputList];
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list of <see cref="User32.INPUT"/> messages.
    /// </summary>
    /// <returns>An enumerator for the list of <see cref="User32.INPUT"/> messages.</returns>
    public IEnumerator<User32.INPUT> GetEnumerator()
    {
        return _inputList.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list of <see cref="User32.INPUT"/> messages.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the list of <see cref="User32.INPUT"/> messages.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets the <see cref="User32.INPUT"/> at the specified position.
    /// </summary>
    /// <value>The <see cref="User32.INPUT"/> message at the specified position.</value>
    public User32.INPUT this[int position] => _inputList[position];

    /// <summary>
    /// Determines if the <see cref="User32.VK"/> is an extended key.
    /// </summary>
    /// <param name="keyCode">The key code.</param>
    /// <returns><c>true</c> if the key code is an extended key; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP,
    /// PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key;
    /// the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
    /// </remarks>
    public static bool IsExtendedKey(User32.VK keyCode)
    {
        return
            keyCode == User32.VK.VK_MENU
         || keyCode == User32.VK.VK_LMENU
         || keyCode == User32.VK.VK_RMENU
         || keyCode == User32.VK.VK_CONTROL
         || keyCode == User32.VK.VK_RCONTROL
         || keyCode == User32.VK.VK_INSERT
         || keyCode == User32.VK.VK_DELETE
         || keyCode == User32.VK.VK_HOME
         || keyCode == User32.VK.VK_END
         || keyCode == User32.VK.VK_PRIOR
         || keyCode == User32.VK.VK_NEXT
         || keyCode == User32.VK.VK_RIGHT
         || keyCode == User32.VK.VK_UP
         || keyCode == User32.VK.VK_LEFT
         || keyCode == User32.VK.VK_DOWN
         || keyCode == User32.VK.VK_NUMLOCK
         || keyCode == User32.VK.VK_CANCEL
         || keyCode == User32.VK.VK_SNAPSHOT
         || keyCode == User32.VK.VK_DIVIDE;
    }

    /// <summary>
    /// Adds a key down to the list of <see cref="User32.INPUT"/> messages.
    /// </summary>
    /// <param name="keyCode">The <see cref="User32.VK"/>.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddKeyDown(User32.VK keyCode, bool? isExtendedKey = null)
    {
        bool isUseExtendedKey = isExtendedKey == null ? IsExtendedKey(keyCode) : isExtendedKey.Value;

        if ((User32.VK2)keyCode == User32.VK2.VK_NUMPAD_ENTER)
        {
            keyCode = User32.VK.VK_RETURN;
            isUseExtendedKey = true;
        }

        User32.INPUT input = new()
        {
            type = User32.INPUTTYPE.INPUT_KEYBOARD,
            ki = new User32.KEYBDINPUT()
            {
                wVk = (ushort)keyCode,
                wScan = (ushort)(User32.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                dwFlags = isUseExtendedKey ? User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY : 0,
                time = 0,
                dwExtraInfo = IntPtr.Zero,
            },
        };
        User32.INPUT item = input;
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a key up to the list of <see cref="User32.INPUT"/> messages.
    /// </summary>
    /// <param name="keyCode">The <see cref="User32.VK"/>.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddKeyUp(User32.VK keyCode, bool? isExtendedKey = null)
    {
        bool isUseExtendedKey = isExtendedKey == null ? IsExtendedKey(keyCode) : isExtendedKey.Value;

        if ((User32.VK2)keyCode == User32.VK2.VK_NUMPAD_ENTER)
        {
            keyCode = User32.VK.VK_RETURN;
            isUseExtendedKey = true;
        }

        User32.INPUT input = new()
        {
            type = User32.INPUTTYPE.INPUT_KEYBOARD,
            ki = new User32.KEYBDINPUT()
            {
                wVk = (ushort)keyCode,
                wScan = (ushort)(User32.MapVirtualKey((uint)keyCode, 0) & 0xFFU),
                dwFlags = (isUseExtendedKey ? User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY : 0) | User32.KEYEVENTF.KEYEVENTF_KEYUP,
                time = 0,
                dwExtraInfo = IntPtr.Zero,
            },
        };
        User32.INPUT item = input;
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a key press to the list of <see cref="User32.INPUT"/> messages which is equivalent to a key down followed by a key up.
    /// </summary>
    /// <param name="keyCode">The <see cref="User32.VK"/>.</param>
    /// <param name="isExtendedKey">Whether the key is extended.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddKeyPress(User32.VK keyCode, bool? isExtendedKey = null)
    {
        AddKeyDown(keyCode, isExtendedKey);
        AddKeyUp(keyCode, isExtendedKey);
        return this;
    }

    /// <summary>
    /// Adds the character to the list of <see cref="User32.INPUT"/> messages.
    /// </summary>
    /// <param name="character">The character to be added.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddCharacter(char character)
    {
        User32.INPUT input = new()
        {
            type = User32.INPUTTYPE.INPUT_KEYBOARD,
            ki = new User32.KEYBDINPUT()
            {
                wVk = 0,
                wScan = character,
                dwFlags = User32.KEYEVENTF.KEYEVENTF_UNICODE,
                time = 0,
                dwExtraInfo = IntPtr.Zero,
            },
        };
        User32.INPUT item = input;
        User32.INPUT input2 = new()
        {
            type = User32.INPUTTYPE.INPUT_KEYBOARD,
            ki = new User32.KEYBDINPUT()
            {
                wVk = 0,
                wScan = character,
                dwFlags = User32.KEYEVENTF.KEYEVENTF_KEYUP | User32.KEYEVENTF.KEYEVENTF_UNICODE,
                time = 0,
                dwExtraInfo = IntPtr.Zero,
            },
        };
        User32.INPUT item2 = input2;
        if ((character & '\u1234') == '\u1234')
        {
            item.ki = new User32.KEYBDINPUT()
            {
                wVk = item.ki.wVk,
                wScan = item.ki.wScan,
                dwFlags = item.ki.dwFlags | User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY,
                time = item.ki.time,
                dwExtraInfo = item.ki.dwExtraInfo,
            };
            item2.ki = new User32.KEYBDINPUT()
            {
                wVk = item2.ki.wVk,
                wScan = item2.ki.wScan,
                dwFlags = item2.ki.dwFlags | User32.KEYEVENTF.KEYEVENTF_EXTENDEDKEY,
                time = item2.ki.time,
                dwExtraInfo = item2.ki.dwExtraInfo,
            };
        }
        _inputList.Add(item);
        _inputList.Add(item2);
        return this;
    }

    /// <summary>
    /// Adds all of the characters in the specified sequence.
    /// </summary>
    /// <param name="characters">The characters to add.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddCharacters(IEnumerable<char> characters)
    {
        foreach (char character in characters)
        {
            AddCharacter(character);
        }
        return this;
    }

    /// <summary>
    /// Adds the characters in the specified string.
    /// </summary>
    /// <param name="characters">The string of characters to add.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddCharacters(string characters)
    {
        return AddCharacters(characters.ToCharArray());
    }

    /// <summary>
    /// Moves the mouse relative to its current position.
    /// </summary>
    /// <param name="x">The relative x movement.</param>
    /// <param name="y">The relative y movement.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddRelativeMouseMovement(int x, int y)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dx = x,
                dy = y,
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Moves the mouse to an absolute position.
    /// </summary>
    /// <param name="absoluteX">The absolute x position.</param>
    /// <param name="absoluteY">The absolute y position.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dx = absoluteX,
                dy = absoluteY,
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE | User32.MOUSEEVENTF.MOUSEEVENTF_ABSOLUTE,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Moves the mouse to the absolute position on the virtual desktop.
    /// </summary>
    /// <param name="absoluteX">The absolute x position on the virtual desktop.</param>
    /// <param name="absoluteY">The absolute y position on the virtual desktop.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dx = absoluteX,
                dy = absoluteY,
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE | User32.MOUSEEVENTF.MOUSEEVENTF_ABSOLUTE | User32.MOUSEEVENTF.MOUSEEVENTF_VIRTUALDESK,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a mouse button down for the specified button.
    /// </summary>
    /// <param name="button">The mouse button.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseButtonDown(MouseButton button)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = ToMouseButtonDownFlag(button),
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a mouse X button down for the specified button.
    /// </summary>
    /// <param name="xButtonId">The X button id.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseXButtonDown(int xButtonId)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_XDOWN,
                mouseData = xButtonId,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a mouse button up for the specified button.
    /// </summary>
    /// <param name="button">The mouse button.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseButtonUp(MouseButton button)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = ToMouseButtonUpFlag(button),
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a mouse X button up for the specified button.
    /// </summary>
    /// <param name="xButtonId">The X button id.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseXButtonUp(int xButtonId)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_XUP,
                mouseData = xButtonId,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a single click of the specified mouse button.
    /// </summary>
    /// <param name="button">The mouse button.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseButtonClick(MouseButton button)
    {
        return AddMouseButtonDown(button).AddMouseButtonUp(button);
    }

    /// <summary>
    /// Adds a single click of the specified X mouse button.
    /// </summary>
    /// <param name="xButtonId">The X button id.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseXButtonClick(int xButtonId)
    {
        return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
    }

    /// <summary>
    /// Adds a double click of the specified mouse button.
    /// </summary>
    /// <param name="button">The mouse button.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
    {
        return AddMouseButtonClick(button).AddMouseButtonClick(button);
    }

    /// <summary>
    /// Adds a double click of the specified X mouse button.
    /// </summary>
    /// <param name="xButtonId">The X button id.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
    {
        return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
    }

    /// <summary>
    /// Scrolls the vertical mouse wheel by the specified amount.
    /// </summary>
    /// <param name="scrollAmount">The scroll amount.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_WHEEL,
                mouseData = scrollAmount,
            },
        };
        _inputList.Add(item);
        return this;
    }

    /// <summary>
    /// Scrolls the horizontal mouse wheel by the specified amount.
    /// </summary>
    /// <param name="scrollAmount">The scroll amount.</param>
    /// <returns>This <see cref="InputBuilder"/> instance.</returns>
    public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
    {
        User32.INPUT item = new()
        {
            type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_HWHEEL,
                mouseData = scrollAmount,
            },
        };
        _inputList.Add(item);
        return this;
    }

    private static User32.MOUSEEVENTF ToMouseButtonDownFlag(MouseButton button)
    {
        return button switch
        {
            MouseButton.LeftButton => User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN,
            MouseButton.MiddleButton => User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN,
            MouseButton.RightButton => User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN,
            _ => User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN,
        };
    }

    private static User32.MOUSEEVENTF ToMouseButtonUpFlag(MouseButton button)
    {
        return button switch
        {
            MouseButton.LeftButton => User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP,
            MouseButton.MiddleButton => User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP,
            MouseButton.RightButton => User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP,
            _ => User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP,
        };
    }

    private readonly List<User32.INPUT> _inputList;
}

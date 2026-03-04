![logo64](https://raw.githubusercontent.com/QL-Win/SecondaryClick/refs/heads/master/branding/Logo64.png)

[![GitHub license](https://img.shields.io/github/license/QL-Win/SecondaryClick)](https://github.com/QL-Win/SecondaryClick/blob/master/LICENSE-GPL.txt) [![Actions](https://github.com/QL-Win/SecondaryClick/actions/workflows/build.yml/badge.svg)](https://github.com/QL-Win/SecondaryClick/actions/workflows/build.yml) [![Platform](https://img.shields.io/badge/platform-Windows-blue?logo=windowsxp&color=1E9BFA)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)

# SecondaryClick

Bring macOS “Secondary Click” gesture to Windows.

SecondaryClick lets you perform right-click actions from touchpad gestures and optional keyboard modifiers.

## Features

- Secondary click support for touchpads
- Two-finger tap
- Bottom-right click zone
- Optional keyboard modifier triggers (`Alt`, `Control`, `Shift`)
- Lightweight system tray app

## Requirements

- Windows with a touchpad
- .NET Framework 4.8 runtime
- Precision touchpad is recommended for full touchpad options

## Install

Download the latest build from [Releases](https://github.com/QL-Win/SecondaryClick/releases).

## Usage

After launch, SecondaryClick runs in the system tray.

- Right-click the tray icon to open settings
- Enable or disable touchpad gestures:
	- Two-finger tap
	- Bottom-right click
- Enable keyboard-assisted triggers:
	- Off
	- Alt Key
	- Control Key
	- Shift Key
- Optional: enable Start with Windows from tray menu
- Optional: hide tray icon from tray menu (`Hide tray icon`)

If tray icon is hidden, run `RestoreTrayIcon.bat` from the app output folder to restore it.

Settings are saved per user and restored automatically on next launch.

## License

<img src="https://www.gnu.org/graphics/gplv3-127x51.png" alt="GPL v3">

`SecondaryClick` is free and open source software under [GPL-3.0](https://opensource.org/licenses/GPL-3.0).

`SecondaryClick.MouseKeyHook` is based on [MouseKeyHook](https://github.com/gmamaladze/globalmousekeyhook) and distributed under [MIT](https://github.com/gmamaladze/globalmousekeyhook#MIT-1-ov-file).

`SecondaryClick.WindowsInput` is based on [InputSimulator](https://github.com/michaelnoonan/inputsimulator) and distributed under [MIT](https://github.com/michaelnoonan/inputsimulator?tab=MIT-1-ov-file#readme).

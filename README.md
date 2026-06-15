<p align="center">
  <img src="https://raw.githubusercontent.com/QL-Win/SecondaryClick/refs/heads/master/branding/Logo64.png" alt="SecondaryClick Logo" width="96" height="96">
</p>

<h1 align="center">🖱️ SecondaryClick</h1>

<p align="center">
  Bring macOS <strong>Secondary Click</strong> gestures to Windows.
</p>

<p align="center">
  <img src="https://img.shields.io/github/license/QL-Win/SecondaryClick?style=for-the-badge" alt="License">
  <img src="https://img.shields.io/github/actions/workflow/status/QL-Win/SecondaryClick/build.yml?style=for-the-badge&label=Build" alt="Build Status">
  <img src="https://img.shields.io/badge/Platform-Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white" alt="Platform">
</p>

---

SecondaryClick lets you perform right-click actions using touchpad gestures and optional keyboard modifiers.

## ✨ Features

* 🖱️ Secondary click support for touchpads
* 👆👆 Two-finger tap
* 📍 Bottom-right click zone
* ⌨️ Optional keyboard modifier triggers (`Alt`, `Control`, `Shift`)
* 🚀 Lightweight system tray app

## 📋 Requirements

* 💻 Windows with a touchpad
* ⚙️ .NET Framework 4.8 runtime
* 🎯 Precision touchpad recommended for full touchpad functionality

## 📦 Install

Download the latest build from **[Releases](https://github.com/QL-Win/SecondaryClick/releases)**.

## 🚀 Usage

After launch, SecondaryClick runs in the system tray.

### 👆 Touchpad Gestures

Enable or disable:

* 👆👆 Two-finger tap
* 📍 Bottom-right click

### ⌨️ Keyboard-Assisted Triggers

Choose one of:

*  Off
*  `Alt` Key
*  `Control` Key
*  `Shift` Key

### ⚙️ Additional Options

* 🖱️ Right-click the tray icon to open settings
* 🔄 Enable **Start with Windows** from the tray menu
* 👻 Hide the tray icon using **Hide tray icon**

If the tray icon is hidden, run:

```bat
RestoreTrayIcon.bat
```

from the application output folder to restore it.

💾 Settings are saved per user and automatically restored on the next launch.

## 🎨 Designer

Application icon designed by **[@Shomnipotence](https://github.com/Shomnipotence)**.

## 📜 License

<p align="center">
  <img src="https://www.gnu.org/graphics/gplv3-127x51.png" alt="GPL v3">
</p>

🔓 `SecondaryClick` is free and open-source software licensed under **GPL-3.0**.

🔧 `SecondaryClick.MouseKeyHook` is based on **MouseKeyHook** and distributed under the **MIT License**.

🔧 `SecondaryClick.WindowsInput` is based on **InputSimulator** and distributed under the **MIT License**.

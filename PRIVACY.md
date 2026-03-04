# Privacy Policy for SecondaryClick

Last updated: 2026-03-05

This Privacy Policy explains how **SecondaryClick** handles data when you use the app distributed through Microsoft Store.

## Summary

- SecondaryClick is a local desktop utility for touchpad/keyboard gesture behavior.
- SecondaryClick does **not** require account sign-in.
- SecondaryClick does **not** upload your personal data to our servers.
- SecondaryClick stores only required app settings on your device (Windows Registry, current user scope).

## Data We Process

### 1) Local configuration data (stored on your PC)
SecondaryClick stores feature preferences in the current user registry:

- `HKEY_CURRENT_USER\\Software\\SecondaryClick`
  - Modifier settings (Alt/Control/Shift)
  - Tray icon visibility setting

If you enable "Start with Windows", Windows startup entry is written to:

- `HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run` (value name: `SecondaryClick`)

For touchpad feature state display and toggling, the app may read/write Windows touchpad settings under:

- `HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\PrecisionTouchPad`

### 2) Runtime input handling (not persisted)
To provide its functionality, SecondaryClick may read keyboard modifier state and touchpad-related system settings during runtime.

- This runtime handling is used only to execute gesture logic.
- SecondaryClick does **not** store typed content, passwords, or clipboard content as part of normal functionality.

## Data We Do Not Collect

SecondaryClick does not intentionally collect or transmit:

- Name, email, phone number, address
- Account credentials
- Files, photos, or document content
- Precise location
- Advertising identifiers

## Network and Telemetry

- SecondaryClick itself is designed to run locally and does not provide a built-in cloud account system.
- We do not operate analytics or telemetry endpoints for this app.
- If you install/update through Microsoft Store, Microsoft may process diagnostic or commerce data under Microsoft’s own policies.

## Purpose of Processing

Local data is processed only for:

- Enabling/disabling gesture features
- Remembering your preferences between app launches
- Optional startup behavior when you choose "Start with Windows"

## Legal Basis (where applicable)

Where data protection law applies, processing is based on:

- Legitimate interest in providing requested app functionality, and/or
- Your choice and control over optional settings (such as startup behavior)

## Retention

- Local settings remain on your device until changed or removed.
- Uninstalling the app may not always remove all registry values automatically.
- You can remove app settings manually, or use provided cleanup script(s) such as `ClearRegistry.bat` where available.

## Your Choices and Controls

You can at any time:

- Disable optional features from the tray menu
- Disable "Start with Windows"
- Remove local settings from registry
- Uninstall SecondaryClick from Windows

## Data Sharing

SecondaryClick does not share your local settings with us because the app does not transmit them to our servers.

## Children’s Privacy

SecondaryClick is not directed to children under 13, and we do not knowingly collect children’s personal information.

## Security

SecondaryClick follows a local-processing design to minimize data exposure. However, no software can guarantee absolute security on every device configuration.

## Changes to This Policy

We may update this policy when app behavior changes. The "Last updated" date above indicates the latest revision.

## Contact

If you have privacy questions, please contact the publisher via:

- GitHub Issues: https://github.com/QL-Win/SecondaryClick/issues
- Project page: https://github.com/QL-Win/SecondaryClick

@echo off
reg add "HKCU\Software\SecondaryClick" /v HideTrayIcon /t REG_DWORD /d 0 /f
echo Tray icon restore value has been written.

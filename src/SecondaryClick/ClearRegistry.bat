@echo off
reg delete "HKCU\Software\SecondaryClick" /f >nul 2>&1
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v SecondaryClick /f >nul 2>&1
echo SecondaryClick registry entries have been cleared.

Write-Host @"
 ██████╗ ██╗      ██╗    ██╗██╗███╗   ██╗
██╔═══██╗██║      ██║    ██║██║████╗  ██║
██║   ██║██║      ██║ █╗ ██║██║██╔██╗ ██║
██║▄▄ ██║██║      ██║███╗██║██║██║╚██╗██║
╚██████╔╝███████╗ ╚███╔███╔╝██║██║ ╚████║
 ╚══▀▀═╝ ╚══════╝  ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝
"@

Write-Host ("Build started {0:yyyy.MM.dd HH:mm:ss}" -f (Get-Date))

$scriptRoot = $PSScriptRoot
$version = git describe --always --tags --exclude latest
$globalPackages = (dotnet nuget locals global-packages --list) -split ":\s*", 2 | Select-Object -Last 1

$sevenZip = Join-Path $globalPackages "micasetup.tools\2.5.0\build\bin\7z.exe"
$makemicaPath = Join-Path $globalPackages "micasetup.tools\2.5.0\build\makemica.exe"

Set-Location $scriptRoot\..\
dotnet restore
dotnet build -c Release

if (-not (Test-Path $sevenZip)) {
    throw "7z.exe file not found: $sevenZip"
}
if (-not (Test-Path $makemicaPath)) {
    throw "makemica.exe file not found: $makemicaPath"
}

Set-Location $scriptRoot
Remove-Item .\Package.7z -ErrorAction SilentlyContinue
& $sevenZip a Package.7z $scriptRoot\..\src\SecondaryClick\bin\Release\net48\* -t7z -mx=9 -ms=on -m0=lzma2 -mf=BCJ2 -r -y
& $makemicaPath micasetup.json

Compress-Archive $scriptRoot\..\src\SecondaryClick\bin\Release\net48\* SecondaryClick-$version.zip
Rename-Item .\SecondaryClick.exe SecondaryClick-$version.exe
Rename-Item .\Package.7z SecondaryClick-$version.7z

Write-Host "`nPress any key to exit..."
[void][System.Console]::ReadKey($true)
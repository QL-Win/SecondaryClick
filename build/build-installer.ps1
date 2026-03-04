[CmdletBinding()]
param(
	[ValidateSet("Debug", "Release")]
	[string]$Configuration = "Release",

	[ValidateSet("x86")]
	[string]$Platform = "x86"
)

$ErrorActionPreference = "Stop"

Write-Host ("Installer build started {0:yyyy.MM.dd HH:mm:ss}" -f (Get-Date))

$scriptRoot = $PSScriptRoot
$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $scriptRoot ".."))
$appProject = Join-Path $repoRoot "src\SecondaryClick\SecondaryClick.csproj"
$installerProject = Join-Path $repoRoot "src\SecondaryClick.Installer\SecondaryClick.Installer.wixproj"

$msbuildExe = $null
$vsWhereCmd = Get-Command vswhere.exe -ErrorAction SilentlyContinue
if (-not $vsWhereCmd) {
	throw "vswhere.exe not found in PATH. Please install Visual Studio/Build Tools and ensure vswhere is available."
}

$msbuildFromVsWhere = & $vsWhereCmd.Source -latest -products * -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
if (-not [string]::IsNullOrWhiteSpace($msbuildFromVsWhere) -and (Test-Path $msbuildFromVsWhere)) {
	$msbuildExe = $msbuildFromVsWhere
}

if (-not $msbuildExe) {
	throw "MSBuild.exe not found via vswhere. Please install Visual Studio Build Tools with MSBuild component."
}

if ([string]::IsNullOrWhiteSpace($env:WIX)) {
	throw "WIX environment variable is not set. This project's PreBuildEvent uses $(WIX)bin\heat."
}

if (-not (Test-Path $appProject)) {
	throw "App project not found: $appProject"
}

if (-not (Test-Path $installerProject)) {
	throw "Installer project not found: $installerProject"
}

Set-Location $repoRoot

dotnet restore $appProject
if ($LASTEXITCODE -ne 0) {
	throw "dotnet restore failed for: $appProject"
}

& $msbuildExe $installerProject /t:Build /p:Configuration=$Configuration /p:Platform=$Platform /nologo
if ($LASTEXITCODE -ne 0) {
	throw "Installer build failed: $installerProject"
}

$buildDir = Join-Path $repoRoot "build"
$msiPath = Join-Path $buildDir "SecondaryClick.msi"
$versionedMsi = Get-ChildItem $buildDir -Filter "SecondaryClick-*.msi" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ((-not (Test-Path $msiPath)) -and (-not $versionedMsi)) {
	Write-Warning "Build finished but no MSI found in: $buildDir"
}

Write-Host "Installer build completed."

$version = git -C "$PSScriptRoot\.." describe --always --tags --exclude latest

if (Test-Path "$PSScriptRoot\SecondaryClick.msi") {
	Remove-Item "$PSScriptRoot\SecondaryClick-$version.msi" -ErrorAction SilentlyContinue
	Rename-Item "$PSScriptRoot\SecondaryClick.msi" "SecondaryClick-$version.msi"
}
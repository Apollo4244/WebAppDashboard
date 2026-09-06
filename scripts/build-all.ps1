# WebAppDashboard - Lokale Publish-Bau-Schleife.
# Baut alle Varianten (Kernprojekt + variants/*) als Single-File-EXE nach publish/<BrandId>/.
#
# Verwendung:
#   pwsh .\scripts\build-all.ps1
# oder klassisch:
#   powershell -ExecutionPolicy Bypass -File .\scripts\build-all.ps1

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

$projects = @(
    "WebAppDashboard\WebAppDashboard.csproj"
) + (Get-ChildItem -Path "variants" -Filter *.csproj -Recurse | ForEach-Object { $_.FullName })

$outRoot = Join-Path $root "publish"

foreach ($csproj in $projects) {
    $name = [System.IO.Path]::GetFileNameWithoutExtension($csproj)
    $out = Join-Path $outRoot $name
    Write-Host "==> Publishing $name  ->  $out" -ForegroundColor Cyan
    dotnet publish $csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o $out
    if ($LASTEXITCODE -ne 0) { throw "Publish failed for $csproj" }
}

Write-Host ""
Write-Host "Fertig. Release-EXEs:" -ForegroundColor Green
Get-ChildItem -Path $outRoot -Filter *.exe -Recurse | ForEach-Object { "  " + $_.FullName }
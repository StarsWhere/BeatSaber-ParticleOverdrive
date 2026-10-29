[CmdletBinding()]
param([Parameter(Mandatory = $true)][string]$GameRoot)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$game = (Resolve-Path -LiteralPath $GameRoot).Path
if (-not (Test-Path -LiteralPath (Join-Path $game 'Beat Saber_Data/Managed/Main.dll'))) {
    throw "Game assemblies not found under $game"
}
$obj = Join-Path $root '.tmp/obj/'
$out = Join-Path $root '.tmp/out/'
dotnet build (Join-Path $root 'ParticleOverdrive/ParticleOverdrive.csproj') -c Release `
    "-p:BeatSaberDir=$game" "-p:BaseIntermediateOutputPath=$obj" "-p:OutputPath=$out" `
    '-p:DisableCopyToPlugins=True' '-p:DisableZipRelease=True'
if ($LASTEXITCODE -ne 0) { throw "Build failed ($LASTEXITCODE)" }

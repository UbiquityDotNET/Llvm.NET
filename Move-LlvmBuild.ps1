$plat = "win32"
$basePath = Join-Path $PSScriptRoot llvm

# Copy from cached headers.
if (!(Test-Path (Join-Path $basePath include))) {
    Write-Information "Unzipping include files..."
    Expand-Archive -LiteralPath (Join-path $basePath xplat $plat include.zip) -DestinationPath $basePath -Force
}

# Copy from cached binaries.
$libDir = Join-Path $basePath lib
if (!(Test-Path $libDir)) {
    New-Item -ItemType Directory $libDir
    foreach ($zip in (Get-ChildItem (Join-Path $basePath xplat $plat) -Recurse -Include "lib.*.zip")) {
        Write-Information "Unzipping $($zip.fullname)..."
        Expand-Archive -LiteralPath $zip.fullname -DestinationPath $libDir -Force
    }
    Copy-Item -Force -Recurse (Join-Path $basePath xplat ExecutionEngine) $libDir -Verbose
}
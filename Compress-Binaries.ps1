[CmdletBinding()]
param (
    [Parameter()]
    [String]
    $SourcePath
)
$baseName = Split-Path -leaf (Resolve-Path $SourcePath)
$files = Get-ChildItem $SourcePath -Recurse
$count = 1
foreach ($file in $files) {
    if ($file.length -gt 400mb) {
        $count += 1
    }
    $zip = Compress-Archive -Update -LiteralPath $file.fullname -DestinationPath (Join-Path . "$baseName.$count.zip") -CompressionLevel Optimal -Passthru
    Write-Verbose "File $($file.name): $($file.length) -> Zip-$count : $($zip.length)"
    if ($zip.length -gt 50mb) {
        $count += 1
    }
}

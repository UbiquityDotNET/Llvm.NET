<#
    This helper script will compress a folder into numbered zip files with the goal of keeping
    each file to around 50mb if possible (the recommended file size limit for GitHub). It does NOT
    split up a single uncompressed file across multiple zip files in order to make decompression
    easier later, which limits its effectiveness on large files.
#>

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

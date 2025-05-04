function Expand-ArchiveStream([Parameter(Mandatory=$true, ValueFromPipeLine)]$src, [Parameter(Mandatory=$true)]$OutputPath)
{
<#
.SYNOPSIS
    Expands an archive stream

.PARAMETER src
    Input stream containing compressed ZIP archive data to expand

.PARAMETER OutputPath
    Out put destination for the decompressed data
#>
    $zipArchive = [System.IO.Compression.ZipArchive]::new($src)
    [System.IO.Compression.ZipFileExtensions]::ExtractToDirectory( $zipArchive, $OutputPath)
}

function Expand-StreamFromUri([Parameter(Mandatory=$true, ValueFromPipeLine)]$uri, [Parameter(Mandatory=$true)]$OutputPath)
{
<#
.SYNOPSIS
    Downloads and expands a ZIP file to the specified destination

.PARAMETER uri
    URI of the ZIP file to download and expand

.PARAMETER OutputPath
    Output folder to expand the ZIP contents into
#>
    $strm = (Invoke-WebRequest -UseBasicParsing -Uri $uri).RawContentStream
    Expand-ArchiveStream $strm $OutputPath
}


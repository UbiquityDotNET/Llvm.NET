$pkgs = Get-ChildItem BuildOutput\Nuget\**\*.nupkg
$srcDir = $env:BUILD_SOURCESDIRECTORY
if( [String]::IsNullOrWhiteSpace( $srcDir ) )
{
    $srcDir = (Get-Location).Path
}

$relatavieRoot = [System.IO.Path]::Combine( $srcDir, "BuildOutput\Nuget")
"relativeRoot: " + $relatavieRoot
Foreach( $pkg in $pkgs )
{ 
    $targetFolder = $pkg.DirectoryName.Replace( $relatavieRoot,"c:\Nuget")
    if( ![System.IO.Directory]::Exists( $targetFolder ) )
    {
        [System.IO.Directory]::CreateDirectory( $targetFolder )
    }
    $targetFile = [System.IO.Path]::Combine($targetFolder, $pkg.Name )
    $pkg.CopyTo( $targetFile, $true )
}
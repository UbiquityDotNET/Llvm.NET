<#
The header file intrin0.h in MSVC has a bug in it that prevents CppSharp from correctly parsing the file.
The fix is present in Visual Studio 2019 16.9 Preview 2, but has not be backported, and preview versions
are not present in pipeline images. To work around this, we have to edit the file to avoid the bad path.
See below for relevant links:

Microsoft STL Issue and fix: https://github.com/microsoft/STL/issues/1300
CppSharp workaround: https://github.com/mono/CppSharp/pull/1514
#>

$intrin = 'C:/Program Files (x86)/Microsoft Visual Studio/2019/Enterprise/VC/Tools/MSVC/14.28.29333/include/intrin0.h'
if (!(Test-Path $intrin)) {
  Write-Warning "This hack is no longer needed and should be removed."
}
else {
    $content = ((Get-Content $intrin) -replace 'ifdef __clang__', 'ifdef __avoid_this_path__')
    [IO.File]::WriteAllLines($intrin, $content)
}

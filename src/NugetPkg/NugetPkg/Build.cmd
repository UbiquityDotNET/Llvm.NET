@rem args: %1=build|rebuild|clean %2=$(Configuration)
setlocal
set OUTPUT_DIR=..\..\..\BuildOutput\Nuget\%2
set NUPKG=Llvm.NET.3.6.2.nupkg
set clean=

if /i '%1'=='rebuild' set clean=true
if /i '%1'=='clean' set clean=true
if DEFINED clean del /q %OUTPUT_DIR%\%NUPKG%
if '%1'=='clean' goto :return

if NOT EXIST %OUTPUT_DIR% md %OUTPUT_DIR%
packages\NuGet.CommandLine.2.8.6\tools\nuget pack Llvm.NET.3.6.1.nuspec -OutputDirectory %OUTPUT_DIR% -Properties configuration=%2

:return
endlocal

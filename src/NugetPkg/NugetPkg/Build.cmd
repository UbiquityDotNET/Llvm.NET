@rem args: %1=build|rebuild|clean %2=$(Configuration)
setlocal
set BUILDOUTPUT=..\..\..\BuildOutput
set OUTPUT_DIR=%BuildOutput%\Nuget\%2
set clean=

REM Don't delete old versions, otherwise clients won't be able to perform an update
REM if they don't have the package installed (e.g. get from version control while
REM it references and old version, then try to update!) Since this outputs to the
REM local gallery that would cause problems.
REM if /i '%1'=='rebuild' set clean=true
REM if /i '%1'=='clean' set clean=true
if DEFINED clean del /q %OUTPUT_DIR%\Llvm.NET.*.nupkg
if '%1'=='clean' goto :return

if NOT EXIST %OUTPUT_DIR% md %OUTPUT_DIR%
packages\NuGet.CommandLine.2.8.6\tools\nuget pack Llvm.NET.nuspec -OutputDirectory %OUTPUT_DIR% -Properties configuration=%2;buildoutput=%BUILDOUTPUT% -NoPackageAnalysis

:return
endlocal

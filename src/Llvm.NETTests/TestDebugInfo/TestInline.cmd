llvm-as -verify-debug-info NeverInlined.ll
if %ERRORLEVEL% NEQ 0 goto :EOF
llvm-dis -o=neverInlined-dis.ll NeverInlined.bc
if %ERRORLEVEL% NEQ 0 goto :EOF
llc.exe -filetype=obj NeverInlined.bc
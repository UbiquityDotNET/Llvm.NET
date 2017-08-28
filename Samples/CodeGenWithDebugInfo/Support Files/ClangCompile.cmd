@echo off

@REM -- Generate bitcode for x86 from test.c
Call :GenerateCode x86_64-pc-windows-msvc18.0.0 test.c test_x86

@REM -- Generate bitcode for Cortex-M3 from test.c
Call :GenerateCode thumbv7m-none-eabi test.c test_M3

goto :EOF

@REM - %1 = Triple (i.e. x86_64-pc-windows-msvc18.0.0,thumbv7m-none-eabi)
@REM - %2 = Source File (C/C++)
@REM - %3 = Output files base name
:GenerateCode
@echo Compiling '%2' for %1
clang -c -g -emit-llvm --target=%1 %2
if EXIST %3.bc del %3.bc
ren %~n2.bc %3.bc
llvm-dis %3.bc
opt -O3 %3.bc -o %3_opt.bc
llvm-dis %3_opt.bc
llc -filetype=asm -asm-show-inst %3_opt.bc
goto :EOF

@REM -- Generate bitcode for x86 from test.c
clang -c -g -emit-llvm --target=x86_64-pc-windows-msvc18.0.0 test.c
if EXIST test_x86.bc del test_x86.bc
ren test.bc test_x86.bc
llvm-dis test_x86.bc
opt -O3 test_x86.bc -o test_x86_opt.bc
llvm-dis test_x86_opt.bc

@REM -- Generate bitcode for Cortex-M3 from test.c
clang -c -g -emit-llvm --target=thumbv7m-none-eabi test.c
if EXIST test_M3.bc del test_M3.bc
ren test.bc test_M3.bc
llvm-dis test_M3.bc
opt -O3 test_M3.bc -o test_M3_opt.bc
llvm-dis test_M3_opt.bc
llc -filetype=asm -asm-show-inst test_M3_opt.bc

@REM -- Generate bitcode for x86 from test2.cpp
clang -c -g -emit-llvm --target=x86_64-pc-windows-msvc18.0.0 test2.cpp
if EXIST test2_x86.bc del test2_x86.bc
ren test2.bc test2_x86.bc
llvm-dis test2_x86.bc
opt -O3 test2_x86.bc -o test2_x86_opt.bc
llvm-dis test2_x86_opt.bc

@REM -- Generate bitcode for Cortex-M3 from test2.cpp
clang -c -g -emit-llvm --target=thumbv7m-none-eabi test2.cpp
if EXIST test2_M3.bc del test2_M3.bc
ren test2.bc test2_M3.bc
llvm-dis test2_M3.bc
opt -O3 test2_M3.bc -o test2_M3_opt.bc
llvm-dis test2_M3_opt.bc
llc -filetype=asm -asm-show-inst test2_M3_opt.bc

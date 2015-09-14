clang -c -g -emit-llvm test.c
if EXIST test_x86.bc del test_x86.bc
ren test.bc test_x86.bc
llvm-dis test_x86.bc
opt -O3 test_x86.bc -o test_x86_opt.bc
llvm-dis test_x86_opt.bc

clang -c -g -emit-llvm --target=thumbv7m-none-eabi test.c
if EXIST test_M3.bc del test_M3.bc
ren test.bc test_M3.bc
llvm-dis test_M3.bc
opt -O3 test_M3.bc -o test_M3_opt.bc
llvm-dis test_M3_opt.bc
llc -filetype=asm -asm-show-inst test_M3_opt.bc
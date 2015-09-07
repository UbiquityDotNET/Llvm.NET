clang -c -g -emit-llvm test.c
llvm-dis test.bc
opt -O3 test.bc -o test_opt.bc
llvm-dis test_opt.bc
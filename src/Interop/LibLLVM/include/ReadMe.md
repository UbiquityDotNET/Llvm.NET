# Header File Layout
Some headers are only intended for consumption by the LibLLVM C++ code, others are intended as 
declarations of the exported LibLLVM-C API. The exported headers **MUST** go into the libllvm-c
folder, as the tooling to generate the interop code only looks in that directory. Any other headers
not intended for interop biding/projection belong in the same folder as this ReadMe

# Header File Layout
Some headers are only intended for consumption by the LibLLVM C++ code, others are intended as 
declarations of the exported LibLLVM-C API. The exported headers **MUST** go into the libllvm-c
folder, as the tooling to generate the interop code only looks in that directory. Any other headers
not intended for interop binding/projection belong in the same folder as this ReadMe

# libllvm-c
This folder contains all of the headers for the extended C API exported by the native library.
The Build-Interop.ps1 script assumes the extended APIs are placed here so the bindings generator
can use them and mark the APIs appropriately with comments as needed (Presently this is only in
the exports.def)

# Build Configurations
This library ONLY supports a release build (with debug information). This is a tradefoff for the
LLVM libraries. The production of such a large set of libraries AND their related debug information
is just too large to handle in free" OSS builds. (It's a major strain on local or "paid" services
as well). Thus to help keep things sane, the LLVM libraries are always RelWithDebInfo configurations
and this library matches that. (Mismatches in debug/release settings can cause a real nightmare of
compiler/linker warnings and errors due to unexpected missing libraries etc... Not to mention the
problems of publishing a debug build (The MSVC debug libraries are NOT part of any redist package.)

# About
Ubiquity.NET.Llvm provides a managed Object Oriented (OO) wrapper around an extended C ABI
for LLVM (An extended form of the official LLVM-C ABI is needed to fill in some gaps
in the official implementation to provide a robust managed OO wrapper. The number of
extensions required generally decreases with each release of LLVM).

## Key Features
* OO Wrapper around the LLVM API that closely follows the underlying C++ object model
* Sensible patterns for ownership and cleanup
    - Including ownership transfer ("move" semantics)
* Just In Time (JIT) compilation support
    - Including fully lazy compilation
* Generation of detailed debug information

### Full documentation
[Full documentation](https://ubiquitydotnet.github.io/Llvm.NET/) is available online.

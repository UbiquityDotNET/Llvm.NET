# ABI Interop code
This source was originally generated from the LLVM and LibLLVM extended headers. It
is now maintaine directly. The source generation was too problematic/complex to
generalize to the point where the output was usable "as-is". Additionally, it used
marshalling hints via a custom YAML configuration file. Ultimately, this file ended
up as a foreign language form of the marshalling attributes in C# code. So it was
abandoned.

The generator had one advantage in that it could read the configuration file AND
validate that the functions listed in it were still in the actual headers (though
it failed to identify any addition elements in the headers NOT in the config file!).
There may yet be some way to salvage some parts of the generator to perform a sanity
check and report any missing or mismatched information.

# ABI Function Pointers
ABI function pointers are represented as real .NET function pointers with an unmanaged
signature. Context handles are just value types that wrap around a runtime nint
(basically a strong typedef). Therefore, they are blittable value types and don't need
any marshaling. Global handles, however, do need marshalling and therefore **CANNOT**
appear in the signature of an unmanaged function pointer. Implementations MUST handle
marshalling of the ABI types manually.

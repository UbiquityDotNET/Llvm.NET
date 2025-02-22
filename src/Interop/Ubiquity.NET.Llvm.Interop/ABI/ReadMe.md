# ABI Interop code
This source was originally generated from the LLVM and LibLLVM extended headers. It
is now maintained directly. The source generation was too problematic/complex to
generalize to the point where the output was usable "as-is". Additionally, it used
marshalling hints via a custom YAML configuration file. Ultimately, this file ended
up as a foreign language form of the marshalling attributes in C# code. So it was
mostly abandoned. (It is still used to generate the exports and perform some validations
of the extension code at the native level)

The generator had one advantage in that it could read the configuration file AND
validate that the functions listed in it were still in the actual headers (though
it failed to identify any additional elements in the headers NOT in the config file!).
There may yet be some way to salvage some parts of the generator to perform a sanity
check and report any missing or mismatched information. Or possibly automatiacly generate
the proper code again. (Though that seems unlikely as the type of string is a major
problematatic factor.)

## ABI Function Pointers
ABI function pointers are represented as real .NET function pointers with an unmanaged
signature.

### special consideration for handles
LLVM Context handles are  just value types that wrap around a runtime nint (basically a
strong typedef for a pointer). Therefore, they are blittable value types and don't need
any significant marshaling support. Global handles, however, are reference types derived
from `SafeHandle` as they need special release semantics. All LLVM handle managed
projections **CANNOT** appear in the signature of an unmanaged function pointer as there
is way to mark the marshalling behavior for unmanaged pointers. Implementations of the
"callbacks" MUST handle marshalling of the ABI types manually. Normally, they will
leverage a `GCHandle` as the "context", perform marshalling, and forward the results on
to the managed context object. But implemenations are free to deal with things as they
prefer.

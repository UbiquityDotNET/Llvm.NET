# Internal details
This section is focused on providing internal details of the Ubiquity.NET.Llvm implementation for
developers contributing to the contents of the Ubiquity.NET.Llvm library itself. If you are only
interested in using the Ubiquity.NET.Llvm APIs you don't need this information, though it may
satisfy curiosity 8^).

## Generate Handles
The source for the handles is generated from the headers by the LibLLVM repository build. They
are created and published in the `Ubiquity.NET.Llvm.Interop.Handles` NuGet package. This package
has dependencies on types in the `Ubiquity.NET.Llvm.Interop` namespace and library so there's
naturally some tensions or issues with coherency there. The intent is to move ALL of the LIBLLVM
support into this repo. But doing so means careful use of the build to ensure only the parts that
have changed are built. (Specifically, that changes to the managed code portions of the wrappers
DO NOT re-build the LLVM library. [THat's a HUGE beast that takes significant resources to build,
but changes rarely so a rebuild after a release should be kept to a minimum.]) Until the
unification happens, the tension exists.

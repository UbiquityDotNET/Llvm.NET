# Internal details
This section is focused on providing internal details of the Ubiquity.NET.Llvm
implementation for developers contributing to the contents of the Ubiquity.NET.Llvm library
itself. If you are only interested in using the `Ubiquity.NET.Llvm` APIs you don't need this
information, though it may satisfy curiosity 8^).

## Generate Handles
The source for the handles is generated from the headers by the LibLLVM repository build.
They are created  by the `LLvmBindingsGenerator` from the headers contained in the 
`Ubiquity.NET.LibLLvm` package. The LibLLVM package is a bundle of the RID neutral headers
along with any RID specific headers. It is ultimately a "Uber" package that references the
RID specific native libraries. This keeps the size of each package down to meet NuGet
standards.

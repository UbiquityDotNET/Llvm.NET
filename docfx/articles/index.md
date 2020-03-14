# Articles
This section provides articles and general concepts on using Ubiquity.NET.Llvm. 

It is important to point out that the Ubiquity.NET.Llvm documentation is not a substitute
for the official LLVM documentation itself. That is, the content here is focused on
using LUbiquity.NET.Llvm and how it maps to the underlying LLVM. The LLVM documentation is,
generally speaking, required reading to understand Ubiquity.NET.Llvm. The topics here contain
links to the official LLVM documentation to help in further understanding the
functionality of the library.

#### [Samples](Samples/index.md)
##### Code Generation
###### [CodeGenerator With Debug Information](Samples/codegeneration.md)
##### Kaleidoscope
###### [Chapter 1 - Language Introduction](Samples/Kaleidoscope.md)
###### [Chapter 2 - Implementing the parser](Samples/Kaleidoscope-ch2.md)
###### [Chapter 3 - Generating LLVM IR](Samples/Kaleidoscope-ch3.md)
###### [Chapter 4 - JIT and Optimizer Support](Samples/Kaleidoscope-ch4.md)
###### [Chapter 5 - Control Flow](Samples/Kaleidoscope-ch5.md)
###### [Chapter 6 - User Defined Operators](Samples/Kaleidoscope-ch6.md)
###### [Chapter 7 - Mutable Variables](Samples/Kaleidoscope-ch7.md)
###### [Chapter 7.1 - Extreme Lazy JIT](Samples/Kaleidoscope-ch7.1.md)
###### [Chapter 8 - AOT Compilation](Samples/Kaleidoscope-ch8.md)
###### [Chapter 9 - Debug Information](Samples/Kaleidoscope-ch9.md)
###### [ParseTree Visitor](Samples/Kaleidoscope-ParseTreeVisitor.md)
###### [ParseTree Examples](Samples/Kaleidoscope-ParseTree-examples.md)

#### [Internal Details](InternalDetails/index.md)
##### LLVM-C API Handles
###### [Wrapping LLVM-C Handles](InternalDetails/llvm-handles.md)
###### [Interning LLVM-C Handles](InternalDetails/handleref-interning.md)
##### Marshaling LLVM types
###### [Marshaling Strings](InternalDetails/marshal-string.md)
###### [Marshaling LLVMBool](InternalDetails/marshal-LLVMBool.md)

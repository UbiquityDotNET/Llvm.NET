# Articles
This section provides articles and general concepts on using Ubiquity.NET.Llvm. 

It is important to point out that the Ubiquity.NET.Llvm documentation is not a substitute
for the official LLVM documentation itself. That is, the content here is focused on
using Ubiquity.NET.Llvm and how it maps to the underlying LLVM. The LLVM documentation is,
generally speaking, required reading to understand Ubiquity.NET.Llvm. The topics here contain
links to the official LLVM documentation to help in further understanding the functionality
of the library.

#### [Samples](Samples/index.md)
##### Code Generation
###### [CodeGenerator With Debug Information](xref:code-generation-with-debug-info)
##### Kaleidoscope
###### [Chapter 1 - Language Introduction](xref:Kaleidoscope-Overview)
###### [Chapter 2 - Implementing the parser](xref:Kaleidoscope-ch2)
###### [Chapter 3 - Generating LLVM IR](xref:Kaleidoscope-ch3)
###### [Chapter 4 - JIT and Optimizer Support](xref:Kaleidoscope-ch4)
###### [Chapter 5 - Control Flow](xref:Kaleidoscope-ch5)
###### [Chapter 6 - User Defined Operators](xref:Kaleidoscope-ch6)
###### [Chapter 7 - Mutable Variables](xref:Kaleidoscope-ch7)
###### [Chapter 7.1 - Extreme Lazy JIT](xref:Kaleidoscope-ch7.1)
###### [Chapter 8 - AOT Compilation](xref:Kaleidoscope-ch8)
###### [Chapter 9 - Debug Information](xref:Kaleidoscope-ch9)
###### [ParseTree Visitor](xref:Kaleidoscope-ParseTreeVisitor)
###### [ParseTree Examples](xref:Kaleidoscope-Parsetree-examples)

#### [Internal Details](InternalDetails/index.md)
##### LLVM-C API Handles
###### [Wrapping LLVM-C Handles](InternalDetails/llvm-handles.md)
###### [Interning LLVM-C Handles](InternalDetails/handleref-interning.md)
##### Marshaling LLVM types
###### [Marshaling Strings](InternalDetails/marshal-string.md)
###### [Marshaling LLVMBool](InternalDetails/marshal-LLVMBool.md)

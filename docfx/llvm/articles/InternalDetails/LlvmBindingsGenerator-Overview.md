# LlvmBindingsGenerator
LlvmBindingsGenerator is a tool used in building the Ubiquity.NET.Llvm.Interop library. As the
name implies, this tool generates the base level bindings between the native LibLlvm
library and managed code (e.g. It generates all the P/Invoke signatures, structure and
enumeration definitions and declarations). 

## Auto generation
The generator uses the [CppSharp library](https://github.com/mono/CppSharp) to parse
LLVM headers and the LibLLVM extended headers to get all the required declarations into
an Abstract Syntax Tree (AST). The generator then uses the AST to generate the required 
.NET interop code. 

## Challenges of interop for LLVM-C
Auto generation of the interop layer saves time and errors by moving tedious and error prone
hand coding into a tool that is run as part of the regular automated builds. However, the
job of auto generating for LLVM is not without some challenges. The original support for the
interop in Ubiquity.NET.Llvm was done using a different tool that generated code, which then
needed significant hand editing thereby defeating much of the value of the automated generation.
In the process several challenges for the LLVM library were defined:

 1. There are several different ways that strings are provided as either out parameters or
return value.
    1. As a raw const char*, this basically is just an alias that the managed code must then
       copy into a managed System.String and leave the native pointer alone (no dispose).
    2. As an allocated buffer that the managed application will need to release through some
       sort of Dispose type API.
        1. There are actually several such dispose APIs so the caller has to know which one to
           use for each case.
 1. LLVM-C uses a typedef LLVMBool as a return value for many functions, however the semantics
    of the return value depend on the function used. In some cases it is literally a boolean
    success(non-zero)/failure(zero) indication. While other functions use it as a status where
    success is zero and failure is any non zero value. This causes a great deal of confusion,
    especially if LLVMBool is blindly transformed to System.Boolean.
 1. LLVM-C uses typedefs for opaque pointers for objects in the underlying C++ implementation.
    Unfortunately, not all of the typedefs are consistent in how the are declared. Most are
    declared with the pointer as part of the typedef, but some are not. This makes automated
    generation more complex.
 1. Input and output array handling in the LLVM-C API is inconsistent.
    1. There are cases where the length required is retrieved from a function and other cases
       where it is an out parameter.
 1. Some flags type enumerations use a typedef to an unsigned value, since C limits the range
    of an enumerated value to an integer. Thus, what should be an enum with an underlying
    unsigned value ends up as a typedef to an unsigned.
 1. In many cases function pointer declarations (i.e. call back delegates) and function
    declarations in the LLVM-C headers don't include names for the parameters.
 1. In array vs out scalar semantics are not expressible in C (e.g. void foo(int* baz) could
    declare a function with a single integer as an out parameter, or it could mean a function
    that accepts an array of integers, with a fixed known size or possibly some sort of tag
    value to indicate the end, etc...)

## Configuration
LlvmBindings takes care of all of the challenges by using various custom passes on the AST to
correct or transform it to handle the special cases. Since it is not possible to automatically
determine the correct choice on many of the cases (like in vs. out array semantics, if a string
needs dispose, and which dispose method is needed, etc...) the LlvmBindings generator uses a
configuration data structure to determine the correct action. It also uses a set of validation
passes to ensure that any ambiguities without entries in the configuration trigger errors. It is
hoped that moving forward the only thing needed to update to a new version of LLVM is to update
the [configuration](LlvmBindingsGenerator-Configuration.md) tables. Although it is always possible
that some new pattern of code will require some new pass to handle.

# Ubiquity.NET.Llvm.Interop Generation 
The code generation for the Ubiquity.NET.Llvm.Interop namespace leverages [CppSharp] for parsing and processing
the LLVM-C (and custom extension) headers. The actual code generation is done using a custom system of
T4 templates. While CppSharp has a code generation system it is focused primarily on projecting the full
C++ type system (including implementing derived types in C#!). However, the generation is pretty inflexible
when it comes to the final form of the output in C# and how it handles marshaling. Ubiquity.NET.Llvm uses custom
handle types for all references in the C API along with custom string marshaling to handle the various kinds
of string disposal used in the C API. Unfortunately, CppSharp wasn't flexible enough to handle that with it's
built-in generation. Thus, the Ubiquity.NET.Llvm.Interop bindings are generated using customized support based on a
few T4 templates.

## T4 Templates
### ContextHandleTemplate.tt
Provides a template for all Context handles (see below for details of handles)

### GlobalHandleTemplate.tt
Provides a template for all Global handles (see below for details of handles)

### LLVMErrorRefTemplate.tt
Specialized template for handling LLVMErrorRef, which has multiple disposal APIs

### PerHeaderInteropTemplate.tt
This is the main template used to generate the interop for a given parsed header. It includes all the enums
structures, delegates and P/Invoke calls defined to match the headers.

### StringMarshalerTemplate.tt
This contains the template for custom string marshaling that incorporates the appropriate cleanup
handling depending on the requirements of the API. (Sadly, the LLVM-C API is not consistent with how
this is handled)

## LLVM-C Handle wrappers

Handles for LLVM are just opaque pointers. THey generally come in one of three forms.

  1. Context owned  
     Where there is always a well known owner that ultimately is responsible for
     disposing/releasing the resource.
  2. Global resources  
     Where there is no parent child ownership relationship and callers must manually release the resource
  3. An unowned alias to a global resource  
     This occurs when a child of a global resource contains a reference to the parent. In such
     a case the handle should be considered like an alias and not disposed.

The Handle implementations here follow consistent patterns for implementing each form of handle.

### Contextual handles

These handles are never manually released or disposed, though releasing their containers will make them
invalid. The general pattern for implementing such handles is taken care of by the T4 template
`ContextHandleTemplate.tt`

### Global Handles
Global handles require the caller to explicitly release the resources.
In Ubiquity.NET.Llvm these are managed with the .NET SafeHandles types through
an Ubiquity.NET.Llvm specific derived type LlvmObject. Thus, all resources in
LLVM requiring explicit release are handled consistently by the T4 template `GlobalHandleTemplate.tt`

### Global Alias handles
Global alias handles are a specialized form of global handles where they do not
participate in ownership control/release. These are commonly used when a child
of a global container exposes a property that references the parent container.
In such cases the reference retrieved from the child shouldn't be used to destroy
the child or the parent when no longer used.

In Ubiquity.NET.Llvm this is represented as a distinct context handle type that has
implicit casting to allow for simpler usage scenarios. (That, is an alias can cast to
an unowned global handle when needed to allow passing it in to native APIs without
taking ownership) Most APIs will have the alias type as the signature, especially for
[In] parameters. This helps to re-inforce the intended semantics for the parameter. To
make life easy there is an implicit cast from the global handle to an alias (which is
just a value type) when needed.

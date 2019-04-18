# Llvm.NET.Interop Generation 
The code generation for the Llvm.NET.Interop namespace leverages [CppSharp] for parsing and processing
the LLVM-C (and custom extension) headers. The actual code generation is done using a custom system of
T4 templates. While CppSharp has a code generation system it is focused primarily on projecting the full
C++ type system (including implementing derived types in C#!). However, the generation is pretty inflexible
when it comes to the final form of the output in C# and how it handles marshaling. Llvm.NET uses custom
handle types for all references in the C API along with custom string marshaling to handle the various kinds
of string disposal used in the C API. Unfortunately, CppSharp wasn't flexible enough to handle that with it's
built-in generation. Thus, the Llvm.NET.Interop bindings are generated using customized support based on a
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
invalid. The general pattern for implementing such handles is as follows:

``` C#
using System;
using System.Collections.Generic;

namespace Llvm.NET.Native
{
    internal struct LLVMxyzRef
        : IEquatable<LLVMxyzRef>
    {
        public override int GetHashCode( ) => Handle.GetHashCode( );

        public override bool Equals( object obj )
            => !( obj is null )
             && ( obj is LLVMxyxRef r )
             && ( r.Handle == Handle );

        public bool Equals( LLVMxyxRef other )
            => Handle == other.Handle;

        public static bool operator ==( LLVMxyxRef lhs, LLVMxyxRef rhs )
            => EqualityComparer<LLVMxyxRef>.Default.Equals( lhs, rhs );

        public static bool operator !=( LLVMxyxRef lhs, LLVMxyxRef rhs )
            => !( lhs == rhs );

        internal LLVMxyxRef( IntPtr pointer )
        {
            Handle = pointer;
        }

        private readonly IntPtr Handle;
    }
}
```

### Global Handles
Global handles require the caller to explicitly release the resources.
In Llvm.NET these are managed with the .NET SafeHandles types through
an Llvm.NET specific derived type LlvmObject. Thus, all resources in
LLVM requiring explicit release are handled consistently using the
following basic pattern:

``` C#
using System;
using System.Runtime.InteropServices;
using System.Security;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMxyzRef
        : LlvmObjectRef
    {
        public LLVMxyzRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            LLVMDisposeXyz( handle );
            return true;
        }

        private LLVMxyzRef( )
            : base( true )
        {
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeXyz( IntPtr @xyz );
    }
}
```

### Global Alias handles
Global alias handles are a specialized form of global handles where they do not
participate in ownership control/release. These are commonly used when a child
of a global container exposes a property that references the parent container.
In such cases the reference retrieved from the child shouldn't be used to destroy
the parent when no longer used. 

In Llvm.NET this is represented as a distinct handle type derived from the global
handle as follows:

``` C#
// xyz alias
internal class LLVMxyzAlias
    : LLVMxyzRef
{
    private LLVMxyzAlias()
        : base( IntPtr.Zero, false )
    {
    }
}
```
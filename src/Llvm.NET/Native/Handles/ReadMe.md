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

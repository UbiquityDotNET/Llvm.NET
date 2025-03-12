// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    // internal type to wrap GC handles with ref counting to ensure the
    // referenced object stays alive while in use for native ABI as a
    // "context" for callbacks. This is the key to ref counted behavior to hold 'o'
    // (and anything it references) alive for the GC. The "ownership" of the refcount
    // is handed to native code while the calling code is free to no longer reference
    // the containing instance as it holds an allocated GCHandle for itself and THAT
    // is kept alive by a ref count that is "owned" by native code.
    //
    // This is used as a member of such a holding type so that 'AddRefAndGetNativeContext'
    // retrieves a marshalled GCHandle for the containing/Controlling instance that is
    // then provided as the native "Context" parameter.
    internal class SafeGCHandle
        : SafeHandle
    {
        public SafeGCHandle(object o)
            : base(0, ownsHandle: true)
        {
            handle = (nint)GCHandle.Alloc(o);
        }

        public override bool IsInvalid => handle == 0;

        internal unsafe void* AddRefAndGetNativeContext()
        {
            bool ignoredButRequired = false;
            DangerousAddRef(ref ignoredButRequired);
            return (void*)handle;
        }

        protected override bool ReleaseHandle()
        {
            GCHandle.FromIntPtr(handle).Free();
            return true;
        }
    }
}

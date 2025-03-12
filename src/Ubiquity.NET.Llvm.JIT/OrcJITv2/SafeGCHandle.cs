// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    // internal type to wrap GC handles with ref counting to ensure the
    // referenced object stays alive while in use for native ABI as a
    // "context" for callbacks.
    internal class SafeGCHandle
        : SafeHandle
    {
        public SafeGCHandle(object o)
            : base(0, ownsHandle: true)
        {
            handle = (nint)GCHandle.Alloc(o);
        }

        public override bool IsInvalid => handle == 0;

        protected override bool ReleaseHandle()
        {
            GCHandle.FromIntPtr(handle).Free();
            return true;
        }
    }
}

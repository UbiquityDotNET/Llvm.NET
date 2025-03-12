// -----------------------------------------------------------------------
// <copyright file="ResourceTracker.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if FUTURE_DEVELOPMENT_AREA

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Resource tracker</summary>
    public class ResourceTracker
    {
        /// <summary>Moves all resources associated with this tracker to <paramref name="other"/></summary>
        /// <param name="other">Destination tracker to receive all the resources</param>
        public void MoveTo(ResourceTracker other)
        {
            ArgumentNullException.ThrowIfNull(other);
            LLVMOrcResourceTrackerTransferTo(Handle, other.Handle);
        }

        internal ResourceTracker(LLVMOrcResourceTrackerRef h)
        {
            Handle = h;
        }

        internal LLVMOrcResourceTrackerRef Handle { get; init; }
    }
}
#endif

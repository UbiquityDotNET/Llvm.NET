// -----------------------------------------------------------------------
// <copyright file="DITypeVoid.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Special Debug type to represent void</summary>
    /// <remarks>
    /// This doesn't have a corresponding node type in LLVM as the actual IR
    /// representation is <see langword="null"/>. However, allowing null to
    /// represent the "void" type complicates the API surface with respect to
    /// nullability and correctness checks. Therefore, this type is the "null
    /// object pattern" implementation for the debug type system to indicate
    /// the void type.
    /// </remarks>
    public class DITypeVoid
                : DIType
    {
        /// <summary>Gets the singleton DITypeVoid instance</summary>
        public static DITypeVoid Instance => LazyInstance.Value;

        private DITypeVoid( )
            : base( default )
        {
            // LlvmMetadata.FromHandle<T>, internally knows about this null object pattern
            // and when T is DIType and handle == default will get the singleton instance of
            // this class. Since default(LLVMMetadaRef) isn't sufficiently distinct to use
            // for caching a singleton is required to ensure reference equality "just works".
        }

        private static readonly Lazy<DITypeVoid> LazyInstance = new Lazy<DITypeVoid>(()=>new DITypeVoid());
    }
}

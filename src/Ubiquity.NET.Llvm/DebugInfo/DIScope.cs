﻿// -----------------------------------------------------------------------
// <copyright file="DIScope.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for all Debug information scopes</summary>
    public class DIScope
        : DINode
    {
        /// <summary>Gets the <see cref="DIFile"/> describing the file this scope belongs to</summary>
        /// <remarks>If this scope is a <see cref="DIFile"/> then this returns <see langword="this"/></remarks>
        public DIFile? File
        {
            get
            {
                if( MetadataHandle == default )
                {
                    return null;
                }

                return this is DIFile file ? file : FromHandle<DIFile>( LLVMDIScopeGetFile( MetadataHandle ) );
            }
        }

        /// <summary>Gets the parent scope for this scope or <see langword="null"/> if no parent scope exists</summary>
        public virtual DIScope? Scope => null;

        /// <summary>Gets the name of the scope or an empty string if the scope doesn't have a name</summary>
        public virtual string Name { get; } = string.Empty;

        private protected DIScope( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

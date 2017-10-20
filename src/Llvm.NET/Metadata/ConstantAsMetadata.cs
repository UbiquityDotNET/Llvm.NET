// <copyright file="ConstantAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET
{
    /// <summary>Constant <see cref="Value"/> as metadata</summary>
    public class ConstantAsMetadata
        : ValueAsMetadata
    {
        internal ConstantAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        /*
        public Constant Constant { get; }

        static public ConstantAsMetadata GetIfExists(Constant const);
        static public ConstantAsMetadata Create(Constant const);
        */
    }
}

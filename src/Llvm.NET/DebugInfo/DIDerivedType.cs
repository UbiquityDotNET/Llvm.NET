// <copyright file="DIDerivedType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diderivedtype"/></summary>
    /// <seealso href="xref:llvm_langref#diderivedtype">LLVM DIDerivedType</seealso>
    public class DIDerivedType
        : DIType
    {
        // MD operannds:
        //    0 - File
        //    1 - Scope
        //    2 - Name
        //    3 - Base Type
        //    4 - Extra data
        public DIType BaseType => Operands[ 3 ].Metadata as DIType;

        /// <summary>Creates a new <see cref="DIDerivedType"/> from an <see cref="LLVMMetadataRef"/></summary>
        /// <param name="handle">Handle to wrap</param>
        internal DIDerivedType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

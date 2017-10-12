// <copyright file="DICompositeType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a composite type</summary>
    /// <seealso href="xref:llvm_langref#dicompositetype">LLVM DICompositeType</seealso>
    public class DICompositeType : DIType
    {
        public DIType BaseType => Operands[ 3 ].Metadata as DIType;

        public IReadOnlyList<DINode> Elements => new TupleTypedArrayWrapper<DINode>( Operands[ 4 ].Metadata as MDTuple );

        // TODO: VTableHolder   Operands[5]
        // TODO: TemplateParams Operands[6]
        // TODO: Identifier     Operands[7]

        /// <summary>Initializes a new instance of the <see cref="DICompositeType"/> class from an LLVM-C API Metadata handle</summary>
        /// <param name="handle">LLVM handle to wrap</param>
        internal DICompositeType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

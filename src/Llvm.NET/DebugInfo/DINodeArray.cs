// <copyright file="DINodeArray.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Array of <see cref="DINode"/> debug information nodes for use with <see cref="DebugInfoBuilder"/> methods</summary>
    /// <seealso cref="DebugInfoBuilder.GetOrCreateArray(IEnumerable{DINode})"/>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This matches the wrapped native type" )]
    public class DINodeArray : TupleTypedArrayWrapper<DINode>
    {
        internal DINodeArray( MDTuple tuple )
            : base( tuple )
        {
        }
    }
}

// <copyright file="DITypeArray.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.DebugInfo
{
    /// <summary>Array of <see cref="DIType"/> nodes for use with see <see cref="DebugInfoBuilder"/> methods</summary>
    public class DITypeArray
        : TupleTypedArrayWrapper<DIType>
    {
        internal DITypeArray( MDTuple tuple )
            : base( tuple )
        {
        }
    }
}

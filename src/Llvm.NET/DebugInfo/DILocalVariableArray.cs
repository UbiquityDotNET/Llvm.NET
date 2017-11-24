// <copyright file="DITypeArray.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.DebugInfo
{
    /// <summary>Array of <see cref="DILocalVariable"/> nodes for use with see <see cref="DebugInfoBuilder"/> methods</summary>
    public class DILocalVariableArray
        : TupleTypedArrayWrapper<DILocalVariable>
    {
        internal DILocalVariableArray( MDTuple tuple )
            : base( tuple )
        {
        }
    }
}

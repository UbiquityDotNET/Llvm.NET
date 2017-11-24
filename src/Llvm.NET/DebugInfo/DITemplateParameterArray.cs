// <copyright file="DITypeArray.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.DebugInfo
{
    /// <summary>Array of <see cref="DITemplateParameter"/> nodes for use with see <see cref="DebugInfoBuilder"/> methods</summary>
    public class DITemplateParameterArray
        : TupleTypedArrayWrapper<DITemplateParameter>
    {
        internal DITemplateParameterArray( MDTuple tuple )
            : base( tuple )
        {
        }
    }
}

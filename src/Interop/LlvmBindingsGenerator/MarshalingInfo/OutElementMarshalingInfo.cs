// -----------------------------------------------------------------------
// <copyright file="OutElementMarshalingInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    // special case for legacy APIs using handleType* x as an out array param, when some other GetNumHandleType() provides the count of entries
    // this requires pre-allocating an array for the correct size (even if zero elements) and then using the first element of the array as the out param.
    // (e.g. In "C" the parameter is a pointer to a pre-allocated array of the correct size)
    internal class OutElementMarshalingInfo
        : MarshalInfoBase
    {
        public OutElementMarshalingInfo(string functionName, string paramName)
            : base( functionName, paramName, ParamSemantics.Out)
        {
        }

        public override IEnumerable<Attribute> Attributes => Enumerable.Empty<Attribute>( );

        public override QualifiedType TransformType( QualifiedType type )
        {
            return type.Type is PointerType pt ? new QualifiedType( pt.Pointee ) : base.TransformType( type );
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="PrimitiveTypeMarshalInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Can be used in configuration" )]
    internal class PrimitiveTypeMarshalInfo
        : MarshalInfoBase
    {
        public PrimitiveTypeMarshalInfo(string functionName, BuiltinType type, string parameterName, ParamSemantics semantics)
            : base( functionName, parameterName, semantics )
        {
            MarshalType = type;
        }

        public override Type Type => MarshalType;

        public override IEnumerable<CppSharp.AST.Attribute> Attributes => Enumerable.Empty<CppSharp.AST.Attribute>( );

        private readonly BuiltinType MarshalType;
    }
}

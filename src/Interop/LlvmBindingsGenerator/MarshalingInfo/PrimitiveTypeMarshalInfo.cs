// -----------------------------------------------------------------------
// <copyright file="PrimitiveTypeMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
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
        public PrimitiveTypeMarshalInfo( string functionName, PrimitiveType primitiveType)
            : this( functionName, primitiveType, MarshalingInfoMap.ReturnParamName, ParamSemantics.Return )
        {
        }

        public PrimitiveTypeMarshalInfo(string functionName, PrimitiveType type, string parameterName, ParamSemantics semantics)
            : base( functionName, parameterName, semantics )
        {
            MarshalType = new BuiltinType( type );
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            return new QualifiedType( MarshalType, type.Qualifiers );
        }

        public override IEnumerable<CppSharp.AST.Attribute> Attributes => Enumerable.Empty<CppSharp.AST.Attribute>( );

        private readonly BuiltinType MarshalType;
    }
}

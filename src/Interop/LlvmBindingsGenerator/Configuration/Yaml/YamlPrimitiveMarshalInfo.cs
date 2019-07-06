// -----------------------------------------------------------------------
// <copyright file="PrimitiveMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [DebuggerDisplay( "{Kind}" )]
    internal class YamlPrimitiveMarshalInfo
        : YamlBindingTransform
    {
        public PrimitiveType Kind { get; set; }

        public override IEnumerable<CppSharp.AST.Attribute> Attributes => Enumerable.Empty<CppSharp.AST.Attribute>( );

        public override QualifiedType TransformType( QualifiedType type )
        {
            return new QualifiedType( new BuiltinType( Kind ), type.Qualifiers );
        }
    }
}

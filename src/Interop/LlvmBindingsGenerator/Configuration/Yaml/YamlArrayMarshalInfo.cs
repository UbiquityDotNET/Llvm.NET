// -----------------------------------------------------------------------
// <copyright file="ArrayMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using CppSharp.AST;

using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "De-serialized from YAML" )]
    [DebuggerDisplay( "Array<{SubType}>" )]
    internal class YamlArrayMarshalInfo
        : YamlBindingTransform
    {
        public UnmanagedType SubType { get; set; } = UnmanagedType.U1;

        public int? SizeParam { get; set; }

        public override IEnumerable<CppSharp.AST.Attribute> Attributes
        {
            get
            {
                switch( Semantics )
                {
                case ParamSemantics.In:
                    yield return InAttribute;
                    break;

                case ParamSemantics.Out:
                    yield return OutAttribute;
                    break;

                case ParamSemantics.InOut:
                    yield return InAttribute;
                    yield return OutAttribute;
                    break;
                }

                var attrib = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPArray", $"ArraySubType = UnmanagedType.{SubType}" );
                if( SizeParam.HasValue )
                {
                    attrib.AddParameter( $"SizeParamIndex = {SizeParam}" );
                }

                yield return attrib;
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            // attempt to get a more precise type than sbyte* for byte sized values and bool
            QualifiedType elementType;
            switch(SubType)
            {
            case UnmanagedType.Bool:
                elementType = new QualifiedType( new BuiltinType( PrimitiveType.Bool ), type.Qualifiers);
                break;

            case UnmanagedType.I1:
                elementType = new QualifiedType( new BuiltinType( PrimitiveType.SChar ), type.Qualifiers );
                break;
            case UnmanagedType.U1:
                elementType = new QualifiedType( new BuiltinType( PrimitiveType.UChar ), type.Qualifiers );
                break;
            default:
                elementType = ( type.Type as PointerType ).QualifiedPointee;
                break;
            }

            return new QualifiedType( new ArrayType( ) { QualifiedType = elementType } );
        }
    }
}

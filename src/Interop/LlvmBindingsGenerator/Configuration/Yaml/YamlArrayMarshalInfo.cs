// -----------------------------------------------------------------------
// <copyright file="ArrayMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

using CppSharp;
using CppSharp.AST;

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
                    if (SizeParam.HasValue)
                    {
                        Diagnostics.Warning( "Array parameter {0} [Line: {1}] specifies a size parameter, but is marked as In. SizeParam ignored.", Name, Start.Line );
                    }

                    yield break; // only one attribute needed for "in" semantics

                case ParamSemantics.Out:
                    yield return OutAttribute;
                    break;

                case ParamSemantics.InOut:
                    yield return InAttribute;
                    yield return OutAttribute;
                    break;
                }

                if (SizeParam.HasValue)
                {
                    var attrib = new TargetedAttribute( typeof( MarshalUsingAttribute ));
                    attrib.AddParameter( $"CountElementName = {SizeParam}" );
                    yield return attrib;
                }
                else if (Semantics == ParamSemantics.Out)
                {
                    // Size parameter is required for OUT parameters as the marshaller has no other way to know the size of the array
                    // Additionally, this assumes it is a native "alias" pointer and copied to the managed heap. This may be prematurely
                    // pessimistic, but such is the tradeoff of a generalized code generation.
                    Diagnostics.Error( "Array parameter {0} [Line: {1}] with {2} semantics MUST specify a size parameter", Name, Start.Line, Semantics );
                }
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            // attempt to get a more precise type than sbyte* for byte sized values and bool
            var elementType = SubType switch
            {
                UnmanagedType.Bool => new QualifiedType( new BuiltinType( PrimitiveType.Bool ), type.Qualifiers ),
                UnmanagedType.I1 => new QualifiedType( new BuiltinType( PrimitiveType.SChar ), type.Qualifiers ),
                UnmanagedType.U1 => new QualifiedType( new BuiltinType( PrimitiveType.UChar ), type.Qualifiers ),
                _ => (type.Type as PointerType).QualifiedPointee,
            };

            return new QualifiedType( new ArrayType( ) { QualifiedType = elementType } );
        }
    }
}

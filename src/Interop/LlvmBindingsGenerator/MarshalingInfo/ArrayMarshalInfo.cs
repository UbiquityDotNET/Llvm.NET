// -----------------------------------------------------------------------
// <copyright file="ByteArrayMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using CppSharp.AST;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator
{
    // [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] =>IN
    // [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = n)] =>OUT,INOUT
    internal class ArrayMarshalInfo
        : MarshalInfoBase
    {
        public ArrayMarshalInfo( string functionName, string paramName )
            : this(functionName, paramName, UnmanagedType.U1, ParamSemantics.In, null )
        {
        }

        public ArrayMarshalInfo( string functionName, string paramName, UnmanagedType subType )
            : this( functionName, paramName, subType, ParamSemantics.In)
        {
        }

        public ArrayMarshalInfo( string functionName, string paramName, UnmanagedType subType, ParamSemantics semantics )
            : this( functionName, paramName, subType, semantics, null )
        {
        }

        public ArrayMarshalInfo( string functionName, string paramName, UnmanagedType subType, ParamSemantics semantics, int? sizeParam)
            : base( functionName, paramName, semantics )
        {
            ElementMarshalType = subType;
            Attrib = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPArray", $"ArraySubType = UnmanagedType.{subType}" );
            SizeParam = sizeParam;
            if( SizeParam.HasValue )
            {
                Attrib.AddParameter( $"SizeParamIndex = {sizeParam}" );
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            return new QualifiedType( new ArrayType( ) { QualifiedType = ( type.Type as PointerType ).QualifiedPointee } );
        }

        public override IEnumerable<CppSharp.AST.Attribute> Attributes
        {
            get
            {
                switch( Semantics )
                {
                case ParamSemantics.Return:
                    break;

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

                yield return Attrib;
            }
        }

        public int? SizeParam { get; }

        public UnmanagedType ElementMarshalType { get; }

        private readonly TargetedAttribute Attrib;

        private static readonly CppSharp.AST.Attribute InAttribute = new TargetedAttribute( typeof( InAttribute ) );
        private static readonly CppSharp.AST.Attribute OutAttribute = new TargetedAttribute( typeof( OutAttribute ) );
    }
}

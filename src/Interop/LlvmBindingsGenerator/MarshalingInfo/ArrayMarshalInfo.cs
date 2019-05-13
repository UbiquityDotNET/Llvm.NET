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
    // [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] =>IN
    // [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = n)] =>OUT,INOUT
    internal class ArrayMarshalInfo
        : MarshalInfoBase
    {
        public ArrayMarshalInfo( string functionName, string paramName, UnmanagedType subType = UnmanagedType.I1, ParamSemantics semantics = ParamSemantics.In, int sizeParam = 0 )
            : base( functionName, paramName, semantics )
        {
            SizeParam = sizeParam;
            ElementMarshalType = subType;
            Attrib = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPArray", $"ArraySubType = UnmanagedType.{subType}" );
            if( Semantics != ParamSemantics.In )
            {
                Attrib.AddParameter( $"SizeParamIndex = {sizeParam}" );
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            if( ElementMarshalType == UnmanagedType.I1 )
            {
                return new QualifiedType( new CILType( typeof( byte[ ] ) ) );
            }

            return new QualifiedType( new ArrayType( ) { QualifiedType = type } );
        }

        public override IEnumerable<CppSharp.AST.Attribute> Attributes
        {
            get { yield return Attrib; }
        }

        public int SizeParam { get; }

        public UnmanagedType ElementMarshalType { get; }

        private readonly TargetedAttribute Attrib;
    }
}

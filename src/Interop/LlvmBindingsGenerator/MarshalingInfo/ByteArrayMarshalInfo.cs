// -----------------------------------------------------------------------
// <copyright file="ByteArrayMarshalInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
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
    internal class ByteArrayMarshalInfo
        : MarshalInfoBase
    {
        public ByteArrayMarshalInfo( string functionName, string paramName, int sizeParam = 0, ParamSemantics semantics = ParamSemantics.In )
            : base( functionName, paramName, semantics )
        {
            SizeParam = sizeParam;
            Attrib = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPArray", "ArraySubType = UnmanagedType.I1" );
            if( Semantics != ParamSemantics.In )
            {
                Attrib.AddParameter( $"SizeParamIndex = {sizeParam}" );
            }
        }

        public override CppSharp.AST.Type Type => new CILType( typeof( byte[ ] ) );

        public override IEnumerable<CppSharp.AST.Attribute> Attributes
        {
            get { yield return Attrib; }
        }

        public int SizeParam { get; }

        private readonly TargetedAttribute Attrib;
    }
}

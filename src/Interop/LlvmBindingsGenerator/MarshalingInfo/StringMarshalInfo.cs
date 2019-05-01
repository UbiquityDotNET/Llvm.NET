// -----------------------------------------------------------------------
// <copyright file="StringMarshalInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CppSharp.AST;
using LlvmBindingsGenerator.CppSharpExtensions;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal enum StringDisposal
    {
        CopyAlias,
        DisposeMessage,
        OrcDisposeMangledSymbol,
        DisposeErrorMesage
    }

    internal class StringMarshalInfo
        : MarshalInfoBase
    {
        public StringMarshalInfo( string functionName, StringDisposal disposalKind = StringDisposal.CopyAlias )
            : this(functionName, string.Empty, ParamSemantics.Return, disposalKind )
        {
        }

        public StringMarshalInfo( string functionName, string paramName, ParamSemantics semantics, StringDisposal disposalKind = StringDisposal.CopyAlias )
            : base( functionName, paramName, semantics )
        {
            DisposalKind = disposalKind;
        }

        public StringDisposal DisposalKind { get; }

        public override QualifiedType TransformType( QualifiedType type )
        {
            var transformedType = Semantics == ParamSemantics.InOut ? StringBuilderType : StringType;
            return new QualifiedType( transformedType );
        }

        public override IEnumerable<Attribute> Attributes
        {
            get
            {
                var args = new List<string>( );
                switch( Semantics )
                {
                case ParamSemantics.In:
                    args.Add( "UnmanagedType.LPStr" );
                    break;
                case ParamSemantics.Return:
                case ParamSemantics.Out:
                    var (marshalClassName, _) = StringDisposalMarshalerMap.LookupMarshaler( DisposalKind );
                    args.Add( "UnmanagedType.CustomMarshaler" );
                    args.Add( $"MarshalTypeRef = typeof( {marshalClassName} )" );
                    break;

                case ParamSemantics.InOut:
                    yield break;
                }

                yield return new TargetedAttribute(
                    Semantics == ParamSemantics.Return ? AttributeTarget.Return : AttributeTarget.Default,
                    typeof( MarshalAsAttribute ),
                    args
                    );
            }
        }

        private static readonly CILType StringType = new CILType( typeof( string ) );
        private static readonly CILType StringBuilderType = new CILType( typeof( StringBuilder ) );
    }
}

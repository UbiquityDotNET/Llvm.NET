// -----------------------------------------------------------------------
// <copyright file="StringMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using CppSharp.AST;

using LlvmBindingsGenerator.CppSharpExtensions;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration
{
    internal enum StringDisposal
    {
        CopyAlias,
        DisposeMessage,
        OrcDisposeMangledSymbol,
        DisposeErrorMesage
    }

    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [DebuggerDisplay( "string=>{Kind}" )]
    internal class YamlStringMarshalInfo
        : YamlBindingTransform
    {
        public StringDisposal Kind { get; set; }

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
                    var (marshalClassName, _) = StringDisposalMarshalerMap.LookupMarshaler( Kind );
                    args.Add( "UnmanagedType.CustomMarshaler" );
                    args.Add( $"MarshalTypeRef = typeof( {marshalClassName} )" );
                    break;

                case ParamSemantics.InOut:
                    yield break;
                }

                yield return new TargetedAttribute( Semantics == ParamSemantics.Return ? AttributeTarget.Return : AttributeTarget.Default
                                                  , typeof( MarshalAsAttribute )
                                                  , args
                                                  );
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            var transformedType = Semantics == ParamSemantics.InOut ? StringBuilderType : StringType;
            return new QualifiedType( transformedType );
        }

        private static readonly CILType StringType = new CILType( typeof( string ) );
        private static readonly CILType StringBuilderType = new CILType( typeof( StringBuilder ) );
    }
}

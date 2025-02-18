// -----------------------------------------------------------------------
// <copyright file="StringMarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

using CppSharp.AST;

using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration
{
    internal enum StringDisposal
    {
        None,
        DisposeMessage,
        OrcDisposeMangledSymbol,
        DisposeErrorMessage
    }

    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Layering of analyzers is too stupid to see it..." )]
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
                    // "in" strings are always marshalled using standard ANSI string marshalling
                    args.Add( $"typeof({nameof(AnsiStringMarshaller)})" );
                    break;
                case ParamSemantics.Return:
                case ParamSemantics.Out:
                    var (marshalClassName, _) = StringDisposalMarshalerMap.LookupMarshaller( Kind );
                    args.Add( $"typeof( {marshalClassName} )" );
                    break;

                case ParamSemantics.InOut:
                    yield break;
                }

                yield return new TargetedAttribute( Semantics == ParamSemantics.Return ? AttributeTarget.Return : AttributeTarget.Default
                                                  , typeof( MarshalUsingAttribute )
                                                  , args
                                                  );
            }
        }

        public override QualifiedType TransformType( QualifiedType type )
        {
            return Semantics == ParamSemantics.InOut ? type : new QualifiedType( StringType );
        }

        private static readonly CILType StringType = new( typeof( string ) );

        // private static readonly CILType PreAllocatedAnsiStringType = new( typeof(byte*) );
    }
}

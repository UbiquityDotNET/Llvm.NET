// -----------------------------------------------------------------------
// <copyright file="ParsedFunction.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CppSharp.AST;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Templates
{
    internal class ParsedFunctionSignature
    {
        public ParsedFunctionSignature( Function f )
        {
            if( f.IsOperator || f.IsThisCall )
            {
                throw new ArgumentException( "Unsupported function type", nameof(f) );
            }

            Signature = f.FunctionType.Type as FunctionType;
            Comments = new ParsedComment( f.Comment );
            Name = f.Name;
            Attributes = f.Attributes;
            Introducer = "public static extern ";
        }

        public ParsedFunctionSignature( TypedefNameDecl td )
        {
            if( td.TryGetFunctionSignature( out FunctionType signature ) )
            {
                if( signature.CallingConvention != CppSharp.AST.CallingConvention.C )
                {
                    throw new NotSupportedException( "Only delegates with the 'C' Calling convention are supported" );
                }

                Signature = signature;
                Comments = new ParsedComment( td.Comment );
                Name = td.Name;
                Attributes = new[ ] { UnmanagedFunctionPointerAttrib };
                Introducer = "public delegate ";
            }
            else
            {
                throw new ArgumentException( "pointer to function type required" );
            }
        }

        public ParsedComment Comments { get; }

        public string Name { get; }

        public IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        public string ReturnType => GetTypeName( Signature.ReturnType.Type );

        public IEnumerable<string> Parameters => GetParameters( Signature.Parameters );

        public override string ToString( )
        {
            var bldr = new StringBuilder( );
            if(ReturnType.Contains("*"))
            {
                bldr.Append( "unsafe " );
            }

            bldr.Append( Introducer )
                .Append( ReturnType )
                .Append( ' ' )
                .Append( Name )
                .Append( "( " );

            foreach(string param in Parameters )
            {
                bldr.Append( param );
            }

            bldr.AppendLine( " );" );
            return bldr.ToString( );
        }

        public FunctionType Signature { get; }

        internal static string GetTypeName( CppSharp.AST.Type type )
        {
            string retVal;
            switch( type )
            {
            case TypedefType tdt when( tdt.Declaration.Name == "LLVMStatus" ):
                retVal = "LLVMStatus";
                break;

            case CppSharp.AST.Type t when t.TryGetHandleDecl( out TypedefNameDecl decl ):
                retVal = decl.Name;
                break;

            default:
                retVal = type.ToString( );
                break;
            }

            return retVal;
        }

        private static string CreateEscapedIdentifier( Parameter p )
        {
            return CodeDom.CreateEscapedIdentifier( p.Name );
        }

        private static IEnumerable<string> GetParameters(IList<Parameter> parameters)
        {
            var bldr = new StringBuilder( );
            for( int i = 0; i < parameters.Count; ++i )
            {
                var arg = parameters[ i ];
                var argType = arg.Type;
                foreach( var attr in arg.Attributes )
                {
                    bldr.Append( attr.AsString( ) );
                }

                if( arg.IsOut )
                {
                    bldr.Append( "out " );
                    if( argType.ToString( ) != "string" && argType is PointerType pt )
                    {
                        argType = pt.Pointee;
                    }
                }

                if( arg.IsInOut )
                {
                    bldr.Append( "ref " );
                    if( argType.ToString( ) != "string" && argType is PointerType pt )
                    {
                        argType = pt.Pointee;
                    }
                }

                bldr.Append( GetTypeName( argType ) )
                    .Append( ' ' )
                    .Append( CreateEscapedIdentifier( arg ) );

                if( i < parameters.Count - 1 )
                {
                    bldr.Append( ", " );
                }

                yield return bldr.ToString( );
                bldr.Clear( );
            }
        }

        private readonly string Introducer;

        private static readonly CodeDomProvider CodeDom
            = new Microsoft.CSharp.CSharpCodeProvider( new Dictionary<string, string> { [ "CompilerVersion" ] = "v7.3" } );

        private static readonly CppSharp.AST.Attribute UnmanagedFunctionPointerAttrib =
            new TargetedAttribute( typeof( UnmanagedFunctionPointerAttribute ), "global::System.Runtime.InteropServices.CallingConvention.Cdecl" );
    }
}

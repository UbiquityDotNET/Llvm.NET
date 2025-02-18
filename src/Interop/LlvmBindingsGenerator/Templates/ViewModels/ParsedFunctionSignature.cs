// -----------------------------------------------------------------------
// <copyright file="ParsedFunction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using CppSharp.AST;
using CppSharp.AST.Extensions;

namespace LlvmBindingsGenerator.Templates
{
    internal class ParsedFunctionSignature
    {
        public ParsedFunctionSignature(Function f, ITypePrinter2 typePrinter)
        {
            TypePrinter = typePrinter;
            if(f.IsOperator || f.IsThisCall)
            {
                throw new ArgumentException( "Unsupported function type", nameof( f ) );
            }

            Signature = f.FunctionType.Type as FunctionType;
            Name = f.Name;
            Attributes = f.Attributes;
            Introducer = "public static unsafe partial ";
        }

        // FUTURE: use C# syntax for an unmanaged function pointer with Cdecl
        public ParsedFunctionSignature(TypedefNameDecl td, ITypePrinter2 typePrinter)
        {
            TypePrinter = typePrinter;
            if(td.TryGetFunctionSignature( out FunctionType signature ))
            {
                if(signature.CallingConvention != CppSharp.AST.CallingConvention.C)
                {
                    throw new NotSupportedException( "Only delegates with the 'C' Calling convention are supported" );
                }

                Signature = signature;
                Name = td.Name;
                Attributes = [ UnmanagedFunctionPointerAttrib ];
                Introducer = "public unsafe delegate ";
            }
            else
            {
                throw new ArgumentException( "pointer to function type required" );
            }
        }

        public string Name { get; }

        public IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        public bool HasNonVoidReturn => !Signature.ReturnType.Type.IsPrimitiveType( PrimitiveType.Void );

        public string ReturnType => TypePrinter.GetName( Signature.ReturnType.Type, TypeNameKind.Managed );

        public IEnumerable<string> Parameters => GetParameters( Signature.Parameters );

        public IEnumerable<string> ParameterNames => Signature.Parameters.Select( p => p.Name );

        public override string ToString()
        {
            var bldr = new StringBuilder( );

            bldr.Append( Introducer )
                .Append( ReturnType )
                .Append( ' ' )
                .Append( Name )
                .Append( "( " );

            foreach(string parameter in Parameters)
            {
                bldr.Append( parameter );
            }

            bldr.AppendLine( " );" );
            return bldr.ToString();
        }

        public FunctionType Signature { get; }

        private static string CreateEscapedIdentifier(Parameter p)
        {
            return CodeDom.CreateEscapedIdentifier( p.Name );
        }

        private IEnumerable<string> GetParameters(IList<Parameter> parameters)
        {
            var bldr = new StringBuilder( );
            for(int i = 0; i < parameters.Count; ++i)
            {
                var arg = parameters[ i ];
                var argType = arg.Type;
                foreach(var attr in arg.Attributes)
                {
                    bldr.Append( attr.AsString() );
                }

                if(arg.IsOut)
                {
                    bldr.Append( "out " );
                    if(!argType.IsConstCharString() && argType is PointerType pt)
                    {
                        argType = pt.Pointee;
                    }
                }

                if(arg.IsInOut)
                {
                    if(!IsCharPointer( argType ))
                    {
                        bldr.Append( "ref " );
                    }
                }

                bldr.Append( TypePrinter.GetName( argType, TypeNameKind.Managed ) )
                    .Append( ' ' )
                    .Append( CreateEscapedIdentifier( arg ) );

                if(i < parameters.Count - 1)
                {
                    bldr.Append( ", " );
                }

                yield return bldr.ToString();
                bldr.Clear();
            }
        }

        private static bool IsCharPointer(CppSharp.AST.Type T)
        {
            return T is PointerType pt && pt.Pointee.Desugar().IsPrimitiveType( PrimitiveType.Char );
        }

        private readonly ITypePrinter2 TypePrinter;
        private readonly string Introducer;

        private static readonly Microsoft.CSharp.CSharpCodeProvider CodeDom
            = new( new Dictionary<string, string> { [ "CompilerVersion" ] = "v7.3" } );

        private static readonly CppSharp.AST.Attribute UnmanagedFunctionPointerAttrib =
            new TargetedAttribute( typeof( UnmanagedFunctionPointerAttribute ), "global::System.Runtime.InteropServices.CallingConvention.Cdecl" );
    }
}

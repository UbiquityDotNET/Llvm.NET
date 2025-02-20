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
            IsPointer = false;
            TypePrinter = typePrinter;
            if(f.IsOperator || f.IsThisCall)
            {
                throw new ArgumentException( "Unsupported function type; operators and 'this call' functions are not supported [Strict ISO-C ABI only]", nameof( f ) );
            }

            Signature = f.FunctionType.Type as FunctionType;
            Name = f.Name;
            Attributes = f.Attributes;
        }

        public ParsedFunctionSignature(TypedefNameDecl td, ITypePrinter2 typePrinter)
        {
            IsPointer = true;
            TypePrinter = typePrinter;
            if(td.TryGetFunctionSignature( out FunctionType signature ))
            {
                if(signature.CallingConvention != CppSharp.AST.CallingConvention.C)
                {
                    throw new NotSupportedException( "Only function pointers with the 'C' Calling convention are supported" );
                }

                Signature = signature;
                Name = td.Name;
                Attributes = [];
            }
            else
            {
                throw new ArgumentException( "pointer to function type required" );
            }
        }

        public bool IsPointer { get; init; }

        public string Name { get; }

        public IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        public bool HasNonVoidReturn => !Signature.ReturnType.Type.IsPrimitiveType( PrimitiveType.Void );

        public string ReturnType => TypePrinter.GetName( Signature.ReturnType.Type, TypeNameKind.Managed );

        public IEnumerable<string> Parameters => GetParameters( Signature.Parameters );

        public IEnumerable<string> ManagedParameterTypeNames
        {
            get
            {
                foreach(var parameter in Signature.Parameters)
                {
                    if (parameter.Type.TryGetHandleDecl(out TypedefNameDecl handleNameDecl))
                    {
                        yield return $"/*{handleNameDecl.Name} {parameter.Name}*/ nint";
                    }

                    yield return $"{TypePrinter.GetName(parameter.Type, TypeNameKind.Managed)} /*{parameter.Name}*/";
                }
            }
        }

        public override string ToString()
        {
            var bldr = new StringBuilder( );

            if(IsPointer)
            {
                bldr.Append( "delegate* unmanaged[Cdecl]<" )
                    .Append( string.Join( ',', ManagedParameterTypeNames ) )
                    .Append(',')
                    .Append( ReturnType )
                    .Append( "/*retVal*/>" );
            }
            else
            {
                bldr.Append( ReturnType )
                    .Append( ' ' )
                    .Append( Name )
                    .Append( "( " );

                foreach(string parameter in Parameters)
                {
                    bldr.Append( parameter );
                }

                bldr.AppendLine( " );" );
            }

            return bldr.ToString();
        }

        public FunctionType Signature { get; }

        private static string CreateEscapedIdentifier(Parameter p)
        {
            return CodeDom.CreateEscapedIdentifier( p.Name );
        }

        private IEnumerable<string> GetParameters(List<Parameter> parameters)
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

        private static readonly Microsoft.CSharp.CSharpCodeProvider CodeDom
            = new( new Dictionary<string, string> { [ "CompilerVersion" ] = "v7.3" } );
    }
}

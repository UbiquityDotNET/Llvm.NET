// -----------------------------------------------------------------------
// <copyright file="ASTContextExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Passes;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator
{
    internal static class ASTContextExtensions
    {
        public static IEnumerable<TranslationUnit> GeneratedUnits( this ASTContext ctx )
        {
            return from tu in ctx.TranslationUnits
                   where tu.IsGenerated
                   select tu;
        }

        public static bool IsCoreHeader( this TranslationUnit tu )
        {
            return tu.IsValid && tu.IncludePath != null && tu.FileRelativeDirectory.StartsWith( "llvm-c", StringComparison.Ordinal );
        }

        public static bool IsExtensionHeader( this TranslationUnit tu )
        {
            return !tu.IsCoreHeader() && tu.IsValid && !tu.IsSystemHeader && tu.FileNameWithoutExtension.EndsWith( "Bindings", StringComparison.Ordinal );
        }

        public static IEnumerable<TypedefNameDecl> GetHandleTypeDefs( this ASTContext ctx )
        {
            return from tu in ctx.GeneratedUnits()
                   from td in tu.Typedefs
                   where td.IsHandleTypeDef( )
                   select td;
        }

        public static IEnumerable<Declaration> GetHandleDeclarations( this ASTContext ctx )
        {
            return from td in ctx.GetHandleTypeDefs( )
                   select ( ( TagType )( ( PointerType )td.Type ).Pointee ).Declaration;
        }

        public static bool TryGetHandleDecl( this CppSharp.AST.Type astType, out TypedefNameDecl decl )
        {
            switch( astType )
            {
            // standard form: typedef struct LLVMOpaqueFoo* LLVMFooRef;
            case TypedefType tdt when( tdt.Declaration.IsHandleTypeDef( ) ):
                decl = tdt.Declaration;
                return true;

            // bad form: typedef struct LLVMOpaqueFoo LLVMFoo;
            // found in some APIs (LLVMOpaqueValueMetadataEntry, LLVMOpaqueModuleFlagEntry)
            // the type passed used for params and returns is always a LLVMFoo*
            case PointerType pt when( pt.Pointee is TypedefType tdt && tdt.Declaration.IsHandleTypeDef( ) ):
                decl = tdt.Declaration;
                return true;

            default:
                decl = null;
                return false;
            }
        }

        public static bool IsHandleType( this CppSharp.AST.Type t )
        {
            return t.TryGetHandleDecl( out TypedefNameDecl _ );
        }

        public static bool IsOpaqueStruct( this TagType tt )
        {
            return tt.Declaration is Class c && c.IsOpaque;
        }

        public static bool IsHandleTypeDef( this TypedefNameDecl td )
        {
            if( td.Type is PointerType pt )
            {
                return ( pt.Pointee is TagType tt && tt.IsOpaqueStruct( ) )
                    || ( pt.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Void );
            }

            return td.Type is TagType tt2 && tt2.IsOpaqueStruct( );
        }

        public static DiagnosticKind ConvertToDiagnosticKind( this CppSharp.Parser.ParserDiagnosticLevel level )
        {
            switch( level )
            {
            case CppSharp.Parser.ParserDiagnosticLevel.Ignored:
                return DiagnosticKind.Debug;

            case CppSharp.Parser.ParserDiagnosticLevel.Note:
                return DiagnosticKind.Message;

            case CppSharp.Parser.ParserDiagnosticLevel.Warning:
                return DiagnosticKind.Warning;

            case CppSharp.Parser.ParserDiagnosticLevel.Error:
            case CppSharp.Parser.ParserDiagnosticLevel.Fatal:
                return DiagnosticKind.Error;

            default:
                throw new ArgumentException( "Unknown diagnostic level" );
            }
        }

        public static IEnumerable<TypedefNameDecl> GetAllTypeDefs( this ASTContext ast )
        {
            return from tu in ast.TranslationUnits
                   from td in tu.Typedefs
                   select td;
        }

        public static void StripComments( this ASTContext ctx )
        {
            var q = from tu in ctx.TranslationUnits
                    from decl in tu.Declarations
                    where decl.Comment != null
                    select decl;

            foreach( var decl in q )
            {
                decl.Comment = null;
            }

            var q2 = from tu in ctx.TranslationUnits
                     from e in tu.Enums
                     from item in e.Items
                     where item.Comment != null
                     select item;
            foreach( var item in q2 )
            {
                item.Comment = null;
            }
        }

        public static bool IsDelegate( this TypedefNameDecl td )
        {
            return td.Type is PointerType pt && pt.Pointee is FunctionType;
        }

        public static void RemoveTranslationUnitPass<T>( this BindingContext context )
            where T : TranslationUnitPass
        {
            var genSymbolsPass = context.TranslationUnitPasses.FindPass<T>( );
            if( genSymbolsPass != null )
            {
                context.TranslationUnitPasses.Passes.Remove( genSymbolsPass );
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "It's supposed to be all lowercase" )]
        public static string AsString( this CppSharp.AST.Attribute attr, bool useFullNamespace = false )
        {
            var bldr = new StringBuilder( "[" );
            if(attr is TargetedAttribute ta && ta.Target != AttributeTarget.Default)
            {
                bldr.Append( ta.Target.ToString( ).ToLowerInvariant( ) );
                bldr.Append( ": " );
            }

            bldr.Append( useFullNamespace ? attr.Type.FullName : attr.Type.Name.Substring(0, attr.Type.Name.Length - 9 /*Len(Attribute)*/ ) );
            if( !string.IsNullOrWhiteSpace( attr.Value ) )
            {
                bldr.Append( "( " );
                bldr.Append( attr.Value );
                bldr.Append( " )" );
            }

            bldr.Append( "]" );
            return bldr.ToString( );
        }

        public static bool IsDelegateTypeDef( this TypedefNameDecl td)
        {
            return td.TryGetFunctionSignature( out _ );
        }

        public static bool TryGetFunctionSignature( this TypedefNameDecl td, out FunctionType signature )
        {
            signature = null;
            if( td.Type is PointerType pt && pt.Pointee is FunctionType sig )
            {
                signature = sig;
                return true;
            }

            return false;
        }
    }
}

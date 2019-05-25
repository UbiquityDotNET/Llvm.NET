// -----------------------------------------------------------------------
// <copyright file="ASTContextExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CppSharp.AST;
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
            return !tu.IsCoreHeader()
                && tu.IsValid
                && !tu.IsSystemHeader
                && tu.FileNameWithoutExtension.EndsWith( "Bindings", StringComparison.Ordinal );
        }

        public static IEnumerable<TypedefNameDecl> GetHandleTypeDefs( this ASTContext ctx )
        {
            return from tu in ctx.GeneratedUnits()
                   from td in tu.Typedefs
                   where td.IsHandleTypeDef( )
                   select td;
        }

        public static bool TryGetHandleDecl( this CppSharp.AST.Type astType, out TypedefNameDecl decl )
        {
            switch( astType )
            {
            case TypedefType tdt when( tdt.Declaration.IsHandleTypeDef( ) ):
                decl = tdt.Declaration;
                return true;

            case PointerType pt when( pt.Pointee is TypedefType tdt && tdt.Declaration.IsHandleTypeDef( ) ):
                decl = tdt.Declaration;
                return true;

            default:
                decl = null;
                return false;
            }
        }

        public static bool IsOpaqueStruct( this TagType tt )
        {
            return tt.Declaration is Class c && c.IsOpaque;
        }

        public static bool IsHandleTypeDef( this TypedefNameDecl td )
        {
            return IsCannonicalHandleTypeDef( td ) || IsOpaquHandleTypeDef( td );
        }

        public static bool IsOpaquHandleTypeDef( this TypedefNameDecl td )
        {
            // bad form, declaration is the opaque struct, not a pointer to the struct
            return td.Type is TagType tt2 && tt2.IsOpaqueStruct( );
        }

        public static bool IsCannonicalHandleTypeDef( this TypedefNameDecl td )
        {
            // Canonical form, declaration is a pointer to an opaque struct
            if( !(td.Type is PointerType pt ))
            {
                return false;
            }

            return ( pt.Pointee is TagType tt && tt.IsOpaqueStruct( ) ) || ( pt.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Void );
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

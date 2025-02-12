// -----------------------------------------------------------------------
// <copyright file="ASTContextExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using CppSharp.AST;

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

        public static IEnumerable<TranslationUnit> ValidUnits( this ASTContext ctx )
        {
            return from tu in ctx.TranslationUnits
                   where tu.IsValid
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
                   where td.IsHandleTypeDef()
                   select td;
        }

        public static bool TryGetHandleDecl( this CppSharp.AST.Type astType, out TypedefNameDecl decl )
        {
            switch( astType )
            {
            case TypedefType tdt when( tdt.Declaration.IsHandleTypeDef() ):
                decl = tdt.Declaration;
                return true;

            case PointerType pt when( pt.Pointee is TypedefType tdt && tdt.Declaration.IsHandleTypeDef() ):
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
            return td.Type is TagType tt2 && tt2.IsOpaqueStruct();
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is anything but simplified!" )]
        public static bool IsCannonicalHandleTypeDef( this TypedefNameDecl td )
        {
            // Canonical form, declaration is a pointer to an opaque struct
            if( td.Type is not PointerType pt )
            {
                return false;
            }

            return ( pt.Pointee is TagType tt && tt.IsOpaqueStruct() )
                || ( pt.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Void );
        }

        [SuppressMessage( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "It's supposed to be all lowercase" )]
        [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It is needed, tooling is too stupid to see that..." )]
        public static string AsString( this CppSharp.AST.Attribute attr, bool useFullNamespace = false )
        {
            var bldr = new StringBuilder( "[" );
            if( attr is TargetedAttribute ta && ta.Target != AttributeTarget.Default )
            {
                bldr.Append( ta.Target.ToString().ToLowerInvariant() );
                bldr.Append( ": " );
            }

            bldr.Append( useFullNamespace ? attr.Type.FullName : attr.Type.Name[ 0..^9 ] );
            if( !string.IsNullOrWhiteSpace( attr.Value ) )
            {
                bldr.Append( "( " );
                bldr.Append( attr.Value );
                bldr.Append( " )" );
            }

            bldr.Append( ']' );
            return bldr.ToString();
        }

        public static bool IsDelegateTypeDef( this TypedefNameDecl td )
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

        public static FunctionType GetFunctionPointerType( this TypedefNameDecl td )
        {
            return ( td.Type is PointerType pt && pt.Pointee is FunctionType ft ) ? ft : null;
        }

        public static IReadOnlyDictionary<string, FunctionType> GetFunctionPointers( this ASTContext context )
        {
            return ( from tu in context.TranslationUnits
                     from td in tu.Typedefs
                     let ft = td.GetFunctionPointerType()
                     where ft != null
                     select (td.Name, Signature: ft)
                   ).ToDictionary( item => item.Name, item => item.Signature );
        }
    }
}

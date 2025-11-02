// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
                   where td.IsHandleTypeDef()
                   select td;
        }

        public static bool TryGetHandleDecl( this CppSharp.AST.Type astType, [MaybeNullWhen(false)] out TypedefNameDecl? decl )
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
            return IsCanonicalHandleTypeDef( td ) || IsOpaqueHandleTypeDef( td );
        }

        public static bool IsOpaqueHandleTypeDef( this TypedefNameDecl td )
        {
            // bad form, declaration is the opaque struct, not a pointer to the struct
            return td.Type is TagType tt2 && tt2.IsOpaqueStruct();
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is anything but simplified!" )]
        public static bool IsCanonicalHandleTypeDef( this TypedefNameDecl td )
        {
            // Canonical form, declaration is a pointer to an opaque struct
            if( td.Type is not PointerType pt )
            {
                return false;
            }

            return ( pt.Pointee is TagType tt && tt.IsOpaqueStruct() )
                || ( pt.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Void );
        }

        public static FunctionType? GetFunctionPointerType( this TypedefNameDecl td )
        {
            return ( td.Type is PointerType pt && pt.Pointee is FunctionType ft ) ? ft : null;
        }
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    internal class NativeTypePrinter
        : ITypePrinter
        , ITypeVisitor<string>
    {
        public string ToString( CppSharp.AST.Type type )
        {
            return type.Visit( this );
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "More complicated that way" )]
        public string VisitArrayType( ArrayType array, TypeQualifiers quals )
        {
            if( array.SizeType is ArrayType.ArraySize.Constant or ArrayType.ArraySize.Incomplete )
            {
                return array.Size == 0 ? $"{array.Type}*" : $"{array.QualifiedType}[{array.Size}]";
            }

            throw new NotSupportedException( );
        }

        public string VisitAttributedType( AttributedType attributed, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitBuiltinType( BuiltinType builtin, TypeQualifiers quals )
        {
            return VisitPrimitiveType(builtin.Type, quals);
        }

        public string VisitCILType( CILType type, TypeQualifiers quals )
        {
            return type.Type.ToString();
        }

        public string VisitDecayedType( DecayedType decayed, TypeQualifiers quals )
        {
            return decayed.Decayed.Visit(this);
        }

        public string VisitDeclaration( Declaration decl, TypeQualifiers quals )
        {
            return decl.QualifiedOriginalName;
        }

        public string VisitDependentNameType( DependentNameType dependent, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitDependentTemplateSpecializationType( DependentTemplateSpecializationType template, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitFunctionType( FunctionType function, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitInjectedClassNameType( InjectedClassNameType injected, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitMemberPointerType( MemberPointerType member, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitPackExpansionType( PackExpansionType packExpansionType, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitPointerType( PointerType pointer, TypeQualifiers quals )
        {
            return $"{pointer.QualifiedPointee.Visit(this)}*";
        }

        public string VisitPrimitiveType( PrimitiveType type, TypeQualifiers quals )
        {
            return type switch
            {
                PrimitiveType.Bool => "bool",
                PrimitiveType.Char => "char",
                PrimitiveType.Double => "double",
                PrimitiveType.Float => "float",
                PrimitiveType.Int => "int",
                PrimitiveType.IntPtr => "intptr_t",
                PrimitiveType.Long => "long",
                PrimitiveType.LongLong => "long long",
                PrimitiveType.Short => "short",
                PrimitiveType.UChar => "unsigned char",
                PrimitiveType.UInt => "unsigned int",
                PrimitiveType.UIntPtr => "uintptr_t",
                PrimitiveType.ULong => "unsigned long",
                PrimitiveType.ULongLong => "unsigned long long",
                PrimitiveType.UShort => "unsigned short",
                PrimitiveType.Void => "void",
                PrimitiveType.WideChar => "wchar_t",
                _ => throw new NotSupportedException( ),
            };
        }

        public string VisitQualifiedType( QualifiedType type )
        {
            StringBuilder bldr = new();
            bldr.Append(type.Type.Visit(this));
            if( type.Qualifiers.IsConst )
            {
                bldr.Append("const ");
            }

            if( type.Qualifiers.IsRestrict )
            {
                throw new NotSupportedException("restricted types not allowed; only defined for C99, at best a non-standard extension in C++");
            }

            if(type.Qualifiers.IsVolatile)
            {
                bldr.Append( "volatile " );
            }

            // TODO: figure out what "mode is supposed to be and how to deal with it...
            return bldr.ToString();
        }

        public string VisitTagType( TagType tag, TypeQualifiers quals )
        {
            return VisitDeclaration(tag.Declaration, quals);
        }

        public string VisitTemplateParameterSubstitutionType( TemplateParameterSubstitutionType param, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitTemplateParameterType( TemplateParameterType param, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitTemplateSpecializationType( TemplateSpecializationType template, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitTypedefType( TypedefType typedef, TypeQualifiers quals )
        {
            return typedef.Declaration.OriginalName;
        }

        public string VisitUnaryTransformType( UnaryTransformType unaryTransformType, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitUnresolvedUsingType( UnresolvedUsingType unresolvedUsingType, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitUnsupportedType( UnsupportedType type, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }

        public string VisitVectorType( VectorType vectorType, TypeQualifiers quals )
        {
            throw new NotSupportedException( );
        }
    }
}

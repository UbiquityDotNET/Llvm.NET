// -----------------------------------------------------------------------
// <copyright file="FixInconsistentLLVMHandleDeclarations.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Fix inconsistencies in typedefs for handles in LLVM</summary>
    /// <remarks>
    /// <para>
    /// The canonical pattern is 'typedef struct LLVMOpaqueGoodFoo* LLVMGoodFooRef',
    /// which is used directly as a parameter like: 'void LLVMGoodFooDoSomething(LLVMGoodFooRef foo)'.
    /// Unfortunately not all of the opaque typedefs, follow this pattern. Instead, they
    /// declare the typedef without the pointer 'typedef struct LLVMOpaqueBadFoo LLVMBadFoo'.
    /// This then requires use of the pointer in the signature of the function
    /// ''void LLVMBadFooDoSomething(LLVMBadFoo* foo)'.</para>
    /// <para>This all poses a challenge for code generation that needs to map the handles to a
    /// proper managed type (Like a smart pointer or RAII pattern) since a handle is not as easily
    /// detected and these variances need special handling. Rather than spreading such handling
    /// throughout the transform passes, this handles it all in one place to transform the AST into
    /// the canonical form.
    /// </para>
    /// </remarks>
    internal class FixInconsistentLLVMHandleDeclarations
        : TranslationUnitPass
    {
        public FixInconsistentLLVMHandleDeclarations( )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = true;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = true;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;
        }

        public override bool VisitTypedefDecl( TypedefDecl typedef )
        {
            if( typedef.IsOpaquHandleTypeDef( ) )
            {
                var ptrType = new PointerType( typedef.QualifiedType );
                typedef.QualifiedType = new QualifiedType( ptrType );

                Diagnostics.Debug( "NOTE: Bad form 'REF' declaration for {0} found in LLVM source at {1}@{2}", typedef.Name, typedef.TranslationUnit.FileName, typedef.LineNumberStart );
                RedefinedHandleDeclarations.Add( typedef.Name, new TypedefType( typedef ) );
                return true;
            }

            return base.VisitTypedefDecl( typedef );
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if( !base.VisitFunctionDecl( function ) )
            {
                return false;
            }

            if( TryGetRemappedHandleDecl( function.ReturnType.Type, out TypedefType remappedType ) )
            {
                function.ReturnType = new QualifiedType( remappedType, function.ReturnType.Qualifiers );
                var signature = function.FunctionType.Type as FunctionType;
                signature.ReturnType = function.ReturnType;
            }

            return true;
        }

        public override bool VisitParameterDecl( Parameter parameter )
        {
            if( TryGetRemappedHandleDecl( parameter.Type, out TypedefType decl ) )
            {
                parameter.QualifiedType = new QualifiedType( decl, parameter.QualifiedType.Qualifiers );
            }

            return base.VisitParameterDecl( parameter );
        }

        private bool TryGetRemappedHandleDecl( Type type, out TypedefType decl )
        {
            decl = null;
            return type is PointerType pt
                && pt.Pointee is TypedefType tdt
                && RedefinedHandleDeclarations.TryGetValue( tdt.Declaration.Name, out decl );
        }

        private readonly Dictionary<string, TypedefType> RedefinedHandleDeclarations = new Dictionary<string, TypedefType>();
    }
}

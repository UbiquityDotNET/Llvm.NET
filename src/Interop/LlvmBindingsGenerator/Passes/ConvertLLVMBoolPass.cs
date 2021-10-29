// -----------------------------------------------------------------------
// <copyright file="ConvertLLVMBoolPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to handle the status vs. bool return type problems/ambiguity in LLVM-C APIs</summary>
    /// <remarks>
    /// LLVM-C uses an LLVMBool type for boolean results, however, the meaning of the value (success vs failure) depends on
    /// the actual API used. In some cases success is 0 and any non-zero value is a failure code. Others, a non-zero value is
    /// a literal TRUE (e.g. success) and 0 is FALSE (Failure). The managed projection handles this by using the YAML configuration
    /// to disambiguate the usage. For any real boolean values, the .NET type is System.Boolean, for others, it is LlvmStatus.
    /// This pass handles conversions of the return type to either bool or LlvmStatus depending on the information in the YAML
    /// configuration file.
    /// </remarks>
    internal class ConvertLLVMBoolPass
        : TranslationUnitPass
    {
        public ConvertLLVMBoolPass( IGeneratorConfig config )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = false;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;

            Configuration = config;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            var signature = ( FunctionType )function.FunctionType.Type;
            switch( function.ReturnType.Type )
            {
            // Any function listed in the map is converted regardless of declared return type
            // Some functions use int instead of LLVMBool, apparently to avoid the confusion.
            // Unfortunately since this is not consistent, it creates more confusion.
            case Type _ when IsStatusReturning( function ):
                function.ReturnType = LlvmStatusType;
                signature.ReturnType = function.ReturnType;
                Diagnostics.Debug( "Converted return type of function {0} to LLVMStatus", function.Name );
                break;

            // functions not mapped but having LLVMBool are converted to returning "bool"
            case TypedefType tdt when tdt.Declaration.Name == LLVMBoolTypeName:
                function.Attributes.Add( ReturnBoolAttribute );
                function.ReturnType = BoolType;
                signature.ReturnType = function.ReturnType;
                Diagnostics.Debug( "Converted return type of function {0} to bool", function.Name );
                break;
            }

            foreach( var p in function.Parameters )
            {
                if( ( p.Type is TypedefType tdt2 ) && ( tdt2.Declaration.Name == LLVMBoolTypeName ) )
                {
                    p.QualifiedType = new QualifiedType( new CILType( typeof( bool ) ) );
                    p.Attributes.Add( ParamBoolAttribute );
                    Diagnostics.Debug( "Converted type of function {0} parameter {1}[{2}] to bool", function.Name, p.Name ?? string.Empty, p.Index );
                }
            }

            return true;
        }

        private bool IsStatusReturning( Function function )
        {
            return Configuration.FunctionBindings.TryGetValue( function.Name, out YamlFunctionBinding binding )
                   && binding.ReturnTransform is YamlReturnStatusMarshalInfo;
        }

        private readonly IGeneratorConfig Configuration;
        private static QualifiedType BoolType = new QualifiedType( new CILType( typeof(bool ) ) );
        private static QualifiedType LlvmStatusType =
            new QualifiedType(
                new TypedefType(
                    new TypedefDecl
                    {
                        Name = "LLVMStatus",
                        QualifiedType = new QualifiedType( new BuiltinType( PrimitiveType.Int ) )
                    }
                    )
                );

        private static Attribute ReturnBoolAttribute { get; } = new TargetedAttribute( AttributeTarget.Return, typeof( MarshalAsAttribute ), UnmanagedTypeBool );

        private static Attribute ParamBoolAttribute { get; } = new TargetedAttribute( typeof( MarshalAsAttribute ), UnmanagedTypeBool );

        private const string LLVMBoolTypeName = "LLVMBool";
        private const string UnmanagedTypeBool = "UnmanagedType.Bool";
    }
}

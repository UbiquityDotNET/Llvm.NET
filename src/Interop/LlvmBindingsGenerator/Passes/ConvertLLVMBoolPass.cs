// -----------------------------------------------------------------------
// <copyright file="ConvertLLVMBoolPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Passes
{
    internal class ConvertLLVMBoolPass
        : TranslationUnitPass
    {
        public ConvertLLVMBoolPass( ISet<string> functionNames )
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

            FunctionNames = functionNames;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits() )
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
            // Some functions use int instead of LlvmBool, apparently to avoid the confusion.
            // Unfortunately since this is not consistent, it creates more confusion.
            case Type _ when FunctionNames.Contains( function.Name ):
                function.ReturnType = LlvmStatusType;
                signature.ReturnType = function.ReturnType;
                Diagnostics.Debug( "Converted return type of function {0} to LLVMStatus", function.Name );
                break;

            // functions not mapped but having LlvmBool are converted to returning "bool"
            case TypedefType tdt when tdt.Declaration.Name == LLVMBoolTypeName:
                function.Attributes.Add( ReturnBoolAttribute );
                function.ReturnType = BoolType;
                signature.ReturnType = function.ReturnType;
                Diagnostics.Debug( "Converted return type of function {0} to bool", function.Name );
                break;
            }

            foreach( var p in function.Parameters )
            {
                if( (p.Type is TypedefType tdt2 ) && ( tdt2.Declaration.Name == LLVMBoolTypeName ) )
                {
                    p.QualifiedType = new QualifiedType( new CILType( typeof( bool ) ) );
                    p.Attributes.Add( ParamBoolAttribute );
                    Diagnostics.Debug( "Converted type of function {0} parameter {1}[{2}] to bool", function.Name, p.Name??string.Empty, p.Index );
                }
            }

            return true;
        }

        private readonly ISet<string> FunctionNames;
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

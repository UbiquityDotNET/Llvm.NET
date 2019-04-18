// -----------------------------------------------------------------------
// <copyright file="ConvertLLVMBoolPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
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
            if( function.ReturnType.Type is TypedefType tdt && tdt.Declaration.Name == LLVMBoolTypeName )
            {
                var signature = ( FunctionType )function.FunctionType.Type;
                if( FunctionNames.Contains( function.Name ) )
                {
                    var statusDecl = new TypedefDecl( )
                    {
                        Name = "LLVMStatus",
                        QualifiedType = tdt.Declaration.QualifiedType
                    };
                    function.ReturnType = new QualifiedType( new TypedefType( statusDecl ), function.ReturnType.Qualifiers );
                    signature.ReturnType = function.ReturnType;
                    Diagnostics.Debug( "Converted return type of function {0} to LLVMStatus", function.Name );
                }
                else
                {
                    function.Attributes.Add( ReturnBoolAttribute );
                    function.ReturnType = new QualifiedType( new CILType( typeof( bool ) ) );
                    signature.ReturnType = function.ReturnType;
                    Diagnostics.Debug( "Converted return type of function {0} to bool", function.Name );
                }
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

        private static Attribute ReturnBoolAttribute { get; } = new TargetedAttribute( AttributeTarget.Return, typeof( MarshalAsAttribute ), UnmanagedTypeBool );

        private static Attribute ParamBoolAttribute { get; } = new TargetedAttribute( typeof( MarshalAsAttribute ), UnmanagedTypeBool );

        private readonly ISet<string> FunctionNames;

        private const string LLVMBoolTypeName = "LLVMBool";
        private const string UnmanagedTypeBool = "UnmanagedType.Bool";
    }
}

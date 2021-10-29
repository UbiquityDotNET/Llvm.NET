// -----------------------------------------------------------------------
// <copyright file="AddMissingParameterNamesPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to add valid parameter names to functions declared without a name</summary>
    /// <remarks>
    /// C and C++ allow function declarations with only the type of a parameter. This isn't an option in C# or most
    /// other .NET languages. This pass will create a parameter name like _n where n is the index of the parameter
    /// so that all parameters have a real name usable by .NET projections at least. In the future this might pull
    /// data from the YAML file to find a more self documenting name for any such functions.
    /// </remarks>
    internal class AddMissingParameterNamesPass
        : TranslationUnitPass
    {
        public AddMissingParameterNamesPass( )
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

        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitParameterDecl( Parameter parameter )
        {
            if( string.IsNullOrWhiteSpace( parameter.Name ) )
            {
                parameter.Name = $"_{parameter.Index}";
            }

            return false;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="DeAnonymizeEnumsPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>TranslationUnit pass to support creating named enums from anonymous enums</summary>
    /// <remarks>
    /// C/C++ allow anonymous enums that effectively introduce a const into the parent namespace.
    /// Generally, that's not desired for the interop. In fact the only known use cases for this
    /// in the LLVM-C headers is to handle some FLAGS type enums where the language limits the values
    /// of an enum to the range of an int. So what LLVM does is define the anonymous enum, and then
    /// define a typedef of unsigned to a name for the enum. (see: Core.h\LLVMAttributeIndex as an
    /// example)
    /// </remarks>
    internal class DeAnonymizeEnumsPass
        : TranslationUnitPass
    {
        /// <summary>Initializes a new instance of the <see cref="DeAnonymizeEnumsPass"/> class.</summary>
        /// <param name="firstItemToNameMap">collection of first enum member names and the typedef name associated with the enum</param>
        /// <remarks>
        /// The <paramref name="firstItemToNameMap"/> maps the name of the first element of an anonymous
        /// enum with the name of the typedef used to represent the enumeration. This is used to find the
        /// correct declaration in the AST.
        /// </remarks>
        public DeAnonymizeEnumsPass( IReadOnlyDictionary<string, string> firstItemToNameMap )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = true;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = false;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;

            FirstItemToNameMap = firstItemToNameMap;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitEnumDecl( Enumeration @enum )
        {
            if( !string.IsNullOrWhiteSpace( @enum.Name ) )
            {
                return false;
            }

            if( FirstItemToNameMap.TryGetValue( @enum.Items[ 0 ].Name, out string enumName ) )
            {
                @enum.Name = enumName;
                return true;
            }

            Diagnostics.Error( "Anonymous enum '{0}[{1}]' has no mapping", @enum.TranslationUnit.FileName, @enum.LineNumberStart );
            return false;
        }

        private readonly IReadOnlyDictionary<string,string> FirstItemToNameMap;
    }
}

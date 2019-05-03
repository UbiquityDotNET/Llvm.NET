// -----------------------------------------------------------------------
// <copyright file="ValidateHasStringMarshalingAttributes.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Passes
{
    internal class ValidateHasStringMarshalingAttributes
        : TranslationUnitPass
    {
        public ValidateHasStringMarshalingAttributes( )
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
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if(function.ReturnType.Type.ToString() == "string" )
            {
                bool hasCustomMarshaling = ( from attrib in function.Attributes.OfType<TargetedAttribute>( )
                                             where attrib.Target == AttributeTarget.Return && IsStringMarshalingAttribute( attrib )
                                             select attrib
                                           ).Any( );
                if(!hasCustomMarshaling)
                {
                    Diagnostics.Error( "ERROR: Function {0} has string return type, but does not have string custom marshaling attribute to define marshaling behavior!", function.Name );
                }
            }

            var outStrings = from p in function.Parameters
                             where (p.IsOut || p.IsInOut)
                                && p.Type.ToString( ) == "string"
                                && !p.Attributes.Any( IsStringMarshalingAttribute )
                             select p;

            foreach(var param in outStrings)
            {
                Diagnostics.Error( "ERROR: Parameter {0} of function {1} is an out string but has no custom marshaling attribute to define marshaling behavior", param.Name, function.Name );
            }

            return true;
        }

        private static bool IsStringMarshalingAttribute( CppSharp.AST.Attribute attribute)
        {
            return attribute.Type.Name == "MarshalAsAttribute"
                && attribute.Value.StartsWith( "UnmanagedType.CustomMarshaler", System.StringComparison.InvariantCulture );
        }
    }
}

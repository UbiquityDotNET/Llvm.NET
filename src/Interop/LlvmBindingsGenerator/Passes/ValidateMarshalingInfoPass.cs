﻿// -----------------------------------------------------------------------
// <copyright file="ValidateHasStringMarshalingAttributes.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Passes
{
    internal class ValidateMarshalingInfoPass
        : TranslationUnitPass
    {
        public ValidateMarshalingInfoPass( IGeneratorConfig config )
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

            Config = config;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if( !function.IsGenerated )
            {
                return false;
            }

            if( function.ReturnType.Type is CILType cilType && cilType.Type.Name == "string" )
            {
                bool hasCustomMarshaling = ( from attrib in function.Attributes.OfType<TargetedAttribute>( )
                                             where attrib.Target == AttributeTarget.Return && IsStringMarshalingAttribute( attrib )
                                             select attrib
                                           ).Any( );
                if( !hasCustomMarshaling )
                {
                    Diagnostics.Error( "ERROR: Function '{0}' has string return type, but does not have string custom marshaling attribute to define marshaling behavior!", function.Name );
                }
            }
            else if( function.ReturnType.Type is PointerType pt )
            {
                bool hasMarhsalAsAttrib = ( from attrib in function.Attributes.OfType<TargetedAttribute>( )
                                            where attrib.Target == AttributeTarget.Return && attrib.Type.Name == "MarshalAsAttribute"
                                            select attrib
                                          ).Any( );
                if( !hasMarhsalAsAttrib )
                {
                    bool allowUnsafeReturn = Config.FunctionBindings.TryGetValue( function.Name, out YamlFunctionBinding binding )
                                          && (binding.ReturnTransform?.IsUnsafe ?? false);

                    if( !allowUnsafeReturn )
                    {
                        Diagnostics.Error( "ERROR: Function '{0}' has unsafe return type '{1}', without a marshaling attribute - (Possible missing FunctionBindings entry)"
                                         , function.Name
                                         , pt.ToString( )
                                         );
                    }
                }
            }

            var outStrings = from p in function.Parameters
                             where (p.IsOut || p.IsInOut)
                                && ( p.Type.ToString( ) == "string" || p.Type.ToString() == "sbyte")
                                && !p.Attributes.Any( IsStringMarshalingAttribute )
                             select p;

            foreach( var param in outStrings )
            {
                Diagnostics.Error( "ERROR: Parameter '{0}' of function '{1}' is an out string but has no custom marshaling attribute to define marshaling behavior", param.Name, function.Name );
            }

            // indicate input char* params without marshalling.
            // Generally these are legitimately strings (and there are a LOT of them) so this is a debug diagnostic
            // to use when updating the bindings generation for a new version of LLVM or detecting cases where the
            // signature is wrong.
            var inStrings = from p in function.Parameters
                             where p.IsIn
                                && ( p.Type.ToString( ) == "string" || p.Type.ToString() == "sbyte")
                                && !p.Attributes.Any( a=> IsStringMarshalingAttribute(a) || IsArrayMarshalingAttribute(a) )
                             select p;

            foreach( var param in inStrings )
            {
                Diagnostics.Debug( "NOTE: Parameter '{0}' of function '{1}' is an in pointer but has no custom marshaling attribute to define marshaling behavior, string is assumed", param.Name, function.Name );
            }

            return true;
        }

        private static bool IsStringMarshalingAttribute( CppSharp.AST.Attribute attribute )
        {
            return attribute.Type.Name == "MarshalAsAttribute"
                && attribute.Value.StartsWith( "UnmanagedType.CustomMarshaler", System.StringComparison.InvariantCulture );
        }

        private static bool IsArrayMarshalingAttribute( CppSharp.AST.Attribute attribute )
        {
            return attribute.Type.Name == "MarshalAsAttribute"
                && attribute.Value.StartsWith( "UnmanagedType.LPArray", System.StringComparison.InvariantCulture );
        }

        private readonly IGeneratorConfig Config;
    }
}

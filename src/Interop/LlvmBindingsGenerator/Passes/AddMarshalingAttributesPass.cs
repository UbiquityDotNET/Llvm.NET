﻿// -----------------------------------------------------------------------
// <copyright file="AddMarshalingAttributesPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CppSharp;
using CppSharp.AST;
using CppSharp.AST.Extensions;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to add marshaling attributes to function parameters and return types</summary>
    /// <remarks>
    /// Marshalling attributes needed are determined from the YAML configuration file.
    /// </remarks>
    internal class AddMarshalingAttributesPass
        : TranslationUnitPass
    {
        public AddMarshalingAttributesPass( IGeneratorConfig configuration )
        {
            Configuration = configuration;
        }

        // Add LayoutKind.Sequential to all value types
        public override bool VisitClassDecl( Class @class )
        {
            if( !@class.IsValueType )
            {
                return false;
            }

            // Don't add the attributes if they are already present
            if( !@class.Attributes.Contains( StructLayoutAttr ))
            {
                @class.Attributes.Add( StructLayoutAttr );
            }

            if( !@class.Attributes.Contains( GeneratedCodeAttrib ))
            {
                @class.Attributes.Add( GeneratedCodeAttrib );
            }

            return base.VisitClassDecl( @class );
        }

        public override bool VisitFieldDecl( Field field )
        {
            var (usage, type) = TryAddImplicitMarahalingAttributesForType( field.QualifiedType, field.Attributes );
            if( usage != ParameterUsage.Unknown )
            {
                Diagnostics.Debug( "Converting type of field {0}.{1} to {2}", field.Namespace, field.Name, type);
                field.QualifiedType = new QualifiedType( type, field.QualifiedType.Qualifiers );
            }

            return true;
        }

        // visit delegate types by pushing the type onto the declaration stack
        // as the current scope for parameter handling etc... The typedef for
        // a function pointer has the name (FunctionType has no name )
        public override bool VisitTypedefDecl( TypedefDecl typedef )
        {
            if( typedef.IsDelegateTypeDef( ) )
            {
                DeclarationStack.Push( typedef );
                try
                {
                    return base.VisitTypedefDecl( typedef );
                }
                finally
                {
                    DeclarationStack.Pop( );
                }
            }

            return true;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if( !VisitDeclaration( function ) )
            {
                return false;
            }

            // push function declaration as current scope for parameters and return type
            DeclarationStack.Push( function );
            try
            {
                function.Attributes.Add( SuppressUnmanagedSecAttrib );
                ApplyDllImportAttribute( function );
                QualifiedType returnType = function.ReturnType;

                if( returnType.Type != null )
                {
                    VisitReturnType( ( FunctionType )function.FunctionType.Type );
                }

                foreach( Parameter parameter in function.Parameters )
                {
                    parameter.Visit( this );
                }

                return true;
            }
            finally
            {
                DeclarationStack.Pop( );
            }
        }

        public override bool VisitParameterDecl( Parameter parameter )
        {
            var scope = DeclarationStack.Peek( );

            // mapped settings override any implicit handling
            if( TryGetTransformInfo( scope.Name, parameter.Name, out YamlBindingTransform xform ) )
            {
                ApplyParameterMarshaling( parameter, xform );
            }
            else
            {
                ApplyImplicitParamsUsage( parameter );
            }

            return true;
        }

        public override bool VisitFunctionType( FunctionType function, TypeQualifiers quals )
        {
            if( !VisitType( function, quals ) )
            {
                return false;
            }

            if( function.ReturnType.Type != null )
            {
                VisitReturnType( function );
            }

            if( VisitOptions.VisitFunctionParameters )
            {
                foreach( Parameter parameter in function.Parameters )
                {
                    parameter.Visit( this );
                }
            }

            return true;
        }

        private bool VisitReturnType( FunctionType signature )
        {
            var scope = DeclarationStack.Peek( );
            if( TryGetTransformInfo( scope.Name, out YamlBindingTransform xform ) )
            {
                signature.ReturnType = xform.TransformType( signature.ReturnType );
                scope.Attributes.AddRange( xform.Attributes );
                if( scope is Function f )
                {
                    // sadly the CppSharp AST treats Function.ReturnType distinct from Function.FunctionType.ReturnType.
                    f.ReturnType = signature.ReturnType;
                }
            }

            return signature.ReturnType.Visit( this );
        }

        private static void ApplyImplicitParamsUsage( Parameter p )
        {
            var (usage, type) = TryAddImplicitMarahalingAttributesForType( p.QualifiedType, p.Attributes );
            if( usage != ParameterUsage.Unknown )
            {
                Diagnostics.Debug( "Converting type of parameter {0}::{1} to {2} [Usage: {3}]", p.Namespace, p.Name, type, usage );
                p.Usage = usage;
                p.QualifiedType = new QualifiedType( type, p.QualifiedType.Qualifiers );
                return;
            }

            if( p.Type is PointerType pt )
            {
                switch( pt.Pointee.Desugar( ) )
                {
                // void* => IntPtr
                case BuiltinType bt when bt.Type == PrimitiveType.Void:
                    Diagnostics.Debug( "Converting void* parameter {0}::{1} to IntPtr [In]", p.Namespace, p.Name );
                    p.QualifiedType = new QualifiedType( new CILType( typeof( IntPtr ) ) );
                    break;

                // Pointer to Pointer and Pointer to built in types are out parameters
                default:
                    Diagnostics.Debug( "Setting usage semantics of pointer to pointer parameter {0}::{1} to [Out]", p.Namespace, p.Name );
                    p.Usage = ParameterUsage.Out;
                    break;
                }
            }
        }

        private static (ParameterUsage, CppSharp.AST.Type) TryAddImplicitMarahalingAttributesForType( QualifiedType type, IList<CppSharp.AST.Attribute> attributes )
        {
            // currently only handle strings and arrays of strings by default
            switch( type.Type )
            {
            case PointerType pt when pt.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Char:
                attributes.Add( MarshalAsLPStrAttrib );
                return (ParameterUsage.In, StringType);

            case PointerType pt when pt.Pointee is PointerType pt2 && pt2.Pointee is BuiltinType bt && bt.Type == PrimitiveType.Char:
                attributes.Add( MarshalAsLPStrArrayAttrib );
                return (ParameterUsage.In, StringArrayType);

            default:
                return (ParameterUsage.Unknown, null);
            }
        }

        private static void ApplyParameterMarshaling( Parameter parameter, YamlBindingTransform marshaling )
        {
            parameter.Attributes.AddRange( marshaling.Attributes );
            parameter.QualifiedType = marshaling.TransformType( parameter.QualifiedType );
            parameter.Usage = marshaling.Semantics switch
            {
                ParamSemantics.In => ParameterUsage.In,
                ParamSemantics.Out => ParameterUsage.Out,
                ParamSemantics.InOut => parameter.Type is ArrayType ? ParameterUsage.In : ParameterUsage.InOut,
                _ => throw new InvalidOperationException( ),
            };
        }

        private static void ApplyDllImportAttribute( Function function )
        {
            // add DllImportAttribute
            var pinvokeArgs = new List<string>( ) { "LibraryPath" };
            if( function.CallingConvention == CppSharp.AST.CallingConvention.C )
            {
                pinvokeArgs.Add( "CallingConvention=global::System.Runtime.InteropServices.CallingConvention.Cdecl" );
            }
            else
            {
                Diagnostics.Error( "Function '{0}' has unsupported calling convention '{1}'", function.Name, function.CallingConvention );
            }

            string args = string.Join( ", ", pinvokeArgs );
            function.Attributes.Add( new TargetedAttribute( typeof( DllImportAttribute ), args ) );
        }

        private bool TryGetTransformInfo( string functionName, string paramName, out YamlBindingTransform xform )
        {
            xform = null;
            return Configuration.FunctionBindings.TryGetValue( functionName, out YamlFunctionBinding binding )
                && binding.ParamTransforms.TryGetValue( paramName, out xform );
        }

        private bool TryGetTransformInfo( string name, out YamlBindingTransform xform )
        {
            xform = null;
            if( !Configuration.FunctionBindings.TryGetValue( name, out YamlFunctionBinding binding ) )
            {
                return false;
            }

            if( binding.ReturnTransform == null )
            {
                return false;
            }

            xform = binding.ReturnTransform;
            return true;
        }

        private static CppSharp.AST.Attribute SuppressUnmanagedSecAttrib { get; }
            = new CppSharp.AST.Attribute( )
            {
                Type = typeof( System.Security.SuppressUnmanagedCodeSecurityAttribute ),
                Value = string.Empty
            };

        private readonly IGeneratorConfig Configuration;

        private readonly Stack<Declaration> DeclarationStack = new();

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrAttrib
            = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPStr" );

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrArrayAttrib
            = new TargetedAttribute(typeof(MarshalAsAttribute), "UnmanagedType.LPStr", "ArraySubType = UnmanagedType.LPStr");

        private static readonly CppSharp.AST.Type StringType = new CILType( typeof(string) );
        private static readonly CppSharp.AST.Type StringArrayType = new CILType( typeof(string[]) );
        private static readonly TargetedAttribute StructLayoutAttr = new( typeof(StructLayoutAttribute ), "LayoutKind.Sequential" );
        private static readonly TargetedAttribute GeneratedCodeAttrib
            = new( typeof( GeneratedCodeAttribute )
                 , "\"LlvmBindingsGenerator\""
                 , $"\"{typeof(AddMarshalingAttributesPass).Assembly.GetName( ).Version}\""
                 );
    }
}

// -----------------------------------------------------------------------
// <copyright file="AddMarshalingAttributesPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
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
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Passes
{
    internal class AddMarshalingAttributesPass
        : TranslationUnitPass
    {
        public AddMarshalingAttributesPass( MarshalingInfoMap marshalInfo )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = true;
            VisitOptions.VisitFunctionReturnType = true;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = true;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;

            ParamMarshalingMap = marshalInfo;
        }

        // Add LayoutKind.Sequential to all value types
        public override bool VisitClassDecl( Class @class )
        {
            if( !@class.IsValueType )
            {
                return false;
            }

            @class.Attributes.Add( StructLayoutAttr );
            @class.Attributes.Add( GeneratedCodeAttrib );
            return base.VisitClassDecl( @class );
        }

        public override bool VisitFieldDecl( Field field )
        {
            TryAddImplicitMarahalingAttributesForType( field.QualifiedType, field.Attributes );
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
            if( ParamMarshalingMap.TryGetValue( scope.Name, parameter.Index, out IMarshalInfo marshalInfo ) )
            {
                ApplyParameterMarshaling( parameter, marshalInfo );
            }
            else
            {
                ApplyImplicitParamsUsage( parameter );
            }

            return true;
        }

        private bool VisitReturnType( FunctionType signature )
        {
            var scope = DeclarationStack.Peek( );
            if( ParamMarshalingMap.TryGetValue( scope.Name, MarshalingInfoMap.ReturnParamIndex, out IMarshalInfo marshalInfo ) )
            {
                signature.ReturnType = marshalInfo.TransformType( signature.ReturnType );
                scope.Attributes.AddRange( marshalInfo.Attributes );
                if( scope is Function f )
                {
                    // sadly the CppSharp AST treats Function.ReturnType distinct from Function.FunctionType.ReturnType.
                    f.ReturnType = signature.ReturnType;
                }
            }

            return true;
        }

        private static void ApplyImplicitParamsUsage( Parameter p )
        {
            var (usage, type) = TryAddImplicitMarahalingAttributesForType( p.QualifiedType, p.Attributes );
            if( usage != ParameterUsage.Unknown )
            {
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
                    p.QualifiedType = new QualifiedType( new CILType( typeof( IntPtr ) ) );
                    break;

                // some handle typedefs do not follow the standard typedef patterns 'typedef struct OpaqueFoo* Foo;'
                // and instead use 'typedef struct OpaqueFoo Foo;' (e.g. the pointer is not part of the typedef)
                // this is the sort of inconsistency that drives users insane... Deal with it by treating the single
                // pointer case as an "in" of the handle type to hide the differences from the generated bindings
                case TagType _ when pt.Pointee.TryGetHandleDecl( out TypedefNameDecl handleDecl ):
                    p.Usage = ParameterUsage.In;
                    p.QualifiedType = new QualifiedType( new TypedefType( handleDecl ) );
                    break;

                // Pointer to Pointer and Pointer to built in types are out parameters
                default:
                    p.Usage = ParameterUsage.Out;
                    break;
                }
            }
        }

        private static (ParameterUsage, CppSharp.AST.Type) TryAddImplicitMarahalingAttributesForType( QualifiedType type, IList<CppSharp.AST.Attribute> attributes )
        {
            // currently only handle strings and arrays of strings by default
            switch( type.ToString( ) )
            {
            case "string":
                attributes.Add( MarshalAsLPStrAttrib );
                return (ParameterUsage.In, StringType);

            case "sbyte**":
                attributes.Add( MarshalAsLPStrArrayAttrib );
                return (ParameterUsage.In, StringArrayType);

            default:
                return (ParameterUsage.Unknown, null);
            }
        }

        private static void ApplyParameterMarshaling( Parameter parameter, IMarshalInfo marshaling )
        {
            parameter.Attributes.AddRange( marshaling.Attributes );
            parameter.QualifiedType = marshaling.TransformType( parameter.QualifiedType );
            switch( marshaling.Semantics )
            {
            case ParamSemantics.Return:
                break;

            case ParamSemantics.In:
                parameter.Usage = ParameterUsage.In;
                break;

            case ParamSemantics.Out:
                parameter.Usage = ParameterUsage.Out;
                break;

            case ParamSemantics.InOut:
                parameter.Usage = ParameterUsage.InOut;
                break;

            default:
                throw new InvalidOperationException( );
            }
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

        private static CppSharp.AST.Attribute SuppressUnmanagedSecAttrib { get; }
            = new CppSharp.AST.Attribute( )
            {
                Type = typeof( System.Security.SuppressUnmanagedCodeSecurityAttribute ),
                Value = string.Empty
            };

        private readonly MarshalingInfoMap ParamMarshalingMap;

        private readonly Stack<Declaration> DeclarationStack = new Stack<Declaration>();

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrAttrib
            = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPStr" );

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrArrayAttrib
            = new TargetedAttribute(typeof(MarshalAsAttribute), "UnmanagedType.LPStr", "ArraySubType = UnmanagedType.LPStr");

        private static readonly CppSharp.AST.Type StringType = new CILType( typeof(string) );
        private static readonly CppSharp.AST.Type StringArrayType = new CILType( typeof(string[]) );
        private static readonly TargetedAttribute StructLayoutAttr = new TargetedAttribute( typeof(StructLayoutAttribute ), "LayoutKind.Sequential" );
        private static readonly TargetedAttribute GeneratedCodeAttrib
            = new TargetedAttribute( typeof( GeneratedCodeAttribute )
                                   , "\"LlvmBindingsGenerator\""
                                   , $"\"{typeof(AddMarshalingAttributesPass).Assembly.GetName( ).Version.ToString()}\""
                                   );
    }
}

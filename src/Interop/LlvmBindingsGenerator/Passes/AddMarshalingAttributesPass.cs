// -----------------------------------------------------------------------
// <copyright file="AddMarshalingAttributesPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
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
        public AddMarshalingAttributesPass( IEnumerable<IMarshalInfo> marshalInfo )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = true;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;

            MarshalInfo = marshalInfo;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            // resolve parameter names with actual declarations to get index
            // marshaling info deals with index since, the name is optional for declarations
            Map = new MarshalingInfoMap( context );
            foreach( var mi in MarshalInfo )
            {
                Map.Add( mi );
            }

            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitClassDecl( Class @class )
        {
            if( !@class.IsValueType )
            {
                return false;
            }

            @class.Attributes.Add( new TargetedAttribute( typeof( StructLayoutAttribute ), "LayoutKind.Sequential" ) );
            return base.VisitClassDecl( @class );
        }

        public override bool VisitFieldDecl( Field field )
        {
            TryAddImplicitMarahalingAttributesForType( field.QualifiedType, field.Attributes );
            return true;
        }

        public override bool VisitFunctionType( FunctionType function, TypeQualifiers quals )
        {
            if( function.ReturnType.Type != null )
            {
                VisitReturnType( function );
            }

            foreach( Parameter parameter in function.Parameters )
            {
                parameter.Visit( this );
            }

            return true;
        }

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
            var decl = DeclarationStack.Peek( );

            // mapped settings override any implicit handling
            if( Map.TryGetValue( decl.Name, parameter.Index, out IMarshalInfo marshalInfo ) )
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
            var decl = DeclarationStack.Peek( );
            if( Map.TryGetValue( decl.Name, MarshalingInfoMap.ReturnParamIndex, out IMarshalInfo marshalInfo ) )
            {
                signature.ReturnType = new QualifiedType( marshalInfo.Type, signature.ReturnType.Qualifiers );
                decl.Attributes.AddRange( marshalInfo.Attributes );
                if( decl is Function f )
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
            }

            if( p.Type is PointerType pt && pt.Pointee.Desugar( ) is BuiltinType bt )
            {
                if( bt.Type == PrimitiveType.Void )
                {
                    p.QualifiedType = new QualifiedType( new CILType( typeof( IntPtr ) ) );
                }
                else
                {
                    p.Usage = ParameterUsage.Out;
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
            parameter.QualifiedType = new QualifiedType( marshaling.Type );
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

        private MarshalingInfoMap Map;

        private readonly IEnumerable<IMarshalInfo> MarshalInfo;

        private readonly Stack<Declaration> DeclarationStack = new Stack<Declaration>();

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrAttrib
            = new TargetedAttribute( typeof( MarshalAsAttribute ), "UnmanagedType.LPStr" );

        private static readonly CppSharp.AST.Attribute MarshalAsLPStrArrayAttrib
            = new TargetedAttribute(typeof(MarshalAsAttribute), "UnmanagedType.LPStr", "ArraySubType = UnmanagedType.LPStr");

        private static readonly CppSharp.AST.Type StringType = new CILType( typeof(string) );
        private static readonly CppSharp.AST.Type StringArrayType = new CILType( typeof(string[]) );
    }
}

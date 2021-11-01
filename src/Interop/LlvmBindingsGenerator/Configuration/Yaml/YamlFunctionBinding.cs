// -----------------------------------------------------------------------
// <copyright file="FunctionBinding.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CppSharp;
using CppSharp.AST;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [DebuggerDisplay( "{Name}" )]
    internal class YamlFunctionBinding
        : IYamlNodeLocation
    {
        public string Name { get; set; }

        public bool IsObsolete { get; set; } = false;

        public bool IsExported { get; set; } = true;

        public bool IsProjected { get; set; } = true;

        public string DeprecationMessage { get; set; }

        public YamlBindingTransform ReturnTransform { get; set; }

        public YamlParamBindingCollection ParamTransforms { get; set; } = new YamlParamBindingCollection( );

        [YamlIgnore]
        public Mark Start { get; set; }

        public void ResolveParameterIndeces( ASTContext context )
        {
            var funcPtrMap = context.GetFunctionPointers( );
            foreach( var p in ParamTransforms )
            {
                ResolveParamIndex( context, funcPtrMap, p );
            }
        }

        private void ResolveParamIndex( ASTContext context, IReadOnlyDictionary<string, FunctionType> functionPointerMap, YamlBindingTransform item )
        {
            if( !item.ParameterIndex.HasValue )
            {
                var function = context.FindFunction( Name ).SingleOrDefault( );
                if( function != null )
                {
                    ResolveFunctionParamIndex( function, item );
                }
                else
                {
                    ResolveFunctionTypeParamIndex( functionPointerMap, item );
                }
            }
        }

        private void ResolveFunctionTypeParamIndex( IReadOnlyDictionary<string, FunctionType> functionPointerMap, YamlBindingTransform item )
        {
            if( !functionPointerMap.TryGetValue( item.Name, out FunctionType signature ) )
            {
                Diagnostics.Error( "Function {0} declared in map does not exist in source as a function or delegate", Name );
            }
            else
            {
                var mappedParam = ( from p in signature.Parameters
                                    where p.Name == item.Name
                                    select p
                                  ).SingleOrDefault( );
                if( mappedParam == null )
                {
                    Diagnostics.Error( "Function {0} does not contain a parameter named '{1}' declared in map", Name, item.Name );
                }

                item.ParameterIndex = mappedParam.Index;
            }
        }

        private static void ResolveFunctionParamIndex( Function function, YamlBindingTransform item )
        {
            var mappedParam = ( from p in function.Parameters
                                where p.Name == item.Name
                                select p
                              ).SingleOrDefault( );
            if( mappedParam == null )
            {
                Diagnostics.Error( "Function {0} does not contain a parameter named '{1}' declared in map", function, item.Name );
                return;
            }

            item.ParameterIndex = mappedParam.Index;
        }
    }
}

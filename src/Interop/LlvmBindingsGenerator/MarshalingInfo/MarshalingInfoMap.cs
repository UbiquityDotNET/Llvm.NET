// -----------------------------------------------------------------------
// <copyright file="MarshalingInfoMap.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CppSharp;
using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    internal enum ParamSemantics
    {
        Return, // indicates a function return rather than an actual parameter
        In,
        Out,
        InOut
    }

    internal class MarshalingInfoMap
        : KeyedCollection<string, IMarshalInfo>
    {
        public const string ReturnParamName = "$return";
        public const uint ReturnParamIndex = uint.MaxValue - 1;
        public const uint UnresolvedParamIndex = uint.MaxValue;

        public MarshalingInfoMap( ASTContext context, IEnumerable<IMarshalInfo> marshalInfo )
        {
            FunctionPointerMap = ( from tu in context.TranslationUnits
                                   from td in tu.Typedefs
                                   let pt = td.Type as PointerType
                                   where pt != null
                                   let ft = pt.Pointee as FunctionType
                                   where ft != null
                                   select (td.Name, Signature: ft)
                                 ).ToDictionary( item => item.Name, item => item.Signature );
            Context = context;

            foreach( var mi in marshalInfo )
            {
                Add( mi );
            }
        }

        public bool TryGetValue( string functionName, uint paramIndex, out IMarshalInfo marshalInfo )
        {
            marshalInfo = null;
            string key = GetKeyName( functionName, paramIndex );
            if( Contains( key ) )
            {
                marshalInfo = this[ key ];
                return true;
            }

            return false;
        }

        protected override string GetKeyForItem( IMarshalInfo item )
        {
            return GetKeyName( item.FunctionName, ResolveParamIndex( item ) );
        }

        private static string GetKeyName( string functionName, uint paramIndex )
        {
            return $"{functionName}${paramIndex}";
        }

        private uint ResolveParamIndex( IMarshalInfo item )
        {
            if( item.ParameterIndex == UnresolvedParamIndex )
            {
                if( item.Semantics == ParamSemantics.Return )
                {
                    item.ParameterIndex = ReturnParamIndex;
                }
                else
                {
                    var function = Context.FindFunction( item.FunctionName ).SingleOrDefault( );
                    if( function != null )
                    {
                        ResolveFunctionParamIndex( function, item );
                    }
                    else
                    {
                        ResolveFunctionTypeParamIndex( item );
                    }
                }
            }

            return item.ParameterIndex;
        }

        private void ResolveFunctionTypeParamIndex( IMarshalInfo item )
        {
            if( !FunctionPointerMap.TryGetValue( item.FunctionName, out FunctionType signature ) )
            {
                Diagnostics.Error( "Function {0} declared in map does not exist in source as a function or delegate", item.FunctionName );
            }
            else
            {
                var mappedParam = ( from p in signature.Parameters
                                    where p.Name == item.ParameterName
                                    select p
                                  ).SingleOrDefault( );
                if( mappedParam == null )
                {
                    Diagnostics.Error( "Function {0} does not contain a parameter named '{1}' declared in map", item.FunctionName, item.ParameterName );
                }

                item.ParameterIndex = mappedParam.Index;
            }
        }

        private static void ResolveFunctionParamIndex( Function function, IMarshalInfo item )
        {
            var mappedParam = ( from p in function.Parameters
                                where p.Name == item.ParameterName
                                select p
                              ).SingleOrDefault( );
            if( mappedParam == null )
            {
                Diagnostics.Error( "Function {0} does not contain a parameter named '{1}' declared in map", function, item.ParameterName );
                return;
            }

            item.ParameterIndex = mappedParam.Index;
        }

        private readonly IReadOnlyDictionary<string, FunctionType> FunctionPointerMap;
        private readonly ASTContext Context;
    }
}

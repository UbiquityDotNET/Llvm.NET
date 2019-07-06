// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LlvmBindingsGenerator.Templates;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    internal class YamlConfiguration
    {
        public YamlBindingsCollection FunctionBindings { get; set; } = new YamlBindingsCollection();

        public List<string> IgnoredHeaders { get; set; } = new List<string>( );

        public List<IHandleInfo> HandleMap { get; set; } = new List<IHandleInfo>( );

        public Dictionary<string, string> AnonymousEnums { get; set; }

        public static YamlConfiguration ParseFrom( string path )
        {
            using( var input = File.OpenText( path ) )
            {
                var deserializer = new DeserializerBuilder( )
                                .WithNamingConvention( new PascalCaseNamingConvention( ))
                                .IgnoreUnmatchedProperties()
                                .WithTagMapping("!Status", typeof(YamlReturnStatusMarshalInfo))
                                .WithTagMapping("!String", typeof(YamlStringMarshalInfo))
                                .WithTagMapping("!Primitive", typeof(YamlPrimitiveMarshalInfo))
                                .WithTagMapping("!Array", typeof(YamlArrayMarshalInfo))
                                .WithTagMapping("!Alias", typeof(YamlAliasReturn))
                                .WithTagMapping("!ContextHandle", typeof(YamlContextHandle))
                                .WithTagMapping("!GlobalHandle", typeof(YamlGlobalHandle))
                                .Build( );

                var retVal = deserializer.Deserialize<YamlConfiguration>( input );

                // Force all return transforms to use Return semantics so that
                // transform passes will generate correct attributes for the
                // return value.
                var returnTransforms = from x in retVal.FunctionBindings.Values
                                       where x.ReturnTransform != null
                                       select x.ReturnTransform;
                foreach(YamlBindingTransform xform in returnTransforms)
                {
                    xform.Semantics = ParamSemantics.Return;
                }

                return retVal;
            }
        }

        public HandleTemplateMap BuildTemplateMap()
        {
            var handleTemplates = from h in HandleMap
                                  select Transform(h);

            var retVal = new HandleTemplateMap( )
            {
               new LLVMErrorRefTemplate() // error ref is always present so just add it
            };

            foreach( var h in handleTemplates )
            {
                retVal.Add( h );
            }

            return retVal;
        }

        private static IHandleCodeTemplate Transform( IHandleInfo h )
        {
            switch( h )
            {
            case YamlGlobalHandle ygh:
                return new GlobalHandleTemplate( ygh.HandleName, ygh.Disposer, ygh.Alias );

            case YamlContextHandle ych:
                return new ContextHandleTemplate( ych.HandleName );

            case YamlErrorRef _:
                return new LLVMErrorRefTemplate( );

            default:
                throw new InvalidOperationException( "Unknown handle info kind encountered" );
            }
        }

#if LEGACY_CONFIG
        private YamlFunctionBinding GetOrCreateFunctionBinding( string name )
        {
            if( FunctionBindings.TryGetValue( name, out YamlFunctionBinding binding ) )
            {
                return binding;
            }

            binding = new YamlFunctionBinding( ) { Name = name };
            FunctionBindings.Add( binding );
            return binding;
        }

        private static IHandleInfo ConvertToYamlHandleInfo( IHandleCodeTemplate handleTemplate )
        {
            switch( handleTemplate )
            {
            case GlobalHandleTemplate ght:
                return new YamlGlobalHandle( ) { HandleName = ght.HandleName, Disposer = ght.HandleDisposeFunction, Alias = ght.NeedsAlias };

            case ContextHandleTemplate cht:
                return new YamlContextHandle( ) { HandleName = cht.HandleName };

            case LLVMErrorRefTemplate _:
                return new YamlErrorRef( );

            default:
                throw new ArgumentException( "unknown handle type" );
            }
        }

        private void UpdateBindingMarshaling( YamlFunctionBinding fb, IMarshalInfo mi )
        {
            YamlBindingTransform xform;
            switch( mi )
            {
            case ArrayMarshalInfo ami:
                xform = new YamlArrayMarshalInfo( )
                {
                    Name = ami.ParameterName,
                    Semantics = ami.Semantics,
                    SubType = ami.ElementMarshalType,
                    SizeParam = ami.SizeParam,
                };
                break;

            case PrimitiveTypeMarshalInfo ptmi:
                xform = new YamlPrimitiveMarshalInfo( )
                {
                    Name = ptmi.ParameterName,
                    Kind = ptmi.PrimitiveType,
                    Semantics = ptmi.Semantics,
                };
                break;

            case StringMarshalInfo smi:
                xform = new YamlStringMarshalInfo( )
                {
                    Name = smi.ParameterName,
                    Kind = smi.DisposalKind,
                    Semantics = smi.Semantics,
                };
                break;

            default:
                throw new ArgumentException( "Unknown marshal info" );
            }

            if(mi.Semantics == ParamSemantics.Return )
            {
                fb.ReturnTransform = xform;
            }
            else
            {
                fb.ParamTransforms.Add( xform );
            }
        }
#endif
    }
}

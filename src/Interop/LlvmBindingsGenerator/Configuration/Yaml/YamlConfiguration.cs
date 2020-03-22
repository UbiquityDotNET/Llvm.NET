// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using LlvmBindingsGenerator.Templates;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    internal class YamlConfiguration
    {
        public YamlBindingsCollection FunctionBindings { get; set; } = new YamlBindingsCollection( );

        public List<IncludeRef> IgnoredHeaders { get; set; } = new List<IncludeRef>( );

        public List<IHandleInfo> HandleMap { get; set; } = new List<IHandleInfo>( );

        public Dictionary<string, string> AnonymousEnums { get; set; }

        public static YamlConfiguration ParseFrom( string path )
        {
            using( var input = File.OpenText( path ) )
            {
                var deserializer = new DeserializerBuilder( )
                                  .WithNodeDeserializer( inner => new YamlConfigNodeDeserializer(inner)
                                                       , s => s.InsteadOf<ObjectNodeDeserializer>()
                                                       )
                                  .WithTypeConverter( new IncludeRefConverter())
                                  .WithNamingConvention( PascalCaseNamingConvention.Instance )
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
                foreach( YamlBindingTransform xform in returnTransforms )
                {
                    xform.Semantics = ParamSemantics.Return;
                }

                return retVal;
            }
        }

        public HandleTemplateMap BuildTemplateMap( )
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

        private class IncludeRefConverter
            : IYamlTypeConverter
        {
            public bool Accepts( Type type )
            {
                return type == typeof( IncludeRef );
            }

            public object ReadYaml( IParser parser, Type type )
            {
                var scalarEvent = parser.Consume<Scalar>();
                return new IncludeRef( ) { Path = NormalizePathSep( scalarEvent.Value ), Start = scalarEvent.Start };
            }

            public void WriteYaml( IEmitter emitter, object value, Type type )
            {
                throw new NotSupportedException( );
            }

            internal string NormalizePathSep( string path )
            {
                return path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );
            }
        }

        private class YamlConfigNodeDeserializer
            : INodeDeserializer
        {
            public YamlConfigNodeDeserializer( INodeDeserializer inner )
            {
                Inner = inner;
            }

            public bool Deserialize( IParser reader, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value )
            {
                // System.Diagnostics.Debug.WriteLine( "ExpectedType: {0} @[{1},{2}]", expectedType.Name, reader.Current.Start.Line, reader.Current.Start.Column );
                var start = reader.Current.Start;
                if( Inner.Deserialize( reader, expectedType, nestedObjectDeserializer, out value ) )
                {
                    var ctx = new ValidationContext(value, null, null);
                    Validator.ValidateObject( value, ctx, true );
                    if( value is IYamlConfigLocation node )
                    {
                        node.Start = start;
                    }

                    return true;
                }

                return false;
            }

            private readonly INodeDeserializer Inner;
        }
    }
}

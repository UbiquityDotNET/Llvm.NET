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

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It is necessary, tooling can't agree on the point. (removing it generates a warning)" )]
    internal class YamlConfiguration
    {
        public List<IncludeRef> IgnoredHeaders { get; set; } = new List<IncludeRef>();

        public List<IHandleInfo> HandleMap { get; set; } = new List<IHandleInfo>();

        public static YamlConfiguration ParseFrom( string path )
        {
            using var input = File.OpenText( path );
            var deserializer = new DeserializerBuilder( )
                                  .WithNodeDeserializer( inner => new YamlLocationNodeDeserializer(inner)
                                                       , s => s.InsteadOf<ObjectNodeDeserializer>()
                                                       )
                                  .WithTypeConverter( new IncludeRefConverter())
                                  .WithNamingConvention( PascalCaseNamingConvention.Instance )
                                  .WithTagMapping("!ContextHandle", typeof(YamlContextHandle))
                                  .WithTagMapping("!GlobalHandle", typeof(YamlGlobalHandle))
                                  .Build( );

            return deserializer.Deserialize<YamlConfiguration>( input );
        }

        public ILookup<string, IHandleCodeTemplate> BuildTemplateMap()
        {
            // get all the templates to use for generating the output code
            var handleTemplates = from h in HandleMap
                                  from template in Transforms( h )
                                  select (h.HandleName, template);
            return handleTemplates.ToLookup((p)=>p.HandleName, (p)=>p.template);
        }

        private static IEnumerable<IHandleCodeTemplate> Transforms( IHandleInfo h )
        {
            switch(h)
            {
            case YamlGlobalHandle ygh:
                yield return new GlobalHandleTemplate( ygh.HandleName, ygh.Disposer, ygh.Alias );

                // for aliases treat them like a context handle as they are
                // not owned by the managed code and only reference the native
                // handle via a simple nint. Context Handle template creates
                // a type safe wrapper around the raw 'nint' (as a value type) that
                // does NOT implement IDisposable. (Unlike a SafeHandle)
                if(ygh.Alias)
                {
                    yield return new ContextHandleTemplate( $"{ygh.HandleName}Alias" );
                }

                break;

            case YamlContextHandle ych:
                yield return new ContextHandleTemplate( ych.HandleName );
                break;

            default:
                throw new InvalidOperationException( "Unknown handle info kind encountered" );
            }
        }

        // special converter to ensure runtime platform normalized paths for any include paths in the file
        private class IncludeRefConverter
            : IYamlTypeConverter
        {
            public bool Accepts( Type type )
            {
                return type == typeof( IncludeRef );
            }

            public object ReadYaml( IParser parser, Type type, ObjectDeserializer rootDeserializer)
            {
                var scalarEvent = parser.Consume<Scalar>();
                return new IncludeRef() { Path = NormalizePathSep( scalarEvent.Value ), Start = scalarEvent.Start };
            }

            public void WriteYaml( IEmitter emitter, object? value, Type type, ObjectSerializer serializer )
            {
                throw new NotSupportedException();
            }

            internal static string NormalizePathSep( string path )
            {
                return path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );
            }
        }
    }
}

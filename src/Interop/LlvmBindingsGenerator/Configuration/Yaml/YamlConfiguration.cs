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

        public HandleTemplateMap BuildTemplateMap()
        {
            var handleTemplates = from h in HandleMap
                                  select Transform(h);

            var retVal = new HandleTemplateMap( );

            foreach( var h in handleTemplates )
            {
                retVal.Add( h );
            }

            return retVal;
        }

        private static IHandleCodeTemplate Transform( IHandleInfo h )
        {
            return h switch
            {
                YamlGlobalHandle ygh => new GlobalHandleTemplate( ygh.HandleName, ygh.Disposer, ygh.Alias ),
                YamlContextHandle ych => new ContextHandleTemplate( ych.HandleName ),
                _ => throw new InvalidOperationException( "Unknown handle info kind encountered" ),
            };
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

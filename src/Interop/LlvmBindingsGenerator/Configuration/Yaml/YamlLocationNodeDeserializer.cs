// -----------------------------------------------------------------------
// <copyright file="YamlConfigNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface and class to detect it are a matched pair" )]
    [SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "IDE/Tooling is confused, it is needed" )]
    internal interface IYamlNodeLocation
    {
        Mark Start { get; set; }
    }

    // Wrapper for standard object deserializer that tests for IYamlConfigLocation to add location inforamtion
    // to the deserialized nodes. This allows far better error reporting etc.. when performing a semantic pass
    // on the data. Unfortunately, the owner of YamlDotNet is presently unwilling to consider such a thing in
    // a general manner as part of the library (see: https://github.com/aaubry/YamlDotNet/issues/494).
    internal class YamlLocationNodeDeserializer
        : INodeDeserializer
    {
        public YamlLocationNodeDeserializer(INodeDeserializer inner)
        {
            Inner = inner;
        }

        public bool Deserialize(
            IParser reader,
            Type expectedType,
            Func<IParser, Type, object> nestedObjectDeserializer,
            out object value,
            ObjectDeserializer rootDeserializer
            )
        {
            var start = reader.Current.Start;
            if(Inner.Deserialize( reader, expectedType, nestedObjectDeserializer, out value, rootDeserializer ))
            {
                if(value is IYamlNodeLocation node)
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

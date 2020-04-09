// -----------------------------------------------------------------------
// <copyright file="YamlUnsafeReturn.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using CppSharp.AST;

namespace LlvmBindingsGenerator.Configuration
{
    [SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via de-serialization" )]
    internal class YamlUnsafeReturn
        : YamlBindingTransform
    {
        public YamlUnsafeReturn( )
        {
            IsUnsafe = true;
        }

        public override IEnumerable<Attribute> Attributes => Enumerable.Empty<Attribute>( );
    }
}

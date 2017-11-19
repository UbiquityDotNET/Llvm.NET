// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Kaleidoscope.Grammar;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope
{
    /// <summary>Encapsulates data describing a function signature</summary>
    /// <remarks>
    /// This is used to enable consistent representation when the prototype
    /// is synthesized during code generation (i.e. Anonymous expressions)
    /// </remarks>
    public class Prototype
    {
        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="name">name of the function</param>
        /// <param name="parameters">names of each parameter</param>
        public Prototype( string name, params string[ ] parameters )
            : this( new Identifier( name, default ), parameters.Select( p => new Identifier( p, default ) ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="name"><see cref="Kaleidoscope.Identifier"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Kaleidoscope.Identifier"/>s for the names of each parameter</param>
        public Prototype( Identifier name, IEnumerable<Identifier> parameters )
        {
            Identifier = name;
            Parameters = parameters.ToList();
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="name"><see cref="Kaleidoscope.Identifier"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Kaleidoscope.Identifier"/>s for the names of each parameter</param>
        public Prototype( Identifier name, params Identifier[ ] parameters )
            : this( name, ( IEnumerable<Identifier> )parameters )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="ctx"><see cref="FunctionPrototypeContext"/> to extract parameters an source location information from</param>
        public Prototype( FunctionPrototypeContext ctx )
            : this( new Identifier( ctx.Name, ctx.GetSourceSpan( ) ), ctx.Parameters.Select( i => new Identifier( i.Name, i.Span ) ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="ctx"><see cref="BinaryPrototypeContext"/> to extract parameters an source location information from</param>
        /// <param name="linkageName">name of the operator function as seen by the linker</param>
        public Prototype( BinaryPrototypeContext ctx, string linkageName )
            : this( new Identifier( linkageName, ctx.GetSourceSpan( ) ), ctx.Parameters.Select( i => new Identifier( i.Name, i.Span ) ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="ctx"><see cref="UnaryPrototypeContext"/> to extract parameters an source location information from</param>
        /// <param name="linkageName">name of the operator function as seen by the linker</param>
        public Prototype( UnaryPrototypeContext ctx, string linkageName )
            : this( new Identifier( linkageName, ctx.GetSourceSpan( ) ), ctx.Parameters.Select( i => new Identifier( i.Name, i.Span ) ) )
        {
        }

        public Identifier Identifier { get; }

        public IReadOnlyList<Identifier> Parameters { get; }
    }
}

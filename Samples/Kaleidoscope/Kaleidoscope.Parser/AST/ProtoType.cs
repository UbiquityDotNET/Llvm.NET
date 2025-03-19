// -----------------------------------------------------------------------
// <copyright file="ProtoType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Encapsulates data describing a function signature</summary>
    /// <remarks>
    /// This is used to enable consistent representation when the prototype
    /// is synthesized during code generation (i.e. Anonymous expressions)
    /// </remarks>
    public class Prototype
        : IAstNode
    {
        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name"><see cref="Name"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Name"/>s for the names of each parameter</param>
        public Prototype( SourceSpan location, string name, params ParameterDeclaration[ ] parameters )
            : this( location, name, false, false, parameters )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name"><see cref="Name"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Name"/>s for the names of each parameter</param>
        public Prototype( SourceSpan location, string name, IEnumerable<ParameterDeclaration> parameters )
            : this( location, name, false, false, parameters )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="name">Name of the function</param>
        /// <param name="parameters">Names of each parameter</param>
        /// <remarks>
        /// This version of the constructor is used to create synthetic prototypes that don't
        /// exist within the original source.
        /// </remarks>
        public Prototype( string name, params string[ ] parameters )
            : this( default, name, false, false, parameters.Select( ( n, i ) => new ParameterDeclaration( default, name, i ) ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name">name of the function</param>
        /// <param name="isCompilerGenerated">Indicates if this is a compiler generated prototype</param>
        public Prototype( SourceSpan location, string name, bool isCompilerGenerated )
            : this( location, name, isCompilerGenerated, false, Enumerable.Empty<ParameterDeclaration>( ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name">name of the function</param>
        /// <param name="isCompilerGenerated">Indicates if this is a compiler generated prototype</param>
        /// <param name="isExtern">Indicates if this is an external prototype</param>
        /// <param name="parameters">names of each parameter</param>
        public Prototype( SourceSpan location, string name, bool isCompilerGenerated, bool isExtern, IEnumerable<ParameterDeclaration> parameters )
        {
            Location = location;
            Name = name;
            Parameters = parameters.ToImmutableArray( );
            IsCompilerGenerated = isCompilerGenerated;
            IsExtern = isExtern;
        }

        /// <inheritdoc/>
        public SourceSpan Location { get; }

        /// <summary>Gets the name of the function</summary>
        public string Name { get; }

        /// <summary>Gets a value indicating whether the function prototype is an extern declaration</summary>
        public bool IsExtern { get; }

        /// <summary>Gets a value indicating whether the function prototype is generated internally by compiler</summary>
        public bool IsCompilerGenerated { get; }

        /// <summary>Gets the parameters for the function</summary>
        public IReadOnlyList<ParameterDeclaration> Parameters { get; }

        /// <inheritdoc/>
        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit( this );
        }

        /// <inheritdoc/>
        public virtual TResult? Accept<TResult, TArg>(IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : class
            where TArg : struct, allows ref struct
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public IEnumerable<IAstNode> Children => Parameters;

        /// <inheritdoc/>
        public override string ToString( )
        {
            var bldr = new StringBuilder( );
            if( IsExtern )
            {
                bldr.Append( "[extern]" );
            }

            if( IsCompilerGenerated )
            {
                bldr.Append( "[CompilerGenerated]" );
            }

            bldr.Append( Name );
            bldr.Append( '(' );
            if( Parameters.Count > 0 )
            {
                bldr.Append( string.Join( ", ", Parameters.Select( p => p.ToString( ) ) ) );
            }

            bldr.Append( ')' );
            return bldr.ToString( );
        }
    }
}

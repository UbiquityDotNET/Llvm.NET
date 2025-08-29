// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Encapsulates data describing a function signature</summary>
    /// <remarks>
    /// This is used to enable consistent representation when the prototype
    /// is synthesized during code generation (i.e. Anonymous expressions)
    /// </remarks>
    public sealed class Prototype
        : AstNode
        , IAstNode
    {
        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name"><see cref="Name"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Name"/>s for the names of each parameter</param>
        public Prototype( SourceRange location, string name, params ParameterDeclaration[] parameters )
            : this( location, name, false, false, parameters )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name"><see cref="Name"/> containing the name of the function</param>
        /// <param name="parameters">Collection of <see cref="Name"/>s for the names of each parameter</param>
        public Prototype( SourceRange location, string name, IEnumerable<ParameterDeclaration> parameters )
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
        public Prototype( string name, params string[] parameters )
            : this( default, name, false, false, parameters.Select( ( n, i ) => new ParameterDeclaration( default, name, i ) ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name">name of the function</param>
        /// <param name="isCompilerGenerated">Indicates if this is a compiler generated prototype</param>
        public Prototype( SourceRange location, string name, bool isCompilerGenerated )
            : this( location, name, isCompilerGenerated, false, [] )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Prototype"/> class.</summary>
        /// <param name="location">Source location covering the complete signature</param>
        /// <param name="name">name of the function</param>
        /// <param name="isCompilerGenerated">Indicates if this is a compiler generated prototype</param>
        /// <param name="isExtern">Indicates if this is an external prototype</param>
        /// <param name="parameters">names of each parameter</param>
        public Prototype(
            SourceRange location,
            string name,
            bool isCompilerGenerated,
            bool isExtern,
            IEnumerable<ParameterDeclaration> parameters
            )
            : base( location )
        {
            Name = name;
            Parameters = [ .. parameters ];
            IsCompilerGenerated = isCompilerGenerated;
            IsExtern = isExtern;
        }

        /// <summary>Gets the name of the function</summary>
        public string Name { get; }

        /// <summary>Gets a value indicating whether the function prototype is an extern declaration</summary>
        public bool IsExtern { get; }

        /// <summary>Gets a value indicating whether the function prototype is generated internally by compiler</summary>
        public bool IsCompilerGenerated { get; }

        /// <summary>Gets the parameters for the function</summary>
        public IReadOnlyList<ParameterDeclaration> Parameters { get; }

        public override TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult> klsVisitor
                   ? klsVisitor.Visit( this )
                   : visitor.Visit( this );
        }

        public override TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TResult : default
        {
            return visitor is IKaleidoscopeAstVisitor<TResult, TArg> klsVisitor
                   ? klsVisitor.Visit( this, in arg )
                   : visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public override IEnumerable<IAstNode> Children => Parameters;

        /// <inheritdoc/>
        public override string ToString( )
        {
            var bldr = new StringBuilder( );
            if(IsExtern)
            {
                bldr.Append( "[extern]" );
            }

            if(IsCompilerGenerated)
            {
                bldr.Append( "[CompilerGenerated]" );
            }

            bldr.Append( Name );
            bldr.Append( '(' );
            if(Parameters.Count > 0)
            {
                bldr.Append( string.Join( ", ", Parameters.Select( p => p.ToString() ) ) );
            }

            bldr.Append( ')' );
            return bldr.ToString();
        }
    }
}

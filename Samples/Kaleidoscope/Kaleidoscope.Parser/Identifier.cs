// <copyright file="PrototypeCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Kaleidoscope.Grammar;

namespace Kaleidoscope
{
    /// <summary>Description of an identifier in Kaleidoscope code generation</summary>
    /// <remarks>
    /// This is distinct from the parse tree nodes as code generation sometimes needs to
    /// synthesize an identifier (i.e. Building the function definition for an anonymous
    /// top level expression)
    /// </remarks>
    public struct Identifier
        : IEquatable<Identifier>
    {
        public Identifier( string name )
            : this( name, default )
        {
        }

        public Identifier( string name, SourceSpan span )
        {
            Name = name;
            Span = span;
        }

        public Identifier( IToken context )
            : this( context.Text, context.GetSourceSpan() )
        {
        }

        public string Name { get; }

        public SourceSpan Span { get; }

        public override bool Equals( object obj )
        {
            return obj is Identifier && Equals( ( Identifier )obj );
        }

        public bool Equals( Identifier other )
        {
            return Name == other.Name
                && Span.Equals( other.Span );
        }

        public override int GetHashCode( )
        {
            int hashCode = -875978281;
            hashCode = ( hashCode * -1521134295 ) + base.GetHashCode( );
            hashCode = ( hashCode * -1521134295 ) + EqualityComparer<string>.Default.GetHashCode( Name );
            hashCode = ( hashCode * -1521134295 ) + EqualityComparer<SourceSpan>.Default.GetHashCode( Span );
            return hashCode;
        }

        public static bool operator ==( Identifier identifier1, Identifier identifier2 ) => identifier1.Equals( identifier2 );

        public static bool operator !=( Identifier identifier1, Identifier identifier2 ) => !( identifier1 == identifier2 );
    }
}

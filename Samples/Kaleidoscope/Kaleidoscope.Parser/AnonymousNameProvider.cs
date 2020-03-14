// -----------------------------------------------------------------------
// <copyright file="AnonymousNameProvider.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kaleidoscope.Grammar
{
    /// <summary>Provides anonymous names as a sequence of strings</summary>
    /// <remarks>Each call to <see cref="GetEnumerator"/> starts a new sequence</remarks>
    [SuppressMessage( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "This isn't a collection, shouldn't be named as one" )]
    public class AnonymousNameProvider
        : IEnumerable<string>
    {
        /// <summary>Initializes a new instance of the <see cref="AnonymousNameProvider"/> class.</summary>
        /// <param name="prefix">Prefix to use for the name</param>
        public AnonymousNameProvider( string prefix )
        {
            NamePrefix = prefix;
        }

        public IEnumerator<string> GetEnumerator( )
        {
            int anonymousIndex = 0;
            while( true )
            {
                yield return $"{NamePrefix}{anonymousIndex++}";
            }

            // ReSharper disable once IteratorNeverReturns
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        private readonly string NamePrefix;
    }
}

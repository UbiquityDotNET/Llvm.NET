// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Provides anonymous names as a sequence of strings with an enumerated integral suffix</summary>
    /// <remarks>
    /// <para>Each call to <see cref="GetEnumerator"/> starts a new sequence. This is generally used to provide
    /// a name for a compiler generated function with module level visibility.</para>
    /// <note type="important">Each resulting enumerator is conceptually infinite. However, since an unsigned
    /// integer (<see cref="UInt64"/>) is used as the value it will wrap around to the beginning again. Thus,
    /// the values are not guaranteed unique even from a single instance. Though this is rarely an issue in
    /// practical circumstances.
    /// </note>
    /// </remarks>
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

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator( )
        {
            UInt64 anonymousIndex = 0;
            while(true)
            {
                yield return $"{NamePrefix}{anonymousIndex++}";
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator();
        }

        private readonly string NamePrefix;
    }
}

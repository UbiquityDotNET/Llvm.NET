// -----------------------------------------------------------------------
// <copyright file="size_t.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>CLR equivalent to the C/C++ architecture specific size_t</summary>
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Generated code relies on this to match C++" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Matching native Interop type" )]
    [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Matching native Interop type" )]
    public readonly record struct size_t
    {
        /// <summary>Gets a 0 size value</summary>
        public static size_t Value { get; } = new(0);

        private size_t(nint size)
        {
            Size = size;
        }

        internal readonly nint Size;

        public int CompareTo(object? obj) => ((IComparable)Size).CompareTo( obj );
    }
}

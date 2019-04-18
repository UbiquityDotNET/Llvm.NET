// -----------------------------------------------------------------------
// <copyright file="size_t.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Llvm.NET.Interop
{
    /// <summary>CLLR equivalent to the C/C++ architecture specific size_t</summary>
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Generated code relies on this to match C++" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed." )]
    internal struct size_t
    {
        public static explicit operator size_t( int size ) => new size_t( ( IntPtr )size );

        public static implicit operator int( size_t size ) => size.Pointer.ToInt32( );

        public static implicit operator long( size_t size ) => size.Pointer.ToInt64( );

        internal size_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }
}

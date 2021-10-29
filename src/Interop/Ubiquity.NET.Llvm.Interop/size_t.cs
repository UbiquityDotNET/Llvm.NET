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
    public struct size_t
        : IEquatable<size_t>
    {
        /// <inheritdoc/>
        public bool Equals( size_t other )
        {
            return Pointer.Equals( other.Pointer );
        }

        /// <inheritdoc/>
        public override bool Equals( object obj )
        {
            return Pointer.Equals( obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return Pointer.GetHashCode( );
        }

        /// <summary>Equality operator</summary>
        /// <param name="left">Left hand side of comparison</param>
        /// <param name="right">Right hand side of comparison</param>
        /// <returns>Result of comparison</returns>
        public static bool operator ==( size_t left, size_t right ) => left.Equals( right );

        /// <summary>Inequality operator</summary>
        /// <param name="left">Left hand side of comparison</param>
        /// <param name="right">Right hand side of comparison</param>
        /// <returns>Result of comparison</returns>
        public static bool operator !=( size_t left, size_t right ) => !( left == right );

        /// <summary>Converts a size to an int</summary>
        /// <returns>Size as an int</returns>
        public int ToInt32( ) => Pointer.ToInt32( );

        /// <summary>Converts the size to an Int64</summary>
        /// <returns>Size as an Int64</returns>
        public long ToInt64( ) => Pointer.ToInt64( );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int32"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromInt32( int size ) => new size_t( ( IntPtr )size );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int64"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromInt64( long size ) => new size_t( ( IntPtr )size );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int32"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromUInt32( uint size ) => new size_t( ( IntPtr )size );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int64"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromUInt64( ulong size ) => new size_t( ( IntPtr )size );

        /// <summary>Converts an int to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t( int size ) => FromInt32( size );

        /// <summary>Converts a <see cref="System.Int64"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t( long size ) => FromInt64( size );

        /// <summary>Converts a <see cref="System.UInt32"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t( uint size ) => FromUInt32( size );

        /// <summary>Converts a <see cref="System.Int64"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t( ulong size ) => FromUInt64( size );

        /// <summary>Converts the size to a <see cref="System.Int32"/></summary>
        /// <returns>Size as an Int32</returns>
        /// <param name="size">size to convert</param>
        public static implicit operator int( size_t size ) => size.ToInt32( );

        /// <summary>Converts the size to a <see cref="System.Int64"/></summary>
        /// <returns>Size as an Int64</returns>
        /// <param name="size">size to convert</param>
        public static implicit operator long( size_t size ) => size.ToInt64( );

        /// <summary>Gets a 0 size value</summary>
        public static size_t Zero { get; } = FromInt32( 0 );

        internal size_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }
}

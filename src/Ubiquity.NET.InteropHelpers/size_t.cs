// -----------------------------------------------------------------------
// <copyright file="size_t.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.InteropHelpers
{
    // TODO: This should have the same interfaces as System.IntPtr and implemented through the native pointer
    // If C#/.NET had the concept of a proper typedef that's what this would be...

    /// <summary>CLR equivalent to the C/C++ architecture specific size_t</summary>
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Generated code relies on this to match C++" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Matching native Interop type" )]
    [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Matching native Interop type" )]
    public readonly record struct size_t
        : IComparable<size_t>
    {
        /// <summary>Converts a size to an int</summary>
        /// <returns>Size as an int</returns>
        /// <exception cref="OverflowException"> if the size is not representable in a 32 bit value</exception>
        public Int32 ToInt32() => checked((Int32)Size.ToUInt32());

        /// <summary>Converts the size to an Int64</summary>
        /// <returns>Size as an Int64</returns>
        public Int64 ToInt64() => checked((Int64)Size.ToUInt64());

        /// <summary>Converts the size to an UInt64</summary>
        /// <returns>Size as an UInt64</returns>
        public UInt64 ToUInt64() => Size.ToUInt64();

        /// <inheritdoc/>
        public int CompareTo(size_t other) => Size.CompareTo(other.Size);

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int32"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromInt32(int size) => new( checked((nuint)(Int64)size) );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int64"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromInt64(Int64 size) => new( checked((nuint)size) );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.Int32"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromUInt32(UInt32 size) => new( (nuint)size );

        /// <summary>Create a <see cref="size_t"/> from an <see cref="System.UInt64"/></summary>
        /// <param name="size">value to convert</param>
        /// <returns><paramref name="size"/> as a size_t</returns>
        public static size_t FromUInt64(UInt64 size) => new( (nuint)size );

        /// <summary>Converts a <see cref="System.Int32"/> int to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t(Int32 size) => FromInt32( size );

        /// <summary>Converts a <see cref="System.Int64"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t(Int64 size) => FromInt64( size );

        /// <summary>Converts a <see cref="System.UInt32"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t(UInt32 size) => FromUInt32( size );

        /// <summary>Converts a <see cref="System.UInt64"/> to a <see cref="size_t"/></summary>
        /// <param name="size">Value to convert</param>
        public static implicit operator size_t(UInt64 size) => FromUInt64( size );

        /// <summary>Converts the size to a <see cref="System.Int32"/></summary>
        /// <returns>Size as an Int32</returns>
        /// <param name="size">size to convert</param>
        public static implicit operator int(size_t size) => size.ToInt32();

        /// <summary>Converts the size to a <see cref="System.Int64"/></summary>
        /// <returns>Size as an Int64</returns>
        /// <param name="size">size to convert</param>
        public static implicit operator long(size_t size) => size.ToInt64();

        /// <summary>Gets a 0 size value</summary>
        public static size_t Zero { get; } = new(0);

        private size_t(nuint size)
        {
            Size = size;
        }

        internal readonly nuint Size;
    }
}

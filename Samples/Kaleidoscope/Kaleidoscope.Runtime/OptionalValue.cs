// -----------------------------------------------------------------------
// <copyright file="OptionalValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Kaleidoscope.Runtime
{
    /// <summary>Simple Value type that provides for null safe optional generic operations</summary>
    /// <typeparam name="T">Type of value</typeparam>
    /// <remarks>
    /// This is used in place of trying to add nullability attributes to <see cref="System.ValueTuple"/>
    /// as that's not allowed. The default constructor is used for cases when there is no value.
    /// </remarks>
    public struct OptionalValue<T>
        : IEquatable<OptionalValue<T>>
    {
        /// <summary>Initializes a new instance of the <see cref="OptionalValue{T}"/> struct with a valid value.</summary>
        /// <param name="value">Value to store. For reference types this may not be <see langword="null"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This constructor is used to initialize a new instance and validate that, for reference types, the provided
        /// value is not <see langword="null"/>. This is used to provide the guarantee that, for reference types,
        /// <see cref="Value"/> is not <see langword="null"/> when <see cref="HasValue"/> is <see langword="true"/>.
        /// And, conversely, that <see cref="Value"/> is <see langword="null"/> whenever <see cref="HasValue"/> is
        /// <see langword="false"/>.
        /// </remarks>
        public OptionalValue( T value )
        {
            HasValue = true;

            // caller indicates a value, but didn't provide one => error
            if( value is null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            Value = value!;
        }

        /// <summary>Gets a value indicating whether <see cref="Value"/> is valid and, for reference types, is not <see langword="null"/></summary>
        public bool HasValue { get; }

        /// <summary>Gets the value, which, for reference types, may be null if, and only, if <see cref="HasValue"/> is <see langword="false"/></summary>
        [MaybeNull]
        public T Value { get; }

        public override bool Equals( object? obj )
        {
            return obj is OptionalValue<T> other && Equals( other );
        }

        public override int GetHashCode( )
        {
            return HasValue ? Value!.GetHashCode( ) : 0;
        }

        public static bool operator ==( OptionalValue<T> left, OptionalValue<T> right ) => left.Equals( right );

        public static bool operator !=( OptionalValue<T> left, OptionalValue<T> right ) => !( left == right );

        public bool Equals( OptionalValue<T> other )
        {
            return HasValue
                && other.HasValue
                && Value!.Equals( other.Value );
        }

        public void Deconstruct( out bool hasValue, [MaybeNull] out T value)
        {
            hasValue = HasValue;
            value = Value;
        }
    }

    public static class OptionalValue
    {
        public static OptionalValue<T> Create<T>( T value )
        {
            return new OptionalValue<T>( value );
        }
    }
}

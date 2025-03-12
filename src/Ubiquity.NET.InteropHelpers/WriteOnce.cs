// -----------------------------------------------------------------------
// <copyright file="WriteOnce.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Ubiquity.NET.InteropHelpers.Properties;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Wrapper class to provide Write-Once semantics to a value</summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    /// <remarks>
    /// This provides write once semantics for fields that may require initialization
    /// outside the context of a constructor, but once set should never be set again.
    /// Allowing a sort of lazy <see langword="readonly"/>.
    /// </remarks>
    [DebuggerDisplay( "{" + nameof( ValueOrDefault ) + "}" )]
    public struct WriteOnce<T>
        : IEquatable<T>
    {
        /// <summary>Initializes a new instance of the <see cref="WriteOnce{T}"/> struct.</summary>
        public WriteOnce()
        {
        }

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            return (HasValue && ValueOrDefault is not null)
                 ? Convert.ToString( ValueOrDefault, CultureInfo.CurrentCulture ) ?? string.Empty
                 : string.Empty;
        }

        /// <summary>Throws an exception if this instance is already set</summary>
        /// <exception cref="InvalidOperationException"><see cref="HasValue"/> is <see langword="true"/></exception>
        public readonly void ThrowIfHasValue()
        {
            if(HasValue)
            {
                throw new InvalidOperationException( Resources.Value_already_set );
            }
        }

        /// <summary>Throws an exception if this instance was NOT already set</summary>
        /// <exception cref="InvalidOperationException"><see cref="HasValue"/> is <see langword="false"/></exception>
        public readonly void ThrowIfNotSet()
        {
            if(HasValue)
            {
                throw new InvalidOperationException( Resources.Value_not_set );
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// For a <see cref="WriteOnce{T}"/> that remains unset, the result of equality checks against <typeparamref name="T"/>
        /// will ALWAYS return <see langword="false"/>. That is, an unknow/unset value will NEVER compare <see langword="true"/>
        /// with any value of <typeparamref name="T"/> including <see langword="null"/> if <typeparamref name="T"/> is a nullable type.
        /// </remarks>
        public readonly bool Equals(T? other) => ValueOrDefault?.Equals(other) ?? false;

        /// <inheritdoc/>
        /// <inheritdoc cref="Equals(T?)" path="/remarks"/>
        public override readonly bool Equals(object? obj) => obj is WriteOnce<T> other && Equals(other);

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            if (!HasValue)
            {
                Debug.Assert(HasValue, "Value not assigned. GetHashCode() of unwritten value is not supported");
                return 0;
            }

            return Value!.GetHashCode();
        }

        /// <summary>Gets or sets the value for this instance</summary>
        /// <exception cref="InvalidOperationException">Setting the value when a value is already set</exception>
        /// <inheritdoc cref="ValueOrDefault" path="/remarks"/>
        [MaybeNull]
        public T Value
        {
            readonly get => ValueOrDefault;

            set
            {
                ThrowIfHasValue();
                ValueOrDefault = value;
                HasValue = true;
            }
        }

        /// <summary>Gets a value indicating whether the <see cref="Value"/> was set or not</summary>
        public bool HasValue { get; private set; }

        /// <summary>Gets the current value or the default value for <typeparamref name="T"/> if not yet set</summary>
        /// <remarks>
        /// The value retrieved from this property may be <see langword="null"/> IFF <typeparamref name="T"/>
        /// is a reference type That is, the default value of <typeparamref name="T"/> is returned for any
        /// values not yet set. Do NOT rely on the value retrieved from this property to determine if it is
        /// set or not. Always use <see cref="HasValue"/> to test if this instance is set to a non-default
        /// value.
        ///
        /// <note type="important">It is possible, and explicitly allowed, that the value is set to <see langword="null"/>
        /// if <typeparamref name="T"/> is a nullable type! Therefore, there is no way to know if the value is set
        /// or not without looking at the <see cref="HasValue"/> property.</note>
        /// </remarks>
        [MaybeNull]
        public T ValueOrDefault { get; private set; }

        /// <summary>Convenience implicit cast as a wrapper around the <see cref="ValueOrDefault"/> property</summary>
        /// <param name="value"> <see cref="WriteOnce{T}"/> instance to extract a value from</param>
        /// <remarks>
        /// The result of the cast may be <see langword="null"/> IFF the default value of <typeparamref name="T"/> is
        /// <see langword="null"/>.
        /// </remarks>
        [return: MaybeNull]
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "See Value and ValueOrDefault" )]
        public static implicit operator T(WriteOnce<T> value) => value.ValueOrDefault;

        public static bool operator ==(WriteOnce<T> left, WriteOnce<T> right) => left.Equals( right );

        public static bool operator !=(WriteOnce<T> left, WriteOnce<T> right) => !(left == right);
    }
}

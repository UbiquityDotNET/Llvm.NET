// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#if !NET7_0_OR_GREATER

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// NOTE: Custom Exception messages are NOT translatable resources (en-us only)

namespace System
{
    /// <summary>polyfill extensions for static methods added in .NET 7</summary>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "BS, Extension" )]
    [SuppressMessage( "Naming", "CA1708:Identifiers should differ by more than case", Justification = "BS, Extension (Don't even have visibility into what it's complaining about" )]
    public static class PolyFillExtensions
    {
        extension(ArgumentNullException ex)
        {
            public static void ThrowIfNull(
                [NotNull] object? argument,
                [CallerArgumentExpression(nameof(argument))] string? paramName = default
                )
            {
                if(argument is null)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }

        extension(ArgumentException ex)
        {
            public static void ThrowIfNullOrWhiteSpace(
                [NotNull] string? argument,
                [CallerArgumentExpression(nameof(argument))] string? paramName = default
                )
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);
                if(string.IsNullOrWhiteSpace(argument))
                {
                    throw new ArgumentException("string parameter must not be null or whitespace", paramName);
                }
            }
        }

        extension(ArgumentOutOfRangeException ex)
        {
            // without support for static abstract these aren't an option
            // Best alternate is rolling out type specific variants based on usage...
            // BCL for .NET 7, where `static abstract` was introduced (non-preview), already includes these

            ///// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.</summary>
            ///// <param name="value">The argument to validate as non-zero.</param>
            ///// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            //public static void ThrowIfZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
            //    where T : INumberBase<T>
            //{
            //    if (T.IsZero(value))
            //        ThrowZero(value, paramName);
            //}

            ///// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
            ///// <param name="value">The argument to validate as non-negative.</param>
            ///// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            //public static void ThrowIfNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
            //    where T : INumberBase<T>
            //{
            //    if (T.IsNegative(value))
            //        ThrowNegative(value, paramName);
            //}

            ///// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
            ///// <param name="value">The argument to validate as non-zero or non-negative.</param>
            ///// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            //public static void ThrowIfNegativeOrZero<T>(T value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
            //    where T : INumberBase<T>
            //{
            //     if (T.IsNegative(value) || T.IsZero(value))
            //         ThrowNegativeOrZero(value, paramName);
            //}

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IEquatable<T>?
            {
                if (EqualityComparer<T>.Default.Equals(value, other))
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must not be equal to '{other}'.");
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as equal to <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IEquatable<T>?
            {
                if (!EqualityComparer<T>.Default.Equals(value, other))
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must be equal to '{other}'.");
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as less or equal than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IComparable<T>
            {
                if (value.CompareTo(other) > 0)
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must be less than or equal to '{other}'.");
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as less than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IComparable<T>
            {
                if (value.CompareTo(other) >= 0)
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must be less than '{other}'.");
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as greater than or equal to <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IComparable<T>
            {
                if (value.CompareTo(other) < 0)
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must be greater than or equal to '{other}'.");
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as greater than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
                where T : IComparable<T>
            {
                if (value.CompareTo(other) <= 0)
                {
                    throw new ArgumentOutOfRangeException(paramName, $"{nameof(value)} ('{value}') must be greater than '{other}'.");
                }
            }
        }
    }
}
#endif

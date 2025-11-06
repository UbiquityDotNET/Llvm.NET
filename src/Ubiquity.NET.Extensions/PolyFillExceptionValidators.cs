// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// .NET 7 added the various exception static methods for parameter validation
// This will back fill them for earlier versions.
//
// NOTE: C #14 extension keyword support is required to make this work.
#if !NET7_0_OR_GREATER

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace System
{
    /// <summary>poly fill extensions for static methods added in .NET 7</summary>
    /// <remarks>
    /// This requires support of the C#14 keyword `extension` to work properly. There is
    /// no other way to add static methods to non-partial types for source compatibility.
    /// Otherwise code cannot use the modern .NET runtime implementations and instead
    /// must always use some extension methods, or litter around a LOT of #if/#else/#endif
    /// based on the framework version...
    /// </remarks>
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "Extension - Broken analyzer" )]
    [SuppressMessage( "Naming", "CA1708:Identifiers should differ by more than case", Justification = "Extension - broken analyzer" )]
    public static class PolyFillExceptionValidators
    {
        /// <summary>Poly fill Extensions for <see cref="ArgumentException"/></summary>
        extension( ArgumentException )
        {
            /// <summary>Throw an <see cref="ArgumentException"/> if a string is <see langword="null"/>m empty, or all whitepsace.</summary>
            /// <param name="argument">input string to test</param>
            /// <param name="paramName">expression or name of the string to test; normally provided by compiler</param>
            /// <exception cref="ArgumentException">string is <see langword="null"/>m empty, or all whitepsace</exception>
            public static void ThrowIfNullOrWhiteSpace(
                [NotNull] string? argument,
                [CallerArgumentExpression( nameof( argument ) )] string? paramName = null
                )
            {
                ArgumentNullException.ThrowIfNull(argument, paramName);

                // argument is non-null verified by this, sadly older frameworks don't have
                // attributes to declare that.
                if(string.IsNullOrWhiteSpace( argument ))
                {
                    throw new ArgumentException( SR.Argument_EmptyOrWhiteSpaceString, paramName );
                }
            }
        }

        /// <summary>Poly fill Extensions for <see cref="ArgumentNullException"/></summary>
        extension( ArgumentNullException )
        {
            /// <summary>Throws an aexception if the tested argument is <see langword="null"/></summary>
            /// <param name="argument">value to test</param>
            /// <param name="paramName">expression for the name of the value; normally provided by compiler</param>
            /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/></exception>
            public static void ThrowIfNull(
                [NotNull] object? argument,
                [CallerArgumentExpression( nameof( argument ) )] string? paramName = default
                )
            {
                if(argument is null)
                {
                    throw new ArgumentNullException( paramName );
                }
            }
        }

        /// <summary>Poly fill Extensions for <see cref="ObjectDisposedException"/></summary>
        extension( ObjectDisposedException )
        {
            /// <summary>Throws an <see cref="ObjectDisposedException"/> if <paramref name="condition"/> is <see langword="true"/>.</summary>
            /// <param name="condition">Condition to determine if the instance is disposed</param>
            /// <param name="instance">instance that is tested; Used to get type name for exception</param>
            /// <exception cref="ObjectDisposedException"><paramref name="condition"/> is <see langword="true"/></exception>
            public static void ThrowIf(
                [DoesNotReturnIf( true )] bool condition,
                object instance
                )
            {
                if(condition)
                {
                    throw new ObjectDisposedException( instance?.GetType().FullName );
                }
            }
        }

        /// <summary>Poly fill Extensions for <see cref="ObjectDisposedException"/></summary>
        extension( ArgumentOutOfRangeException )
        {
            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfEqual<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IEquatable<T>?
            {
                if(EqualityComparer<T>.Default.Equals( value, other ))
                {
                    ThrowEqual( value, other, paramName );
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as equal to <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfNotEqual<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IEquatable<T>?
            {
                if(!EqualityComparer<T>.Default.Equals( value, other ))
                {
                    ThrowNotEqual( value, other, paramName );
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as less or equal than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfGreaterThan<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IComparable<T>
            {
                if(value.CompareTo( other ) > 0)
                {
                    ThrowGreater( value, other, paramName );
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as less than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfGreaterThanOrEqual<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IComparable<T>
            {
                if(value.CompareTo( other ) >= 0)
                {
                    ThrowGreaterEqual( value, other, paramName );
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as greatar than or equal than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfLessThan<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IComparable<T>
            {
                if(value.CompareTo( other ) < 0)
                {
                    ThrowLess( value, other, paramName );
                }
            }

            /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal <paramref name="other"/>.</summary>
            /// <param name="value">The argument to validate as greatar than than <paramref name="other"/>.</param>
            /// <param name="other">The value to compare with <paramref name="value"/>.</param>
            /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
            public static void ThrowIfLessThanOrEqual<T>( T value, T other, [CallerArgumentExpression( nameof( value ) )] string? paramName = null )
                where T : IComparable<T>
            {
                if(value.CompareTo( other ) <= 0)
                {
                    ThrowLessEqual( value, other, paramName );
                }
            }

            [DoesNotReturn]
            private static void ThrowZero<T>( T value, string? paramName ) =>
                        throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeNonZero, paramName, value ) );

            [DoesNotReturn]
            private static void ThrowNegative<T>( T value, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeNonNegative, paramName, value ) );

            [DoesNotReturn]
            private static void ThrowNegativeOrZero<T>( T value, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeNonNegativeNonZero, paramName, value ) );

            [DoesNotReturn]
            private static void ThrowGreater<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeLessOrEqual, paramName, value, other ) );

            [DoesNotReturn]
            private static void ThrowGreaterEqual<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeLess, paramName, value, other ) );

            [DoesNotReturn]
            private static void ThrowLess<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeGreaterOrEqual, paramName, value, other ) );

            [DoesNotReturn]
            private static void ThrowLessEqual<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeGreater, paramName, value, other ) );

            [DoesNotReturn]
            private static void ThrowEqual<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeNotEqual, paramName, (object?)value ?? "null", (object?)other ?? "null" ) );

            [DoesNotReturn]
            private static void ThrowNotEqual<T>( T value, T other, string? paramName ) =>
                throw new ArgumentOutOfRangeException( paramName, value, SR.Format( SR.ArgumentOutOfRange_Generic_MustBeEqual, paramName, (object?)value ?? "null", (object?)other ?? "null" ) );
        }
    }
}
#endif

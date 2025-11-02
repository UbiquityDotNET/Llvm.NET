// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly, marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be supressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx) don't support the new syntax yet and it isn't clear if they will in the future.
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

    /// <summary>Extension class to provide Fluent validation of arguments</summary>
    /// <remarks>
    /// These are similar to many of the built-in support checks except that
    /// they use a `Fluent' style to allow validation of parameters used as inputs
    /// to other functions that ultimately produce parameters for a base constructor.
    /// They also serve to provide validation when using body expressions for property
    /// method implementations etc... Though the C# 14 <c>field</c> keyword makes that
    /// use mostly a moot point.
    /// </remarks>
    public static class FluentValidationExtensions
    {
        /// <summary>Throws an exception if <paramref name="self"/> is <see langword="null"/></summary>
        /// <typeparam name="T">Type of reference parameter to test for</typeparam>
        /// <param name="self">Instance to test</param>
        /// <param name="exp">Name or expression of the value in <paramref name="self"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="self"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/></exception>
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>( [NotNull] this T? self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
        {
            ArgumentNullException.ThrowIfNull( self, exp );

            return self;
        }

        /// <summary>Throws an exception if an argument is outside of a given (Inclusive) range</summary>
        /// <typeparam name="T">Type of value to test</typeparam>
        /// <param name="self">Value to test</param>
        /// <param name="min">Minimum value allowed for <paramref name="self"/></param>
        /// <param name="max">Maximum value allowed for <paramref name="self"/></param>
        /// <param name="exp">Name or expression of the value in <paramref name="self"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="self"/></returns>
        [DebuggerStepThrough]
        public static T ThrowIfOutOfRange<T>( this T self, T min, T max, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
            where T : struct, IComparable<T>
        {
            ArgumentNullException.ThrowIfNull(self, exp);
            ArgumentOutOfRangeException.ThrowIfLessThan( self, min, exp );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( self, max, exp );

            return self;
        }

        /// <summary>Tests if an enum is defined or not</summary>
        /// <typeparam name="T">Type of value to test</typeparam>
        /// <param name="self">Value to test</param>
        /// <param name="exp">Name or expression of the value in <paramref name="self"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="self"/></returns>
        /// <exception cref="InvalidEnumArgumentException">The enumerated value is not defined</exception>
        /// <remarks>
        /// This is useful to prevent callers from playing tricks with casts, etc... to land with a value
        /// that is otherwise undefined. Note: This is mostly useless on an enumeration marked with
        /// <see cref="FlagsAttribute"/> as a legit value that is a combination of flags does not have
        /// a defined value (Only single bit values do)
        /// </remarks>
        [DebuggerStepThrough]
        public static T ThrowIfNotDefined<T>( this T self, [CallerArgumentExpression( nameof( self ) )] string? exp = null )
            where T : struct, Enum
        {
            exp ??= string.Empty;

            if(Enum.IsDefined( typeof(T), self ))
            {
                return self;
            }

            try
            {
                int underlyingValue = (int)Convert.ChangeType(self, typeof(int), CultureInfo.InvariantCulture);
                throw new InvalidEnumArgumentException( exp, underlyingValue, typeof( T ) );
            }
            catch(Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
            {
                // InvalidEnumArgumentException constructors ONLY provide paramater name value set for values
                // that are representable as an int. Thus, anything else requires a custom message that at
                // least includes the original value in question. (Normally an enum does fit an int, but for
                // interop might not) the resulting exception will have "ParamName" as the default of "null"!
                //
                // TODO: Move the exception message to a resource for globalization
                // This matches the overloaded constructor version but allows for reporting enums with non-int underlying type.
                throw new InvalidEnumArgumentException( $"The value of argument '{exp}' ({self}) is invalid for Enum of type '{typeof( T )}'" );
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="FluentValidators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Extension class to provide Fluent validation of arguments</summary>
    /// <remarks>
    /// These are similar to many of the built-in support checks except that
    /// they use a `Fluent' style to allow validation of parameters used as inputs
    /// to other functions that ultimately produce parameters for a base constructor.
    /// They also serve to provide validation when using body expressions for property
    /// method implementations etc...
    /// </remarks>
    public static class ValidationExtensions
    {
        /// <summary>Throws an exception if <paramref name="obj"/> is <see langword="null"/></summary>
        /// <typeparam name="T">Type of reference parameter to test for</typeparam>
        /// <param name="obj">Instance to test</param>
        /// <param name="exp">Name or expression of the value in <paramref name="obj"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="obj"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/></exception>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T? obj, [CallerArgumentExpression(nameof(obj))] string? exp = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, exp);
            return obj;
        }

        /// <summary>Throws an exception if an argument is outside of a given (Inclusive) range</summary>
        /// <typeparam name="T">Type of value to test</typeparam>
        /// <param name="self">Value to test</param>
        /// <param name="min">Minimum value allowed for <paramref name="self"/></param>
        /// <param name="max">Maximum value allowed for <paramref name="self"/></param>
        /// <param name="exp">Name or expression of the value in <paramref name="self"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="self"/></returns>
        public static T ThrowIfOutOfRange<T>(this T self, T min, T max, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, IComparable<T>
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(self, min, exp);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(self, max, exp);
            return self;
        }

        /// <summary>Tests if an enum is defined or not</summary>
        /// <typeparam name="T">Type of value to test</typeparam>
        /// <param name="self">Value to test</param>
        /// <param name="exp">Name or expression of the value in <paramref name="self"/> [Default: provided by compiler]</param>
        /// <returns><paramref name="self"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">The enumerated value is not defined</exception>
        /// <remarks>
        /// This is useful to prevent callers from playing tricks with casts, etc... to land with a value
        /// that is otherwise undefined. Note: This is mostly useless on an enumeration marked with
        /// <see cref="FlagsAttribute"/> as a legit value that is a combination of flags does not have
        /// a defined value (Only single bit values do)
        /// </remarks>
        public static T ThrowIfNotDefined<T>(this T self, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, Enum
        {
            return Enum.IsDefined<T>(self) ? self : throw new ArgumentOutOfRangeException(exp);
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Internal, only used here, keeps compiler happy" )]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}

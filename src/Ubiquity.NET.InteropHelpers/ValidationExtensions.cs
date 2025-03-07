// -----------------------------------------------------------------------
// <copyright file="FluentValidators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Ubiquity.NET.InteropHelpers
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class ValidatedNotNullAttribute : Attribute {}

    public static class ValidationExtensions
    {
        public static T ThrowIfNull<T>([ValidatedNotNull] this T? obj, [CallerArgumentExpression(nameof(obj))] string? exp = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, exp);
            return obj;
        }

        public static T ThrowIfOutOfRange<T>(this T self, T min, T max, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, IComparable<T>
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(self, min, exp);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(self, max, exp);
            return self;
        }

        public static T ThrowIfNotDefined<T>(this T self, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, Enum
        {
            return Enum.IsDefined<T>(self) ? self : throw new ArgumentOutOfRangeException(exp);
        }
    }
}

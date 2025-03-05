// -----------------------------------------------------------------------
// <copyright file="FluentValidators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Ubiquity.NET.Llvm
{
    internal static class ValidationExtensions
    {
        public static T ThrowIfNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string? exp = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, exp);
            return obj;
        }

        public static void ThrowIfOutOfRange<T>(this T self, T min, T max, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, IComparable<T>
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(self, min, exp);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(self, max, exp);
        }

        public static T ThrowIfNotDefined<T>(this T self, [CallerArgumentExpression(nameof(self))] string? exp = null)
            where T : struct, Enum
        {
            return Enum.IsDefined<T>(self) ? self : throw new ArgumentOutOfRangeException(exp);
        }
    }
}

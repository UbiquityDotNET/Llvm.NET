// -----------------------------------------------------------------------
// <copyright file="FluentValidators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Kaleidoscope.Grammar
{
    internal static class FluentValidators
    {
        public static T ThrowIfNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string? exp = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(obj, exp);
            return obj;
        }
    }
}

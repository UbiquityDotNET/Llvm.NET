// -----------------------------------------------------------------------
// <copyright file="FluentValidators.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Attribute to mark parameters as validated not null in a method</summary>
    /// <remarks>
    /// This is ordinarily applied to protected methods that are available to derived types. This indicates
    /// that the protected implementation will validate the parameter and the derived type need not do so.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}

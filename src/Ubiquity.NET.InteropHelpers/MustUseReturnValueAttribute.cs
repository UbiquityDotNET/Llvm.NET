// -----------------------------------------------------------------------
// <copyright file="MustUseReturnValueAttribute.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>`MustUseRetVal` analyzer compatible attribute</summary>
    /// <seealso href="https://www.nuget.org/packages/MustUseRetVal"/>
    /// <seealso href="https://github.com/mykolav/must-use-ret-val-fs"/>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class MustUseReturnValueAttribute
        : Attribute
    {
    }
}

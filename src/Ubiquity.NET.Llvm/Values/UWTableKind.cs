// -----------------------------------------------------------------------
// <copyright file="AttributeKindExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Enumeration for values of the 'uwtable' attribute</summary>
    public enum UWTableKind
    {
        /// <summary>Default [invalid] value</summary>
        None,

        /// <summary>Generate table for synchronous unwind</summary>
        Sync,

        /// <summary>Generate table for asynchronous unwind</summary>
        Async
    }
}

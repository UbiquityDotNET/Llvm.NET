// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

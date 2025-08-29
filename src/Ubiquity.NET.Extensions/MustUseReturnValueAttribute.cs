// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

namespace Ubiquity.NET.Extensions
{
    /// <summary>`MustUseRetVal` analyzer compatible attribute</summary>
    /// <seealso href="https://www.nuget.org/packages/MustUseRetVal"/>
    /// <seealso href="https://github.com/mykolav/must-use-ret-val-fs"/>
    [AttributeUsage( AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct )]
    public sealed class MustUseReturnValueAttribute
        : Attribute
    {
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Static class to hold constant strings for well known operand bundle tag names</summary>
    public static class OperandBundleNames
    {
        /// <summary>Name of the well-known deopt operand bundle</summary>
        public const string DeOpt = "deopt";

        /// <summary>Name of the well-known funclet operand bundle</summary>
        public const string Funclet = "funclet";

        /// <summary>Name of the well-known gc-transition operand bundle</summary>
        public const string GcTransition = "gc-transition";
    }
}

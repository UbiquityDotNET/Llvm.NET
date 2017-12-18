// <copyright file="OperandBundleNames.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET
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

// <copyright file="TargetDependentDetails.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET;
using Llvm.NET.Values;

namespace TestDebugInfo
{
    internal interface ITargetDependentDetails
    {
        string ShortName { get; }

        string Triple { get; }

        string Cpu { get; }

        string Features { get; }

        IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx );

        void AddABIAttributesForByValueStructure( Function function, int paramIndex );

        void AddModuleFlags( BitcodeModule module );
    }
}

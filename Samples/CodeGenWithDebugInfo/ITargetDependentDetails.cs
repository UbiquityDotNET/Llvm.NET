﻿// <copyright file="TargetDependentDetails.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET;
using Llvm.NET.Values;

namespace TestDebugInfo
{
    #region ITargetDependentDetails
    internal interface ITargetDependentDetails
    {
        string ShortName { get; }

        TargetMachine TargetMachine { get; }

        IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx );

        void AddABIAttributesForByValueStructure( IrFunction function, int paramIndex );

        void AddModuleFlags( BitcodeModule module );
    }
    #endregion
}

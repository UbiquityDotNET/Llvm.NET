// -----------------------------------------------------------------------
// <copyright file="ITargetDependentDetails.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Values;

namespace CodeGenWithDebugInfo
{
    #region ITargetDependentDetails
    internal interface ITargetDependentDetails
    {
        string ShortName { get; }

        TargetMachine TargetMachine { get; }

        IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx );

        void AddABIAttributesForByValueStructure( Function function, int paramIndex );

        void AddModuleFlags( BitcodeModule module );
    }
    #endregion
}

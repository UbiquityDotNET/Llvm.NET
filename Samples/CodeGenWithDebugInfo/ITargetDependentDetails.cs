// <copyright file="TargetDependentDetails.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET;
using Llvm.NET.Values;

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs

namespace TestDebugInfo
{
    // <ITargetDependentDetails>
    internal interface ITargetDependentDetails
    {
        string ShortName { get; }

        TargetMachine TargetMachine { get; }

        IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx );

        void AddABIAttributesForByValueStructure( Function function, int paramIndex );

        void AddModuleFlags( BitcodeModule module );
    }
    // </ITargetDependentDetails>
}

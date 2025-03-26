// -----------------------------------------------------------------------
// <copyright file="ITargetABI.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Values;

namespace CodeGenWithDebugInfo
{
    #region ITargetABI
    /// <summary>Simplistic interface for target specific ABI</summary>
    /// <remarks>
    /// This is NOT a generalized ABI representation - just one that covers the
    /// needs of this generator. Sadly LLVM does not have any sort of abstraction
    /// on the target dependent ABI. That's left to the source generator even though
    /// they all need to effectively implement the same thing. (There is some discussion
    /// about moving the Clang ABI implementations into the LLVM core itself as a
    /// generalized abstraction. That is NOT yet a feature of LLVM)
    /// </remarks>
    /// <seealso href="https://discourse.llvm.org/t/llvm-introduce-an-abi-lowering-library/84554"/>
    internal interface ITargetABI
        : IDisposable
    {
        string ShortName { get; }

        ImmutableArray<AttributeValue> BuildTargetDependentFunctionAttributes( IContext ctx );

        void AddAttributesForByValueStructure( Function function, int paramIndex );

        void AddModuleFlags( Module module );

        TargetMachine CreateTargetMachine();
    }
    #endregion
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.DebugInfo;
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
    {
        string ShortName { get; }

        ImmutableArray<AttributeValue> BuildTargetDependentFunctionAttributes( IContext ctx );

        // NOTE: The debug form of the signature is needed to know the type of the pointer in the native sig
        // Since the native sign rebuilds the types from information provided by the native LLVM code
        // the element type of any pointers is lost (It's ALWAYS OPAQUE now..). While it is theoretically
        // plausible to rebuild the debug information if available that's a lot of work not implemented
        // in the LLVM libraries (especially when it is more useful to keep track of such things in the
        // generating app)
        void AddAttributesForByValueStructure( Function function, DebugFunctionType debugSig, int paramIndex );

        void AddModuleFlags( Module module );

        TargetMachine CreateTargetMachine( );
    }
    #endregion
}

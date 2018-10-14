// <copyright file="ValueKind.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

// SA1515  Single-line comment should be preceded by blank line
#pragma warning disable SA1515

namespace Llvm.NET.Native
{
    internal enum ValueKind : uint
    {
        Function,              // This is an instance of Function
        GlobalAlias,           // This is an instance of GlobalAlias
        GlobalIFunc,           // Global Indirect Function (derived from GlobalIndirectSymbol)
        GlobalVariable,        // This is an instance of GlobalVariable
        BlockAddress,          // This is an instance of BlockAddress
        ConstantExpr,          // This is an instance of ConstantExpr
        ConstantArray,         // This is an instance of ConstantArray
        ConstantStruct,        // This is an instance of ConstantStruct
        ConstantVector,        // This is an instance of ConstantVector

        UndefValue,            // This is an instance of UndefValue
        ConstantAggregateZero, // This is an instance of ConstantAggregateZero
        ConstantDataArray,     // This is an instance of ConstantDataArray
        ConstantDataVector,    // This is an instance of ConstantDataVector
        ConstantInt,           // This is an instance of ConstantInt
        ConstantFP,            // This is an instance of ConstantFP
        ConstantPointerNull,   // This is an instance of ConstantPointerNull
        ConstantTokenNone,     // This is an instance of ConstantTokenNone

        Argument,              // This is an instance of Argument
        BasicBlock,            // This is an instance of BasicBlock

        MetadataAsValue,       // This is an instance of MetadataAsValue
        InlineAsm,             // This is an instance of InlineAsm
        MemoryUse,             // ???
        MemoryDef,             // ???
        MemoryPhi,             // ???

        Instruction,           // This is an instance of Instruction
                               // Enum values starting at InstructionVal are used for Instructions;

        // instruction values come directly from LLVM Instruction.def which is different from the "stable"
        // LLVM-C API, therefore they are less "stable" and bound to the C++ implementation version and
        // subject to change from version to version.
        Return = 1 + Instruction, // Terminators
        Branch = 2 + Instruction,
        Switch = 3 + Instruction,
        IndirectBranch = 4 + Instruction,
        Invoke = 5 + Instruction,
        Resume = 6 + Instruction,
        Unreachable = 7 + Instruction,
        CleanUpReturn = 8 + Instruction,
        CatchReturn = 9 + Instruction,
        CatchSwitch = 10 + Instruction,

        Add = 11 + Instruction, // binary operators
        FAdd = 12 + Instruction,
        Sub = 13 + Instruction,
        FSub = 14 + Instruction,
        Mul = 15 + Instruction,
        FMul = 16 + Instruction,
        UDiv = 17 + Instruction,
        SDiv = 18 + Instruction,
        FDiv = 19 + Instruction,
        URem = 20 + Instruction,
        SRem = 21 + Instruction,
        FRem = 22 + Instruction,

        Shl = 23 + Instruction, // Logical Operators
        LShr = 24 + Instruction,
        AShr = 25 + Instruction,
        And = 26 + Instruction,
        Or = 27 + Instruction,
        Xor = 28 + Instruction,

        Alloca = 29 + Instruction, // Memory Operators
        Load = 30 + Instruction,
        Store = 31 + Instruction,
        GetElementPtr = 32 + Instruction,
        Fence = 33 + Instruction,
        AtomicCmpXchg = 34 + Instruction,
        AtomicRMW = 35 + Instruction,

        Trunc = 36 + Instruction, // cast/conversion operators
        ZeroExtend = 37 + Instruction,
        SignExtend = 38 + Instruction,
        FPToUI = 39 + Instruction,
        FPToSI = 40 + Instruction,
        UIToFP = 41 + Instruction,
        SIToFP = 42 + Instruction,
        FPTrunc = 43 + Instruction,
        FPExt = 44 + Instruction,
        PtrToInt = 45 + Instruction,
        IntToPtr = 46 + Instruction,
        BitCast = 47 + Instruction,
        AddrSpaceCast = 48 + Instruction,

        CleanupPad = 49 + Instruction, // New Exception pads
        CatchPad = 50 + Instruction,

        ICmp = 51 + Instruction,
        FCmp = 52 + Instruction,
        Phi = 53 + Instruction,
        Call = 54 + Instruction,
        Select = 55 + Instruction,
        UserOp1 = 56 + Instruction,
        UserOp2 = 57 + Instruction,
        VaArg = 58 + Instruction,
        ExtractElement = 59 + Instruction,
        InsertElement = 60 + Instruction,
        ShuffleVector = 61 + Instruction,
        ExtractValue = 62 + Instruction,
        InsertValue = 63 + Instruction,
        LandingPad = 64 + Instruction,

        // Markers:
        ConstantFirstVal = Function,
        ConstantLastVal = ConstantTokenNone,

        ConstantDataFirstVal = UndefValue,
        ConstantDataLastVal = ConstantTokenNone,
        ConstantAggregateFirstVal = ConstantArray,
        ConstantAggregateLastVal = ConstantVector
    }

    internal static partial class NativeMethods
    {
        // retrieves the raw underlying native C++ ValueKind enumeration for a value
        // This is generally only used in the mapping of an LLVMValueRef to the Llvm.NET
        // instance wrapping it. Since the Stable C API uses a distinct enum for the
        // instruction codes, they don't actually match the underlying C++ kind and
        // actually overlap it in incompatible ways. So, this uses the underlying enum
        // to build up the correct .NET types for a given LLVMValueRef.
        internal static ValueKind LLVMGetValueIdAsKind( LLVMValueRef valueRef ) => ( ValueKind )LLVMGetValueID( valueRef );
    }
}

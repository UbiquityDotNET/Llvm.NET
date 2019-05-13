// <copyright file="ValueKind.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

// SA1515 - Single-line comment should be preceded by blank line
// CS1591 - Missing XML comment for publicly visible type or member 'Foo'
#pragma warning disable SA1515,CS1591

using System.Diagnostics.CodeAnalysis;

namespace Llvm.NET.Interop
{
    /// <summary>Enum to match the underlying raw LLVM Value kind enumeration</summary>
    /// <remarks>
    /// In order to map from an arbitrary LLVMValueRef to a wrapped managed type the
    /// exact kind of value must be known. However, the LLVM-C API uses a value kind
    /// that is not complete and doesn't match the underlying enum values. (It includes
    /// complex transforms to ensure the 'C' API is "stable". This, causes problems for
    /// managed projections like Llvm.NET. So as an extension to LLVM-V the LibLLVM library
    /// supports a method to get at the underlying value.
    /// <note type="Important">
    /// It is important to note that the enumerated values here are subject to change with
    /// every version of LLVM. Generally, this is only intended for use within the object model
    /// projection of Llvm.NET.
    /// </note>
    /// </remarks>
    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "This is not a flags enum")]
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Documented by LLVM")]
    public enum ValueKind
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

        FNeg = 11 + Instruction, // unary operators

        Add = 12 + Instruction, // binary operators
        FAdd = 13 + Instruction,
        Sub = 14 + Instruction,
        FSub = 15 + Instruction,
        Mul = 16 + Instruction,
        FMul = 17 + Instruction,
        UDiv = 18 + Instruction,
        SDiv = 19 + Instruction,
        FDiv = 20 + Instruction,
        URem = 21 + Instruction,
        SRem = 22 + Instruction,
        FRem = 23 + Instruction,

        Shl = 24 + Instruction, // Logical Operators
        LShr = 25 + Instruction,
        AShr = 26 + Instruction,
        And = 27 + Instruction,
        Or = 28 + Instruction,
        Xor = 29 + Instruction,

        Alloca = 30 + Instruction, // Memory Operators
        Load = 31 + Instruction,
        Store = 32 + Instruction,
        GetElementPtr = 33 + Instruction,
        Fence = 34 + Instruction,
        AtomicCmpXchg = 35 + Instruction,
        AtomicRMW = 36 + Instruction,

        Trunc = 37 + Instruction, // cast/conversion operators
        ZeroExtend = 38 + Instruction,
        SignExtend = 39 + Instruction,
        FPToUI = 40 + Instruction,
        FPToSI = 41 + Instruction,
        UIToFP = 42 + Instruction,
        SIToFP = 43 + Instruction,
        FPTrunc = 44 + Instruction,
        FPExt = 45 + Instruction,
        PtrToInt = 46 + Instruction,
        IntToPtr = 47 + Instruction,
        BitCast = 48 + Instruction,
        AddrSpaceCast = 49 + Instruction,

        CleanupPad = 50 + Instruction, // New Exception pads
        CatchPad = 51 + Instruction,

        ICmp = 52 + Instruction,
        FCmp = 53 + Instruction,
        Phi = 54 + Instruction,
        Call = 55 + Instruction,
        Select = 56 + Instruction,
        UserOp1 = 57 + Instruction,
        UserOp2 = 58 + Instruction,
        VaArg = 59 + Instruction,
        ExtractElement = 60 + Instruction,
        InsertElement = 61 + Instruction,
        ShuffleVector = 62 + Instruction,
        ExtractValue = 63 + Instruction,
        InsertValue = 64 + Instruction,
        LandingPad = 65 + Instruction,

        // Markers:
        ConstantFirstVal = Function,
        ConstantLastVal = ConstantTokenNone,

        ConstantDataFirstVal = UndefValue,
        ConstantDataLastVal = ConstantTokenNone,
        ConstantAggregateFirstVal = ConstantArray,
        ConstantAggregateLastVal = ConstantVector
    }
}

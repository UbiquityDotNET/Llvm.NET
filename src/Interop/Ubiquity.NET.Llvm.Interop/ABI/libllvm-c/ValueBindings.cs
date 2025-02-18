// -----------------------------------------------------------------------
// <copyright file="ValueBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
// Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LibLLVMValueCacheItemDeletedCallback = delegate* unmanaged[Cdecl] <LLVMValueRef /*@ref*/, nint /*handle*/, void /*retVal*/>;
    using unsafe LibLLVMValueCacheItemReplacedCallback = delegate* unmanaged[Cdecl] <LLVMValueRef /*oldValue*/, nint /*handle*/, LLVMValueRef /*newValue*/, nint /*retVal*/ >;
#pragma warning restore IDE0065, SA1200

    public enum LibLLVMValueKind
        : Int32
    {
        Function = 0,
        GlobalAlias = 1,
        GlobalIFunc = 2,
        GlobalVariable = 3,
        BlockAddress = 4,
        ConstantExpr = 5,
        DSOLocalEquivalent = 6,
        NoCFIValue = 7,
        ConstantPtrAuth = 8,
        ConstantArray = 9,
        ConstantStruct = 10,
        ConstantVector = 11,
        UndefValue = 12,
        PoisonValue = 13,
        ConstantAggregateZero = 14,
        ConstantDataArray = 15,
        ConstantDataVector = 16,
        ConstantInt = 17,
        ConstantFP = 18,
        ConstantTargetNone = 19,
        ConstantPointerNull = 20,
        ConstantTokenNone = 21,
        Argument = 22,
        BasicBlock = 23,
        MetadataAsValue = 24,
        InlineAsm = 25,
        MemoryUse = 26,
        MemoryDef = 27,
        MemoryPhi = 28,
        Instruction = 29,
        Ret = 30,
        Br = 31,
        Switch = 32,
        IndirectBr = 33,
        Invoke = 34,
        Resume = 35,
        Unreachable = 36,
        CleanupRet = 37,
        CatchRet = 38,
        CatchSwitch = 39,
        CallBr = 40,
        FNeg = 41,
        Add = 42,
        FAdd = 43,
        Sub = 44,
        FSub = 45,
        Mul = 46,
        FMul = 47,
        UDiv = 48,
        SDiv = 49,
        FDiv = 50,
        URem = 51,
        SRem = 52,
        FRem = 53,
        Shl = 54,
        LShr = 55,
        AShr = 56,
        And = 57,
        Or = 58,
        Xor = 59,
        Alloca = 60,
        Load = 61,
        Store = 62,
        GetElementPtr = 63,
        Fence = 64,
        AtomicCmpXchg = 65,
        AtomicRMW = 66,
        Trunc = 67,
        ZExt = 68,
        SExt = 69,
        FPToUI = 70,
        FPToSI = 71,
        UIToFP = 72,
        SIToFP = 73,
        FPTrunc = 74,
        FPExt = 75,
        PtrToInt = 76,
        IntToPtr = 77,
        BitCast = 78,
        AddrSpaceCast = 79,
        CleanupPad = 80,
        CatchPad = 81,
        ICmp = 82,
        FCmp = 83,
        PHI = 84,
        Call = 85,
        Select = 86,
        UserOp1 = 87,
        UserOp2 = 88,
        VAArg = 89,
        ExtractElement = 90,
        InsertElement = 91,
        ShuffleVector = 92,
        ExtractValue = 93,
        InsertValue = 94,
        LandingPad = 95,
        Freeze = 96,
        ConstantFirstVal = 0,
        ConstantLastVal = 21,
        ConstantDataFirstVal = 12,
        ConstantDataLastVal = 21,
        ConstantAggregateFirstVal = 9,
        ConstantAggregateLastVal = 11,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsConstantZeroValue( LLVMValueRef valueRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMRemoveGlobalFromParent( LLVMValueRef valueRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LibLLVMValueKind LibLLVMGetValueKind( LLVMValueRef valueRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMGetAliasee( LLVMValueRef Val );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial UInt32 LibLLVMGetArgumentIndex( LLVMValueRef Val );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMGlobalVariableAddDebugExpression( LLVMValueRef globalVar, LLVMMetadataRef exp );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMFunctionAppendBasicBlock( LLVMValueRef function, LLVMBasicBlockRef block );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMValueRef LibLLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LibLLVMValueCacheRef LibLLVMCreateValueCache( LibLLVMValueCacheItemDeletedCallback deletedCallback, LibLLVMValueCacheItemReplacedCallback replacedCallback );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMValueCacheAdd( LibLLVMValueCacheRef cacheRef, LLVMValueRef value, nint handle );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial nint LibLLVMValueCacheLookup( LibLLVMValueCacheRef cacheRef, LLVMValueRef valueRef );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsConstantCString( LLVMValueRef C );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial UInt32 LibLLVMGetConstantDataSequentialElementCount( LLVMValueRef C );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial byte* LibLLVMGetConstantDataSequentialRawData( LLVMValueRef C, out size_t Length );
    }
}

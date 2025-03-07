// -----------------------------------------------------------------------
// <copyright file="AttributeBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1602 // An item within a C# enumeration is missing an Xml documentation header.
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1512 // Single-line comments should not be followed by blank line
    public enum LibLLVMAttrKind
    {
        None,
        FirstEnumAttr = 1,
        AllocAlign = FirstEnumAttr,
        AllocatedPointer = 2,
        AlwaysInline = 3,
        Builtin = 4,
        Cold = 5,
        Convergent = 6,
        CoroDestroyOnlyWhenComplete = 7,
        CoroElideSafe = 8,
        DeadOnUnwind = 9,
        DisableSanitizerInstrumentation = 10,
        FnRetThunkExtern = 11,
        Hot = 12,
        HybridPatchable = 13,
        ImmArg = 14,
        InReg = 15,
        InlineHint = 16,
        JumpTable = 17,
        MinSize = 18,
        MustProgress = 19,
        Naked = 20,
        Nest = 21,
        NoAlias = 22,
        NoBuiltin = 23,
        NoCallback = 24,
        NoCapture = 25,
        NoCfCheck = 26,
        NoDivergenceSource = 27,
        NoDuplicate = 28,
        NoExt = 29,
        NoFree = 30,
        NoImplicitFloat = 31,
        NoInline = 32,
        NoMerge = 33,
        NoProfile = 34,
        NoRecurse = 35,
        NoRedZone = 36,
        NoReturn = 37,
        NoSanitizeBounds = 38,
        NoSanitizeCoverage = 39,
        NoSync = 40,
        NoUndef = 41,
        NoUnwind = 42,
        NonLazyBind = 43,
        NonNull = 44,
        NullPointerIsValid = 45,
        OptForFuzzing = 46,
        OptimizeForDebugging = 47,
        OptimizeForSize = 48,
        OptimizeNone = 49,
        PresplitCoroutine = 50,
        ReadNone = 51,
        ReadOnly = 52,
        Returned = 53,
        ReturnsTwice = 54,
        SExt = 55,
        SafeStack = 56,
        SanitizeAddress = 57,
        SanitizeHWAddress = 58,
        SanitizeMemTag = 59,
        SanitizeMemory = 60,
        SanitizeNumericalStability = 61,
        SanitizeRealtime = 62,
        SanitizeRealtimeBlocking = 63,
        SanitizeThread = 64,
        SanitizeType = 65,
        ShadowCallStack = 66,
        SkipProfile = 67,
        Speculatable = 68,
        SpeculativeLoadHardening = 69,
        StackProtect = 70,
        StackProtectReq = 71,
        StackProtectStrong = 72,
        StrictFP = 73,
        SwiftAsync = 74,
        SwiftError = 75,
        SwiftSelf = 76,
        WillReturn = 77,
        Writable = 78,
        WriteOnly = 79,
        ZExt = 80,
        LastEnumAttr = ZExt,
        FirstTypeAttr = 81,
        ByRef = FirstTypeAttr,
        ByVal = 82,
        ElementType = 83,
        InAlloca = 84,
        Preallocated = 85,
        StructRet = 86,
        LastTypeAttr = StructRet,
        FirstIntAttr = 87,
        Alignment = FirstIntAttr,
        AllocKind = 88,
        AllocSize = 89,
        Captures = 90,
        Dereferenceable = 91,
        DereferenceableOrNull = 92,
        Memory = 93,
        NoFPClass = 94,
        StackAlignment = 95,
        UWTable = 96,
        VScaleRange = 97,
        LastIntAttr = VScaleRange,
        FirstConstantRangeAttr = 98,
        Range = FirstConstantRangeAttr,
        LastConstantRangeAttr = Range,
        FirstConstantRangeListAttr = 99,
        Initializes = FirstConstantRangeListAttr,
        LastConstantRangeListAttr = Initializes,
        // EndAttrKinds,          // Sentinel value useful for loops
        // EmptyKey,              // Use as Empty key for DenseMap of AttrKind
        // TombstoneKey,          // Use as Tombstone key for DenseMap of AttrKind
    }
#pragma warning restore SA1512 // Single-line comments should not be followed by blank line
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line
#pragma warning restore SA1602 // An item within a C# enumeration is missing an Xml documentation header.
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    public static partial class AttributeBindings
    {
        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMAttributeToString(LLVMAttributeRef attribute);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMGetAttributeKindName(LibLLVMAttrKind attrKind);

        [LibraryImport( NativeMethods.LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMGetEnumAttributeKindName(LLVMAttributeRef attribute);
    }
}

// -----------------------------------------------------------------------
// <copyright file="AttributeKindExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Id values for enumerated LLVM attributes</summary>
    /// <remarks>
    /// <note type="important">LLVM officially documents NO stability for the names or
    /// values of the enumerations here. Unfortunately, the C API only ever deals in
    /// an integral value (C++ consumers have FULL access to the Attribute::AttrKind
    /// enumeration...). This leaves any language bindings with the ONLY option of
    /// redefining the enum OR dealing only in unnamed integral values. Using just
    /// an integer is a VERY bad experience as it leans toward the "magic" number
    /// scenario. Worse is that the meaning of said "magic" values could change.
    /// This is built from the headers of a specific version and is bound to that.
    /// So, while the names and values may change over time they WON'T change for
    /// a given release of LLVM. (And likely won't change across even a minor version
    /// change. But, sadly, there is no promise on that...) Thus projections/bindings
    /// and, more importantly consumers of such, are stuck with the reality that this
    /// is an area that is likely to cause breaking changes.
    /// </note>
    /// <para>
    /// </para>
    /// </remarks>
    public enum AttributeKind
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable CA1069 // Enums should not have duplicate values
        None = LibLLVMAttrKind.None,
        FirstEnumAttr = LibLLVMAttrKind.FirstEnumAttr,
        AllocAlign = LibLLVMAttrKind.AllocAlign,
        AllocatedPointer = LibLLVMAttrKind.AllocatedPointer,
        AlwaysInline = LibLLVMAttrKind.AlwaysInline,
        BuiltIn = LibLLVMAttrKind.Builtin,
        Cold = LibLLVMAttrKind.Cold,
        Convergent = LibLLVMAttrKind.Convergent,
        CoroDestroyOnlyWhenComplete = LibLLVMAttrKind.CoroDestroyOnlyWhenComplete,
        CoroElideSafe = LibLLVMAttrKind.CoroElideSafe,
        DeadOnUnwind = LibLLVMAttrKind.DeadOnUnwind,
        DisableSanitizerInstrumentation = LibLLVMAttrKind.DisableSanitizerInstrumentation,
        FnRetThunkExtern = LibLLVMAttrKind.FnRetThunkExtern,
        Hot = LibLLVMAttrKind.Hot,
        HybridPatchable = LibLLVMAttrKind.HybridPatchable,
        ImmArg = LibLLVMAttrKind.ImmArg,
        InReg = LibLLVMAttrKind.InReg,
        InlineHint = LibLLVMAttrKind.InlineHint,
        JumpTable = LibLLVMAttrKind.JumpTable,
        MinSize = LibLLVMAttrKind.MinSize,
        MustProgress = LibLLVMAttrKind.MustProgress,
        Naked = LibLLVMAttrKind.Naked,
        Nest = LibLLVMAttrKind.Nest,
        NoAlias = LibLLVMAttrKind.NoAlias,
        NoBuiltIn = LibLLVMAttrKind.NoBuiltin,
        NoCallback = LibLLVMAttrKind.NoCallback,
        NoCapture = LibLLVMAttrKind.NoCapture,
        NoCfCheck = LibLLVMAttrKind.NoCfCheck,
        NoDivergenceSource = LibLLVMAttrKind.NoDivergenceSource,
        NoDuplicate = LibLLVMAttrKind.NoDuplicate,
        NoExt = LibLLVMAttrKind.NoExt,
        NoFree = LibLLVMAttrKind.NoFree,
        NoImplicitFloat = LibLLVMAttrKind.NoImplicitFloat,
        NoInline = LibLLVMAttrKind.NoInline,
        NoMerge = LibLLVMAttrKind.NoMerge,
        NoProfile = LibLLVMAttrKind.NoProfile,
        NoRecurse = LibLLVMAttrKind.NoRecurse,
        NoRedZone = LibLLVMAttrKind.NoRedZone,
        NoReturn = LibLLVMAttrKind.NoReturn,
        NoSanitizeBounds = LibLLVMAttrKind.NoSanitizeBounds,
        NoSanitizeCoverage = LibLLVMAttrKind.NoSanitizeCoverage,
        NoSync = LibLLVMAttrKind.NoSync,
        NoUndef = LibLLVMAttrKind.NoUndef,
        NoUnwind = LibLLVMAttrKind.NoUnwind,
        NonLazyBind = LibLLVMAttrKind.NonLazyBind,
        NonNull = LibLLVMAttrKind.NonNull,
        NullPointerIsValid = LibLLVMAttrKind.NullPointerIsValid,
        OptForFuzzing = LibLLVMAttrKind.OptForFuzzing,
        OptimizeForDebugging = LibLLVMAttrKind.OptimizeForDebugging,
        OptimizeForSize = LibLLVMAttrKind.OptimizeForSize,
        OptimizeNone = LibLLVMAttrKind.OptimizeNone,
        PresplitCoroutine = LibLLVMAttrKind.PresplitCoroutine,
        ReadNone = LibLLVMAttrKind.ReadNone,
        ReadOnly = LibLLVMAttrKind.ReadOnly,
        Returned = LibLLVMAttrKind.Returned,
        ReturnsTwice = LibLLVMAttrKind.ReturnsTwice,
        SExt = LibLLVMAttrKind.SExt,
        SafeStack = LibLLVMAttrKind.SafeStack,
        SanitizeAddress = LibLLVMAttrKind.SanitizeAddress,
        SanitizeHWAddress = LibLLVMAttrKind.SanitizeHWAddress,
        SanitizeMemTag = LibLLVMAttrKind.SanitizeMemTag,
        SanitizeMemory = LibLLVMAttrKind.SanitizeMemory,
        SanitizeNumericalStability = LibLLVMAttrKind.SanitizeNumericalStability,
        SanitizeRealtime = LibLLVMAttrKind.SanitizeRealtime,
        SanitizeRealtimeBlocking = LibLLVMAttrKind.SanitizeRealtimeBlocking,
        SanitizeThread = LibLLVMAttrKind.SanitizeThread,
        SanitizeType = LibLLVMAttrKind.SanitizeType,
        ShadowCallStack = LibLLVMAttrKind.ShadowCallStack,
        SkipProfile = LibLLVMAttrKind.SkipProfile,
        Speculatable = LibLLVMAttrKind.Speculatable,
        SpeculativeLoadHardening = LibLLVMAttrKind.SpeculativeLoadHardening,
        StackProtect = LibLLVMAttrKind.StackProtect,
        StackProtectReq = LibLLVMAttrKind.StackProtectReq,
        StackProtectStrong = LibLLVMAttrKind.StackProtectStrong,
        StrictFP = LibLLVMAttrKind.StrictFP,
        SwiftAsync = LibLLVMAttrKind.SwiftAsync,
        SwiftError = LibLLVMAttrKind.SwiftError,
        SwiftSelf = LibLLVMAttrKind.SwiftSelf,
        WillReturn = LibLLVMAttrKind.WillReturn,
        Writable = LibLLVMAttrKind.Writable,
        WriteOnly = LibLLVMAttrKind.WriteOnly,
        ZExt = LibLLVMAttrKind.ZExt,
        LastEnumAttr = LibLLVMAttrKind.LastEnumAttr,
        FirstTypeAttr = LibLLVMAttrKind.FirstTypeAttr,
        ByRef = LibLLVMAttrKind.ByRef,
        ByVal = LibLLVMAttrKind.ByVal,
        ElementType = LibLLVMAttrKind.ElementType,
        InAlloca = LibLLVMAttrKind.InAlloca,
        Preallocated = LibLLVMAttrKind.Preallocated,
        StructRet = LibLLVMAttrKind.StructRet,
        LastTypeAttr = LibLLVMAttrKind.LastTypeAttr,
        FirstIntAttr = LibLLVMAttrKind.FirstIntAttr,
        Alignment = LibLLVMAttrKind.Alignment,
        AllocKind = LibLLVMAttrKind.AllocKind,
        AllocSize = LibLLVMAttrKind.AllocSize,
        Captures = LibLLVMAttrKind.Captures,
        Dereferenceable = LibLLVMAttrKind.Dereferenceable,
        DereferenceableOrNull = LibLLVMAttrKind.DereferenceableOrNull,
        Memory = LibLLVMAttrKind.Memory,
        NoFPClass = LibLLVMAttrKind.NoFPClass,
        StackAlignment = LibLLVMAttrKind.StackAlignment,
        UWTable = LibLLVMAttrKind.UWTable,
        VScaleRange = LibLLVMAttrKind.VScaleRange,
        LastIntAttr = LibLLVMAttrKind.LastIntAttr,
        FirstConstantRangeAttr = LibLLVMAttrKind.FirstConstantRangeAttr,
        Range = LibLLVMAttrKind.Range,
        LastConstantRangeAttr = LibLLVMAttrKind.LastConstantRangeAttr,
        FirstConstantRangeListAttr = LibLLVMAttrKind.FirstConstantRangeListAttr,
        Initializes = LibLLVMAttrKind.Initializes,
        LastConstantRangeListAttr = LibLLVMAttrKind.LastConstantRangeListAttr,
#pragma warning restore CA1069 // Enums should not have duplicate values
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    enum UWTableKind
        : UInt64
    {
        None,
        Sync,
        Async
    }
}

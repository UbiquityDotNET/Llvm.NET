// -----------------------------------------------------------------------
// <copyright file="Orc.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // NOTE: Context handles are just value types that wrap around a runtime nint (basically a strong typedef)
    //       Therefore, they are blittable value types and don't need any marshaling. Global handles, however,
    //       do need marshalling and therefore CANNOT appear in the signature of an unmanaged function pointer.
    //       Implementations MUST handle marshalling of the ABI types manually

    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOrcCAPIDefinitionGeneratorTryToGenerateFunction = delegate* unmanaged[Cdecl]<
        nint /*LLVMOrcDefinitionGeneratorRef*/ /*GeneratorObj*/,
        void* /*Ctx*/,
        /*[Out]*/ LLVMOrcLookupStateRef* /*LookupState*/,
        LLVMOrcLookupKind /*Kind*/,
        LLVMOrcJITDylibRef /*JD*/,
        LLVMOrcJITDylibLookupFlags /*JDLookupFlags*/,
        LLVMOrcCLookupSetElement* /*LookupSet*/,
        size_t /*LookupSetSize*/,
        nint /*LLVMErrorRef*//*retVal*/
        >;
    using unsafe LLVMOrcDisposeCAPIDefinitionGeneratorFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, void /*retVal*/>;
    using unsafe LLVMOrcErrorReporterFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, nint /*LLVMErrorRef*/ /*Err*/, void /*retVal*/>;
    using unsafe LLVMOrcExecutionSessionLookupHandleResultFunction = delegate* unmanaged[Cdecl]<nint /*LLVMErrorRef*/ /*Err*/, LLVMOrcCSymbolMapPair* /*Result*/, size_t /*NumPairs*/, void* /*Ctx*/, void /*retVal*/>;
    using unsafe LLVMOrcGenericIRModuleOperationFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, nint /*LLVMModuleRef*/ /*M*/, nint /*LLVMErrorRef*/ /*retVal*/ >;
    using unsafe LLVMOrcIRTransformLayerTransformFunction = delegate* unmanaged[Cdecl]<
        void* /*Ctx*/,
        /*[Out]*/ nint* /*LLVMOrcThreadSafeModuleRef*/ /*ModInOut*/,
        nint /*LLVMOrcMaterializationResponsibilityRef*/ /*MR*/,
        nint /*LLVMErrorRef*/ /*retVal*/
        >;
    using unsafe LLVMOrcMaterializationUnitDestroyFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, void /*retVal*/ >;
    using unsafe LLVMOrcMaterializationUnitDiscardFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, LLVMOrcJITDylibRef /*JD*/, nint /*LLVMOrcSymbolStringPoolEntryRef*/ /*Symbol*/, void /*retVal*/>;
    using unsafe LLVMOrcMaterializationUnitMaterializeFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, nint /*LLVMOrcMaterializationResponsibilityRef*/ /*MR*/, void /*retVal*/>;
    using unsafe LLVMOrcObjectTransformLayerTransformFunction = delegate* unmanaged[Cdecl]<void* /*Ctx*/, /*[Out]*/ nint* /*LLVMMemoryBufferRef*/ /*ObjInOut*/, nint /*LLVMErrorRef*/ /*retVal*/ >;
    using unsafe LLVMOrcSymbolPredicate = delegate* unmanaged[Cdecl]<void* /*Ctx*/, nint /*LLVMOrcSymbolStringPoolEntryRef*/ /*Sym*/, int /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI naming" )]
    public enum LLVMJITSymbolGenericFlags
        : Int32
    {
        LLVMJITSymbolGenericFlagsNone = 0,
        LLVMJITSymbolGenericFlagsExported = 1,
        LLVMJITSymbolGenericFlagsWeak = 2,
        LLVMJITSymbolGenericFlagsCallable = 4,
        LLVMJITSymbolGenericFlagsMaterializationSideEffectsOnly = 8,
    }

    public enum LLVMOrcLookupKind
        : Int32
    {
        LLVMOrcLookupKindStatic = 0,
        LLVMOrcLookupKindDLSym = 1,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It does have one, and the name reflects the meaning, which is NOT 'None'" )]
    public enum LLVMOrcJITDylibLookupFlags
        : Int32
    {
        LLVMOrcJITDylibLookupFlagsMatchExportedSymbolsOnly = 0,
        LLVMOrcJITDylibLookupFlagsMatchAllSymbols = 1,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It does have one, and the name reflects the meaning, which is NOT 'None'" )]
    public enum LLVMOrcSymbolLookupFlags
        : Int32
    {
        LLVMOrcSymbolLookupFlagsRequiredSymbol = 0,
        LLVMOrcSymbolLookupFlagsWeaklyReferencedSymbol = 1,
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMJITSymbolFlags
    {
        public readonly byte /*LLVMJITSymbolGeneric*/ GenericFlags;
        public readonly byte TargetFlags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMJITEvaluatedSymbol
    {
        public readonly UInt64 Address;
        public readonly LLVMJITSymbolFlags Flags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolFlagsMapPair
    {
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public readonly LLVMJITSymbolFlags Flags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolMapPair
    {
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public readonly LLVMJITEvaluatedSymbol Sym;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolAliasMapEntry
    {
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public readonly LLVMJITSymbolFlags Flags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolAliasMapPair
    {
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public readonly LLVMOrcCSymbolAliasMapEntry Entry;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolsList
    {
        // This is an Array/Span of LLVMOrcSymbolStringPoolEntryRef
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef* */ Symbols;
        public readonly size_t Length;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCDependenceMapPair
    {
        public readonly nint /*LLVMOrcJITDylibRef*/ JD;
        public readonly LLVMOrcCSymbolsList Names;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolDependenceGroup
    {
        public readonly LLVMOrcCSymbolsList Symbols;

        // This is an Array/Span of LLVMOrcCDependenceMapPair
        public readonly nint /*LLVMOrcCDependenceMapPair* */ Dependencies;
        public readonly size_t NumDependencies;
    }

    [StructLayout( LayoutKind.Sequential )]
    public record struct LLVMOrcCJITDylibSearchOrderElement
    {
        public LLVMOrcJITDylibRef JD;
        public LLVMOrcJITDylibLookupFlags JDLookupFlags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public record struct LLVMOrcCLookupSetElement
    {
        public nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public LLVMOrcSymbolLookupFlags LookupFlags;
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcExecutionSessionSetErrorReporter(LLVMOrcExecutionSessionRef ES, LLVMOrcErrorReporterFunction ReportError, void* Ctx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolRef LLVMOrcExecutionSessionGetSymbolStringPool(LLVMOrcExecutionSessionRef ES);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcSymbolStringPoolClearDeadEntries(LLVMOrcSymbolStringPoolRef SSP);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcExecutionSessionIntern(LLVMOrcExecutionSessionRef ES, string Name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcExecutionSessionLookup(
            LLVMOrcExecutionSessionRef ES,
            LLVMOrcLookupKind K,
            LLVMOrcCJITDylibSearchOrderElement* SearchOrder,
            size_t SearchOrderSize,
            LLVMOrcCLookupSetElement* Symbols,
            size_t SymbolsSize,
            LLVMOrcExecutionSessionLookupHandleResultFunction HandleResult,
            void* Ctx
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcRetainSymbolStringPoolEntry(LLVMOrcSymbolStringPoolEntryRef S);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseSymbolStringPoolEntry(LLVMOrcSymbolStringPoolEntryRef S);

        // This does NOT marshal the string, it only provides the raw pointer so that a span is constructible
        // from the pointer. The memory for the string is OWNED by the entry so the returned pointer is valid
        // for the lifetime of the referenced entry.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte* LLVMOrcSymbolStringPoolEntryStr(LLVMOrcSymbolStringPoolEntryRef S);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseResourceTracker(LLVMOrcResourceTrackerRef RT);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcResourceTrackerTransferTo(LLVMOrcResourceTrackerRef SrcRT, LLVMOrcResourceTrackerRef DstRT);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcResourceTrackerRemove(LLVMOrcResourceTrackerRef RT);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDefinitionGenerator(LLVMOrcDefinitionGeneratorRef DG);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationUnit(LLVMOrcMaterializationUnitRef MU);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcCreateCustomMaterializationUnit(string Name, void* Ctx, LLVMOrcCSymbolFlagsMapPair* Syms, size_t NumSyms, LLVMOrcSymbolStringPoolEntryRef InitSym, LLVMOrcMaterializationUnitMaterializeFunction Materialize, LLVMOrcMaterializationUnitDiscardFunction Discard, LLVMOrcMaterializationUnitDestroyFunction Destroy);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcAbsoluteSymbols(LLVMOrcCSymbolMapPair* Syms, size_t NumPairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcLazyReexports(LLVMOrcLazyCallThroughManagerRef LCTM, LLVMOrcIndirectStubsManagerRef ISM, LLVMOrcJITDylibRef SourceRef, LLVMOrcCSymbolAliasMapPair* CallableAliases, size_t NumPairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationResponsibility(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcMaterializationResponsibilityGetTargetDylib(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcExecutionSessionRef LLVMOrcMaterializationResponsibilityGetExecutionSession(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcCSymbolFlagsMapPair* LLVMOrcMaterializationResponsibilityGetSymbols(LLVMOrcMaterializationResponsibilityRef MR, out size_t NumPairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeCSymbolFlagsMap(LLVMOrcCSymbolFlagsMapPair* Pairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcMaterializationResponsibilityGetInitializerSymbol(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcMaterializationResponsibilityGetRequestedSymbols(LLVMOrcMaterializationResponsibilityRef MR, out size_t NumSymbols);

        // TODO: Create custom marshaller to use this and hide it. This is tricky as it is disposing of a pointer
        // to the strings (That is the array itself is disposed, but NOT the symbol strings). It should ONLY be
        // used in a custom marshaller for the array of symbol strings. (Internally this simply calls free() on
        // the pointer so any referenced strings are still valid)
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeSymbols(nint* Symbols);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyResolved(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcCSymbolFlagsMapPair* Symbols, size_t NumPairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyEmitted(LLVMOrcMaterializationResponsibilityRef MR, out LLVMOrcCSymbolDependenceGroup SymbolDepGroups, size_t NumSymbolDepGroups);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDefineMaterializing(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcCSymbolFlagsMapPair* Pairs, size_t NumPairs);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcMaterializationResponsibilityFailMaterialization(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityReplace(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcMaterializationUnitRef MU);

        // NOTE: The normal ArrayMarshaller and SafeHandleMarsaller are NOT capable of handling arrays of references to safe handles,
        // so a caller must manually create a pinned array of the underlying handle values and provide a pointer to the first element.
        // This is normally done with the RefHandleMarshaller class.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDelegate(
            LLVMOrcMaterializationResponsibilityRef MR,
            /*[In] LLVMOrcSymbolStringPoolEntryRef[]*/ nint* Symbols,
            size_t NumSymbols,
            out LLVMOrcMaterializationResponsibilityRef Result
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionCreateBareJITDylib(LLVMOrcExecutionSessionRef ES, byte* Name);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcExecutionSessionCreateJITDylib(LLVMOrcExecutionSessionRef ES, out LLVMOrcJITDylibRef Result, string Name);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionGetJITDylibByName(LLVMOrcExecutionSessionRef ES, string Name);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibCreateResourceTracker(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibGetDefaultResourceTracker(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibDefine(LLVMOrcJITDylibRef JD, LLVMOrcMaterializationUnitRef MU);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibClear(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITDylibAddGenerator(LLVMOrcJITDylibRef JD, LLVMOrcDefinitionGeneratorRef DG);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDefinitionGeneratorRef LLVMOrcCreateCustomCAPIDefinitionGenerator(LLVMOrcCAPIDefinitionGeneratorTryToGenerateFunction F, void* Ctx, LLVMOrcDisposeCAPIDefinitionGeneratorFunction Dispose);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLookupStateContinueLookup(LLVMOrcLookupStateRef S, LLVMErrorRef Err);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForProcess(out LLVMOrcDefinitionGeneratorRef Result, sbyte GlobalPrefx, LLVMOrcSymbolPredicate Filter, void* FilterCtx);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForPath(out LLVMOrcDefinitionGeneratorRef Result, string FileName, sbyte GlobalPrefix, LLVMOrcSymbolPredicate Filter, void* FilterCtx);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateStaticLibrarySearchGeneratorForPath(out LLVMOrcDefinitionGeneratorRef Result, LLVMOrcObjectLayerRef ObjLayer, string FileName, string TargetTriple);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeContextRef LLVMOrcCreateNewThreadSafeContext();

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMOrcThreadSafeContextGetContext(LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeContext(LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeModuleRef LLVMOrcCreateNewThreadSafeModule(LLVMModuleRef M, LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeModule(LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcThreadSafeModuleWithModuleDo(LLVMOrcThreadSafeModuleRef TSM, LLVMOrcGenericIRModuleOperationFunction F, void* Ctx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITTargetMachineBuilderDetectHost(out LLVMOrcJITTargetMachineBuilderRef Result);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITTargetMachineBuilderRef LLVMOrcJITTargetMachineBuilderCreateFromTargetMachine(LLVMTargetMachineRef TM);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeJITTargetMachineBuilder(LLVMOrcJITTargetMachineBuilderRef JTMB);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMOrcJITTargetMachineBuilderGetTargetTriple(LLVMOrcJITTargetMachineBuilderRef JTMB);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITTargetMachineBuilderSetTargetTriple(LLVMOrcJITTargetMachineBuilderRef JTMB, string TargetTriple);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFile(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcJITDylibRef JD, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFileWithRT(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcResourceTrackerRef RT, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectLayerEmit(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcMaterializationResponsibilityRef R, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeObjectLayer(LLVMOrcObjectLayerRef ObjLayer);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerEmit(LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerSetTransform(LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcIRTransformLayerTransformFunction TransformFunction, void* Ctx);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectTransformLayerSetTransform(LLVMOrcObjectTransformLayerRef ObjTransformLayer, LLVMOrcObjectTransformLayerTransformFunction TransformFunction, void* Ctx);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIndirectStubsManagerRef LLVMOrcCreateLocalIndirectStubsManager(string TargetTriple);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeIndirectStubsManager(LLVMOrcIndirectStubsManagerRef ISM);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateLocalLazyCallThroughManager(string TargetTriple, LLVMOrcExecutionSessionRef ES, UInt64 ErrorHandlerAddr, out LLVMOrcLazyCallThroughManagerRef LCTM);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeLazyCallThroughManager(LLVMOrcLazyCallThroughManagerRef LCTM);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDumpObjectsRef LLVMOrcCreateDumpObjects(string DumpDir, string IdentifierOverride);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDumpObjects(LLVMOrcDumpObjectsRef DumpObjects);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcDumpObjects_CallOperator(LLVMOrcDumpObjectsRef DumpObjects, out LLVMMemoryBufferRef ObjBuffer);
    }
}

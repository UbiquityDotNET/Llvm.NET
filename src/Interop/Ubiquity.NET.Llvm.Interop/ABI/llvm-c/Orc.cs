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
    //       Therefore, they are blittable value types and don't need any marshaling.
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOrcCAPIDefinitionGeneratorTryToGenerateFunction = delegate* unmanaged[Cdecl]<
        nint /*LLVMOrcDefinitionGeneratorRef*/ /*GeneratorObj*/,
        void* /*Ctx*/,
        /*[Out]*/ nint* /*LLVMOrcLookupStateRef*/ /*LookupState*/,
        LLVMOrcLookupKind /*Kind*/,
        LLVMOrcJITDylibRef /*JD*/,
        LLVMOrcJITDylibLookup /*JDLookupFlags*/,
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
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "ABI compatibility" )]
    public enum LLVMJITSymbolGeneric
        : byte
    {
        None = 0,
        Exported = 1,
        Weak = 2,
        Callable = 4,
        MaterializationSideEffectsOnly = 8,
    }

    public enum LLVMOrcLookupKind
        : Int32
    {
        Static = 0,
        DLSym = 1,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It does have one, and the name reflects the meaning" )]
    public enum LLVMOrcJITDylibLookup
        : Int32
    {
        MatchExportedSymbolsOnly = 0,
        MatchAllSymbols = 1,
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "It does have one, and the name reflects the meaning" )]
    public enum LLVMOrcSymbolLookup
        : Int32
    {
        RequiredSymbol = 0,
        WeaklyReferencedSymbol = 1,
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMJITSymbolFlags
    {
        public readonly LLVMJITSymbolGeneric GenericFlags;
        public readonly byte TargetFlags; // exact meaning is defined by target...
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
        // This is a Pointer to pointer!
        public readonly nint /*LLVMOrcSymbolStringPoolEntryRef[]*/ Symbols;
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
        public readonly nint /*LLVMOrcCDependenceMapPair[]*/ Dependencies;
        public readonly size_t NumDependencies;
    }

    [StructLayout( LayoutKind.Sequential )]
    public record struct LLVMOrcCJITDylibSearchOrderElement
    {
        public LLVMOrcJITDylibRef JD;
        public LLVMOrcJITDylibLookup JDLookupFlags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public record struct LLVMOrcCLookupSetElement
    {
        public nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        public LLVMOrcSymbolLookup LookupFlags;
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcExecutionSessionSetErrorReporter(LLVMOrcExecutionSessionRef ES, LLVMOrcErrorReporterFunction ReportError, void* Ctx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolRef LLVMOrcExecutionSessionGetSymbolStringPool(LLVMOrcExecutionSessionRef ES);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcSymbolStringPoolClearDeadEntries(LLVMOrcSymbolStringPoolRef SSP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcExecutionSessionIntern(LLVMOrcExecutionSessionRef ES, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
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

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcRetainSymbolStringPoolEntry(LLVMOrcSymbolStringPoolEntryRef S);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseSymbolStringPoolEntry(LLVMOrcSymbolStringPoolEntryRef S);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMOrcSymbolStringPoolEntryStr(LLVMOrcSymbolStringPoolEntryRef S);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseResourceTracker(LLVMOrcResourceTrackerRef RT);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcResourceTrackerTransferTo(LLVMOrcResourceTrackerRef SrcRT, LLVMOrcResourceTrackerRef DstRT);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcResourceTrackerRemove(LLVMOrcResourceTrackerRef RT);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDefinitionGenerator(LLVMOrcDefinitionGeneratorRef DG);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationUnit(LLVMOrcMaterializationUnitRef MU);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcCreateCustomMaterializationUnit([MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, void* Ctx, LLVMOrcCSymbolFlagsMapPair* Syms, size_t NumSyms, LLVMOrcSymbolStringPoolEntryRef InitSym, LLVMOrcMaterializationUnitMaterializeFunction Materialize, LLVMOrcMaterializationUnitDiscardFunction Discard, LLVMOrcMaterializationUnitDestroyFunction Destroy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcAbsoluteSymbols(LLVMOrcCSymbolMapPair* Syms, size_t NumPairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcLazyReexports(LLVMOrcLazyCallThroughManagerRef LCTM, LLVMOrcIndirectStubsManagerRef ISM, LLVMOrcJITDylibRef SourceRef, LLVMOrcCSymbolAliasMapPair* CallableAliases, size_t NumPairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationResponsibility(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcMaterializationResponsibilityGetTargetDylib(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcExecutionSessionRef LLVMOrcMaterializationResponsibilityGetExecutionSession(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcCSymbolFlagsMapPair* LLVMOrcMaterializationResponsibilityGetSymbols(LLVMOrcMaterializationResponsibilityRef MR, out size_t NumPairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeCSymbolFlagsMap(LLVMOrcCSymbolFlagsMapPair* Pairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcMaterializationResponsibilityGetInitializerSymbol(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcMaterializationResponsibilityGetRequestedSymbols(LLVMOrcMaterializationResponsibilityRef MR, out size_t NumSymbols);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeSymbols(out LLVMOrcSymbolStringPoolEntryRef Symbols);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyResolved(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcCSymbolFlagsMapPair* Symbols, size_t NumPairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyEmitted(LLVMOrcMaterializationResponsibilityRef MR, out LLVMOrcCSymbolDependenceGroup SymbolDepGroups, size_t NumSymbolDepGroups);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDefineMaterializing(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcCSymbolFlagsMapPair* Pairs, size_t NumPairs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcMaterializationResponsibilityFailMaterialization(LLVMOrcMaterializationResponsibilityRef MR);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityReplace(LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcMaterializationUnitRef MU);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDelegate(LLVMOrcMaterializationResponsibilityRef MR, out LLVMOrcSymbolStringPoolEntryRef Symbols, size_t NumSymbols, out LLVMOrcMaterializationResponsibilityRef Result);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionCreateBareJITDylib(LLVMOrcExecutionSessionRef ES, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcExecutionSessionCreateJITDylib(LLVMOrcExecutionSessionRef ES, out LLVMOrcJITDylibRef Result, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionGetJITDylibByName(LLVMOrcExecutionSessionRef ES, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibCreateResourceTracker(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibGetDefaultResourceTracker(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibDefine(LLVMOrcJITDylibRef JD, LLVMOrcMaterializationUnitRef MU);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibClear(LLVMOrcJITDylibRef JD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITDylibAddGenerator(LLVMOrcJITDylibRef JD, LLVMOrcDefinitionGeneratorRef DG);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDefinitionGeneratorRef LLVMOrcCreateCustomCAPIDefinitionGenerator(LLVMOrcCAPIDefinitionGeneratorTryToGenerateFunction F, void* Ctx, LLVMOrcDisposeCAPIDefinitionGeneratorFunction Dispose);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLookupStateContinueLookup(LLVMOrcLookupStateRef S, LLVMErrorRef Err);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForProcess(out LLVMOrcDefinitionGeneratorRef Result, sbyte GlobalPrefx, LLVMOrcSymbolPredicate Filter, void* FilterCtx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForPath(out LLVMOrcDefinitionGeneratorRef Result, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string FileName, sbyte GlobalPrefix, LLVMOrcSymbolPredicate Filter, void* FilterCtx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateStaticLibrarySearchGeneratorForPath(out LLVMOrcDefinitionGeneratorRef Result, LLVMOrcObjectLayerRef ObjLayer, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string FileName, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string TargetTriple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeContextRef LLVMOrcCreateNewThreadSafeContext();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRef LLVMOrcThreadSafeContextGetContext(LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeContext(LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeModuleRef LLVMOrcCreateNewThreadSafeModule(LLVMModuleRef M, LLVMOrcThreadSafeContextRef TSCtx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeModule(LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcThreadSafeModuleWithModuleDo(LLVMOrcThreadSafeModuleRef TSM, LLVMOrcGenericIRModuleOperationFunction F, void* Ctx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITTargetMachineBuilderDetectHost(out LLVMOrcJITTargetMachineBuilderRef Result);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITTargetMachineBuilderRef LLVMOrcJITTargetMachineBuilderCreateFromTargetMachine(LLVMTargetMachineRef TM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeJITTargetMachineBuilder(LLVMOrcJITTargetMachineBuilderRef JTMB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LLVMOrcJITTargetMachineBuilderGetTargetTriple(LLVMOrcJITTargetMachineBuilderRef JTMB);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITTargetMachineBuilderSetTargetTriple(LLVMOrcJITTargetMachineBuilderRef JTMB, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string TargetTriple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFile(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcJITDylibRef JD, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFileWithRT(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcResourceTrackerRef RT, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectLayerEmit(LLVMOrcObjectLayerRef ObjLayer, LLVMOrcMaterializationResponsibilityRef R, LLVMMemoryBufferRef ObjBuffer);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeObjectLayer(LLVMOrcObjectLayerRef ObjLayer);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerEmit(LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcThreadSafeModuleRef TSM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerSetTransform(LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcIRTransformLayerTransformFunction TransformFunction, void* Ctx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectTransformLayerSetTransform(LLVMOrcObjectTransformLayerRef ObjTransformLayer, LLVMOrcObjectTransformLayerTransformFunction TransformFunction, void* Ctx);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIndirectStubsManagerRef LLVMOrcCreateLocalIndirectStubsManager([MarshalUsing( typeof( AnsiStringMarshaller ) )] string TargetTriple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeIndirectStubsManager(LLVMOrcIndirectStubsManagerRef ISM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateLocalLazyCallThroughManager([MarshalUsing( typeof( AnsiStringMarshaller ) )] string TargetTriple, LLVMOrcExecutionSessionRef ES, UInt64 ErrorHandlerAddr, out LLVMOrcLazyCallThroughManagerRef LCTM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeLazyCallThroughManager(LLVMOrcLazyCallThroughManagerRef LCTM);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDumpObjectsRef LLVMOrcCreateDumpObjects([MarshalUsing( typeof( AnsiStringMarshaller ) )] string DumpDir, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string IdentifierOverride);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDumpObjects(LLVMOrcDumpObjectsRef DumpObjects);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcDumpObjects_CallOperator(LLVMOrcDumpObjectsRef DumpObjects, out LLVMMemoryBufferRef ObjBuffer);
    }
}

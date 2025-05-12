// -----------------------------------------------------------------------
// <copyright file="Orc.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
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
        nuint /*LookupSetSize*/,
        nint /*LLVMErrorRef*//*retVal*/
        >;
    using unsafe LLVMOrcDisposeCAPIDefinitionGeneratorFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, void /*retVal*/>;
    using unsafe LLVMOrcErrorReporterFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, nint /*LLVMErrorRef*/ /*Err*/, void /*retVal*/>;
    using unsafe LLVMOrcExecutionSessionLookupHandleResultFunction = delegate* unmanaged[Cdecl]< nint /*LLVMErrorRef*/ /*Err*/, LLVMOrcCSymbolMapPair* /*Result*/, nuint /*NumPairs*/, void* /*Ctx*/, void /*retVal*/>;
    using unsafe LLVMOrcGenericIRModuleOperationFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, LLVMModuleRefAlias /*M*/, nint /*LLVMErrorRef*/ /*retVal*/ >;
    using unsafe LLVMOrcIRTransformLayerTransformFunction = delegate* unmanaged[Cdecl]<
        void* /*Ctx*/,
        /*[Out]*/ nint* /*LLVMOrcThreadSafeModuleRef*/ /*ModInOut*/,
        nint /*LLVMOrcMaterializationResponsibilityRef*/ /*MR*/,
        nint /*LLVMErrorRef*/ /*retVal*/
        >;
    using unsafe LLVMOrcMaterializationUnitDestroyFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, void /*retVal*/ >;
    using unsafe LLVMOrcMaterializationUnitDiscardFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, /*LLVMOrcJITDylibRef*/nint /*JD*/, nint /*LLVMOrcSymbolStringPoolEntryRef*/ /*Symbol*/, void /*retVal*/>;
    using unsafe LLVMOrcMaterializationUnitMaterializeFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, nint /*LLVMOrcMaterializationResponsibilityRef*/ /*MR*/, void /*retVal*/>;
    using unsafe LLVMOrcObjectTransformLayerTransformFunction = delegate* unmanaged[Cdecl]< void* /*Ctx*/, /*[Out]*/ nint* /*LLVMMemoryBufferRef*/ /*ObjInOut*/, nint /*LLVMErrorRef*/ /*retVal*/ >;
    using unsafe LLVMOrcSymbolPredicate = delegate* unmanaged[Cdecl]< void* /*Ctx*/, nint /*LLVMOrcSymbolStringPoolEntryRef*/ /*Sym*/, int /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI naming" )]
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    public enum LLVMJITSymbolGenericFlags
        : byte
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
    public readonly record struct LLVMJITSymbolFlags( LLVMJITSymbolGenericFlags GenericFlags, byte TargetFlags );

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMJITEvaluatedSymbol( UInt64 Address, LLVMJITSymbolFlags Flags );

    // Only an "unmanaged" struct is usable directly in native APIs
    // so these only store the raw handle value. Callers must account for any move or ref counted lifetimes, etc...
    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolFlagsMapPair( LLVMOrcSymbolStringPoolEntryRefAlias Name, LLVMJITSymbolFlags Flags );

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolMapPair( LLVMOrcSymbolStringPoolEntryRefAlias Name, LLVMJITEvaluatedSymbol sym );

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolAliasMapEntry( LLVMOrcSymbolStringPoolEntryRefAlias Name, LLVMJITSymbolFlags Flags );

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCSymbolAliasMapPair( LLVMOrcSymbolStringPoolEntryRefAlias name, LLVMOrcCSymbolAliasMapEntry entry );

    [StructLayout( LayoutKind.Sequential )]
    public readonly /*record*/ struct LLVMOrcCSymbolsList
        : IEquatable<LLVMOrcCSymbolsList>
    {
        // This is a pointer to a contiguous span of LLVMOrcSymbolStringPoolEntryRef (Length field indicates the size)
        public unsafe readonly LLVMOrcSymbolStringPoolEntryRefAlias* Symbols;
        public readonly nuint Length;

        // NOTE: unsafe types not supported by a "record" struct so have to do equality manually
        #region IEquatable<LLVMOrcCSymbolsList>
        public bool Equals( LLVMOrcCSymbolsList other )
        {
            // Nothing inherently unsafe about comparing pointers...
            unsafe
            {
                return Symbols == other.Symbols;
            }
        }

        public override bool Equals(object? obj ) => obj is LLVMOrcCSymbolsList other && Equals(other);

        public override int GetHashCode( )
        {
            unsafe
            {
                return HashCode.Combine((nint)Symbols, Length);
            }
        }

        public static bool operator ==( LLVMOrcCSymbolsList left, LLVMOrcCSymbolsList right ) => left.Equals( right );

        public static bool operator !=( LLVMOrcCSymbolsList left, LLVMOrcCSymbolsList right ) => !(left == right);
        #endregion
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
        public readonly nuint NumDependencies;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCJITDylibSearchOrderElement
    {
        public readonly LLVMOrcJITDylibRef JD;
        public readonly LLVMOrcJITDylibLookupFlags JDLookupFlags;
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LLVMOrcCLookupSetElement
    {
        public LLVMOrcCLookupSetElement(LLVMOrcSymbolStringPoolEntryRef name, LLVMOrcSymbolLookupFlags flags)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name.DangerousGetHandle();
            LookupFlags = flags;
        }

        private readonly nint /*LLVMOrcSymbolStringPoolEntryRef*/ Name;
        private readonly LLVMOrcSymbolLookupFlags LookupFlags;
    }

    public static partial class Orc
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcExecutionSessionSetErrorReporter( LLVMOrcExecutionSessionRef ES, LLVMOrcErrorReporterFunction ReportError, void* Ctx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolRef LLVMOrcExecutionSessionGetSymbolStringPool( LLVMOrcExecutionSessionRef ES );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcSymbolStringPoolClearDeadEntries( LLVMOrcSymbolStringPoolRef SSP );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRef LLVMOrcExecutionSessionIntern(
            LLVMOrcExecutionSessionRef ES,
            LazyEncodedString Name
            );

        [Experimental( "LLVM001" )]
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcExecutionSessionLookup(
            LLVMOrcExecutionSessionRef ES,
            LLVMOrcLookupKind K,
            [In] LLVMOrcCJITDylibSearchOrderElement[] SearchOrder, nuint SearchOrderSize,
            [In] LLVMOrcCLookupSetElement[] Symbols, nuint SymbolsSize,
            LLVMOrcExecutionSessionLookupHandleResultFunction HandleResult,
            void* Ctx
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcRetainSymbolStringPoolEntry( LLVMOrcSymbolStringPoolEntryRef S );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseSymbolStringPoolEntry( LLVMOrcSymbolStringPoolEntryRefAlias S );

        // This does NOT marshal the string, it only provides the raw pointer so that a span is constructible
        // from the pointer. The memory for the string is OWNED by the entry so the returned pointer is valid
        // for the lifetime of the referenced entry.
        // TODO: Consider if this can't or shouldn't be a LazyEncodedString to contain potentially both forms
        //       as needed.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte* LLVMOrcSymbolStringPoolEntryStr( LLVMOrcSymbolStringPoolEntryRef S );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcReleaseResourceTracker( LLVMOrcResourceTrackerRef RT );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcResourceTrackerTransferTo( LLVMOrcResourceTrackerRef SrcRT, LLVMOrcResourceTrackerRef DstRT );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcResourceTrackerRemove( LLVMOrcResourceTrackerRef RT );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDefinitionGenerator( LLVMOrcDefinitionGeneratorRef DG );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationUnit( LLVMOrcMaterializationUnitRef MU );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcCreateCustomMaterializationUnit(
            LazyEncodedString Name,
            void* Ctx,
            LLVMOrcCSymbolFlagsMapPair* Syms, nuint NumSyms,
            LLVMOrcSymbolStringPoolEntryRef InitSym,
            LLVMOrcMaterializationUnitMaterializeFunction Materialize,
            LLVMOrcMaterializationUnitDiscardFunction Discard,
            LLVMOrcMaterializationUnitDestroyFunction Destroy
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcCreateCustomMaterializationUnit(
            byte* Name,
            void* Ctx,
            [In] LLVMOrcCSymbolFlagsMapPair[] Syms, nuint NumSyms,
            LLVMOrcSymbolStringPoolEntryRef InitSym,
            LLVMOrcMaterializationUnitMaterializeFunction Materialize,
            LLVMOrcMaterializationUnitDiscardFunction Discard,
            LLVMOrcMaterializationUnitDestroyFunction Destroy
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcAbsoluteSymbols(
            LLVMOrcCSymbolMapPair* Syms,
            nuint NumPairs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcMaterializationUnitRef LLVMOrcLazyReexports(
            LLVMOrcLazyCallThroughManagerRef LCTM,
            LLVMOrcIndirectStubsManagerRef ISM,
            LLVMOrcJITDylibRef SourceRef,
            LLVMOrcCSymbolAliasMapPair* CallableAliases, nuint NumPairs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeMaterializationResponsibility( LLVMOrcMaterializationResponsibilityRef MR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcMaterializationResponsibilityGetTargetDylib( LLVMOrcMaterializationResponsibilityRef MR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcExecutionSessionRef LLVMOrcMaterializationResponsibilityGetExecutionSession( LLVMOrcMaterializationResponsibilityRef MR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(CountElementName=nameof(NumPairs))]
        public static unsafe partial LLVMOrcCSymbolFlagsMapPair[] LLVMOrcMaterializationResponsibilityGetSymbols(
            LLVMOrcMaterializationResponsibilityRef MR,
            out nuint NumPairs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeCSymbolFlagsMap( LLVMOrcCSymbolFlagsMapPair* Pairs );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcSymbolStringPoolEntryRefAlias LLVMOrcMaterializationResponsibilityGetInitializerSymbol( LLVMOrcMaterializationResponsibilityRef MR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial /*LLVMOrcSymbolStringPoolEntryRef[]*/ nint* LLVMOrcMaterializationResponsibilityGetRequestedSymbols(
            LLVMOrcMaterializationResponsibilityRef MR, out nuint NumSymbols
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeSymbols( nint* Symbols );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyResolved(
            LLVMOrcMaterializationResponsibilityRef MR,
            [In] LLVMOrcCSymbolFlagsMapPair[] Symbols, nuint NumPairs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityNotifyEmitted(
            LLVMOrcMaterializationResponsibilityRef MR,
            [In] LLVMOrcCSymbolDependenceGroup[] SymbolDepGroups, nuint NumSymbolDepGroups
            );

        //public static LLVMErrorRef LLVMOrcMaterializationResponsibilityDefineMaterializing(
        //    LLVMOrcMaterializationResponsibilityRef MR,
        //    [In] LLVMOrcCSymbolFlagsMapPair[] Pairs
        //    )
        //{
        //    return LLVMOrcMaterializationResponsibilityDefineMaterializing(MR, Pairs, checked((nuint)Pairs.LongLength));
        //}

        //[LibraryImport( LibraryName )]
        //[UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        //private static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDefineMaterializing(
        //    LLVMOrcMaterializationResponsibilityRef MR,
        //    [In] LLVMOrcCSymbolFlagsMapPair[] Pairs, nuint NumPairs
        //    );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDefineMaterializing(
            LLVMOrcMaterializationResponsibilityRef MR,
            LLVMOrcCSymbolFlagsMapPair* Pairs, nuint NumPairs
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcMaterializationResponsibilityFailMaterialization( LLVMOrcMaterializationResponsibilityRef MR );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityReplace(
            LLVMOrcMaterializationResponsibilityRef MR,
            LLVMOrcMaterializationUnitRef MU
            );

        // NOTE: The normal ArrayMarshaller and SafeHandleMarshaller are NOT capable of handling arrays of references to safe handles,
        // so a caller must manually create a pinned array of the underlying handle values and provide a pointer to the first element.
        // This is normally done with the RefHandleMarshaller class.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcMaterializationResponsibilityDelegate(
            LLVMOrcMaterializationResponsibilityRef MR,
            /*[In] LLVMOrcSymbolStringPoolEntryRef[]*/ nint* Symbols,
            nuint NumSymbols,
            out LLVMOrcMaterializationResponsibilityRef Result
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionCreateBareJITDylib( LLVMOrcExecutionSessionRef ES, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcExecutionSessionCreateJITDylib(
            LLVMOrcExecutionSessionRef ES,
            out LLVMOrcJITDylibRef Result,
            LazyEncodedString Name
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITDylibRef LLVMOrcExecutionSessionGetJITDylibByName( LLVMOrcExecutionSessionRef ES, LazyEncodedString Name );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibCreateResourceTracker( LLVMOrcJITDylibRef JD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcResourceTrackerRef LLVMOrcJITDylibGetDefaultResourceTracker( LLVMOrcJITDylibRef JD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibDefine( LLVMOrcJITDylibRef JD, LLVMOrcMaterializationUnitRef MU );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITDylibClear( LLVMOrcJITDylibRef JD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITDylibAddGenerator( LLVMOrcJITDylibRef JD, LLVMOrcDefinitionGeneratorRef DG );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDefinitionGeneratorRef LLVMOrcCreateCustomCAPIDefinitionGenerator(
            LLVMOrcCAPIDefinitionGeneratorTryToGenerateFunction F,
            void* Ctx,
            LLVMOrcDisposeCAPIDefinitionGeneratorFunction Dispose
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcLookupStateContinueLookup( LLVMOrcLookupStateRef S, LLVMErrorRef Err );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForProcess(
            out LLVMOrcDefinitionGeneratorRef Result,
            sbyte GlobalPrefx,
            LLVMOrcSymbolPredicate Filter,
            void* FilterCtx
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateDynamicLibrarySearchGeneratorForPath(
            out LLVMOrcDefinitionGeneratorRef Result,
            LazyEncodedString FileName,
            sbyte GlobalPrefix,
            LLVMOrcSymbolPredicate Filter,
            void* FilterCtx
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateStaticLibrarySearchGeneratorForPath(
            out LLVMOrcDefinitionGeneratorRef Result,
            LLVMOrcObjectLayerRef ObjLayer,
            LazyEncodedString FileName,
            LazyEncodedString TargetTriple
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeContextRef LLVMOrcCreateNewThreadSafeContext( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LLVMOrcThreadSafeContextGetContext( LLVMOrcThreadSafeContextRef TSCtx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeContext( LLVMOrcThreadSafeContextRef TSCtx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcThreadSafeModuleRef LLVMOrcCreateNewThreadSafeModule( LLVMModuleRef M, LLVMOrcThreadSafeContextRef TSCtx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeThreadSafeModule( LLVMOrcThreadSafeModuleRef TSM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcThreadSafeModuleWithModuleDo( LLVMOrcThreadSafeModuleRef TSM, LLVMOrcGenericIRModuleOperationFunction F, void* Ctx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcJITTargetMachineBuilderDetectHost( out LLVMOrcJITTargetMachineBuilderRef Result );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcJITTargetMachineBuilderRef LLVMOrcJITTargetMachineBuilderCreateFromTargetMachine( LLVMTargetMachineRef TM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeJITTargetMachineBuilder( LLVMOrcJITTargetMachineBuilderRef JTMB );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LazyEncodedString LLVMOrcJITTargetMachineBuilderGetTargetTriple( LLVMOrcJITTargetMachineBuilderRef JTMB );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcJITTargetMachineBuilderSetTargetTriple( LLVMOrcJITTargetMachineBuilderRef JTMB, LazyEncodedString TargetTriple );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFile( LLVMOrcObjectLayerRef ObjLayer, LLVMOrcJITDylibRef JD, LLVMMemoryBufferRef ObjBuffer );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcObjectLayerAddObjectFileWithRT( LLVMOrcObjectLayerRef ObjLayer, LLVMOrcResourceTrackerRef RT, LLVMMemoryBufferRef ObjBuffer );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectLayerEmit( LLVMOrcObjectLayerRef ObjLayer, LLVMOrcMaterializationResponsibilityRef R, LLVMMemoryBufferRef ObjBuffer );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeObjectLayer( LLVMOrcObjectLayerRef ObjLayer );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerEmit( LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcMaterializationResponsibilityRef MR, LLVMOrcThreadSafeModuleRef TSM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcIRTransformLayerSetTransform( LLVMOrcIRTransformLayerRef IRTransformLayer, LLVMOrcIRTransformLayerTransformFunction TransformFunction, void* Ctx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcObjectTransformLayerSetTransform( LLVMOrcObjectTransformLayerRef ObjTransformLayer, LLVMOrcObjectTransformLayerTransformFunction TransformFunction, void* Ctx );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIndirectStubsManagerRef LLVMOrcCreateLocalIndirectStubsManager( LazyEncodedString TargetTriple );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcIndirectStubsManagerRef LLVMOrcCreateLocalIndirectStubsManager( byte* TargetTriple );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeIndirectStubsManager( LLVMOrcIndirectStubsManagerRef ISM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcCreateLocalLazyCallThroughManager(
            LazyEncodedString TargetTriple,
            LLVMOrcExecutionSessionRef ES,
            UInt64 ErrorHandlerAddr,
            out LLVMOrcLazyCallThroughManagerRef LCTM
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeLazyCallThroughManager( LLVMOrcLazyCallThroughManagerRef LCTM );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMOrcDumpObjectsRef LLVMOrcCreateDumpObjects( LazyEncodedString DumpDir, LazyEncodedString IdentifierOverride );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMOrcDisposeDumpObjects( LLVMOrcDumpObjectsRef DumpObjects );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LLVMOrcDumpObjects_CallOperator( LLVMOrcDumpObjectsRef DumpObjects, out LLVMMemoryBufferRef ObjBuffer );
    }
}

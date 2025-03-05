// -----------------------------------------------------------------------
// <copyright file="Disassembler.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // Misplaced using directive; It isn't misplaced - tooling is too brain dead to know the difference between an alias and a using directive
#pragma warning disable IDE0065, SA1200
    using unsafe LLVMOpInfoCallback = delegate* unmanaged[Cdecl]<void* /*Context*/, UInt64 /*PC*/, UInt64 /*Offset*/, UInt64 /*OpSize*/, UInt64 /*InstSize*/, int /*TagType*/, void* /*TagBuf*/, int /*retVal*/>;
    using unsafe LLVMSymbolLookupCallback = delegate* unmanaged[Cdecl]<void* /*Context*/, UInt64 /*ReferenceValue*/, UInt64* /*ReferenceType*/, UInt64 /*ReferencePC*/, byte** /*ReferenceName*/, byte* /*retVal*/>;
#pragma warning restore IDE0065, SA1200

    // These were all originally untyped #defines in the LLVM source
    public static partial class Constants
    {
        /* The option to produce marked up assembly. */
        public const Int32 LLVMDisassembler_Option_UseMarkup = 1;

        /* The option to print immediates as hex. */
        public const Int32 LLVMDisassembler_Option_PrintImmHex = 2;

        /* The option use the other assembler printer variant */
        public const Int32 LLVMDisassembler_Option_AsmPrinterVariant = 4;

        /* The option to set comment on instructions */
        public const Int32 LLVMDisassembler_Option_SetInstrComments = 8;

        /* The option to print latency information alongside instructions */
        public const Int32 LLVMDisassembler_Option_PrintLatency = 16;

        /* The option to print in color */
        public const Int32 LLVMDisassembler_Option_Color = 32;

        /* No input reference type or no output reference type. */
        public const UInt64 LLVMDisassembler_ReferenceType_InOut_None = 0;

        /* The input reference is from a branch instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_Branch = 1;

        /* The input reference is from a PC relative load instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_PCrel_Load = 2;

        /* The input reference is from an ARM64::ADRP instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_ARM64_ADRP = 0x100000001;

        /* The input reference is from an ARM64::ADDXri instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_ARM64_ADDXri = 0x100000002;

        /* The input reference is from an ARM64::LDRXui instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_ARM64_LDRXui = 0x100000003;

        /* The input reference is from an ARM64::LDRXl instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_ARM64_LDRXl = 0x100000004;

        /* The input reference is from an ARM64::ADR instruction. */
        public const UInt64 LLVMDisassembler_ReferenceType_In_ARM64_ADR = 0x100000005;

        /* The output reference is to as symbol stub. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_SymbolStub = 1;

        /* The output reference is to a symbol address in a literal pool. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_LitPool_SymAddr = 2;

        /* The output reference is to a cstring address in a literal pool. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_LitPool_CstrAddr = 3;

        /* The output reference is to a Objective-C CoreFoundation string. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_Objc_CFString_Ref = 4;

        /* The output reference is to a Objective-C message. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_Objc_Message = 5;

        /* The output reference is to a Objective-C message ref. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_Objc_Message_Ref = 6;

        /* The output reference is to a Objective-C selector ref. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_Objc_Selector_Ref = 7;

        /* The output reference is to a Objective-C class ref. */
        public const UInt64 LLVMDisassembler_ReferenceType_Out_Objc_Class_Ref = 8;

        /* The output reference is to a C++ symbol name. */
        public const UInt64 LLVMDisassembler_ReferenceType_DeMangled_Name = 9;
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasm(string TripleName, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasmCPU(string Triple, string CPU, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDisasmContextRef LLVMCreateDisasmCPUFeatures(string Triple, string CPU, string Features, void* DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMSetDisasmOptions(LLVMDisasmContextRef DC, UInt64 Options);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial size_t LLVMDisasmInstruction(LLVMDisasmContextRef DC, nint Bytes, UInt64 BytesSize, UInt64 PC, byte* OutString, size_t OutStringSize);
    }
}

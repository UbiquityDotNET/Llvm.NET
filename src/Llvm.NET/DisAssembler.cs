// -----------------------------------------------------------------------
// <copyright file="DisAssembler.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

// warning CS0649: Field 'xxx' is never assigned to, and will always have its default value 0
#pragma warning disable 649

namespace Llvm.NET
{
    internal class DisAssembler
    {
        /* TODO: Implement DisAssembler */
        internal static class NativeMethods
        {
            internal struct LLVMOpInfoSymbol1
            {
                internal int Present;
                [MarshalAs( UnmanagedType.LPStr )]
                internal string Name;
                internal int Value;
            }

            internal struct LLVMOpInfo1
            {
                internal LLVMOpInfoSymbol1 AddSymbol;
                internal LLVMOpInfoSymbol1 SubtractSymbol;
                internal int Value;
                internal int VariantKind;
            }

            /**
             * The type for the symbol lookup function.  This may be called by the
             * disassembler for things like adding a comment for a PC plus a constant
             * offset load instruction to use a symbol name instead of a load address value.
             * It is passed the block information is saved when the disassembler context is
             * created and the ReferenceValue to look up as a symbol.  If no symbol is found
             * for the ReferenceValue NULL is returned.  The ReferenceType of the
             * instruction is passed indirectly as is the PC of the instruction in
             * ReferencePC.  If the output reference can be determined its type is returned
             * indirectly in ReferenceType along with ReferenceName if any, or that is set
             * to NULL.
             */
            [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
            internal delegate string LLVMSymbolLookupCallback( IntPtr disInfo, int referenceValue, out int referenceType, int referencePC, out IntPtr referenceName );

            [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
            internal delegate int LLVMOpInfoCallback( IntPtr disInfo, int pc, int offset, int size, int tagType, IntPtr tagBuf );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMDisasmContextRef LLVMCreateDisasm( [MarshalAs( UnmanagedType.LPStr )] string TripleName, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPU( [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMDisasmContextRef LLVMCreateDisasmCPUFeatures( [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, [MarshalAs( UnmanagedType.LPStr )] string Features, IntPtr DisInfo, int TagType, LLVMOpInfoCallback GetOpInfo, LLVMSymbolLookupCallback SymbolLookUp );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern int LLVMSetDisasmOptions( LLVMDisasmContextRef DC, int Options );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisasmDispose( LLVMDisasmContextRef DC );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern ulong LLVMDisasmInstruction( LLVMDisasmContextRef DC, IntPtr Bytes, long BytesSize, long PC, IntPtr OutString, size_t OutStringSize );
        }
    }
}

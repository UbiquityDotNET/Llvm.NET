// -----------------------------------------------------------------------
// <copyright file="DisAssembler.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Text;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Options flags for the disassembler</summary>
    [Flags]
    public enum DisassemblerOptions
    {
        /// <summary>No options</summary>
        None = 0,

        /// <summary>Produce marked up assembly</summary>
        UseMarkup = 1,

        /// <summary>Print immediate as HEX</summary>
        ImmediatesAsHex = 2,

        /// <summary>Use the alternate assembly printer variant</summary>
        AsmPrinterVariant = 4,

        /// <summary>Set comments on instructions</summary>
        InstructionComments = 8,

        /// <summary>Print latency information alongside instructions</summary>
        PrintLatency = 16,
    }

    /// <summary>Dis-assembler</summary>
    public class Disassembler
    {
        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="disInfo">Context value passed to <paramref name="infoCallBack"/></param>
        /// <param name="tagType">TODO:Explain this...</param>
        /// <param name="infoCallBack">Op info callback</param>
        /// <param name="symbolLookup">Symbol lookup delegate</param>
        public Disassembler( Triple triple
                           , IntPtr disInfo
                           , int tagType
                           , LLVMOpInfoCallback infoCallBack
                           , LLVMSymbolLookupCallback symbolLookup
                           )
        {
            InfoCallBack = infoCallBack;
            SymbolLookupCallback = symbolLookup;
            DisasmHandle = LLVMCreateDisasm( triple.ToString( ), disInfo, tagType, InfoCallBack, SymbolLookupCallback );
        }

        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="cpu">CPU string for the instruction set</param>
        /// <param name="disInfo">Context value passed to <paramref name="infoCallBack"/></param>
        /// <param name="tagType">TODO:Explain this...</param>
        /// <param name="infoCallBack">Op info callback</param>
        /// <param name="symbolLookup">Symbol lookup delegate</param>
        public Disassembler( Triple triple
                           , string cpu
                           , IntPtr disInfo
                           , int tagType
                           , LLVMOpInfoCallback infoCallBack
                           , LLVMSymbolLookupCallback symbolLookup
                           )
        {
            InfoCallBack = infoCallBack;
            SymbolLookupCallback = symbolLookup;
            DisasmHandle = LLVMCreateDisasmCPU( triple.ToString( ), cpu, disInfo, tagType, InfoCallBack, SymbolLookupCallback );
        }

        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="cpu">CPU string for the instruction set</param>
        /// <param name="features">CPU features for disassembling the instruction set</param>
        /// <param name="disInfo">Context value passed to <paramref name="infoCallBack"/></param>
        /// <param name="tagType">TODO:Explain this...</param>
        /// <param name="infoCallBack">Op info callback</param>
        /// <param name="symbolLookup">Symbol lookup delegate</param>
        public Disassembler( Triple triple
                           , string cpu
                           , string features
                           , IntPtr disInfo
                           , int tagType
                           , LLVMOpInfoCallback infoCallBack
                           , LLVMSymbolLookupCallback symbolLookup
                           )
        {
            InfoCallBack = infoCallBack;
            SymbolLookupCallback = symbolLookup;
            DisasmHandle = LLVMCreateDisasmCPUFeatures( triple.ToString( ), cpu, features, disInfo, tagType, InfoCallBack, SymbolLookupCallback );
        }

        /// <summary>Set the options for the disassembly</summary>
        /// <param name="options">Options</param>
        /// <returns><see langword="true"/> if the options are all supported</returns>
        public bool SetOptions( DisassemblerOptions options)
        {
            return LLVMSetDisasmOptions( DisasmHandle, ( ulong )options );
        }

        /// <summary>Disassembles an instruction</summary>
        /// <param name="instruction">Start of instruction stream</param>
        /// <param name="pc">Program counter address to assume for the instruction disassembly</param>
        /// <param name="stringBufferSize">Size of string buffer to use for the disassembly (default=1024)</param>
        /// <returns>Disassembly string and count of bytes in the instruction as a tuple</returns>
        public (string Disassembly, int InstructionByteCount) Disassemble(ReadOnlySpan<byte> instruction, ulong pc, int stringBufferSize = 1024 )
        {
            var bldr = new StringBuilder(stringBufferSize);
            unsafe
            {
                fixed ( byte* ptr = &MemoryMarshal.GetReference( instruction ) )
                {
                    size_t instSize = LLVMDisasmInstruction(DisasmHandle, (IntPtr)ptr, (ulong)instruction.Length, pc, ref bldr, bldr.Capacity);
                    return (bldr.ToString( ), instSize.ToInt32( ));
                }
            }
        }

        private readonly LLVMOpInfoCallback InfoCallBack;
        private readonly LLVMSymbolLookupCallback SymbolLookupCallback;
        private readonly LLVMDisasmContextRef DisasmHandle;
    }
}

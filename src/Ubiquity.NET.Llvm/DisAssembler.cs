// -----------------------------------------------------------------------
// <copyright file="DisAssembler.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// this file declares and uses the "experimental" interface `IDisassemblerCallbacks`.
#pragma warning disable LLVM002 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

// regions are used sparingly and help mark the NATIVE ONLY callbacks
#pragma warning disable SA1124 // Do not use regions

namespace Ubiquity.NET.Llvm
{
    /// <summary>Interface for disassembler callbacks</summary>
    /// <remarks>
    /// Exact use and purpose of this interface is not well documented in LLVM and it's use
    /// is therefore classified as "experimental". Key to the ambiguity is the parameter and
    /// return types of the methods. LLVM headers use byte* and it isn't clear if it is a "blob",
    /// string or what? Nor, does it say anything about lifetime or ownership of the data they
    /// point to... (This is mostly in regards to the parameters and return of
    /// <see cref="SymbolLookup(ulong, ref ulong, ulong, out string?)"/> but also applies to the
    /// <see cref="nint"/> `TagBuf` parameter of <see cref="OpInfo(ulong, ulong, ulong, ulong, int, nint)"/>
    /// </remarks>
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Closely related only used here" )]
    [Experimental("LLVM002")]
    public interface IDisassemblerCallbacks
    {
        /// <summary>Purpose not fully known or well explained in LLVM docs</summary>
        /// <param name="PC">Program counter</param>
        /// <param name="Offset">offset [Of what? Relative to what?]</param>
        /// <param name="OpSize">OpSize [Not entirely clear what an "op" is or how a size matters]</param>
        /// <param name="InstSize">Instruction size [Also not clear why this matters or what an implementation should do about it]</param>
        /// <param name="TagType">Tag type [Best guess: discriminator type for the opaque buffer]</param>
        /// <param name="TagBuf">Raw pointer to the buffer. [It is currently assumed this is readonly and the size and shape are determined by <paramref name="TagType"/>]</param>
        /// <returns>Unknown</returns>
        int OpInfo(UInt64 PC, UInt64 Offset, UInt64 OpSize, UInt64 InstSize, int TagType, nint TagBuf);

        /// <summary>Performs symbol lookup for the disassembler</summary>
        /// <param name="referenceValue">referenceValue [Unknown what this is in "reference" to]</param>
        /// <param name="referenceType">referenceType [Unknown what this is in "reference" to or why an implementation might need/want to change this]</param>
        /// <param name="referencePC">Unknown but assumed to relate to a program counter</param>
        /// <param name="ReferenceName">Completely unknown. [Is this really an OUT string or an IN array?]</param>
        /// <returns>Unknown</returns>
        [SuppressMessage( "Design", "CA1045:Do not pass types by reference", Justification = "Matches ABI; Otherwise requires returning (and marshalling) a tuple" )]
        string? SymbolLookup(UInt64 referenceValue, ref UInt64 referenceType, UInt64 referencePC, out string? ReferenceName);
    }

    /// <summary>Options flags for the disassembler</summary>
    [Flags]
    public enum DisassemblerOptions
    {
        /// <summary>No options</summary>
        None = 0,

        /// <summary>Produce marked up assembly</summary>
        UseMarkup = Constants.LLVMDisassembler_Option_UseMarkup,

        /// <summary>Print immediate as HEX</summary>
        ImmediateAsHex = Constants.LLVMDisassembler_Option_PrintImmHex,

        /// <summary>Use the alternate assembly printer variant</summary>
        AsmPrinterVariant = Constants.LLVMDisassembler_Option_AsmPrinterVariant,

        /// <summary>Set comments on instructions</summary>
        InstructionComments = Constants.LLVMDisassembler_Option_SetInstrComments,

        /// <summary>Print latency information alongside instructions</summary>
        PrintLatency = Constants.LLVMDisassembler_Option_PrintLatency,

        /// <summary>Print in color</summary>
        Color = Constants.LLVMDisassembler_Option_Color,
    }

    /// <summary>LLVM Disassembler</summary>
    public sealed class Disassembler
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="tagType">TODO: Explain this...</param>
        /// <param name="callBacks">Optional callbacks [Default: <see langword="null"/>]</param>
        /// <remarks>The <paramref name="callBacks"/> parameter is experimental and recommended left as the default value</remarks>
        public Disassembler( Triple triple, int tagType, IDisassemblerCallbacks? callBacks = null)
            : this(triple, string.Empty, string.Empty, tagType, callBacks)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="cpu">CPU string for the instruction set</param>
        /// <param name="tagType">TODO: Explain this...</param>
        /// <param name="callBacks">Optional callbacks [Default: <see langword="null"/>]</param>
        /// <remarks>The <paramref name="callBacks"/> parameter is experimental and recommended left as the default value</remarks>
        public Disassembler( Triple triple
                           , string cpu
                           , int tagType
                           , IDisassemblerCallbacks? callBacks = null
                           )
            : this(triple, cpu, string.Empty, tagType, callBacks)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Disassembler"/> class.</summary>
        /// <param name="triple">Triple for the instruction set to disassemble</param>
        /// <param name="cpu">CPU string for the instruction set</param>
        /// <param name="features">CPU features for disassembling the instruction set</param>
        /// <param name="tagType">TODO:Explain this...</param>
        /// <param name="callBacks">Optional callbacks [Default: <see langword="null"/>]</param>
        /// <remarks>The <paramref name="callBacks"/> parameter is experimental and recommended left as the default value</remarks>
        public Disassembler( Triple triple
                           , string cpu
                           , string features
                           , int tagType
                           , IDisassemblerCallbacks? callBacks = null
                           )
        {
            ArgumentNullException.ThrowIfNull(triple);
            ArgumentNullException.ThrowIfNull(cpu); // may be empty
            ArgumentNullException.ThrowIfNull(features);

            unsafe
            {
                ArgumentNullException.ThrowIfNull(triple);
                CallBacksHandle = callBacks is null ? null : GCHandle.Alloc(callBacks);
                Handle = LLVMCreateDisasmCPUFeatures(
                        triple.ToString( ) ?? string.Empty,
                        cpu,
                        features,
                        CallBacksHandle.HasValue ? GCHandle.ToIntPtr(CallBacksHandle.Value).ToPointer() : null,
                        tagType,
                        CallBacksHandle.HasValue ? &NativeInfoCallBack : null,
                        CallBacksHandle.HasValue ? &NativeSymbolLookupCallback : null
                        );
            }
        }

        /// <summary>Set the options for the disassembly</summary>
        /// <param name="options">Options</param>
        /// <returns><see langword="true"/> if the options are all supported</returns>
        public bool SetOptions( DisassemblerOptions options )
        {
            return LLVMSetDisasmOptions( Handle, ( ulong )options );
        }

        /// <summary>Disassembles an instruction</summary>
        /// <param name="instruction">Start of instruction stream</param>
        /// <param name="pc">Program counter address to assume for the instruction disassembly</param>
        /// <param name="stringBufferSize">Size of string buffer to use for the disassembly (default=1024)</param>
        /// <returns>Disassembly string and count of bytes in the instruction as a tuple</returns>
        public (LazyEncodedString Disassembly, nuint InstructionByteCount) Disassemble( ReadOnlySpan<byte> instruction, ulong pc, int stringBufferSize = 1024 )
        {
            unsafe
            {
                using var nativeMemory = MemoryPool<byte>.Shared.Rent(stringBufferSize);
                using var nativeMemoryPinnedHandle = nativeMemory.Memory.Pin();
                fixed( byte* ptr = &MemoryMarshal.GetReference( instruction ) )
                {
                    byte* pDisasmData = (byte*)nativeMemoryPinnedHandle.Pointer;
                    Debug.Assert(pDisasmData != null, "should never get a null pointer here");
                    nuint instSize = LLVMDisasmInstruction(Handle, ptr, (UInt64)instruction.Length, pc, pDisasmData, (nuint)stringBufferSize);
                    return (LazyEncodedString.FromUnmanaged(pDisasmData)!, instSize);
                }
            }
        }

        private readonly GCHandle? CallBacksHandle;
        private readonly LLVMDisasmContextRef Handle;

        #region Native marshalling callbacks
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "Helps clarify type for native interop" )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe byte* NativeSymbolLookupCallback(void* disInfo, UInt64 referenceValue, UInt64* pReferenceType, UInt64 referencePC, byte** referenceName)
        {
            try
            {
                if (GCHandle.FromIntPtr((nint)disInfo).Target is not IDisassemblerCallbacks callBacks)
                {
                    return null;
                }

                string? managedRetVal = callBacks.SymbolLookup(referenceValue, ref *pReferenceType, referencePC, out string? managedRefName);

                // Use of normal marshalling pattern is broken here... The lifetime of the effectively OUT byte pointer referenceName
                // is undefined. If the marshalling allocates memory for the buffer then it is invalid as soon as this call returns
                // so not much use as an OUT pointer...
                if (referenceName is not null)
                {
                    *referenceName = null;
                }

                // Ditto for the return pointer - Who releases it?
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                // managedRefNameMarshaller.Free();
            }
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe int NativeInfoCallBack(void* disInfo, UInt64 pc, UInt64 offset, UInt64 opSize, UInt64 instSize, int tagType, void* tagBuf)
        {
            try
            {
                return GCHandle.FromIntPtr((nint)disInfo).Target is not IDisassemblerCallbacks callBacks
                    ? 0 // TODO: Is this a legit failure return value?
                    : callBacks.OpInfo(pc, offset, opSize, instSize, tagType, (nint)tagBuf);
            }
            catch
            {
                return 0; // TODO: Is this a legit failure return value?
            }
        }
        #endregion
    }
}

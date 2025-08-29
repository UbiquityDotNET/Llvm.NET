// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// until compiler bug [#40325](https://github.com/dotnet/roslyn/issues/40325) is resolved disable this
// as adding the parameter would result in 'warning: Duplicate parameter 'passes' found in comments, the latter one is ignored.'
// when generating the final documentation. (It picks the first one and reports a warning).
// Unfortunately there's no way to suppress this without cluttering up the code with pragmas.
// [ Sadly the SuppressMessageAttribute does NOT work for this warning. ]
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace Ubiquity.NET.Llvm.Values
{
    /* values for CallingConvention enum come directly from LLVM's CallingConv.h
    // rather then the mapped C API version as the C version is not
    // a complete set.
    */

    /// <summary>Calling Convention for functions</summary>
    /// <seealso href="xref:llvm_langref#calling-conventions">LLVM calling conventions</seealso>
    public enum CallingConvention
    {
        /// <summary> The default llvm calling convention, compatible with C</summary>
        /// <remarks>
        /// This convention is the only calling convention that supports varargs calls.
        /// As with typical C calling conventions, the callee/caller have to
        /// tolerate certain amounts of prototype mismatch
        /// </remarks>
        C = 0,

        // [gap]

        /// <summary>Fast calling convention</summary>
        FastCall = 8,

        /// <summary>Cold calling</summary>
        ColdCall = 9,

        /// <summary>Glasgow Haskell Compiler</summary>
        GlasgowHaskellCompiler = 10,

        /// <summary>The High=Performance Erlang convention</summary>
        HiPE = 11,

        /// <summary>Webkit JavaScript calling convention</summary>
        WebKitJS = 12,

        /// <summary>Calling convention for dynamic register based calls (e.g.stackmap and patchpoint intrinsics)</summary>
        AnyReg = 13,

        /// <summary>Preserve most calling convention for runtime calls that preserves most registers.</summary>
        PreserveMost = 14,

        /// <summary>Preserve all calling convention for runtime calls that preserves (almost) all registers.</summary>
        PreserveAll = 15,

        /// <summary>Swift calling convention</summary>
        Swift = 16,

        /// <summary>Calling convention for access functions.</summary>
        CxxFastTls = 17,

        // [Gap]

        /// <summary>Marker enum that identifies the start of the target specific conventions all values greater than or equal to this value are target specific</summary>
        FirstTargetSpecific = 64, // [marker]

        /// <summary>X86 stdcall convention</summary>
        /// <remarks>
        /// This calling convention is mostly used by the Win32 API. It is basically the same as the C
        /// convention with the difference in that the callee is responsible for popping the arguments
        /// from the stack.
        /// </remarks>
        X86StdCall = FirstTargetSpecific,

        /// <summary>X86 fast call convention</summary>
        /// <remarks>
        /// 'fast' analog of <see cref="X86StdCall"/>. Passes first two arguments
        /// in ECX:EDX registers, others - via stack. Callee is responsible for
        /// stack cleaning.
        /// </remarks>
        X86FastCall = 65,

        /// <summary>ARM APCS (officially obsolete but some old targets use it)</summary>
        ArmAPCS = 66,

        /// <summary>ARM Architecture Procedure Calling Standard calling convention (aka EABI). Soft float variant</summary>
        ArmAAPCS = 67,

        /// <summary>Same as <see cref="ArmAAPCS"/> but uses hard floating point ABI</summary>
        ArmAAPCSVfp = 68,

        /// <summary>Calling convention used for MSP430 interrupt routines</summary>
        MSP430Interrupt = 69,

        /// <summary>Similar to <see cref="X86StdCall"/>, passes first 'this' argument in ECX all others via stack</summary>
        /// <remarks>
        /// Callee is responsible for stack cleaning. MSVC uses this by default for C++ instance methods in its ABI
        /// </remarks>
        X86ThisCall = 70,

        /// <summary>Call to a PTX kernel</summary>
        /// <remarks>Passes all arguments in parameter space</remarks>
        PtxKernel = 71,

        /// <summary>Call to a PTX device function</summary>
        /// <remarks>
        /// Passes all arguments in register or parameter space.
        /// </remarks>
        PtxDevice = 72,

        /// <summary>Calling convention for SPIR non-kernel device functions.</summary>
        SpirFunction = 75,

        /// <summary>Calling convention for SPIR kernel functions.</summary>
        SpirKernel = 76,

        /// <summary>Calling conventions for Intel OpenCL built-ins</summary>
        IntelOpenCLBuiltIn = 77,

        /// <summary>The C convention as specified in the x86-64 supplement to the System V ABI, used on most non-Windows systems.</summary>
        X86x64SysV = 78,

        /// <summary>The C convention as implemented on Windows/x86-64 and AArch64.</summary>
        /// <remarks>
        /// <para>This convention differs from the more common <see cref="X86x64SysV"/> convention in a number of ways, most notably in
        /// that XMM registers used to pass arguments are shadowed by GPRs, and vice versa.</para>
        /// <para>On AArch64, this is identical to the normal C (AAPCS) calling convention for normal functions,
        /// but floats are passed in integer registers to variadic functions</para>
        /// </remarks>
        X86x64Win64 = 79,

        /// <summary>MSVC calling convention that passes vectors and vector aggregates in SSE registers</summary>
        X86VectorCall = 80,

        /// <summary>Calling convention used by HipHop Virtual Machine (HHVM)</summary>
        HHVM = 81,

        /// <summary>HHVM calling convention for invoking C/C++ helpers</summary>
        HHVMCCall = 82,

        /// <summary>x86 hardware interrupt context</summary>
        /// <remarks>
        /// Callee may take one or two parameters, where the 1st represents a pointer to hardware context frame
        /// and the 2nd represents hardware error code, the presence of the later depends on the interrupt vector
        /// taken. Valid for both 32- and 64-bit sub-targets.
        /// </remarks>
        X86Interrupt = 83,

        /// <summary>Used for AVR interrupt routines</summary>
        AVRInterrupt = 84,

        /// <summary>Calling convention used for AVR signal routines</summary>
        AVRSignal = 85,

        /// <summary>Calling convention used for special AVR rtlib functions which have an "optimized" convention to preserve registers.</summary>
        AVRBuiltIn = 86,

        /// <summary>Calling convention used for Mesa vertex shaders.</summary>
        AMDGpuVertexShader = 87,

        /// <summary>Calling convention used for Mesa geometry shaders.</summary>
        AMDGpuGeometryShader = 88,

        /// <summary>Calling convention used for Mesa pixel shaders.</summary>
        AMDGpuPixelShader = 89,

        /// <summary>Calling convention used for Mesa compute shaders.</summary>
        AMDGpuComputeShader = 90,

        /// <summary>Calling convention for AMDGPU code object kernels.</summary>
        AMDGpuKernel = 91,

        /// <summary>Register calling convention used for parameters transfer optimization</summary>
        X86RegCall = 92,

        /// <summary>Calling convention used for Mesa hull shaders. (= tessellation control shaders)</summary>
        AMDGpuHullShader = 93,

        /// <summary>Calling convention used for special MSP430 rtlib functions which have an "optimized" convention using additional registers.</summary>
        MSP430BuiltIn = 94,

        /// <summary>Calling convention used for AMDPAL vertex shader if tessellation is in use.</summary>
        AMDGpuLS = 95,

        /// <summary>Calling convention used for AMDPAL shader stage before geometry shader if geometry is in use.</summary>
        /// <remarks>
        /// Either the domain (= tessellation evaluation) shader if tessellation is in use, or otherwise the vertex shader.
        /// </remarks>
        AMDGpuEs = 96,

        /// <summary>The highest possible calling convention ID. Must be some 2^k - 1.</summary>
        MaxCallingConvention = 1023
    }
}

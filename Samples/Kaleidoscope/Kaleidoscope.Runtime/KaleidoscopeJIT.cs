// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.JIT.OrcJITv2;

namespace Kaleidoscope.Runtime
{
    /// <summary>JIT engine for Kaleidoscope language</summary>
    /// <remarks>
    /// This engine uses the <see cref="Ubiquity.NET.Llvm.JIT.OrcJITv2.LlJIT"/> engine to support lazy
    /// compilation of LLVM IR modules added to the JIT.
    /// </remarks>
    public sealed class KaleidoscopeJIT
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        public KaleidoscopeJIT( )
        {
            SymbolFlags symFlags = new(SymbolGenericOption.Callable);

            // Add a materializer for the well-known symbols for the managed code implementations
            unsafe
            {
                List<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> absoluteSymbols = [
                    new(OrcJit.MangleAndIntern("putchard"), new(MakeRawPtr(&NativeMethods.PutChard), symFlags)),
                    new(OrcJit.MangleAndIntern("printd"), new(MakeRawPtr(&NativeMethods.Printd), symFlags)),
                ];

                using var absoluteMaterializer = new AbsoluteMaterializationUnit(absoluteSymbols);
                OrcJit.MainLib.Define(absoluteMaterializer);
            }
        }

        public ResourceTracker Add(ThreadSafeContext ctx, BitcodeModule module)
        {
            ArgumentNullException.ThrowIfNull(ctx);
            ArgumentNullException.ThrowIfNull(module);

            ResourceTracker retVal = OrcJit.MainLib.CreateResourceTracker();

            // Apply the data JIT's Layout
            module.DataLayoutString = OrcJit.DataLayoutString;

            using ThreadSafeModule tsm = new(ctx, module);
            OrcJit.AddModule(retVal, tsm);
            return retVal;
        }

        public void Dispose()
        {
            OrcJit.Dispose();
        }

        /// <summary>Gets or sets the output writer for output from the program.</summary>
        /// <remarks>The default writer is <see cref="Console.Out"/>.</remarks>
        public static TextWriter OutputWriter { get; set; } = Console.Out;

        /// <summary>Gets the ORCJit for this Kaleidoscope JIT instance</summary>
        public LlJIT OrcJit { get; } = new();

        // Cleaner workaround for ugly compiler casting requirements
        // The & operator officially has no type and MUST be provided
        // so the parameter type does the job and allows a simple cast
        // the the required JIT address form.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe UInt64 MakeRawPtr(delegate* unmanaged[Cdecl]<double, double> funcPtr)
        {
            return (UInt64)funcPtr;
        }

        private static class NativeMethods
        {
            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static double Printd( double x )
            {
                // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
                try
                {
                    OutputWriter?.WriteLine( x );
                    return 0.0F;
                }
                catch
                {
                    return 0.0;
                }
            }

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static double PutChard( double x )
            {
                // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
                try
                {
                    OutputWriter?.Write( ( char )x );
                    return 0.0F;
                }
                catch
                {
                    return 0.0;
                }
            }
        }
    }
}

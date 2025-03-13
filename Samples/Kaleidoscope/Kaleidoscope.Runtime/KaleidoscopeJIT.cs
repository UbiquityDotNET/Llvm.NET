// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.InteropHelpers;
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
        : LlJIT
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        public KaleidoscopeJIT( )
        {
            SymbolFlags symFlags = new(SymbolGenericOption.Callable);

            // Add a materializer for the well-known symbols for the managed code implementations
            unsafe
            {
                List<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> absoluteSymbols = [
                    new(MangleAndIntern("putchard"), new(MakeRawPtr(&NativeMethods.PutChard), symFlags)),
                    new(MangleAndIntern("printd"), new(MakeRawPtr(&NativeMethods.Printd), symFlags)),
                ];

                using var absoluteMaterializer = new AbsoluteMaterializationUnit(absoluteSymbols);
                MainLib.Define(absoluteMaterializer);
            }

            TransformLayer.SetTransform(ModuleTransformer);
        }

        /// <summary>Adds a module to this JIT with removal tracking</summary>
        /// <param name="ctx">Thread safe context this module is part of</param>
        /// <param name="module">Module to add</param>
        /// <returns>Resource tracker for this instance</returns>
        public ResourceTracker Add(ThreadSafeContext ctx, BitcodeModule module)
        {
            ArgumentNullException.ThrowIfNull(ctx);
            ArgumentNullException.ThrowIfNull(module);

            ResourceTracker retVal = MainLib.CreateResourceTracker();

            using ThreadSafeModule tsm = new(ctx, module);
            AddModule(retVal, tsm);
            return retVal;
        }

        public IReadOnlyCollection<string> Passes
        {
            get => new ReadOnlyCollection<string>(OptimizationPasses);
            set => OptimizationPasses = [.. value.ThrowIfNull()];
        }

        /// <summary>Gets or sets the output writer for output from the program.</summary>
        /// <remarks>The default writer is <see cref="Console.Out"/>.</remarks>
        public static TextWriter OutputWriter { get; set; } = Console.Out;

        // Optimization passes used in the transform function for materialized modules
        // Default set covers the basics; ore are possible and may produce better results
        // but could also end up taking more effort than just materializing the native code
        // and executing it...
        private string[] OptimizationPasses = [
                "mem2reg",
                "simplifycfg",
                "instcombine",
                "reassociate",
                "gvn",
        ];

        private void ModuleTransformer(ThreadSafeModule module, MaterializationResponsibility responsibility, out ThreadSafeModule? replacementModule)
        {
            // This implementation does not replace the module
            replacementModule = null;

            // work on the module directly
            module.WithPerThreadModule((module)=>
            {
                // force it to use the JIT's data layout
                module.DataLayoutString = DataLayoutString;

                // perform optimizations
                return module.TryRunPasses(OptimizationPasses);
            });
        }

        // Cleaner workaround for ugly compiler casting requirements
        // The & operator officially has no type and MUST be cast.
        // Thus the provided parameter type does the job and allows
        // a simple cast to the required JIT address form.
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

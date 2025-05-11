// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.OrcJITv2;

namespace Kaleidoscope.Runtime
{
    /// <summary>JIT engine for Kaleidoscope language</summary>
    /// <remarks>
    /// This engine uses the <see cref="Ubiquity.NET.Llvm.OrcJITv2.LlJIT"/> engine to support lazy
    /// compilation of LLVM IR modules added to the JIT.
    /// </remarks>
    public sealed class KaleidoscopeJIT
        : LlJIT
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        /// <remarks>This creates a JIT with a default set of passes for 'O3'</remarks>
        public KaleidoscopeJIT()
            : this("default<O3>"u8)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        /// <param name="optimizationPasses">Optimization passes to use for each module transform in this JIT</param>
        public KaleidoscopeJIT( params LazyEncodedString[] optimizationPasses )
        {
            OptimizationPasses = optimizationPasses;
            SymbolFlags symFlags = new(SymbolGenericOption.Callable);

            // Add a materializer for the well-known symbols for the managed code implementations
            unsafe
            {
                using var putchardName = MangleAndIntern("putchard"u8);
                using var printdName = MangleAndIntern("printd"u8);

                var absoluteSymbols = new KvpArrayBuilder<SymbolStringPoolEntry, EvaluatedSymbol> {
                    [putchardName] = new(MakeRawPtr(&BuiltIns.PutChard), symFlags),
                    [printdName] = new(MakeRawPtr(&BuiltIns.Printd), symFlags),
                }.ToImmutable();

                using var absoluteMaterializer = new AbsoluteMaterializationUnit(absoluteSymbols);
                MainLib.Define(absoluteMaterializer);
            }

            TransformLayer.SetTransform(ModuleTransformer);
        }

        /// <summary>Adds a module to this JIT with removal tracking</summary>
        /// <param name="ctx">Thread safe context this module is part of</param>
        /// <param name="module">Module to add</param>
        /// <returns>Resource tracker for this instance</returns>
        [MustUseReturnValue]
        public ResourceTracker AddWithTracking(ThreadSafeContext ctx, Module module)
        {
            ArgumentNullException.ThrowIfNull(ctx);
            ArgumentNullException.ThrowIfNull(module);

            ResourceTracker tracker = MainLib.CreateResourceTracker();

            using ThreadSafeModule tsm = new(ctx, module);
            AddModule(tracker, tsm);
            return tracker;
        }

        /// <summary>Gets or sets the output writer for output from the program.</summary>
        /// <remarks>The default writer is <see cref="Console.Out"/>.</remarks>
        public static TextWriter OutputWriter { get; set; } = Console.Out;

        // Optimization passes used in the transform function for materialized modules
        // Default constructor set covers the basics; more are possible and may produce
        // better results but could also end up taking more effort than just materializing
        // the native code and executing it... Exploration with LLVM's `OPT` tool is encouraged.
        private readonly LazyEncodedString[] OptimizationPasses;

        // call back to handle per module transforms in the JIT
        private void ModuleTransformer(ThreadSafeModule module, MaterializationResponsibility responsibility, out ThreadSafeModule? replacementModule)
        {
            // This implementation does not replace the module
            replacementModule = null;

            // work on the per thread module directly
            module.WithPerThreadModule((module)=>
            {
                // force it to use the JIT's data layout
                module.DataLayoutString = DataLayoutString;

                // perform optimizations on the whole module if there are
                // any passes for this JIT instance.
                return OptimizationPasses.Length == 0 ? default : module.TryRunPasses(OptimizationPasses);
            });
        }

        // Cleaner workaround for ugly compiler casting requirements
        // The & operator officially has no type and MUST be cast to
        // or passed into a value of a function pointer type.
        // Thus the provided parameter type does the job and allows
        // a simple cast to the required JIT address form.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe UInt64 MakeRawPtr(delegate* unmanaged[Cdecl]<double, double> funcPtr)
        {
            return (UInt64)funcPtr;
        }

        // These are the built-in functions known by the Kaleidoscope JIT and used by many of the sample chapters.
        // They are registered with the JIT in the constructor as absolute symbols.
        private static class BuiltIns
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

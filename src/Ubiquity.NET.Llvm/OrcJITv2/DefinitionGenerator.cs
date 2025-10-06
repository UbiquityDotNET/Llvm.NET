// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Definition Generator for ORC JIT v2</summary>
    public class DefinitionGenerator
        : DisposableObject
    {
        /// <inheritdoc/>
        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "OWNED by this class; Constructor has move semantics" )]
        protected override void Dispose( bool disposing )
        {
            if(disposing && !Handle.IsNull)
            {
                Handle.Dispose();
                InvalidateAfterMove();
            }

            base.Dispose( disposing );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvalidateAfterMove( )
        {
            Handle = default;
        }

        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Ordering is correct, analyzer is too rigid to allow customization" )]
        internal DefinitionGenerator( LLVMOrcDefinitionGeneratorRef h )
        {
            Handle = h;
        }

        internal LLVMOrcDefinitionGeneratorRef Handle { get; private set; }
    }
}

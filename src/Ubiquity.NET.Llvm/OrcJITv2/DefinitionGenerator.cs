// -----------------------------------------------------------------------
// <copyright file="DefinitionGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Definition Generator for Orc JIT v2</summary>
    public class DefinitionGenerator
        : DisposableObject
    {
        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
                Handle.Dispose();
            }

            base.Dispose(disposing);
        }

        internal DefinitionGenerator(LLVMOrcDefinitionGeneratorRef h)
        {
            Handle = h.Move();
        }

        internal LLVMOrcDefinitionGeneratorRef Handle {get;}
    }
}

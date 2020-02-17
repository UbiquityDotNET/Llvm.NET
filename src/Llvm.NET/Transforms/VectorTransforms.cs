// -----------------------------------------------------------------------
// <copyright file="VectorTransforms.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Transforms
{
    /// <summary>Extension methods for adding vector transform passes</summary>
    public static class VectorTransforms
    {
        /// <summary>Adds a loop vectorizer pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        public static T AddLoopVectorizePass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddLoopVectorizePass( passManager.Handle );
            return passManager;
        }

        /// <summary>Adds a bottom-up SLP vectorizer pass</summary>
        /// <typeparam name="T"><see cref="PassManager"/> type</typeparam>
        /// <param name="passManager">Pass manager to add the pass to</param>
        /// <returns><paramref name="passManager"/> for fluent style construction of a pass manager</returns>
        public static T AddSLPVectorizePass<T>( [ValidatedNotNull] this T passManager )
            where T : PassManager
        {
            passManager.ValidateNotNull( nameof( passManager ) );
            LLVMAddSLPVectorizePass( passManager.Handle );
            return passManager;
        }
    }
}

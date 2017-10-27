// <copyright file="VectorTransforms.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    public static class VectorTransforms
    {
        public static T AddLoopVectorizePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddLoopVectorizePass( passManager.Handle );
            return passManager;
        }

        public static T AddSLPVectorizePass<T>( this T passManager )
            where T : PassManager
        {
            LLVMAddSLPVectorizePass( passManager.Handle );
            return passManager;
        }
    }
}

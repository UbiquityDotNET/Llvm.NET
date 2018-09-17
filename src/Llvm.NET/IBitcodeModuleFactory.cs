// <copyright file="IBitcodeModuleFactory.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.DebugInfo;

namespace Llvm.NET
{
    /// <summary>Interface for a <see cref="BitcodeModule"/> factory</summary>
    /// <remarks>
    /// Modules are owned by the context and thus not created, freestanding.
    /// This interface provides factory methods for constructing modules. It
    /// is implemented by the <see cref="Context"/> and also internally by
    /// the handle caches that ultimately call the underlying LLVM module
    /// creation APIs.
    /// </remarks>
    public interface IBitcodeModuleFactory
    {
        /// <summary>Creates a new instance of the <see cref="BitcodeModule"/> class in this context</summary>
        /// <returns><see cref="BitcodeModule"/></returns>
        BitcodeModule CreateBitcodeModule( );

        /// <summary>Creates a new instance of the <see cref="BitcodeModule"/> class in a given context</summary>
        /// <param name="moduleId">Module's ID</param>
        /// <returns><see cref="BitcodeModule"/></returns>
        BitcodeModule CreateBitcodeModule( string moduleId );

        /// <summary>Initializes a new instance of the <see cref="BitcodeModule"/> class with a root <see cref="DICompileUnit"/> to contain debugging information</summary>
        /// <param name="moduleId">Module name</param>
        /// <param name="language">Language to store in the debugging information</param>
        /// <param name="srcFilePath">path of source file to set for the compilation unit</param>
        /// <param name="producer">Name of the application producing this module</param>
        /// <param name="optimized">Flag to indicate if the module is optimized</param>
        /// <param name="compilationFlags">Additional flags (use <see cref="string.Empty"/> if none are needed)</param>
        /// <param name="runtimeVersion">Runtime version if any (use 0 if the runtime version has no meaning)</param>
        /// <returns><see cref="BitcodeModule"/></returns>
        BitcodeModule CreateBitcodeModule( string moduleId
                                         , SourceLanguage language
                                         , string srcFilePath
                                         , string producer
                                         , bool optimized = false
                                         , string compilationFlags = ""
                                         , uint runtimeVersion = 0
                                         );
    }
}

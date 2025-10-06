// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.PassBuilderOptionsBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.PassBuilder;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Class to hold options for an LLVM pass builder</summary>
    public ref struct PassBuilderOptions
    {
        /// <summary>Initializes a new instance of the <see cref="PassBuilderOptions"/> struct.</summary>
        public PassBuilderOptions( )
            : this( LLVMCreatePassBuilderOptions() )
        {
        }

        /* NOTE:
            The property getters here ALL use the LibLLVM LibLLVMPassBuilderOptionsGetxxx functions
            as LLVM does not expose any api for retrieving the property values.
        */

        /// <summary>Gets or Sets a value indicating whether the Verifier pass is added.</summary>
        /// <remarks>
        /// The verifier pass ensures that all functions in the module are valid. This
        /// is especially useful during debugging and IR generation development.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool VerifyEach
        {
            get => LibLLVMPassBuilderOptionsGetVerifyEach( Handle );
            set => LLVMPassBuilderOptionsSetVerifyEach( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether debug logging is used when running passes</summary>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool DebugLogging
        {
            get => LibLLVMPassBuilderOptionsGetDebugLogging( Handle );
            set => LLVMPassBuilderOptionsSetDebugLogging( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool LoopInterleaving
        {
            get => LibLLVMPassBuilderOptionsGetLoopInterleaving( Handle );
            set => LLVMPassBuilderOptionsSetLoopInterleaving( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool LoopVectorization
        {
            get => LibLLVMPassBuilderOptionsGetLoopVectorization( Handle );
            set => LLVMPassBuilderOptionsSetLoopVectorization( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool SLPVectorization
        {
            get => LibLLVMPassBuilderOptionsGetSLPVectorization( Handle );
            set => LLVMPassBuilderOptionsSetSLPVectorization( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool LoopUnrolling
        {
            get => LibLLVMPassBuilderOptionsGetLoopUnrolling( Handle );
            set => LLVMPassBuilderOptionsSetLoopUnrolling( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool ForgetAllSCEVInLoopUnroll
        {
            get => LibLLVMPassBuilderOptionsGetForgetAllSCEVInLoopUnroll( Handle );
            set => LLVMPassBuilderOptionsSetForgetAllSCEVInLoopUnroll( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool CallGraphProfile
        {
            get => LibLLVMPassBuilderOptionsGetCallGraphProfile( Handle );
            set => LLVMPassBuilderOptionsSetCallGraphProfile( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether the optimization should honor this functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public bool MergeFunctions
        {
            get => LibLLVMPassBuilderOptionsGetMergeFunctions( Handle );
            set => LLVMPassBuilderOptionsSetMergeFunctions( Handle, value );
        }

        /// <summary>Gets or sets a value for a custom Alias Analysis pipeline</summary>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public string AliasAnalysisPipeline
        {
            get => LibLLVMPassBuilderOptionsGetAAPipeline( Handle ) ?? string.Empty;
            set => LLVMPassBuilderOptionsSetAAPipeline( Handle, value ); // BUSTED, lifetime of the memory provided MUST outlive this instance... :(
        }

        /// <summary>Gets or Sets a value for the limit of the optimization functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public uint LicmMssaOptCap
        {
            get => LibLLVMPassBuilderOptionsGetLicmMssaOptCap( Handle );
            set => LLVMPassBuilderOptionsSetLicmMssaOptCap( Handle, value );
        }

        /// <summary>Gets or Sets a value for the limit of the optimization functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public uint LicmMssaNoAccForPromotionCap
        {
            get => LibLLVMPassBuilderOptionsGetLicmMssaNoAccForPromotionCap( Handle );
            set => LLVMPassBuilderOptionsSetLicmMssaNoAccForPromotionCap( Handle, value );
        }

        /// <summary>Gets or Sets a value for the limit of the optimization functionality</summary>
        /// <remarks>
        /// LLVM-C APIs are lacking documentation on this, so the information is sadly vague.
        /// </remarks>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public int InlinerThreshold
        {
            get => LibLLVMPassBuilderOptionsGetInlinerThreshold( Handle );
            set => LLVMPassBuilderOptionsSetInlinerThreshold( Handle, value );
        }

        /// <summary>Disposes the underlying LLVM handle</summary>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Semantics - not readonly" )]
        public void Dispose( )
        {
            if(!Handle.IsNull)
            {
                Handle.Dispose();
                Handle = default;
            }
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "Constructor uses move semantics")]
        internal LLVMPassBuilderOptionsRef Handle { get; private set; }

        private PassBuilderOptions( LLVMPassBuilderOptionsRef h )
        {
            Handle = h;
        }
    }
}

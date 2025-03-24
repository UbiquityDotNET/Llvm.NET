// -----------------------------------------------------------------------
// <copyright file="CStringHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if SUPPORT_LAZY_DISPOSE_MESSAGE_STRING
namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Manages Common LLVM strings that use the `LLVMDisposeMessage` API to release the resources for the string</summary>
    /// <remarks>While this does place a burden of ownership on callers it does defer the marshalling/dispose as late as possible</remarks>
    public class DisposeMessageString
        : CStringHandle
    {
        /// <summary>Default constructor is required for marshalling</summary>
        public DisposeMessageString()
        {
        }

        protected override bool ReleaseHandle()
        {
            LLVMDisposeMessage( handle );
            return true;

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeMessage(nint p);
        }
    }
}
#endif

// <copyright file="GlobalValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Linkage specification for functions and globals</summary>
    /// <seealso href="xref:llvm_langref#linkage-types">LLVM Linkage Types</seealso>
    public enum Linkage
    {
        /// <summary>Externally visible Global</summary>
        External = LLVMLinkage.LLVMExternalLinkage,    /*< Externally visible function */

        /// <summary>Available Externally</summary>
        /// <remarks>Globals with “available_externally” linkage are never emitted into the object file corresponding to the LLVM module.
        /// From the linker’s perspective, an available_externally global is equivalent to an external declaration. They exist to allow
        /// inlining and other optimizations to take place given knowledge of the definition of the global, which is known to be somewhere
        /// outside the module. Globals with available_externally linkage are allowed to be discarded at will, and allow inlining and other
        /// optimizations. This linkage type is only allowed on definitions, not declarations.
        /// </remarks>
        AvailableExternally = LLVMLinkage.LLVMAvailableExternallyLinkage,

        /// <summary>Keep a single copy when linking</summary>
        LinkOnceAny = LLVMLinkage.LLVMLinkOnceAnyLinkage,

        /// <summary>Like <see cref="LinkOnceAny"/> but can only be replaced by equivalent (One Definition Rule)</summary>
        LinkOnceODR = LLVMLinkage.LLVMLinkOnceODRLinkage,

        // LLVMLinkage.LLVMLinkOnceODRAutoHideLinkage, /**< Obsolete */

        /// <summary>Keep one copy when linking (weak)</summary>
        Weak = LLVMLinkage.LLVMWeakAnyLinkage,

        /// <summary>Like <seealso cref="Weak"/> but only replaced by something equivalent (e.g. One Definition Rule)</summary>
        WeakODR = LLVMLinkage.LLVMWeakODRLinkage,

        /// <summary>Special purpose, applies only to global arrays</summary>
        /// <seealso href="xref:llvm_langref#linkage-types"/>
        Append = LLVMLinkage.LLVMAppendingLinkage,

        /// <summary>Rename collision when linking (i.e static function)</summary>
        Internal = LLVMLinkage.LLVMInternalLinkage,

        /// <summary>Link as <see cref="Internal"/> but omit from the generated symbol table</summary>
        Private = LLVMLinkage.LLVMPrivateLinkage,

        /// <summary>Global to be imported from a DLL</summary>
        DllImport = LLVMLinkage.LLVMDLLImportLinkage,

        /// <summary>Global to be Exported from a DLL</summary>
        DllExport = LLVMLinkage.LLVMDLLExportLinkage,

        /// <summary>External weak linkage</summary>
        /// <remarks>
        /// The semantics of this linkage follow the ELF object file model: the symbol is weak until linked,
        /// if not linked, the symbol becomes null instead of being an undefined reference.
        /// </remarks>
        ExternalWeak = LLVMLinkage.LLVMExternalWeakLinkage,/*< ExternalWeak linkage description */

        // LLVMLinkage.LLVMGhostLinkage,       /*< Obsolete */

        /// <summary>Tenatative definitions</summary>
        Common = LLVMLinkage.LLVMCommonLinkage,

        /// <summary>Like <see cref="Private"/> but the linker remove this symbol</summary>
        LinkerPrivate = LLVMLinkage.LLVMLinkerPrivateLinkage,

        /// <summary>Weak form of <see cref="LinkerPrivate"/></summary>
        LinkerPrivateWeak = LLVMLinkage.LLVMLinkerPrivateWeakLinkage /*< Like LinkerPrivate, but is weak. */
    }

    // TODO: auto enforce default visibilty in Linkage setter(s)
    // TODO: verify default visbility in globalvalue factory methods

    /// <summary>Enumeration for the visibility of a global value</summary>
    /// <remarks>A symbol with <see cref="Linkage.Internal"/> or <see cref="Linkage.Private"/> must have <see cref="Default"/> visibility</remarks>
    /// <seealso href="xref:llvm_lang_ref#visibility-styles">LLVM Visibility Styles</seealso>
    public enum Visibility
    {
        /// <summary>Default visibility for a <see cref="GlobalValue"/></summary>
        Default = LLVMVisibility.LLVMDefaultVisibility,

        /// <summary>Two declarations of an object with hidden visibility refer to the same object if they are in the same shared object</summary>
        Hidden = LLVMVisibility.LLVMHiddenVisibility,

        /// <summary>Symbol cannot be overridden by another module</summary>
        Protected = LLVMVisibility.LLVMProtectedVisibility
    }

    /// <summary>LLVM Global value </summary>
    public class GlobalValue
        : Constant
    {
        /// <summary>Gets or sets the visibility of this global value</summary>
        public Visibility Visibility
        {
            get => ( Visibility )NativeMethods.LLVMGetVisibility( ValueHandle );
            set => NativeMethods.LLVMSetVisibility( ValueHandle, ( LLVMVisibility )value );
        }

        /// <summary>Gets or sets the linkage specification for this symbol</summary>
        public Linkage Linkage
        {
            get => ( Linkage )NativeMethods.LLVMGetLinkage( ValueHandle );
            set => NativeMethods.LLVMSetLinkage( ValueHandle, ( LLVMLinkage )value );
        }

        /// <summary>Gets or sets a value indicating whether this is an Unnamed address</summary>
        public bool UnnamedAddress
        {
            get => NativeMethods.LLVMHasUnnamedAddr( ValueHandle );
            set => NativeMethods.LLVMSetUnnamedAddr( ValueHandle, value );
        }

        /// <summary>Gets a value indicating whether this is a declaration</summary>
        public bool IsDeclaration => NativeMethods.LLVMIsDeclaration( ValueHandle );

        /// <summary>Gets the Module containing this global value</summary>
        public BitcodeModule ParentModule => NativeType.Context.GetModuleFor( NativeMethods.LLVMGetGlobalParent( ValueHandle ) );

        internal GlobalValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

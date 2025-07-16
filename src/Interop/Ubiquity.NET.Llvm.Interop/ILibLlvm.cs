// -----------------------------------------------------------------------
// <copyright file="ILibLlvm.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Versioning;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interface to the core LLVM library itself</summary>
    /// <remarks>
    /// When this instance is disposed the LLVM libraries are no longer usable in the process
    /// <note type="important">
    /// It is important to note that the LLVM library does NOT currently support re-initialization in
    /// the same process. Therefore, it is recommended that initialization is done once at process startup
    /// and then the resulting interface disposed just before the process exits.
    /// </note>
    /// </remarks>
    public interface ILibLlvm
        : IDisposable
    {
        /// <summary>Registers components for the specified targets</summary>
        /// <param name="target">ResolverTarget architecture to register/initialize</param>
        /// <param name="registrations">Flags indicating which components for the target to register/enable</param>
        void RegisterTarget( LibLLVMCodeGenTarget target, LibLLVMTargetRegistrationKind registrations = LibLLVMTargetRegistrationKind.TargetRegistration_CodeGen );

        /// <summary>Gets the full list of all targets supported by this instance of the native library</summary>
        /// <remarks>
        /// This is a simple set of enumerated values for the known targets supported by the library. It
        /// is distinct from the registered targets. Registration of each top level enumerated target may indeed
        /// register support for more targets (e.g., ARM includes thumb big and little endian targets). This reports
        /// only the top level architectural targets that are supported and available to register not what is currently
        /// registered.
        /// </remarks>
        ImmutableArray<LibLLVMCodeGenTarget> SupportedTargets { get; }

        /// <summary>Gets version information for the library implementation</summary>
        /// <returns><see cref="FileVersionQuad"/> for the native interop library</returns>
        /// <remarks>
        /// <note type="note">Since it is dealing with an extension of LLVM, the version is NOT guaranteed to match that
        /// of LLVM itself. Though, to avoid confusion, the major, minor and patch usually does and would only deviate
        /// when no option is available (Such as the extension APIs changed even though LLVM itself did not)</note>
        /// </remarks>
        SemVer ExtendedAPIVersion { get; }

        /// <summary>Gets version information for LLVM</summary>
        /// <remarks>
        /// This is a short hand wrapper around the <see cref="ABI.llvm_c.Core.LLVMGetVersion(out uint, out uint, out uint)"/>
        /// for a given library instance. It converts the multiple out params to a <see cref="SemVer"/>.
        /// <note type="note">
        /// LLVM and the underlying APIs do not support any form of the pre-release information, nor do they support or provide
        /// the build meta (which doesn't participate in ordering anyway). Therefore, any ordering from <see cref="SemVerComparer"/>
        /// or <see cref="SemVerComparer.CaseSensitive"/> is valid for these versions and will produce the correct ordering.
        /// </note>
        /// </remarks>
        SemVer LlvmVersion { get; }
    }
}

// -----------------------------------------------------------------------
// <copyright file="ILibLlvm.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
        void RegisterTarget(LibLLVMCodeGenTarget target, LibLLVMTargetRegistrationKind registrations = LibLLVMTargetRegistrationKind.TargetRegistration_CodeGen);

        /// <summary>Gets the full list of all targets supported by this instance of the native library</summary>
        /// <remarks>
        /// This is a simple set of enumerated values for the known targets supported by the library. It
        /// is distinct from the registered targets. Registration of each top level enumerated target may indeed
        /// register support for more targets (e.g., ARM includes thumb big and little endian targets).
        /// </remarks>
        ImmutableArray<LibLLVMCodeGenTarget> SupportedTargets {get;}
    }
}

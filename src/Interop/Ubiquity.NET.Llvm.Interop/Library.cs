// <copyright file="StaticState.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System.Reflection;
using Ubiquity.NET.Extensions;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetRegistration;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed partial class Library
        : ILibLlvm
    {
        public void RegisterTarget(CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All)
        {
            // NOTE: All logic for handling the targets is in native code
            //       If an invalid target is provided for the library loaded
            //       in InitializeLLVM() below then the native code generates
            //       an error which is transformed to an exception here.
            LibLLVMRegisterTarget(target, registrations).ThrowIfFailed();
        }

        /// <summary>Initializes the native LLVM library support</summary>
        /// <param name="target">Target to use in resolving the proper library that implements the LLVM native code. [Default: CodeGenTarget.Native]</param>
        /// <returns><see cref="ILibLlvm"/> implementation for the library</returns>
        /// <remarks>
        /// <para>This can be called multiple times per application BUT all such calls MUST use the same value for
        /// <paramref name="target"/> in order to load the underlying native LLVM library.</para>
        /// <para><see cref="Dispose()"/> will release any resources allocated by the library but NOT the library itself.
        /// That is loaded once the first time this is called. The .NET runtime does *NOT* support re-load of a P/Invoke
        /// library within the same process. Thus, this is best used at the top level of the application and released at
        /// or near process exit. An access violation crash is likely to occur if any attempts to use the library's functions
        /// occurs after it is unloaded as there is no way to invalidate the results of resolving the method + library into
        /// an address.</para>
        /// <para>While any variant of the native library will support <see cref="CodeGenTarget.Native"/> they can support up
        /// to one other target. Thus if the consumer is ever going to support/ cross-platform scenarios, then it MUST specify
        /// the target the first time this is called. This restriction is a tradeoff from the cost of building the native interop
        /// library. Building all possible processor targets into a single library for every possible runtime is just not feasible
        /// in the automated builds for most projects let alone a no budget OSS project like this one.</para>
        /// </remarks>
        /// <ImplementationNote>
        /// The constraint on native+one target is currently not used in reality. It is theoretical at
        /// best. (Needs changes to and testing of native library generation) Currently this library ONLY supports Win-x64 as the
        /// native target/runtime BUT any target supported by LLVM is OK. (But for future compat the restriction on initialization
        /// is retained, callers cannot re-init to a different target)
        /// </ImplementationNote>
        /// <exception cref="InvalidOperationException">Native Interop library already loaded for a different target</exception>
        /// <exception cref="ArgumentOutOfRangeException">The target provided is undefined or <see cref="CodeGenTarget.All"/></exception>
        public static ILibLlvm InitializeLLVM(CodeGenTarget target = CodeGenTarget.Native)
        {
// NOTHING in this "zone" may use P/Invoke to the native LLVM interop (LibLLVM)
// This sets up the resolver AND the values it requires - interop calls may not
// occur until that is complete.
#region NO P/INVOKE ZONE
            target.ThrowIfNotDefined();
            if(target == CodeGenTarget.All)
            {
                throw new ArgumentOutOfRangeException(nameof(target), "Cannot load library for ALL available targets; ONLY one is allowed");
            }

            // if target is requested for native, adjust it to the the explicit target
            // to allow resolving to the correct library (Native is always an option)
            if (target == CodeGenTarget.Native)
            {
                target = GetNativeTarget();
            }

            // Only setup an import resolver once per process. Multiple attempts would result in an exception
            // Once a library is loaded it cannot be unloaded and reloaded. The runtime will ONLY call the
            // resolver once per imported API. After it has the module and the address of the symbol it replaces
            // the P/Invoke in the same way that native code replaces the thunk for a dllimport. That is, once
            // the address of the symbol is known, no further resolution is used.
            if(!Interlocked.Exchange(ref ResolverApplied, true))
            {
                ResolverTarget = target == CodeGenTarget.Native ? GetNativeTarget() : target;
                NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), NativeLibResolver);
            }
#endregion

            if (ResolverTarget != target)
            {
                throw new InvalidOperationException("Cannot re-initialize to a different target");
            }

            // Verify the version of LLVM in LibLLVM, this will trigger the resolver to load
            // the DLL and set the Native Library handle in NativeLibHandle if not already loaded
            LLVMGetVersion( out uint actualMajor, out uint actualMinor, out uint actualPatch );
            if(actualMajor != SupportedVersionMajor
            || actualMinor != SupportedVersionMinor
            || actualPatch < SupportedVersionPatch
            )
            {
                string msgFmt = Resources.Mismatched_LibLLVM_version_Expected_0_1_2_Actual_3_4_5;
                string msg = string.Format( CultureInfo.CurrentCulture
                                          , msgFmt
                                          , SupportedVersionMajor
                                          , SupportedVersionMinor
                                          , SupportedVersionPatch
                                          , actualMajor
                                          , actualMinor
                                          , actualPatch
                                          );
                throw new InvalidOperationException( msg );
            }

            return new Library();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            LLVMShutdown();
        }

        // Expected version info for verification of matched LibLLVM
        private const int SupportedVersionMajor = 20;
        private const int SupportedVersionMinor = 1;
        private const int SupportedVersionPatch = 0;

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler(byte* reason)
        {
            try
            {
                // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", ExecutionEncodingStringMarshaller.ConvertToManaged( reason ) );
            }
            catch
            {
            }
        }

        // NOTE: This needs to be VERY fast as it is called for EVERY P/Invoke encountered.
        // BUT it is ONLY called once per import. After the import is resolved (LL+GPA) then
        // the resulting address is used internally without involving this callback. Thus the
        // library must remain valid until the app will never again call the imports. There is
        // no way to "invalidate" previous resolution as the final resolution to an address is
        // outside the control of this function.
        private static nint NativeLibResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
        {
            // Any library other than the one known about here gets default handling
            // There must be an initialized Library instance or there's no point in
            // this custom resolver
            if(libraryName != LibraryName )
            {
                return nint.Zero;
            }

            // if not already loaded/resolved do so now.
            if (NativeLibHandle.IsInvalid)
            {
                // Debug verify the requirements for resolution...
                // Although this value is NOT currently used, this detects issue for the future where it is needed.
                Debug.Assert(ResolverTarget != CodeGenTarget.Native, "Internal error: ResolverTarget should be set by now!");

                // Native binary is in a RID specific runtime folder, build that path as relative
                // to this assembly and load the library from there.
                string relativePath = @$"runtimes/{RuntimeInformation.RuntimeIdentifier}/native/{libraryName}-{GetNativeTarget()}-{ResolverTarget}";
                NativeLibHandle = NativeLibraryHandle.Load(relativePath, assembly, DllImportSearchPath.AssemblyDirectory);

                // setup the error handler callback.
                // Note: This will land in a recursive call to this method to resolve the library for this
                // P/Invoke call but NativeLibHandle is now set, so it is NOT an infinite recursion and
                // only happens once.
                unsafe
                {
                    LLVMInstallFatalErrorHandler( &FatalErrorHandler );
                }
            }

            return NativeLibHandle.DangerousGetHandle();
        }

        private static NativeLibraryHandle NativeLibHandle = new();
        private static CodeGenTarget ResolverTarget = CodeGenTarget.Native;
        private static bool ResolverApplied = false;

        private static CodeGenTarget GetNativeTarget()
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 or
                Architecture.X64 => CodeGenTarget.X86, // 64 vs 32 bit distinction is a CPU/feature of the target
                Architecture.Arm or
                Architecture.Armv6 => CodeGenTarget.ARM, // Distinction is a CPU/Feature of the target
                Architecture.Arm64 => CodeGenTarget.AArch64,
                Architecture.Wasm => CodeGenTarget.WebAssembly,
                Architecture.LoongArch64 => CodeGenTarget.LoongArch,
                Architecture.Ppc64le => CodeGenTarget.PowerPC,
                Architecture.RiscV64 => CodeGenTarget.RISCV, // 64 vs 32 bit distinction is a CPU/Feature of the target
                _ => throw new NotSupportedException("Native code gen target is unknown")
            };
        }
    }
}

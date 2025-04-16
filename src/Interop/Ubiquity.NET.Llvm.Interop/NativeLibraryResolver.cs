using System.ComponentModel;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Internal static 'utility' class to handle resolving the correct binary library to use</summary>
    /// <remarks>
    /// Ideally, this would be much simpler. However, the needs of an OSS project and Free (as in beer) build
    /// support collide. Even the paid builds would struggle to build all of the LLVM targets for a given runtime
    /// into a single library. Even assuming it can, the result is so large that it isn't usable with standard
    /// package services like NuGet.org. So, the solution is that the library is built supporting no more than
    /// two targets. The native target for the runtime is ALWAYS supported and, optionally, one additional target
    /// is included. Additionally, the target library is built into it's own NuGet package with a single "meta"
    /// package for the runtime that references each of the targets. This keeps the packages small and the downloads
    /// limited to the targeted runtime.
    /// </remarks>
    internal static class NativeLibraryResolver
    {
        private static NativeLibraryHandle NativeLibHandle = new();
        private static string? ResolverTarget = null;

        // !!NOTHING in this method may use P/Invoke to the native LLVM library (LibLLVM)!!
        //
        // This sets up the resolver AND the values it requires - interop calls may not
        // occur until that is complete. [They are guaranteed to fail (App Crash)!]
        internal static void Apply(LibLLVMCodeGenTarget target)
        {
            target.ThrowIfNotDefined();
            // NOTE: While it is allowed to register for all targets it is NOT allowed for loading as
            //       the additional target is part of the name of the library to load.
            if(target == LibLLVMCodeGenTarget.CodeGenTarget_All)
            {
                throw new ArgumentOutOfRangeException( nameof( target ), "Cannot load library for ALL available targets; ONLY one is allowed" );
            }

            // if target is requested for native, adjust it to the the explicit target
            // to allow resolving to the correct library (Native is always an option)
            if(target == LibLLVMCodeGenTarget.CodeGenTarget_Native)
            {
                target = GetNativeTarget();
            }

            // Only setup an import resolver once per process. Multiple attempts would result in an exception.
            // Once a library is loaded it cannot be unloaded and reloaded. The runtime will ONLY call the
            // resolver once per imported API. After it has the module and the address of the symbol it replaces
            // the P/Invoke in the same way that native code replaces the thunk for a dllimport. That is, once
            // the address of the symbol is known, no further resolution is used.
            string? oldTarget = Interlocked.CompareExchange(ref ResolverTarget, GetNameFor(target), null);
            if(!string.IsNullOrWhiteSpace(oldTarget))
            {
                throw new InvalidOperationException( "Cannot re-initialize to a different target" );
            }

            NativeLibrary.SetDllImportResolver( Assembly.GetExecutingAssembly(), NativeLibResolver );
        }

        // NOTE: This needs to be VERY fast as it is called for EVERY P/Invoke encountered.
        // BUT it is ONLY called once per import. After the import is resolved (LL+GPA) then
        // the resulting address is used internally without involving this callback. Thus the
        // library must remain valid until the app will never again call the imports. There is
        // no way to "invalidate" previous resolution as the final resolution to an address is
        // outside the control of this function.
        internal static nint NativeLibResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
        {
            // Any library other than the one known about here gets default handling
            // There must be an initialized Library instance or there's no point in
            // this custom resolver
            if(libraryName != LibraryName)
            {
                return nint.Zero;
            }

            // if not already loaded/resolved do so now.
            if(NativeLibHandle.IsInvalid)
            {
                // Debug verify the requirements for resolution...
                Debug.Assert( !string.IsNullOrWhiteSpace(ResolverTarget), "Internal error: ResolverTarget should be set by now!" );

                // Native binary is in a RID specific runtime folder, build that path as relative
                // to this assembly and load the library from there.
                string relativePath = @$"runtimes/{RuntimeInformation.RuntimeIdentifier}/native/{libraryName}-{ResolverTarget}";
                NativeLibHandle = NativeLibraryHandle.Load( relativePath, assembly, DllImportSearchPath.AssemblyDirectory );

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

        private static string GetNameFor( LibLLVMCodeGenTarget target )
        {
            return target switch
            {
                LibLLVMCodeGenTarget.CodeGenTarget_AArch64 => "AArch64",
                LibLLVMCodeGenTarget.CodeGenTarget_AMDGPU => "AMDGPU",
                LibLLVMCodeGenTarget.CodeGenTarget_ARM => "ARM",
                LibLLVMCodeGenTarget.CodeGenTarget_AVR => "AVR",
                LibLLVMCodeGenTarget.CodeGenTarget_BPF => "BPF",
                LibLLVMCodeGenTarget.CodeGenTarget_Hexagon => "Hexagon",
                LibLLVMCodeGenTarget.CodeGenTarget_Lanai => "Lana",
                LibLLVMCodeGenTarget.CodeGenTarget_LoongArch => "LoongArch",
                LibLLVMCodeGenTarget.CodeGenTarget_MIPS => "MIPS",
                LibLLVMCodeGenTarget.CodeGenTarget_MSP430 => "MSP430",
                LibLLVMCodeGenTarget.CodeGenTarget_NVPTX => "NVPTX",
                LibLLVMCodeGenTarget.CodeGenTarget_PowerPC => "PowerPC",
                LibLLVMCodeGenTarget.CodeGenTarget_RISCV => "RISCV",
                LibLLVMCodeGenTarget.CodeGenTarget_Sparc => "Sparc",
                LibLLVMCodeGenTarget.CodeGenTarget_SPIRV => "SPIRV",
                LibLLVMCodeGenTarget.CodeGenTarget_SystemZ => "SystemZ",
                LibLLVMCodeGenTarget.CodeGenTarget_VE => "VE",
                LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly => "WebAssembly",
                LibLLVMCodeGenTarget.CodeGenTarget_X86 => "X86",
                LibLLVMCodeGenTarget.CodeGenTarget_XCore => "XCore",
                // NOTE: None, Native and All, land in an exception as they are not viable as REAL targets.
                _ => throw new InvalidEnumArgumentException(nameof(target), (int)target, typeof(LibLLVMCodeGenTarget))
            };
        }

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler( byte* reason )
        {
            try
            {
                // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", ExecutionEncodingStringMarshaller.ConvertToManaged( reason ) );
            }
            catch(Exception ex)
            {
                // No finalizers will occur after this, it's a HARD termination of the app.
                Environment.FailFast( $"Unhandled exception in {nameof( FatalErrorHandler )}.", ex );
            }
        }

        private static LibLLVMCodeGenTarget GetNativeTarget( )
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 or
                Architecture.X64 => LibLLVMCodeGenTarget.CodeGenTarget_X86, // 64 vs 32 bit distinction is a CPU/feature of the target
                Architecture.Arm or
                Architecture.Armv6 => LibLLVMCodeGenTarget.CodeGenTarget_ARM, // Distinction is a CPU/Feature of the target
                Architecture.Arm64 => LibLLVMCodeGenTarget.CodeGenTarget_AArch64,
                Architecture.Wasm => LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly,
                Architecture.LoongArch64 => LibLLVMCodeGenTarget.CodeGenTarget_LoongArch,
                Architecture.Ppc64le => LibLLVMCodeGenTarget.CodeGenTarget_PowerPC,
                Architecture.RiscV64 => LibLLVMCodeGenTarget.CodeGenTarget_RISCV, // 64 vs 32 bit distinction is a CPU/Feature of the target
                _ => throw new NotSupportedException( "Native code gen target is unknown" )
            };
        }
    }
}

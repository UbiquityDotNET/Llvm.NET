﻿namespace Ubiquity.NET.Llvm.Interop
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
        private static bool ResolverApplied = false;

        // !!NOTHING in this method may use P/Invoke to the native LLVM library (LibLLVM)!!
        //
        // This sets up the resolver AND the values it requires - interop calls may not
        // occur until that is complete. [They are guaranteed to fail (App Crash)!]
        // Returns if the resolver was applied in this call or false if already applied
        internal static bool Apply()
        {
            // Only setup an import resolver once per process. Multiple attempts would result in an exception.
            // This translates such a case into a return of 'false'. Once a library is loaded it cannot be
            // unloaded and reloaded. The runtime will ONLY call the resolver once per imported API. After it
            // has the module and the address of the symbol it replaces the P/Invoke in the same way that
            // native code replaces the thunk for a dllimport. That is, once the address of the exported symbol
            // is known, no further resolution is used.
            if(Interlocked.CompareExchange(ref ResolverApplied, true, false))
            {
                return false;
            }

            NativeLibrary.SetDllImportResolver( Assembly.GetExecutingAssembly(), NativeLibResolver );
            return true;
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
                Debug.Assert( ResolverApplied, "Internal error: ResolverTarget should be set by now!" );

                // Native binary is in a RID specific runtime folder, build that path as relative
                // to this assembly and load the library from there.
                string relativePath = @$"runtimes/{RuntimeInformation.RuntimeIdentifier}/native/{libraryName}";
                NativeLibHandle = NativeLibraryHandle.Load( relativePath, assembly, DllImportSearchPath.AssemblyDirectory );
            }

            return NativeLibHandle.DangerousGetHandle();
        }
    }
}

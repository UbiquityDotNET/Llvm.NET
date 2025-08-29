// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Internal static 'utility' class to handle resolving the correct binary library to use</summary>
    internal static class NativeLibraryResolver
    {
        // !!NOTHING in this method may use P/Invoke to the native LLVM library (Ubiquity.NET.LibLLVM)!!
        //
        // This sets up the resolver AND the values it requires - interop calls may not
        // occur until that is complete. [They are guaranteed to fail (App Crash)!]
        // Returns if the resolver was applied in this call or false if already applied
        internal static bool Apply( )
        {
            // Only setup an import resolver once per process. Multiple attempts would result in an exception.
            // This translates such a case into a return of 'false'. Once a library is loaded it cannot be
            // unloaded and reloaded. The runtime will ONLY call the resolver once per imported API. After it
            // has the module and the address of the symbol it replaces the P/Invoke in the same way that
            // native code replaces the thunk for a dllimport. That is, once the address of the exported symbol
            // is known, no further resolution is used.
            if(Interlocked.CompareExchange( ref ResolverApplied, true, false ))
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
                string relativePath = Path.Combine("runtimes", RuntimeInformation.RuntimeIdentifier, "native", libraryName);
#pragma warning disable IDISP003 // Dispose previous before re-assigning
#pragma warning disable CS8601 // Possible null reference assignment.
                // Should NOT dispose previous as it already is tested for invalid (Known null here)
                // NOT a null reference as the result of the "try" pattern is tested.
                if(!NativeLibraryHandle.TryLoad( relativePath, assembly, DllImportSearchPath.AssemblyDirectory, out NativeLibHandle ))
                {
                    // Not found using the normal JIT location, BUT AOT will place it in the same
                    // location as the exe, so test for that..
                    if(!NativeLibraryHandle.TryLoad( libraryName, assembly, DllImportSearchPath.AssemblyDirectory, out NativeLibHandle ))
                    {
                        throw new InvalidOperationException( "Required LibLLVM native library does not exist!" );
                    }
                }
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore IDISP003 // Dispose previous before re-assigning
            }

            return NativeLibHandle.DangerousGetHandle();
        }

        private static NativeLibraryHandle NativeLibHandle = new();
        private static bool ResolverApplied = false;
    }
}

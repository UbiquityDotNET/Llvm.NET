// <copyright file="ComdatCollection.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET
{
    /// <summary>Collection of <see cref="Comdat"/> entries for a module</summary>
    /// <remarks>
    /// This type is used to provide enumeration and manipulation of <see cref="Comdat"/>s
    /// in the module.
    /// </remarks>
    public class ComdatCollection
        : IEnumerable<Comdat>
    {
        /// <summary>Retrieves <see cref="Comdat"/> by its name</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <returns><see cref="Comdat"/> or <see langword="null"/></returns>
        /// <exception cref="System.ArgumentNullException">Key is null</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Key does not exist in the collection</exception>
        public Comdat this[ string key ] => InternalComdatMap[ key ];

        /// <summary>Gets the number of items in the collection</summary>
        public int Count => InternalComdatMap.Count;

        /// <summary>Inserts or updates a <see cref="Comdat"/> entry</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <param name="kind"><see cref="ComdatKind"/> for the entry</param>
        /// <returns>New or updated <see cref="Comdat"/></returns>
        public Comdat InsertOrUpdate( string key, ComdatKind kind )
        {
            key.ValidateNotNullOrWhiteSpace( nameof( key ) );
            kind.ValidateDefined( nameof( kind ) );

            LLVMComdatRef comdatRef = LLVMModuleInsertOrUpdateComdat( Module.ModuleHandle, key, ( LLVMComdatSelectionKind )kind );
            if(!InternalComdatMap.TryGetValue( key, out Comdat retVal ))
            {
                retVal = new Comdat( Module, comdatRef );
                InternalComdatMap.Add( retVal.Name, retVal );
            }

            return retVal;
        }

        /// <summary>Removes all the <see cref="Comdat"/> entries from the module</summary>
        public void Clear( )
        {
            foreach( var obj in GetModuleGlobalObjects() )
            {
                obj.Comdat = null;
            }

            InternalComdatMap.Clear( );
            LLVMModuleComdatClear( Module.ModuleHandle );
        }

        /// <summary>Gets a value that indicates if a <see cref="Comdat"/> with a given name exists in the collection</summary>
        /// <param name="key">Name of the <see cref="Comdat"/> to test for</param>
        /// <returns><see langword="true"/> if the entry is present and <see langword="false"/> if not</returns>
        public bool Contains( string key ) => InternalComdatMap.ContainsKey( key );

        /// <inheritdoc/>
        public IEnumerator<Comdat> GetEnumerator( ) => InternalComdatMap.Values.GetEnumerator( );

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => InternalComdatMap.Values.GetEnumerator( );

        /// <summary>Removes a <see cref="Comdat"/> entry from the module</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <returns>
        /// <see langword="true"/> if the value was in the list or
        /// <see langword="false"/> otherwise
        /// </returns>
        public bool Remove( string key )
        {
            key.ValidateNotNullOrWhiteSpace( nameof( key ) );

            if(!InternalComdatMap.TryGetValue( key, out Comdat value ))
            {
                return false;
            }

            bool retVal = InternalComdatMap.Remove( key );
            if( retVal )
            {
                ClearComdatFromGlobals( key );
                LLVMModuleComdatRemove( Module.ModuleHandle, value.ComdatHandle );
            }

            return retVal;
        }

        /// <summary>Gets a value form the collection if it exists</summary>
        /// <param name="key">Name of the item to retrieve</param>
        /// <param name="value">Value of the item if found or <see langword="null"/> if not found</param>
        /// <returns>
        /// <see langword="true"/> if the value was found
        /// the list or <see langword="false"/> otherwise.
        /// </returns>
        public bool TryGetValue( string key, out Comdat value )
        {
            key.ValidateNotNullOrWhiteSpace( nameof( key ) );
            return InternalComdatMap.TryGetValue( key, out value );
        }

        /// <summary>Initializes a new instance of the <see cref="ComdatCollection"/> class for a module</summary>
        /// <param name="module">Module the comdats are enumerated from</param>
        internal ComdatCollection( BitcodeModule module )
        {
            module.ValidateNotNull( nameof( module ) );

            Module = module;
            LLVMModuleEnumerateComdats( Module.ModuleHandle, AddComdat );
        }

        private IEnumerable<GlobalObject> GetModuleGlobalObjects()
        {
            foreach( var gv in Module.Globals.OfType<GlobalObject>( ) )
            {
                yield return gv;
            }

            foreach( var func in Module.Functions )
            {
                yield return func;
            }
        }

        private void ClearComdatFromGlobals( string name )
        {
            var matchingGlobals = from gv in GetModuleGlobalObjects()
                                  where gv.Comdat.Name == name
                                  select gv;

            foreach( var gv in matchingGlobals )
            {
                gv.Comdat = null;
            }
        }

        private bool AddComdat( LLVMComdatRef comdatRef )
        {
            comdatRef.ValidateNotDefault( nameof( comdatRef ) );
            var comdat = new Comdat( Module, comdatRef );
            InternalComdatMap.Add( comdat.Name, comdat );
            return true;
        }

        private BitcodeModule Module;
        private Dictionary<string, Comdat> InternalComdatMap = new Dictionary<string, Comdat>( );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMComdatSelectionKind kind );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMModuleComdatClear( LLVMModuleRef module );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private delegate bool ComdatIteratorCallback( LLVMComdatRef comdatRef );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMModuleEnumerateComdats( LLVMModuleRef module, ComdatIteratorCallback callback );
    }
}

// -----------------------------------------------------------------------
// <copyright file="ComdatCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Collection of Comdats in a module</summary>
    /// <remarks>
    /// This is a 'ref struct' to ensure that ownership of the collection
    /// remains with the module itself and this is only used to iterate
    /// over the individual items. It is not allowed to store this as it
    /// references the owning module and ultimately the Comdat values themselves
    /// are owned in the native code layer and only "projected" to managed
    /// here.
    /// </remarks>
    public readonly ref struct ComdatCollection
        : IEnumerable<Comdat>
    {
        /// <summary>Gets a count of comdats in the associated module</summary>
        public int Count => checked((int)LibLLVMModuleGetNumComdats(OwningModule.GetUnownedHandle()));

        /// <summary>Retrieves <see cref="Comdat"/> by its name</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <returns><see cref="Comdat"/> or <see langword="null"/></returns>
        /// <exception cref="System.ArgumentException">Key is null</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Key does not exist in the collection</exception>
        public Comdat this[ string key ]
        {
            get
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(key);

                return TryGetValue(key, out Comdat retVal) ? retVal : throw new KeyNotFoundException(key);
            }
        }

        /// <summary>Adds or updates a <see cref="Comdat"/> entry</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <param name="kind"><see cref="ComdatKind"/> for the entry</param>
        /// <returns>New or updated <see cref="Comdat"/></returns>
        public Comdat AddOrUpdate( string key, ComdatKind kind )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( key );
            kind.ThrowIfNotDefined();

            return new(LibLLVMModuleInsertOrUpdateComdat( OwningModule.GetUnownedHandle(), key, ( LLVMComdatSelectionKind )kind ));
        }

        /// <summary>Gets a value form the collection if it exists</summary>
        /// <param name="key">Name of the item to retrieve</param>
        /// <param name="value">Value of the item if found or <see langword="null"/> if not found</param>
        /// <returns>
        /// <see langword="true"/> if the value was found
        /// the list or <see langword="false"/> otherwise.
        /// </returns>
        public bool TryGetValue( string key, out Comdat value)
        {
            value = new(LibLLVMModuleGetComdat(OwningModule.GetUnownedHandle(), key));
            return !value.Handle.IsNull;
        }

        /// <summary>Gets a value that indicates if a <see cref="Comdat"/> with a given name exists in the collection</summary>
        /// <param name="key">Name of the <see cref="Comdat"/> to test for</param>
        /// <returns><see langword="true"/> if the entry is present and <see langword="false"/> if not</returns>
        public bool Contains( string key ) => TryGetValue( key, out Comdat _);

        /// <summary>Removes a <see cref="Comdat"/> entry from the module</summary>
        /// <param name="key">Name of the <see cref="Comdat"/></param>
        /// <returns>
        /// <see langword="true"/> if the value was in the list or
        /// <see langword="false"/> otherwise
        /// </returns>
        public bool Remove( string key )
        {
            if(!TryGetValue(key, out Comdat value))
            {
                return false;
            }

            WithGlobalObjects(go=>
            {
                if (!go.Comdat.IsNull && go.Comdat.Name == key)
                {
                    go.Comdat = default;
                }
            });

            LibLLVMModuleComdatRemove( OwningModule.GetUnownedHandle(), value.Handle );
            return true;
        }

        /// <summary>Removes all the <see cref="Comdat"/> entries from the module</summary>
        public void Clear( )
        {
            WithGlobalObjects(go => go.Comdat = default);
            LibLLVMModuleComdatClear( OwningModule.GetUnownedHandle() );
        }

        /// <inheritdoc/>
        IEnumerator<Comdat> IEnumerable<Comdat>.GetEnumerator() => new Enumerator(OwningModule);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(OwningModule);

        /// <summary>Enumerator for comdats</summary>
        internal sealed class Enumerator
            : IEnumerator<Comdat>
        {
            /// <inheritdoc/>
            public Comdat Current => new(LibLLVMCurrentComdat(Handle));

            /// <inheritdoc/>
            /// <remarks>Throws a <see cref="NotSupportedException"/> as boxing the ref struct for return is NOT allowed</remarks>
            /// <exception cref="NotSupportedException">Unconditionally</exception>
            object IEnumerator.Current
            {
                [DoesNotReturn]
                get => throw new NotSupportedException();
            }

            /// <inheritdoc/>
            public void Dispose() => Handle.Dispose();

            /// <inheritdoc/>
            public bool MoveNext() => LibLLVMNextComdat(Handle);

            /// <inheritdoc/>
            public void Reset() => LibLLVMModuleComdatIteratorReset(Handle);

            internal Enumerator(IModule module)
            {
                Handle = LibLLVMModuleBeginComdats(module.GetUnownedHandle());
            }

            private readonly LibLLVMComdatIteratorRef Handle;
        }

        internal ComdatCollection(IModule owningModule)
        {
            OwningModule = owningModule;
        }

        // iterates over a set of global objects since creating an enumerable with 'yield return' is not
        // an option for a ref struct
        private void WithGlobalObjects( Action<GlobalObject> op )
        {
            foreach( var gv in OwningModule.Globals )
            {
                op(gv);
            }

            foreach( var func in OwningModule.Functions )
            {
                op(func);
            }
        }

        // This is an interface reference (alias) as this instance doesn't own the module and does not need disposal
        private readonly IModule OwningModule;
    }
}

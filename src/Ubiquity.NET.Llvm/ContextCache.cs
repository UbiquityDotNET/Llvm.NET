// -----------------------------------------------------------------------
// <copyright file="ContextCache.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Maintains a global cache of <see cref="LLVMContextRef"/> to <see cref="Context"/> mappings</summary>
    internal static class ContextCache
    {
        internal static bool TryGetValue( LLVMContextRef h, out Context value )
        {
            return Instance.Value.TryGetValue( h, out value );
        }

        internal static bool Remove( LLVMContextRef h )
        {
            return Instance.Value.Remove( h );
        }

        internal static Context Add( Context context )
        {
            Instance.Value.Add( context.ContextHandle, context );
            return context;
        }

        internal static Context GetContextFor( LLVMContextRef contextRef )
        {
            contextRef.ValidateNotNull( nameof( contextRef ) );

            if( TryGetValue( contextRef, out Context retVal ) )
            {
                return retVal;
            }

            // Context constructor will add itself to this cache
            // and remove itself on Dispose/finalize
            return new Context( contextRef );
        }

        private static IDictionary<LLVMContextRef, Context> CreateInstance( )
        {
            return new ConcurrentDictionary<LLVMContextRef, Context>( EqualityComparer<LLVMContextRef>.Default );
        }

        private static readonly Lazy<IDictionary<LLVMContextRef, Context>> Instance
            = new Lazy<IDictionary<LLVMContextRef, Context>>( CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
    }
}

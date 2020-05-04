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
            return Instance.Value.TryRemove( h, out Context _ );
        }

        internal static Context Add( Context context )
        {
            return Instance.Value.GetOrAdd( context.ContextHandle, context );
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
            // TODO: resolve thread safety bug (https://github.com/UbiquityDotNET/Llvm.NET/issues/179)
            return new Context( contextRef );
        }

        private static ConcurrentDictionary<LLVMContextRef, Context> CreateInstance( )
        {
            return new ConcurrentDictionary<LLVMContextRef, Context>( EqualityComparer<LLVMContextRef>.Default );
        }

        private static readonly Lazy<ConcurrentDictionary<LLVMContextRef, Context>> Instance
            = new Lazy<ConcurrentDictionary<LLVMContextRef, Context>>( CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
    }
}

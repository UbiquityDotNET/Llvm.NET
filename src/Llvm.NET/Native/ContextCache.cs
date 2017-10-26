// <copyright file="ContextCache.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Llvm.NET.Native
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

        private static IDictionary<LLVMContextRef, Context> CreateInstance()
        {
            return new ConcurrentDictionary<LLVMContextRef, Context>( );
        }

        private static Lazy<IDictionary<LLVMContextRef, Context>> Instance
            = new Lazy<IDictionary<LLVMContextRef, Context>>( CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);
    }
}

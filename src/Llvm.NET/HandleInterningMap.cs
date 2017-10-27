// <copyright file="HandleInterningMap.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;
using Llvm.NET.Native;

#pragma warning disable SA1649

namespace Llvm.NET
{
    internal interface IHandleInterning<THandle, TMappedType>
        where THandle : ILlvmHandle
    {
        TMappedType GetItemFor( THandle handle, Context context );

        void Clear( );
    }

    internal class HandleInterningMap<THandle, TMappedType>
        : IHandleInterning<THandle, TMappedType>
        where THandle : ILlvmHandle
    {
        public HandleInterningMap( Func<THandle, Context, TMappedType> itemFactory, [CanBeNull] Action<TMappedType> disposer = null )
        {
            ItemFactory = itemFactory;
            ItemDisposer = disposer;
        }

        public TMappedType GetItemFor( THandle handle, Context context )
        {
            if( handle.Handle.IsNull() )
            {
                return default;
            }

            if( HandleMap.TryGetValue( handle, out TMappedType retVal ) )
            {
                return retVal;
            }

            retVal = ItemFactory( handle, context );
            HandleMap.Add( handle, retVal );
            return retVal;
        }

        public void Clear( )
        {
            if( ItemDisposer != null )
            {
                foreach( var value in HandleMap.Values )
                {
                    ItemDisposer( value );
                }
            }

            HandleMap.Clear( );
        }

        private Func<THandle, Context, TMappedType> ItemFactory;
        private Action<TMappedType> ItemDisposer;
        private IDictionary<THandle, TMappedType> HandleMap = new ConcurrentDictionary<THandle, TMappedType>();
    }
}

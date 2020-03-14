// <copyright file="DisposableAction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Disposable type that runs a specified action on dispose</summary>
    /// <remarks>
    /// This is used in RAII pattern style code instead of try/finally.
    /// It is most valuable when the scope extends beyond a single function
    /// where a try/finally simply won't work.
    /// </remarks>
    public struct DisposableAction
        : IDisposable
        , IEquatable<DisposableAction>
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableAction"/> struct.</summary>
        /// <param name="onDispose">Action to run when <see cref="Dispose"/>is called.</param>
        public DisposableAction( Action onDispose )
        {
            OnDispose = onDispose ?? throw new ArgumentNullException( nameof( onDispose ) );
            HashCode = onDispose.GetHashCode( );
        }

        /// <summary>Runs the action provided in the constructor (<see cref="Ubiquity.NET.Llvm.Interop.DisposableAction(System.Action)" /></summary>
        public void Dispose( )
        {
            Interlocked.Exchange( ref OnDispose, null )?.Invoke( );
        }

        /// <summary>Gets a Default disposable action that does nothing</summary>
        public static DisposableAction Nop => new DisposableAction( ( ) => { } );

        /// <inheritdoc/>
        public override bool Equals( object obj )
        {
            return OnDispose?.Equals( obj ) ?? false;
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return HashCode;
        }

        /// <inheritdoc/>
        public bool Equals( DisposableAction other )
        {
            return OnDispose?.Equals( other.OnDispose ) ?? false;
        }

        /// <summary>Compares two actions for equality</summary>
        /// <param name="left">Left action to compare</param>
        /// <param name="right">Right action to compare</param>
        /// <returns><see langword="true"/> if the delegates the actions wrap are equal <see langword="false"/> if not</returns>
        public static bool operator ==( DisposableAction left, DisposableAction right ) => left.Equals( right );

        /// <summary>Compares two actions for inequality</summary>
        /// <param name="left">Left action to compare</param>
        /// <param name="right">Right action to compare</param>
        /// <returns><see langword="true"/> if the delegates the actions wrap are not equal <see langword="false"/> if not</returns>
        public static bool operator !=( DisposableAction left, DisposableAction right ) => !( left == right );

        private Action? OnDispose;
        private readonly int HashCode;
    }
}

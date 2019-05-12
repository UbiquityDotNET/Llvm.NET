// -----------------------------------------------------------------------
// <copyright file="DisposableAction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Kaleidoscope.Grammar
{
    /// <summary>Disposable type that runs a specified action on dispose</summary>
    /// <remarks>
    /// This is used in a C++ RAII pattern style code instead of try/finally.
    /// It is most valuable when the scope extends beyond a single function
    /// where a try/finally simply won't work.
    /// </remarks>
    internal struct DisposableAction
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableAction"/> struct.</summary>
        /// <param name="onDispose">Action to run when <see cref="Dispose"/>is called.</param>
        public DisposableAction( Action onDispose )
        {
            OnDispose = onDispose ?? throw new ArgumentNullException( nameof( onDispose ) );
        }

        /// <summary>Runs the action provided in the constructor (<see cref="DisposableAction(Action)"/></summary>
        public void Dispose( )
        {
            OnDispose( );
        }

        /// <summary>Gets a Default disposable action that does nothing</summary>
        public static DisposableAction Nop => new DisposableAction( ()=> { } );

        private readonly Action OnDispose;
    }
}

// <copyright file="DisposableAction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Disposable type that runs a specified action on dispose</summary>
    /// <remarks>
    /// This is used in RAII pattern style code instead of try/finally.
    /// It is most valuable when the scope extends beyond a single function
    /// where a try/finally simply won't work.
    /// </remarks>
    public readonly record struct DisposableAction
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableAction"/> class.</summary>
        /// <param name="onDispose">Action to run when <see cref="Dispose"/>is called.</param>
        public DisposableAction(Action onDispose)
        {
            OnDispose = onDispose ?? throw new ArgumentNullException( nameof( onDispose ) );
        }

        /// <summary>Runs the action provided in the constructor (<see cref="Ubiquity.NET.Llvm.Interop.DisposableAction(System.Action)" /></summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>Gets a Default disposable action that does nothing</summary>
        public static DisposableAction Nop => new( () => { } );

        private readonly Action OnDispose;
    }
}

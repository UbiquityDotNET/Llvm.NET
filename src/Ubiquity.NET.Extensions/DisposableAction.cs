// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Disposable type that runs a specified action on dispose</summary>
    /// <remarks>
    /// This is used in RAII pattern style code instead of try/finally. It is most
    /// valuable when the scope extends beyond a single function (as a return of
    /// <see cref="IDisposable"/>) where a try/finally simply won't work.
    /// </remarks>
    public readonly record struct DisposableAction
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableAction"/> struct.</summary>
        /// <param name="onDispose">Action to run when <see cref="Dispose"/>is called.</param>
        public DisposableAction( Action onDispose )
        {
            OnDispose = onDispose ?? throw new ArgumentNullException( nameof( onDispose ) );
        }

        /// <summary>Runs the action provided in the constructor (<see cref="DisposableAction(System.Action)" /></summary>
        public void Dispose( )
        {
            OnDispose();
        }

        private readonly Action OnDispose;
    }
}

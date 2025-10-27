// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Disposable type that runs a specified action on dispose</summary>
    /// <remarks>
    /// This is used in RAII pattern style code instead of try/finally. It is most
    /// valuable when the scope extends beyond a single function (as a return of
    /// <see cref="IDisposable"/>) where a try/finally simply won't work.
    /// </remarks>
    public sealed class DisposableAction
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableAction"/> class.</summary>
        /// <param name="onDispose">Action to run when <see cref="Dispose"/> is called.</param>
        /// <param name="exp">Expression for any exceptions; default normally provided by compiler as expression for <paramref name="onDispose"/></param>
        public DisposableAction( Action onDispose, [CallerArgumentExpression(nameof(onDispose))] string? exp = null )
        {
            OnDispose = onDispose ?? throw new ArgumentNullException( exp );
        }

        /// <summary>Runs the action provided in the constructor (<see cref="DisposableAction(Action, string?)"/>)</summary>
        /// <exception cref="ObjectDisposedException">This instance is already disposed</exception>
        public void Dispose( )
        {
            var disposeOp = Interlocked.Exchange(ref OnDispose, null);
            ObjectDisposedException.ThrowIf(disposeOp is null, this);
            disposeOp!();
        }

        /// <summary>Creates an implementation of <see cref="IDisposable"/> that does nothing for the "Null Object" pattern</summary>
        /// <returns>The <see cref="IDisposable"/> that does nothing on <see cref="IDisposable.Dispose"/></returns>
        /// <remarks>
        /// The instance returned is allocated from the managed heap to ensure that <see cref="IDisposable.Dispose"/> is
        /// called only once (any additional call results in an <see cref="ObjectDisposedException"/>).
        /// </remarks>
        public static IDisposable CreateNOP()
        {
            return new DisposableAction(()=> { });
        }

        private Action? OnDispose;
    }
}

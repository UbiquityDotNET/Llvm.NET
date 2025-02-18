using System;
using System.Threading;

namespace SourceGenerator.Utils
{
    // internal type to provide RAII like operations via IDisposable and 'using'
    internal record DisposableAction
        : IDisposable
    {
        public DisposableAction(Action disposeAction)
        {
            DisposeAction = disposeAction;
        }

        public void Dispose()
        {
            var action = Interlocked.Exchange(ref DisposeAction, null);
            if (action is not null)
            {
                action();
            }
        }

        private Action? DisposeAction = null;
    }
}

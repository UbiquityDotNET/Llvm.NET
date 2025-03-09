// -----------------------------------------------------------------------
// <copyright file="HandleInterningMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    internal abstract class HandleInterningMapWithContext<THandle, TMappedType>
        : HandleInterningMap<THandle, TMappedType>
        where THandle : notnull, IEquatable<THandle>
    {
        /// <summary>Gets the context for the handles in this map</summary>
        public Context Context { get; }

        /// <summary>Initializes a new instance of the <see cref="HandleInterningMapWithContext{THandle, TMappedType}"/> class.</summary>
        /// <param name="context">Context this map is bound to</param>
        private protected HandleInterningMapWithContext( Context context )
        {
            Context = context;
        }
    }
}

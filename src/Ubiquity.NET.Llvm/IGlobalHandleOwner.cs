// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm
{
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Closely related interfaces" )]
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal Interface" )]
    internal interface IHandleWrapper<THandle>
    {
        /// <summary>Gets the handle for this wrapper</summary>
        /// <returns>ABI handle value for this implementation</returns>
        /// <remarks>
        /// For a handle that includes an alias this should be the alias handle type
        /// for global handles that don't support an alias, or contextual handles
        /// this is just the handle type.
        /// </remarks>
        internal THandle Handle { get; }
    }

    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal Interface" )]
    internal interface IGlobalHandleOwner<THandle>
        : IDisposable
        where THandle : GlobalHandleBase
    {
        /// <summary>Gets the handle owned by this instance</summary>
        /// <remarks>
        /// This is normally used when getting the handle as part of a "move" for an in parameter.
        /// Ordinarily this is used **BEFORE** <see cref="InvalidateFromMove"/> to get the handle
        /// that is moved and only mark it as such on success.
        /// </remarks>
        internal THandle OwnedHandle { get; }

        /// <summary>Used to provide 'Move' semantics for the ABI handle as an 'in' parameter ***AFTER** transferring ownership</summary>
        /// <remarks>
        /// This is used AFTER calling <see cref="IHandleWrapper{THandle}.Handle"/> AND calling the native API that takes
        /// ownership. This sequence/ordering is important to ensure that ownership remains in the managed code if an exception
        /// occurs in calling the native API. Only after that returns successfully is this called to invalidate the managed
        /// handle in a manner that makes disposal a NOP. (Callers should NOT care about this and still call Dispose()
        /// in case there was an exception and they still own it!)
        /// </remarks>
        internal void InvalidateFromMove( );
    }
}

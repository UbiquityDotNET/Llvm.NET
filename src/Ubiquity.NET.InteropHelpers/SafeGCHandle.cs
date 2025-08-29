// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Helper type to wrap GC handles with ref counting</summary>
    /// <remarks>
    /// <para>Instances of this class (as a private member of a controlled type) help
    /// to ensure the referenced object stays alive while in use for native ABI as a
    /// "context" for callbacks. This is the key to ref counted behavior to hold 'o'
    /// (and anything it references) alive for the GC. The "ownership" of the refcount
    /// is handed to native code while the calling code is free to no longer reference
    /// the containing instance as it holds an allocated GCHandle for itself and THAT
    /// is kept alive by a ref count that is "owned" by native code.</para>
    ///
    /// <para>This is used as a member of such a holding type so that 'AddRefAndGetNativeContext'
    /// retrieves a marshaled GCHandle for the containing/Controlling instance that is
    /// then provided as the native "Context" parameter.</para>
    ///
    /// <para>It is assumed that instances of this type are held in a disposable type such
    /// that when the containing type is disposed, then this is disposed. Additionally,
    /// this assumes that the native code MIGHT dispose of this instance and that callers
    /// should otherwise account for the ref count increase to hold the instance alive. That
    /// is, by holding a <see cref="GCHandle"/> to self, with an AddRef'd handle the instance
    /// would live until the app is terminated! Thus applications using this MUST understand
    /// the native code use and account for the disposable of any instances with this as a
    /// member. Typically a callback provided to the native code is used to indicate release
    /// of the resource. That callback will call dispose to decrement the refcount on the
    /// handle. If the ref count lands at 0, then the object it refers to is subject to
    /// normal GC.</para>
    /// </remarks>
    public sealed class SafeGCHandle
        : SafeHandle
    {
        /// <summary>Initializes a new instance of the <see cref="SafeGCHandle"/> class.</summary>
        /// <param name="o">Object to allocate a GCHandle for that is controlled by this instance</param>
        /// <remarks>
        /// It is expected that the type of <paramref name="o"/> has this <see cref="SafeGCHandle"/>
        /// as a private member so that it controls the lifetime of it's container.
        /// </remarks>
        public SafeGCHandle( object o )
            : base( 0, ownsHandle: true )
        {
            handle = (nint)GCHandle.Alloc( o );
        }

        /// <inheritdoc/>
        public override bool IsInvalid => handle == 0;

        /// <summary>Adds a ref count to this handle AND converts the allocated <see cref="GCHandle"/> for use in native code</summary>
        /// <returns>context value for us in native code to refer to the <see cref="GCHandle"/> held by this instance</returns>
        /// <remarks>
        /// A native call back that receives the returned context can reconstitute the <see cref="GCHandle"/> via <see cref="GCHandle.FromIntPtr(nint)"/>
        /// and from that it can get the original instance the handle refers to via <see cref="GCHandle.Target"/>
        /// </remarks>
        public unsafe nint AddRefAndGetNativeContext( )
        {
            bool ignoredButRequired = false;
            DangerousAddRef( ref ignoredButRequired );
            return handle;
        }

        /// <inheritdoc/>
        protected override bool ReleaseHandle( )
        {
            GCHandle.FromIntPtr( handle ).Free();
            return true;
        }
    }
}

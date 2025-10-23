// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>ORC JIT v2 Object linking layer</summary>
    /// <remarks>
    /// Since instances of an Object Linking layer are ONLY ever created by <see cref="ObjectLayerFactory"/> and
    /// returned directly to the native code as the raw handle, they are not generally disposable. They do
    /// implement IDisposable as a means to enusre proper release in the face of an exception in an implementation
    /// of <see cref="ObjectLayerFactory"/> but are not something that generally needs disposal at a managed level.
    /// </remarks>
    public class ObjectLayer
        : DisposableObject
    {
        /// <summary>Adds an object file to the specified library</summary>
        /// <param name="jitDyLib">Library to add the object file to</param>
        /// <param name="objBuffer">Buffer for the object file</param>
        /// <remarks>
        /// The <paramref name="objBuffer"/> is transferred to the native implementation
        /// of the library and is no longer usable after this (Dispose is still a NOP,
        /// but any other operation results in an <see cref="ObjectDisposedException"/>)
        /// </remarks>
        public void Add( JITDyLib jitDyLib, MemoryBuffer objBuffer )
        {
            using LLVMErrorRef errorRef = LLVMOrcObjectLayerAddObjectFile(Handle, jitDyLib.Handle, objBuffer.Handle);

            // Even if an error occurred, ownership is transferred
            objBuffer.InvalidateAfterMove();
            errorRef.ThrowIfFailed();
        }

        /// <summary>Adds an object file to the specified <see cref="ResourceTracker"/></summary>
        /// <param name="rt">Tracker to add the object file to</param>
        /// <param name="objBuffer">Buffer for the object file</param>
        /// <remarks>
        /// The <paramref name="objBuffer"/> is transferred to the native implementation
        /// of the library and is no longer usable after this (Dispose is still a NOP,
        /// but any other operation results in an <see cref="ObjectDisposedException"/>)
        /// </remarks>
        public void Add( ResourceTracker rt, MemoryBuffer objBuffer )
        {
            using LLVMErrorRef errorRef = LLVMOrcObjectLayerAddObjectFileWithRT(Handle, rt.Handle, objBuffer.Handle);

            // Even if an error occurred, ownership is transferred
            objBuffer.InvalidateAfterMove();
            errorRef.ThrowIfFailed();
        }

        /// <summary>Emits an object buffer to the object layer</summary>
        /// <param name="resp">Materialization Responsibility</param>
        /// <param name="objBuffer">Object buffer to emit</param>
        /// <remarks>
        /// Ownership of both parameters is transferred to the native code.
        /// Calling Dispose() on either after this is called is a NOP, other
        /// operations result in an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Emit( MaterializationResponsibility resp, MemoryBuffer objBuffer )
        {
            LLVMOrcObjectLayerEmit( Handle, resp.Handle, objBuffer.Handle );
            resp.InvalidateAfterMove();
            objBuffer.InvalidateAfterMove();
        }

        /// <inheritdoc/>
        [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Ownership transferred in constructor" )]
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
            if(!Handle.IsNull)
            {
                Handle.Dispose();
                Handle = default;
            }
        }

        internal ObjectLayer( LLVMOrcObjectLayerRef h )
        {
            Handle = h;
        }

        internal ObjectLayer()
        {
        }

        internal LLVMOrcObjectLayerRef Handle
        {
            get;

            // Only accessible from derived types, since the modifier of this property is `internal` that
            // means `internal` AND `protected`
            private protected set
            {
                if(!field.IsNull)
                {
                    throw new InvalidOperationException("INTERNAL: Setting handle multiple times is not allowed!");
                }

                field = value;
            }
        }
    }
}

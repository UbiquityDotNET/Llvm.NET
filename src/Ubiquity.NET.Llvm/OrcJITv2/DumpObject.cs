// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Diagnostic class to allow dumping of object files for off-line review</summary>
    public sealed class DumpObject
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="DumpObject"/> class</summary>
        /// <param name="dir">Directory for the object file</param>
        /// <param name="identifierOverride">Identifier override</param>
        public DumpObject( LazyEncodedString dir, LazyEncodedString? identifierOverride )
        {
            Handle = LLVMOrcCreateDumpObjects( dir, identifierOverride );
        }

        /// <summary>Dumps a single memory buffer as an object file</summary>
        /// <param name="objBuffer">Buffer for the object file to dump</param>
        /// <remarks>
        /// This will write the object file using the parameters provided in construction
        /// of this instance.
        /// <note type="important">
        /// It is important to note that the provided buffer is destroyed on exceptions
        /// (errors from LLVM interop). Under normal conditions the buffer ownership remains
        /// with the caller. Either way the caller can (and should) call Dispose() [usually,
        /// via a using statement of some sort] However, other methods are NOT guaranteed to
        /// function properly unless there is no exception.
        /// </note>
        /// </remarks>
        public void Dump( MemoryBuffer objBuffer )
        {
            // This API uses an odd (for LLVM at least) signature in that
            // the ref parameter is a pointer to a handle, that is then
            // used in a call accepting an std::unique_ptr<> (via std::move(*pHandle))
            // meaning that the object buffer is, in fact moved to the dump
            // routine and it is NOT just a const ref disguised in a C wrapper.
            //
            // But, wait, there's more...
            //
            // The call operator RETURNS the unique_ptr holding the object
            // buffer pointer via another std::move(), but ONLY on success.
            // Thus, "moving" the ownership responsibility back to the caller.
            // The C wrapper then re-assigns the objBuffer to the de-referenced
            // pointer parameter UNLESS the dump produced an error. If it did,
            // the buffer pointer is set to null and an error is returned...
            //
            // Upshot is that the buffer is destroyed by native code IFF
            // the [Out] leg of the ref handle is null

            // MemoryBuffer.Handle is a property, not a field so it is not
            // usable with the "ref" keyword, Thus a local is used instead.
            // Native code will set this to null (marshalling creates an
            // invalid handle) if it is destroyed.
            var bufHandle = objBuffer.Handle;
            using LLVMErrorRef errRef = LLVMOrcDumpObjects_CallOperator(Handle, ref bufHandle);
            if(bufHandle.IsNull)
            {
                // Buffer was released internally by native call
                objBuffer.InvalidateAfterMove();
            }

            errRef.ThrowIfFailed();
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        internal LLVMOrcDumpObjectsRef Handle { get; }
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// They are order by access, unfortunately this analyzer has stupid built-in defaults that
// puts internal as higher priority than protected and no way to override it.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Interface for an LLVM OrcV2 JIT</summary>
    /// <remarks>
    /// ORC JIT instances are created through a builder, which makes derived types
    /// impossible. Instead composition is used with this interface as the means of
    /// generalizing the distinction. Custom runtime JITs will implement this interface
    /// and generally forward calls to an internal <see cref="LlJIT"/> instance it created
    /// via a builder.
    /// </remarks>
    public interface IOrcJit
        : IDisposable
    {
        /// <summary>Gets the main library for this JIT instance</summary>
        JITDyLib MainLib { get; }

        /// <summary>Gets the data layout string for this JIT</summary>
        LazyEncodedString DataLayoutString { get; }

        /*
        TODO: Add LibLLVMxxx to make this go away, the underlying JIT HAS a Triple instance!
              So this is building a string from that, then passed around as a string marshaled to/from
              the native abi and then re-parsed back to a !@#$ Triple again - WASTED overhead!
        */

        /// <summary>Gets a string representation of the target triple for this JIT</summary>
        LazyEncodedString TripleString { get; }

        /// <summary>Gets the IR transform layer for this JIT</summary>
        IrTransformLayer TransformLayer { get; }

        /// <summary>Gets the Execution session for this JIT</summary>
        ExecutionSession Session { get; }

        /// <summary>Adds a module to this JIT with removal tracking</summary>
        /// <param name="ctx">Thread safe context this module is part of</param>
        /// <param name="module">Module to add</param>
        /// <param name="lib">Library to work on</param>
        /// <returns>Resource tracker for this instance</returns>
        [MustUseReturnValue]
        ResourceTracker AddWithTracking( ThreadSafeContext ctx, Module module, JITDyLib lib = default );

        /// <summary>Looks up the native address of a symbol</summary>
        /// <param name="name">NameField of the symbol to find the address of</param>
        /// <returns>Address of the symbol</returns>
        /// <exception cref="LlvmException">Error occurred with lookup [Most likely the symbol is not found]</exception>
        UInt64 Lookup( LazyEncodedString name );

        /// <summary>Adds a module to the JIT</summary>
        /// <param name="lib">Library to add the module to in this JIT</param>
        /// <param name="module">Module to add</param>
        /// <remarks>
        /// This function has "move" semantics in that the JIT takes ownership of the
        /// input module and it is no longer usable (Generates <see cref="ObjectDisposedException"/>)
        /// for any use other than Dispose(). This allows normal clean up in the event of an exception
        /// to occur.
        /// <note type="important">
        /// Transfer of ownership does NOT occur in the face of an error (exception)! However the
        /// <see cref="ThreadSafeModule.Dispose"/> method is idempotent and will NOT throw an exception
        /// if disposed so it is safe to declare instances with a "using".
        /// </note>
        /// </remarks>
        void Add( JITDyLib lib, ThreadSafeModule module );

        /// <summary>Adds a module to the JIT</summary>
        /// <param name="tracker">Resource tracker to manage the module</param>
        /// <param name="module">Module to add</param>
        /// <remarks>
        /// This function has "move" semantics in that the JIT takes ownership of the
        /// input module and it is no longer usable (Generates <see cref="ObjectDisposedException"/>)
        /// for any use other than Dispose(). This allows normal clean up in the event of an exception
        /// to occur.
        /// <note type="important">
        /// Transfer of ownership does NOT occur in the face of an error (exception)! However the
        /// <see cref="ThreadSafeModule.Dispose"/> method is idempotent and will NOT throw an exception
        /// if disposed so it is safe to declare instances with a "using".
        /// </note>
        /// </remarks>
        void Add( ResourceTracker tracker, ThreadSafeModule module );

        /// <summary>Mangles and interns a symbol in the JIT's symbol pool</summary>
        /// <param name="name">Symbol name to add</param>
        /// <returns>Entry to the string pool for the symbol</returns>
        SymbolStringPoolEntry MangleAndIntern( LazyEncodedString name );
    }
}

// -----------------------------------------------------------------------
// <copyright file="LLJitBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Interface for a Factory of ORC Object Layers</summary>
    public interface IObjectLinkingLayerFactory
    {
        /// <summary>Factory Function for an ORC Object layer</summary>
        /// <param name="session">ORC Execution session for creation</param>
        /// <param name="triple">Target Triple string for creation</param>
        /// <returns>Object Layer</returns>
        /// <remarks>
        /// <note type="important">
        /// It is important to note that the returned <see cref="ObjectLayer"/>
        /// is OWNED by the LLVM ORC v2 system and must not be used after this.
        /// The underlying handle is invalidated making the <see cref="ObjectLayer.Dispose"/>
        /// a NOP. This allows code to remain ignorant of the transfer with a
        /// `using` statement or other language equivalent for a disposable type.
        /// Most API use on the layer will report an <see cref="ObjectDisposedException"/>
        /// as the underlying instance is already transferred to LLVM.
        /// </note>
        /// </remarks>
        /// <ImplementationNote>
        /// The type of argument for triple may change to an unsafe pointer as it
        /// may be converted to a <see cref="Triple"/> instance without needing to
        /// marshal to managed and then back again just to create the <see cref="Triple"/>
        /// instance.
        /// </ImplementationNote>
        ObjectLayer Create(ExecutionSession session, string triple);
    }
}

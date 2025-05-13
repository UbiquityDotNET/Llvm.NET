// -----------------------------------------------------------------------
// <copyright file="Switch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Switch instruction</summary>
    public sealed class Switch
        : Terminator
    {
        /// <summary>Gets the default <see cref="BasicBlock"/> for the switch</summary>
        public BasicBlock Default => BasicBlock.FromHandle( LLVMGetSwitchDefaultDest( Handle ).ThrowIfInvalid( ) )!;

        /// <summary>Adds a new case to the <see cref="Switch"/> instruction</summary>
        /// <param name="onVal">Value for the case to match</param>
        /// <param name="destination">Destination <see cref="BasicBlock"/> if the case matches</param>
        public void AddCase( Value onVal, BasicBlock destination )
        {
            ArgumentNullException.ThrowIfNull( onVal );
            ArgumentNullException.ThrowIfNull( destination );

            LLVMAddCase( Handle, onVal.Handle, destination.BlockHandle );
        }

        internal Switch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="FuncletPad.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Funclet pad for exception handling</summary>
    public class FuncletPad
        : Instruction
    {
        /// <summary>Gets the outer EH-pad this funclet is nested withing</summary>
        /// <remarks>
        /// <note type="note">This returns the associated <see cref="CatchSwitch"/> if this
        /// <see cref="FuncletPad"/> is a <see cref="CleanupPad"/> instruction.</note>
        /// </remarks>
        public Value ParentPad => Operands[ ^1 ]!;

        /// <summary>Gets the argument operands for this <see cref="FuncletPad"/>.</summary>
        public IOperandCollection<Value?> ArgOperands => ((IOperandCollection<Value?>)Operands)[1..];

        internal FuncletPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="LandingPad.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Marks a <see cref="BasicBlock"/> as a catch handler</summary>
    /// <remarks>
    /// Like the <see cref="CatchPad"/>, instruction this must be the first non-phi instruction
    /// in the block.
    /// </remarks>
    /// <seealso href="xref:llvm_langref#i-landingpad">LLVM landing Instruction</seealso>
    /// <seealso href="xref:llvm_exception_handling#exception-handling-in-llvm">Exception Handling in LLVM</seealso>
    public class LandingPad
        : Instruction
    {
        /// <summary>Gets or sets a value indicating whether this <see cref="LandingPad"/> is a cleanup pad</summary>
        public bool IsCleanup
        {
            get => LLVMIsCleanup( ValueHandle );
            set => LLVMSetCleanup( ValueHandle, value );
        }

        /// <summary>Gets the collection of clauses for this landing pad</summary>
        public ValueOperandListCollection<Constant> Clauses { get; }

        internal LandingPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Clauses = new ValueOperandListCollection<Constant>( this );
        }
    }
}

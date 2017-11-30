// <copyright file="LandingPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
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
        /// <summary>Adds a clause to the <see cref="LandingPad"/></summary>
        /// <param name="clause">A catch or filter clause for the LandingPad</param>
        public void AddClause(Value clause)
        {
            if( clause == null )
            {
                throw new ArgumentNullException( nameof( clause ) );
            }

            LLVMAddClause( ValueHandle, clause.ValueHandle);
        }

        /*
        TODO: Implement clauses property
        This basically requires OperandList.Cast<COnstant>.ToList(), but without the overhead of the
        duplication and enumeration caused from running ToList(), see notes in UserOperandList

        unsigned LLVMGetNumClauses(LLVMValueRef LandingPad) {
          return unwrap<LandingPadInst>(LandingPad)->getNumClauses();
        }

        LLVMValueRef LLVMGetClause(LLVMValueRef LandingPad, unsigned Idx) {
          return wrap(unwrap<LandingPadInst>(LandingPad)->getClause(Idx));
        }
        */

        /// <summary>Gets or sets a value indicating whether this <see cref="LandingPad"/> is a cleanup pad</summary>
        public bool IsCleanup
        {
            get => LLVMIsCleanup( ValueHandle );
            set => LLVMSetCleanup( ValueHandle, value );
        }

        internal LandingPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

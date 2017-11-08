// <copyright file="User.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Contains an LLVM User value</summary>
    /// <remarks>
    /// A user is one role in the user->uses relationship
    /// conveyed by the LLVM value model. A User can contain
    /// references (e.g. uses) of other values.
    /// </remarks>
    public class User
        : Value
    {
        /// <summary>Gets a collection of operands for this <see cref="User"/></summary>
        public IReadOnlyList<Value> Operands => OperandList;

        /// <summary>Gets a collection of <see cref="Use"/>s used by this User</summary>
        public IEnumerable<Use> Uses
        {
            get
            {
                LLVMUseRef current = NativeMethods.LLVMGetFirstUse( ValueHandle );
                while( current != default )
                {
                    // TODO: intern the use instances?
                    yield return new Use( current );
                    current = NativeMethods.LLVMGetNextUse( current );
                }
            }
        }

        internal User( LLVMValueRef userRef )
            : base( userRef )
        {
            OperandList = new UserOperandList( this );
        }

        private readonly UserOperandList OperandList;
    }
}

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

        /// <summary>Gets an operand at the specified index cast to a specific type</summary>
        /// <typeparam name="T">TYpe to cast the operand to</typeparam>
        /// <param name="index">Index of the operand</param>
        /// <returns>Value at the specified index cast to <typeparamref name="T"/></returns>
        public T GetOperand<T>( int index )
            where T : Value
        {
            return ( T )Operands[ index ];
        }

        internal User( LLVMValueRef userRef )
            : base( userRef )
        {
            OperandList = new UserOperandList( this );
        }

        private readonly UserOperandList OperandList;
    }
}

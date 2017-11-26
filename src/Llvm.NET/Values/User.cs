// <copyright file="User.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

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
        /// <remarks>
        /// This property is intended as a temporary measure and is expected to be removed
        /// in the future. (e.g. callers should consider this obsolete for the most part
        /// and only use it when the properties of a <see cref="User"/> or derived type
        /// do not provide a more appropriate explicit property. When all such types have
        /// properties to wrap this low level list, this property will be officially
        /// obsoleted.
        /// </remarks>
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
            if( Operands.Count == 0 )
            {
                throw new InvalidOperationException( "Cannot index into an empty list" );
            }

            if( index < 0 )
            {
                index = Operands.Count - 1 - index;
            }

            index.ValidateRange( 0, Operands.Count - 1, nameof( index ) );

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

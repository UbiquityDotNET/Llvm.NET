// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Contains an LLVM User value</summary>
    /// <remarks>
    /// A user is one role in the user->uses relationship
    /// conveyed by the LLVM value model. A User can contain
    /// references (e.g. uses) of other values.
    /// </remarks>
    public class User
        : Value
        , IOperandContainer<Value>
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
        public IList<Value> Operands => OperandList;

        /// <summary>Gets a collection of <see cref="Use"/>s used by this User</summary>
        public IEnumerable<Use> Uses
        {
            get
            {
                LLVMUseRef current = LLVMGetFirstUse( ValueHandle );
                while( current != default )
                {
                    // TODO: intern the use instances?
                    yield return new Use( current );
                    current = LLVMGetNextUse( current );
                }
            }
        }

        /// <summary>Gets an operand at the specified index cast to a specific type</summary>
        /// <typeparam name="T">Type to cast the operand to</typeparam>
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
                index = Operands.Count + index;
            }

            index.ValidateRange( 0, Operands.Count - 1, nameof( index ) );

            return ( T )Operands[ index ];
        }

        /// <summary>Gets an operand at the specified index cast to a specific type</summary>
        /// <typeparam name="T">Type to cast the operand to</typeparam>
        /// <param name="index">Index of the operand</param>
        /// <param name="value">value for the operand</param>
        public void SetOperand<T>( int index, T value )
            where T : Value
        {
            if( Operands.Count == 0 )
            {
                throw new InvalidOperationException( "Cannot index into an empty list" );
            }

            if( index < 0 )
            {
                index = Operands.Count + index;
            }

            index.ValidateRange( 0, Operands.Count - 1, nameof( index ) );

            Operands[ index ] = value;
        }

        /// <inheritdoc/>
        long IOperandContainer<Value>.Count => LLVMGetNumOperands( ValueHandle );

        /// <inheritdoc/>
        Value? IOperandContainer<Value>.this[ int index ]
        {
            get => FromHandle( LLVMGetOperand( ValueHandle, ( uint )index ) );
            set => LLVMSetOperand( ValueHandle, ( uint )index, value?.ValueHandle ?? LLVMValueRef.Zero );
        }

        /// <inheritdoc/>
        void IOperandContainer<Value>.Add( Value? item ) => throw new NotSupportedException( );

        internal User( LLVMValueRef userRef )
            : base( userRef )
        {
            OperandList = new OperandList<Value>( this );
        }

        private readonly OperandList<Value> OperandList;
    }
}

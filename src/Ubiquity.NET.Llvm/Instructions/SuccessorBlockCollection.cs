// -----------------------------------------------------------------------
// <copyright file="SuccessorBlockCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Support class to provide read/update semantics for the successor blocks of an instruction</summary>
    /// <remarks>
    /// This class is used to implement Operand lists of elements including sub lists based on an offset.
    /// The latter case is useful for types that expose some fixed set of operands as properties and some
    /// arbitrary number of additional items as operands.
    /// </remarks>
    public sealed class SuccessorBlockCollection
        : IOperandCollection<BasicBlock>
    {
        /// <inheritdoc/>
        public BasicBlock this[ int index ]
        {
            get
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                return BasicBlock.FromHandle(LLVMGetSuccessor( Container.ValueHandle, (uint)index ))!;
            }

            set
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );
                LLVMSetSuccessor( Container.ValueHandle, ( uint )index, value?.BlockHandle ?? default );
            }
        }

        /// <inheritdoc/>
        public int Count => checked((int)LLVMGetNumSuccessors( Container.ValueHandle ));

        /// <inheritdoc/>
        public IEnumerator<BasicBlock> GetEnumerator( )
        {
            for( int i = 0; i < Count; ++i )
            {
                yield return BasicBlock.FromHandle( LLVMGetSuccessor( Container.ValueHandle, ( uint )i ) )!;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        /// <inheritdoc/>
        public bool Contains( BasicBlock item ) => this.Any( n => n == item );

        internal SuccessorBlockCollection( Instruction container )
        {
            Container = container;
        }

        private readonly Instruction Container;
    }
}

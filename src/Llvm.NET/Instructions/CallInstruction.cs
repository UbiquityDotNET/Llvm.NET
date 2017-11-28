// <copyright file="Call.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Call instruction</summary>
    /// <seealso href="xref:llvm_langref#call-instruction"/>
    public class CallInstruction
        : Instruction
        , IAttributeAccessor
    {
        /// <summary>Gets the target function of the call</summary>
        public Function TargetFunction => FromHandle<Function>( LLVMGetCalledValue( ValueHandle ) );

        /// <summary>Gets or sets a value indicating whether the call is a tail call</summary>
        public bool IsTailCall
        {
            get => LLVMIsTailCall( ValueHandle );
            set => LLVMSetTailCall( ValueHandle, value );
        }

        /// <summary>Gets the attributes for this call site</summary>
        public IAttributeDictionary Attributes { get; }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );
            LLVMAddCallSiteAttribute( ValueHandle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return LLVMGetCallSiteAttributeCount( ValueHandle, ( LLVMAttributeIndex )index );
        }

        /// <inheritdoc/>
        public IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index )
        {
            uint count = GetAttributeCountAtIndex( index );
            if( count == 0 )
            {
                return Enumerable.Empty<AttributeValue>();
            }

            var buffer = new LLVMAttributeRef[ count ];
            LLVMGetCallSiteAttributes( ValueHandle, ( LLVMAttributeIndex )index, out buffer[ 0 ] );
            return from attribRef in buffer
                   select AttributeValue.FromHandle( Context, attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = LLVMGetCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "name cannot be null or empty", nameof( name ) );
            }

            var handle = LLVMGetCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            LLVMRemoveCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            LLVMRemoveCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        internal CallInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ()=>TargetFunction );
        }
    }
}

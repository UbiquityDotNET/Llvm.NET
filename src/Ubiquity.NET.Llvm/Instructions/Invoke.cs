// -----------------------------------------------------------------------
// <copyright file="Invoke.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to invoke (call) a function with exception handling</summary>
    /// <seealso href="xref:llvm_langref#i-invoke">LLVM invoke Instruction</seealso>
    public class Invoke
        : Terminator
        , IAttributeAccessor
    {
        /// <summary>Gets the target function of the invocation</summary>
        public Function TargetFunction => FromHandle<Function>( LLVMGetCalledValue( ValueHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets or sets the normal destination for the invoke</summary>
        public BasicBlock NormalDestination
        {
            get => BasicBlock.FromHandle( LLVMGetNormalDest( ValueHandle ).ThrowIfInvalid() )!;
            set => LLVMSetNormalDest( ValueHandle, value.ThrowIfNull().BlockHandle );
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
                return Enumerable.Empty<AttributeValue>( );
            }

            var buffer = new LLVMAttributeRef[ count ];
            LLVMGetCallSiteAttributes( ValueHandle, ( LLVMAttributeIndex )index, buffer );
            return from attribRef in buffer
                   select AttributeValue.FromHandle( Context, attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = LLVMGetCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, (uint)kind );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMGetCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            LLVMRemoveCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, (uint)kind );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            LLVMRemoveCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        internal Invoke( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ( ) => TargetFunction );
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="CallBr.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Ubiquity.NET.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>CallBr instruction</summary>
    /// <seealso href="xref:llvm_langref#i-callbr"/>
    public class CallBr
        : Instruction
        , IAttributeAccessor
    {
        /// <summary>Gets the target function of the call</summary>
        public IrFunction TargetFunction
            => FromHandle<IrFunction>( LLVMGetCalledValue( ValueHandle ).ThrowIfInvalid( ) )!;

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
                return [];
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
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

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
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            LLVMRemoveCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        internal CallBr( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ( ) => TargetFunction );
        }
    }
}

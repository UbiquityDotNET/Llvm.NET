// <copyright file="Call.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    public class CallInstruction
        : Instruction
        , IAttributeAccessor
    {
        public Function TargetFunction => FromHandle<Function>( NativeMethods.GetCalledValue( ValueHandle ) );

        public bool IsTailCall
        {
            get => NativeMethods.IsTailCall( ValueHandle );
            set => NativeMethods.SetTailCall( ValueHandle, value );
        }

        public IAttributeDictionary Attributes { get; }

        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );
            NativeMethods.AddCallSiteAttribute( ValueHandle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return NativeMethods.GetCallSiteAttributeCount( ValueHandle, ( LLVMAttributeIndex )index );
        }

        public IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index )
        {
            uint count = GetAttributeCountAtIndex( index );
            if( count == 0 )
            {
                return Enumerable.Empty<AttributeValue>();
            }

            var buffer = new LLVMAttributeRef[ count ];
            NativeMethods.GetCallSiteAttributes( ValueHandle, ( LLVMAttributeIndex )index, out buffer[ 0 ] );
            return from attribRef in buffer
                   select AttributeValue.FromHandle( Context, attribRef );
        }

        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = NativeMethods.GetCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
            return AttributeValue.FromHandle( Context, handle );
        }

        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "name cannot be null or empty", nameof( name ) );
            }

            var handle = NativeMethods.GetCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
            return AttributeValue.FromHandle( Context, handle );
        }

        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            NativeMethods.RemoveCallSiteEnumAttribute( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
        }

        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            NativeMethods.RemoveCallSiteStringAttribute( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        internal CallInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ()=>TargetFunction );
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="CallInstruction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Call instruction</summary>
    /// <seealso href="xref:llvm_langref#call-instruction"/>
    public class CallInstruction
        : Instruction
        , IAttributeAccessor
    {
        /// <summary>Gets the target function of the call</summary>
        public Function TargetFunction
            => FromHandle<Function>( LLVMGetCalledValue( Handle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets or sets a value indicating whether the call is a tail call</summary>
        public bool IsTailCall
        {
            get => LLVMIsTailCall( Handle );
            set => LLVMSetTailCall( Handle, value );
        }

        /// <summary>Gets the attributes for this call site</summary>
        public IAttributeDictionary Attributes { get; }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );
            LLVMAddCallSiteAttribute( Handle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return LLVMGetCallSiteAttributeCount( Handle, ( LLVMAttributeIndex )index );
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
            LLVMGetCallSiteAttributes( Handle, ( LLVMAttributeIndex )index, buffer );
            return from attribRef in buffer
                   select new AttributeValue( attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = LLVMGetCallSiteEnumAttribute( Handle, ( LLVMAttributeIndex )index, (uint)kind );
            return new AttributeValue( handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            var handle = LLVMGetCallSiteStringAttribute( Handle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
            return new AttributeValue( handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            LLVMRemoveCallSiteEnumAttribute( Handle, ( LLVMAttributeIndex )index, (uint)kind );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            LLVMRemoveCallSiteStringAttribute( Handle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        internal CallInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ( ) => TargetFunction );
        }
    }
}

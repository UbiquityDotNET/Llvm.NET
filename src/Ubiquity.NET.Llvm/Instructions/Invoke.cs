// -----------------------------------------------------------------------
// <copyright file="Invoke.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to invoke (call) a function with exception handling</summary>
    /// <seealso href="xref:llvm_langref#i-invoke">LLVM invoke Instruction</seealso>
    public sealed class Invoke
        : Terminator
        , IAttributeAccessor
    {
        /// <summary>Gets the target function of the invocation</summary>
        public Function TargetFunction => FromHandle<Function>( LLVMGetCalledValue( Handle ).ThrowIfInvalid() )!;

        /// <summary>Gets or sets the normal destination for the invoke</summary>
        public BasicBlock NormalDestination
        {
            get => BasicBlock.FromHandle( LLVMGetNormalDest( Handle ).ThrowIfInvalid() )!;
            set => LLVMSetNormalDest( Handle, value.ThrowIfNull().BlockHandle );
        }

        /// <summary>Gets the attributes for this call site</summary>
        public IAttributeDictionary Attributes { get; }

        /// <inheritdoc/>
        public void AddAttributeAtIndex(FunctionAttributeIndex index, AttributeValue attrib)
        {
            attrib.VerifyValidOn( index, this );

            LLVMAddCallSiteAttribute( Handle, (LLVMAttributeIndex)index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex(FunctionAttributeIndex index)
        {
            return LLVMGetCallSiteAttributeCount( Handle, (LLVMAttributeIndex)index );
        }

        /// <inheritdoc/>
        public IEnumerable<AttributeValue> GetAttributesAtIndex(FunctionAttributeIndex index)
        {
            uint count = GetAttributeCountAtIndex( index );
            if(count == 0)
            {
                return Enumerable.Empty<AttributeValue>();
            }

            var buffer = new LLVMAttributeRef[ count ];
            LLVMGetCallSiteAttributes( Handle, (LLVMAttributeIndex)index, buffer );
            return from attribRef in buffer
                   select new AttributeValue( attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex(FunctionAttributeIndex index, AttributeKind kind)
        {
            var handle = LLVMGetCallSiteEnumAttribute( Handle, ( LLVMAttributeIndex )index, (uint)kind );
            return new( handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex(FunctionAttributeIndex index, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMGetCallSiteStringAttribute( Handle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
            return new( handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex(FunctionAttributeIndex index, AttributeKind kind)
        {
            LLVMRemoveCallSiteEnumAttribute( Handle, (LLVMAttributeIndex)index, (uint)kind );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex(FunctionAttributeIndex index, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            LLVMRemoveCallSiteStringAttribute( Handle, (LLVMAttributeIndex)index, name, (uint)name.Length );
        }

        internal Invoke(LLVMValueRef valueRef)
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, () => TargetFunction );
        }
    }
}

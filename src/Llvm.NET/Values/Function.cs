// <copyright file="Function.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Llvm.NET.DebugInfo;
using Llvm.NET.Native;
using Llvm.NET.Types;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Function definition</summary>
    public class Function
        : GlobalObject
        , IAttributeAccessor
    {
        /// <summary>Gets the signature type of the function</summary>
        public IFunctionType Signature => TypeRef.FromHandle<IFunctionType>( LLVMGetElementType( LLVMTypeOf( ValueHandle ) ) );

        /// <summary>Gets the Entry block for this function</summary>
        public BasicBlock EntryBlock
        {
            get
            {
                if( LLVMCountBasicBlocks( ValueHandle ) == 0 )
                {
                    return null;
                }

                return BasicBlock.FromHandle( LLVMGetEntryBasicBlock( ValueHandle ) );
            }
        }

        /// <summary>Gets the basic blocks for the function</summary>
        public ICollection<BasicBlock> BasicBlocks { get; }

        /// <summary>Gets the parameters for the function including any method definition specific attributes (i.e. ByVal)</summary>
        public IReadOnlyList<Argument> Parameters => new FunctionParameterList( this );

        /// <summary>Gets or sets the Calling convention for the method</summary>
        public CallingConvention CallingConvention
        {
            get => ( CallingConvention )LLVMGetFunctionCallConv( ValueHandle );
            set => LLVMSetFunctionCallConv( ValueHandle, ( uint )value );
        }

        /// <summary>Gets the LLVM instrinsicID for the method</summary>
        public uint IntrinsicId => LLVMGetIntrinsicID( ValueHandle );

        /// <summary>Gets a value indicating whether the method signature accepts variable arguments</summary>
        public bool IsVarArg => Signature.IsVarArg;

        /// <summary>Gets the return type of the function</summary>
        public ITypeRef ReturnType => Signature.ReturnType;

        /// <summary>Gets or sets the personality function for exception handling in this function</summary>
        public Function PersonalityFunction
        {
            get
            {
                if( !LLVMHasPersonalityFn( ValueHandle ) )
                {
                    return null;
                }

                return FromHandle<Function>( LLVMGetPersonalityFn( ValueHandle ) );
            }

            set => LLVMSetPersonalityFn( ValueHandle, value?.ValueHandle ?? new LLVMValueRef( IntPtr.Zero ) );
        }

        /// <summary>Gets or sets the debug information for this function</summary>
        public DISubProgram DISubProgram
        {
            get => MDNode.FromHandle<DISubProgram>( LLVMFunctionGetSubprogram( ValueHandle ) );

            set
            {
                if( ( value != null ) && !value.Describes( this ) )
                {
                    throw new ArgumentException( "Subprogram does not describe this Function" );
                }

                LLVMFunctionSetSubprogram( ValueHandle, value?.MetadataHandle ?? default );
            }
        }

        /// <summary>Gets or sets the Garbage collection engine name that this function is generated to work with</summary>
        /// <seealso href="xref:llvm_docs_garbagecollection">Garbage Collection with LLVM</seealso>
        public string GcName
        {
            get => LLVMGetGC( ValueHandle );
            set => LLVMSetGC( ValueHandle, value );
        }

        /// <summary>Gets the attributes for this function</summary>
        public IAttributeDictionary Attributes { get; }

        /// <summary>Verifies the function is valid and all blocks properly terminated</summary>
        public void Verify( )
        {
            if( !Verify(out string errMsg ) )
            {
                throw new InternalCodeGeneratorException( errMsg );
            }
        }

        /// <summary>Verifies the function without throwing an exception</summary>
        /// <param name="errMsg">Error message if any, or <see cref="String.Empty"/> if no errors detected</param>
        /// <returns><see langword="true"/> if no errors found</returns>
        public bool Verify( out string errMsg )
        {
            errMsg = string.Empty;
            return LLVMVerifyFunctionEx( ValueHandle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out errMsg ).Succeeded;
        }

        /// <summary>Add a new basic block to the beginning of a function</summary>
        /// <param name="name">Name (label) for the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted at the beginning of the function</returns>
        public BasicBlock PrependBasicBlock( string name )
        {
            LLVMBasicBlockRef firstBlock = LLVMGetFirstBasicBlock( ValueHandle );
            BasicBlock retVal;
            if( firstBlock == default )
            {
                retVal = AppendBasicBlock( name );
            }
            else
            {
                var blockRef = LLVMInsertBasicBlockInContext( NativeType.Context.ContextHandle, firstBlock, name );
                retVal = BasicBlock.FromHandle( blockRef );
            }

            return retVal;
        }

        /// <summary>Appends a new basic block to a function</summary>
        /// <param name="name">Name (label) of the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted onto the end of the function</returns>
        public BasicBlock AppendBasicBlock( string name )
        {
            LLVMBasicBlockRef blockRef = LLVMAppendBasicBlockInContext( NativeType.Context.ContextHandle, ValueHandle, name );
            return BasicBlock.FromHandle( blockRef );
        }

        /// <summary>Retrieves or creates block by name</summary>
        /// <param name="name">Block name (label) to look for or create</param>
        /// <returns><see cref="BasicBlock"/> If the block was created it is appended to the end of function</returns>
        /// <remarks>
        /// This method tries to find a block by it's name and returns it if found, if not found a new block is
        /// created and appended to the current function.
        /// </remarks>
        public BasicBlock FindOrCreateNamedBlock( string name )
        {
            var retVal = BasicBlocks.FirstOrDefault( b => b.Name == name );
            if( retVal is null )
            {
                retVal = AppendBasicBlock( name );
            }

            Debug.Assert( retVal.ContainingFunction.ValueHandle == ValueHandle, "Expected block parented to this function" );
            return retVal;
        }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );

            LLVMAddAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return LLVMGetAttributeCountAtIndex( ValueHandle, ( LLVMAttributeIndex )index );
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
            LLVMGetAttributesAtIndex( ValueHandle, ( LLVMAttributeIndex )index, out buffer[0] );
            return from attribRef in buffer
                   select AttributeValue.FromHandle( Context, attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = LLVMGetEnumAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "name cannot be null or empty", nameof( name ) );
            }

            var handle = LLVMGetStringAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, name, (uint)name.Length );
            return AttributeValue.FromHandle( Context, handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            LLVMRemoveEnumAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Name cannot be null or empty", nameof( name ) );
            }

            LLVMRemoveStringAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        /// <summary>Removes this function from the parent module</summary>
        public void EraseFromParent()
        {
            LLVMDeleteFunction( ValueHandle );
        }

        internal Function( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ()=>this );
            BasicBlocks = new BasicBlockCollection( this );
        }
    }
}

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

namespace Llvm.NET.Values
{
    /// <summary>LLVM Function definition</summary>
    public class Function
        : GlobalObject
        , IAttributeAccessor
    {
        /// <summary>Gets the signature type of the function</summary>
        public IFunctionType Signature => TypeRef.FromHandle<IFunctionType>( NativeMethods.LLVMGetElementType( NativeMethods.LLVMTypeOf( ValueHandle ) ) );

        /// <summary>Gets the Entry block for this function</summary>
        public BasicBlock EntryBlock
        {
            get
            {
                if( NativeMethods.LLVMCountBasicBlocks( ValueHandle ) == 0 )
                {
                    return null;
                }

                return BasicBlock.FromHandle( NativeMethods.LLVMGetEntryBasicBlock( ValueHandle ) );
            }
        }

        /// <summary>Gets the basic blocks for the function</summary>
        public IReadOnlyList<BasicBlock> BasicBlocks
        {
            get
            {
                uint count = NativeMethods.LLVMCountBasicBlocks( ValueHandle );
                var buf = new LLVMBasicBlockRef[ count ];
                if( count > 0 )
                {
                    NativeMethods.LLVMGetBasicBlocks( ValueHandle, out buf[ 0 ] );
                }

                return buf.Select( BasicBlock.FromHandle )
                          .ToList( )
                          .AsReadOnly( );
            }
        }

        /// <summary>Gets the parameters for the function including any method definition specific attributes (i.e. ByVal)</summary>
        public IReadOnlyList<Argument> Parameters => new FunctionParameterList( this );

        /// <summary>Gets or sets the Calling convention for the method</summary>
        public CallingConvention CallingConvention
        {
            get => ( CallingConvention )NativeMethods.LLVMGetFunctionCallConv( ValueHandle );
            set => NativeMethods.LLVMSetFunctionCallConv( ValueHandle, ( uint )value );
        }

        /// <summary>Gets the LLVM instrinsicID for the method</summary>
        public uint IntrinsicId => NativeMethods.LLVMGetIntrinsicID( ValueHandle );

        /// <summary>Gets a value indicating whether the method signature accepts variable arguments</summary>
        public bool IsVarArg => Signature.IsVarArg;

        /// <summary>Gets the return type of the function</summary>
        public ITypeRef ReturnType => Signature.ReturnType;

        /// <summary>Gets or sets the personality function for exception handling in this function</summary>
        public Function PersonalityFunction
        {
            get
            {
                if( !NativeMethods.LLVMHasPersonalityFn( ValueHandle ) )
                {
                    return null;
                }

                return FromHandle<Function>( NativeMethods.LLVMGetPersonalityFn( ValueHandle ) );
            }

            set => NativeMethods.LLVMSetPersonalityFn( ValueHandle, value?.ValueHandle ?? new LLVMValueRef( IntPtr.Zero ) );
        }

        /// <summary>Gets or sets the debug information for this function</summary>
        public DISubProgram DISubProgram
        {
            get => MDNode.FromHandle<DISubProgram>( NativeMethods.LLVMFunctionGetSubprogram( ValueHandle ) );

            set
            {
                if( ( value != null ) && !value.Describes( this ) )
                {
                    throw new ArgumentException( "Subprogram does not describe this Function" );
                }

                NativeMethods.LLVMFunctionSetSubprogram( ValueHandle, value?.MetadataHandle ?? default );
            }
        }

        /// <summary>Gets or sets the Garbage collection engine name that this function is generated to work with</summary>
        /// <seealso href="xref:llvm_doc_garbagecollection"/>
        public string GcName
        {
            get => NativeMethods.LLVMGetGC( ValueHandle );
            set => NativeMethods.LLVMSetGC( ValueHandle, value );
        }

        public IAttributeDictionary Attributes { get; }

        /// <summary>Verifies the function is valid and all blocks properly terminated</summary>
        public void Verify( )
        {
            if( NativeMethods.LLVMVerifyFunctionEx( ValueHandle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out string errMsg ).Failed )
            {
                throw new InternalCodeGeneratorException( errMsg );
            }
        }

        /// <summary>Add a new basic block to the beginning of a function</summary>
        /// <param name="name">Name (label) for the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted at the beginning of the function</returns>
        public BasicBlock PrependBasicBlock( string name )
        {
            LLVMBasicBlockRef firstBlock = NativeMethods.LLVMGetFirstBasicBlock( ValueHandle );
            BasicBlock retVal;
            if( firstBlock == default )
            {
                retVal = AppendBasicBlock( name );
            }
            else
            {
                var blockRef = NativeMethods.LLVMInsertBasicBlockInContext( NativeType.Context.ContextHandle, firstBlock, name );
                retVal = BasicBlock.FromHandle( blockRef );
            }

            return retVal;
        }

        /// <summary>Appends a new basic block to a function</summary>
        /// <param name="name">Name (label) of the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted onto the end of the function</returns>
        public BasicBlock AppendBasicBlock( string name )
        {
            LLVMBasicBlockRef blockRef = NativeMethods.LLVMAppendBasicBlockInContext( NativeType.Context.ContextHandle, ValueHandle, name );
            return BasicBlock.FromHandle( blockRef );
        }

        /// <summary>Retrieves or creates  block by name</summary>
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

        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );

            NativeMethods.LLVMAddAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return NativeMethods.LLVMGetAttributeCountAtIndex( ValueHandle, ( LLVMAttributeIndex )index );
        }

        public IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index )
        {
            uint count = GetAttributeCountAtIndex( index );
            if( count == 0 )
            {
                return Enumerable.Empty<AttributeValue>( );
            }

            var buffer = new LLVMAttributeRef[ count ];
            NativeMethods.LLVMGetAttributesAtIndex( ValueHandle, ( LLVMAttributeIndex )index, out buffer[0] );
            return from attribRef in buffer
                   select AttributeValue.FromHandle( Context, attribRef );
        }

        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = NativeMethods.LLVMGetEnumAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
            return AttributeValue.FromHandle( Context, handle );
        }

        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "name cannot be null or empty", nameof( name ) );
            }

            var handle = NativeMethods.LLVMGetStringAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, name, (uint)name.Length );
            return AttributeValue.FromHandle( Context, handle );
        }

        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            NativeMethods.LLVMRemoveEnumAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, kind.GetEnumAttributeId( ) );
        }

        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Name cannot be null or empty", nameof( name ) );
            }

            NativeMethods.LLVMRemoveStringAttributeAtIndex( ValueHandle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
        }

        public void EraseFromParent()
        {
            NativeMethods.LLVMDeleteFunction( ValueHandle );
        }

        internal Function( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ()=>this );
        }
    }
}

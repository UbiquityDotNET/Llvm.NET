// -----------------------------------------------------------------------
// <copyright file="Function.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// until compiler bug [#40325](https://github.com/dotnet/roslyn/issues/40325) is resolved disable this
// as adding the parameter would result in 'warning: Duplicate parameter 'passes' found in comments, the latter one is ignored.'
// when generating the final documentation. (It picks the first one and reports a warning).
// Unfortunately there's no way to suppress this without cluttering up the code with pragmas.
// [ Sadly the SuppressMessageAttribute does NOT work for this warning. ]
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AnalysisBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.PassBuilder;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>LLVM Function definition</summary>
    public class Function
        : GlobalObject
        , IFunctionAttributeAccessor
    {
        /// <summary>Gets the signature type of the function</summary>
        public IFunctionType Signature => (IFunctionType)ValueType;

        /// <summary>Gets the Entry block for this function</summary>
        public BasicBlock? EntryBlock
            => LLVMCountBasicBlocks( Handle ) == 0 ? null : BasicBlock.FromHandle( LLVMGetEntryBasicBlock( Handle ) );

        /// <summary>Gets the basic blocks for the function</summary>
        public ICollection<BasicBlock> BasicBlocks => new BasicBlockCollection( this );

        /// <summary>Gets the parameters for the function including any method definition specific attributes (i.e. ByVal)</summary>
        public IReadOnlyList<Argument> Parameters => new FunctionParameterList( this );

        /// <summary>Gets or sets the Calling convention for the method</summary>
        public CallingConvention CallingConvention
        {
            get => ( CallingConvention )LLVMGetFunctionCallConv( Handle );
            set => LLVMSetFunctionCallConv( Handle, ( uint )value );
        }

        /// <summary>Gets the LLVM intrinsicID for the method</summary>
        public uint IntrinsicId => LLVMGetIntrinsicID( Handle );

        /// <summary>Gets a value indicating whether the method signature accepts variable arguments</summary>
        public bool IsVarArg => Signature.IsVarArg;

        /// <summary>Gets the return type of the function</summary>
        public ITypeRef ReturnType => Signature.ReturnType;

        /// <summary>Gets or sets the personality function for exception handling in this function</summary>
        public Function? PersonalityFunction
        {
            get => !LLVMHasPersonalityFn( Handle ) ? null : FromHandle<Function>( LLVMGetPersonalityFn( Handle.ThrowIfInvalid( ) ) )!;

            set => LLVMSetPersonalityFn( Handle, value?.Handle ?? LLVMValueRef.Zero );
        }

        /// <summary>Gets or sets the debug information for this function</summary>
        public DISubProgram? DISubProgram
        {
            get => (DISubProgram?)LLVMGetSubprogram( Handle ).CreateMetadata( );

            set
            {
                ArgumentNullException.ThrowIfNull(value);
                LLVMSetSubprogram( Handle, value.Handle );
            }
        }

        /// <summary>Gets or sets the Garbage collection engine name that this function is generated to work with</summary>
        /// <seealso href="xref:llvm_docs_garbagecollection">Garbage Collection with LLVM</seealso>
        public string GcName
        {
            get => LLVMGetGC( Handle ) ?? string.Empty;
            set => LLVMSetGC( Handle, value );
        }

        /// <summary>Verifies the function is valid and all blocks properly terminated</summary>
        public void Verify( )
        {
            if( !Verify( out string? errMsg ) )
            {
                throw new InternalCodeGeneratorException( errMsg );
            }
        }

        /// <summary>Verifies the function without throwing an exception</summary>
        /// <param name="errMsg">Error message if any, or <see cref="string.Empty"/> if no errors detected</param>
        /// <returns><see langword="true"/> if no errors found</returns>
        public bool Verify( [MaybeNullWhen(true)] out string errMsg )
        {
            return LibLLVMVerifyFunctionEx( Handle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out errMsg ).Succeeded;
        }

        /// <summary>Add a new basic block to the beginning of a function</summary>
        /// <param name="name">Name (label) for the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted at the beginning of the function</returns>
        public BasicBlock PrependBasicBlock( string name )
        {
            LLVMBasicBlockRef firstBlock = LLVMGetFirstBasicBlock( Handle );
            BasicBlock retVal;
            if( firstBlock == default )
            {
                retVal = AppendBasicBlock( name );
            }
            else
            {
                var blockRef = LLVMInsertBasicBlockInContext( NativeType.Context.GetUnownedHandle(), firstBlock, name );
                retVal = BasicBlock.FromHandle( blockRef.ThrowIfInvalid( ) )!;
            }

            return retVal;
        }

        /// <summary>Appends a new basic block to a function</summary>
        /// <param name="block">Existing block to append to the function's list of blocks</param>
        public void AppendBasicBlock( BasicBlock block )
        {
            ArgumentNullException.ThrowIfNull( block );
            LLVMAppendExistingBasicBlock( Handle, block.BlockHandle );
        }

        /// <summary>Creates an appends a new basic block to a function</summary>
        /// <param name="name">Name (label) of the block</param>
        /// <returns><see cref="BasicBlock"/> created and inserted onto the end of the function</returns>
        public BasicBlock AppendBasicBlock( string name )
        {
            LLVMBasicBlockRef blockRef = LLVMAppendBasicBlockInContext( NativeType.Context.GetUnownedHandle(), Handle, name );
            return BasicBlock.FromHandle( blockRef.ThrowIfInvalid( ) )!;
        }

        /// <summary>Inserts a basic block before another block in the function</summary>
        /// <param name="name">Name of the block</param>
        /// <param name="insertBefore">Block to insert the new block before</param>
        /// <returns>New <see cref="BasicBlock"/> inserted</returns>
        /// <exception cref="ArgumentException"><paramref name="insertBefore"/> belongs to a different function</exception>
        public BasicBlock InsertBasicBlock( string name, BasicBlock insertBefore )
        {
            ArgumentNullException.ThrowIfNull( insertBefore );
            if( insertBefore.ContainingFunction != null && !EqualityComparer<Function>.Default.Equals(insertBefore.ContainingFunction, this ) )
            {
                throw new ArgumentException( "Basic block belongs to another function", nameof( insertBefore ) );
            }

            LLVMBasicBlockRef basicBlockRef = LLVMInsertBasicBlockInContext( NativeType.Context.GetUnownedHandle(), insertBefore.BlockHandle, name );
            return BasicBlock.FromHandle( basicBlockRef.ThrowIfInvalid( ) )!;
        }

        /// <summary>Retrieves or creates block by name</summary>
        /// <param name="name">Block name (label) to look for or create</param>
        /// <returns><see cref="BasicBlock"/> If the block was created it is appended to the end of function</returns>
        /// <remarks>
        /// This method tries to find a block by it's name and returns it if found, if not found a new block is
        /// created and appended to the current function.
        /// </remarks>
        public BasicBlock FindOrCreateNamedBlock( LazyEncodedString name )
        {
            return BasicBlocks.FirstOrDefault( b => b.Name?.Equals(name) ?? false ) ?? AppendBasicBlock( name );
        }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            FunctionAttributeAccessor.AddAttributeAtIndex(this, index, attrib);
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return FunctionAttributeAccessor.GetAttributeCountAtIndex(this, index);
        }

        /// <inheritdoc/>
        public IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index )
        {
            return FunctionAttributeAccessor.GetAttributesAtIndex(this, index);
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, UInt32 id )
        {
            return FunctionAttributeAccessor.GetAttributeAtIndex(this, index, id);
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name )
        {
            return FunctionAttributeAccessor.GetAttributeAtIndex(this, index, name);
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, UInt32 id )
        {
            FunctionAttributeAccessor.RemoveAttributeAtIndex(this, index, id);
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name )
        {
            FunctionAttributeAccessor.RemoveAttributeAtIndex(this, index, name);
        }

        /// <summary>Removes this function from the parent module</summary>
        public void EraseFromParent( )
        {
            LLVMDeleteFunction( Handle );
        }

        /// <summary>Tries running the specified passes on this function</summary>
        /// <param name="passes">Set of passes to run [Must contain at least one pass]</param>
        /// <returns>Error containing result</returns>
        /// <exception cref="ArgumentNullException">One of the arguments provided was null (see exception details for name of the parameter)</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="passes"/> has less than one pass. At least one is required</exception>
        /// <remarks>
        /// This will try to run all the passes specified against this function. The return value contains
        /// the results and, if an error occurred, any error message text for the error.
        /// <note type="information">
        /// The `try` semantics apply to the actual LLVM call only, normal parameter checks are performed
        /// and may produce an exception.
        /// </note>
        /// </remarks>
        public ErrorInfo TryRunPasses(params LazyEncodedString[] passes)
        {
            using PassBuilderOptions options = new();
            return TryRunPasses(options, passes);
        }

        /// <inheritdoc cref="TryRunPasses(LazyEncodedString[])"/>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses(PassBuilderOptions options, params LazyEncodedString[] passes)
        {
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            // While not explicitly documented either way, the PassBuilder used under the hood is capable
            // of handling a NULL target machine.
            return new(LLVMRunPassesOnFunction(Handle, LazyEncodedString.Join(',', passes), LLVMTargetMachineRef.Zero, options.Handle));
        }

        /// <inheritdoc cref="TryRunPasses(LazyEncodedString[])"/>
        /// <param name="targetMachine">Target machine for the passes</param>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses(TargetMachine targetMachine, PassBuilderOptions options, params LazyEncodedString[] passes)
        {
            ArgumentNullException.ThrowIfNull(targetMachine);
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            return new(LLVMRunPassesOnFunction(Handle, LazyEncodedString.Join(',', passes), targetMachine.Handle, options.Handle));
        }

        internal Function( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

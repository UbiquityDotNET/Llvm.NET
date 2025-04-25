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

namespace Ubiquity.NET.Llvm.Values
{
    /* values for CallingConvention enum come directly from LLVM's CallingConv.h
    // rather then the mapped C API version as the C version is not
    // a complete set.
    */

    /// <summary>Calling Convention for functions</summary>
    /// <seealso href="xref:llvm_langref#calling-conventions">LLVM calling conventions</seealso>
    public enum CallingConvention
    {
        /// <summary> The default llvm calling convention, compatible with C</summary>
        /// <remarks>
        /// This convention is the only calling convention that supports varargs calls.
        /// As with typical C calling conventions, the callee/caller have to
        /// tolerate certain amounts of prototype mismatch
        /// </remarks>
        C = 0,

        // [gap]

        /// <summary>Fast calling convention</summary>
        FastCall = 8,

        /// <summary>Cold calling</summary>
        ColdCall = 9,

        /// <summary>Glasgow Haskell Compiler</summary>
        GlasgowHaskellCompiler = 10,

        /// <summary>The High=Performance Erlang convention</summary>
        HiPE = 11,

        /// <summary>Webkit JavaScript calling convention</summary>
        WebKitJS = 12,

        /// <summary>Calling convention for dynamic register based calls (e.g.stackmap and patchpoint intrinsics)</summary>
        AnyReg = 13,

        /// <summary>Preserve most calling convention for runtime calls that preserves most registers.</summary>
        PreserveMost = 14,

        /// <summary>Preserve all calling convention for runtime calls that preserves (almost) all registers.</summary>
        PreserveAll = 15,

        /// <summary>Swift calling convention</summary>
        Swift = 16,

        /// <summary>Calling convention for access functions.</summary>
        CxxFastTls = 17,

        // [Gap]

        /// <summary>Marker enum that identifies the start of the target specific conventions all values greater than or equal to this value are target specific</summary>
        FirstTargetSpecific = 64, // [marker]

        /// <summary>X86 stdcall convention</summary>
        /// <remarks>
        /// This calling convention is mostly used by the Win32 API. It is basically the same as the C
        /// convention with the difference in that the callee is responsible for popping the arguments
        /// from the stack.
        /// </remarks>
        X86StdCall = FirstTargetSpecific,

        /// <summary>X86 fast call convention</summary>
        /// <remarks>
        /// 'fast' analog of <see cref="X86StdCall"/>. Passes first two arguments
        /// in ECX:EDX registers, others - via stack. Callee is responsible for
        /// stack cleaning.
        /// </remarks>
        X86FastCall = 65,

        /// <summary>ARM APCS (officially obsolete but some old targets use it)</summary>
        ArmAPCS = 66,

        /// <summary>ARM Architecture Procedure Calling Standard calling convention (aka EABI). Soft float variant</summary>
        ArmAAPCS = 67,

        /// <summary>Same as <see cref="ArmAAPCS"/> but uses hard floating point ABI</summary>
        ArmAAPCSVfp = 68,

        /// <summary>Calling convention used for MSP430 interrupt routines</summary>
        MSP430Interrupt = 69,

        /// <summary>Similar to <see cref="X86StdCall"/>, passes first 'this' argument in ECX all others via stack</summary>
        /// <remarks>
        /// Callee is responsible for stack cleaning. MSVC uses this by default for C++ instance methods in its ABI
        /// </remarks>
        X86ThisCall = 70,

        /// <summary>Call to a PTX kernel</summary>
        /// <remarks>Passes all arguments in parameter space</remarks>
        PtxKernel = 71,

        /// <summary>Call to a PTX device function</summary>
        /// <remarks>
        /// Passes all arguments in register or parameter space.
        /// </remarks>
        PtxDevice = 72,

        /// <summary>Calling convention for SPIR non-kernel device functions.</summary>
        SpirFunction = 75,

        /// <summary>Calling convention for SPIR kernel functions.</summary>
        SpirKernel = 76,

        /// <summary>Calling conventions for Intel OpenCL built-ins</summary>
        IntelOpenCLBuiltIn = 77,

        /// <summary>The C convention as specified in the x86-64 supplement to the System V ABI, used on most non-Windows systems.</summary>
        X86x64SysV = 78,

        /// <summary>The C convention as implemented on Windows/x86-64 and AArch64.</summary>
        /// <remarks>
        /// <para>This convention differs from the more common <see cref="X86x64SysV"/> convention in a number of ways, most notably in
        /// that XMM registers used to pass arguments are shadowed by GPRs, and vice versa.</para>
        /// <para>On AArch64, this is identical to the normal C (AAPCS) calling convention for normal functions,
        /// but floats are passed in integer registers to variadic functions</para>
        /// </remarks>
        X86x64Win64 = 79,

        /// <summary>MSVC calling convention that passes vectors and vector aggregates in SSE registers</summary>
        X86VectorCall = 80,

        /// <summary>Calling convention used by HipHop Virtual Machine (HHVM)</summary>
        HHVM = 81,

        /// <summary>HHVM calling convention for invoking C/C++ helpers</summary>
        HHVMCCall = 82,

        /// <summary>x86 hardware interrupt context</summary>
        /// <remarks>
        /// Callee may take one or two parameters, where the 1st represents a pointer to hardware context frame
        /// and the 2nd represents hardware error code, the presence of the later depends on the interrupt vector
        /// taken. Valid for both 32- and 64-bit subtargets.
        /// </remarks>
        X86Interrupt = 83,

        /// <summary>Used for AVR interrupt routines</summary>
        AVRInterrupt = 84,

        /// <summary>Calling convention used for AVR signal routines</summary>
        AVRSignal = 85,

        /// <summary>Calling convention used for special AVR rtlib functions which have an "optimized" convention to preserve registers.</summary>
        AVRBuiltIn = 86,

        /// <summary>Calling convention used for Mesa vertex shaders.</summary>
        AMDGpuVertexShader = 87,

        /// <summary>Calling convention used for Mesa geometry shaders.</summary>
        AMDGpuGeometryShader = 88,

        /// <summary>Calling convention used for Mesa pixel shaders.</summary>
        AMDGpuPixelShader = 89,

        /// <summary>Calling convention used for Mesa compute shaders.</summary>
        AMDGpuComputeShader = 90,

        /// <summary>Calling convention for AMDGPU code object kernels.</summary>
        AMDGpuKernel = 91,

        /// <summary>Register calling convention used for parameters transfer optimization</summary>
        X86RegCall = 92,

        /// <summary>Calling convention used for Mesa hull shaders. (= tessellation control shaders)</summary>
        AMDGpuHullShader = 93,

        /// <summary>Calling convention used for special MSP430 rtlib functions which have an "optimized" convention using additional registers.</summary>
        MSP430BuiltIn = 94,

        /// <summary>Calling convention used for AMDPAL vertex shader if tessellation is in use.</summary>
        AMDGpuLS = 95,

        /// <summary>Calling convention used for AMDPAL shader stage before geometry shader if geometry is in use.</summary>
        /// <remarks>
        /// Either the domain (= tessellation evaluation) shader if tessellation is in use, or otherwise the vertex shader.
        /// </remarks>
        AMDGpuEs = 96,

        /// <summary>The highest possible calling convention ID. Must be some 2^k - 1.</summary>
        MaxCallingConvention = 1023
    }

    /// <summary>LLVM Function definition</summary>
    public class Function
        : GlobalObject
        , IAttributeAccessor
    {
        /// <summary>Gets the signature type of the function</summary>
        public IFunctionType Signature => (IFunctionType)ValueType;

        /// <summary>Gets the Entry block for this function</summary>
        public BasicBlock? EntryBlock
            => LLVMCountBasicBlocks( Handle ) == 0 ? null : BasicBlock.FromHandle( LLVMGetEntryBasicBlock( Handle ) );

        /// <summary>Gets the basic blocks for the function</summary>
        public ICollection<BasicBlock> BasicBlocks { get; }

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

        /// <summary>Gets the attributes for this function</summary>
        public IAttributeDictionary Attributes { get; }

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
        public BasicBlock FindOrCreateNamedBlock( string name )
        {
            return BasicBlocks.FirstOrDefault( b => b.Name == name ) ?? AppendBasicBlock( name );
        }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.VerifyValidOn( index, this );

            LLVMAddAttributeAtIndex( Handle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return LLVMGetAttributeCountAtIndex( Handle, ( LLVMAttributeIndex )index );
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
            LLVMGetAttributesAtIndex( Handle, ( LLVMAttributeIndex )index, buffer );
            return from attribRef in buffer
                   select new AttributeValue( attribRef );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            var handle = LLVMGetEnumAttributeAtIndex( Handle, ( LLVMAttributeIndex )index, (uint)kind );
            return new( handle );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            var handle = LLVMGetStringAttributeAtIndex( Handle, ( LLVMAttributeIndex )index, name, (uint)name.Length );
            return new( handle );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind )
        {
            LLVMRemoveEnumAttributeAtIndex( Handle, ( LLVMAttributeIndex )index, (uint)kind );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            LLVMRemoveStringAttributeAtIndex( Handle, ( LLVMAttributeIndex )index, name, ( uint )name.Length );
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
        public ErrorInfo TryRunPasses(params string[] passes)
        {
            using PassBuilderOptions options = new();
            return TryRunPasses(options, passes);
        }

        /// <inheritdoc cref="TryRunPasses(string[])"/>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses(PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            // TODO: [Optimization] provide overload that accepts a ReadOnlySpan<byte> for the pass "string"
            //       That way the overhead of Join() and conversion to native form is performed exactly once.
            //       (NativeStringView ~= ReadOnlySpan<byte>?)

            // While not explicitly documented either way, the PassBuilder used under the hood is capable
            // of handling a NULL target machine.
            return new(LLVMRunPassesOnFunction(Handle, string.Join(',', passes), LLVMTargetMachineRef.Zero, options.Handle));
        }

        /// <inheritdoc cref="TryRunPasses(string[])"/>
        /// <param name="targetMachine">Target machine for the passes</param>
        /// <param name="options">Options for the passes</param>
        public ErrorInfo TryRunPasses(TargetMachine targetMachine, PassBuilderOptions options, params string[] passes)
        {
            ArgumentNullException.ThrowIfNull(targetMachine);
            ArgumentNullException.ThrowIfNull(passes);
            ArgumentOutOfRangeException.ThrowIfLessThan(passes.Length, 1);

            return new(LLVMRunPassesOnFunction(Handle, string.Join(',', passes), targetMachine.Handle, options.Handle));
        }

        internal Function( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Attributes = new ValueAttributeDictionary( this, ( ) => this );
            BasicBlocks = new BasicBlockCollection( this );
        }
    }
}

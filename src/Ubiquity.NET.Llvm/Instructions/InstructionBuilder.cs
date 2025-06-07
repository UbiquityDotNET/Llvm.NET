// -----------------------------------------------------------------------
// <copyright file="InstructionBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>LLVM Instruction builder allowing managed code to generate IR instructions</summary>
    /// <remarks>
    /// <note type="important">
    /// Unlike the underlying LLVM APIs these methods do NOT accept a name parameter. Instead,
    /// the name is added only when needed via the <see cref="ValueExtensions.RegisterName{T}(T, string)"/>
    /// extension method. That method supports a Fluent style pattern to allow mutating additional
    /// properties and handles the possibility of a Constant folded value, which does NOT have a register
    /// name.</note>
    /// </remarks>
    public sealed class InstructionBuilder
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose( ) => Handle.Dispose();

        /// <summary>Initializes a new instance of the <see cref="InstructionBuilder"/> class for a given <see cref="Ubiquity.NET.Llvm.ContextAlias"/></summary>
        /// <param name="context">ContextAlias used for creating instructions</param>
        public InstructionBuilder( IContext context )
        {
            Context = context;
            Handle = LLVMCreateBuilderInContext( context.ThrowIfNull().GetUnownedHandle() );
        }

        /// <summary>Initializes a new instance of the <see cref="InstructionBuilder"/> class for a <see cref="BasicBlock"/></summary>
        /// <param name="block">Block this builder is initially attached to</param>
        public InstructionBuilder( BasicBlock block )
            : this( block.ThrowIfNull().Context )
        {
            PositionAtEnd( block );
        }

        /// <summary>Gets the context this builder is creating instructions for</summary>
        public IContext Context { get; }

        /// <summary>Gets or sets the current Debug Location for this <see cref="InstructionBuilder"/></summary>
        public DILocation? CurrentDebugLocation
        {
            get => (DILocation?)LLVMGetCurrentDebugLocation2( Handle ).CreateMetadata();
            set => LLVMSetCurrentDebugLocation2( Handle, value.ThrowIfNull()!.Handle );
        }

        /// <summary>Set the current debug location for this <see cref="InstructionBuilder"/></summary>
        /// <param name="line">Source line</param>
        /// <param name="col">Source column</param>
        /// <param name="scope"><see cref="DILocalScope"/> for the location</param>
        /// <param name="inlinedAt"><see cref="DILocation"/>the location is inlined into</param>
        /// <returns>This builder for fluent API usage</returns>
        public InstructionBuilder SetDebugLocation( uint line, uint col, DILocalScope scope, DILocation? inlinedAt = null )
        {
            var loc = new DILocation(Context, line, col, scope, inlinedAt);
            return SetDebugLocation( loc );
        }

        /// <summary>Set the current debug location for this <see cref="InstructionBuilder"/></summary>
        /// <param name="location">Location to set</param>
        /// <returns>This builder for fluent API usage</returns>
        public InstructionBuilder SetDebugLocation( DILocation? location )
        {
            ArgumentNullException.ThrowIfNull( location );
            LLVMSetCurrentDebugLocation2( Handle, location?.Handle ?? default );
            return this;
        }

        /// <summary>Gets the <see cref="BasicBlock"/> this builder is building instructions for</summary>
        public BasicBlock? InsertBlock
        {
            get
            {
                var handle = LLVMGetInsertBlock( Handle );
                return handle == default ? null : BasicBlock.FromHandle( LLVMGetInsertBlock( Handle ) );
            }
        }

        /// <summary>Gets the function this builder currently inserts into</summary>
        public Function? InsertFunction => InsertBlock?.ContainingFunction;

        /// <summary>Positions the builder at the end of a given <see cref="BasicBlock"/></summary>
        /// <param name="basicBlock">Block to set the position of</param>
        public void PositionAtEnd( BasicBlock basicBlock )
        {
            ArgumentNullException.ThrowIfNull( basicBlock );

            LLVMPositionBuilderAtEnd( Handle, basicBlock.BlockHandle );
        }

        /// <summary>Positions the builder before the given instruction</summary>
        /// <param name="instr">Instruction to position the builder before</param>
        /// <remarks>This method will position the builder to add new instructions
        /// immediately before the specified instruction.
        /// <note type="note">It is important to keep in mind that this can change the
        /// block this builder is targeting. That is, <paramref name="instr"/>
        /// is not required to come from the same block the instruction builder is
        /// currently referencing.</note>
        /// </remarks>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public void PositionBefore( Instruction instr )
        {
            ArgumentNullException.ThrowIfNull( instr );

            LLVMPositionBuilderBefore( Handle, instr.Handle );
        }

        /// <summary>Appends a basic block after the <see cref="InsertBlock"/> of this <see cref="InstructionBuilder"/></summary>
        /// <param name="block">Block to insert</param>
        public void AppendBasicBlock( BasicBlock block )
        {
            ArgumentNullException.ThrowIfNull( block );
            LLVMInsertExistingBasicBlockAfterInsertBlock( Handle, block.BlockHandle );
        }

        /// <summary>Creates a floating point negation operator</summary>
        /// <param name="value">value to negate</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FNeg( Value value ) => BuildUnaryOp( LLVMBuildFNeg, value );

        /// <summary>Creates a floating point add operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FAdd( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildFAdd, lhs, rhs );

        /// <summary>Creates a floating point subtraction operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FSub( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildFSub, lhs, rhs );

        /// <summary>Creates a floating point multiple operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FMul( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildFMul, lhs, rhs );

        /// <summary>Creates a floating point division operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FDiv( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildFDiv, lhs, rhs );

        /// <summary>Creates a floating point remainder operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value FRem( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildFRem, lhs, rhs );

        /// <summary>Creates an integer negation operator</summary>
        /// <param name="value">operand to negate</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Neg( Value value ) => BuildUnaryOp( LLVMBuildNeg, value );

        /// <summary>Creates an integer logical not operator</summary>
        /// <param name="value">operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        /// <remarks>LLVM IR doesn't actually have a logical not instruction so this is implemented as value XOR {one} </remarks>
        public Value Not( Value value ) => BuildUnaryOp( LLVMBuildNot, value );

        /// <summary>Creates an integer add operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Add( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildAdd, lhs, rhs );

        /// <summary>Creates an integer bitwise and operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value And( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildAnd, lhs, rhs );

        /// <summary>Creates an integer subtraction operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Sub( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildSub, lhs, rhs );

        /// <summary>Creates an integer multiplication operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Mul( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildMul, lhs, rhs );

        /// <summary>Creates an integer shift left operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value ShiftLeft( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildShl, lhs, rhs );

        /// <summary>Creates an integer arithmetic shift right operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value ArithmeticShiftRight( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildAShr, lhs, rhs );

        /// <summary>Creates an integer logical shift right operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value LogicalShiftRight( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildLShr, lhs, rhs );

        /// <summary>Creates an integer unsigned division operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value UDiv( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildUDiv, lhs, rhs );

        /// <summary>Creates an integer signed division operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value SDiv( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildSDiv, lhs, rhs );

        /// <summary>Creates an integer unsigned remainder operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value URem( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildURem, lhs, rhs );

        /// <summary>Creates an integer signed remainder operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value SRem( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildSRem, lhs, rhs );

        /// <summary>Creates an integer bitwise exclusive or operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Xor( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildXor, lhs, rhs );

        /// <summary>Creates an integer bitwise or operator</summary>
        /// <param name="lhs">left hand side operand</param>
        /// <param name="rhs">right hand side operand</param>
        /// <returns><see cref="Value"/> for the instruction</returns>
        public Value Or( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildOr, lhs, rhs );

        /// <summary>Creates an alloca instruction</summary>
        /// <param name="typeRef">Type of the value to allocate</param>
        /// <returns><see cref="Instructions.Alloca"/> instruction</returns>
        public Alloca Alloca( ITypeRef typeRef )
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            var handle = LLVMBuildAlloca( Handle, typeRef.GetTypeRef( ), string.Empty );

            return handle == default
                ? throw new InternalCodeGeneratorException( "Failed to build an Alloca instruction" )
                : Value.FromHandle<Alloca>( handle )!;
        }

        /// <summary>Creates an alloca instruction</summary>
        /// <param name="typeRef">Type of the value to allocate</param>
        /// <param name="elements">Number of elements to allocate</param>
        /// <returns><see cref="Instructions.Alloca"/> instruction</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public Alloca Alloca( ITypeRef typeRef, ConstantInt elements )
        {
            ArgumentNullException.ThrowIfNull( typeRef );
            ArgumentNullException.ThrowIfNull( elements );

            var instHandle = LLVMBuildArrayAlloca( Handle, typeRef.GetTypeRef( ), elements.Handle, string.Empty );
            return instHandle == default
                ? throw new InternalCodeGeneratorException( "Failed to build an Alloca array instruction" )
                : Value.FromHandle<Alloca>( instHandle )!;
        }

        /// <summary>Creates a return instruction for a function that has no return value</summary>
        /// <returns><see cref="ReturnInstruction"/></returns>
        /// <exception cref="ArgumentException"> the function has a non-void return type</exception>
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Devolves to an incomprehensible set of nested conditional expressions - NOT simpler" )]
        public ReturnInstruction Return( )
        {
            if(InsertBlock is null)
            {
                throw new InvalidOperationException( "No insert block is set for this builder" );
            }

            if(InsertBlock.ContainingFunction is null)
            {
                throw new InvalidOperationException( "Insert block is not associated with a function; inserting a return requires validation of the function signature" );
            }

            if(!InsertBlock.ContainingFunction.ReturnType.IsVoid)
            {
                throw new ArgumentException( "Return instruction for non-void function must have a value" );
            }

            return Value.FromHandle<ReturnInstruction>( LLVMBuildRetVoid( Handle ).ThrowIfInvalid() )!;
        }

        /// <summary>Creates a return instruction with the return value for a function</summary>
        /// <param name="value"><see cref="Value"/> to return</param>
        /// <returns><see cref="ReturnInstruction"/></returns>
        public ReturnInstruction Return( Value value )
        {
            ArgumentNullException.ThrowIfNull( value );

            if(InsertBlock is null)
            {
                throw new InvalidOperationException( "No insert block is set for this builder" );
            }

            if(InsertBlock.ContainingFunction is null)
            {
                throw new InvalidOperationException( "Insert block is not associated with a function; inserting a return requires validation of the function signature" );
            }

            var retType = InsertBlock.ContainingFunction.ReturnType;
            if(retType.IsVoid)
            {
                throw new ArgumentException( Resources.Return_instruction_for_void_function_must_not_have_a_value, nameof( value ) );
            }

            if(!retType.Equals( value.NativeType ))
            {
                // TODO: Update the resource string for more clarity
                // throw new ArgumentException( Resources.Value_for_return_must_match_the_function_signature_s_return_type, nameof( value ) );
                throw new ArgumentException( $"Types must match: retType: '{retType}'; value.NativeType: '{value.NativeType}';" );
            }

            var handle = LLVMBuildRet( Handle, value.Handle ).ThrowIfInvalid();
            return Value.FromHandle<ReturnInstruction>( handle )!;
        }

        /// <summary>Creates a call function</summary>
        /// <param name="func">Function to call</param>
        /// <param name="args">Arguments to pass to the function</param>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction Call( Function func, params Value[] args ) => Call( func, (IReadOnlyList<Value>)args );

        /// <summary>Creates a call function</summary>
        /// <param name="func">Function to call</param>
        /// <param name="args">Arguments to pass to the function</param>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction Call( Function func, IReadOnlyList<Value> args )
        {
            ArgumentNullException.ThrowIfNull( func );
            ArgumentNullException.ThrowIfNull( args );

            LLVMValueRef hCall = BuildCall( func, args ).ThrowIfInvalid();
            return Value.FromHandle<CallInstruction>( hCall )!;
        }

        /// <summary>Creates an <see cref="Instructions.Invoke"/> instruction</summary>
        /// <param name="func">Function to invoke</param>
        /// <param name="args">arguments to pass to the function</param>
        /// <param name="then">Successful continuation block</param>
        /// <param name="catchBlock">Exception handling block</param>
        /// <returns><see cref="Instructions.Invoke"/></returns>
        public Invoke Invoke( Function func, IReadOnlyList<Value> args, BasicBlock then, BasicBlock catchBlock )
        {
            ArgumentNullException.ThrowIfNull( func );
            ArgumentNullException.ThrowIfNull( args );
            ArgumentNullException.ThrowIfNull( then );
            ArgumentNullException.ThrowIfNull( catchBlock );

            ValidateCallArgs( func, args );
            ArgumentNullException.ThrowIfNull( then );
            ArgumentNullException.ThrowIfNull( catchBlock );

            LLVMValueRef[ ] llvmArgs = [ .. args.Select( v => v.Handle ) ];
            LLVMValueRef invoke = LLVMBuildInvoke2( Handle
                                                  , func.NativeType.GetTypeRef( ) // TODO: Is this legit with opaque pointers?
                                                  , func.Handle
                                                  , llvmArgs
                                                  , then.BlockHandle
                                                  , catchBlock.BlockHandle
                                                  , string.Empty
                                                  );

            return Value.FromHandle<Invoke>( invoke.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Instructions.LandingPad"/> instruction</summary>
        /// <param name="resultType">Result type for the pad</param>
        /// <returns><see cref="Instructions.LandingPad"/></returns>
        public LandingPad LandingPad( ITypeRef resultType )
        {
            LLVMValueRef landingPad = LLVMBuildLandingPad( Handle
                                                         , resultType.GetTypeRef( )
                                                         , LLVMValueRef.Zero // personality function no longer part of instruction
                                                         , 0
                                                         , string.Empty
                                                         );

            return Value.FromHandle<LandingPad>( landingPad.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Instructions.Freeze"/> instruction</summary>
        /// <param name="value">Value to freeze</param>
        /// <returns><see cref="Instructions.Freeze"/></returns>
        public Freeze Freeze( Value value )
        {
            ArgumentNullException.ThrowIfNull( value );
            LLVMValueRef inst = LLVMBuildFreeze( Handle, value.Handle, string.Empty);
            return Value.FromHandle<Freeze>( inst.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Instructions.ResumeInstruction"/></summary>
        /// <param name="exception">Exception value</param>
        /// <returns><see cref="Instructions.ResumeInstruction"/></returns>
        public ResumeInstruction Resume( Value exception )
        {
            ArgumentNullException.ThrowIfNull( exception );

            LLVMValueRef resume = LLVMBuildResume( Handle, exception.Handle );
            return Value.FromHandle<ResumeInstruction>( resume.ThrowIfInvalid() )!;
        }

        /// <summary>Builds an LLVM Store instruction</summary>
        /// <param name="value">Value to store in destination</param>
        /// <param name="destination">value for the destination</param>
        /// <returns><see cref="Instructions.Store"/> instruction</returns>
        /// <remarks>
        /// Since store targets memory the type of <paramref name="destination"/>
        /// must be an <see cref="IPointerType"/>. Furthermore, the element type of
        /// the pointer must match the type of <paramref name="value"/>. Otherwise,
        /// an <see cref="ArgumentException"/> is thrown.
        /// </remarks>
        public Store Store( Value value, Value destination )
        {
            ArgumentNullException.ThrowIfNull( value );
            ArgumentNullException.ThrowIfNull( destination );

            if(destination.NativeType is not IPointerType)
            {
                throw new ArgumentException( Resources.Expected_pointer_value, nameof( destination ) );
            }

            LLVMValueRef valueRef = LLVMBuildStore( Handle, value.Handle, destination.Handle );
            return Value.FromHandle<Store>( valueRef.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a load instruction</summary>
        /// <param name="type">Type of the value to load</param>
        /// <param name="sourcePtr">pointer to load the value from</param>
        /// <returns>Load instruction</returns>
        /// <remarks>
        /// The <paramref name="type"/> of the value must be a sized type (e.g. not Opaque with a non-zero size ).
        /// if <paramref name="sourcePtr"/> is a non-opaque pointer then its ElementType must be the same as <paramref name="type"/>
        /// </remarks>
        public Load Load( ITypeRef type, Value sourcePtr )
        {
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( sourcePtr );

            if(sourcePtr.NativeType.Kind != TypeKind.Pointer)
            {
                throw new ArgumentException( Resources.Expected_a_pointer_value, nameof( sourcePtr ) );
            }

            if(!type.IsSized)
            {
                throw new ArgumentException( Resources.Cannot_load_a_value_for_an_opaque_or_unsized_type, nameof( type ) );
            }

            var handle = LLVMBuildLoad2( Handle, type.GetTypeRef( ), sourcePtr.Handle, string.Empty );
            return Value.FromHandle<Load>( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates an atomic exchange (Read, Modify, Write) instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicXchg( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXchg, ptr, val );

        /// <summary>Creates an atomic add instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicAdd( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAdd, ptr, val );

        /// <summary>Creates an atomic subtraction instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicSub( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpSub, ptr, val );

        /// <summary>Creates an atomic AND instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicAnd( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAnd, ptr, val );

        /// <summary>Creates an atomic NAND instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicNand( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpNand, ptr, val );

        /// <summary>Creates an atomic or instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicOr( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpOr, ptr, val );

        /// <summary>Creates an atomic XOR instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicXor( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXor, ptr, val );

        /// <summary>Creates an atomic ADD instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicMax( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMax, ptr, val );

        /// <summary>Creates an atomic MIN instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicMin( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMin, ptr, val );

        /// <summary>Creates an atomic UMax instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicUMax( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMax, ptr, val );

        /// <summary>Creates an atomic UMin instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicUMin( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMin, ptr, val );

        /// <summary>Creates an atomic FAdd instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicFadd( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpFAdd, ptr, val );

        /// <summary>Creates an atomic FSub instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="val">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicRMW AtomicFSub( Value ptr, Value val ) => BuildAtomicRMW( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpFSub, ptr, val );

        /// <summary>Creates an atomic Compare exchange instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="cmp">Comparand for the operation</param>
        /// <param name="value">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicCmpXchg AtomicCmpXchg( Value ptr, Value cmp, Value value )
        {
            ArgumentNullException.ThrowIfNull( ptr );
            ArgumentNullException.ThrowIfNull( cmp );
            ArgumentNullException.ThrowIfNull( value );

            if(ptr.NativeType is not IPointerType)
            {
                throw new ArgumentException( Resources.Expected_pointer_value, nameof( ptr ) );
            }

            var handle = LLVMBuildAtomicCmpXchg( Handle
                                               , ptr.Handle
                                               , cmp.Handle
                                               , value.Handle
                                               , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                               , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                               , false
                                               );

            return Value.FromHandle<AtomicCmpXchg>( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element (field) of a structure</summary>
        /// <param name="type">Type of the pointer</param>
        /// <param name="pointer">OPaque pointer to the structure to get an element from</param>
        /// <param name="index">element index</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        public Value GetStructElementPointer( ITypeRef type, Value pointer, uint index )
        {
            ValidateStructGepArgs( pointer, index );

            // TODO: verify pointer is an opaque pointer or type == pointer.NativeTYpe
            var handle = LLVMBuildStructGEP2( Handle, type.GetTypeRef( ), pointer.Handle, index, string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="type">Type of array,vector or structure to get the element pointer from</param>
        /// <param name="pointer">opaque pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However, that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value GetElementPtr( ITypeRef type, Value pointer, IEnumerable<Value> args )
        {
            var llvmArgs = GetValidatedGEPArgs( type, pointer, args );
            var handle = LLVMBuildGEP2( Handle
                                      , type.GetTypeRef( )
                                      , pointer.Handle
                                      , llvmArgs
                                      , ( uint )llvmArgs.Length
                                      , string.Empty
                                      );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However, that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value GetElementPtr( Value pointer, IEnumerable<Value> args )
            => GetElementPtr( pointer.ThrowIfNull().NativeType, pointer, args );

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However, that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value GetElementPtr( Value pointer, params Value[] args ) => GetElementPtr( pointer, (IEnumerable<Value>)args );

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However, that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        [Obsolete( "Use overload that takes a pointer type and opaque pointer" )]
        public Value GetElementPtrInBounds( Value pointer, IEnumerable<Value> args )
        {
            return GetElementPtrInBounds( pointer.ThrowIfNull().NativeType, pointer, args );
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="type">Base pointer type</param>
        /// <param name="pointer">opaque pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However, that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value GetElementPtrInBounds( ITypeRef type, Value pointer, IEnumerable<Value> args )
        {
            var llvmArgs = GetValidatedGEPArgs( type, pointer, args );
            var hRetVal = LLVMBuildInBoundsGEP2( Handle
                                               , type.GetTypeRef( )
                                               , pointer.Handle
                                               , llvmArgs
                                               , ( uint )llvmArgs.Length
                                               , string.Empty
                                               );
            return Value.FromHandle( hRetVal.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        [Obsolete( "Use overload that accepts base pointer type and na opaque pointer" )]
        public Value GetElementPtrInBounds( Value pointer, params Value[] args )
        {
            return GetElementPtrInBounds( pointer, (IEnumerable<Value>)args );
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="type">Base pointer type</param>
        /// <param name="pointer">opaque pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value GetElementPtrInBounds( ITypeRef type, Value pointer, params Value[] args )
        {
            return GetElementPtrInBounds( type, pointer, (IEnumerable<Value>)args );
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indices for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a User as LLVM may
        /// optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see
        /// <see href="xref:llvm_misunderstood_gep">The Often Misunderstood GEP Instruction</see>.
        /// The basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However that is only syntactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index, even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public static Value ConstGetElementPtrInBounds( Value pointer, params Value[] args )
        {
            ArgumentNullException.ThrowIfNull( pointer );
            var llvmArgs = GetValidatedGEPArgs( pointer.NativeType, pointer, args );
            var handle = LLVMConstInBoundsGEP2(pointer.NativeType.GetTypeRef(), pointer.Handle, llvmArgs, ( uint )llvmArgs.Length );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a cast from an integer to a pointer</summary>
        /// <param name="intValue">Integer value to cast</param>
        /// <param name="ptrType">pointer type to return</param>
        /// <returns>Resulting value from the cast</returns>
        /// <remarks>
        /// The actual type of value returned depends on <paramref name="intValue"/>
        /// and is either a <see cref="ConstantExpression"/> or an <see cref="Instructions.IntToPointer"/>
        /// instruction. Conversion to a constant expression is performed whenever possible.
        /// </remarks>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public Value IntToPointer( Value intValue, IPointerType ptrType )
        {
            ArgumentNullException.ThrowIfNull( intValue );
            ArgumentNullException.ThrowIfNull( ptrType );

            var handle = (intValue is Constant)
                         ? LLVMConstIntToPtr( intValue.Handle, ptrType.GetTypeRef( ) )
                         : LLVMBuildIntToPtr( Handle, intValue.Handle, ptrType.GetTypeRef( ), string.Empty );

            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a cast from a pointer to an integer type</summary>
        /// <param name="ptrValue">Pointer value to cast</param>
        /// <param name="intType">Integer type to return</param>
        /// <returns>Resulting value from the cast</returns>
        /// <remarks>
        /// The actual type of value returned depends on <paramref name="ptrValue"/>
        /// and is either a <see cref="ConstantExpression"/> or a <see cref="Instructions.PointerToInt"/>
        /// instruction. Conversion to a constant expression is performed whenever possible.
        /// </remarks>
        public Value PointerToInt( Value ptrValue, ITypeRef intType )
        {
            ArgumentNullException.ThrowIfNull( ptrValue );
            ArgumentNullException.ThrowIfNull( intType );

            if(ptrValue.NativeType.Kind != TypeKind.Pointer)
            {
                throw new ArgumentException( Resources.Expected_a_pointer_value, nameof( ptrValue ) );
            }

            if(intType.Kind != TypeKind.Integer)
            {
                throw new ArgumentException( Resources.Expected_pointer_to_integral_type, nameof( intType ) );
            }

            var handle = ( ptrValue is Constant )
                         ? LLVMConstPtrToInt( ptrValue.Handle, intType.GetTypeRef( ) )
                         : LLVMBuildPtrToInt( Handle, ptrValue.Handle, intType.GetTypeRef( ), string.Empty );

            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Create an unconditional branch</summary>
        /// <param name="target">Target block for the branch</param>
        /// <returns><see cref="Instructions.Branch"/></returns>
        public Branch Branch( BasicBlock target )
        {
            LLVMValueRef valueRef = LLVMBuildBr( Handle, target.ThrowIfNull().BlockHandle );
            return Value.FromHandle<Branch>( valueRef.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a conditional branch instruction</summary>
        /// <param name="ifCondition">Condition for the branch</param>
        /// <param name="thenTarget">Target block for the branch when <paramref name="ifCondition"/> evaluates to a non-zero value</param>
        /// <param name="elseTarget">Target block for the branch when <paramref name="ifCondition"/> evaluates to a zero value</param>
        /// <returns><see cref="Instructions.Branch"/></returns>
        public Branch Branch( Value ifCondition, BasicBlock thenTarget, BasicBlock elseTarget )
        {
            ArgumentNullException.ThrowIfNull( ifCondition );
            ArgumentNullException.ThrowIfNull( thenTarget );
            ArgumentNullException.ThrowIfNull( elseTarget );

            var handle = LLVMBuildCondBr( Handle
                                        , ifCondition.Handle
                                        , thenTarget.BlockHandle
                                        , elseTarget.BlockHandle
                                        );

            return Value.FromHandle<Branch>( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates an <see cref="Instructions.Unreachable"/> instruction</summary>
        /// <returns><see cref="Instructions.Unreachable"/> </returns>
        public Unreachable Unreachable( )
            => Value.FromHandle<Unreachable>( LLVMBuildUnreachable( Handle ).ThrowIfInvalid() )!;

        /// <summary>Builds an Integer compare instruction</summary>
        /// <param name="predicate">Integer predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( IntPredicate predicate, Value lhs, Value rhs )
        {
            predicate.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            if(!lhs.NativeType.IsInteger && !lhs.NativeType.IsPointer)
            {
                throw new ArgumentException( Resources.Expecting_an_integer_or_pointer_type, nameof( lhs ) );
            }

            if(!rhs.NativeType.IsInteger && !lhs.NativeType.IsPointer)
            {
                throw new ArgumentException( Resources.Expecting_an_integer_or_pointer_type, nameof( rhs ) );
            }

            var handle = LLVMBuildICmp( Handle, ( LLVMIntPredicate )predicate, lhs.Handle, rhs.Handle, string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a Floating point compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( RealPredicate predicate, Value lhs, Value rhs )
        {
            predicate.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            if(!lhs.NativeType.IsFloatingPoint)
            {
                throw new ArgumentException( Resources.Expecting_an_integer_type, nameof( lhs ) );
            }

            if(!rhs.NativeType.IsFloatingPoint)
            {
                throw new ArgumentException( Resources.Expecting_an_integer_type, nameof( rhs ) );
            }

            var handle = LLVMBuildFCmp( Handle
                                      , ( LLVMRealPredicate )predicate
                                      , lhs.Handle
                                      , rhs.Handle
                                      , string.Empty
                                      );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is NOT simpler" )]
        public Value Compare( Predicate predicate, Value lhs, Value rhs )
        {
            if(predicate <= Predicate.LastFcmpPredicate)
            {
                return Compare( (RealPredicate)predicate, lhs, rhs );
            }

            if(predicate >= Predicate.FirstIcmpPredicate && predicate <= Predicate.LastIcmpPredicate)
            {
                return Compare( (IntPredicate)predicate, lhs, rhs );
            }

            throw new ArgumentOutOfRangeException(
                nameof( predicate ),
                string.Format( CultureInfo.CurrentCulture, Resources._0_is_not_a_valid_value_for_a_compare_predicate, predicate )
                );
        }

        /// <summary>Creates a zero extend or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value ZeroExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if(valueRef.NativeType.Equals( targetType ))
            {
                return valueRef;
            }

            LLVMValueRef handle = LLVMBuildZExtOrBitCast( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a sign extend or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SignExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if(valueRef.NativeType.Equals( targetType ))
            {
                return valueRef;
            }

            LLVMValueRef handle = LLVMBuildSExtOrBitCast( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a trunc or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value TruncOrBitCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if(valueRef.NativeType.Equals( targetType ))
            {
                return valueRef;
            }

            LLVMValueRef handle;
            handle = valueRef is Constant
                     ? LLVMConstTruncOrBitCast( valueRef.Handle, targetType.GetTypeRef() )
                     : LLVMBuildTruncOrBitCast( Handle, valueRef.Handle, targetType.GetTypeRef(), string.Empty );

            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a Zero Extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value ZeroExtend( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildZExt( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a Sign Extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SignExtend( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildSExt( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a bitcast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value BitCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if(valueRef.NativeType.Equals( targetType ))
            {
                return valueRef;
            }

            LLVMValueRef handle;
            handle = valueRef is Constant
                   ? LLVMConstBitCast( valueRef.Handle, targetType.GetTypeRef() )
                   : LLVMBuildBitCast( Handle, valueRef.Handle, targetType.GetTypeRef(), string.Empty );

            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates an integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <param name="isSigned">Flag to indicate if the cast is signed or unsigned</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value IntCast( Value valueRef, ITypeRef targetType, bool isSigned )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildIntCast2( Handle, valueRef.Handle, targetType.GetTypeRef( ), isSigned, string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a trunc instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value Trunc( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            var handle = valueRef is Constant
                       ? LLVMConstTrunc( valueRef.Handle, targetType.GetTypeRef( ) )
                       : LLVMBuildTrunc( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );

            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a signed integer to floating point cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SIToFPCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildSIToFP( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates an unsigned integer to floating point cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value UIToFPCast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildUIToFP( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a Floating point to unsigned integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPToUICast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildFPToUI( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a floating point to signed integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPToSICast( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildFPToSI( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a floating point extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPExt( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildFPExt( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a floating point truncate instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPTrunc( Value valueRef, ITypeRef targetType )
        {
            ArgumentNullException.ThrowIfNull( valueRef );
            ArgumentNullException.ThrowIfNull( targetType );

            LLVMValueRef handle = LLVMBuildFPTrunc( Handle, valueRef.Handle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a <see cref="Ubiquity.NET.Llvm.Instructions.SelectInstruction"/> instruction</summary>
        /// <param name="ifCondition">Value for the condition to select between the values</param>
        /// <param name="thenValue">Result value if <paramref name="ifCondition"/> evaluates to 1</param>
        /// <param name="elseValue">Result value if <paramref name="ifCondition"/> evaluates to 0</param>
        /// <returns>Selected value</returns>
        /// <remarks>
        /// If <paramref name="ifCondition"/> is a vector then both values must be a vector of the same
        /// size and the selection is performed element by element. The values must be the same type.
        /// </remarks>
        public Value Select( Value ifCondition, Value thenValue, Value elseValue )
        {
            ArgumentNullException.ThrowIfNull( ifCondition );
            ArgumentNullException.ThrowIfNull( thenValue );
            ArgumentNullException.ThrowIfNull( elseValue );

            var conditionVectorType = ifCondition.NativeType as IVectorType;

            if(ifCondition.NativeType.IntegerBitWidth != 1 && conditionVectorType != null && conditionVectorType.ElementType.IntegerBitWidth != 1)
            {
                throw new ArgumentException( Resources.Condition_value_must_be_an_i1_or_vector_of_i1, nameof( ifCondition ) );
            }

            if(conditionVectorType != null)
            {
                if(thenValue.NativeType is not IVectorType thenVector || thenVector.Size != conditionVectorType.Size)
                {
                    throw new ArgumentException( Resources.When_condition_is_a_vector__selected_values_must_be_a_vector_of_the_same_size, nameof( thenValue ) );
                }

                if(elseValue.NativeType is not IVectorType elseVector || elseVector.Size != conditionVectorType.Size)
                {
                    throw new ArgumentException( Resources.When_condition_is_a_vector__selected_values_must_be_a_vector_of_the_same_size, nameof( elseValue ) );
                }
            }
            else
            {
                if(!elseValue.NativeType.Equals( thenValue.NativeType ))
                {
                    throw new ArgumentException( Resources.Selected_values_must_have_the_same_type );
                }
            }

            var handle = LLVMBuildSelect( Handle
                                        , ifCondition.Handle
                                        , thenValue.Handle
                                        , elseValue.Handle
                                        , string.Empty
                                        );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a Phi instruction</summary>
        /// <param name="resultType">Result type for the instruction</param>
        /// <returns><see cref="Instructions.PhiNode"/></returns>
        public PhiNode PhiNode( ITypeRef resultType )
        {
            var handle = LLVMBuildPhi( Handle, resultType.GetTypeRef( ), string.Empty );
            return Value.FromHandle<PhiNode>( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates an extractvalue instruction</summary>
        /// <param name="instance">Instance to extract a value from</param>
        /// <param name="index">index of the element to extract</param>
        /// <returns>Value for the instruction</returns>
        public Value ExtractValue( Value instance, uint index )
        {
            ArgumentNullException.ThrowIfNull( instance );

            var handle = LLVMBuildExtractValue( Handle, instance.Handle, index, string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a switch instruction</summary>
        /// <param name="value">Value to switch on</param>
        /// <param name="defaultCase">default case if <paramref name="value"/> does match any case</param>
        /// <param name="numCases">Number of cases for the switch</param>
        /// <returns><see cref="Instructions.Switch"/></returns>
        /// <remarks>
        /// Callers can use <see cref="Instructions.Switch.AddCase(Value, BasicBlock)"/> to add cases to the
        /// instruction.
        /// </remarks>
        public Switch Switch( Value value, BasicBlock defaultCase, uint numCases )
        {
            ArgumentNullException.ThrowIfNull( value );
            ArgumentNullException.ThrowIfNull( defaultCase );

            var handle = LLVMBuildSwitch( Handle, value.Handle, defaultCase.BlockHandle, numCases );
            return Value.FromHandle<Switch>( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a call to the llvm.donothing intrinsic</summary>
        /// <returns><see cref="CallInstruction"/></returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="InsertBlock"/> is <see langword="null"/> or it's <see cref="BasicBlock.ContainingFunction"/> is null or has a <see langword="null"/> <see cref="GlobalValue.ParentModule"/>
        /// </exception>
        public CallInstruction DoNothing( )
        {
            IModule module = GetModuleOrThrow( );
            var func = module.GetIntrinsicDeclaration( "llvm.donothing" );
            var hCall = BuildCall( func );
            return Value.FromHandle<CallInstruction>( hCall.ThrowIfInvalid() )!;
        }

        /// <summary>Creates a llvm.debugtrap call</summary>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction DebugTrap( )
        {
            var module = GetModuleOrThrow( );
            var func = module.GetIntrinsicDeclaration( "llvm.debugtrap" );

            return Call( func );
        }

        /// <summary>Creates a llvm.trap call</summary>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction Trap( )
        {
            var module = GetModuleOrThrow( );
            var func = module.GetIntrinsicDeclaration( "llvm.trap" );

            return Call( func );
        }

        /// <summary>Builds a memcpy intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memcpy</param>
        /// <param name="source">Source pointer of the memcpy</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memcpy</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memcpy intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemCpy( Value destination, Value source, Value len, bool isVolatile )
        {
            ArgumentNullException.ThrowIfNull( destination );
            ArgumentNullException.ThrowIfNull( source );
            ArgumentNullException.ThrowIfNull( len );
            var module = GetModuleOrThrow( );

            if(destination.Equals( source ))
            {
                throw new InvalidOperationException( Resources.Source_and_destination_arguments_are_the_same_value );
            }

            if(destination.NativeType is not IPointerType dstPtrType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( destination ) );
            }

            if(source.NativeType is not IPointerType srcPtrType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( source ) );
            }

            if(!len.NativeType.IsInteger)
            {
                throw new ArgumentException( Resources.Integer_type_expected, nameof( len ) );
            }

            if(!Context.Equals( module.Context ))
            {
                throw new ArgumentException( Resources.Module_and_instruction_builder_must_come_from_the_same_context );
            }

            // TODO: Replace with call to LLVMBuildMemCpy

            // find the name of the appropriate overloaded form
            var func = module.GetIntrinsicDeclaration( "llvm.memcpy.p.p.i", dstPtrType, srcPtrType, len.NativeType );

            var call = BuildCall( func
                                , destination
                                , source
                                , len
                                , module.Context.CreateConstant( isVolatile )
                                );
            return Value.FromHandle( call.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a memmove intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memmove</param>
        /// <param name="source">Source pointer of the memmove</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memmove</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memmove intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemMove( Value destination, Value source, Value len, bool isVolatile )
        {
            ArgumentNullException.ThrowIfNull( destination );
            ArgumentNullException.ThrowIfNull( source );
            ArgumentNullException.ThrowIfNull( len );
            var module = GetModuleOrThrow( );

            if(destination.Equals( source ))
            {
                throw new InvalidOperationException( Resources.Source_and_destination_arguments_are_the_same_value );
            }

            if(destination.NativeType is not IPointerType dstPtrType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( destination ) );
            }

            if(source.NativeType is not IPointerType srcPtrType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( source ) );
            }

            if(!len.NativeType.IsInteger)
            {
                throw new ArgumentException( Resources.Integer_type_expected, nameof( len ) );
            }

            if(!Context.Equals( module.Context ))
            {
                throw new ArgumentException( Resources.Module_and_instruction_builder_must_come_from_the_same_context );
            }

            // TODO: Replace with call to LLVMBuildMemMove

            // find the name of the appropriate overloaded form
            var func = module.GetIntrinsicDeclaration( "llvm.memmove.p.p.i", dstPtrType, srcPtrType, len.NativeType );

            var call = BuildCall( func, destination, source, len, module.Context.CreateConstant( isVolatile ) );
            return Value.FromHandle( call.ThrowIfInvalid() )!;
        }

        /// <summary>Builds a memset intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memset</param>
        /// <param name="value">fill value for the memset</param>
        /// <param name="len">length of the data to fill</param>
        /// <param name="isVolatile">Flag to indicate if the fill involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memset</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memset intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemSet( Value destination, Value value, Value len, bool isVolatile )
        {
            ArgumentNullException.ThrowIfNull( destination );
            ArgumentNullException.ThrowIfNull( value );
            ArgumentNullException.ThrowIfNull( len );
            var module = GetModuleOrThrow( );

            if(destination.NativeType is not IPointerType dstPtrType)
            {
                throw new ArgumentException( Resources.Pointer_type_expected, nameof( destination ) );
            }

            if(!value.NativeType.IsInteger)
            {
                throw new ArgumentException( Resources.Integer_type_expected, nameof( value ) );
            }

            if(!len.NativeType.IsInteger)
            {
                throw new ArgumentException( Resources.Integer_type_expected, nameof( len ) );
            }

            if(!Context.Equals( module.Context ))
            {
                throw new ArgumentException( Resources.Module_and_instruction_builder_must_come_from_the_same_context );
            }

            // TODO: Replace with call to LLVMBuildMemSet

            // find the appropriate overloaded form of the function
            var func = module.GetIntrinsicDeclaration( "llvm.memset.p.i", dstPtrType, value.NativeType );

            var call = BuildCall( func
                                , destination
                                , value
                                , len
                                , module.Context.CreateConstant( isVolatile )
                                );

            return Value.FromHandle( call.ThrowIfInvalid() )!;
        }

        /// <summary>Inserts a call to malloc</summary>
        /// <param name="itemType">Type to allocate space for</param>
        /// <returns>Value (virtual register) for the result of a call to malloc</returns>
        /// <exception cref="ArgumentException"><paramref name="itemType"/> is not sized</exception>
        public Value Malloc( ITypeRef itemType )
        {
            return itemType.IsSized
                ? Value.FromHandle( LLVMBuildMalloc( Handle, itemType.GetTypeRef(), string.Empty ) ).ThrowIfNull()
                : throw new ArgumentException( Resources.Type_must_be_sized_to_get_target_size_information );
        }

        /// <summary>Builds an <see cref="Ubiquity.NET.Llvm.Instructions.InsertValue"/> instruction </summary>
        /// <param name="aggValue">Aggregate value to insert <paramref name="elementValue"/> into</param>
        /// <param name="elementValue">Value to insert into <paramref name="aggValue"/></param>
        /// <param name="index">Index to insert the value into</param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value InsertValue( Value aggValue, Value elementValue, uint index )
        {
            ArgumentNullException.ThrowIfNull( aggValue );
            ArgumentNullException.ThrowIfNull( elementValue );

            var handle = LLVMBuildInsertValue( Handle, aggValue.Handle, elementValue.Handle, index, string.Empty );
            return Value.FromHandle( handle.ThrowIfInvalid() )!;
        }

        /// <summary>Generates a call to the llvm.[s|u]add.with.overflow intrinsic</summary>
        /// <param name="lhs">Left hand side of the operation</param>
        /// <param name="rhs">Right hand side of the operation</param>
        /// <param name="signed">Flag to indicate if the operation is signed <see langword="true"/> or unsigned <see langword="false"/></param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value AddWithOverflow( Value lhs, Value rhs, bool signed )
        {
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            char kind = signed ? 's' : 'u';
            string name = $"llvm.{kind}add.with.overflow.i";
            var module = GetModuleOrThrow( );

            var function = module.GetIntrinsicDeclaration( name, lhs.NativeType );
            return Call( function, lhs, rhs );
        }

        /// <summary>Generates a call to the llvm.[s|u]sub.with.overflow intrinsic</summary>
        /// <param name="lhs">Left hand side of the operation</param>
        /// <param name="rhs">Right hand side of the operation</param>
        /// <param name="signed">Flag to indicate if the operation is signed <see langword="true"/> or unsigned <see langword="false"/></param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value SubWithOverflow( Value lhs, Value rhs, bool signed )
        {
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            char kind = signed ? 's' : 'u';
            string name = $"llvm.{kind}sub.with.overflow.i";
            uint id = Intrinsic.LookupId( name );
            var module = GetModuleOrThrow( );

            var function = module.GetIntrinsicDeclaration( id, lhs.NativeType );
            return Call( function, lhs, rhs );
        }

        /// <summary>Generates a call to the llvm.[s|u]mul.with.overflow intrinsic</summary>
        /// <param name="lhs">Left hand side of the operation</param>
        /// <param name="rhs">Right hand side of the operation</param>
        /// <param name="signed">Flag to indicate if the operation is signed <see langword="true"/> or unsigned <see langword="false"/></param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value MulWithOverflow( Value lhs, Value rhs, bool signed )
        {
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            char kind = signed ? 's' : 'u';
            string name = $"llvm.{kind}mul.with.overflow.i";
            uint id = Intrinsic.LookupId( name );
            var module = GetModuleOrThrow( );

            var function = module.GetIntrinsicDeclaration( id, lhs.NativeType );
            return Call( function, lhs, rhs );
        }

        internal static LLVMValueRef[] GetValidatedGEPArgs( ITypeRef type, Value pointer, IEnumerable<Value> args )
        {
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( pointer );
            ArgumentNullException.ThrowIfNull( args );

            if(pointer.NativeType is not IPointerType pointerType)
            {
                throw new ArgumentException( Resources.Pointer_value_expected, nameof( pointer ) );
            }

            // start with the base pointer as type for first index
            ITypeRef elementType = type.CreatePointerType();
            foreach(var index in args)
            {
                switch(elementType)
                {
                case ISequenceType s:
                    elementType = s.ElementType;
                    break;

                case IStructType st:
                    if(index is not ConstantInt constIndex)
                    {
                        throw new ArgumentException( "GEP index into a structure type must be constant" );
                    }

                    long indexValue = constIndex.SignExtendedValue;
                    if(indexValue >= st.Members.Count || indexValue < 0)
                    {
                        throw new ArgumentException( $"GEP index {indexValue} is out of range for {st.Name}" );
                    }

                    elementType = st.Members[ (int)constIndex.SignExtendedValue ];
                    break;

                default:
                    throw new ArgumentException( $"GEP index through a non-aggregate type {elementType}" );
                }
            }

            // if not an array already, pull from source enumerable into an array only once
            var argsArray = args as Value[ ] ?? [ .. args ];
            if(argsArray.Any( a => !a.NativeType.IsInteger ))
            {
                throw new ArgumentException( Resources.GEP_index_arguments_must_be_integers );
            }

            LLVMValueRef[ ] llvmArgs = [ .. argsArray.Select( a => a.Handle ) ];
            return llvmArgs.Length == 0
                ? throw new ArgumentException( Resources.There_must_be_at_least_one_index_argument, nameof( args ) )
                : llvmArgs;
        }

        // TODO: Either validate parameter 'index' or remove it...
        private static void ValidateStructGepArgs( Value pointer, uint index )
        {
            ArgumentNullException.ThrowIfNull( pointer );

            if(pointer.NativeType is not IPointerType)
            {
                throw new ArgumentException( Resources.Pointer_value_expected, nameof( pointer ) );
            }
        }

        private IModule GetModuleOrThrow( )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            return module ?? throw new InvalidOperationException( Resources.Cannot_insert_when_no_block_module_is_available );
        }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a unary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildUnaryOp( Func<LLVMBuilderRef, LLVMValueRef, LazyEncodedString, LLVMValueRef> opFactory
                                  , Value operand
                                  )
        {
            ArgumentNullException.ThrowIfNull( operand );
            var valueRef = opFactory( Handle, operand.Handle, LazyEncodedString.Empty );
            return Value.FromHandle( valueRef.ThrowIfInvalid() )!;
        }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a binary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildBinOp( Func<LLVMBuilderRef, LLVMValueRef, LLVMValueRef, LazyEncodedString, LLVMValueRef> opFactory
                                , Value lhs
                                , Value rhs
                                )
        {
            ArgumentNullException.ThrowIfNull( lhs );
            ArgumentNullException.ThrowIfNull( rhs );

            if(!lhs.NativeType.Equals( rhs.NativeType ))
            {
                throw new ArgumentException( Resources.Types_of_binary_operators_must_be_identical );
            }

            var valueRef = opFactory( Handle, lhs.Handle, rhs.Handle, LazyEncodedString.Empty );
            return Value.FromHandle( valueRef.ThrowIfInvalid() )!;
        }

        private AtomicRMW BuildAtomicRMW( LLVMAtomicRMWBinOp op, Value ptr, Value val )
        {
            ArgumentNullException.ThrowIfNull( ptr );
            ArgumentNullException.ThrowIfNull( val );

            if(ptr.NativeType is not IPointerType)
            {
                throw new ArgumentException( Resources.Expected_pointer_type, nameof( ptr ) );
            }

            var handle = LLVMBuildAtomicRMW( Handle, op, ptr.Handle, val.Handle, LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent, false );
            return Value.FromHandle<AtomicRMW>( handle.ThrowIfInvalid() )!;
        }

        private static IFunctionType ValidateCallArgs( Function func, IReadOnlyList<Value> args )
        {
            IFunctionType signatureType = func.Signature;

            // validate arg count; too few or too many (unless the signature supports varargs) is an error
            if(args.Count < signatureType.ParameterTypes.Count
                || (args.Count > signatureType.ParameterTypes.Count && !signatureType.IsVarArg)
              )
            {
                throw new ArgumentException( Resources.Mismatched_parameter_count_with_call_site, nameof( args ) );
            }

            for(int i = 0; i < signatureType.ParameterTypes.Count; ++i)
            {
                if(!args[ i ].NativeType.Equals( signatureType.ParameterTypes[ i ] ))
                {
                    string msg = string.Format( CultureInfo.CurrentCulture, Resources.Call_site_argument_type_mismatch_for_function_0_at_index_1_argType_equals_2_signatureType_equals_3, func, i, args[ i ].NativeType, signatureType.ParameterTypes[ i ] );
                    throw new ArgumentException( msg, nameof( args ) );
                }
            }

            return signatureType;
        }

        private LLVMValueRef BuildCall( Function func, params Value[] args ) => BuildCall( func, (IReadOnlyList<Value>)args );

        private LLVMValueRef BuildCall( Function func ) => BuildCall( func, new List<Value>() );

        private LLVMValueRef BuildCall( Function func, IReadOnlyList<Value> args )
        {
            IFunctionType sig = ValidateCallArgs( func, args );
            LLVMValueRef[ ] llvmArgs = [ .. args.Select( v => v.Handle ) ];
            return LLVMBuildCall2( Handle, sig.GetTypeRef(), func.Handle, llvmArgs, (uint)llvmArgs.Length, LazyEncodedString.Empty );
        }

        private LLVMBuilderRef Handle { get; }
    }
}

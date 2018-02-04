// <copyright file="InstructionBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Llvm.NET.DebugInfo;
using Llvm.NET.Native;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>LLVM Instruction builder allowing managed code to generate IR instructions</summary>
    public sealed class InstructionBuilder
    {
        /// <summary>Initializes a new instance of the <see cref="InstructionBuilder"/> class for a given <see cref="Llvm.NET.Context"/></summary>
        /// <param name="context">Context used for creating instructions</param>
        public InstructionBuilder( Context context )
        {
            Context = context ?? throw new ArgumentNullException( nameof( context ) );
            BuilderHandle = LLVMCreateBuilderInContext( context.ContextHandle );
        }

        /// <summary>Initializes a new instance of the <see cref="InstructionBuilder"/> class for a <see cref="BasicBlock"/></summary>
        /// <param name="block">Block this builder is initially attached to</param>
        public InstructionBuilder( BasicBlock block )
            : this( block.ValidateNotNull( nameof( block ) ).ContainingFunction.ParentModule.Context )
        {
            PositionAtEnd( block );
        }

        /// <summary>Gets the context this builder is creating instructions for</summary>
        public Context Context { get; }

        /// <summary>Set the current debug location for this <see cref="InstructionBuilder"/></summary>
        /// <param name="line">Source line</param>
        /// <param name="col">Source column</param>
        /// <param name="scope"><see cref="DIScope"/> for the location</param>
        /// <param name="inlinedAt"><see cref="DIScope"/>the location is inlined into</param>
        public void SetDebugLocation( uint line, uint col, DIScope scope = null, DIScope inlinedAt = null )
        {
            LLVMSetCurrentDebugLocation2( BuilderHandle, line, col, scope?.MetadataHandle ?? default, inlinedAt?.MetadataHandle ?? default );
        }

        /// <summary>Gets the <see cref="BasicBlock"/> this builder is building instructions for</summary>
        public BasicBlock InsertBlock
        {
            get
            {
                var handle = LLVMGetInsertBlock( BuilderHandle );
                if( handle == default )
                {
                    return null;
                }

                return BasicBlock.FromHandle( LLVMGetInsertBlock( BuilderHandle ) );
            }
        }

        /// <summary>Positions the builder at the end of a given <see cref="BasicBlock"/></summary>
        /// <param name="basicBlock">Block to set the position of</param>
        public void PositionAtEnd( BasicBlock basicBlock )
        {
            if( basicBlock == null )
            {
                throw new ArgumentNullException( nameof( basicBlock ) );
            }

            LLVMPositionBuilderAtEnd( BuilderHandle, basicBlock.BlockHandle );
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
            if( instr == null )
            {
                throw new ArgumentNullException( nameof( instr ) );
            }

            LLVMPositionBuilderBefore( BuilderHandle, instr.ValueHandle );
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
        public Value SDiv( Value lhs, Value rhs ) => BuildBinOp( LLVMBuildUDiv, lhs, rhs );

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
            var handle = LLVMBuildAlloca( BuilderHandle, typeRef.GetTypeRef( ), string.Empty );
            return Value.FromHandle<Alloca>( handle );
        }

        /// <summary>Creates an alloca instruction</summary>
        /// <param name="typeRef">Type of the value to allocate</param>
        /// <param name="elements">Number of elements to allocate</param>
        /// <returns><see cref="Instructions.Alloca"/> instruction</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public Alloca Alloca( ITypeRef typeRef, ConstantInt elements )
        {
            if( typeRef == null )
            {
                throw new ArgumentNullException( nameof( typeRef ) );
            }

            if( elements == null )
            {
                throw new ArgumentNullException( nameof( elements ) );
            }

            var instHandle = LLVMBuildArrayAlloca( BuilderHandle, typeRef.GetTypeRef( ), elements.ValueHandle, string.Empty );
            return Value.FromHandle<Alloca>( instHandle );
        }

        /// <summary>Creates a return instruction for a function that has no return value</summary>
        /// <returns><see cref="ReturnInstruction"/></returns>
        /// <exception cref="ArgumentException"> the function has a non-void return type</exception>
        public ReturnInstruction Return( )
        {
            if( !InsertBlock.ContainingFunction.ReturnType.IsVoid )
            {
                throw new ArgumentException( "Return instruction for non-void function must have a value" );
            }

            return Value.FromHandle<ReturnInstruction>( LLVMBuildRetVoid( BuilderHandle ) );
        }

        /// <summary>Creates a return instruction with the return value for a function</summary>
        /// <param name="value"><see cref="Value"/> to return</param>
        /// <returns><see cref="ReturnInstruction"/></returns>
        public ReturnInstruction Return( Value value )
        {
            value.ValidateNotNull( nameof( value ) );

            var retType = InsertBlock.ContainingFunction.ReturnType;
            if( retType.IsVoid )
            {
                throw new ArgumentException( "Return instruction for void function must not have a value", nameof( value ) );
            }

            if( retType != value.NativeType )
            {
                throw new ArgumentException( "Value for return must match the function signature's return type", nameof( value ) );
            }

            var handle = LLVMBuildRet( BuilderHandle, value.ValueHandle );
            return Value.FromHandle<ReturnInstruction>( handle );
        }

        /// <summary>Creates a call function</summary>
        /// <param name="func">Function to call</param>
        /// <param name="args">Arguments to pass to the function</param>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction Call( Value func, params Value[ ] args ) => Call( func, ( IReadOnlyList<Value> )args );

        /// <summary>Creates a call function</summary>
        /// <param name="func">Function to call</param>
        /// <param name="args">Arguments to pass to the function</param>
        /// <returns><see cref="CallInstruction"/></returns>
        public CallInstruction Call( Value func, IReadOnlyList<Value> args )
        {
            LLVMValueRef hCall = BuildCall( func, args );
            var retVal = Value.FromHandle<CallInstruction>( hCall );
            return retVal;
        }

        /// <summary>Creates an <see cref="Instructions.Invoke"/> instruction</summary>
        /// <param name="func">Function to invoke</param>
        /// <param name="args">arguments to pass to the function</param>
        /// <param name="then">Successful continuation block</param>
        /// <param name="catchBlock">Exception handling block</param>
        /// <returns><see cref="Instructions.Invoke"/></returns>
        public Invoke Invoke( Value func, IReadOnlyList<Value> args, BasicBlock then, BasicBlock catchBlock )
        {
            ValidateCallArgs( func, args );
            then.ValidateNotNull( nameof( then ) );
            catchBlock.ValidateNotNull( nameof( then ) );

            LLVMValueRef[] llvmArgs = args.Select( v => v.ValueHandle ).ToArray();
            int argCount = llvmArgs.Length;

            // Must always provide at least one element for successful marshaling/interop, but tell LLVM there are none.
            if( argCount == 0 )
            {
                llvmArgs = new LLVMValueRef[1];
            }

            LLVMValueRef invoke = LLVMBuildInvoke( BuilderHandle
                                                 , func.ValueHandle
                                                 , out llvmArgs[0]
                                                 , (uint)argCount
                                                 , then.BlockHandle
                                                 , catchBlock.BlockHandle
                                                 , string.Empty
                                                 );

            return Value.FromHandle<Invoke>( invoke );
        }

        /// <summary>Creates a <see cref="Instructions.LandingPad"/> instruction</summary>
        /// <param name="resultType">Result type for the pad</param>
        /// <returns><see cref="Instructions.LandingPad"/></returns>
        public LandingPad LandingPad( ITypeRef resultType )
        {
            LLVMValueRef landingPad = LLVMBuildLandingPad( BuilderHandle
                                                         , resultType.GetTypeRef()
                                                         , new LLVMValueRef( IntPtr.Zero ) // personality function no longer part of instruction
                                                         , 0
                                                         , string.Empty
                                                         );

            return Value.FromHandle<LandingPad>( landingPad );
        }

        /// <summary>Creates a <see cref="Instructions.ResumeInstruction"/></summary>
        /// <param name="exception">Exception value</param>
        /// <returns><see cref="Instructions.ResumeInstruction"/></returns>
        public ResumeInstruction Resume( Value exception )
        {
            exception.ValidateNotNull( nameof( exception ) );

            LLVMValueRef resume = LLVMBuildResume( BuilderHandle, exception.ValueHandle );
            return Value.FromHandle<ResumeInstruction>( resume );
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
            value.ValidateNotNull( nameof( value ) );
            destination.ValidateNotNull( nameof( destination ) );

            var ptrType = destination.NativeType as IPointerType;
            if( ptrType == null )
            {
                throw new ArgumentException( "Expected pointer value", nameof( destination ) );
            }

            if( !ptrType.ElementType.Equals( value.NativeType )
             || ( value.NativeType.Kind == TypeKind.Integer && value.NativeType.IntegerBitWidth != ptrType.ElementType.IntegerBitWidth )
              )
            {
                throw new ArgumentException( string.Format( IncompatibleTypeMsgFmt, ptrType.ElementType, value.NativeType ) );
            }

            return Value.FromHandle<Store>( LLVMBuildStore( BuilderHandle, value.ValueHandle, destination.ValueHandle ) );
        }

        /// <summary>Creates a <see cref="Instructions.Load"/> instruction</summary>
        /// <param name="sourcePtr">Pointer to the value to load</param>
        /// <returns><see cref="Instructions.Load"/></returns>
        public Load Load( Value sourcePtr )
        {
            sourcePtr.ValidateNotNull( nameof( sourcePtr ) );

            var handle = LLVMBuildLoad( BuilderHandle, sourcePtr.ValueHandle, string.Empty );
            return Value.FromHandle<Load>( handle );
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

        /// <summary>Creates an atomic Compare exchange instruction</summary>
        /// <param name="ptr">Pointer to the value to update (e.g. destination and the left hand operand)</param>
        /// <param name="cmp">Comparand for the operation</param>
        /// <param name="value">Right hand side operand</param>
        /// <returns><see cref="AtomicRMW"/></returns>
        public AtomicCmpXchg AtomicCmpXchg( Value ptr, Value cmp, Value value )
        {
            ptr.ValidateNotNull( nameof( ptr ) );
            cmp.ValidateNotNull( nameof( cmp ) );
            value.ValidateNotNull( nameof( value ) );

            var ptrType = ptr.NativeType as IPointerType;
            if( ptrType == null )
            {
                throw new ArgumentException( "Expected pointer value", nameof( ptr ) );
            }

            if( ptrType.ElementType != cmp.NativeType )
            {
                throw new ArgumentException( string.Format( IncompatibleTypeMsgFmt, ptrType.ElementType, cmp.NativeType ) );
            }

            if( ptrType.ElementType != value.NativeType )
            {
                throw new ArgumentException( string.Format( IncompatibleTypeMsgFmt, ptrType.ElementType, value.NativeType ) );
            }

            var handle = LLVMBuildAtomicCmpXchg( BuilderHandle
                                               , ptr.ValueHandle
                                               , cmp.ValueHandle
                                               , value.ValueHandle
                                               , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                               , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                               , false
                                               );
            return Value.FromHandle< AtomicCmpXchg>( handle );
        }

        /// <summary>Creates a <see cref="Value"/> that accesses an element (field) of a structure</summary>
        /// <param name="pointer">pointer to the structure to get an element from</param>
        /// <param name="index">element index</param>
        /// <returns>
        /// <para><see cref="Value"/> for the member access. This is a <see cref="Value"/>
        /// as LLVM may optimize the expression to a <see cref="ConstantExpression"/> if it
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an exception is thrown.</para>
        /// </returns>
        public Value GetStructElementPointer( Value pointer, uint index )
        {
            pointer.ValidateNotNull( nameof( pointer ) );

            var ptrType = pointer.NativeType as IPointerType;
            if( ptrType == null )
            {
                throw new ArgumentException( "Pointer value expected", nameof( pointer ) );
            }

            var elementStructType = ptrType.ElementType as IStructType;
            if( elementStructType == null )
            {
                throw new ArgumentException( "Pointer to a structure expected", nameof( pointer ) );
            }

            if( !elementStructType.IsSized && index > 0 )
            {
                throw new ArgumentException( "Cannot get element of unsized/opaque structures" );
            }

            if( index >= elementStructType.Members.Count )
            {
                throw new ArgumentException( "Index exceeds number of members in the type", nameof( index ) );
            }

            var handle = LLVMBuildStructGEP( BuilderHandle, pointer.ValueHandle, index, string.Empty );
            return Value.FromHandle( handle );
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
        /// former makes the first index explicit. In order to properly compute the offset for a given
        /// element in an aggregate type LLVM requires an explicit first index even if it is zero.
        /// </remarks>
        public Value GetElementPtr( Value pointer, IEnumerable<Value> args )
        {
            var llvmArgs = GetValidatedGEPArgs( pointer, args );
            var handle = LLVMBuildGEP( BuilderHandle
                                     , pointer.ValueHandle
                                     , out llvmArgs[ 0 ]
                                     , ( uint )llvmArgs.Length
                                     , string.Empty
                                     );
            return Value.FromHandle( handle );
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
        /// former makes the first index explicit. In order to properly compute the offset for a given
        /// element in an aggregate type LLVM requires an explicit first index even if it is zero.
        /// </remarks>
        public Value GetElementPtrInBounds( Value pointer, params Value[ ] args ) => GetElementPtrInBounds( pointer, ( IEnumerable<Value> )args );

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
        /// former makes the first index explicit. In order to properly compute the offset for a given
        /// element in an aggregate type LLVM requires an explicit first index even if it is zero.
        /// </remarks>
        public Value GetElementPtrInBounds( Value pointer, IEnumerable<Value> args )
        {
            var llvmArgs = GetValidatedGEPArgs( pointer, args );
            var hRetVal = LLVMBuildInBoundsGEP( BuilderHandle
                                              , pointer.ValueHandle
                                              , out llvmArgs[ 0 ]
                                              , ( uint )llvmArgs.Length
                                              , string.Empty
                                              );
            return Value.FromHandle( hRetVal );
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
        /// former makes the first index explicit. LLVM requires an explicit first index even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public static Value ConstGetElementPtrInBounds( Value pointer, params Value[ ] args )
        {
            var llvmArgs = GetValidatedGEPArgs( pointer, args );
            var handle = LLVMConstInBoundsGEP( pointer.ValueHandle, out llvmArgs[ 0 ], ( uint )llvmArgs.Length );
            return Value.FromHandle( handle );
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
            intValue.ValidateNotNull( nameof( intValue ) );
            ptrType.ValidateNotNull( nameof( ptrType ) );

            if( intValue is Constant )
            {
                return Value.FromHandle( LLVMConstIntToPtr( intValue.ValueHandle, ptrType.GetTypeRef( ) ) );
            }

            var handle = LLVMBuildIntToPtr( BuilderHandle, intValue.ValueHandle, ptrType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle );
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
            ptrValue.ValidateNotNull( nameof( ptrValue ) );
            intType.ValidateNotNull( nameof( intType ) );

            if( ptrValue.NativeType.Kind != TypeKind.Pointer )
            {
                throw new ArgumentException( "Expected a pointer value", nameof( ptrValue ) );
            }

            if( intType.Kind != TypeKind.Integer )
            {
                throw new ArgumentException( "Expected pointer to integral type", nameof( intType ) );
            }

            if( ptrValue is Constant )
            {
                return Value.FromHandle( LLVMConstPtrToInt( ptrValue.ValueHandle, intType.GetTypeRef( ) ) );
            }

            var handle = LLVMBuildPtrToInt( BuilderHandle, ptrValue.ValueHandle, intType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle );
        }

        /// <summary>Create an unconditional branch</summary>
        /// <param name="target">Target block for the branch</param>
        /// <returns><see cref="Instructions.Branch"/></returns>
        public Branch Branch( BasicBlock target )
            => Value.FromHandle<Branch>( LLVMBuildBr( BuilderHandle, target.ValidateNotNull( nameof( target ) ).BlockHandle ) );

        /// <summary>Creates a conditional branch instruction</summary>
        /// <param name="ifCondition">Condition for the branch</param>
        /// <param name="thenTarget">Target block for the branch when <paramref name="ifCondition"/> evaluates to a non-zero value</param>
        /// <param name="elseTarget">Target block for the branch when <paramref name="ifCondition"/> evaluates to a zero value</param>
        /// <returns><see cref="Instructions.Branch"/></returns>
        public Branch Branch( Value ifCondition, BasicBlock thenTarget, BasicBlock elseTarget )
        {
            ifCondition.ValidateNotNull( nameof( ifCondition ) );
            thenTarget.ValidateNotNull( nameof( thenTarget ) );
            elseTarget.ValidateNotNull( nameof( elseTarget ) );

            var handle = LLVMBuildCondBr( BuilderHandle
                                        , ifCondition.ValueHandle
                                        , thenTarget.BlockHandle
                                        , elseTarget.BlockHandle
                                        );

            return Value.FromHandle<Branch>( handle );
        }

        /// <summary>Creates an <see cref="Instructions.Unreachable"/> instruction</summary>
        /// <returns><see cref="Instructions.Unreachable"/> </returns>
        public Unreachable Unreachable( )
            => Value.FromHandle<Unreachable>( LLVMBuildUnreachable( BuilderHandle ) );

        /// <summary>Builds an Integer compare instruction</summary>
        /// <param name="predicate">Integer predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( IntPredicate predicate, Value lhs, Value rhs )
        {
            predicate.ValidateDefined( nameof( predicate ) );
            lhs.ValidateNotNull( nameof( lhs ) );
            rhs.ValidateNotNull( nameof( rhs ) );

            if( !lhs.NativeType.IsInteger && !lhs.NativeType.IsPointer )
            {
                throw new ArgumentException( "Expecting an integer or pointer type", nameof( lhs ) );
            }

            if( !rhs.NativeType.IsInteger && !lhs.NativeType.IsPointer )
            {
                throw new ArgumentException( "Expecting an integer or pointer type", nameof( rhs ) );
            }

            var handle = LLVMBuildICmp( BuilderHandle, ( LLVMIntPredicate )predicate, lhs.ValueHandle, rhs.ValueHandle, string.Empty );
            return Value.FromHandle( handle );
        }

        /// <summary>Builds a Floating point compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( RealPredicate predicate, Value lhs, Value rhs )
        {
            predicate.ValidateDefined( nameof( predicate ) );
            lhs.ValidateNotNull( nameof( lhs ) );
            rhs.ValidateNotNull( nameof( rhs ) );

            if( !lhs.NativeType.IsFloatingPoint )
            {
                throw new ArgumentException( "Expecting an integer type", nameof( lhs ) );
            }

            if( !rhs.NativeType.IsFloatingPoint )
            {
                throw new ArgumentException( "Expecting an integer type", nameof( rhs ) );
            }

            var handle = LLVMBuildFCmp( BuilderHandle
                                      , ( LLVMRealPredicate )predicate
                                      , lhs.ValueHandle
                                      , rhs.ValueHandle
                                      , string.Empty
                                      );
            return Value.FromHandle( handle );
        }

        /// <summary>Builds a compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( Predicate predicate, Value lhs, Value rhs )
        {
            if( predicate <= Predicate.LastFcmpPredicate )
            {
                return Compare( ( RealPredicate )predicate, lhs, rhs );
            }

            if( predicate >= Predicate.FirstIcmpPredicate && predicate <= Predicate.LastIcmpPredicate )
            {
                return Compare( ( IntPredicate )predicate, lhs, rhs );
            }

            throw new ArgumentOutOfRangeException( nameof( predicate ), $"'{predicate}' is not a valid value for a compare predicate" );
        }

        /// <summary>Creates a zero extend or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value ZeroExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstZExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildZExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a sign extend or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SignExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstSExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildSExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a trunc or bit cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value TruncOrBitCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstTruncOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildTruncOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a Zero Extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value ZeroExtend( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstZExt( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildZExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a Sign Extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SignExtend( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            if( valueRef is Constant )
            {
                return Value.FromHandle( LLVMConstSExt( valueRef.ValueHandle, targetType.GetTypeRef( ) ) );
            }

            var retValueRef = LLVMBuildSExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( retValueRef );
        }

        /// <summary>Creates a bitcast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value BitCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates an integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <param name="isSigned">Flag to indicate if the cast is signed or unsigned</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value IntCast( Value valueRef, ITypeRef targetType, bool isSigned )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstIntCast( valueRef.ValueHandle, targetType.GetTypeRef( ), isSigned );
            }
            else
            {
                handle = LLVMBuildIntCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a trunc instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value Trunc( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            if( valueRef is Constant )
            {
                return Value.FromHandle( LLVMConstTrunc( valueRef.ValueHandle, targetType.GetTypeRef( ) ) );
            }

            return Value.FromHandle( LLVMBuildTrunc( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty ) );
        }

        /// <summary>Creates a signed integer to floating point cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value SIToFPCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstSIToFP( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildSIToFP( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates an unsigned integer to floating point cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value UIToFPCast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstUIToFP( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildUIToFP( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a Floating point to unsigned integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPToUICast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstFPToUI( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildFPToUI( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a floating point to signed integer cast instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPToSICast( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstFPToSI( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildFPToSI( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a floating point extend instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPExt( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstFPExt( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildFPExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Creates a floating point truncate instruction</summary>
        /// <param name="valueRef">Operand for the instruction</param>
        /// <param name="targetType">Target type for the instruction</param>
        /// <returns>Result <see cref="Value"/></returns>
        public Value FPTrunc( Value valueRef, ITypeRef targetType )
        {
            valueRef.ValidateNotNull( nameof( valueRef ) );
            targetType.ValidateNotNull( nameof( targetType ) );

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = LLVMConstFPTrunc( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = LLVMBuildFPTrunc( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        /// <summary>Builds a <see cref="Llvm.NET.Instructions.Select"/> instruction</summary>
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
            ifCondition.ValidateNotNull( nameof( ifCondition ) );
            thenValue.ValidateNotNull( nameof( thenValue ) );
            elseValue.ValidateNotNull( nameof( elseValue ) );

            var conditionVectorType = ifCondition.NativeType as IVectorType;
            var thenVector = thenValue.NativeType as IVectorType;
            var elseVector = elseValue.NativeType as IVectorType;

            if( ifCondition.NativeType.IntegerBitWidth != 1 && conditionVectorType != null && conditionVectorType.ElementType.IntegerBitWidth != 1 )
            {
                throw new ArgumentException( "condition value must be an i1 or vector of i1", nameof( ifCondition ) );
            }

            if( conditionVectorType != null )
            {
                const string errMsg = "When condition is a vector, selected values must be a vector of the same size";
                if( thenVector == null || thenVector.Size != conditionVectorType.Size )
                {
                    throw new ArgumentException( errMsg, nameof( thenValue ) );
                }

                if( elseValue == null || elseVector.Size != conditionVectorType.Size )
                {
                    throw new ArgumentException( errMsg, nameof( elseValue ) );
                }
            }
            else
            {
                if( elseValue.NativeType != thenValue.NativeType )
                {
                    throw new ArgumentException( "Selected values must have the same type" );
                }
            }

            var handle = LLVMBuildSelect( BuilderHandle
                                        , ifCondition.ValueHandle
                                        , thenValue.ValueHandle
                                        , elseValue.ValueHandle
                                        , string.Empty
                                        );
            return Value.FromHandle( handle );
        }

        /// <summary>Creates a Phi instruction</summary>
        /// <param name="resultType">Result type for the instruction</param>
        /// <returns><see cref="Instructions.PhiNode"/></returns>
        public PhiNode PhiNode( ITypeRef resultType )
        {
            var handle = LLVMBuildPhi( BuilderHandle, resultType.GetTypeRef( ), string.Empty );
            return Value.FromHandle<PhiNode>( handle );
        }

        /// <summary>Creates an extractvalue instruction</summary>
        /// <param name="instance">Instance to extract a value from</param>
        /// <param name="index">index of the element to extract</param>
        /// <returns>Value for the instruction</returns>
        public Value ExtractValue( Value instance, uint index )
        {
            instance.ValidateNotNull( nameof( instance ) );

            var handle = LLVMBuildExtractValue( BuilderHandle, instance.ValueHandle, index, string.Empty );
            return Value.FromHandle( handle );
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
        public Instructions.Switch Switch( Value value, BasicBlock defaultCase, uint numCases )
        {
            value.ValidateNotNull( nameof( value ) );
            defaultCase.ValidateNotNull( nameof( defaultCase ) );

            var handle = LLVMBuildSwitch( BuilderHandle, value.ValueHandle, defaultCase.BlockHandle, numCases );
            return Value.FromHandle<Instructions.Switch>( handle );
        }

        /// <summary>Creates a call to the llvm.donothing intrinsic</summary>
        /// <returns><see cref="CallInstruction"/></returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="InsertBlock"/> is <see langword="null"/> or it's <see cref="BasicBlock.ContainingFunction"/> is null or has a <see langword="null"/> <see cref="GlobalValue.ParentModule"/>
        /// </exception>
        public CallInstruction DoNothing( )
        {
            BitcodeModule module = GetModuleOrThrow( );
            var func = module.GetIntrinsicDeclaration( "llvm.donothing" );
            var hCall = BuildCall( func );
            return Value.FromHandle<CallInstruction>( hCall );
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
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memcpy</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memcpy intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemCpy( Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            destination.ValidateNotNull( nameof( destination ) );
            source.ValidateNotNull( nameof( source ) );
            len.ValidateNotNull( nameof( len ) );
            var module = GetModuleOrThrow( );

            if( destination == source )
            {
                throw new InvalidOperationException( "Source and destination arguments are the same value" );
            }

            var dstPtrType = destination.NativeType as IPointerType;
            if( dstPtrType == null )
            {
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );
            }

            var srcPtrType = source.NativeType as IPointerType;
            if( srcPtrType == null )
            {
                throw new ArgumentException( "Pointer type expected", nameof( source ) );
            }

            if( !len.NativeType.IsInteger )
            {
                throw new ArgumentException( "Integer type expected", nameof( len ) );
            }

            if( Context != module.Context )
            {
                throw new ArgumentException( "Module and instruction builder must come from the same context" );
            }

            if( !dstPtrType.ElementType.IsInteger )
            {
                dstPtrType = module.Context.Int8Type.CreatePointerType( );
                destination = BitCast( destination, dstPtrType );
            }

            if( !srcPtrType.ElementType.IsInteger )
            {
                srcPtrType = module.Context.Int8Type.CreatePointerType( );
                source = BitCast( source, srcPtrType );
            }

            // find the name of the appropriate overloaded form
            var func = module.GetIntrinsicDeclaration( "llvm.memcpy.p.p.i", dstPtrType, srcPtrType, len.NativeType );

            var call = BuildCall( func
                                , destination
                                , source
                                , len
                                , module.Context.CreateConstant( align )
                                , module.Context.CreateConstant( isVolatile )
                                );
            return Value.FromHandle( call );
        }

        /// <summary>Builds a memmove intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memmove</param>
        /// <param name="source">Source pointer of the memmove</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memmove</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memmove intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemMove( Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            destination.ValidateNotNull( nameof( destination ) );
            source.ValidateNotNull( nameof( source ) );
            len.ValidateNotNull( nameof( len ) );
            var module = GetModuleOrThrow( );

            if( destination == source )
            {
                throw new InvalidOperationException( "Source and destination arguments are the same value" );
            }

            if( !( destination.NativeType is IPointerType dstPtrType ) )
            {
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );
            }

            if( !( source.NativeType is IPointerType srcPtrType ) )
            {
                throw new ArgumentException( "Pointer type expected", nameof( source ) );
            }

            if( !len.NativeType.IsInteger )
            {
                throw new ArgumentException( "Integer type expected", nameof( len ) );
            }

            if( Context != module.Context )
            {
                throw new ArgumentException( "Module and instruction builder must come from the same context" );
            }

            if( !dstPtrType.ElementType.IsInteger )
            {
                dstPtrType = module.Context.Int8Type.CreatePointerType( );
                destination = BitCast( destination, dstPtrType );
            }

            if( !srcPtrType.ElementType.IsInteger )
            {
                srcPtrType = module.Context.Int8Type.CreatePointerType( );
                source = BitCast( source, srcPtrType );
            }

            // find the name of the appropriate overloaded form
            var func = module.GetIntrinsicDeclaration( "llvm.memmove.p.p.i", dstPtrType, srcPtrType, len.NativeType );

            var call = BuildCall( func, destination, source, len, module.Context.CreateConstant( align ), module.Context.CreateConstant( isVolatile ) );
            return Value.FromHandle( call );
        }

        /// <summary>Builds a memset intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memset</param>
        /// <param name="value">fill value for the memset</param>
        /// <param name="len">length of the data to fill</param>
        /// <param name="align">ALignment of the data for the fill</param>
        /// <param name="isVolatile">Flag to indicate if the fill involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memset</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memset intrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemSet( Value destination, Value value, Value len, Int32 align, bool isVolatile )
        {
            destination.ValidateNotNull( nameof( destination ) );
            value.ValidateNotNull( nameof( value ) );
            len.ValidateNotNull( nameof( len ) );
            var module = GetModuleOrThrow( );

            if( !( destination.NativeType is IPointerType dstPtrType ) )
            {
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );
            }

            if( dstPtrType.ElementType != value.NativeType )
            {
                throw new ArgumentException( "Pointer type doesn't match the value type" );
            }

            if( !value.NativeType.IsInteger )
            {
                throw new ArgumentException( "Integer type expected", nameof( value ) );
            }

            if( !len.NativeType.IsInteger )
            {
                throw new ArgumentException( "Integer type expected", nameof( len ) );
            }

            if( Context != module.Context )
            {
                throw new ArgumentException( "Module and instruction builder must come from the same context" );
            }

            if( !dstPtrType.ElementType.IsInteger )
            {
                dstPtrType = module.Context.Int8Type.CreatePointerType( );
                destination = BitCast( destination, dstPtrType );
            }

            // find the name of the appropriate overloaded form
            var func = module.GetIntrinsicDeclaration( "llvm.memset.p.i", dstPtrType, value.NativeType );

            var call = BuildCall( func
                                , destination
                                , value
                                , len
                                , module.Context.CreateConstant( align )
                                , module.Context.CreateConstant( isVolatile )
                                );

            return Value.FromHandle( call );
        }

        /// <summary>Builds an <see cref="Llvm.NET.Instructions.InsertValue"/> instruction </summary>
        /// <param name="aggValue">Aggregate value to insert <paramref name="elementValue"/> into</param>
        /// <param name="elementValue">Value to insert into <paramref name="aggValue"/></param>
        /// <param name="index">Index to insert the value into</param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value InsertValue( Value aggValue, Value elementValue, uint index )
        {
            aggValue.ValidateNotNull( nameof( aggValue ) );
            elementValue.ValidateNotNull( nameof( elementValue ) );

            var handle = LLVMBuildInsertValue( BuilderHandle, aggValue.ValueHandle, elementValue.ValueHandle, index, string.Empty );
            return Value.FromHandle( handle );
        }

        /// <summary>Generates a call to the llvm.[s|u]add.with.overflow intrinsic</summary>
        /// <param name="lhs">Left hand side of the operation</param>
        /// <param name="rhs">Right hand side of the operation</param>
        /// <param name="signed">Flag to indicate if the operation is signed <see langword="true"/> or unsigned <see langword="false"/></param>
        /// <returns>Instruction as a <see cref="Value"/></returns>
        public Value AddWithOverflow( Value lhs, Value rhs, bool signed )
        {
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
            char kind = signed ? 's' : 'u';
            string name = $"llvm.{kind}mul.with.overflow.i";
            uint id = Intrinsic.LookupId( name );
            var module = GetModuleOrThrow( );

            var function = module.GetIntrinsicDeclaration( id, lhs.NativeType );
            return Call( function, lhs, rhs );
        }

        internal static LLVMValueRef[ ] GetValidatedGEPArgs( Value pointer, IEnumerable<Value> args )
        {
            if( pointer.NativeType.Kind != TypeKind.Pointer )
            {
                throw new ArgumentException( "Pointer value expected", nameof( pointer ) );
            }

            // if not an array already, pull from source enumerable into an array only once
            var argsArray = args as Value[ ] ?? args.ToArray( );
            if( argsArray.Any( a => !a.NativeType.IsInteger ) )
            {
                throw new ArgumentException( $"GEP index arguments must be integers" );
            }

            LLVMValueRef[ ] llvmArgs = argsArray.Select( a => a.ValueHandle ).ToArray( );
            if( llvmArgs.Length == 0 )
            {
                throw new ArgumentException( "There must be at least one index argument", nameof( args ) );
            }

            return llvmArgs;
        }

        internal LLVMBuilderRef BuilderHandle { get; }

        private BitcodeModule GetModuleOrThrow( )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return module;
        }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a unary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildUnaryOp( Func<LLVMBuilderRef, LLVMValueRef, string, LLVMValueRef> opFactory
                                  , Value operand
                                  )
        {
            var valueRef = opFactory( BuilderHandle, operand.ValueHandle, string.Empty );
            return Value.FromHandle( valueRef );
        }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a binary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildBinOp( Func<LLVMBuilderRef, LLVMValueRef, LLVMValueRef, string, LLVMValueRef> opFactory
                                , Value lhs
                                , Value rhs
                                )
        {
            if( lhs.NativeType != rhs.NativeType )
            {
                throw new ArgumentException( "Types of binary operators must be identical" );
            }

            var valueRef = opFactory( BuilderHandle, lhs.ValueHandle, rhs.ValueHandle, string.Empty );
            return Value.FromHandle( valueRef );
        }

        private AtomicRMW BuildAtomicRMW( LLVMAtomicRMWBinOp op, Value ptr, Value val )
        {
            var ptrType = ptr.NativeType as IPointerType;
            if( ptrType == null )
            {
                throw new ArgumentException( "Expected pointer type", nameof( ptr ) );
            }

            if( ptrType.ElementType != val.NativeType )
            {
                throw new ArgumentException( string.Format( IncompatibleTypeMsgFmt, ptrType.ElementType, val.NativeType ) );
            }

            var handle = LLVMBuildAtomicRMW( BuilderHandle, op, ptr.ValueHandle, val.ValueHandle, LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent, false );
            return Value.FromHandle<AtomicRMW>( handle );
        }

        private static void ValidateCallArgs( [NotNull] Value func, IReadOnlyList<Value> args )
        {
            var funcPtrType = func.NativeType as IPointerType;
            if( funcPtrType == null )
            {
                throw new ArgumentException( "Expected pointer to function", nameof( func ) );
            }

            var elementType = funcPtrType.ElementType as FunctionType;
            if( elementType == null )
            {
                throw new ArgumentException( "A pointer to a function is required for an indirect call", nameof( func ) );
            }

            if( args.Count != elementType.ParameterTypes.Count )
            {
                throw new ArgumentException( "Mismatched parameter count with call site", nameof( args ) );
            }

            for( int i = 0; i < args.Count; ++i )
            {
                if( args[ i ].NativeType != elementType.ParameterTypes[ i ] )
                {
                    string msg = $"Call site argument type mismatch for function {func} at index {i}; argType={args[ i ].NativeType}; signatureType={elementType.ParameterTypes[ i ]}";
                    Debug.WriteLine( msg );
                    throw new ArgumentException( msg, nameof( args ) );
                }
            }
        }

        private LLVMValueRef BuildCall( Value func, params Value[ ] args ) => BuildCall( func, ( IReadOnlyList<Value> )args );

        private LLVMValueRef BuildCall( Value func ) => BuildCall( func, new List<Value>( ) );

        private LLVMValueRef BuildCall( Value func, IReadOnlyList<Value> args )
        {
            ValidateCallArgs( func, args );

            LLVMValueRef[ ] llvmArgs = args.Select( v => v.ValueHandle ).ToArray( );
            int argCount = llvmArgs.Length;

            // must always provide at least one element for successful marshaling/interop, but tell LLVM there are none
            if( argCount == 0 )
            {
                llvmArgs = new LLVMValueRef[ 1 ];
            }

            return LLVMBuildCall( BuilderHandle, func.ValueHandle, out llvmArgs[ 0 ], ( uint )argCount, string.Empty );
        }

        private const string IncompatibleTypeMsgFmt = "Incompatible types: destination pointer must be of the same type as the value stored.\n"
                                                    + "Types are:\n"
                                                    + "\tDestination: {0}\n"
                                                    + "\tValue: {1}";
    }
}

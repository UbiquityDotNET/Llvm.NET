﻿// <copyright file="InstructionBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Llvm.NET.Native;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

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
            BuilderHandle = NativeMethods.CreateBuilderInContext( context.ContextHandle );
        }

        /// <summary>Initializes a new instance of the <see cref="InstructionBuilder"/> class for a <see cref="BasicBlock"/></summary>
        /// <param name="block">Block this builder is initially attached to</param>
        public InstructionBuilder( BasicBlock block )
            : this( block.ValidateNotNull( nameof( block ) ).ContainingFunction.ParentModule.Context )
        {
            PositionAtEnd( block );
        }

        ~InstructionBuilder( )
        {
            BuilderHandle.Close( );
        }

        /// <summary>Gets the context this builder is creating instructions for</summary>
        public Context Context { get; }

        /// <summary>Gets the <see cref="BasicBlock"/> this builder is building instructions for</summary>
        public BasicBlock InsertBlock
        {
            get
            {
                var handle = NativeMethods.GetInsertBlock( BuilderHandle );
                if( handle.Pointer.IsNull( ) )
                {
                    return null;
                }

                return BasicBlock.FromHandle( NativeMethods.GetInsertBlock( BuilderHandle ) );
            }
        }

        /// <summary>Positions the builder at the end of a given <see cref="BasicBlock"/></summary>
        /// <param name="basicBlock">Block to set the poition of</param>
        public void PositionAtEnd( BasicBlock basicBlock )
        {
            if( basicBlock == null )
            {
                throw new ArgumentNullException( nameof( basicBlock ) );
            }

            NativeMethods.PositionBuilderAtEnd( BuilderHandle, basicBlock.BlockHandle );
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public void PositionBefore( Instruction instr )
        {
            if( instr == null )
            {
                throw new ArgumentNullException( nameof( instr ) );
            }

            NativeMethods.PositionBuilderBefore( BuilderHandle, instr.ValueHandle );
        }

        public Value FNeg( Value value ) => BuildUnaryOp( NativeMethods.BuildFNeg, value );

        public Value FAdd( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFAdd, lhs, rhs );

        public Value FSub( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFSub, lhs, rhs );

        public Value FMul( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFMul, lhs, rhs );

        public Value FDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFDiv, lhs, rhs );

        public Value FRem( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFRem, lhs, rhs );

        public Value Neg( Value value ) => BuildUnaryOp( NativeMethods.BuildNeg, value );

        public Value Not( Value value ) => BuildUnaryOp( NativeMethods.BuildNot, value );

        public Value Add( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildAdd, lhs, rhs );

        public Value And( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildAnd, lhs, rhs );

        public Value Sub( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildSub, lhs, rhs );

        public Value Mul( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildMul, lhs, rhs );

        public Value ShiftLeft( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildShl, lhs, rhs );

        public Value ArithmeticShiftRight( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildAShr, lhs, rhs );

        public Value LogicalShiftRight( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildLShr, lhs, rhs );

        public Value UDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs );

        public Value SDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs );

        public Value URem( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildURem, lhs, rhs );

        public Value SRem( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildSRem, lhs, rhs );

        public Value Xor( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildXor, lhs, rhs );

        public Value Or( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildOr, lhs, rhs );

        public Alloca Alloca( ITypeRef typeRef )
        {
            var handle = NativeMethods.BuildAlloca( BuilderHandle, typeRef.GetTypeRef( ), string.Empty );
            return Value.FromHandle<Alloca>( handle );
        }

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

            var instHandle = NativeMethods.BuildArrayAlloca( BuilderHandle, typeRef.GetTypeRef( ), elements.ValueHandle, string.Empty );
            return Value.FromHandle<Alloca>( instHandle );
        }

        public ReturnInstruction Return( )
        {
            if( !InsertBlock.ContainingFunction.ReturnType.IsVoid )
            {
                throw new ArgumentException( "Return instruction for non-void function must have a value" );
            }

            return Value.FromHandle<ReturnInstruction>( NativeMethods.BuildRetVoid( BuilderHandle ) );
        }

        public ReturnInstruction Return( Value value )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            var retType = InsertBlock.ContainingFunction.ReturnType;
            if( retType.IsVoid )
            {
                throw new ArgumentException( "Return instruction for void function must not have a value", nameof( value ) );
            }

            if( retType != value.NativeType )
            {
                throw new ArgumentException( "Value for return must match the function signature's return type", nameof( value ) );
            }

            var handle = NativeMethods.BuildRet( BuilderHandle, value.ValueHandle );
            return Value.FromHandle<ReturnInstruction>( handle );
        }

        public CallInstruction Call( Value func, params Value[ ] args ) => Call( func, ( IReadOnlyList<Value> )args );

        public CallInstruction Call( Value func, IReadOnlyList<Value> args )
        {
            LLVMValueRef hCall = BuildCall( func, args );
            var retVal = Value.FromHandle<CallInstruction>( hCall );
            return retVal;
        }

        public Invoke Invoke( Value func, IReadOnlyList<Value> args, BasicBlock then, BasicBlock catchBlock )
        {
            if( then == null )
            {
                throw new ArgumentNullException( nameof( then ) );
            }

            if( catchBlock == null )
            {
                throw new ArgumentNullException( nameof( catchBlock ) );
            }

            ValidateCallArgs( func, args );

            LLVMValueRef[] llvmArgs = args.Select( v => v.ValueHandle ).ToArray();
            int argCount = llvmArgs.Length;

            // Must always provide at least one element for successful marshaling/interop, but tell LLVM there are none.
            if( argCount == 0 )
            {
                llvmArgs = new LLVMValueRef[1];
            }

            LLVMValueRef invoke = NativeMethods.BuildInvoke( BuilderHandle
                                                           , func.ValueHandle
                                                           , out llvmArgs[0]
                                                           , (uint)argCount
                                                           , then.BlockHandle
                                                           , catchBlock.BlockHandle
                                                           , string.Empty
                                                           );

            return Value.FromHandle<Invoke>( invoke );
        }

        public LandingPad LandingPad( ITypeRef resultType )
        {
            LLVMValueRef landingPad = NativeMethods.BuildLandingPad( BuilderHandle
                                                                   , resultType.GetTypeRef()
                                                                   , new LLVMValueRef( IntPtr.Zero ) // personality function no longer part of instruction
                                                                   , 0
                                                                   , string.Empty
                                                                   );

            return Value.FromHandle<LandingPad>( landingPad );
        }

        public ResumeInstruction Resume( Value exception )
        {
            if( exception == null )
            {
                throw new ArgumentNullException( nameof( exception ) );
            }

            LLVMValueRef resume = NativeMethods.BuildResume( BuilderHandle, exception.ValueHandle );
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
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( destination == null )
            {
                throw new ArgumentNullException( nameof( destination ) );
            }

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

            return Value.FromHandle<Store>( NativeMethods.BuildStore( BuilderHandle, value.ValueHandle, destination.ValueHandle ) );
        }

        public Load Load( Value sourcePtr )
        {
            if( sourcePtr == null )
            {
                throw new ArgumentNullException( nameof( sourcePtr ) );
            }

            var handle = NativeMethods.BuildLoad( BuilderHandle, sourcePtr.ValueHandle, string.Empty );
            return Value.FromHandle<Load>( handle );
        }

        public Value AtomicXchg( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXchg, ptr, val );

        public Value AtomicAdd( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAdd, ptr, val );

        public Value AtomicSub( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpSub, ptr, val );

        public Value AtomicAnd( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpAnd, ptr, val );

        public Value AtomicNand( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpNand, ptr, val );

        public Value AtomicOr( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpOr, ptr, val );

        public Value AtomicXor( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpXor, ptr, val );

        public Value AtomicMax( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMax, ptr, val );

        public Value AtomicMin( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpMin, ptr, val );

        public Value AtomicUMax( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMax, ptr, val );

        public Value AtomicUMin( Value ptr, Value val ) => BuildAtomicBinOp( LLVMAtomicRMWBinOp.LLVMAtomicRMWBinOpUMin, ptr, val );

        public Value AtomicCmpXchg( Value ptr, Value cmp, Value value )
        {
            if( ptr == null )
            {
                throw new ArgumentNullException( nameof( ptr ) );
            }

            if( cmp == null )
            {
                throw new ArgumentNullException( nameof( cmp ) );
            }

            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

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

            var handle = NativeMethods.BuildAtomicCmpXchg( BuilderHandle
                                                         , ptr.ValueHandle
                                                         , cmp.ValueHandle
                                                         , value.ValueHandle
                                                         , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                                         , LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent
                                                         , false
                                                         );
            return Value.FromHandle( handle );
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
            if( pointer == null )
            {
                throw new ArgumentNullException( nameof( pointer ) );
            }

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

            var handle = NativeMethods.BuildStructGEP( BuilderHandle, pointer.ValueHandle, index, string.Empty );
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
        /// For details on GetElementPointer (GEP) see http://llvm.org/docs/GetElementPtr.html. The
        /// basic gist is that the GEP instruction does not access memory, it only computes a pointer
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
            var handle = NativeMethods.BuildGEP( BuilderHandle
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
        /// For details on GetElementPointer (GEP) see http://llvm.org/docs/GetElementPtr.html. The
        /// basic gist is that the GEP instruction does not access memory, it only computes a pointer
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
        /// For details on GetElementPointer (GEP) see http://llvm.org/docs/GetElementPtr.html. The
        /// basic gist is that the GEP instruction does not access memory, it only computes a pointer
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
            var hRetVal = NativeMethods.BuildInBoundsGEP( BuilderHandle
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
        /// For details on GetElementPointer (GEP) see http://llvm.org/docs/GetElementPtr.html. The
        /// basic gist is that the GEP instruction does not access memory, it only computes a pointer
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
            var handle = NativeMethods.ConstInBoundsGEP( pointer.ValueHandle, out llvmArgs[ 0 ], ( uint )llvmArgs.Length );
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
            if( intValue == null )
            {
                throw new ArgumentNullException( nameof( intValue ) );
            }

            if( ptrType == null )
            {
                throw new ArgumentNullException( nameof( ptrType ) );
            }

            if( intValue is Constant )
            {
                return Value.FromHandle( NativeMethods.ConstIntToPtr( intValue.ValueHandle, ptrType.GetTypeRef( ) ) );
            }

            var handle = NativeMethods.BuildIntToPtr( BuilderHandle, intValue.ValueHandle, ptrType.GetTypeRef( ), string.Empty );
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
            if( ptrValue == null )
            {
                throw new ArgumentNullException( nameof( ptrValue ) );
            }

            if( intType == null )
            {
                throw new ArgumentNullException( nameof( intType ) );
            }

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
                return Value.FromHandle( NativeMethods.ConstPtrToInt( ptrValue.ValueHandle, intType.GetTypeRef( ) ) );
            }

            var handle = NativeMethods.BuildPtrToInt( BuilderHandle, ptrValue.ValueHandle, intType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( handle );
        }

        public Branch Branch( BasicBlock target )
            => Value.FromHandle<Branch>( NativeMethods.BuildBr( BuilderHandle, target.ValidateNotNull( nameof( target ) ).BlockHandle ) );

        public Branch Branch( Value ifCondition, BasicBlock thenTarget, BasicBlock elseTarget )
        {
            if( ifCondition == null )
            {
                throw new ArgumentNullException( nameof( ifCondition ) );
            }

            if( thenTarget == null )
            {
                throw new ArgumentNullException( nameof( thenTarget ) );
            }

            if( elseTarget == null )
            {
                throw new ArgumentNullException( nameof( elseTarget ) );
            }

            var handle = NativeMethods.BuildCondBr( BuilderHandle
                                                  , ifCondition.ValueHandle
                                                  , thenTarget.BlockHandle
                                                  , elseTarget.BlockHandle
                                                  );

            return Value.FromHandle<Branch>( handle );
        }

        public Unreachable Unreachable( ) => Value.FromHandle<Unreachable>( NativeMethods.BuildUnreachable( BuilderHandle ) );

        /// <summary>Builds an Integer compare instruction</summary>
        /// <param name="predicate">Integer predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( IntPredicate predicate, Value lhs, Value rhs )
        {
            if( lhs == null )
            {
                throw new ArgumentNullException( nameof( lhs ) );
            }

            if( rhs == null )
            {
                throw new ArgumentNullException( nameof( rhs ) );
            }

            if( !lhs.NativeType.IsInteger && !lhs.NativeType.IsPointer )
            {
                throw new ArgumentException( "Expecting an integer or pointer type", nameof( lhs ) );
            }

            if( !rhs.NativeType.IsInteger && !lhs.NativeType.IsPointer )
            {
                throw new ArgumentException( "Expecting an integer or pointer type", nameof( rhs ) );
            }

            var handle = NativeMethods.BuildICmp( BuilderHandle, ( LLVMIntPredicate )predicate, lhs.ValueHandle, rhs.ValueHandle, string.Empty );
            return Value.FromHandle( handle );
        }

        /// <summary>Builds a Floating point compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( RealPredicate predicate, Value lhs, Value rhs )
        {
            if( lhs == null )
            {
                throw new ArgumentNullException( nameof( lhs ) );
            }

            if( rhs == null )
            {
                throw new ArgumentNullException( nameof( rhs ) );
            }

            if( !lhs.NativeType.IsFloatingPoint )
            {
                throw new ArgumentException( "Expecting an integer type", nameof( lhs ) );
            }

            if( !rhs.NativeType.IsFloatingPoint )
            {
                throw new ArgumentException( "Expecting an integer type", nameof( rhs ) );
            }

            var handle = NativeMethods.BuildFCmp( BuilderHandle
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

        public Value ZeroExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstZExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildZExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value SignExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstSExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildSExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value TruncOrBitCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstTruncOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildTruncOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value ZeroExtend( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstZExt( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildZExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value SignExtend( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            if( valueRef is Constant )
            {
                return Value.FromHandle( NativeMethods.ConstSExt( valueRef.ValueHandle, targetType.GetTypeRef( ) ) );
            }

            var retValueRef = NativeMethods.BuildSExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            return Value.FromHandle( retValueRef );
        }

        public Value BitCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.NativeType == targetType )
            {
                return valueRef;
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstBitCast( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value IntCast( Value valueRef, ITypeRef targetType, bool isSigned )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstIntCast( valueRef.ValueHandle, targetType.GetTypeRef( ), isSigned );
            }
            else
            {
                handle = NativeMethods.BuildIntCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value Trunc( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            if( valueRef is Constant )
            {
                return Value.FromHandle( NativeMethods.ConstTrunc( valueRef.ValueHandle, targetType.GetTypeRef( ) ) );
            }

            return Value.FromHandle( NativeMethods.BuildTrunc( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty ) );
        }

        public Value SIToFPCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstSIToFP( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildSIToFP( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value UIToFPCast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstUIToFP( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildUIToFP( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value FPToUICast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstFPToUI( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildFPToUI( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value FPToSICast( Value valueRef, ITypeRef targetType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( targetType == null )
            {
                throw new ArgumentNullException( nameof( targetType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstFPToSI( valueRef.ValueHandle, targetType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildFPToSI( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value FPExt( Value valueRef, ITypeRef toType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( toType == null )
            {
                throw new ArgumentNullException( nameof( toType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstFPExt( valueRef.ValueHandle, toType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildFPExt( BuilderHandle, valueRef.ValueHandle, toType.GetTypeRef( ), string.Empty );
            }

            return Value.FromHandle( handle );
        }

        public Value FPTrunc( Value valueRef, ITypeRef toType )
        {
            if( valueRef == null )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( toType == null )
            {
                throw new ArgumentNullException( nameof( toType ) );
            }

            LLVMValueRef handle;
            if( valueRef is Constant )
            {
                handle = NativeMethods.ConstFPTrunc( valueRef.ValueHandle, toType.GetTypeRef( ) );
            }
            else
            {
                handle = NativeMethods.BuildFPTrunc( BuilderHandle, valueRef.ValueHandle, toType.GetTypeRef( ), string.Empty );
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
            if( ifCondition == null )
            {
                throw new ArgumentNullException( nameof( ifCondition ) );
            }

            if( thenValue == null )
            {
                throw new ArgumentNullException( nameof( thenValue ) );
            }

            if( elseValue == null )
            {
                throw new ArgumentNullException( nameof( elseValue ) );
            }

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

            var handle = NativeMethods.BuildSelect( BuilderHandle
                                                  , ifCondition.ValueHandle
                                                  , thenValue.ValueHandle
                                                  , elseValue.ValueHandle
                                                  , string.Empty
                                                  );
            return Value.FromHandle( handle );
        }

        public PhiNode PhiNode( ITypeRef resultType )
        {
            var handle = NativeMethods.BuildPhi( BuilderHandle, resultType.GetTypeRef( ), string.Empty );
            return Value.FromHandle<PhiNode>( handle );
        }

        public Value ExtractValue( Value instance, uint index )
        {
            if( instance == null )
            {
                throw new ArgumentNullException( nameof( instance ) );
            }

            var handle = NativeMethods.BuildExtractValue( BuilderHandle, instance.ValueHandle, index, string.Empty );
            return Value.FromHandle( handle );
        }

        public Instructions.Switch Switch( Value value, BasicBlock defaultCase, uint numCases )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( defaultCase == null )
            {
                throw new ArgumentNullException( nameof( defaultCase ) );
            }

            var handle = NativeMethods.BuildSwitch( BuilderHandle, value.ValueHandle, defaultCase.BlockHandle, numCases );
            return Value.FromHandle<Instructions.Switch>( handle );
        }

        public Value DoNothing( )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return DoNothing( module );
        }

        public Value DoNothing( NativeModule module )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            var func = module.GetFunction( Intrinsic.DoNothingName );
            if( func == null )
            {
                var ctx = module.Context;
                var signature = ctx.GetFunctionType( ctx.VoidType );
                func = module.AddFunction( Intrinsic.DoNothingName, signature );
            }

            var hCall = BuildCall( func );
            return Value.FromHandle( hCall );
        }

        public Value DebugTrap( )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return DebugTrap( module );
        }

        public Value DebugTrap( NativeModule module )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            var func = module.GetFunction( Intrinsic.DebugTrapName );
            if( func == null )
            {
                var ctx = module.Context;
                var signature = ctx.GetFunctionType( ctx.VoidType );
                func = module.AddFunction( Intrinsic.DebugTrapName, signature );
            }

            var hCall = NativeMethods.BuildCall( BuilderHandle, func.ValueHandle, out LLVMValueRef args, 0U, string.Empty );
            return Value.FromHandle( hCall );
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
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return MemCpy( module, destination, source, len, align, isVolatile );
        }

        /// <summary>Builds a memcpy intrinsic call</summary>
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
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
        public Value MemCpy( NativeModule module, Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( destination == null )
            {
                throw new ArgumentNullException( nameof( destination ) );
            }

            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            if( len == null )
            {
                throw new ArgumentNullException( nameof( len ) );
            }

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
            string intrinsicName = Instructions.MemCpy.GetIntrinsicNameForArgs( dstPtrType, srcPtrType, len.NativeType );
            var func = module.GetFunction( intrinsicName );
            if( func == null )
            {
                var signature = module.Context.GetFunctionType( module.Context.VoidType
                                                              , dstPtrType
                                                              , srcPtrType
                                                              , len.NativeType
                                                              , module.Context.Int32Type
                                                              , module.Context.BoolType
                                                              );
                func = module.AddFunction( intrinsicName, signature );
            }

            var call = BuildCall( func
                                , destination
                                , source
                                , len
                                , module.Context.CreateConstant( align )
                                , module.Context.CreateConstant( isVolatile )
                                );
            return Value.FromHandle( call );
        }

        /// <summary>Builds a memmov intrinsic call</summary>
        /// <param name="destination">Destination pointer of the memmov</param>
        /// <param name="source">Source pointer of the memmov</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memmov</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memmov intrinsic, this implementation currently assumes the
        /// single form defined by <see cref="Intrinsic.MemMoveName"/>, which matches the classic "C" style memmov
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code (as is done with
        /// <see cref="MemCpy(NativeModule, Value, Value, Value, int, bool)"/>.)
        /// </remarks>
        public Value MemMove( Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return MemMove( module, destination, source, len, align, isVolatile );
        }

        /// <summary>Builds a memmov intrinsic call</summary>
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
        /// <param name="destination">Destination pointer of the memmov</param>
        /// <param name="source">Source pointer of the memmov</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memmov</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memmov intrinsic, this implementation currently assumes the
        /// single form defined by <see cref="Intrinsic.MemMoveName"/>, which matches the classic "C" style memmov
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code (as is done with
        /// <see cref="MemCpy(NativeModule, Value, Value, Value, int, bool)"/>.)
        /// </remarks>
        public Value MemMove( NativeModule module, Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( destination == null )
            {
                throw new ArgumentNullException( nameof( destination ) );
            }

            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            if( len == null )
            {
                throw new ArgumentNullException( nameof( len ) );
            }

            // TODO: make this auto select the LLVM intrinsic signature like memcpy...
            if( !destination.NativeType.IsPointer )
            {
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );
            }

            if( !source.NativeType.IsPointer )
            {
                throw new ArgumentException( "Pointer type expected", nameof( source ) );
            }

            if( !len.NativeType.IsInteger )
            {
                throw new ArgumentException( "Integer type expected", nameof( len ) );
            }

            var ctx = module.Context;

            destination = BitCast( destination, ctx.Int8Type.CreatePointerType( ) );
            source = BitCast( source, ctx.Int8Type.CreatePointerType( ) );

            var func = module.GetFunction( Intrinsic.MemMoveName );
            if( func == null )
            {
                var signature = ctx.GetFunctionType( ctx.VoidType
                                                   , ctx.Int8Type.CreatePointerType( )
                                                   , ctx.Int8Type.CreatePointerType( )
                                                   , ctx.Int32Type
                                                   , ctx.Int32Type
                                                   , ctx.BoolType
                                                   );
                func = module.AddFunction( Intrinsic.MemMoveName, signature );
            }

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
        /// LLVM has many overloaded variants of the memset intrinsic, this implementation currently assumes the
        /// single form defined by <see cref="Intrinsic.MemSetName"/>, which matches the classic "C" style memset
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code (as is done with
        /// <see cref="MemCpy(NativeModule, Value, Value, Value, int, bool)"/>.)
        /// </remarks>
        public Value MemSet( Value destination, Value value, Value len, Int32 align, bool isVolatile )
        {
            var module = InsertBlock?.ContainingFunction?.ParentModule;
            if( module == null )
            {
                throw new InvalidOperationException( "Cannot insert when no block/module is available" );
            }

            return MemSet( module, destination, value, len, align, isVolatile );
        }

        /// <summary>Builds a memset intrinsic call</summary>
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
        /// <param name="destination">Destination pointer of the memset</param>
        /// <param name="value">fill value for the memset</param>
        /// <param name="len">length of the data to fill</param>
        /// <param name="align">ALignment of the data for the fill</param>
        /// <param name="isVolatile">Flag to indicate if the fill involves volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memset</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memset intrinsic, this implementation currently assumes the
        /// single form defined by <see cref="Intrinsic.MemSetName"/>, which matches the classic "C" style memset
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code (as is done with
        /// <see cref="MemCpy(NativeModule, Value, Value, Value, int, bool)"/>.)
        /// </remarks>
        public Value MemSet( NativeModule module, Value destination, Value value, Value len, Int32 align, bool isVolatile )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( destination == null )
            {
                throw new ArgumentNullException( nameof( destination ) );
            }

            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( len == null )
            {
                throw new ArgumentNullException( nameof( len ) );
            }

            if( destination.NativeType.Kind != TypeKind.Pointer )
            {
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );
            }

            if( value.NativeType.IntegerBitWidth != 8 )
            {
                throw new ArgumentException( "8bit value expected", nameof( value ) );
            }

            var ctx = module.Context;

            destination = BitCast( destination, ctx.Int8Type.CreatePointerType( ) );

            var func = module.GetFunction( Intrinsic.MemSetName );
            if( func == null )
            {
                var signature = ctx.GetFunctionType( ctx.VoidType
                                                   , ctx.Int8Type.CreatePointerType( )
                                                   , ctx.Int8Type
                                                   , ctx.Int32Type
                                                   , ctx.Int32Type
                                                   , ctx.BoolType
                                                   );
                func = module.AddFunction( Intrinsic.MemSetName, signature );
            }

            var call = BuildCall( func
                                , destination
                                , value
                                , len
                                , module.Context.CreateConstant( align )
                                , module.Context.CreateConstant( isVolatile )
                                );

            return Value.FromHandle( call );
        }

        public Value InsertValue( Value aggValue, Value elementValue, uint index )
        {
            if( aggValue == null )
            {
                throw new ArgumentNullException( nameof( aggValue ) );
            }

            if( elementValue == null )
            {
                throw new ArgumentNullException( nameof( elementValue ) );
            }

            var handle = NativeMethods.BuildInsertValue( BuilderHandle, aggValue.ValueHandle, elementValue.ValueHandle, index, string.Empty );
            return Value.FromHandle( handle );
        }

        internal static LLVMValueRef[ ] GetValidatedGEPArgs( Value pointer, IEnumerable<Value> args )
        {
            if( pointer == null )
            {
                throw new ArgumentNullException( nameof( pointer ) );
            }

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

        private Value BuildAtomicBinOp( LLVMAtomicRMWBinOp op, Value ptr, Value val )
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

            var handle = NativeMethods.BuildAtomicRMW( BuilderHandle, op, ptr.ValueHandle, val.ValueHandle, LLVMAtomicOrdering.LLVMAtomicOrderingSequentiallyConsistent, false );
            return Value.FromHandle( handle );
        }

        private static void ValidateCallArgs( Value func, IReadOnlyList<Value> args )
        {
            if( func == null )
            {
                throw new ArgumentNullException( nameof( func ) );
            }

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

            return NativeMethods.BuildCall( BuilderHandle, func.ValueHandle, out llvmArgs[ 0 ], ( uint )argCount, string.Empty );
        }

        private const string IncompatibleTypeMsgFmt = "Incompatible types: destination pointer must be of the same type as the value stored.\n"
                                            + "Types are:\n"
                                            + "\tDestination: {0}\n"
                                            + "\tValue: {1}";
    }
}

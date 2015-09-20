using System;
using System.Linq;
using Llvm.NET.Instructions;
using System.Collections.Generic;
using Llvm.NET.Values;
using Llvm.NET.Types;

namespace Llvm.NET
{
    // TODO: Now that the RegisterName extension method is available, remove all overloads providing the register name

    ///<summary>LLVM Instruction builder allowing managed code to generate IR instructions</summary>
    public class InstructionBuilder
        : IDisposable
    {
        /// <summary>Creates an <see cref="InstructionBuilder"/> for a given context</summary>
        /// <param name="context">Context used for creating instructions</param>
        public InstructionBuilder( Context context )
        {
            Context = context;
            BuilderHandle = NativeMethods.CreateBuilderInContext( context.ContextHandle );
        }

        /// <summary>Creates an <see cref="InstructionBuilder"/> for a <see cref="BasicBlock"/></summary>
        /// <param name="block">Block this builder is initially attached to</param>
        public InstructionBuilder( BasicBlock block )
            : this( block.ContainingFunction.ParentModule.Context )
        {
            PositionAtEnd( block );
        }

        public Context Context { get; }

        /// <summary>Positions the builder at the end of a given <see cref="BasicBlock"/></summary>
        /// <param name="basicBlock"></param>
        public void PositionAtEnd( BasicBlock basicBlock )
        {
            NativeMethods.PositionBuilderAtEnd( BuilderHandle, basicBlock.BlockHandle );
        }

        public void PositionBefore( Instruction instr )
        {
            NativeMethods.PositionBuilderBefore( BuilderHandle, instr.ValueHandle );
        }

        public Value FNeg( Value value ) => FNeg( value, string.Empty );

        public Value FNeg( Value value, string name ) => BuildUnaryOp( NativeMethods.BuildFNeg, value, name );

        public Value FAdd( Value lhs, Value rhs ) => FAdd( lhs, rhs, string.Empty );

        public Value FAdd( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildFAdd, lhs, rhs, name );

        public Value FSub( Value lhs, Value rhs ) => FSub( lhs, rhs, string.Empty );

        public Value FSub( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildFSub, lhs, rhs, name );

        public Value FMul( Value lhs, Value rhs ) => FMul( lhs, rhs, string.Empty );

        public Value FMul( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildFMul, lhs, rhs, name );

        public Value FDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildFDiv, lhs, rhs, string.Empty );

        public Value FDiv( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildFDiv, lhs, rhs, name );

        public Value Neg( Value value ) => Neg( value, string.Empty );

        public Value Neg( Value value, string name ) => BuildUnaryOp( NativeMethods.BuildNeg, value, name );

        public Value Not( Value value ) => Not( value, string.Empty );

        public Value Not( Value value, string name ) => BuildUnaryOp( NativeMethods.BuildNot, value, name );

        public Value Add( Value lhs, Value rhs ) => Add( lhs, rhs, string.Empty );

        public Value Add( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildAdd, lhs, rhs, name );

        public Value And( Value lhs, Value rhs ) => And( lhs, rhs, string.Empty );

        public Value And( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildAnd, lhs, rhs, name );

        public Value Sub( Value lhs, Value rhs ) => Sub( lhs, rhs, string.Empty );

        public Value Sub( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildSub, lhs, rhs, name );

        public Value Mul( Value lhs, Value rhs ) => Mul( lhs, rhs, string.Empty );

        public Value Mul( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildMul, lhs, rhs, name );

        public Value ShiftLeft( Value lhs, Value rhs ) => ShiftLeft( lhs, rhs, string.Empty );

        public Value ShiftLeft( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildShl, lhs, rhs, name );

        public Value ArithmeticShiftRight( Value lhs, Value rhs ) => ArithmeticShiftRight( lhs, rhs, string.Empty );

        public Value ArithmeticShiftRight( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildAShr, lhs, rhs, name );

        public Value LogicalShiftRight( Value lhs, Value rhs ) => LogicalShiftRight( lhs, rhs, string.Empty );

        public Value LogicalShiftRight( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildLShr, lhs, rhs, name );

        public Value UDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs, string.Empty );

        public Value UDiv( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs, name );

        public Value SDiv( Value lhs, Value rhs ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs, string.Empty );

        public Value SDiv( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildUDiv, lhs, rhs, name );

        public Value URem( Value lhs, Value rhs ) => URem( lhs, rhs, string.Empty );

        public Value URem( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildURem, lhs, rhs, name );

        public Value SRem( Value lhs, Value rhs ) => SRem( lhs, rhs, string.Empty );

        public Value SRem( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildSRem, lhs, rhs, name );

        public Value Xor( Value lhs, Value rhs ) => Xor( lhs, rhs, string.Empty );

        public Value Xor( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildXor, lhs, rhs, name );

        public Value Or( Value lhs, Value rhs ) => Or( lhs, rhs, string.Empty );

        public Value Or( Value lhs, Value rhs, string name ) => BuildBinOp( NativeMethods.BuildOr, lhs, rhs, name );

        public Alloca Alloca( ITypeRef typeRef ) => Alloca( typeRef, string.Empty );

        public Alloca Alloca( ITypeRef typeRef, string name )
        {
            var handle = NativeMethods.BuildAlloca( BuilderHandle, typeRef.GetTypeRef(), name );
            return Value.FromHandle<Alloca>( handle );
        }

        public Alloca Alloca( ITypeRef typeRef, ConstantInt elements ) => Alloca( typeRef, elements, string.Empty );

        public Alloca Alloca( ITypeRef typeRef, ConstantInt elements, string name )
        {
            var instHandle = NativeMethods.BuildArrayAlloca( BuilderHandle, typeRef.GetTypeRef(), elements.ValueHandle, name );
            return Value.FromHandle<Alloca>( instHandle );
        }

        public Return Return( ) => Value.FromHandle<Return>( NativeMethods.BuildRetVoid( BuilderHandle ) );

        public Return Return( Value value ) => Value.FromHandle<Return>( NativeMethods.BuildRet( BuilderHandle, value.ValueHandle ) );

        public Call Call( Value func, params Value[ ] args ) => Call( string.Empty, func, ( IReadOnlyList<Value> )args );
        public Call Call( Value func, IReadOnlyList<Value> args ) => Call( string.Empty, func, args );
        public Call Call( string name, Value func, params Value[ ] args ) => Call( name, func, ( IReadOnlyList<Value> )args );
        public Call Call( string name, Value func, IReadOnlyList<Value> args )
        {
            LLVMValueRef hCall = BuildCall( name, func, args );
            return Value.FromHandle<Call>( hCall );
        }

        /// <summary>Builds an LLVM Store instruction</summary>
        /// <param name="value">Value to store in destination</param>
        /// <param name="destination">value for the destination</param>
        /// <returns><see cref="Instructions.Store"/> instruction</returns>
        /// <remarks>
        /// Since store targets memory the type of <paramref name="destination"/>
        /// must be a <see cref="IPointerType"/>. Furthermore, the element type of
        /// the pointer must match the type of <paramref name="value"/> Otherwise an
        /// <see cref="ArgumentException"/> is thrown.
        /// </remarks>
        public Store Store( Value value, Value destination )
        {
            var ptrType = destination.Type as IPointerType;
            if( ptrType == null )
                throw new ArgumentException( "Expected pointer value", nameof( destination ) );

            if( !ptrType.ElementType.Equals( value.Type )
             || ( value.Type.Kind == TypeKind.Integer && value.Type.IntegerBitWidth != ptrType.ElementType.IntegerBitWidth )
              )
            {
                throw new ArgumentException( string.Format( IncompatibleTypeMsgFmt, ptrType.ElementType, value.Type ) );
            }

            return Value.FromHandle<Store>( NativeMethods.BuildStore( BuilderHandle, value.ValueHandle, destination.ValueHandle ) );
        }

        public Load Load( Value sourcePtr ) => Value.FromHandle<Load>( NativeMethods.BuildLoad( BuilderHandle, sourcePtr.ValueHandle, string.Empty ) );

        /// <summary>Creates a <see cref="User"/> that accesses an element (field) of a structure</summary>
        /// <param name="pointer">Pointer to the structure to get an element from</param>
        /// <param name="index">Element index</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
        /// </returns>
        public Value GetStructElementPointer( Value pointer, uint index ) => GetStructElementPointer( pointer, index, string.Empty );

        /// <summary>Creates a <see cref="User"/> that accesses an element (field) of a structure</summary>
        /// <param name="pointer">pointer to the strucure to get an element from</param>
        /// <param name="index">element index</param>
        /// <param name="name">Name for the instruction</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
        /// </returns>
        public Value GetStructElementPointer( Value pointer, uint index, string name )
        {
            if( pointer.Type.Kind != TypeKind.Pointer )
                throw new ArgumentException( "Pointer value expected", nameof( pointer ) );

            var hRetVal = NativeMethods.BuildStructGEP( BuilderHandle, pointer.ValueHandle, index, name );
            return Value.FromHandle( hRetVal );
        }

        /// <summary>Creates a <see cref="User"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indeces for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
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
        public Value GetElementPtr( Value pointer, IEnumerable<Value> args ) => GetElementPtr( pointer, args, string.Empty );

        /// <summary>Creates a <see cref="User"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indeces for computing the resulting pointer</param>
        /// <param name="name">Name to give to the instruction</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
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
        public Value GetElementPtr( Value pointer, IEnumerable<Value> args, string name )
        {
            var llvmArgs = GetValidatedGEPArgs( pointer, args );
            var hRetVal = NativeMethods.BuildGEP( BuilderHandle, pointer.ValueHandle, out llvmArgs[ 0 ], ( uint )llvmArgs.Length, string.Empty );
            return Value.FromHandle( hRetVal );
        }

        /// <summary>Creates a <see cref="User"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indeces for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
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
        public Value GetElementPtrInBounds( Value pointer, params Value[ ] args ) => GetElementPtrInBounds( pointer, args, string.Empty );
        public Value GetElementPtrInBounds( Value pointer, string name, params Value[ ] args ) => GetElementPtrInBounds( pointer, args, name );

        /// <summary>Creates a <see cref="User"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indeces for computing the resulting pointer</param>
        /// <param name="name">Name for the instruction</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
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
        public Value GetElementPtrInBounds( Value pointer, IEnumerable<Value> args, string name )
        {
            var llvmArgs = GetValidatedGEPArgs( pointer, args );
            var hRetVal = NativeMethods.BuildInBoundsGEP( BuilderHandle, pointer.ValueHandle, out llvmArgs[ 0 ], ( uint )llvmArgs.Length, string.Empty );
            return Value.FromHandle( hRetVal );
        }

        /// <summary>Creates a <see cref="User"/> that accesses an element of a type referenced by a pointer</summary>
        /// <param name="pointer">pointer to get an element from</param>
        /// <param name="args">additional indeces for computing the resulting pointer</param>
        /// <returns>
        /// <para><see cref="User"/> for the member access. This is a User as LLVM may 
        /// optimize the expression to a <see cref="ConstantExpression"/> if it 
        /// can so the actual type of the result may be <see cref="ConstantExpression"/>
        /// or <see cref="Instructions.GetElementPtr"/>.</para>
        /// <para>Note that <paramref name="pointer"/> must be a pointer to a structure
        /// or an excpetion is thrown.</para>
        /// </returns>
        /// <remarks>
        /// For details on GetElementPointer (GEP) see http://llvm.org/docs/GetElementPtr.html. The
        /// basic gist is that the GEP instruction does not access memory, it only computes a pointer
        /// offset from a base. A common confusion is around the first index and what it means. For C
        /// and C++ programmers an expression like pFoo->bar seems to only have a single offset or
        /// index. However that is only sytactic sugar where the compiler implicitly hides the first
        /// index. That is, there is no difference between pFoo[0].bar and pFoo->bar except that the
        /// former makes the first index explicit. LLVM requires an explicit first index even if it is
        /// zero, in order to properly compute the offset for a given element in an aggregate type.
        /// </remarks>
        public Value ConstGetElementPtrInBounds( Value pointer, params Value[ ] args )
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
        public Value IntToPointer( Value intValue, IPointerType ptrType )
        {
            if( intValue is Constant )
                return Value.FromHandle( NativeMethods.ConstIntToPtr( intValue.ValueHandle, ptrType.GetTypeRef() ) );

            var hValue = NativeMethods.BuildIntToPtr( BuilderHandle, intValue.ValueHandle, ptrType.GetTypeRef(), string.Empty );
            return Value.FromHandle( hValue );
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
            if( ptrValue.Type.Kind != TypeKind.Pointer )
                throw new ArgumentException( "Expected a pointer value", nameof( ptrValue ) );

            if( intType.Kind != TypeKind.Integer )
                throw new ArgumentException( "Expected pointer to integral type", nameof( intType ) );

            if( ptrValue is Constant )
                return Value.FromHandle( NativeMethods.ConstPtrToInt( ptrValue.ValueHandle, intType.GetTypeRef() ) );

            var hValue = NativeMethods.BuildPtrToInt( BuilderHandle, ptrValue.ValueHandle, intType.GetTypeRef(), string.Empty );
            return Value.FromHandle( hValue );
        }

        public Branch Branch( BasicBlock target ) => Value.FromHandle<Branch>( NativeMethods.BuildBr( BuilderHandle, target.BlockHandle ) );

        public Branch Branch( Value ifCondition, BasicBlock thenTarget, BasicBlock elseTarget )
        {
            var branchHandle = NativeMethods.BuildCondBr( BuilderHandle, ifCondition.ValueHandle, thenTarget.BlockHandle, elseTarget.BlockHandle );
            return Value.FromHandle<Branch>( branchHandle );
        }

        /// <summary>Builds an Integer compare instruction</summary>
        /// <param name="predicate">Integer predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( IntPredicate predicate, Value lhs, Value rhs ) => Compare( predicate, lhs, rhs, string.Empty );

        /// <summary>Builds an Integer compare instruction</summary>
        /// <param name="predicate">Integer predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <param name="name">Name for the instructio</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( IntPredicate predicate, Value lhs, Value rhs, string name )
        {
            if( !lhs.Type.IsInteger )
                throw new ArgumentException( "Expecting an integer type", nameof( lhs ) );

            if( !rhs.Type.IsInteger )
                throw new ArgumentException( "Expecting an integer type", nameof( rhs ) );

            return Value.FromHandle( NativeMethods.BuildICmp( BuilderHandle, ( LLVMIntPredicate )predicate, lhs.ValueHandle, rhs.ValueHandle, name ) );
        }

        /// <summary>Builds a Floating point compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( RealPredicate predicate, Value lhs, Value rhs ) => Compare( predicate, lhs, rhs, string.Empty );

        /// <summary>Builds a Floating point compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <param name="name">Name for the instruction</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( RealPredicate predicate, Value lhs, Value rhs, string name )
        {
            if( !lhs.Type.IsFloatingPoint )
                throw new ArgumentException( "Expecting an integer type", nameof( lhs ) );

            if( !rhs.Type.IsFloatingPoint )
                throw new ArgumentException( "Expecting an integer type", nameof( rhs ) );

            return Value.FromHandle( NativeMethods.BuildFCmp( BuilderHandle, ( LLVMRealPredicate )predicate, lhs.ValueHandle, rhs.ValueHandle, name ) );
        }

        /// <summary>Builds a compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( Predicate predicate, Value lhs, Value rhs ) => Compare( predicate, lhs, rhs, string.Empty );

        /// <summary>Builds a compare instruction</summary>
        /// <param name="predicate">predicate for the comparison</param>
        /// <param name="lhs">Left hand side of the comparison</param>
        /// <param name="rhs">Right hand side of the comparison</param>
        /// <param name="name">Name for the instruction</param>
        /// <returns>Comparison instruction</returns>
        public Value Compare( Predicate predicate, Value lhs, Value rhs, string name )
        {
            if( predicate <= Predicate.LastFcmpPredicate )
                return Compare( ( RealPredicate )predicate, lhs, rhs, name );
            else if( predicate >= Predicate.FirstIcmpPredicate && predicate <= Predicate.LastIcmpPredicate )
                return Compare( ( IntPredicate )predicate, lhs, rhs, name );
            else
                throw new ArgumentOutOfRangeException( nameof( predicate ), $"'{predicate}' is not a valid value for a compare predicate" );
        }

        public Value ZeroExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            return ZeroExtendOrBitCast( valueRef, targetType, string.Empty );
        }

        public Value ZeroExtendOrBitCast( Value valueRef, ITypeRef targetType, string name )
        {
            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.Type == targetType )
                return valueRef;

            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstZExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildZExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value SignExtendOrBitCast( Value valueRef, ITypeRef targetType )
        {
            return SignExtendOrBitCast( valueRef, targetType, string.Empty );
        }

        public Value SignExtendOrBitCast( Value valueRef, ITypeRef targetType, string name )
        {
            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.Type == targetType )
                return valueRef;

            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstSExtOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildSExtOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value TruncOrBitCast( Value valueRef, ITypeRef targetType )
        {
            return TruncOrBitCast( valueRef, targetType, string.Empty );
        }

        public Value TruncOrBitCast( Value valueRef, ITypeRef targetType, string name )
        {
            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.Type == targetType )
                return valueRef;

            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstTruncOrBitCast( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildTruncOrBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }


        public Value ZeroExtend( Value valueRef, ITypeRef targetType )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstZExt( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildZExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), string.Empty ) );
        }

        public Value SignExtend( Value valueRef, ITypeRef targetType )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstSExt( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
            {
                var retValueRef = NativeMethods.BuildSExt( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), string.Empty );
                return Value.FromHandle( retValueRef );
            }
        }

        public Value BitCast( Value valueRef, ITypeRef targetType ) => BitCast( valueRef, targetType, string.Empty );

        public Value BitCast( Value valueRef, ITypeRef targetType, string name )
        {
            // short circuit cast to same type as it won't be a Constant or a BitCast
            if( valueRef.Type == targetType )
                return valueRef;

            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstBitCast( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildBitCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value IntCast( Value valueRef, ITypeRef targetType, bool isSigned ) => IntCast( valueRef, targetType, isSigned, string.Empty );

        public Value IntCast( Value valueRef, ITypeRef targetType, bool isSigned, string name )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstIntCast( valueRef.ValueHandle, targetType.GetTypeRef(), isSigned ) );
            else
                return Value.FromHandle( NativeMethods.BuildIntCast( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value Trunc( Value valueRef, ITypeRef targetType )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstTrunc( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildTrunc( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), string.Empty ) );
        }

        public Value SIToFPCast( Value valueRef, ITypeRef targetType ) => SIToFPCast( valueRef, targetType, string.Empty );

        public Value SIToFPCast( Value valueRef, ITypeRef targetType, string name )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstSIToFP( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildSIToFP( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value FPToUICast( Value valueRef, ITypeRef targetType ) => FPToUICast( valueRef, targetType, string.Empty );

        public Value FPToUICast( Value valueRef, ITypeRef targetType, string name )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstFPToUI( valueRef.ValueHandle, targetType.GetTypeRef() ) );
            else
                return Value.FromHandle( NativeMethods.BuildFPToUI( BuilderHandle, valueRef.ValueHandle, targetType.GetTypeRef(), name ) );
        }

        public Value FPExt( Value valueRef, ITypeRef toType, string name )
        {
            if( valueRef is Constant )
                return Value.FromHandle( NativeMethods.ConstFPExt( valueRef.ValueHandle, toType.GetTypeRef() ) );
            else
                return Cast.FromHandle<Value>( NativeMethods.BuildFPExt( BuilderHandle, valueRef.ValueHandle, toType.GetTypeRef(), name ) );
        }

        public PhiNode PhiNode( ITypeRef resultType ) => PhiNode( resultType, string.Empty );
        public PhiNode PhiNode( ITypeRef resultType, string name )
        {
            var handle = NativeMethods.BuildPhi( BuilderHandle, resultType.GetTypeRef(), string.Empty );
            return Value.FromHandle<PhiNode>( handle );
        }

        public Value ExtractValue( Value instance, uint index ) => ExtractValue( instance, index, string.Empty );
        public Value ExtractValue( Value instance, uint index, string name )
        {
            var hResult = NativeMethods.BuildExtractValue( BuilderHandle, instance.ValueHandle, index, name );
            return Value.FromHandle( hResult );
        }

        public Switch Switch( Value value, BasicBlock defaultCase, uint numCases )
        {
            return Value.FromHandle<Switch>( NativeMethods.BuildSwitch( BuilderHandle, value.ValueHandle, defaultCase.BlockHandle, numCases ) );
        }

        public Value DoNothing( Module module )
        {
            var func = module.GetFunction( Intrinsic.DoNothingName );
            if( func == null )
            {
                var ctx = module.Context;
                var signature = ctx.GetFunctionType( ctx.VoidType );
                func = module.AddFunction( Intrinsic.DoNothingName, signature );
            }

            var hCall = BuildCall( string.Empty, func );
            return Value.FromHandle( hCall );
        }

        public Value DebugTrap( Module module )
        {
            var func = module.GetFunction( Intrinsic.DebugTrapName );
            if( func == null )
            {
                var ctx = module.Context;
                var signature = ctx.GetFunctionType( ctx.VoidType );
                func = module.AddFunction( Intrinsic.DebugTrapName, signature );
            }

            LLVMValueRef args;
            var hCall = NativeMethods.BuildCall( BuilderHandle, func.ValueHandle, out args, 0U, string.Empty );
            return Value.FromHandle( hCall );
        }

        /// <summary>Builds a memcpy intrinsic call</summary>
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
        /// <param name="destination">Destination pointer of the memcpy</param>
        /// <param name="source">Source pointer of the memcpy</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy invovles volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memcpy</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memcpy instrinsic, this implementation will deduce the types from
        /// the provided values and generate a more specific call without the need to provide overloaded forms of this
        /// method and otherwise complicating the calling code.
        /// </remarks>
        public Value MemCpy( Module module, Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            var dstPtrType = destination.Type as IPointerType;
            if( dstPtrType == null )
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );

            var srcPtrType = source.Type as IPointerType;
            if( srcPtrType == null )
                throw new ArgumentException( "Pointer type expected", nameof( source ) );

            if( !len.Type.IsInteger )
                throw new ArgumentException( "Integer type expected", nameof( len ) );

            if( Context != module.Context )
                throw new ArgumentException( "Module and instruction builder must come from the same context" );

            if( !dstPtrType.ElementType.IsInteger )
            {
                dstPtrType = module.Context.Int8Type.CreatePointerType();
                destination = BitCast( destination, dstPtrType );
            }

            if( !srcPtrType.ElementType.IsInteger )
            {
                srcPtrType = module.Context.Int8Type.CreatePointerType();
                source = BitCast( destination, srcPtrType );
            }

            var intrinsicName = Instructions.MemCpy.GetIntrinsicNameForArgs( dstPtrType, srcPtrType, len.Type );
            var func = module.GetFunction( intrinsicName );
            if( func == null )
            {
                var signature = module.Context.GetFunctionType( module.Context.VoidType  // return
                                                   , dstPtrType
                                                   , srcPtrType
                                                   , len.Type
                                                   , module.Context.Int32Type // alignment
                                                   , module.Context.BoolType  // isVolatile
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
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
        /// <param name="destination">Destination pointer of the memcpy</param>
        /// <param name="source">Source pointer of the memcpy</param>
        /// <param name="len">length of the data to copy</param>
        /// <param name="align">Alignment of the data for the copy</param>
        /// <param name="isVolatile">Flag to indicate if the copy invovles volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memcpy</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memmov instrinsic, this implementation currently assumes the 
        /// single form defined by <see cref="Intrinsic.MemMoveName"/>, which matches the classic "C" style memmov
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code. 
        /// </remarks>
        public Value MemMove( Module module, Value destination, Value source, Value len, Int32 align, bool isVolatile )
        {
            //TODO: make this auto select the LLVM instrinsic signature like memcpy...
            if( !destination.Type.IsPointer )
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );

            if( !source.Type.IsPointer )
                throw new ArgumentException( "Pointer type expected", nameof( source ) );

            if( !len.Type.IsInteger )
                throw new ArgumentException( "Integer type expected", nameof( len ) );

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
        /// <param name="module">Module to add the declaration of the intrinsic to if it doesn't already exist</param>
        /// <param name="destination">Destination pointer of the memset</param>
        /// <param name="value">fill value for the memset</param>
        /// <param name="len">length of the data to fill</param>
        /// <param name="align">ALignment of the data for the fill</param>
        /// <param name="isVolatile">Flag to indicate if the fill invovles volatile data such as physical registers</param>
        /// <returns><see cref="Intrinsic"/> call for the memcpy</returns>
        /// <remarks>
        /// LLVM has many overloaded variants of the memcpy instrinsic, this implementation currently assumes the 
        /// single form defined by <see cref="Intrinsic.MemSetName"/>, which matches the classic "C" style memcpy
        /// function. However future implementations should be able to deduce the types from the provided values
        /// and generate a more specific call without changing any caller code. 
        /// </remarks>
        public Value MemSet( Module module, Value destination, Value value, Value len, Int32 align, bool isVolatile )
        {
            if( destination.Type.Kind != TypeKind.Pointer )
                throw new ArgumentException( "Pointer type expected", nameof( destination ) );

            if( value.Type.IntegerBitWidth != 8 )
                throw new ArgumentException( "8bit value expected", nameof( value ) );

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
            return InsertValue( aggValue, elementValue, index, string.Empty );
        }

        public Value InsertValue( Value aggValue, Value elementValue, uint index, string name )
        {
            var handle = NativeMethods.BuildInsertValue( BuilderHandle, aggValue.ValueHandle, elementValue.ValueHandle, index, name );
            return Value.FromHandle( handle );
        }

        #region Disposable Pattern
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            NativeMethods.DisposeBuilder( BuilderHandle );
        }

        ~InstructionBuilder( )
        {
            Dispose( false );
        }
        #endregion

        LLVMValueRef[ ] GetValidatedGEPArgs( Value pointer, IEnumerable<Value> args)
        {
            if( pointer.Type.Kind != TypeKind.Pointer )
                throw new ArgumentException( "Pointer value expected", nameof( pointer ) );

            if( args.Any( a => !a.Type.IsInteger ) )
                throw new ArgumentException( $"GEP index arguments must be integers" );

            LLVMValueRef[ ] llvmArgs = args.Select( a => a.ValueHandle ).ToArray( );
            if( llvmArgs.Length == 0 )
                throw new ArgumentException( "There must be at least one index argument", nameof( args ) );

            return llvmArgs;
        }

        internal LLVMBuilderRef BuilderHandle { get; }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a unary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildUnaryOp( Func<LLVMBuilderRef, LLVMValueRef, string, LLVMValueRef> opFactory
                                  , Value operand
                                  , string name
                                  )
        {
            var valueRef = opFactory( BuilderHandle,operand.ValueHandle, name );
            return Value.FromHandle( valueRef );
        }

        // LLVM will automatically perform constant folding, thus the result of applying
        // a binary operator instruction may actually be a constant value and not an instruction
        // this deals with that to produce a correct managed wrapper type
        private Value BuildBinOp( Func<LLVMBuilderRef, LLVMValueRef, LLVMValueRef, string, LLVMValueRef> opFactory
                                , Value lhs
                                , Value rhs
                                , string name
                                )
        {
            var valueRef = opFactory( BuilderHandle, lhs.ValueHandle, rhs.ValueHandle, name );
            return Value.FromHandle( valueRef );
        }

        private LLVMValueRef BuildCall( Value func, params Value[] args )
        {
            return BuildCall( string.Empty, func, args );
        }

        private LLVMValueRef BuildCall( string name, Value func )
        {
            return BuildCall( name, func, new List<Value>() );
        }

        private LLVMValueRef BuildCall( string name, Value func, IReadOnlyList<Value> args )
        {
            if( func == null )
                throw new ArgumentNullException( nameof( func ) );

            var funcPtrType = func.Type as IPointerType;
            if( funcPtrType == null )
                throw new ArgumentException( "Expected pointer to function", nameof( func ) );

            var elementType = funcPtrType.ElementType as FunctionType;
            if( elementType == null )
                throw new ArgumentException( "A pointer to a function is required for an indirect call", nameof( func ) );

            if( args.Count != elementType.ParameterTypes.Count )
                throw new ArgumentException( "Mismatched parameter count with call site", nameof( args ) );

            for(int i = 0; i < args.Count; ++i )
            {
                if( args[i].Type != elementType.ParameterTypes[ i ] )
                {
                    var msg = $"Call site argument type mismatch at index {i}; argType={args[ i ].Type}; signatureType={elementType.ParameterTypes[ i ]}";
                    System.Diagnostics.Debug.WriteLine( msg );
                    throw new ArgumentException( msg, nameof( args ) );
                }
            }

            LLVMValueRef[ ] llvmArgs = args.Select( v => v.ValueHandle ).ToArray( );
            int argCount = llvmArgs.Length;

            // must always provide at least one element for succesful marshaling/interop, but tell LLVM there are none
            if( argCount == 0 )
                llvmArgs = new LLVMValueRef[ 1 ];

            var hCall = NativeMethods.BuildCall( BuilderHandle, func.ValueHandle, out llvmArgs[ 0 ], ( uint )argCount, name );
            return hCall;
        }

        const string IncompatibleTypeMsgFmt = "Incompatible types: destination pointer must be of the same type as the value stored.\n"
                                            + "Types are:\n"
                                            + "\tDestination: {0}\n"
                                            + "\tValue: {1}";
    }
}

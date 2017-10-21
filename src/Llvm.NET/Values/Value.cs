// <copyright file="Value.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;
using Llvm.NET.Types;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Value</summary>
    /// <remarks>
    /// Value is the root of a hierarchy of types representing values
    /// in LLVM. Values (and derived classes) are never constructed
    /// directly with the new operator. Instead, they are produced by
    /// other classes in this library internally. This is because they
    /// are just wrappers around the LLVM-C API handles and must
    /// maintain the "uniqueing" semantics. (e.g. allowing reference
    /// equality for values that are fundamentally the same value)
    /// This is generally hidden in the internals of the Llvm.NET
    /// library so callers need not be concerned with the details
    /// but can rely on the expected behavior that two Value instances
    /// referring to the same actual value (i.e. a function) are actually
    /// the same .NET object as well within the same <see cref="Context"/>
    /// </remarks>
    [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Mapping factory creates child types from interop handles" )]
    public class Value
        : IExtensiblePropertyContainer
    {
        /// <summary>Gets or sets name of the value (if any)</summary>
        /// <remarks>
        /// <note type="note">
        /// LLVM will add a numeric suffix to the name set if a
        /// valaue with the name already exists. Thus, the name
        /// read from this property may not match what is set.
        /// </note>
        /// </remarks>
        public string Name
        {
            get => Context.IsDisposed ? string.Empty : LLVMGetValueName( ValueHandle );

            set => LLVMSetValueName( ValueHandle, value );
        }

        /// <summary>Gets a value indicating whether this value is Undefined</summary>
        public bool IsUndefined => LLVMIsUndef( ValueHandle );

        /// <summary>Gets a value indicating whether the Value represents the NULL value for the values type</summary>
        public bool IsNull => LLVMIsNull( ValueHandle );

        /// <summary>Gets the type of the value</summary>
        public ITypeRef NativeType => TypeRef.FromHandle( LLVMTypeOf( ValueHandle ) );

        public Context Context => NativeType.Context;

        public bool IsInstruction => LLVMGetValueIdAsKind( ValueHandle ) > ValueKind.Instruction;

        public bool IsFunction => LLVMGetValueIdAsKind( ValueHandle ) == ValueKind.Function;

        public bool IsCallSite
        {
            get
            {
                var kind = LLVMGetValueIdAsKind( ValueHandle );
                return (kind == ValueKind.Call) || (kind == ValueKind.Invoke);
            }
        }

        /// <summary>Generates a string representing the LLVM syntax of the value</summary>
        /// <returns>string version of the value formatted by LLVM</returns>
        public override string ToString( ) => LLVMPrintValueToString( ValueHandle );

        /// <summary>Replace all uses of a <see cref="Value"/> with another one</summary>
        /// <param name="other">New value</param>
        public void ReplaceAllUsesWith( Value other )
        {
            if( other == null )
            {
                throw new ArgumentNullException( nameof( other ) );
            }

            LLVMReplaceAllUsesWith( ValueHandle, other.ValueHandle );
        }

        /// <inheritdoc/>
        public bool TryGetExtendedPropertyValue<T>( string id, out T value ) => ExtensibleProperties.TryGetExtendedPropertyValue<T>( id, out value );

        /// <inheritdoc/>
        public void AddExtendedPropertyValue( string id, object value ) => ExtensibleProperties.AddExtendedPropertyValue( id, value );

        internal Value( LLVMValueRef valueRef )
        {
            if( valueRef.Pointer == IntPtr.Zero )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

#if DEBUG
            var context = Context.GetContextFor( valueRef );
            context.AssertValueNotInterned( valueRef );
#endif
            ValueHandle = valueRef;
        }

        internal LLVMValueRef ValueHandle { get; }

        /// <summary>Gets an Llvm.NET managed wrapper for a LibLLVM value handle</summary>
        /// <param name="valueRef">Value handle to wrap</param>
        /// <returns>LLVM.NET managed instance for the handle</returns>
        /// <remarks>
        /// This method uses a cached mapping to ensure that two calls given the same
        /// input handle returns the same managed instance so that reference equality
        /// works as expected.
        /// </remarks>
        internal static Value FromHandle( LLVMValueRef valueRef ) => FromHandle<Value>( valueRef );

        /// <summary>Gets an Llvm.NET managed wrapper for a LibLLVM value handle</summary>
        /// <typeparam name="T">Required type for the handle</typeparam>
        /// <param name="valueRef">Value handle to wrap</param>
        /// <returns>LLVM.NET managed instance for the handle</returns>
        /// <remarks>
        /// This method uses a cached mapping to ensure that two calls given the same
        /// input handle returns the same managed instance so that reference equality
        /// works as expected.
        /// </remarks>
        /// <exception cref="InvalidCastException">When the handle is for a different type of handle than specified by <typeparamref name="T"/></exception>
        internal static T FromHandle<T>( LLVMValueRef valueRef )
            where T : Value
        {
            var context = Context.GetContextFor( valueRef );
            return ( T )context.GetValueFor( valueRef, StaticFactory );
        }

        /// <summary>Central factory for creating instances of <see cref="Value"/> and all derived types</summary>
        /// <param name="h">LibLLVM handle for the value</param>
        /// <returns>New Value or derived type instance that wraps the underlying LibLLVM handle</returns>
        /// <remarks>
        /// This method will determine the correct type for the handle and construct an instance of that
        /// type wrapping the handle.
        /// </remarks>
        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Factory that maps wrappers with underlying types" )]
        [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Factory that maps wrappers with underlying types" )]
        private static Value StaticFactory( LLVMValueRef h )
        {
            var kind = LLVMGetValueIdAsKind( h );
            switch( kind )
            {
            case ValueKind.Argument:
                return new Argument( h );

            case ValueKind.BasicBlock:
                return new BasicBlock( h );

            case ValueKind.Function:
                return new Function( h );

            case ValueKind.GlobalAlias:
                return new GlobalAlias( h );

            case ValueKind.GlobalVariable:
                return new GlobalVariable( h );

            case ValueKind.UndefValue:
                return new UndefValue( h );

            case ValueKind.BlockAddress:
                return new BlockAddress( h );

            case ValueKind.ConstantExpr:
                return new ConstantExpression( h );

            case ValueKind.ConstantAggregateZero:
                return new ConstantAggregateZero( h );

            case ValueKind.ConstantDataArray:
                return new ConstantDataArray( h );

            case ValueKind.ConstantDataVector:
                return new ConstantDataVector( h );

            case ValueKind.ConstantInt:
                return new ConstantInt( h );

            case ValueKind.ConstantFP:
                return new ConstantFP( h );

            case ValueKind.ConstantArray:
                return new ConstantArray( h );

            case ValueKind.ConstantStruct:
                return new ConstantStruct( h );

            case ValueKind.ConstantVector:
                return new ConstantVector( h );

            case ValueKind.ConstantPointerNull:
                return new ConstantPointerNull( h );

            case ValueKind.MetadataAsValue:
                return new MetadataAsValue( h );

            case ValueKind.InlineAsm:
                return new InlineAsm( h );

            case ValueKind.Instruction:
                throw new ArgumentException( "Value with kind==Instruction is not valid" );

            case ValueKind.Return:
                return new Instructions.ReturnInstruction( h );

            case ValueKind.Branch:
                return new Instructions.Branch( h );

            case ValueKind.Switch:
                return new Instructions.Switch( h );

            case ValueKind.IndirectBranch:
                return new Instructions.IndirectBranch( h );

            case ValueKind.Invoke:
                return new Instructions.Invoke( h );

            case ValueKind.Unreachable:
                return new Instructions.Unreachable( h );

            case ValueKind.Add:
            case ValueKind.FAdd:
            case ValueKind.Sub:
            case ValueKind.FSub:
            case ValueKind.Mul:
            case ValueKind.FMul:
            case ValueKind.UDiv:
            case ValueKind.SDiv:
            case ValueKind.FDiv:
            case ValueKind.URem:
            case ValueKind.SRem:
            case ValueKind.FRem:
            case ValueKind.Shl:
            case ValueKind.LShr:
            case ValueKind.AShr:
            case ValueKind.And:
            case ValueKind.Or:
            case ValueKind.Xor:
                return new Instructions.BinaryOperator( h );

            case ValueKind.Alloca:
                return new Instructions.Alloca( h );

            case ValueKind.Load:
                return new Instructions.Load( h );

            case ValueKind.Store:
                return new Instructions.Store( h );

            case ValueKind.GetElementPtr:
                return new Instructions.GetElementPtr( h );

            case ValueKind.Trunc:
                return new Instructions.Trunc( h );

            case ValueKind.ZeroExtend:
                return new Instructions.ZeroExtend( h );

            case ValueKind.SignExtend:
                return new Instructions.SignExtend( h );

            case ValueKind.FPToUI:
                return new Instructions.FPToUI( h );

            case ValueKind.FPToSI:
                return new Instructions.FPToSI( h );

            case ValueKind.UIToFP:
                return new Instructions.UIToFP( h );

            case ValueKind.SIToFP:
                return new Instructions.SIToFP( h );

            case ValueKind.FPTrunc:
                return new Instructions.FPTrunc( h );

            case ValueKind.FPExt:
                return new Instructions.FPExt( h );

            case ValueKind.PtrToInt:
                return new Instructions.PointerToInt( h );

            case ValueKind.IntToPtr:
                return new Instructions.IntToPointer( h );

            case ValueKind.BitCast:
                return new Instructions.BitCast( h );

            case ValueKind.AddrSpaceCast:
                return new Instructions.AddressSpaceCast( h );

            case ValueKind.ICmp:
                return new Instructions.IntCmp( h );

            case ValueKind.FCmp:
                return new Instructions.FCmp( h );

            case ValueKind.Phi:
                return new Instructions.PhiNode( h );

            case ValueKind.Call:
                return new Instructions.CallInstruction( h );

            case ValueKind.Select:
                return new Instructions.Select( h );

            case ValueKind.UserOp1:
                return new Instructions.UserOp1( h );

            case ValueKind.UserOp2:
                return new Instructions.UserOp2( h );

            case ValueKind.VaArg:
                return new Instructions.VaArg( h );

            case ValueKind.ExtractElement:
                return new Instructions.ExtractElement( h );

            case ValueKind.InsertElement:
                return new Instructions.InsertElement( h );

            case ValueKind.ShuffleVector:
                return new Instructions.ShuffleVector( h );

            case ValueKind.ExtractValue:
                return new Instructions.ExtractValue( h );

            case ValueKind.InsertValue:
                return new Instructions.InsertValue( h );

            case ValueKind.Fence:
                return new Instructions.Fence( h );

            case ValueKind.AtomicCmpXchg:
                return new Instructions.AtomicCmpXchg( h );

            case ValueKind.AtomicRMW:
                return new Instructions.AtomicRMW( h );

            case ValueKind.Resume:
                return new Instructions.ResumeInstruction( h );

            case ValueKind.LandingPad:
                return new Instructions.LandingPad( h );

            case ValueKind.CleanUpReturn:
                return new Instructions.CleanupReturn( h );

            case ValueKind.CatchReturn:
                return new Instructions.CatchReturn( h );

            case ValueKind.CatchPad:
                return new Instructions.CatchPad( h );

            case ValueKind.CleanupPad:
                return new Instructions.CleanupPad( h );

            case ValueKind.CatchSwitch:
                return new Instructions.CatchSwitch( h );

            default:
                if( kind >= ValueKind.ConstantFirstVal && kind <= ValueKind.ConstantLastVal )
                {
                    return new Constant( h );
                }

                return kind > ValueKind.Instruction ? new Instructions.Instruction( h ) : new Value( h );
            }
        }

        private readonly ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}

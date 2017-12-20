﻿// <copyright file="Value.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
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

        /// <summary>Gets the context for this value</summary>
        public Context Context => NativeType.Context;

        /// <summary>Gets a value indicating whether the Value is an instruction</summary>
        public bool IsInstruction => LLVMGetValueIdAsKind( ValueHandle ) > ValueKind.Instruction;

        /// <summary>Gets a value indicating whether the Value is a function</summary>
        public bool IsFunction => LLVMGetValueIdAsKind( ValueHandle ) == ValueKind.Function;

        /// <summary>Gets a value indicating whether the Value is a call-site</summary>
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
            if( valueRef == default )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

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
            var context = GetContextFor( valueRef );
            return ( T )context.GetValueFor( valueRef );
        }

        internal class InterningFactory
            : HandleInterningMap<LLVMValueRef, Value>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Factory that maps wrappers with underlying types" )]
            [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Factory that maps wrappers with underlying types" )]
            private protected override Value ItemFactory( LLVMValueRef handle )
            {
                var handleContext = GetContextFor( handle );
                if( handleContext != Context )
                {
                    throw new ArgumentException( "Context for the handle provided doesn't match the context for this cache", nameof( handle ) );
                }

                var kind = LLVMGetValueIdAsKind( handle );
                switch( kind )
                {
                case ValueKind.Argument:
                    return new Argument( handle );

                case ValueKind.BasicBlock:
                    return new BasicBlock( handle );

                case ValueKind.Function:
                    return new Function( handle );

                case ValueKind.GlobalAlias:
                    return new GlobalAlias( handle );

                case ValueKind.GlobalVariable:
                    return new GlobalVariable( handle );

                case ValueKind.GlobalIFunc:
                    return new GlobalIFunc( handle );

                case ValueKind.UndefValue:
                    return new UndefValue( handle );

                case ValueKind.BlockAddress:
                    return new BlockAddress( handle );

                case ValueKind.ConstantExpr:
                    return new ConstantExpression( handle );

                case ValueKind.ConstantAggregateZero:
                    return new ConstantAggregateZero( handle );

                case ValueKind.ConstantDataArray:
                    return new ConstantDataArray( handle );

                case ValueKind.ConstantDataVector:
                    return new ConstantDataVector( handle );

                case ValueKind.ConstantInt:
                    return new ConstantInt( handle );

                case ValueKind.ConstantFP:
                    return new ConstantFP( handle );

                case ValueKind.ConstantArray:
                    return new ConstantArray( handle );

                case ValueKind.ConstantStruct:
                    return new ConstantStruct( handle );

                case ValueKind.ConstantVector:
                    return new ConstantVector( handle );

                case ValueKind.ConstantPointerNull:
                    return new ConstantPointerNull( handle );

                case ValueKind.MetadataAsValue:
                    return new MetadataAsValue( handle );

                case ValueKind.InlineAsm:
                    return new InlineAsm( handle );

                case ValueKind.Instruction:
                    throw new ArgumentException( "Value with kind==Instruction is not valid" );

                case ValueKind.Return:
                    return new Instructions.ReturnInstruction( handle );

                case ValueKind.Branch:
                    return new Instructions.Branch( handle );

                case ValueKind.Switch:
                    return new Instructions.Switch( handle );

                case ValueKind.IndirectBranch:
                    return new Instructions.IndirectBranch( handle );

                case ValueKind.Invoke:
                    return new Instructions.Invoke( handle );

                case ValueKind.Unreachable:
                    return new Instructions.Unreachable( handle );

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
                    return new Instructions.BinaryOperator( handle );

                case ValueKind.Alloca:
                    return new Instructions.Alloca( handle );

                case ValueKind.Load:
                    return new Instructions.Load( handle );

                case ValueKind.Store:
                    return new Instructions.Store( handle );

                case ValueKind.GetElementPtr:
                    return new Instructions.GetElementPtr( handle );

                case ValueKind.Trunc:
                    return new Instructions.Trunc( handle );

                case ValueKind.ZeroExtend:
                    return new Instructions.ZeroExtend( handle );

                case ValueKind.SignExtend:
                    return new Instructions.SignExtend( handle );

                case ValueKind.FPToUI:
                    return new Instructions.FPToUI( handle );

                case ValueKind.FPToSI:
                    return new Instructions.FPToSI( handle );

                case ValueKind.UIToFP:
                    return new Instructions.UIToFP( handle );

                case ValueKind.SIToFP:
                    return new Instructions.SIToFP( handle );

                case ValueKind.FPTrunc:
                    return new Instructions.FPTrunc( handle );

                case ValueKind.FPExt:
                    return new Instructions.FPExt( handle );

                case ValueKind.PtrToInt:
                    return new Instructions.PointerToInt( handle );

                case ValueKind.IntToPtr:
                    return new Instructions.IntToPointer( handle );

                case ValueKind.BitCast:
                    return new Instructions.BitCast( handle );

                case ValueKind.AddrSpaceCast:
                    return new Instructions.AddressSpaceCast( handle );

                case ValueKind.ICmp:
                    return new Instructions.IntCmp( handle );

                case ValueKind.FCmp:
                    return new Instructions.FCmp( handle );

                case ValueKind.Phi:
                    return new Instructions.PhiNode( handle );

                case ValueKind.Call:
                    return new Instructions.CallInstruction( handle );

                case ValueKind.Select:
                    return new Instructions.Select( handle );

                case ValueKind.UserOp1:
                    return new Instructions.UserOp1( handle );

                case ValueKind.UserOp2:
                    return new Instructions.UserOp2( handle );

                case ValueKind.VaArg:
                    return new Instructions.VaArg( handle );

                case ValueKind.ExtractElement:
                    return new Instructions.ExtractElement( handle );

                case ValueKind.InsertElement:
                    return new Instructions.InsertElement( handle );

                case ValueKind.ShuffleVector:
                    return new Instructions.ShuffleVector( handle );

                case ValueKind.ExtractValue:
                    return new Instructions.ExtractValue( handle );

                case ValueKind.InsertValue:
                    return new Instructions.InsertValue( handle );

                case ValueKind.Fence:
                    return new Instructions.Fence( handle );

                case ValueKind.AtomicCmpXchg:
                    return new Instructions.AtomicCmpXchg( handle );

                case ValueKind.AtomicRMW:
                    return new Instructions.AtomicRMW( handle );

                case ValueKind.Resume:
                    return new Instructions.ResumeInstruction( handle );

                case ValueKind.LandingPad:
                    return new Instructions.LandingPad( handle );

                case ValueKind.CleanUpReturn:
                    return new Instructions.CleanupReturn( handle );

                case ValueKind.CatchReturn:
                    return new Instructions.CatchReturn( handle );

                case ValueKind.CatchPad:
                    return new Instructions.CatchPad( handle );

                case ValueKind.CleanupPad:
                    return new Instructions.CleanupPad( handle );

                case ValueKind.CatchSwitch:
                    return new Instructions.CatchSwitch( handle );

                // Default to constant, Instruction or generic base Value
                default:
                    if( kind >= ValueKind.ConstantFirstVal && kind <= ValueKind.ConstantLastVal )
                    {
                        return new Constant( handle );
                    }

                    return kind > ValueKind.Instruction ? new Instructions.Instruction( handle ) : new Value( handle );
                }
            }
        }

        private static Context GetContextFor( LLVMValueRef valueRef )
        {
            if( valueRef == default )
            {
                return null;
            }

            var hType = LLVMTypeOf( valueRef );
            Debug.Assert( hType != default, "Should not get a null pointer from LLVM" );
            var type = TypeRef.FromHandle( hType );
            return type.Context;
        }

        private readonly ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}

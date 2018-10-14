// <copyright file="ValueCache.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Llvm.NET.Instructions;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    internal class ValueCache
        : IHandleInterning<LLVMValueRef, Value>
    {
        public Context Context { get; }

        public Value GetOrCreateItem( LLVMValueRef valueRef )
        {
            IntPtr managedHandlePtr = LLVMValueCacheLookup( Handle, valueRef );
            if( managedHandlePtr != IntPtr.Zero )
            {
                return (Value)GCHandle.FromIntPtr( managedHandlePtr ).Target;
            }

            Value instance = CreateValueInstance( valueRef );
            var managedHandle = GCHandle.Alloc( instance );
            LLVMValueCacheAdd( Handle, valueRef, (IntPtr)managedHandle );
            ValueRefToGCHandleMap[ valueRef ] = managedHandle;
            return instance;
        }

        public void Remove( LLVMValueRef handle )
        {
            throw new NotSupportedException("Cannot remove items from the ValueCache, they are tracked automatically internally");
        }

        public void Clear( )
        {
            throw new NotSupportedException( "Cannot remove items from the ValueCache, they are tracked automatically internally" );
        }

        public IEnumerator<Value> GetEnumerator( )
        {
            return ValueRefToGCHandleMap.Values.Select( gch => ( Value )gch.Target ).GetEnumerator( );
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal ValueCache( Context context )
        {
            Context = context;

            // methods used as cross P/Invoke callbacks so the delegates must remain alive for duration
            WrappedOnDeleted = new WrappedNativeCallback( new LLVMValueCacheItemDeletedCallback( OnItemDeleted ) );
            WrappedOnReplaced = new WrappedNativeCallback( new LLVMValueCacheItemReplacedCallback( OnItemReplaced ) );

            Handle = LLVMCreateValueCache( WrappedOnDeleted.NativeFuncPtr, WrappedOnReplaced.NativeFuncPtr );
        }

        private void OnItemDeleted( LLVMValueRef valueRef, IntPtr handle )
        {
            ValueRefToGCHandleMap[ valueRef ].Free( );
            ValueRefToGCHandleMap.Remove( valueRef );
        }

        private IntPtr OnItemReplaced( LLVMValueRef valueRef, IntPtr handle, LLVMValueRef newValue )
        {
            var managedHandle = GCHandle.Alloc( CreateValueInstance( newValue ) );
            ValueRefToGCHandleMap[ valueRef ] = managedHandle;
            return (IntPtr)managedHandle;
        }

        private WrappedNativeCallback WrappedOnDeleted;
        private WrappedNativeCallback WrappedOnReplaced;

        private readonly Dictionary<LLVMValueRef,GCHandle> ValueRefToGCHandleMap = new Dictionary<LLVMValueRef, GCHandle>();

        private readonly LLVMValueCacheRef Handle;

        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Factory that maps wrappers with underlying types" )]
        [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Factory that maps wrappers with underlying types" )]
        private Value CreateValueInstance( LLVMValueRef handle )
        {
            var handleContext = handle.GetContext();
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
                return new ReturnInstruction( handle );

            case ValueKind.Branch:
                return new Branch( handle );

            case ValueKind.Switch:
                return new Switch( handle );

            case ValueKind.IndirectBranch:
                return new IndirectBranch( handle );

            case ValueKind.Invoke:
                return new Invoke( handle );

            case ValueKind.Unreachable:
                return new Unreachable( handle );

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
                return new BinaryOperator( handle );

            case ValueKind.Alloca:
                return new Alloca( handle );

            case ValueKind.Load:
                return new Load( handle );

            case ValueKind.Store:
                return new Store( handle );

            case ValueKind.GetElementPtr:
                return new GetElementPtr( handle );

            case ValueKind.Trunc:
                return new Trunc( handle );

            case ValueKind.ZeroExtend:
                return new ZeroExtend( handle );

            case ValueKind.SignExtend:
                return new SignExtend( handle );

            case ValueKind.FPToUI:
                return new FPToUI( handle );

            case ValueKind.FPToSI:
                return new FPToSI( handle );

            case ValueKind.UIToFP:
                return new UIToFP( handle );

            case ValueKind.SIToFP:
                return new SIToFP( handle );

            case ValueKind.FPTrunc:
                return new FPTrunc( handle );

            case ValueKind.FPExt:
                return new FPExt( handle );

            case ValueKind.PtrToInt:
                return new PointerToInt( handle );

            case ValueKind.IntToPtr:
                return new IntToPointer( handle );

            case ValueKind.BitCast:
                return new BitCast( handle );

            case ValueKind.AddrSpaceCast:
                return new AddressSpaceCast( handle );

            case ValueKind.ICmp:
                return new IntCmp( handle );

            case ValueKind.FCmp:
                return new FCmp( handle );

            case ValueKind.Phi:
                return new PhiNode( handle );

            case ValueKind.Call:
                return new CallInstruction( handle );

            case ValueKind.Select:
                return new Select( handle );

            case ValueKind.UserOp1:
                return new UserOp1( handle );

            case ValueKind.UserOp2:
                return new UserOp2( handle );

            case ValueKind.VaArg:
                return new VaArg( handle );

            case ValueKind.ExtractElement:
                return new ExtractElement( handle );

            case ValueKind.InsertElement:
                return new InsertElement( handle );

            case ValueKind.ShuffleVector:
                return new ShuffleVector( handle );

            case ValueKind.ExtractValue:
                return new ExtractValue( handle );

            case ValueKind.InsertValue:
                return new InsertValue( handle );

            case ValueKind.Fence:
                return new Fence( handle );

            case ValueKind.AtomicCmpXchg:
                return new AtomicCmpXchg( handle );

            case ValueKind.AtomicRMW:
                return new AtomicRMW( handle );

            case ValueKind.Resume:
                return new ResumeInstruction( handle );

            case ValueKind.LandingPad:
                return new LandingPad( handle );

            case ValueKind.CleanUpReturn:
                return new CleanupReturn( handle );

            case ValueKind.CatchReturn:
                return new CatchReturn( handle );

            case ValueKind.CatchPad:
                return new CatchPad( handle );

            case ValueKind.CleanupPad:
                return new CleanupPad( handle );

            case ValueKind.CatchSwitch:
                return new CatchSwitch( handle );

            // Default to constant, Instruction or generic base Value
            default:
                if( kind >= ValueKind.ConstantFirstVal && kind <= ValueKind.ConstantLastVal )
                {
                    return new Constant( handle );
                }

                return kind > ValueKind.Instruction ? new Instruction( handle ) : new Value( handle );
            }
        }

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate void LLVMValueCacheItemDeletedCallback( LLVMValueRef valueRef, IntPtr handle );

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate IntPtr LLVMValueCacheItemReplacedCallback( LLVMValueRef valueRef, IntPtr handle, LLVMValueRef newValue );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern LLVMValueCacheRef LLVMCreateValueCache( IntPtr deletedCallback, IntPtr replacedCallback );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern void LLVMValueCacheAdd( LLVMValueCacheRef cacheRef, LLVMValueRef value, IntPtr handle );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
        private static extern IntPtr LLVMValueCacheLookup( LLVMValueCacheRef cacheRef, LLVMValueRef valueRef );
    }
}

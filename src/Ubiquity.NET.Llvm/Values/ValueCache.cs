// -----------------------------------------------------------------------
// <copyright file="ValueCache.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Values
{
    // Wrapper around extended native code to allow mapping the projected types to an LLVM owned type that supports
    // llvm's `Replace All Uses With` (RAUW) operations, which is otherwise not possible in managed code.
    [SuppressMessage( "Maintainability", "CA1506:Avoid excessive class coupling", Justification = "Interop projection factory" )]
    internal class ValueCache
        : DisposableObject
        , IHandleInterning<LLVMValueRef, Value>
    {
        public Context Context { get; }

        public Value GetOrCreateItem( LLVMValueRef valueRef, Action<LLVMValueRef>? foundHandleRelease = null )
        {
            nint managedHandlePtr = LibLLVMValueCacheLookup( Handle, valueRef );
            if( managedHandlePtr != nint.Zero )
            {
                foundHandleRelease?.Invoke( valueRef );
                return ( Value )(GCHandle.FromIntPtr( managedHandlePtr ).Target ?? throw new InvalidOperationException( "GC handle returned a null target!" ));
            }

            Value instance = CreateValueInstance( valueRef );
            var managedHandle = GCHandle.Alloc( instance );
            LibLLVMValueCacheAdd( Handle, valueRef, ( nint )managedHandle );
            ValueRefToGCHandleMap[ valueRef ] = managedHandle;
            return instance;
        }

        public void Remove( LLVMValueRef handle )
        {
            throw new NotSupportedException( "Cannot remove items from the ValueCache, they are tracked automatically internally" );
        }

        public void Clear( )
        {
            throw new NotSupportedException( "Cannot remove items from the ValueCache, they are tracked automatically internally" );
        }

        public IEnumerator<Value> GetEnumerator( )
        {
            var q = from gch in ValueRefToGCHandleMap.Values
                    select (Value)(gch.Target ?? throw new InvalidOperationException("GC handle returned a null target!"));

            return q.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal ValueCache( Context context )
        {
            Context = context;

            // methods used as cross P/Invoke callbacks so the delegates must remain alive for duration
            NativeCallbackContext = GCHandle.Alloc(this);
            unsafe
            {
                Handle = LibLLVMCreateValueCache( GCHandle.ToIntPtr(NativeCallbackContext), &NativeOnItemDeleted, &NativeOnItemReplaced );
            }
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                Handle.Dispose( );
                NativeCallbackContext.Free();
            }
        }

        private void OnItemDeleted( LLVMValueRef valueRef, nint handle )
        {
            ValueRefToGCHandleMap[ valueRef ].Free( );
            ValueRefToGCHandleMap.Remove( valueRef );
        }

        private nint OnItemReplaced( LLVMValueRef valueRef, nint handle, LLVMValueRef newValue )
        {
            var managedHandle = GCHandle.Alloc( CreateValueInstance( newValue ) );
            ValueRefToGCHandleMap[ valueRef ] = managedHandle;
            return ( nint )managedHandle;
        }

        private readonly GCHandle NativeCallbackContext;
        private readonly Dictionary<LLVMValueRef,GCHandle> ValueRefToGCHandleMap = [];

        private readonly LibLLVMValueCacheRef Handle;

        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Factory that maps wrappers with underlying types" )]
        [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Factory that maps wrappers with underlying types" )]
        private Value CreateValueInstance( LLVMValueRef handle )
        {
            var handleContext = handle.GetContext();
            if( handleContext != Context )
            {
                throw new ArgumentException( Resources.Context_for_the_handle_provided_doesn_t_match_the_context_for_this_cache, nameof( handle ) );
            }

            var kind = LibLLVMGetValueKind( handle );
            switch( kind )
            {
            case LibLLVMValueKind.ArgumentKind:
                return new Argument( handle );

            case LibLLVMValueKind.BasicBlockKind:
                return new BasicBlock( handle );

            case LibLLVMValueKind.FunctionKind:
                return new Function( handle );

            case LibLLVMValueKind.GlobalAliasKind:
                return new GlobalAlias( handle );

            case LibLLVMValueKind.GlobalVariableKind:
                return new GlobalVariable( handle );

            case LibLLVMValueKind.GlobalIFuncKind:
                return new GlobalIFunc( handle );

            case LibLLVMValueKind.UndefValueKind:
                return new UndefValue( handle );

            case LibLLVMValueKind.BlockAddressKind:
                return new BlockAddress( handle );

            case LibLLVMValueKind.ConstantExprKind:
                return new ConstantExpression( handle );

            case LibLLVMValueKind.ConstantAggregateZeroKind:
                return new ConstantAggregateZero( handle );

            case LibLLVMValueKind.ConstantDataArrayKind:
                return new ConstantDataArray( handle );

            case LibLLVMValueKind.ConstantDataVectorKind:
                return new ConstantDataVector( handle );

            case LibLLVMValueKind.ConstantIntKind:
                return new ConstantInt( handle );

            case LibLLVMValueKind.ConstantFPKind:
                return new ConstantFP( handle );

            case LibLLVMValueKind.ConstantArrayKind:
                return new ConstantArray( handle );

            case LibLLVMValueKind.ConstantStructKind:
                return new ConstantStruct( handle );

            case LibLLVMValueKind.ConstantVectorKind:
                return new ConstantVector( handle );

            case LibLLVMValueKind.ConstantPointerNullKind:
                return new ConstantPointerNull( handle );

            case LibLLVMValueKind.MetadataAsValueKind:
                return new MetadataAsValue( handle );

            case LibLLVMValueKind.InlineAsmKind:
                return new InlineAsm( handle );

            case LibLLVMValueKind.InstructionKind:
                throw new ArgumentException( "Value with kind==InstructionKind is not valid" );

            case LibLLVMValueKind.RetKind:
                return new ReturnInstruction( handle );

            case LibLLVMValueKind.BrKind:
                return new Branch( handle );

            case LibLLVMValueKind.SwitchKind:
                return new Instructions.Switch( handle );

            case LibLLVMValueKind.IndirectBrKind:
                return new IndirectBranch( handle );

            case LibLLVMValueKind.InvokeKind:
                return new Invoke( handle );

            case LibLLVMValueKind.UnreachableKind:
                return new Unreachable( handle );

            case LibLLVMValueKind.AddKind:
            case LibLLVMValueKind.FAddKind:
            case LibLLVMValueKind.SubKind:
            case LibLLVMValueKind.FSubKind:
            case LibLLVMValueKind.MulKind:
            case LibLLVMValueKind.FMulKind:
            case LibLLVMValueKind.UDivKind:
            case LibLLVMValueKind.SDivKind:
            case LibLLVMValueKind.FDivKind:
            case LibLLVMValueKind.URemKind:
            case LibLLVMValueKind.SRemKind:
            case LibLLVMValueKind.FRemKind:
            case LibLLVMValueKind.ShlKind:
            case LibLLVMValueKind.LShrKind:
            case LibLLVMValueKind.AShrKind:
            case LibLLVMValueKind.AndKind:
            case LibLLVMValueKind.OrKind:
            case LibLLVMValueKind.XorKind:
                return new BinaryOperator( handle );

            case LibLLVMValueKind.AllocaKind:
                return new Alloca( handle );

            case LibLLVMValueKind.LoadKind:
                return new Load( handle );

            case LibLLVMValueKind.StoreKind:
                return new Store( handle );

            case LibLLVMValueKind.GetElementPtrKind:
                return new GetElementPtr( handle );

            case LibLLVMValueKind.TruncKind:
                return new Trunc( handle );

            case LibLLVMValueKind.ZExtKind:
                return new ZeroExtend( handle );

            case LibLLVMValueKind.SExtKind:
                return new SignExtend( handle );

            case LibLLVMValueKind.FPToUIKind:
                return new FPToUI( handle );

            case LibLLVMValueKind.FPToSIKind:
                return new FPToSI( handle );

            case LibLLVMValueKind.UIToFPKind:
                return new UIToFP( handle );

            case LibLLVMValueKind.SIToFPKind:
                return new SIToFP( handle );

            case LibLLVMValueKind.FPTruncKind:
                return new FPTrunc( handle );

            case LibLLVMValueKind.FPExtKind:
                return new FPExt( handle );

            case LibLLVMValueKind.PtrToIntKind:
                return new PointerToInt( handle );

            case LibLLVMValueKind.IntToPtrKind:
                return new IntToPointer( handle );

            case LibLLVMValueKind.BitCastKind:
                return new BitCast( handle );

            case LibLLVMValueKind.AddrSpaceCastKind:
                return new AddressSpaceCast( handle );

            case LibLLVMValueKind.ICmpKind:
                return new IntCmp( handle );

            case LibLLVMValueKind.FCmpKind:
                return new FCmp( handle );

            case LibLLVMValueKind.PHIKind:
                return new PhiNode( handle );

            case LibLLVMValueKind.CallKind:
                return new CallInstruction( handle );

            case LibLLVMValueKind.SelectKind:
                return new SelectInstruction( handle );

            case LibLLVMValueKind.UserOp1Kind:
                return new UserOp1( handle );

            case LibLLVMValueKind.UserOp2Kind:
                return new UserOp2( handle );

            case LibLLVMValueKind.VAArgKind:
                return new VaArg( handle );

            case LibLLVMValueKind.ExtractElementKind:
                return new ExtractElement( handle );

            case LibLLVMValueKind.InsertElementKind:
                return new InsertElement( handle );

            case LibLLVMValueKind.ShuffleVectorKind:
                return new ShuffleVector( handle );

            case LibLLVMValueKind.ExtractValueKind:
                return new ExtractValue( handle );

            case LibLLVMValueKind.InsertValueKind:
                return new InsertValue( handle );

            case LibLLVMValueKind.FenceKind:
                return new Fence( handle );

            case LibLLVMValueKind.AtomicCmpXchgKind:
                return new AtomicCmpXchg( handle );

            case LibLLVMValueKind.AtomicRMWKind:
                return new AtomicRMW( handle );

            case LibLLVMValueKind.ResumeKind:
                return new ResumeInstruction( handle );

            case LibLLVMValueKind.LandingPadKind:
                return new LandingPad( handle );

            case LibLLVMValueKind.CleanupRetKind:
                return new CleanupReturn( handle );

            case LibLLVMValueKind.CatchRetKind:
                return new CatchReturn( handle );

            case LibLLVMValueKind.CatchPadKind:
                return new CatchPad( handle );

            case LibLLVMValueKind.CleanupPadKind:
                return new CleanupPad( handle );

            case LibLLVMValueKind.CatchSwitchKind:
                return new CatchSwitch( handle );

            // Default to constant, Instruction or generic base Value
            default:
                if( kind >= LibLLVMValueKind.ConstantFirstValKind && kind <= LibLLVMValueKind.ConstantLastValKind )
                {
                    return new Constant( handle );
                }

                return kind > LibLLVMValueKind.InstructionKind ? new Instruction( handle ) : new Value( handle );
            }
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static void NativeOnItemDeleted(nint context, nint valRef, nint handle)
        {
            try
            {
                if (GCHandle.FromIntPtr( context ).Target is ValueCache self)
                {
                    self.OnItemDeleted(LLVMValueRef.FromABI(valRef), handle);
                }
            }
            catch
            {
            }
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static nint NativeOnItemReplaced(nint context, nint valRef, nint handle, nint newValRef)
        {
            try
            {
                return GCHandle.FromIntPtr( context ).Target is not ValueCache self
                    ? nint.Zero
                    : self.OnItemReplaced(LLVMValueRef.FromABI(valRef), handle, LLVMValueRef.FromABI(newValRef));
            }
            catch
            {
                return nint.Zero;
            }
        }
    }
}

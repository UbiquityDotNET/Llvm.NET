// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Provides extension methods to <see cref="Value"/> that cannot be achieved as members of the class</summary>
    /// <remarks>
    /// Using generic static extension methods allows for fluent coding while retaining the type of the "this" parameter.
    /// If these were members of the <see cref="Value"/> class then the only return type could be <see cref="Value"/>,
    /// thus losing the original type and requiring a cast to get back to it.
    /// </remarks>
    public static class ValueExtensions
    {
        private const LibLLVMValueKind ConstantFirstValKind = LibLLVMValueKind.FunctionKind;
        private const LibLLVMValueKind ConstantLastValKind = LibLLVMValueKind.ConstantTokenNoneKind;

        //private const LibLLVMValueKind ConstantDataFirstValKind = LibLLVMValueKind.UndefValueKind;
        //private const LibLLVMValueKind ConstantDataLastValKind = LibLLVMValueKind.ConstantTokenNoneKind;
        //private const LibLLVMValueKind ConstantAggregateFirstValKind = LibLLVMValueKind.ConstantArrayKind;
        //private const LibLLVMValueKind ConstantAggregateLastValKind = LibLLVMValueKind.ConstantVectorKind;

        /// <summary>Sets the virtual register name for a value</summary>
        /// <typeparam name="T"> Type of the value to set the name for</typeparam>
        /// <param name="value">Value to set register name for</param>
        /// <param name="name">Name for the virtual register the value represents</param>
        /// <remarks>
        /// <para>Technically speaking only an <see cref="Instructions.Instruction"/> can have register name
        /// information. However, since LLVM will perform constant folding in the <see cref="Instructions.InstructionBuilder"/>
        /// most of the methods in <see cref="Instructions.InstructionBuilder"/> return a <see cref="Value"/> rather
        /// than a more specific <see cref="Instructions.Instruction"/>. Thus, without this extension method here,
        /// code would need to know ahead of time that an actual instruction would be produced then cast the result
        /// to an <see cref="Instructions.Instruction"/> and then set the debug location. This makes the code rather
        /// ugly and tedious to manage. Placing this as a generic extension method ensures that the return type matches
        /// the original and no additional casting is needed, which would defeat the purpose of doing this. For
        /// <see cref="Value"/> types that are not instructions this does nothing. This allows for a simpler fluent
        /// style of programming where the actual type is retained even in cases where an <see cref="Instructions.InstructionBuilder"/>
        /// method will always return an actual instruction.</para>
        /// <para>Since the <see cref="Value.Name"/> property is available on all <see cref="Value"/>s this is slightly
        /// redundant. It is useful for maintaining the fluent style of coding along with expressing intent more clearly.
        /// (e.g. using this makes it expressly clear that the intent is to set the virtual register name and not the
        /// name of a local variable etc...) Using the fluent style allows a significant reduction in the number of
        /// overloaded methods in <see cref="Instructions.InstructionBuilder"/> to account for all variations with or without a name.
        /// </para>
        /// </remarks>
        /// <returns><paramref name="value"/> for fluent usage</returns>
        public static T RegisterName<T>( this T value, string name )
            where T : Value
        {
            if(value is Instruction)
            {
                value.Name = name;
            }

            return value;
        }

        internal static ContextAlias GetContext( this LLVMValueRef valueRef )
        {
            return valueRef == default
                ? throw new ArgumentException( "Value ref is null", nameof( valueRef ) )
                : new( LLVMGetTypeContext( LLVMTypeOf( valueRef ) ) );
        }

        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Factory that maps wrappers with underlying types" )]
        [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Factory that maps wrappers with underlying types" )]
        internal static Value CreateValueInstance( this LLVMValueRef handle )
        {
            var kind = LibLLVMGetValueKind( handle );
            switch(kind)
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
                if(kind >= ConstantFirstValKind && kind <= ConstantLastValKind)
                {
                    return new Constant( handle );
                }

                return kind > LibLLVMValueKind.InstructionKind ? new Instruction( handle ) : new Value( handle );
            }
        }
    }
}

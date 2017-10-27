// <copyright file="ConstantExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Instructions;
using Llvm.NET.Native;
using Llvm.NET.Types;

namespace Llvm.NET.Values
{
    /// <summary>While technically a type in LLVM, ConstantExpression is primarily a static factory for Constants</summary>
    public class ConstantExpression
        : Constant
    {
        public OpCode OpCode => ( OpCode )NativeMethods.LLVMGetConstOpcode( ValueHandle );

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static Constant IntToPtrExpression( Constant value, ITypeRef type )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( value.NativeType.Kind != TypeKind.Integer )
            {
                throw new ArgumentException( "Integer Type expected", nameof( value ) );
            }

            if( !( type is IPointerType ) )
            {
                throw new ArgumentException( "pointer type expected", nameof( type ) );
            }

            return FromHandle<Constant>( NativeMethods.LLVMConstIntToPtr( value.ValueHandle, type.GetTypeRef( ) ) );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static Constant BitCast( Constant value, ITypeRef toType )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            var handle = NativeMethods.LLVMConstBitCast( value.ValueHandle, toType.GetTypeRef( ) );
            return FromHandle<Constant>( handle );
        }

        public static Constant GetElementPtr( Constant value, params Constant[ ] args )
            => GetElementPtr( value, ( IEnumerable<Constant> )args );

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call")]
        public static Constant GetElementPtr(Constant value, IEnumerable<Constant> args)
        {
            var llvmArgs = InstructionBuilder.GetValidatedGEPArgs(value, args);
            var handle = NativeMethods.LLVMConstGEP( value.ValueHandle, out llvmArgs[0], (uint)llvmArgs.Length);
            return FromHandle<Constant>(handle);
        }

        internal ConstantExpression( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

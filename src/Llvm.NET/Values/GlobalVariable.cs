// <copyright file="GlobalVariable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.DebugInfo;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Global Variable</summary>
    public class GlobalVariable
        : GlobalObject
    {
        /// <summary>Gets or sets a value indicating whether this variable is initialized in an external module</summary>
        public bool IsExternallyInitialized
        {
            get => NativeMethods.LLVMIsExternallyInitialized( ValueHandle );
            set => NativeMethods.LLVMSetExternallyInitialized( ValueHandle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is a Constant</summary>
        public bool IsConstant
        {
            get => NativeMethods.LLVMIsGlobalConstant( ValueHandle );
            set => NativeMethods.LLVMSetGlobalConstant( ValueHandle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is stored per thread</summary>
        public bool IsThreadLocal
        {
            get => NativeMethods.LLVMIsThreadLocal( ValueHandle );
            set => NativeMethods.LLVMSetThreadLocal( ValueHandle, value );
        }

        /// <summary>Gets or sets the initial value for the variable</summary>
        public Constant Initializer
        {
            get
            {
                var handle = NativeMethods.LLVMGetInitializer( ValueHandle );
                if( handle.Pointer == IntPtr.Zero )
                {
                    return null;
                }

                return FromHandle<Constant>( handle );
            }

            set => NativeMethods.LLVMSetInitializer( ValueHandle, value?.ValueHandle ?? new LLVMValueRef( IntPtr.Zero ) );
        }

        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop" )]
        public void AddDebugInfo(DIGlobalVariableExpression expression)
        {
            expression.ValidateNotNull( nameof( expression ) );

            NativeMethods.LLVMGlobalVariableAddDebugExpression( ValueHandle, expression.MetadataHandle );
        }

        /// <summary>Removes the value from its parent module, but does not delete it</summary>
        public void RemoveFromParent() => NativeMethods.LLVMRemoveGlobalFromParent( ValueHandle );

        internal GlobalVariable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

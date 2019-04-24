// <copyright file="GlobalVariable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.DebugInfo;
using Llvm.NET.Interop;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Global Variable</summary>
    public class GlobalVariable
        : GlobalObject
    {
        /// <summary>Gets or sets a value indicating whether this variable is initialized in an external module</summary>
        public bool IsExternallyInitialized
        {
            get => LLVMIsExternallyInitialized( ValueHandle );
            set => LLVMSetExternallyInitialized( ValueHandle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is a Constant</summary>
        public bool IsConstant
        {
            get => LLVMIsGlobalConstant( ValueHandle );
            set => LLVMSetGlobalConstant( ValueHandle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is stored per thread</summary>
        public bool IsThreadLocal
        {
            get => LLVMIsThreadLocal( ValueHandle );
            set => LLVMSetThreadLocal( ValueHandle, value );
        }

        /// <summary>Gets or sets the initial value for the variable</summary>
        public Constant Initializer
        {
            get
            {
                var handle = LLVMGetInitializer( ValueHandle );
                if( handle == default )
                {
                    return null;
                }

                return FromHandle<Constant>( handle );
            }

            set => LLVMSetInitializer( ValueHandle, value?.ValueHandle ?? LLVMValueRef.Zero );
        }

        /// <summary>Adds a <see cref="DIGlobalVariableExpression"/> for a <see cref="GlobalVariable"/></summary>
        /// <param name="expression">Expression to add</param>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop" )]
        public void AddDebugInfo(DIGlobalVariableExpression expression)
        {
            expression.ValidateNotNull( nameof( expression ) );

            LLVMGlobalVariableAddDebugExpression( ValueHandle, expression.MetadataHandle );
        }

        /// <summary>Removes the value from its parent module, but does not delete it</summary>
        public void RemoveFromParent() => LLVMRemoveGlobalFromParent( ValueHandle );

        internal GlobalVariable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

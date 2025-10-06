// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>An LLVM Global Variable</summary>
    public class GlobalVariable
        : GlobalObject
    {
        /// <summary>Gets or sets a value indicating whether this variable is initialized in an external module</summary>
        public bool IsExternallyInitialized
        {
            get => LLVMIsExternallyInitialized( Handle );
            set => LLVMSetExternallyInitialized( Handle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is a Constant</summary>
        public bool IsConstant
        {
            get => LLVMIsGlobalConstant( Handle );
            set => LLVMSetGlobalConstant( Handle, value );
        }

        /// <summary>Gets or sets a value indicating whether this global is stored per thread</summary>
        public bool IsThreadLocal
        {
            get => LLVMIsThreadLocal( Handle );
            set => LLVMSetThreadLocal( Handle, value );
        }

        /// <summary>Gets or sets the initial value for the variable</summary>
        public Constant? Initializer
        {
            get
            {
                var handle = LLVMGetInitializer( Handle );
                return handle == default ? null : FromHandle<Constant>( handle );
            }

            set => LLVMSetInitializer( Handle, value?.Handle ?? default );
        }

        /// <summary>Adds a <see cref="DIGlobalVariableExpression"/> for a <see cref="GlobalVariable"/></summary>
        /// <param name="expression">Expression to add</param>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop" )]
        public void AddDebugInfo( DIGlobalVariableExpression expression )
        {
            ArgumentNullException.ThrowIfNull( expression );

            LibLLVMGlobalVariableAddDebugExpression( Handle, expression.Handle );
        }

        /// <summary>Removes the value from its parent module, but does not delete it</summary>
        public void RemoveFromParent( ) => LibLLVMRemoveGlobalFromParent( Handle );

        internal GlobalVariable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

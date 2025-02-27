// -----------------------------------------------------------------------
// <copyright file="GlobalVariable.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.ArgValidators;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Values
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
        public Constant? Initializer
        {
            get
            {
                var handle = LLVMGetInitializer( ValueHandle );
                return handle == default ? null : FromHandle<Constant>( handle );
            }

            set => LLVMSetInitializer( ValueHandle, value?.ValueHandle ?? LLVMValueRef.Zero );
        }

        /// <summary>Adds a <see cref="DIGlobalVariableExpression"/> for a <see cref="GlobalVariable"/></summary>
        /// <param name="expression">Expression to add</param>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop" )]
        public void AddDebugInfo( DIGlobalVariableExpression expression )
        {
            expression.ValidateNotNull( nameof( expression ) );

            LibLLVMGlobalVariableAddDebugExpression( ValueHandle, expression.MetadataHandle );
        }

        /// <summary>Removes the value from its parent module, but does not delete it</summary>
        public void RemoveFromParent( ) => LibLLVMRemoveGlobalFromParent( ValueHandle );

        internal GlobalVariable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

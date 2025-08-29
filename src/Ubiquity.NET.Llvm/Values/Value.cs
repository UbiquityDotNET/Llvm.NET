// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>LLVM Value</summary>
    /// <remarks>
    /// Value is the root of a hierarchy of types representing values in LLVM. Values (and derived classes)
    /// are never constructed directly with the new operator. Instead, they are produced by other classes
    /// in this library internally. This is because they are just wrappers around the LLVM-C API handles
    /// and must maintain the "uniqueing" semantics. (e.g. allowing reference equality for values that are
    /// fundamentally the same value). This is generally hidden in the internals of the Ubiquity.NET.Llvm
    /// library so that callers need not be concerned with the details but can rely on the expected behavior
    /// that two <see cref="Value"/> instances referring to the same actual value (i.e. a function) are
    /// actually the same .NET object as well (within the same <see cref="Ubiquity.NET.Llvm.ContextAlias"/>)
    /// </remarks>
    public class Value
        : IEquatable<Value>
    {
        #region IEquatable<Value>

        /// <inheritdoc/>
        public bool Equals( Value? other ) => other is not null && Handle.Equals( other.Handle );

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => Equals( obj as Value );

        /// <inheritdoc/>
        public override int GetHashCode( ) => Handle.GetHashCode();
        #endregion

        /// <summary>Gets or sets name of the value (if any)</summary>
        /// <remarks>
        /// <note type="note">
        /// LLVM will add a numeric suffix to the name set if a value with the name already exists.
        /// Thus, the name read from this property may not match what is set.
        /// </note>
        /// The value may not have a name, thus it is possible to get a <see langword="null"/> value
        /// for this property. It is never valid to set the Name to <see langword="null"/>.
        /// </remarks>
        [DisallowNull]
        public LazyEncodedString? Name
        {
            get => LLVMGetValueName2( Handle )!;
            set
            {
                ArgumentNullException.ThrowIfNull( value );
                LLVMSetValueName2( Handle, value );
            }
        }

        /// <summary>Gets a value indicating whether this value is Undefined</summary>
        public bool IsUndefined => LLVMIsUndef( Handle );

        /// <summary>Gets a value indicating whether the Value represents the NULL value for the values type</summary>
        public bool IsNull => LLVMIsNull( Handle );

        /// <summary>Gets the type of the value</summary>
        public ITypeRef NativeType => LLVMTypeOf( Handle ).CreateType();

        /// <summary>Gets the context for this value</summary>
        public IContext Context => NativeType.Context;

        /// <summary>Gets a value indicating whether the Value is an instruction</summary>
        public bool IsInstruction => LibLLVMGetValueKind( Handle ) > LibLLVMValueKind.InstructionKind;

        /// <summary>Gets a value indicating whether the Value is a function</summary>
        public bool IsFunction => LibLLVMGetValueKind( Handle ) == LibLLVMValueKind.FunctionKind;

        /// <summary>Gets a value indicating whether the Value is a call-site</summary>
        public bool IsCallSite
        {
            get
            {
                var kind = LibLLVMGetValueKind( Handle );
                return (kind == LibLLVMValueKind.CallKind) || (kind == LibLLVMValueKind.InvokeKind);
            }
        }

        /// <summary>Generates a string representing the LLVM syntax of the value</summary>
        /// <returns>string version of the value formatted by LLVM</returns>
        public override string? ToString( )
        {
            return LLVMPrintValueToString( Handle );
        }

        /// <summary>Replace all uses of a <see cref="Value"/> with another one</summary>
        /// <param name="other">New value</param>
        public void ReplaceAllUsesWith( Value other )
        {
            ArgumentNullException.ThrowIfNull( other );
            LLVMReplaceAllUsesWith( Handle, other.Handle );
        }

        internal Value( LLVMValueRef valueRef )
        {
            if(valueRef == default)
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            Handle = valueRef;
        }

        internal LLVMValueRef Handle { get; }

        /// <summary>Gets an Ubiquity.NET.Llvm managed wrapper for a LibLLVM value handle</summary>
        /// <param name="valueRef">Value handle to wrap</param>
        /// <returns>Ubiquity.NET.Llvm managed instance for the handle</returns>
        internal static Value? FromHandle( LLVMValueRef valueRef ) => FromHandle<Value>( valueRef );

        /// <summary>Gets an Ubiquity.NET.Llvm managed wrapper for a LibLLVM value handle</summary>
        /// <typeparam name="T">Required type for the handle</typeparam>
        /// <param name="valueRef">Value handle to wrap</param>
        /// <returns>Ubiquity.NET.Llvm managed instance for the handle</returns>
        /// <exception cref="InvalidCastException">When the handle is for a different type of handle than specified by <typeparamref name="T"/></exception>
        internal static T? FromHandle<T>( LLVMValueRef valueRef )
            where T : Value
        {
            return valueRef.CreateValueInstance() as T;
        }
    }
}

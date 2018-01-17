// <copyright file="Value.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
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
        /// value with the name already exists. Thus, the name
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
            var context = valueRef.GetContext( );

            return ( T )context.GetValueFor( valueRef );
        }

        private readonly ExtensiblePropertyContainer ExtensibleProperties = new ExtensiblePropertyContainer( );
    }
}

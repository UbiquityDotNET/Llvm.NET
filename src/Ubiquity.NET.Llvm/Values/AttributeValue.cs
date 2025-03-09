// -----------------------------------------------------------------------
// <copyright file="AttributeValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Single attribute for functions, function returns and function parameters</summary>
    /// <remarks>
    /// This is the equivalent to the underlying llvm::AttributeImpl class. The name was changed to
    /// AttributeValue in .NET to prevent confusion with the standard <see cref="Attribute"/> class
    /// that is used throughout .NET libraries.
    /// </remarks>
    public readonly record struct AttributeValue
    {
        /// <summary>Gets the context that owns this <see cref="AttributeValue"/></summary>
        public Context Context { get; }

        /// <summary>Gets the kind of the attribute</summary>
        /// <value>The unique ID for the named attribute</value>
        /// <remarks>
        /// The ID is unique for the current runtime but is NOT guaranteed stable across runtime or LLVM version (even minor patches really).
        /// Applications **MUST NOT*** assume these values have any consistency beyond the current run and should NOT persist them. Additionally,
        /// string attributes have NO id.
        /// </remarks>
        public AttributeKind Id => LLVMIsStringAttribute( NativeAttribute )
                                 ? AttributeKind.None
                                 : (AttributeKind)LLVMGetEnumAttributeKind( NativeAttribute );

        /// <summary>Gets the Name of the attribute</summary>
        public string Name
            => IsString
                ? LLVMGetStringAttributeKind( NativeAttribute, out uint _ ) ?? string.Empty
                : LibLLVMGetEnumAttributeKindName(NativeAttribute).ToString() ?? string.Empty;

        /// <summary>Gets the value for named attributes with values</summary>
        /// <value>The value as a string or <see lang="null"/> if the attribute has no value</value>
        public string? StringValue => IsString ? LLVMGetStringAttributeValue( NativeAttribute, out uint _ ) : null;

        /// <summary>Gets the Integer value of the attribute or 0 if the attribute doesn't have a value</summary>
        public UInt64 IntegerValue => IsEnum ? LLVMGetEnumAttributeValue( NativeAttribute ) : 0;

        /// <summary>Gets the Type value of this attribute, if any</summary>
        public ITypeRef? TypeValue => IsType ? TypeRef.FromHandle( LLVMGetTypeAttributeValue( NativeAttribute ) ) : null;

        /// <summary>Gets a value indicating whether this attribute is a target specific string value</summary>
        public bool IsString => LLVMIsStringAttribute( NativeAttribute );

        /// <summary>Gets a value indicating whether this attribute is a simple enumeration value</summary>
        public bool IsEnum => LLVMIsEnumAttribute( NativeAttribute );

        /// <summary>Gets a value indicating whether this attribute has a type value</summary>
        public bool IsType => LLVMIsTypeAttribute( NativeAttribute );

        /// <summary>Verifies the attribute is valid for a <see cref="Value"/> on a given <see cref="FunctionAttributeIndex"/></summary>
        /// <param name="index">Index to verify</param>
        /// <param name="value">Value to check this attribute on</param>
        /// <exception cref="ArgumentException">The attribute is not valid on <paramref name="value"/> for the <paramref name="index"/></exception>
        public void VerifyValidOn( FunctionAttributeIndex index, Value value )
        {
            ArgumentNullException.ThrowIfNull( value );

            // TODO: Attributes on globals??
            if( !( value.IsCallSite || value.IsFunction ) )
            {
                throw new ArgumentException( "Attributes only allowed on functions and call sites" );
            }

            // for now all string attributes are valid everywhere as they are target dependent
            // (e.g. no way to verify the validity of an arbitrary attribute without knowing the target)
            if( IsString || IsType )
            {
                return;
            }

            Id.VerifyAttributeUsage( index, value );
        }

        /// <summary>Gets a string representation of the attribute</summary>
        /// <returns>Attribute as a string</returns>
        public override string? ToString( )
        {
            return LibLLVMAttributeToString( NativeAttribute ).ToString();
        }

        internal static AttributeValue FromHandle( Context context, LLVMAttributeRef handle )
        {
            return context.GetAttributeFor( handle );
        }

        internal LLVMAttributeRef NativeAttribute { get; }

        internal class InterningFactory
            : HandleInterningMapWithContext<LLVMAttributeRef, AttributeValue>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            private protected override AttributeValue ItemFactory( LLVMAttributeRef handle )
            {
                return new AttributeValue( Context, handle );
            }
        }

        private AttributeValue( Context context, LLVMAttributeRef nativeValue )
        {
            ArgumentNullException.ThrowIfNull( context );
            Context = context;
            NativeAttribute = nativeValue;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="AttributeValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

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
        /// <summary>Gets the kind of the attribute</summary>
        /// <value>The unique ID for the named attribute</value>
        /// <remarks>
        /// The ID is unique for the current runtime but is NOT guaranteed stable across runtime or LLVM version (even minor patches really).
        /// Applications **MUST NOT*** assume these values have any consistency beyond the current run and should NOT persist them. Additionally,
        /// string attributes have NO id.
        /// </remarks>
        public UInt32 Id => LLVMIsStringAttribute( NativeAttribute ) ? 0 : LLVMGetEnumAttributeKind( NativeAttribute );

        /// <summary>Gets the Name of the attribute</summary>
        public LazyEncodedString Name
            => IsString
             ? LLVMGetStringAttributeKind( NativeAttribute ) ?? LazyEncodedString.Empty
             : LibLLVMGetAttributeNameFromID( Id ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the value for named attributes with values</summary>
        /// <value>The value as a string or <see lang="null"/> if the attribute has no value</value>
        public LazyEncodedString? StringValue => !IsString ? null : LLVMGetStringAttributeValue( NativeAttribute );

        /// <summary>Gets the Integer value of the attribute or 0 if the attribute doesn't have a value</summary>
        public UInt64 IntegerValue => IsInt ? LLVMGetEnumAttributeValue( NativeAttribute ) : 0;

        /// <summary>Gets the Type value of this attribute, if any</summary>
        public ITypeRef? TypeValue => IsType ? LLVMGetTypeAttributeValue( NativeAttribute ).CreateType() : null;

        /// <summary>Gets a value indicating whether this attribute is a target specific string value</summary>
        public bool IsString => LLVMIsStringAttribute( NativeAttribute );

        /// <summary>Gets a value indicating whether this attribute is a simple enumeration value [No arg value]</summary>
        public bool IsEnum => AttributeInfo.ArgKind == AttributeArgKind.None;

        /// <summary>Gets a value indicating whether this attribute has an integral ID AND requires an integer arg value</summary>
        public bool IsInt => AttributeInfo.ArgKind == AttributeArgKind.Int;

        /// <summary>Gets a value indicating whether this attribute has an integral ID AND requires an Type arg value</summary>
        public bool IsType => LLVMIsTypeAttribute( NativeAttribute );

        /// <summary>Gets the <see cref="AttributeInfo"/> for this value</summary>
        public AttributeInfo AttributeInfo => AttributeInfo.From(Name);

        /// <summary>Gets a string representation of the attribute</summary>
        /// <returns>Attribute as a string</returns>
        public override string? ToString( ) => LibLLVMAttributeToString( NativeAttribute );
        internal AttributeValue( LLVMAttributeRef nativeValue )
        {
            NativeAttribute = nativeValue;
        }

        internal LLVMAttributeRef NativeAttribute { get; }
    }
}

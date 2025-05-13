using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Kind of argument (if any) for an attribute</summary>
    public enum AttributeArgKind
    {
        /// <summary>No argument for this attribute</summary>
        None = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_None,

        /// <summary>Attribute has a single integral value <see cref="UInt64"/></summary>
        Int = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Int,

        /// <summary>Attribute has a single type value</summary>
        Type = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_Type,

        /// <summary>Attribute has a constant range value</summary>
        ConstantRange = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_ConstantRange,

        /// <summary>Attribute has a list of constant ranges as the value</summary>
        ConstantRangeList = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_ConstantRangeList,

        /// <summary>Attribute name is a string AND has a string value</summary>
        /// <remarks>
        /// The string value may have further constraints such as it is a Boolean with
        /// the string value having the literal 'true' or 'false' [Case sensitive].
        /// </remarks>
        String = LibLLVMAttributeArgKind.LibLLVMAttributeArgKind_String,
    };

    /// <summary>Flags enumeration of places where a given attribute is applicable</summary>
    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI naming" )]
    public enum AttributeAllowedOn
    {
        /// <summary>Attribute has no allowed options</summary>
        /// <remarks>This is a default invalid value</remarks>
        None = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_None,

        /// <summary>Attribute is applicable to a function return</summary>
        Return = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Return,

        /// <summary>Attribute is applicable to a parameter</summary>
        Parameter = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Parameter,

        /// <summary>Attribute is applicable to a function</summary>
        Function = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Function,

        /// <summary>Attribute is applicable to a call site</summary>
        CallSite = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_CallSite,

        /// <summary>Attribute is applicable to a global</summary>
        Global = LibLLVMAttributeAllowedOn.LibLLVMAttributeAllowedOn_Global,
    };

    /// <summary>Information regarding an attribute</summary>
    /// <remarks>
    /// If <see cref="ArgKind"/> is <see cref="AttributeArgKind.String"/> then <see cref="ID"/>
    /// is 0. There is no enumerated value ID for string attributes. Additionally, <see cref="AllowedOn"/>
    /// indicates it is allowed everywhere. This may or may not be true, sadly there is no known way
    /// to determine applicability of a string attribute.
    /// </remarks>
    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct AttributeInfo
    {
        /// <summary>Gets the ID value for this attribute in this runtime</summary>
        /// <remarks>
        /// <note type="Important">The names and ID values of attributes is subject to change
        /// from release to release. Therefore, apps should not rely on any particular value
        /// for the ID of an attribute. In fact, they should assume this value will change from
        /// run to run of the same application and treat is as completely opaque.</note>
        /// </remarks>
        public UInt32 ID { get; }

        /// <summary>Gets a value indicating the kind of argument for this attribute</summary>
        public AttributeArgKind ArgKind { get; }

        /// <summary>Gets a value indicating where the attribute is allowed</summary>
        /// <remarks>
        /// <note type="warning">
        /// String attributes are special in that they have no way to determine applicability.
        /// Therefore, this will indicate they are valid everywhere even if they are not. Sadly,
        /// the underlying LLVM API does not provide a means to determine such a thing.</note>
        /// </remarks>
        public AttributeAllowedOn AllowedOn { get; }

        // To maintain layout compatibility with native code requirements NOTHING else
        // may declare any fields for this struct.

        /// <summary>Gets a value indicating whether this instance is invalid</summary>
        public bool IsInvalid => this == default
                                 || LLVMGetLastEnumAttributeKind() < ID;

        /// <summary>Gets a value indicating whether this instance is valid</summary>
        public bool IsValid => !IsInvalid;

        /// <summary>Gets a value indicating whether this attribute is a string</summary>
        public bool IsString => ID == 0 && ArgKind == AttributeArgKind.String;

        /// <summary>Gets a value indicating whether this attribute has an argument</summary>
        public bool HasArg => ArgKind != AttributeArgKind.None;

        /// <summary>Gets information about an attribute from the name</summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns>Information on the attribute</returns>
        /// <exception cref="LlvmException">Attribute name is not known</exception>
        public static AttributeInfo From( LazyEncodedString name )
        {
            // maps directly to native struct, so use it instead of dealing with translation overhead
            AttributeInfo retVal;
            unsafe
            {
                using var errorRef = LibLLVMGetAttributeInfo( name, (LibLLVMAttributeInfo*)&retVal );
                errorRef.ThrowIfFailed();
            }
            return retVal;
        }
    }
}

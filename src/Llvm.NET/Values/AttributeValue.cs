using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Values
{
    /// <summary>Single attribute for functions, function returns and function parameters</summary>
    /// <remarks>
    /// This is the equivalent to the underlying llvm::Attribute class. The name was changed to 
    /// AttributeValue in .NET to prevent confusion with the <see cref="System.Attribute"/> class
    /// that is used throughout .NET libraries. As with the enderlying LLVM type, this is an 
    /// immutable value type. 
    /// </remarks>
    public struct AttributeValue
    {
        /// <summary>Creates a simple boolean attribute</summary>
        /// <param name="ctx">Context for creating the attribute</param>
        /// <param name="kind">Kind of attribute</param>
        public AttributeValue( Context ctx, AttributeKind kind )
            : this( ctx, kind, 0ul )
        {
            if( kind.RequiresIntValue() )
                throw new ArgumentException( $"Attribute {kind} requires a value", nameof( kind ) );
        }

        /// <summary>Creates an attribute with an integer value parameter</summary>
        /// <param name="ctx">Context used for uniqueing attributes</param>
        /// <param name="kind">The kind of attribute</param>
        /// <param name="value">Value for the attribute</param>
        /// <remarks>
        /// <para>Not all attributes support a value and those that do don't all support
        /// a full 64bit value. The following table provdes the kinds of attributes
        /// accepting a value and the allowed size of the values.</para>
        /// <list type="table">
        /// <listheader><term><see cref="AttributeKind"/></term><term>Bit Length</term></listheader>
        /// <item><term><see cref="AttributeKind.Alignment"/></term><term>32</term></item>
        /// <item><term><see cref="AttributeKind.StackAlignment"/></term><term>32</term></item>
        /// <item><term><see cref="AttributeKind.Dereferenceable"/></term><term>64</term></item>
        /// <item><term><see cref="AttributeKind.DereferenceableOrNull"/></term><term>64</term></item>
        /// </list>
        /// </remarks>
        public AttributeValue( Context ctx, AttributeKind kind, UInt64 value )
        {
            Context = ctx;
            NativeAttribute = NativeMethods.CreateAttribute( ctx.ContextHandle, ( LLVMAttrKind )kind, 0 );
        }

        /// <summary>Adds a valueless named attribute</summary>
        /// <param name="ctx">Context to use for uniqueing attributes</param>
        /// <param name="name">Attribute name</param>
        public AttributeValue( Context ctx, string name )
            : this( ctx, name, string.Empty )
        {
        }

        /// <summary>Adds a Target specific named attrinute with value</summary>
        /// <param name="ctx">Context to use for uniqueing attributes</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        public AttributeValue( Context ctx, string name, string value )
        {
            if( string.IsNullOrWhiteSpace( name ) )
                throw new ArgumentException( "AttributeValue name cannot be null, Empty or all whitespace" );

            NativeAttribute = NativeMethods.CreateTargetDependentAttribute( ctx.ContextHandle, name, value );
            Context = ctx;
        }

        #region IEquatable
        public override int GetHashCode( ) => NativeAttribute.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is AttributeValue )
                return Equals( ( LLVMMetadataRef )obj );

            if( obj is UIntPtr )
                return NativeAttribute.Equals( obj );

            return base.Equals( obj );
        }

        public bool Equals( AttributeValue other ) => NativeAttribute == other.NativeAttribute;
        
        public static bool operator ==( AttributeValue lhs, AttributeValue rhs ) => lhs.Equals( rhs );
        public static bool operator !=( AttributeValue lhs, AttributeValue rhs ) => !lhs.Equals( rhs );
        #endregion

        public Context Context { get; }

        /// <summary>Kind of the attribute, or null for target specifc named attributes</summary>
        public AttributeKind? Kind => IsEnum ? ( AttributeKind )NativeMethods.GetAttributeKind( NativeAttribute ) : (AttributeKind?)null;

        /// <summary>Name of a named attribute or null for other kinds of attributes</summary>
        public string Name
        {
            get
            {
                if( !IsString )
                    return null;

                var llvmString = NativeMethods.GetAttributeName( NativeAttribute );
                return Marshal.PtrToStringAnsi( llvmString );
            }
        }

        /// <summary>StringValue for named attributes with values</summary>
        public string StringValue
        {
            get
            {
                if( !IsString )
                    return null;

                var llvmString = NativeMethods.GetAttributeStringValue( NativeAttribute );
                return Marshal.PtrToStringAnsi( llvmString );
            }
        }

        /// <summary>Integer value of the attribute or null if the attribute doens't have a value</summary>
        public UInt64? IntegerValue => IsInt ? ( UInt64 )NativeMethods.GetAttributeValue( NativeAttribute ) : (UInt64?)null;

        /// <summary>Flag to indicate if this attribute is a target specific string value</summary>
        public bool IsString => NativeMethods.IsStringAttribute( NativeAttribute );

        /// <summary>Flag to indicate if this attribute has an integer attibrute</summary>
        public bool IsInt => NativeMethods.IsIntAttribute( NativeAttribute );

        /// <summary>Flag to indicate if this attribute is a simple enumeration value</summary>
        public bool IsEnum => NativeMethods.IsEnumAttribute( NativeAttribute );

        internal readonly UIntPtr NativeAttribute;
    }
}

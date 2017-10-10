// <copyright file="AttributeValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Values
{
    /// <summary>Single attribute for functions, function returns and function parameters</summary>
    /// <remarks>
    /// This is the equivalent to the underlying llvm::AttributeImpl class. The name was changed to
    /// AttributeValue in .NET to prevent confusion with the standard <see cref="Attribute"/> class
    /// that is used throughout .NET libraries.
    /// </remarks>
    public struct AttributeValue
        : IEquatable<AttributeValue>
    {
        public Context Context { get; }

        public override int GetHashCode( ) => NativeAttribute.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is AttributeValue attrib )
            {
                return Equals( attrib );
            }

            if( obj is UIntPtr )
            {
                return NativeAttribute.Equals( obj );
            }

            return false;
        }

        public bool Equals( AttributeValue other )
        {
            if( ReferenceEquals( other, null ) )
            {
                return false;
            }

            return NativeAttribute.Pointer == other.NativeAttribute.Pointer;
        }

        /// <summary>Kind of the attribute, <see cref="AttributeKind.None"/> for target specific named attributes</summary>
        public AttributeKind Kind
        {
            get
            {
                if( !NativeMethods.IsEnumAttribute( NativeAttribute ) )
                {
                    return AttributeKind.None;
                }

                return AttributeKindExtensions.LookupId( NativeMethods.GetEnumAttributeKind( NativeAttribute ) );
            }
        }

        /// <summary>Name of a named attribute or null for other kinds of attributes</summary>
        public string Name
        {
            get
            {
                if( IsString )
                {
                    return NativeMethods.GetStringAttributeKind( NativeAttribute, out uint length );
                }
                else
                {
                    return AttributeKindExtensions.LookupId( NativeMethods.GetEnumAttributeKind( NativeAttribute ) ).GetAttributeName( );
                }
            }
        }

        /// <summary>StringValue for named attributes with values</summary>
        public string StringValue
        {
            get
            {
                if( !IsString )
                {
                    return null;
                }

                return NativeMethods.GetStringAttributeValue( NativeAttribute, out uint len );
            }
        }

        /// <summary>Integer value of the attribute or null if the attribute doesn't have a value</summary>
        public UInt64? IntegerValue => IsInt ? NativeMethods.GetEnumAttributeValue( NativeAttribute ) : ( UInt64? )null;

        /// <summary>Flag to indicate if this attribute is a target specific string value</summary>
        public bool IsString => NativeMethods.IsStringAttribute( NativeAttribute );

        /// <summary>Flag to indicate if this attribute has an integer attribute</summary>
        public bool IsInt => Kind.RequiresIntValue( );

        /// <summary>Flag to indicate if this attribute is a simple enumeration value</summary>
        public bool IsEnum => NativeMethods.IsEnumAttribute( NativeAttribute );

        public bool IsValidOn( FunctionAttributeIndex index, Value value )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            // for now all string attributes are valid everywhere as they are target dependent
            // (e.g. no way to verify the validity of an arbitrary without knowing the target)
            if( IsString )
            {
                return value.IsCallSite || value.IsFunction; // TODO: Attributes on globals??
            }

            return Kind.CheckAttributeUsage( index, value );
        }

        public void VerifyValidOn( FunctionAttributeIndex index, Value value)
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            // TODO: Attributes on globals??
            if( !( value.IsCallSite || value.IsFunction ) )
            {
                throw new ArgumentException( "Attributes only allowed on functions and call sites" );
            }

            // for now all string attributes are valid everywhere as they are target dependent
            // (e.g. no way to verify the validity of an arbitrary without knowing the target)
            if( IsString )
            {
                return;
            }

            Kind.VerifyAttributeUsage( index, value );
        }

        public override string ToString( )
        {
            return NativeMethods.AttributeToString( NativeAttribute );
        }

        public static bool operator ==( AttributeValue left, AttributeValue right ) => Equals( left, right );

        public static bool operator !=( AttributeValue left, AttributeValue right ) => !Equals( left, right );

        internal static AttributeValue FromHandle( Context context, LLVMAttributeRef handle)
        {
            return context.GetAttributeFor( handle, (c,h)=>new AttributeValue(c, h) );
        }

        internal LLVMAttributeRef NativeAttribute { get; }

        private AttributeValue( Context context, LLVMAttributeRef nativeValue )
        {
            context.ValidateNotNull( nameof( context ) );
            Context = context;
            NativeAttribute = nativeValue;
        }
    }
}

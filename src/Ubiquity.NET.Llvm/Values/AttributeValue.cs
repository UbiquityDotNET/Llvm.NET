﻿// -----------------------------------------------------------------------
// <copyright file="AttributeValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Types;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Values
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
        /// <summary>Gets the context that owns this <see cref="AttributeValue"/></summary>
        public Context Context { get; }

        /// <summary>Gets a hash code for this instance</summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode( ) => NativeAttribute.GetHashCode( );

        /// <summary>Performs equality checks against an <see cref="object"/></summary>
        /// <param name="obj">object to test for equality with this instance</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is equal to this instance</returns>
        public override bool Equals( object? obj )
        {
            return obj is AttributeValue attrib ? Equals( attrib ) : obj is UIntPtr && NativeAttribute.Equals( obj );
        }

        /// <summary>Performs equality checks against an <see cref="AttributeValue"/></summary>
        /// <param name="other">object to test for equality with this instance</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is equal to this instance</returns>
        public bool Equals( AttributeValue other )
        {
            return NativeAttribute.Equals( other.NativeAttribute );
        }

        /// <summary>Gets the kind of the attribute</summary>
        /// <value>The <see cref="AttributeKind"/> or <see cref="AttributeKind.None"/> for named attributes</value>
        public AttributeKind Kind
            => LLVMIsStringAttribute( NativeAttribute )
                 ? AttributeKind.None
                 : AttributeKindExtensions.LookupId( LLVMGetEnumAttributeKind( NativeAttribute ) );

        /// <summary>Gets the Name of the attribute</summary>
        public string Name
            => IsString
                ? LLVMGetStringAttributeKind( NativeAttribute, out uint _ )
                : AttributeKindExtensions.LookupId( LLVMGetEnumAttributeKind( NativeAttribute ) ).GetAttributeName( );

        /// <summary>Gets the value for named attributes with values</summary>
        /// <value>The value as a string or <see lang="null"/> if the attribute has no value</value>
        public string? StringValue => !IsString ? null : LLVMGetStringAttributeValue( NativeAttribute, out uint _ );

        /// <summary>Gets the Integer value of the attribute or <see lang="null"/> if the attribute doesn't have a value</summary>
        public UInt64? IntegerValue => HasIntegerVaue ? LLVMGetEnumAttributeValue( NativeAttribute ) : null;

        /// <summary>Gets the Type value of this attribute, if any</summary>
        public ITypeRef? TypeValue => TypeRef.FromHandle( LibLLVMGetAttributeTypeValue( NativeAttribute ) );

        /// <summary>Gets a value indicating whether this attribute is a target specific string value</summary>
        public bool IsString => LLVMIsStringAttribute( NativeAttribute );

        /// <summary>Gets a value indicating whether this attribute has an integer attribute</summary>
        public bool HasIntegerVaue => Kind.RequiresIntValue( );

        /// <summary>Gets a value indicating whether this attribute is a simple enumeration value</summary>
        public bool IsEnum => LLVMIsEnumAttribute( NativeAttribute );

        /// <summary>Tests if the attribute is valid for a <see cref="Value"/> on a given <see cref="FunctionAttributeIndex"/></summary>
        /// <param name="index">Attribute index to test if the attribute is valid on</param>
        /// <param name="value"><see cref="Value"/> </param>
        /// <returns><see lang="true"/> if the attribute is valid on the specified <paramref name="index"/> of the given <paramref name="value"/></returns>
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

        /// <summary>Verifies the attribute is valid for a <see cref="Value"/> on a given <see cref="FunctionAttributeIndex"/></summary>
        /// <param name="index">Index to verify</param>
        /// <param name="value">Value to check this attribute on</param>
        /// <exception cref="ArgumentException">The attribute is not valid on <paramref name="value"/> for the <paramref name="index"/></exception>
        public void VerifyValidOn( FunctionAttributeIndex index, Value value )
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
            // (e.g. no way to verify the validity of an arbitrary attribute without knowing the target)
            if( IsString )
            {
                return;
            }

            Kind.VerifyAttributeUsage( index, value );
        }

        /// <summary>Gets a string representation of the attribute</summary>
        /// <returns>Attribute as a string</returns>
        public override string ToString( )
        {
            return LibLLVMAttributeToString( NativeAttribute );
        }

        /// <summary>Tests attributes for equality</summary>
        /// <param name="left">Left side of the comparison</param>
        /// <param name="right">Right side of the comparison</param>
        /// <returns><see lang="true"/> if the attributes are equal</returns>
        public static bool operator ==( AttributeValue left, AttributeValue right ) => Equals( left, right );

        /// <summary>Tests attributes for inequality</summary>
        /// <param name="left">Left side of the comparison</param>
        /// <param name="right">Right side of the comparison</param>
        /// <returns><see lang="true"/> if the attributes are not equal</returns>
        public static bool operator !=( AttributeValue left, AttributeValue right ) => !Equals( left, right );

        internal static AttributeValue FromHandle( Context context, LLVMAttributeRef handle )
        {
            return context.GetAttributeFor( handle );
        }

        internal LLVMAttributeRef NativeAttribute { get; }

        internal class InterningFactory
            : HandleInterningMap<LLVMAttributeRef, AttributeValue>
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
            context.ValidateNotNull( nameof( context ) );
            Context = context;
            NativeAttribute = nativeValue;
        }
    }
}

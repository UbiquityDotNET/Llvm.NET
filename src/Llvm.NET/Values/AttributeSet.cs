using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Llvm.NET.Values
{
    /// <summary>AttributeSet for a <see cref="Function"/>, <see cref="Instructions.CallInstruction"/>, or <see cref="Instructions.Invoke"/> instruction</summary>
    /// <remarks>
    /// The underlying LLVM AttributeSet class is an immutable value type, unfortunately it includes a non-default constructor
    /// and therefore isn't a POD. However, it is trivially copy constructable and standard layout so this class simply wraps
    /// the llvm AttributeSet class.
    /// </remarks>
    public struct AttributeSet
        : IEquatable<AttributeSet>
    {
        public AttributeSet( Function function )
            : this( function, UIntPtr.Zero )
        {
        }
        
        public AttributeSet( Function function, FunctionAttributeIndex index, AttributeBuilder bldr )
            : this( function, NativeMethods.CreateAttributeSetFromBuilder( function.Context.ContextHandle, (uint)index, bldr.BuilderHandle ) )
        {
        }

        #region IEquatable
        public override int GetHashCode( ) => NativeAttributeSet.GetHashCode( );

        public override bool Equals( object obj )
        {
            if( obj is AttributeSet )
                return Equals( ( LLVMMetadataRef )obj );

            if( obj is UIntPtr )
                return NativeAttributeSet.Equals( obj );

            return base.Equals( obj );
        }

        public bool Equals( AttributeSet other ) => NativeAttributeSet == other.NativeAttributeSet;
        
        public static bool operator ==( AttributeSet lhs, AttributeSet rhs ) => lhs.Equals( rhs );
        public static bool operator !=( AttributeSet lhs, AttributeSet rhs ) => !lhs.Equals( rhs );
        #endregion

        public Context Context => NativeAttributeSet == UIntPtr.Zero ? null : Context.GetContextFor( NativeMethods.AttributeSetGetContext( NativeAttributeSet ) );

        /// <summary>Retrieves an attributeSet filtered by the specified function index</summary>
        /// <param name="index">Index to filter on</param>
        /// <returns>A new <see cref="AttributeSet"/>with attributes from this set belonging to the specified index</returns>
        public AttributeSet this[ FunctionAttributeIndex index ] => new AttributeSet( TargetFunction, NativeMethods.AttributeGetAttributes( NativeAttributeSet, (uint)index ) );

        /// <summary>Gets the attributes for the function return</summary>
        public AttributeSet ReturnAttributes => this[ FunctionAttributeIndex.ReturnType ];

        /// <summary>Gets the attributes for the function itself</summary>
        public AttributeSet FunctionAttributes => this[ FunctionAttributeIndex.Function ];

        /// <summary>Gets the attributes for a function parameter</summary>
        /// <param name="paramIndex">Parameter index [ 0 based ]</param>
        /// <returns><see cref="AttributeSet"/>filtered for the specified parameter</returns>
        public AttributeSet ParameterAttributes( int paramIndex )
        {
            if( paramIndex > TargetFunction.Parameters.Count )
                throw new ArgumentOutOfRangeException( nameof( paramIndex ) );

            var index = FunctionAttributeIndex.Parameter0 + paramIndex;
            return this[ index ];
        }

        /// <summary>Function this attributeSet targets</summary>
        /// <remarks>
        /// It is important to note that, in LLVM, this attribute set is distinct
        /// from the attributes on the function itself. In particular attribute
        /// sets are applied to Call sites (e.g. <see cref="Instructions.CallInstruction"/>
        /// and <see cref="Instructions.Invoke"/> instructions). Thus allowing the
        /// call site to include a different set of attributes from the set for
        /// the function itself. (Though it is currently unclear what scenarios
        /// this is intended for).
        /// </remarks>
        public Function TargetFunction { get; }

        /// <summary>Get LLVM formatted string representation of this <see cref="AttributeSet"/> for a given index</summary>
        /// <param name="index">Index to get the string for</param>
        /// <returns>Formatted string for the specified attribute index</returns>
        public string AsString( FunctionAttributeIndex index )
        {
            var msgPtr = NativeMethods.AttributeSetToString( NativeAttributeSet, ( uint )index, false );
            return NativeMethods.MarshalMsg( msgPtr );
        }

        /// <summary>Creates a formatted string representation of the entire <see cref="AttributeSet"/> (e.g. all indeces)</summary>
        /// <returns>Formatted string representation of the <see cref="AttributeSet"/></returns>
        public override string ToString( )
        {
            var bldr = new StringBuilder( );
            bldr.AppendFormat( "[Return: {0}]", AsString( FunctionAttributeIndex.ReturnType ) ).AppendLine( )
                .AppendFormat( "[Function: {0}]", AsString( FunctionAttributeIndex.Function ) ).AppendLine( );

            // capture "this" for lambda as lambda's in a struct member can't access the "this" pointer
            var self = this;
            var q = from param in TargetFunction.Parameters
                    where self.HasAny( FunctionAttributeIndex.Parameter0 + (int)param.Index )
                    select $"[Parameter({param.Name},{param.Index}): {self.AsString( FunctionAttributeIndex.Parameter0 + ( int )param.Index)}";

            foreach( var param in q )
                bldr.AppendLine( param );

            return bldr.ToString( );
        }

        /// <summary>Adds a set of attributes</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attributes">Attributes to add</param>
        public AttributeSet Add( FunctionAttributeIndex index, params AttributeValue[ ] attributes )
        {
           return Add( index, ( IEnumerable<AttributeValue> )attributes );
        }

        /// <summary>Add a collection of attributes</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attributes"></param>
        public AttributeSet Add( FunctionAttributeIndex index, IEnumerable<AttributeValue> attributes )
        {
            using( var bldr = new AttributeBuilder( this ) )
            {
                foreach( var value in attributes )
                    bldr.Add( index, value );

                return bldr.ToAttributeSet( );
            }
        }

        /// <summary>Adds a single attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attribute"><see cref="AttributeValue"/> kind to add</param>
        public AttributeSet Add( FunctionAttributeIndex index, AttributeValue attribute )
        {
            using( var bldr = new AttributeBuilder( this ) )
            {
                bldr.Add( index, attribute );
                return bldr.ToAttributeSet( );
            }
        }

        /// <summary>Adds Attributes from another attribute set along a given index</summary>
        /// <param name="index">Index to add attributes to and from</param>
        /// <param name="attribute"><see cref="AttributeSet"/> to add the attributes from</param>
        /// <returns>New <see cref="AttributeSet"/>Containing all attributes of this set plus any
        ///  attributes from <paramref name="attribute"/> along the specified <paramref name="index"/></returns>
        public AttributeSet Add( FunctionAttributeIndex index, AttributeSet attribute )
        {
            throw new NotImplementedException( );
        }
        
        /// <summary>Removes the specified attribute from the attribute set</summary>
        public AttributeSet Remove( FunctionAttributeIndex index, AttributeKind kind )
        {
            throw new NotImplementedException( );
        }

        /// <summary>Remove a target specific attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="name">Name of the attribute</param>
        public AttributeSet Remove( FunctionAttributeIndex index, string name )
        {
            throw new NotImplementedException( );
        }

        /// <summary>Get an integer value for an index</summary>
        /// <param name="index">Index to get the value from</param>
        /// <param name="kind"><see cref="AttributeKind"/> to get the value of (see remarks for supported attributes)</param>
        /// <returns>Value of the attribute</returns>
        /// <remarks>
        /// The only attributes supporting an integer value are <see cref="AttributeKind.Alignment"/>,
        /// <see cref="AttributeKind.StackAlignment"/>, <see cref="AttributeKind.Dereferenceable"/>,
        /// <see cref="AttributeKind.DereferenceableOrNull"/>.
        /// </remarks>
        public UInt64 GetAttributeValue( FunctionAttributeIndex index, AttributeKind kind )
        {
            kind.VerifyIntAttributeUsage( index, 0 );
            throw new NotImplementedException( );
        }

        /// <summary>Tests if an <see cref="AttributeSet"/> has any attributes in the specified index</summary>
        /// <param name="index">Index for the attribute</param>
        public bool HasAny( FunctionAttributeIndex index )
        {
            throw new NotImplementedException( );
        }

        /// <summary>Tests if this attribute set has a given AttributeValue kind</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="kind">Kind of AttributeValue to test for</param>
        /// <returns>true if the AttributeValue esists or false if not</returns>
        public bool Has( FunctionAttributeIndex index, AttributeKind kind )
        {
            throw new NotImplementedException( );
        }

        /// <summary>Tests if this attribute set has a given string attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="name">Name of the attribute to test for</param>
        /// <returns>true if the attribute exists or false if not</returns>
        public bool Has( FunctionAttributeIndex index, string name )
        {
            throw new NotImplementedException( );
        }

        public static implicit operator AttributeSet( AttributeBuilder v )
        {
            throw new NotImplementedException( );
        }

        private AttributeSet Add( UIntPtr pThis, FunctionAttributeIndex index, AttributeValue attribute )
        {
            throw new NotImplementedException( );
        }

        internal AttributeSet( Function targetFunction, UIntPtr nativeAttributeSet )
        {
            TargetFunction = targetFunction;
            NativeAttributeSet = nativeAttributeSet;
        }

        // underlying native llvm::AttributeSet follows the basic Pointer to Implementation (PIMPL) pattern.
        // Thus, the total size of the structure is that of a pointer. WHile it isn't POD it is trivially
        // copy constructable and standard layout so it is safe to just use a pointer here and pass that to
        // native code as a simple IntPtr. The implementation for the PIMPL pattern is allocated and owned by
        // the context. Since attributesets are "uniqued" multiple AttributeSets may refer to the same internal
        // implementation instance. There is no dispose for the AttributeSet however as the context will take
        // care of cleaning them up when it is disposed. 
        internal readonly UIntPtr NativeAttributeSet;
    }

}

using System;
using System.Collections.Generic;

namespace Llvm.NET.Values
{
    /// <summary>Interface for a set of attributes on a Function, Function return or parameter</summary>
    public interface IAttributeSet
    {
        /// <summary>Parameter alignement (only valid on parameters)</summary>
        uint? ParamAlignment { get; set; }

        /// <summary>Stack alignment requiirements for this function if not the same as the ABI</summary>
        uint? StackAlignment { get; set; }

        ulong? DereferenceableBytes { get; set; }

        ulong? DereferenceableOrNullBytes { get; set; }

        /// <summary>Adds a set of boolean attributes to the function itself</summary>
        /// <param name="attributes">Attributes to add</param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Add( params AttributeKind[ ] attributes );

        /// <summary>Add a collection of attributes to the function itself</summary>
        /// <param name="attributes"></param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Add( IEnumerable<AttributeKind> attributes );

        /// <summary>Adds a single boolean attribute to the function itself</summary>
        /// <param name="kind">Attribute kind to add</param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Add( AttributeKind kind );

        /// <summary>Add a collection of target dependent attributes as name+value pairs</summary>
        /// <param name="targetDependentAttributes">Attributes to add</param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Add( IDictionary<string, string> targetDependentAttributes );

        /// <summary>Add a target dependent attribute as a name+value pair</summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Value for the attribute</param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Add( string name, string value );

        /// <summary>Removes the specified attribute from the attribute set</summary>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Remove( AttributeKind kind );

        /// <summary>Remove a target specific attribute</summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns>This instance for use in fluent style coding</returns>
        IAttributeSet Remove( string name );

        /// <summary>Tests if this attribute set has a given attribute kind</summary>
        /// <param name="kind">Kind of attribute to test for</param>
        /// <returns>true if the attribute esists or false if not</returns>
        bool Has( AttributeKind kind );
    }

    internal sealed class AttributeSetImpl
        : IAttributeSet
    {
        public override string ToString( )
        {
            if( !NativeMethods.FunctionHasAttributes( OwningFunction.ValueHandle, ( int )Index ) )
                return string.Empty;

            var intPtr = NativeMethods.GetFunctAttributesAsString( OwningFunction.ValueHandle, ( int )Index );
            return NativeMethods.MarshalMsg( intPtr );
        }

        /// <inheritdoc/>
        public IAttributeSet Add( AttributeKind kind )
        {
            OwningFunction.AddAttribute( Index, kind );
            return this;
        }

        /// <inheritdoc/>
        public IAttributeSet Add( string name, string value )
        {
            OwningFunction.AddAttribute( Index, name, value );
            return this;
        }

        /// <inheritdoc/>
        public IAttributeSet Add( IDictionary< string, string > attributes )
        {
            foreach( var kvp in attributes )
                OwningFunction.AddAttribute( Index, kvp.Key, kvp.Value );

            return this;
        }

        /// <inheritdoc/>
        public IAttributeSet Add( IEnumerable< AttributeKind > attributes )
        {
            foreach( AttributeKind kind in attributes )
                OwningFunction.AddAttribute( Index, kind );

            return this;
        }

        /// <inheritdoc/>
        public IAttributeSet Add( params AttributeKind[ ] attributes ) => Add( ( IEnumerable<AttributeKind> )attributes );

        /// <inheritdoc/>
        public IAttributeSet Remove( AttributeKind kind )
        {
            OwningFunction.RemoveAttribute( Index, kind );
            return this;
        }

        /// <inheritdoc/>
        public IAttributeSet Remove( string name )
        {
            OwningFunction.RemoveAttribute( Index, name );
            return this;
        }

        /// <inheritdoc/>
        public bool Has( AttributeKind kind ) => OwningFunction.HasAttribute( Index, kind );

        /// <inheritdoc/>
        public uint? ParamAlignment
        {
            get
            {
                if( !OwningFunction.HasAttribute( Index, AttributeKind.Alignment ) )
                    return null;

                return (uint)OwningFunction.GetAttributeValue( Index, AttributeKind.Alignment );
            }

            set
            {
                if( !value.HasValue )
                {
                    if( OwningFunction.HasAttribute( Index, AttributeKind.Alignment ) )
                        OwningFunction.RemoveAttribute( Index, AttributeKind.Alignment );
                }
                else
                {
                    OwningFunction.AddAttribute( Index, AttributeKind.Alignment, value.Value );
                }
            }
        }

        /// <inheritdoc/>
        public uint? StackAlignment
        {
            get
            {
                if( !Has( AttributeKind.StackAlignment ) )
                    return null;

                return ( uint )OwningFunction.GetAttributeValue( Index, AttributeKind.StackAlignment );
            }

            set
            {
                if( !value.HasValue )
                {
                    if( OwningFunction.HasAttribute( Index, AttributeKind.StackAlignment ) )
                        OwningFunction.RemoveAttribute( Index, AttributeKind.StackAlignment );
                }
                else
                {
                    OwningFunction.AddAttribute( Index, AttributeKind.StackAlignment, value.Value );
                }
            }
        }

        /// <inheritdoc/>
        public ulong? DereferenceableBytes
        {
            get
            {
                if( !Has( AttributeKind.Dereferenceable ) )
                    return null;

                return ( uint )OwningFunction.GetAttributeValue( Index, AttributeKind.Dereferenceable );
            }

            set
            {
                if( !value.HasValue )
                {
                    if( OwningFunction.HasAttribute( Index, AttributeKind.Dereferenceable ) )
                        OwningFunction.RemoveAttribute( Index, AttributeKind.Dereferenceable );
                }
                else
                {
                    OwningFunction.AddAttribute( Index, AttributeKind.Dereferenceable, value.Value );
                }
            }
        }

        /// <inheritdoc/>
        public ulong? DereferenceableOrNullBytes
        {
            get
            {
                if( !Has( AttributeKind.DereferenceableOrNull ) )
                    return null;

                return ( uint )OwningFunction.GetAttributeValue( Index, AttributeKind.DereferenceableOrNull );
            }
            set
            {
                if( !value.HasValue )
                {
                    if( OwningFunction.HasAttribute( Index, AttributeKind.DereferenceableOrNull ) )
                        OwningFunction.RemoveAttribute( Index, AttributeKind.DereferenceableOrNull );
                }
                else
                {
                    OwningFunction.AddAttribute( Index, AttributeKind.DereferenceableOrNull, value.Value );
                }
            }
        }

        internal AttributeSetImpl( Function function, FunctionAttributeIndex index )
        {
            if( function == null )
                throw new ArgumentNullException( nameof( function ) );

            OwningFunction = function;
            // validate index parameter
            switch( index )
            {
            case FunctionAttributeIndex.Function:
            case FunctionAttributeIndex.ReturnType:
                // OK as-is
                break;
            default:
                // check the index against the actual function's param count
                var argIndex = ((int)index) - (int)FunctionAttributeIndex.Parameter0;
                if( argIndex < 0 || argIndex >= OwningFunction.Parameters.Count )
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                break;
            }
            Index = index;
        }

        private readonly Function OwningFunction;
        private readonly FunctionAttributeIndex Index;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Value representing an Argument to a function</summary>
    public class Argument
        : Value
    {
        /// <summary>Function this argument belongs to</summary>
        public Function ContainingFunction => FromHandle<Function>( NativeMethods.GetParamParent( ValueHandle ) );

        /// <summary>Zero based index of the argument</summary>
        public uint Index => NativeMethods.GetArgumentIndex( ValueHandle );

        /// <summary>Sets the alignment for the argument</summary>
        /// <param name="value">Alignment value for this argument</param>
        public Argument SetAlignment( uint value )
        {
            ContainingFunction.AddAttributeAtIndex( FunctionAttributeIndex.Parameter0 + ( int )Index
                                                  , Context.CreateAttribute( AttributeKind.Alignment, value )
                                                  );
            return this;
        }

        public IAttributeCollection Attributes => new ValueAttributeCollection( ContainingFunction, FunctionAttributeIndex.Parameter0 + ( int )Index );

        internal Argument( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402", Justification = "Fluent extensions considered part of the type")]
    public static class ArgumentExtensions
    {
        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, [CanBeNull] params AttributeKind[ ] values )
        {
            self.ValidateNotNull( nameof( self ) );

            if( values != null )
            {
                foreach( var kind in values )
                {
                    AttributeValue attrib = self.Context.CreateAttribute( kind );
                    self.Attributes.Add( attrib );
                }
            }

            return self;
        }

        public static Argument AddAttribute( [ValidatedNotNull] this Argument self, AttributeKind kind )
        {
            self.ValidateNotNull( nameof( self ) );
            kind.ValidateDefined( nameof( self ) );

            self.Attributes.Add( self.Context.CreateAttribute( kind ) );

            return self;
        }

        public static Argument AddAttribute( [ValidatedNotNull] this Argument self, AttributeValue attrib )
        {
            self.ValidateNotNull( nameof( self ) );

            self.Attributes.Add( attrib );

            return self;
        }

        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, params AttributeValue[ ] attributes )
        {
            return AddAttributes( self,  ( IEnumerable<AttributeValue> )attributes );
        }

        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, IEnumerable<AttributeValue> attributes )
        {
            self.ValidateNotNull( nameof( self ) );

            if( attributes != null )
            {
                foreach( var attrib in attributes )
                {
                    self.Attributes.Add( attrib );
                }
            }

            return self;
        }

        public static Argument RemoveAttribute( [ValidatedNotNull] this Argument self, AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( kind == AttributeKind.None )
            {
                return self;
            }

            return RemoveAttribute( self, kind.GetAttributeName( ) );
        }

        public static Argument RemoveAttribute( [ValidatedNotNull] this Argument self, string name )
        {
            self.ValidateNotNull( nameof( self ) );

            self.Attributes.Remove( name );
            return self;
        }
    }
}

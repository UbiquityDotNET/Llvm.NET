// -----------------------------------------------------------------------
// <copyright file="CallSiteAttributeAccessor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    // CONSIDER: Move these to methods on Instruction as they all assume that as the "self" parameter

    internal static class CallSiteAttributeAccessor
    {
        public static void AddAttributeAtIndex( this Instruction self, FunctionAttributeIndex index, AttributeValue attrib )
        {
            ThrowIfNotCallSite( self );
            attrib.AttributeInfo.ThrowIfInvalid( index );
            LLVMAddCallSiteAttribute( self.Handle, (LLVMAttributeIndex)index, attrib.NativeAttribute );
        }

        public static uint GetAttributeCountAtIndex( this Instruction self, FunctionAttributeIndex index )
        {
            ThrowIfNotCallSite( self );
            return LLVMGetCallSiteAttributeCount( self.Handle, (LLVMAttributeIndex)index );
        }

        public static IEnumerable<AttributeValue> GetAttributesAtIndex( this Instruction self, FunctionAttributeIndex index )
        {
            ThrowIfNotCallSite( self );
            return from attribRef in LLVMGetCallSiteAttributes( self.Handle, (LLVMAttributeIndex)index)
                   select new AttributeValue( attribRef );
        }

        public static AttributeValue GetAttributeAtIndex( this Instruction self, FunctionAttributeIndex index, UInt32 id )
        {
            ThrowIfNotCallSite( self );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( id, LLVMGetLastEnumAttributeKind() );
            var handle = LLVMGetCallSiteEnumAttribute( self.Handle, ( LLVMAttributeIndex )index, id );
            return new( handle );
        }

        public static AttributeValue GetAttributeAtIndex( this Instruction self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            if(string.IsNullOrWhiteSpace( name ))
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            ThrowIfNotCallSite( self );

            var info = AttributeInfo.From(name);
            if(info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize this message
                throw new ArgumentException( "Not a string argument", nameof( name ) );
            }

            unsafe
            {
                using var mem = name.Pin();
                var handle = LLVMGetCallSiteStringAttribute(
                    self.Handle,
                    ( LLVMAttributeIndex )index,
                    (byte*)mem.Pointer,
                    (uint)name.NativeStrLen
                );
                return new( handle );
            }
        }

        public static void RemoveAttributeAtIndex( this Instruction self, FunctionAttributeIndex index, UInt32 id )
        {
            ThrowIfNotCallSite( self );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( id, LLVMGetLastEnumAttributeKind() );
            LLVMRemoveCallSiteEnumAttribute( self.Handle, (LLVMAttributeIndex)index, id );
        }

        public static void RemoveAttributeAtIndex( this Instruction self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ThrowIfNotCallSite( self );

            var info = AttributeInfo.From(name);
            if(info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize (and dedup) this message
                throw new ArgumentException( "Not a string argument", nameof( name ) );
            }

            unsafe
            {
                using var mem = name.Pin();
                LLVMRemoveCallSiteStringAttribute( self.Handle, (LLVMAttributeIndex)index, (byte*)mem.Pointer, (uint)name.NativeStrLen );
            }
        }

        [Conditional( "DEBUG" )]
        public static void ThrowIfNotCallSite( this Instruction i, [CallerArgumentExpression( nameof( i ) )] string? exp = null )
        {
            // Unfortunately, these only have Instruction as the common base type
            // While all other forms of call (and thus call sites) all have `CallInstruction`
            // as the base though `CallBr` does not but is still considered a call site.
            if(i is not CallInstruction && i is not CallBr)
            {
                throw new ArgumentException( "Instruction is not a call site!", exp );
            }
        }
    }
}

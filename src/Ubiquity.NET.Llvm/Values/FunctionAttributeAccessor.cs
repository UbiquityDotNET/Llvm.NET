// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    // CONSIDER: move these into Function type itself as they all assume that as the "self" parameter

    internal static class FunctionAttributeAccessor
    {
        public static void AddAttributeAtIndex( this Function self, FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.AttributeInfo.ThrowIfInvalid( index );
            LLVMAddAttributeAtIndex( self.Handle, (LLVMAttributeIndex)index, attrib.NativeAttribute );
        }

        public static uint GetAttributeCountAtIndex( this Function self, FunctionAttributeIndex index )
        {
            return LLVMGetAttributeCountAtIndex( self.Handle, (LLVMAttributeIndex)index );
        }

        public static IEnumerable<AttributeValue> GetAttributesAtIndex( this Function self, FunctionAttributeIndex index )
        {
            uint count = GetAttributeCountAtIndex( self, index );
            if(count == 0)
            {
                return [];
            }

            var buffer = new LLVMAttributeRef[ count ];
            LLVMGetAttributesAtIndex( self.Handle, (LLVMAttributeIndex)index, buffer );
            return from attribRef in buffer
                   select new AttributeValue( attribRef );
        }

        public static AttributeValue GetAttributeAtIndex( this Function self, FunctionAttributeIndex index, UInt32 id )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan( id, LLVMGetLastEnumAttributeKind() );
            var handle = LLVMGetEnumAttributeAtIndex( self.Handle, ( LLVMAttributeIndex )index, id );
            return new( handle );
        }

        public static AttributeValue GetAttributeAtIndex( this Function self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            if(string.IsNullOrWhiteSpace( name ))
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            var info = AttributeInfo.From(name);
            if(info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize this message
                throw new ArgumentException( "Not a string argument", nameof( name ) );
            }

            unsafe
            {
                using var mem = name.Pin();
                var handle = LLVMGetStringAttributeAtIndex(
                    self.Handle,
                    ( LLVMAttributeIndex )index,
                    (byte*)mem.Pointer,
                    (uint)name.NativeStrLen
                );
                return new( handle );
            }
        }

        public static void RemoveAttributeAtIndex( this Function self, FunctionAttributeIndex index, UInt32 id )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan( id, LLVMGetLastEnumAttributeKind() );
            LLVMRemoveEnumAttributeAtIndex( self.Handle, (LLVMAttributeIndex)index, id );
        }

        public static void RemoveAttributeAtIndex( this Function self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var info = AttributeInfo.From(name);
            if(info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize (and dedup) this message
                throw new ArgumentException( "Not a string argument", nameof( name ) );
            }

            unsafe
            {
                using var mem = name.Pin();
                LLVMRemoveStringAttributeAtIndex( self.Handle, (LLVMAttributeIndex)index, (byte*)mem.Pointer, (uint)name.NativeStrLen );
            }
        }
    }
}

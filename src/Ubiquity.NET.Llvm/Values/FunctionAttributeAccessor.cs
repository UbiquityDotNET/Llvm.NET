// -----------------------------------------------------------------------
// <copyright file="FunctionAttributeAccessor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    internal static class FunctionAttributeAccessor
    {
        /// <inheritdoc/>
        public static void AddAttributeAtIndex(Function self, FunctionAttributeIndex index, AttributeValue attrib )
        {
            attrib.AttributeInfo.ThrowIfInvalid( index );
            LLVMAddAttributeAtIndex( self.Handle, ( LLVMAttributeIndex )index, attrib.NativeAttribute );
        }

        /// <inheritdoc/>
        public static uint GetAttributeCountAtIndex(Function self, FunctionAttributeIndex index )
        {
            return LLVMGetAttributeCountAtIndex( self.Handle, ( LLVMAttributeIndex )index );
        }

        /// <inheritdoc/>
        public static IEnumerable<AttributeValue> GetAttributesAtIndex(Function self, FunctionAttributeIndex index )
        {
            uint count = GetAttributeCountAtIndex( self, index );
            if( count == 0 )
            {
                return [];
            }

            var buffer = new LLVMAttributeRef[ count ];
            LLVMGetAttributesAtIndex( self.Handle, ( LLVMAttributeIndex )index, buffer );
            return from attribRef in buffer
                   select new AttributeValue( attribRef );
        }

        /// <inheritdoc/>
        public static AttributeValue GetAttributeAtIndex( Function self, FunctionAttributeIndex index, UInt32 id )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(id, LLVMGetLastEnumAttributeKind());
            var handle = LLVMGetEnumAttributeAtIndex( self.Handle, ( LLVMAttributeIndex )index, id );
            return new( handle );
        }

        /// <inheritdoc/>
        public static AttributeValue GetAttributeAtIndex( Function self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( Resources.Name_cannot_be_null_or_empty, nameof( name ) );
            }

            var info = AttributeInfo.From(name);
            if (info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize this message
                throw new ArgumentException("Not a string argument", nameof(name));
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

        /// <inheritdoc/>
        public static void RemoveAttributeAtIndex(Function self, FunctionAttributeIndex index, UInt32 id )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(id, LLVMGetLastEnumAttributeKind());
            LLVMRemoveEnumAttributeAtIndex( self.Handle, ( LLVMAttributeIndex )index, id );
        }

        /// <inheritdoc/>
        public static void RemoveAttributeAtIndex(Function self, FunctionAttributeIndex index, LazyEncodedString name )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var info = AttributeInfo.From(name);
            if (info.ArgKind != AttributeArgKind.String)
            {
                // TODO: Localize (and dedup) this message
                throw new ArgumentException("Not a string argument", nameof(name));
            }

            unsafe
            {
                using var mem = name.Pin();
                LLVMRemoveStringAttributeAtIndex( self.Handle, ( LLVMAttributeIndex )index, (byte*)mem.Pointer, ( uint )name.NativeStrLen);
            }
        }
    }
}

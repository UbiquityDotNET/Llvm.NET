// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// Interface+internal type matches file name
#pragma warning disable SA1649

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface for a named type with members</summary>
    /// <remarks>This is a common interface for structures and unions</remarks>
    public interface INamedStructuralType
        : ITypeRef
    {
        /// <summary>Gets the name of the structure</summary>
        string Name { get; }

        /// <summary>Gets a value indicating whether the structure is opaque (e.g. has no body defined yet)</summary>
        bool IsOpaque { get; }

        /// <summary>Gets a list of types for all member elements of the structure</summary>
        IReadOnlyList<ITypeRef> Members { get; }
    }

    /// <summary>Interface for an LLVM structure type</summary>
    public interface IStructType
        : INamedStructuralType
    {
        /// <summary>Gets a value indicating whether the structure is packed (e.g. no automatic alignment padding between elements)</summary>
        bool IsPacked { get; }

        /// <summary>Sets the body of the structure</summary>
        /// <param name="packed">Flag to indicate if the body elements are packed (e.g. no padding)</param>
        /// <param name="elements">Types of each element (may be empty)</param>
        void SetBody( bool packed, params IEnumerable<ITypeRef> elements );
    }

    internal sealed class StructType
        : TypeRef
        , IStructType
    {
        public void SetBody( bool packed, params IEnumerable<ITypeRef> elements )
        {
            LLVMTypeRef[ ] llvmArgs = [ .. elements.Select( e => e.GetTypeRef() ) ];
            LLVMStructSetBody( Handle, llvmArgs, (uint)llvmArgs.Length, packed );
        }

        public string Name => LLVMGetStructName( Handle ) ?? string.Empty;

        public bool IsOpaque => LLVMIsOpaqueStruct( Handle );

        public bool IsPacked => LLVMIsPackedStruct( Handle );

        public IReadOnlyList<ITypeRef> Members
        {
            get
            {
                var members = new List<ITypeRef>( );
                if(Kind == TypeKind.Struct && !IsOpaque)
                {
                    uint count = LLVMCountStructElementTypes( Handle );
                    if(count > 0)
                    {
                        var structElements = new LLVMTypeRef[ count ];
                        LLVMGetStructElementTypes( Handle, structElements );

                        members.AddRange( from hRef in structElements
                                          select hRef.CreateType()
                                        );
                    }
                }

                return members;
            }
        }

        internal StructType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
        }
    }
}

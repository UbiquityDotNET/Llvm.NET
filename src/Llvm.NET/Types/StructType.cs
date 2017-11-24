// <copyright file="StructType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Native;

// Interface+interal type matches file name
#pragma warning disable SA1649

namespace Llvm.NET.Types
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
        /// <param name="elements">Optional types of each element</param>
        /// <remarks>
        /// To set the body, at least one element type is required. If none are provided this is a NOP.
        /// </remarks>
        void SetBody( bool packed, params ITypeRef[ ] elements );
    }

    internal class StructType
        : TypeRef
        , IStructType
    {
        public void SetBody( bool packed, params ITypeRef[ ] elements )
        {
            LLVMTypeRef[ ] llvmArgs = elements.Select( e => e.GetTypeRef() ).ToArray( );
            uint argsLength = (uint)llvmArgs.Length;

            // To interop correctly, we need to have an array of at least size one.
            if ( argsLength == 0 )
            {
                llvmArgs = new LLVMTypeRef[ 1 ];
            }

            NativeMethods.LLVMStructSetBody( TypeRefHandle, out llvmArgs[ 0 ], argsLength, packed );
        }

        public string Name => NativeMethods.LLVMGetStructName( TypeRefHandle );

        public bool IsOpaque => NativeMethods.LLVMIsOpaqueStruct( TypeRefHandle );

        public bool IsPacked => NativeMethods.LLVMIsPackedStruct( TypeRefHandle );

        public IReadOnlyList<ITypeRef> Members
        {
            get
            {
                var members = new List<ITypeRef>( );
                if( Kind == TypeKind.Struct && !IsOpaque )
                {
                    uint count = NativeMethods.LLVMCountStructElementTypes( TypeRefHandle );
                    if (count > 0)
                    {
                        LLVMTypeRef[] structElements = new LLVMTypeRef[ count ];
                        NativeMethods.LLVMGetStructElementTypes( TypeRefHandle, out structElements[ 0 ] );
                        members.AddRange( structElements.Select( h => FromHandle<ITypeRef>( h ) ) );
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

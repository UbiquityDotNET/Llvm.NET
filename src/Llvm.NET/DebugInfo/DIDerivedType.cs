// <copyright file="DIDerivedType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Dervied type</summary>
    /// <remarks>
    /// Debug information for a type derived from an existing type
    /// </remarks>
    /// <seealso href="xref:llvm_langref#diderivedtype">LLVM DIDerivedType</seealso>
    public class DIDerivedType
        : DIType
    {
        /// <summary>Gets the base type of this type</summary>
        public DIType BaseType => GetOperand<DIType>( 3 );

        /// <summary>Gets the extra data attached to this derived type</summary>
        public LlvmMetadata ExtraData => Operands[ 4 ].Metadata;

        /// <summary>Gets the Class type extra data for a pointer to member type</summary>
        public DIType ClassType => Tag != Tag.PtrToMemberType ? null : GetOperand<DIType>( 4 );

        /// <summary>Gets the ObjCProperty extra data</summary>
        public DIObjCProperty ObjCProperty => GetOperand<DIObjCProperty>( 4 );

        /*
        public Constant StorageOffsetInBits
        {
            get
            {
                if( Tag == Tag.Member && DebugInfoFlags.HasFlag( DebugInfoFlags.BitField ) )
                {
                    return GetOperand<ConstantAsMetadata>( 4 )?.Value;
                }

                return null;
            }
        }
        public Constant Constant
        {
            get
            {
                if( Tag == Tag.Member && DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember ) )
                {
                    return GetOperand<ConstantAsMetadata>( 4 )?.Value;
                }

                return null;
            }
        }
        */

        /// <summary>Creates a new <see cref="DIDerivedType"/> from an <see cref="LLVMMetadataRef"/></summary>
        /// <param name="handle">Handle to wrap</param>
        internal DIDerivedType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

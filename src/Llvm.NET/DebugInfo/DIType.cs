// <copyright file="DIType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for Debug info types</summary>
    public class DIType : DIScope
    {
        public DebugInfoFlags DebugInfoFlags
        {
            get
            {
                if( MetadataHandle.Handle == IntPtr.Zero )
                {
                    return 0;
                }

                return ( DebugInfoFlags )NativeMethods.LLVMDITypeGetFlags( MetadataHandle );
            }
        }

        public DIScope Scope
        {
            get
            {
                var handle = NativeMethods.LLVMDITypeGetScope( MetadataHandle );
                if( handle.Handle == IntPtr.Zero )
                {
                    return null;
                }

                return FromHandle< DIScope >( handle );
            }
        }

        public UInt32 Line => NativeMethods.LLVMDITypeGetLine( MetadataHandle );

        public UInt64 BitSize => NativeMethods.LLVMDITypeGetSizeInBits( MetadataHandle );

        public UInt64 BitAlignment => NativeMethods.LLVMDITypeGetAlignInBits( MetadataHandle );

        public UInt64 BitOffset => NativeMethods.LLVMDITypeGetOffsetInBits( MetadataHandle );

        public bool IsPrivate => ( DebugInfoFlags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Private;

        public bool IsProtected => ( DebugInfoFlags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Protected;

        public bool IsPublic => ( DebugInfoFlags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Public;

        public bool IsForwardDeclaration => DebugInfoFlags.HasFlag( DebugInfoFlags.ForwardDeclaration );

        public bool IsAppleBlockExtension => DebugInfoFlags.HasFlag( DebugInfoFlags.AppleBlock );

        public bool IsBlockByRefStruct => DebugInfoFlags.HasFlag( DebugInfoFlags.BlockByrefStruct );

        public bool IsVirtual => DebugInfoFlags.HasFlag( DebugInfoFlags.Virtual );

        public bool IsArtificial => DebugInfoFlags.HasFlag( DebugInfoFlags.Artificial );

        public bool IsObjectPointer => DebugInfoFlags.HasFlag( DebugInfoFlags.ObjectPointer );

        public bool IsObjClassComplete => DebugInfoFlags.HasFlag( DebugInfoFlags.ObjcClassComplete );

        public bool IsVector => DebugInfoFlags.HasFlag( DebugInfoFlags.Vector );

        public bool IsStaticMember => DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember );

        public bool IsLvalueReference => DebugInfoFlags.HasFlag( DebugInfoFlags.LValueReference );

        public bool IsRvalueReference => DebugInfoFlags.HasFlag( DebugInfoFlags.RValueReference );

        public string Name => NativeMethods.LLVMDITypeGetName( MetadataHandle );

        internal DIType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

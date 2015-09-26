using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for Debug info types</summary>
    public class DIType : DIScope
    {
        internal DIType( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        public DebugInfoFlags Flags
        {
            get
            {
                if( MetadataHandle.Pointer == IntPtr.Zero )
                    return 0;

                return ( DebugInfoFlags )NativeMethods.DITypeGetFlags( MetadataHandle );
            }
        }

        UInt32 Line => NativeMethods.DITypeGetLine( MetadataHandle );
        UInt64 BitSize => NativeMethods.DITypeGetSizeInBits( MetadataHandle );
        UInt64 BitAlignment => NativeMethods.DITypeGetAlignInBits( MetadataHandle );
        UInt64 BitOffset => NativeMethods.DITypeGetOffsetInBits( MetadataHandle );
        DIScope Scope => new DIScope( NativeMethods.DITypeGetScope( MetadataHandle ) );
        bool IsPrivate => ( Flags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Private;
        bool IsProtected => ( Flags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Protected;
        bool IsPublic => ( Flags & DebugInfoFlags.AccessibilityMask ) == DebugInfoFlags.Public;
        bool IsForwardDeclaration => Flags.HasFlag( DebugInfoFlags.FwdDecl );
        bool IsAppleBlockExtension => Flags.HasFlag( DebugInfoFlags.AppleBlock );
        bool IsBlockByRefStruct => Flags.HasFlag( DebugInfoFlags.BlockByrefStruct );
        bool IsVirtual => Flags.HasFlag( DebugInfoFlags.Virtual );
        bool IsArtificial => Flags.HasFlag( DebugInfoFlags.Artificial );
        bool IsObjectPointer => Flags.HasFlag( DebugInfoFlags.ObjectPointer );
        bool IsObjClassComplete => Flags.HasFlag( DebugInfoFlags.ObjcClassComplete );
        bool IsVector => Flags.HasFlag( DebugInfoFlags.Vector );
        bool IsStaticMember => Flags.HasFlag( DebugInfoFlags.StaticMember );
        bool IsLvalueReference => Flags.HasFlag( DebugInfoFlags.LValueReference );
        bool IsRvalueReference => Flags.HasFlag( DebugInfoFlags.RValueReference );

        public string Name => Marshal.PtrToStringAnsi( NativeMethods.DITypeGetName( MetadataHandle ) );
    }
}

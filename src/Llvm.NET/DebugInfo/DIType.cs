using System;

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

                return ( DebugInfoFlags )LLVMNative.DITypeGetFlags( MetadataHandle );
            }
        }

        public string Name => LLVMNative.MarshalMsg( LLVMNative.GetDITypeName( MetadataHandle ) );
    }
}

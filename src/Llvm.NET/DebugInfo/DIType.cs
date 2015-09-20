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

                return ( DebugInfoFlags )NativeMethods.DITypeGetFlags( MetadataHandle );
            }
        }

        public string Name => NativeMethods.MarshalMsg( NativeMethods.GetDITypeName( MetadataHandle ) );
    }
}

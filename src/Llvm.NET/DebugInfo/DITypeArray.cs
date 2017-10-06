using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Array of <see cref="DIType"/> nodes for use with see <see cref="DebugInfoBuilder"/> methods</summary>
    public class DITypeArray
    {
        internal DITypeArray( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }

        internal LLVMMetadataRef MetadataHandle { get; }
    }
}

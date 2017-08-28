using Llvm.NET.Native;

namespace Llvm.NET
{
    public class MDString
        : LlvmMetadata
    {
        internal MDString( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        public override string ToString( )
        {
            return NativeMethods.GetMDStringText( MetadataHandle, out uint len );
        }
    }
}
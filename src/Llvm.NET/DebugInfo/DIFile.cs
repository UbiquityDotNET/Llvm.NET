using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#difile"/></summary>
    public class DIFile
        : DIScope
    {
        internal DIFile( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        public string FileName => NativeMethods.GetDIFileName( MetadataHandle );

        public string Directory => NativeMethods.GetDIFileDirectory( MetadataHandle );

        public string Path => System.IO.Path.Combine( Directory, FileName );
    }
}

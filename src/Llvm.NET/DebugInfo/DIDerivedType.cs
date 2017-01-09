using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#diderivedtype"/></summary>
    public class DIDerivedType : DIType
    {
        internal DIDerivedType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
        
        // operannds:
        //    0 - File
        //    1 - Scope
        //    2 - Name
        //    3 - Base Type
        //    4 - Extra data

        public DIType BaseType => Operands[ 3 ].Metadata as DIType;
    }
}

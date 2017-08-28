using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DIGlobalVariableExpression
        : MDNode
    {
        public DIGlobalVariable Variable
        {
            get
            {
                LLVMMetadataRef handle = NativeMethods.DIGlobalVarExpGetVariable( MetadataHandle );
                if( handle.Pointer.IsNull( ) )
                {
                    return null;
                }

                return FromHandle<DIGlobalVariable>( handle );
            }
        }

        public DIExpression Expression { get; }

        internal DIGlobalVariableExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

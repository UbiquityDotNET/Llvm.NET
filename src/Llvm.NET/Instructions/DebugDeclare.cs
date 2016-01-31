namespace Llvm.NET.Instructions
{
    public class DebugDeclare
        : DebugInfoIntrinsic
    {
        internal DebugDeclare( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

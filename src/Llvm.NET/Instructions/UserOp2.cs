using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class UserOp2 : Instruction
    {
        internal UserOp2( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

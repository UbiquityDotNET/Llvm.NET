using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class SIToFP : Cast
    {
        internal SIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

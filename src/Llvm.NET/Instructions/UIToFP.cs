using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class UIToFP : Cast
    {
        internal UIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

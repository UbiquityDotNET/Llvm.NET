using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPToSI : Cast
    {
        internal FPToSI( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

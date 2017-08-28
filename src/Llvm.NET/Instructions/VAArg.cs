using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class VaArg : UnaryInstruction
    {
        internal VaArg( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

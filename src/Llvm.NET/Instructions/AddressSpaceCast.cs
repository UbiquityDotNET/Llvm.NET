using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class AddressSpaceCast : Cast
    {
        internal AddressSpaceCast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

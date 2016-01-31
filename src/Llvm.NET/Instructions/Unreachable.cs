namespace Llvm.NET.Instructions
{
    public class Unreachable
        : Terminator
    {
        internal Unreachable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

namespace Llvm.NET.Values
{
    public class ConstantDataVector : ConstantDataSequential
    {
        internal ConstantDataVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

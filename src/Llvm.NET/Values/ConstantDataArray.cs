namespace Llvm.NET.Values
{
    public class ConstantDataArray : ConstantDataSequential
    {
        internal ConstantDataArray( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

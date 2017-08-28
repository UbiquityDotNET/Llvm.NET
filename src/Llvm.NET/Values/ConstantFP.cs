using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Floating point constant value in LLVM</summary>
    public class ConstantFP : Constant
    {
        public double Value => GetValueWithLoss( out bool loosesInfo );

        public double GetValueWithLoss( out bool loosesInfo )
        {
            return NativeMethods.ConstRealGetDouble( ValueHandle, out loosesInfo );
        }

        internal ConstantFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

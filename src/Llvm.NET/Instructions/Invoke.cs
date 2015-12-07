using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    public class Invoke
        : Terminator
        , IAttributeSetContainer
    {
        public AttributeSet Attributes
        {
            get 
            {
                if( TargetFunction == null )
                    return null;

                return new AttributeSet( TargetFunction, NativeMethods.GetCallSiteAttributeSet( ValueHandle ));
            }

            set
            {
                NativeMethods.SetCallSiteAttributeSet( ValueHandle, value.NativeAttributeSet );
            }
        }

        public Function TargetFunction
        {
            get
            {
                if( Operands.Count < 1 )
                    return null;

                // last Operand is the target function
                return Operands[ Operands.Count - 1 ] as Function;
            }
        }

        internal Invoke( LLVMValueRef valueRef )
            : this( valueRef, false )
        {
        }

        internal Invoke( LLVMValueRef valueRef, bool preValidated )
            : base( preValidated ? valueRef : ValidateConversion( valueRef, NativeMethods.IsABranchInst ) )
        {
        }
    }
}

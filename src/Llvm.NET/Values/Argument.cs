using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Value representing an Argument to a function</summary>
    public class Argument
        : Value
    {
        /// <summary>Function this argument belongs to</summary>
        public Function ContainingFunction => FromHandle<Function>( NativeMethods.GetParamParent( ValueHandle ) );

        /// <summary>Zero based index of the argument</summary>
        public uint Index => NativeMethods.GetArgumentIndex( ValueHandle );

        /// <summary>Sets the alignment for the argument</summary>
        /// <param name="value">Alignment value for this argument</param>
        public Argument SetAlignment( uint value )
        {
            ContainingFunction.AddAttributeAtIndex( FunctionAttributeIndex.Parameter0 + ( int )Index
                                                  , Context.CreateAttribute( AttributeKind.Alignment, value )
                                                  );
            return this;
        }

        public IAttributeCollection Attributes
        {
            get => new ValueAttributeCollection( ContainingFunction, FunctionAttributeIndex.Parameter0 + (int)Index );
        }

        internal Argument( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

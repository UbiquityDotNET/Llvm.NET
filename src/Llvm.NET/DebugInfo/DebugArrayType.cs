using Llvm.NET.Types;

namespace Llvm.NET.DebugInfo
{
    public class DebugArrayType
        : DebugType<IArrayType, DICompositeType>
        , IArrayType
    {
        public DebugArrayType( IDebugType<ITypeRef, DIType> elementType, Module module, uint count, uint lowerBound = 0 )
            : base( elementType.CreateArrayType( count )
                  , module.DIBuilder.CreateArrayType( module.Layout.BitSizeOf( elementType )
                                                    , module.Layout.AbiBitAlignmentOf( elementType )
                                                    , elementType.DIType
                                                    , module.DIBuilder.CreateSubrange( lowerBound, count )
                                                    )
                  )
        {
            DebugElementType = elementType;
        }

        public DebugArrayType( IArrayType llvmType, Module module, DIType elementType, uint count, uint lowerbound = 0 )
            : this( DebugType.Create( llvmType.ElementType, elementType), module, count, lowerbound )
        {
        }

        public IDebugType<ITypeRef, DIType> DebugElementType { get; }

        public ITypeRef ElementType => DebugElementType;
        public uint Length => ( ( IArrayType )NativeType ).Length;
    }
}

using System.Collections.Generic;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#dicompositetype"/></summary>
    public class DICompositeType : DIType
    {
        internal DICompositeType( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        public DIType BaseType => Operands[ 3 ].Metadata as DIType;
        public IReadOnlyList<DINode> Elements => new TupleTypedArrayWrapper<DINode>( Operands[ 4 ].Metadata as MDTuple );
        // TODO: VTableHolder   [5]
        // TODO: TemplateParams [6]
        // TODO: Identifier     [7]
    }
}

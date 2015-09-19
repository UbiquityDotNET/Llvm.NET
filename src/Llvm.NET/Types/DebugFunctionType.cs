using System.Collections.Generic;
using Llvm.NET.DebugInfo;

namespace Llvm.NET.Types
{
    /// <summary>
    /// DebugFunction type is moslty a wrapper around an existing IFunctionType
    /// however setting the DIType doesn't throw an exception, instead it sets
    /// the DISubroutine type for this instance. Thus allowing for a one to 
    /// many relation between the underlying Function type and the debug type
    /// </summary>
    internal class DebugFunctionType
        : DebugTypePair<DISubroutineType>
        , IFunctionType
    {
        internal DebugFunctionType( IFunctionType rawType, DISubroutineType sub )
            : base( rawType, sub )
        {
        }

        public bool IsVarArg => ((IFunctionType)LlvmType).IsVarArg;

        public ITypeRef ReturnType => ((IFunctionType)LlvmType).ReturnType;

        public IReadOnlyList<ITypeRef> ParameterTypes => ((IFunctionType)LlvmType).ParameterTypes;
    }
}

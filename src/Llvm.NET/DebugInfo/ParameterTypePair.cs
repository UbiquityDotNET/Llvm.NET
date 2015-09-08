using Llvm.NET.Types;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Provides pairing of a <see cref="TypeRef"/> with a <see cref="DIType"/> for function signatures</summary>
    /// <remarks>
    /// <para>Ordinarily, <see cref="TypeRef.DIType"/> serves to provide the correct debug information type.
    /// However, when using the pointer+alloca+memcpy pattern to pass by value the actual source and
    /// debug info type is different than the LLVM function signature. This class is used to construct
    /// signatures to allow for overriding the normal 1:1 type mapping with <see cref="TypeRef.DIType"/>.
    /// </para>
    /// <para>An implicit cast allows constructing an instance of this type directly from a <see cref="TypeRef"/>
    /// to minimize the manual construction of instances and keep code more readable. An explicit instance
    /// is only required when aactually altering the default behaviour thus making the divergence more explicit. 
    /// </para>
    /// </remarks>
    public class ParameterTypePair
    {
        public ParameterTypePair( TypeRef llvmType, DIType diType )
        {
            LlvmType = llvmType;
            DIType = diType;
        }

        public TypeRef LlvmType { get; }
        public DIType DIType { get; }

        public static implicit operator ParameterTypePair( TypeRef llvmType )
        {
            return new ParameterTypePair( llvmType, llvmType.DIType );
        }
    }
}

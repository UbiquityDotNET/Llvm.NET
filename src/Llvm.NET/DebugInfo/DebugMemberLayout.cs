using Llvm.NET.Types;

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugMemberLayout is used to define custom layout information for structure members</summary>
    /// <remarks>
    /// Ordinarily layout information is handle automatically in
    /// <see cref="DebugStructType.SetBody(bool, NativeModule, DIScope, DIFile, uint, DebugInfoFlags, System.Collections.Generic.IEnumerable{DebugMemberInfo})"/>
    /// however in cases where explicitly controlled (or "packed") layout is required, instances of DebugMemberLayout are
    /// used to provide the information necessary to generate a proper type and debug information.
    /// </remarks>
    public class DebugMemberLayout
    {
        /// <summary>Constructs a new <see cref="DebugMemberLayout"/></summary>
        /// <param name="bitSize">Size of the member in bits</param>
        /// <param name="bitAlignment">Alignment of the member in bits</param>
        /// <param name="bitOffset">Offset of the member in bits</param>
        public DebugMemberLayout( ulong bitSize, uint bitAlignment, ulong bitOffset )
        {
            BitSize = bitSize;
            BitAlignment = bitAlignment;
            BitOffset = bitOffset;
        }

        /// <summary>Bit size for the field</summary>
        public ulong BitSize { get; }

        /// <summary>Bit alignment for the field</summary>
        public uint BitAlignment { get; }

        /// <summary>Bit offset for the field in it's containing type</summary>
        public ulong BitOffset { get; }
    }
}

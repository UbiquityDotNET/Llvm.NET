using System;
using System.Linq;
using Llvm.NET;
using Llvm.NET.DebugInfo;
using Llvm.NET.Types;

namespace TestDebugInfo
{
    class DebugTypeInfo
    {
        public DebugTypeInfo( TypeRef type, DebugInfoBuilder diBuilder, TargetData layout, string name, DIScope scope )
        {
            LlvmType = type;
            Layout = layout;
            DebugType = GetInitialDebugType( LlvmType, diBuilder, name, scope );
        }

        public DIType DebugType { get; protected set; }
        public TypeRef LlvmType { get; }
        public TargetData Layout { get; }

        public ulong BitSize => Layout.BitSizeOf( LlvmType );
        public ulong AbiSize => Layout.AbiSizeOf( LlvmType );
        public ulong StoreSize => Layout.StoreSizeOf( LlvmType );
        public uint PreferredAlignment => Layout.PreferredAlignmentOf( LlvmType );
        public uint PreferredBitAlignment => PreferredAlignment * 8;
        public uint AbiAlignment => Layout.AbiAlignmentOf( LlvmType );
        public uint AbiBitAlignment => AbiAlignment * 8;
        public uint CallFrameAlignment => Layout.CallFrameAlignmentOf( LlvmType );
        public uint CallFrameBitAlignment => CallFrameAlignment * 8;
        public uint ElementAtOffset( ulong offset ) => Layout.ElementAtOffset( ( StructType )LlvmType, offset );
        public ulong OffsetOfElement( uint element ) => Layout.OffsetOfElement( ( StructType )LlvmType, element );
        public ulong BitOffsetOfElement( uint element ) => OffsetOfElement( element ) * 8;

        public DebugTypeInfo AsPointerType( DebugInfoBuilder diBuilder )
        {
            var llvmPtr = LlvmType.CreatePointerType( );
            var diPtr = diBuilder.CreatePointerType( DebugType, string.Empty, Layout.BitSizeOf( llvmPtr ), Layout.AbiAlignmentOf( llvmPtr ) * 8 );
            return new DebugTypeInfo( diPtr, llvmPtr, Layout );
        }

        public DebugTypeInfo AsArrayType( DebugInfoBuilder diBuilder, uint lowerBound, uint count )
        {
            var llvmArray = LlvmType.CreateArrayType( count );
            var diArray = diBuilder.CreateArrayType( Layout.BitSizeOf( llvmArray )
                                                   , Layout.AbiAlignmentOf( llvmArray ) * 8
                                                   , DebugType
                                                   , diBuilder.CreateSubrange( lowerBound, count )
                                                   );
            return new DebugTypeInfo( diArray, llvmArray, Layout );
        }

        public static DebugTypeInfo CreateFunctionType( DebugInfoBuilder diBuilder, DIFile diFile, DebugTypeInfo retType, params DebugTypeInfo[] argTypes )
        {
            var llvmArgTypes = argTypes.Select( dti => dti.LlvmType );
            var llvmType = retType.LlvmType.Context.GetFunctionType( retType.LlvmType, llvmArgTypes );
            var diArgTypes = argTypes.Select( dti => dti.DebugType );
            var diType =  diBuilder.CreateSubroutineType( diFile, 0, retType.DebugType, diArgTypes );
            return new DebugTypeInfo( diType, llvmType, retType.Layout );
        }

        public void ReplaceDebugTypeWith( DICompositeType compositeType )
        {
            DebugType.ReplaceAllUsesWith( compositeType );
            DebugType = compositeType;
        }

        protected DebugTypeInfo( DIType debugType, TypeRef llvmType, TargetData layout )
        {
            DebugType = debugType;
            LlvmType = llvmType;
            Layout = layout;
        }

        private DIType GetInitialDebugType( TypeRef llvmType, DebugInfoBuilder diBuilder, string name, DIScope scope )
        {
            switch( llvmType.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
            case TypeKind.Integer:
                return GetDiBasicType( llvmType, diBuilder, name );

            case TypeKind.Struct:
                return diBuilder.CreateReplaceableCompositeType( Tag.StructureType, name, scope, null, 0 );

            case TypeKind.Void:
            case TypeKind.Function:
            case TypeKind.Array:
                return null;

            case TypeKind.Pointer:
                return GetInitialDebugType( ((PointerType )llvmType).ElementType, diBuilder, $"{name}*", scope );

            default:
                throw new NotSupportedException( "Type not supported for this target/language" );
            }
        }

        DIBasicType GetDiBasicType( TypeRef llvmType, DebugInfoBuilder diBuilder, string name )
        {
            var bitSize = Layout.BitSizeOf( llvmType );
            var bitAlignment = Layout.AbiAlignmentOf( llvmType ) * 8;

            switch( llvmType.Kind )
            {
            case TypeKind.Float32:
            case TypeKind.Float64:
            case TypeKind.Float16:
            case TypeKind.X86Float80:
            case TypeKind.Float128m112:
            case TypeKind.Float128:
                return diBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Float );
            case TypeKind.Integer:
                return diBuilder.CreateBasicType( name, bitSize, bitAlignment, DiTypeKind.Signed );
            default:
                throw new NotSupportedException( "Not a basic type!" );
            }
        }
    }
}

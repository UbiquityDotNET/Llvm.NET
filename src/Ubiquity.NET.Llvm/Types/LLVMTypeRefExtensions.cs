// -----------------------------------------------------------------------
// <copyright file="TypeRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    internal static class LLVMTypeRefExtensions
    {
        internal static ITypeRef CreateType( this LLVMTypeRef handle, ITypeRef? elementType = null, [CallerArgumentExpression( nameof( handle ) )] string? exp = null )
        {
            if(handle.IsNull)
            {
                throw new ArgumentException( "Null handle is unmappable", exp );
            }

            var kind = ( TypeKind )LLVMGetTypeKind( handle );
            return kind switch
            {
                TypeKind.Struct => new StructType( handle ),
                TypeKind.Array => new ArrayType( handle ),
                TypeKind.Pointer => new PointerType( handle ) { ElementType = elementType },
                TypeKind.Vector => new VectorType( handle ),
                TypeKind.Function => new FunctionType( handle ), // NOTE: This is a signature rather than a Function, which is a Value
                /* other types not yet supported in Object wrappers as LLVM itself doesn't
                // have any specific types for them (except for IntegerType)
                // but the pattern for doing so should be pretty obvious...
                // case TypeKind.Void:
                // case TypeKind.Float16:
                // case TypeKind.Float32:
                // case TypeKind.Float64:
                // case TypeKind.X86Float80:
                // case TypeKind.Float128m112:
                // case TypeKind.Float128:
                // case TypeKind.Label:
                // case TypeKind.Integer: => IntegerType
                // case TypeKind.IrMetadata:
                // case TypeKind.X86MMX:
                */
                _ => new TypeRef( handle ),
            };
        }
    }
}

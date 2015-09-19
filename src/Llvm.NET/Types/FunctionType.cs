using System.Linq;
using System.Collections.Generic;
using Llvm.NET.DebugInfo;
using System;

namespace Llvm.NET.Types
{
    public interface IFunctionType
        : ITypeRef
    {
        bool IsVarArg { get; }
        ITypeRef ReturnType { get; }
        IReadOnlyList<ITypeRef> ParameterTypes { get; }
    }

    /// <summary>Class to repersent the LLVM type of a function (e.g. a signature)</summary>
    internal class FunctionType
        : TypeRef
        , IFunctionType
    {
        internal FunctionType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
        }

        /// <summary>Flag to indicate if this signature is for a variadic function</summary>
        public bool IsVarArg => LLVMNative.IsFunctionVarArg( TypeHandle_ );
        public override DIType DIType
        {
            get { return null; }
            set { throw new NotSupportedException( "Cannot set the DIType of a raw FunctionType" ); }
        }

        /// <summary></summary>
        public ITypeRef ReturnType => FromHandle<ITypeRef>( LLVMNative.GetReturnType( TypeHandle_ ) );
        public IReadOnlyList<ITypeRef> ParameterTypes
        {
            get
            {
                var paramCount = LLVMNative.CountParamTypes( TypeHandle_ );
                if( paramCount == 0 )
                    return new List<TypeRef>().AsReadOnly();

                var paramTypes = new LLVMTypeRef[ paramCount ];
                LLVMNative.GetParamTypes( TypeHandle_, out paramTypes[ 0 ] );
                return paramTypes.Select( h => FromHandle<TypeRef>( h ) )
                                 .ToList( )
                                 .AsReadOnly( );
            }
        }
    }
}

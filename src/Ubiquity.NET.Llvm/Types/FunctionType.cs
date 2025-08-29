// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Interface+internal type matches file name
#pragma warning disable SA1649

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface to represent the LLVM type of a function (e.g. a signature)</summary>
    public interface IFunctionType
        : ITypeRef
    {
        /// <summary>Gets a value indicating whether this signature is for a variadic function</summary>
        bool IsVarArg { get; }

        /// <summary>Gets the return type of the function</summary>
        ITypeRef ReturnType { get; }

        /// <summary>Gets the types of the parameters for the function</summary>
        IReadOnlyList<ITypeRef> ParameterTypes { get; }
    }

    /// <summary>Class to represent the LLVM type of a function (e.g. a signature)</summary>
    internal sealed class FunctionType
        : TypeRef
        , IFunctionType
    {
        /// <inheritdoc/>
        public bool IsVarArg => LLVMIsFunctionVarArg( Handle );

        /// <inheritdoc/>
        public ITypeRef ReturnType => LLVMGetReturnType( Handle ).CreateType();

        /// <inheritdoc/>
        public IReadOnlyList<ITypeRef> ParameterTypes
        {
            get
            {
                uint paramCount = LLVMCountParamTypes( Handle );
                if(paramCount == 0)
                {
                    return new List<TypeRef>().AsReadOnly();
                }

                var paramTypes = new LLVMTypeRef[ paramCount ];
                LLVMGetParamTypes( Handle, paramTypes );
                return (from p in paramTypes
                        select (TypeRef)p.CreateType()
                       ).ToList()
                        .AsReadOnly();
            }
        }

        internal FunctionType( LLVMTypeRef typeRef )
            : base( typeRef )
        {
        }
    }
}

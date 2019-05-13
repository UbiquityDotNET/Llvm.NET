// -----------------------------------------------------------------------
// <copyright file="LibLLVMTypePrinter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Generators.CSharp;
using Ubiquity.ArgValidators;

namespace LlvmBindingsGenerator
{
    internal class LibLLVMTypePrinter
        : CSharpTypePrinter
    {
        public LibLLVMTypePrinter( BindingContext context )
            : base( context.ValidateNotNull( nameof( context ) ) )
        {
        }

        public override string ToString( Type type )
        {
            string retVal;
            switch( type )
            {
            case TypedefType tdt when tdt.Declaration.Name == "LLVMBool":
                retVal = "bool";
                break;

            case PointerType pt when pt.Pointee is BuiltinType:
            case TypedefType tdt when tdt.Declaration.Name == "intptr_t":
                retVal = "global::System.IntPtr";
                break;

            case TypedefType tdt when tdt.Declaration.Name == "uintptr_t":
                retVal = "global::System.UIntPtr";
                break;

            case TypedefType tdt when tdt.Declaration.Name == "uint8_t":
                retVal = "global::System.Byte";
                break;

            case TypedefType tdt when( tdt.Declaration.Name == "uint32_t" || tdt.Declaration.Name == "LLVMDWARFTypeEncoding" ):
                retVal = "global::System.UInt32";
                break;

            case TypedefType tdt when( tdt.Declaration.Name == "uint64_t" || tdt.Declaration.Name == "LLVMOrcModuleHandle" || tdt.Declaration.Name == "LLVMOrcTargetAddress" ):
                retVal = "global::System.UInt64";
                break;

            case TypedefType tdt when tdt.Declaration.Name == "int8_t":
                retVal = "global::System.SByte";
                break;

            case TypedefType tdt when tdt.Declaration.Name == "int32_t":
                retVal = "global::System.Int32";
                break;

            case TypedefType tdt when tdt.Declaration.Name == "int64_t":
                retVal = "global::System.Int64";
                break;

            case TypedefType tdt:
                retVal = tdt.Declaration.Name;
                break;

            case CppSharp.AST.Type t when t.TryGetHandleDecl( out TypedefNameDecl decl ):
                retVal = decl.Name;
                break;

            case ArrayType at:
                retVal = $"{ToString( at.Type )}[]";
                break;

            default:
                retVal = base.ToString( type );
                break;
            }

            return retVal;
        }
    }
}

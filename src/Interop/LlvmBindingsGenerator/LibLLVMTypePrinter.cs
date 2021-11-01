﻿// -----------------------------------------------------------------------
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
    // CONSIDER: Make this type printer provide the source language name
    // so that Type.TosString() just works as is generally expected.
    // (May be simpler to get the debug signature if available)
    // Then add an extension method to type (i.e. ToTargetLanguageString())
    // that implements the functionality currently in this class

    /// <summary>Specialized type printer for Ubiquity.NET.Llvm.Interop</summary>
    /// <remarks>
    /// <para>Unfortunately <see cref="CppSharp.AST.Type.ToString"/> will fail with a null
    /// reference if there isn't a type printer delegate assigned. E.g. it has no
    /// internal ability to convert the type to a string. Instead, it assumes that the
    /// original source type is irrelevant and that the application is going to generate
    /// code. Furthermore it assumes that the code generation will use ToString() to
    /// get the final output generated code type name. Generally speaking that's a bad
    /// design. It means that display of the type in a debugger doesn't work and you can't
    /// see what you are dealing with easily. With the very surprising consequence of calls
    /// to the ToString() method in code generating a null reference exception if the
    /// delegate isn't set up.</para>
    /// <para>This type serves as an extension to the default <see cref="CSharpTypePrinter"/>
    /// that handles the specific needs of the Ubiquity.NET.Llvm.Interop code generation. The
    /// role of the type printer is to get a string for the type in the syntax of the target
    /// language. (As mentioned this is unfortunately tied into the type.ToString() method,
    /// which means the debugger only shows the target language type names and NOT the original
    /// AST names from the source)</para>
    /// </remarks>
    internal class LibLLVMTypePrinter
        : CSharpTypePrinter
    {
        public LibLLVMTypePrinter( BindingContext context )
            : base( context.ValidateNotNull( nameof( context ) ) )
        {
        }

        public override string ToString( Type type )
        {
            string retVal = type switch
            {
                TypedefType tdt when tdt.Declaration.Name == "LLVMBool" => "bool",
                PointerType pt when pt.Pointee is BuiltinType => $"{ToString( pt.Pointee )}*",
                TypedefType tdt when tdt.Declaration.Name == "intptr_t" => "global::System.IntPtr",
                TypedefType tdt when tdt.Declaration.Name == "uintptr_t" => "global::System.UIntPtr",
                TypedefType tdt when tdt.Declaration.Name == "uint8_t" => "global::System.Byte",
                TypedefType tdt when( ShouldBeUInt32(tdt) ) => "global::System.UInt32",
                TypedefType tdt when( ShouldBeUInt64(tdt) ) => "global::System.UInt64",
                TypedefType tdt when tdt.Declaration.Name == "int8_t" => "global::System.SByte",
                TypedefType tdt when tdt.Declaration.Name == "int32_t" => "global::System.Int32",
                TypedefType tdt when tdt.Declaration.Name == "int64_t" => "global::System.Int64",
                TypedefType tdt => tdt.Declaration.Name,
                BuiltinType bit when bit.Type == PrimitiveType.IntPtr => "global::System.IntPtr",
                CppSharp.AST.Type t when t.TryGetHandleDecl( out TypedefNameDecl decl ) => decl.Name,
                ArrayType at => $"{ToString( at.Type )}[]",
                _ => base.ToString( type ),
            };
            return retVal;
        }

        private static bool ShouldBeUInt32(TypedefType tdt)
        {
            return tdt.Declaration.Name == "uint32_t"
                || tdt.Declaration.Name == "LLVMDWARFTypeEncoding";
        }

        private static bool ShouldBeUInt64(TypedefType tdt)
        {
            return tdt.Declaration.Name == "uint64_t"
                || tdt.Declaration.Name == "LLVMOrcModuleHandle"
                || tdt.Declaration.Name == "LLVMOrcTargetAddress";
        }
    }
}

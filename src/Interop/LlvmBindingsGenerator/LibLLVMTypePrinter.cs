// -----------------------------------------------------------------------
// <copyright file="LibLLVMTypePrinter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using CppSharp.AST;
using CppSharp.Generators.CSharp;

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
        , ITypePrinter2
    {
        public override string ToString( CppSharp.AST.Type type )
        {
            throw new NotSupportedException("Direct calls to ToString are not supported, instead call GetName with the appropriate kind");
        }

        public string GetName( CppSharp.AST.Type type, TypeNameKind kind = TypeNameKind.Native)
        {
            if(kind == TypeNameKind.Native)
            {
                return type.Visit(NativePrinter);
            }

            string retVal = type switch
            {
                TypedefType tdt when tdt.Declaration.Name == "LLVMBool" => "bool",
                PointerType pt when pt.Pointee is BuiltinType => $"{base.ToString( pt.Pointee )}*", // shouldn't see this... Should be caught as an error...
                TypedefType tdt when tdt.Declaration.Name == "intptr_t" => "nint",
                TypedefType tdt when tdt.Declaration.Name == "uintptr_t" => "nuint",
                TypedefType tdt when tdt.Declaration.Name == "uint8_t" => "byte",
                TypedefType tdt when( ShouldBeUInt32(tdt) ) => "UInt32",
                TypedefType tdt when( ShouldBeUInt64(tdt) ) => "UInt64",
                TypedefType tdt when tdt.Declaration.Name == "int8_t" => "sbyte",
                TypedefType tdt when tdt.Declaration.Name == "uint16_t" => "UInt16",
                TypedefType tdt when tdt.Declaration.Name == "int16_t" => "Int16",
                TypedefType tdt when tdt.Declaration.Name == "int32_t" => "Int32",
                TypedefType tdt when tdt.Declaration.Name == "int64_t" => "Int64",
                TypedefType tdt => tdt.Declaration.Name,
                BuiltinType bit when bit.Type == PrimitiveType.IntPtr => "nint",
                CppSharp.AST.Type t when t.TryGetHandleDecl( out TypedefNameDecl decl ) => decl.Name,
                ArrayType at => $"{GetName( at.Type, kind )}[]",
                _ => base.ToString( type ),
            };
            return retVal;
        }

        private static bool ShouldBeUInt32(TypedefType tdt)
        {
            return tdt.Declaration.Name == "uint32_t"
                || tdt.Declaration.Name == "LLVMDWARFTypeEncoding"; // TODO: This should be mapped as a record struct (Closest equivalent to a typedef uint32_t foo;)
        }

        private static bool ShouldBeUInt64(TypedefType tdt)
        {
            return tdt.Declaration.Name == "uint64_t"
                || tdt.Declaration.Name == "LLVMOrcModuleHandle" // TODO: This should be mapped as a record struct
                || tdt.Declaration.Name == "LLVMOrcTargetAddress" // TODO: This should be mapped as a record struct
                || tdt.Declaration.Name == "LLVMOrcJITTargetAddress" // TODO: This should be mapped as a record struct
                || tdt.Declaration.Name == "LLVMOrcExecutorAddress"; // TODO: This should be mapped as a record struct
        }

        private readonly NativeTypePrinter NativePrinter = new();
    }
}

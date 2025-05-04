// -----------------------------------------------------------------------
// <copyright file="DebugType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1649 // File name must match first type ( Justification -  Interface + internal Impl + public extensions )

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Provides pairing of a <see cref="ITypeRef"/> with a <see cref="DebugInfoType"/> for function signatures</summary>
    /// <typeparam name="TNative">Native LLVM type</typeparam>
    /// <typeparam name="TDebug">Debug type description for the type</typeparam>
    /// <remarks>
    /// <para>Primitive types and function signature types are all interned in LLVM, thus there won't be a
    /// strict one to one relationship between an LLVM type and corresponding language specific debug
    /// type. (e.g. unsigned char, char, byte and signed byte might all be 8 bit integer values as far
    /// as LLVM is concerned.) Also, when using the pointer+alloca+memcpy pattern to pass by value the
    /// actual source debug info type is different than the LLVM function signature. This interface and
    /// it's implementations are used to construct native type and debug info pairing to allow applications
    /// to maintain a link from their AST or IR types into the LLVM native type and debug information.
    /// </para>
    /// <note type="note">
    /// It is important to note that the relationship from the <see cref="DebugInfoType"/> to it's <see cref="NativeType"/>
    /// properties is strictly ***one way***. That is, there is no way to take an arbitrary <see cref="ITypeRef"/>
    /// and re-associate it with the DebugInfoType or an implementation of this interface as there may be many such
    /// mappings to choose from.
    /// </note>
    /// </remarks>
    public interface IDebugType<out TNative, out TDebug>
        : ITypeRef
        where TNative : ITypeRef
        where TDebug : DIType
    {
        /// <summary>Gets the LLVM NativeType this interface is associating with debug info in <see cref="DebugInfoType"/></summary>
        TNative NativeType { get; }

        /// <summary>Gets the debug information type this interface is associating with <see cref="NativeType"/></summary>
        TDebug? DebugInfoType { get; }

        /// <summary>Creates a pointer to this type for a given module and address space</summary>
        /// <param name="diBuilder">Debug information builder to use</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns><see cref="DebugPointerType"/></returns>
        DebugPointerType CreatePointerType( ref readonly DIBuilder diBuilder, uint addressSpace );

        /// <summary>Creates a type defining an array of elements of this type</summary>
        /// <param name="diBuilder">Debug information builder to use</param>
        /// <param name="lowerBound">Lower bound of the array</param>
        /// <param name="count">Count of elements in the array</param>
        /// <returns><see cref="DebugArrayType"/></returns>
        DebugArrayType CreateArrayType( ref readonly DIBuilder diBuilder, uint lowerBound, uint count );
    }
}

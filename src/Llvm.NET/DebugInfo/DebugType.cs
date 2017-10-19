﻿// <copyright file="DebugType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

#pragma warning disable SA1649 // File name must match first type ( Interface + Impl )

namespace Llvm.NET.DebugInfo
{
    /// <summary>Provides pairing of a <see cref="ITypeRef"/> with a <see cref="DIType"/> for function signatures</summary>
    /// <typeparam name="TNative">Native LLVM type</typeparam>
    /// <typeparam name="TDebug">Debug type description for the type</typeparam>
    /// <remarks>
    /// <para>Primitive types and function signature types are all interned in LLVM, thus there won't be a
    /// strict one to one relationship between an LLVM type and corresponding language specific debug
    /// type. (e.g. unsigned char, char, byte and signed byte might all be 8 bit integer values as far
    /// as LLVM is concerned. Also, when using the pointer+alloca+memcpy pattern to pass by value the
    /// actual source debug info type is different than the LLVM function signature. This class is used
    /// to construct native type and debug info pairing to allow applications to maintain a link from
    /// their AST or IR types into the LLVM native type and debug information.
    /// </para>
    /// <note type="note">
    /// It is important to note that the relationship between the <see cref="DIType"/> to it's <see cref="NativeType"/>
    /// properties is strictly one way. That is, there is no way to take an arbitrary <see cref="ITypeRef"/> and re-associate
    /// it with the DIType or an implementation of this interface as there may be many such mappings to choose from.
    /// </note>
    /// </remarks>
    public interface IDebugType<out TNative, out TDebug>
        : ITypeRef
        where TNative : ITypeRef
        where TDebug : DIType
    {
        /// <summary>Gets the LLVM NativeType this interface is associating with debug info in <see cref="DIType"/></summary>
        TNative NativeType { get; }

        /// <summary>Gets the debug information type this interface is associating with <see cref="NativeType"/></summary>
        TDebug DIType { get; }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Interface, Generic type and static extension methods form a common type interface" )]
    public class DebugType<TNative, TDebug>
        : IDebugType<TNative, TDebug>
        , ITypeRef
        , ITypeHandleOwner
        where TNative : class, ITypeRef
        where TDebug : DIType
    {
        // Re-assignment will perform RAUW on the current value
        [SuppressMessage( "Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DIType", Justification = "It is spelled correctly 8^)" )]
        public TDebug DIType
        {
            get => DIType_;
            set
            {
                if( DIType_ == null )
                {
                    DIType_ = value;
                }
                else if( DIType_.IsTemporary )
                {
                    if( value.IsTemporary )
                    {
                        throw new InvalidOperationException( "Cannot replace a temporary with another temporary" );
                    }

                    DIType_.ReplaceAllUsesWith( value );
                    DIType_ = value;
                }
                else
                {
                    throw new InvalidOperationException( "Cannot replace non temporary DIType with a new Type" );
                }
            }
        }

        public TNative NativeType
        {
            get => NativeType_;

            protected set
            {
                if( NativeType_ != null )
                {
                    throw new InvalidOperationException( "Once the native type is set it cannot be changed" );
                }

                NativeType_ = value;
            }
        }

        LLVMTypeRef ITypeHandleOwner.TypeHandle => NativeType.GetTypeRef( );

        public bool IsSized => NativeType.IsSized;

        public TypeKind Kind => NativeType.Kind;

        public Context Context => NativeType.Context;

        public uint IntegerBitWidth => NativeType.IntegerBitWidth;

        public bool IsInteger => NativeType.IsInteger;

        public bool IsFloat => NativeType.IsFloat;

        public bool IsDouble => NativeType.IsDouble;

        public bool IsVoid => NativeType.IsVoid;

        public bool IsStruct => NativeType.IsStruct;

        public bool IsPointer => NativeType.IsPointer;

        public bool IsSequence => NativeType.IsSequence;

        public bool IsFloatingPoint => NativeType.IsFloatingPoint;

        public bool IsPointerPointer => NativeType.IsPointerPointer;

        public Constant GetNullValue( ) => NativeType.GetNullValue( );

        public IArrayType CreateArrayType( uint count ) => NativeType.CreateArrayType( count );

        public IPointerType CreatePointerType( ) => NativeType.CreatePointerType( );

        public IPointerType CreatePointerType( uint addressSpace ) => NativeType.CreatePointerType( addressSpace );

        public DebugPointerType CreatePointerType( BitcodeModule module, uint addressSpace )
        {
            if( DIType == null )
            {
                throw new ArgumentException( "Type does not have associated Debug type from which to construct a pointer type" );
            }

            var nativePointer = NativeType.CreatePointerType( addressSpace );
            return new DebugPointerType( nativePointer, module, DIType, string.Empty );
        }

        public DebugArrayType CreateArrayType( BitcodeModule module, uint lowerBound, uint count )
        {
            if( DIType == null )
            {
                throw new ArgumentException( "Type does not have associated Debug type from which to construct an array type" );
            }

            var llvmArray = NativeType.CreateArrayType( count );
            return new DebugArrayType( llvmArray, module, DIType, count, lowerBound );
        }

        public bool TryGetExtendedPropertyValue<TProperty>( string id, out TProperty value )
        {
            if( PropertyContainer.TryGetExtendedPropertyValue( id, out value ) )
            {
                return true;
            }

            return NativeType.TryGetExtendedPropertyValue( id, out value );
        }

        public void AddExtendedPropertyValue( string id, object value )
        {
            PropertyContainer.AddExtendedPropertyValue( id, value );
        }

        [SuppressMessage( "Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Available as a property, this is for convenience" )]
        public static implicit operator TDebug( DebugType<TNative, TDebug> self ) => self.ValidateNotNull(nameof(self)).DIType;

        internal DebugType( )
        {
        }

        internal DebugType( TNative llvmType )
        {
            NativeType = llvmType ?? throw new ArgumentNullException( nameof( llvmType ) );
        }

        [SuppressMessage( "StyleCop.CSharp.NamingRules"
                        , "SA1310:Field names must not contain underscore"
                        , Justification = "Trailing _ indicates value MUST NOT be written to directly, even internally"
                        )
        ]
        private TNative NativeType_;

        [SuppressMessage( "StyleCop.CSharp.NamingRules"
                        , "SA1310:Field names must not contain underscore"
                        , Justification = "Trailing _ indicates value MUST NOT be written to directly, even internally"
                        )
        ]
        private TDebug DIType_;

        private readonly ExtensiblePropertyContainer PropertyContainer = new ExtensiblePropertyContainer( );
    }

    public static class DebugType
    {
        /// <summary>Creates a new <see cref="DebugType"/>instance inferring the generic arguments from the parameters</summary>
        /// <typeparam name="TNative">Type of the Native LLVM type for the association</typeparam>
        /// <typeparam name="TDebug">Type of the debug information type for the association</typeparam>
        /// <param name="nativeType"><typeparamref name="TNative"/> type instance for this association</param>
        /// <param name="debugType"><typeparamref name="TDebug"/> type instance for this association</param>
        /// <returns><see cref="IDebugType{NativeT, DebugT}"/> implementation for the specified association</returns>
        public static IDebugType<TNative, TDebug> Create<TNative, TDebug>( TNative nativeType
                                                                         , TDebug debugType
                                                                         )
            where TNative : class, ITypeRef
            where TDebug : DIType
        {
            return new DebugType<TNative, TDebug>( nativeType ) { DIType = debugType };
        }

        /// <summary>Convenience extensions for determining if the <see cref="DIType"/> property is valid</summary>
        /// <param name="debugType">Debug type to test for valid Debug information</param>
        /// <remarks>In LLVM Debug information a <see langword="null"/> <see cref="Llvm.NET.DebugInfo.DIType"/> is
        /// used to represent the void type. Thus, looking only at the <see cref="DIType"/> property is
        /// insufficient to distinguish between a type with no debug information and one representing the void
        /// type. This property is used to disambiguate the two possibilities.
        /// </remarks>
        /// <returns><see langword="true"/> if the type has debug information</returns>
        public static bool HasDebugInfo( this IDebugType<ITypeRef, DIType> debugType )
        {
            if( debugType == null )
            {
                throw new ArgumentNullException( nameof( debugType ) );
            }

            return debugType.DIType != null || debugType.NativeType.IsVoid;
        }
    }
}

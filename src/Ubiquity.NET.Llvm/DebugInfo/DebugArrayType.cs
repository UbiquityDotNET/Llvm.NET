// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Provides debug information binding between an <see cref="IArrayType"/> and a <see cref="DICompositeType"/></summary>
    /// <seealso href="xref:llvm_langref#dicompositetype">DICompositeType</seealso>
    public class DebugArrayType
        : DebugType<IArrayType, DICompositeType>
        , IArrayType
    {
        /// <summary>Initializes a new instance of the <see cref="DebugArrayType"/> class</summary>
        /// <param name="llvmType">Underlying LLVM array type to bind debug info to</param>
        /// <param name="elementType">Array element type with debug information</param>
        /// <param name="diBuilder">Debug Information builder to use to build the information for this type</param>
        /// <param name="count">Number of elements in the array</param>
        /// <param name="lowerBound">Lower bound of the array [default = 0]</param>
        /// <param name="alignment">Alignment for the type</param>
        public DebugArrayType( IArrayType llvmType
                             , IDebugType<ITypeRef, DIType> elementType
                             , IDIBuilder diBuilder
                             , uint count
                             , uint lowerBound = 0
                             , uint alignment = 0
                             )
            : base( llvmType, BuildDebugType( llvmType, elementType, diBuilder, count, lowerBound, alignment ) )
        {
            ArgumentNullException.ThrowIfNull( elementType );
            ArgumentNullException.ThrowIfNull( elementType.DebugInfoType );

            DebugElementType = elementType;
        }

        /// <summary>Initializes a new instance of the <see cref="DebugArrayType"/> class.</summary>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="diBuilder">Debug Information builder to use to build the information for this type</param>
        /// <param name="count">Number of elements in the array</param>
        /// <param name="lowerBound"><see cref="LowerBound"/> value for the array indices [Default: 0]</param>
        public DebugArrayType( IDebugType<ITypeRef, DIType> elementType, IDIBuilder diBuilder, uint count, uint lowerBound = 0 )
            : this( elementType.ThrowIfNull().CreateArrayType( count )
                  , elementType
                  , diBuilder
                  , count
                  , lowerBound
                  )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DebugArrayType"/> class.</summary>
        /// <param name="llvmType">Native LLVM type for the elements</param>
        /// <param name="diBuilder">Debug Information builder to use to build the information for this type</param>
        /// <param name="elementType">Debug type of the array elements</param>
        /// <param name="count">Number of elements in the array</param>
        /// <param name="lowerBound"><see cref="LowerBound"/> value for the array indices [Default: 0]</param>
        public DebugArrayType( IArrayType llvmType, IDIBuilder diBuilder, DIType elementType, uint count, uint lowerBound = 0 )
            : this( DebugType.Create( llvmType.ThrowIfNull().ElementType, elementType ), diBuilder, count, lowerBound )
        {
        }

        /// <summary>Gets the full <see cref="IDebugType{NativeT, DebugT}"/> type for the elements</summary>
        public IDebugType<ITypeRef, DIType> DebugElementType { get; }

        /// <inheritdoc/>
        public ITypeRef ElementType => DebugElementType;

        /// <inheritdoc/>
        public uint Length => NativeType.Length;

        /// <summary>Gets the lower bound of the array - usually, but not always, zero</summary>
        public uint LowerBound { get; } /*=> DebugInfoType.GetOperand<DISubRange>( 0 ).LowerBound;*/

        /// <summary>Resolves a temporary metadata node for the array if full size information wasn't available at creation time</summary>
        /// <param name="layout">Type layout information</param>
        /// <param name="diBuilder">Debug information builder for creating the new debug information</param>
        public void ResolveTemporary( IDataLayout layout, IDIBuilder diBuilder )
        {
            ArgumentNullException.ThrowIfNull( layout );

            if(DebugInfoType != null && DebugInfoType.IsTemporary && !DebugInfoType.IsResolved)
            {
                DebugInfoType = diBuilder.CreateArrayType( layout.BitSizeOf( NativeType )
                                                         , layout.AbiBitAlignmentOf( NativeType )
                                                         , DebugElementType.DebugInfoType!
                                                         , diBuilder.CreateSubRange( LowerBound, NativeType.Length )
                                                         );
            }
        }

        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Result is anything but 'simplified'" )]
        private static DICompositeType BuildDebugType( IArrayType llvmType
                                                     , IDebugType<ITypeRef, DIType> elementType
                                                     , IDIBuilder diBuilder
                                                     , uint count
                                                     , uint lowerBound
                                                     , uint alignment
                                                     )
        {
            ArgumentNullException.ThrowIfNull( llvmType );
            ArgumentNullException.ThrowIfNull( elementType );

            if(llvmType.ElementType.GetTypeRef() != elementType.GetTypeRef())
            {
                throw new ArgumentException( Resources.ElementType_doesn_t_match_array_element_type );
            }

            if(llvmType.IsSized)
            {
                return diBuilder.CreateArrayType( diBuilder.OwningModule.Layout.BitSizeOf( llvmType )
                                                , alignment
                                                , elementType.DebugInfoType! // validated not null in constructor
                                                , diBuilder.CreateSubRange( lowerBound, count )
                                                );
            }

            return diBuilder.CreateReplaceableCompositeType( Tag.ArrayType
                                                           , string.Empty
                                                           , diBuilder.CompileUnit ?? default
                                                           , default
                                                           , 0
                                                           );
        }
    }
}

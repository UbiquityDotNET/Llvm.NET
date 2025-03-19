// -----------------------------------------------------------------------
// <copyright file="DebugUnionType.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug representation of a union type</summary>
    /// <remarks>The underlying native LLVM type is a structure with a single member</remarks>
    /// <seealso href="xref:llvm_langref#dicompositetype">LLVM DICompositeType</seealso>
    public class DebugUnionType
        : DebugType<INamedStructuralType, DICompositeType>
        , INamedStructuralType
    {
        /// <summary>Initializes a new instance of the <see cref="DebugUnionType"/> class.</summary>
        /// <param name="llvmType">Underlying native type this debug type describes</param>
        /// <param name="diBuilder">Debug information builder to use</param>
        /// <param name="scope">Scope containing this type</param>
        /// <param name="name">Debug/source name of the type</param>
        /// <param name="file">Source file containing this type</param>
        /// <param name="line">Line number for this type</param>
        /// <param name="debugFlags">Debug flags for this type</param>
        /// <param name="elements">Descriptors for the members of the type</param>
        public DebugUnionType( IStructType llvmType
                             , ref readonly DIBuilder diBuilder
                             , DIScope? scope
                             , string name
                             , DIFile? file
                             , uint line
                             , DebugInfoFlags debugFlags
                             , IEnumerable<DebugMemberInfo> elements
                             )
            : base( llvmType.ThrowIfNull()
                  , diBuilder.CreateReplaceableCompositeType( Tag.UnionType
                                                            , name
                                                            , scope
                                                            , file
                                                            , line
                                                            )
                  )
        {
            if( !llvmType.IsOpaque )
            {
                throw new ArgumentException( Resources.Struct_type_used_as_basis_for_a_union_must_not_have_a_body, nameof( llvmType ) );
            }

            SetBody( in diBuilder, scope, file, line, debugFlags, elements );
        }

        /// <summary>Initializes a new instance of the <see cref="DebugUnionType"/> class.</summary>
        /// <param name="diBuilder">Debug information builder to use</param>
        /// <param name="nativeName">Native LLVM type name</param>
        /// <param name="scope">Scope containing this type</param>
        /// <param name="name">Debug/source name of the type</param>
        /// <param name="file">Source file containing this type</param>
        /// <param name="line">Line number for this type</param>
        public DebugUnionType( ref readonly DIBuilder diBuilder
                             , string nativeName
                             , DIScope? scope
                             , string name
                             , DIFile? file
                             , uint line = 0
                             )
            : base( diBuilder.OwningModule.Context.CreateStructType( nativeName )
                  , diBuilder.CreateReplaceableCompositeType( Tag.UnionType
                                                            , name
                                                            , scope
                                                            , file
                                                            , line
                                                            )
                  )
        {
        }

        /// <inheritdoc/>
        public bool IsOpaque => NativeType.IsOpaque;

        /// <inheritdoc/>
        public IReadOnlyList<ITypeRef> Members => NativeType.Members;

        /// <inheritdoc/>
        public string Name => NativeType.Name;

        /// <summary>Gets the description of each member of the type</summary>
        public IReadOnlyList<DebugMemberInfo> DebugMembers { get; private set; } = new List<DebugMemberInfo>( ).AsReadOnly( );

        /// <summary>Sets the body of the union type</summary>
        /// <param name="diBuilder">Debug information to use</param>
        /// <param name="scope">Scope containing this type</param>
        /// <param name="file">File for the type</param>
        /// <param name="line">line number for the type</param>
        /// <param name="debugFlags">Flags for the type</param>
        /// <param name="debugElements">Descriptors for each element in the type</param>
        public void SetBody( ref readonly DIBuilder diBuilder
                           , DIScope? scope
                           , DIFile? file
                           , uint line
                           , DebugInfoFlags debugFlags
                           , IEnumerable<DebugMemberInfo> debugElements
                           )
        {
            ArgumentNullException.ThrowIfNull( debugElements );

            if( diBuilder.OwningModule.Layout == null )
            {
                throw new ArgumentException( Resources.Module_needs_Layout_to_build_basic_types, nameof( diBuilder ) );
            }

            // Native body is a single element of a type with the largest size
            ulong maxSize = 0UL;
            var nativeMembers = new ITypeRef[1];
            foreach( var elem in debugElements )
            {
                ulong? bitSize = elem.ExplicitLayout?.BitSize ?? diBuilder.OwningModule.Layout.BitSizeOf( elem.DebugType );
                if( !bitSize.HasValue )
                {
                    throw new ArgumentException( Resources.Cannot_determine_layout_for_element__The_element_must_have_an_explicit_layout_or_the_module_has_a_layout_to_use, nameof( debugElements ) );
                }

                if( maxSize >= bitSize.Value )
                {
                    continue;
                }

                maxSize = bitSize.Value;
                nativeMembers[ 0 ] = elem.DebugType;
            }

            var nativeType = ( IStructType )NativeType;
            nativeType.SetBody( false, nativeMembers );

            // Debug info contains details of each member of the union
            DebugMembers = new ReadOnlyCollection<DebugMemberInfo>( debugElements as IList<DebugMemberInfo> ?? [ .. debugElements ] );
            var memberTypes = new DIDerivedType[DebugMembers.Count];
            for(int i = 0; i < DebugMembers.Count; ++i)
            {
                memberTypes[i] = CreateMemberType( in diBuilder, DebugMembers[i]);
            }

            var (unionBitSize, unionAlign)
                = memberTypes.Aggregate( (MaxSize: 0ul, MaxAlign: 0ul)
                                       , ( a, d ) => (Math.Max( a.MaxSize, d.BitSize ), Math.Max( a.MaxAlign, d.BitAlignment ))
                                       );

            var concreteType = diBuilder.CreateUnionType( scope: scope
                                                        , name: DebugInfoType!.Name // not null via construction
                                                        , file: file
                                                        , line: line
                                                        , bitSize: checked((uint)unionBitSize)
                                                        , bitAlign: checked((uint)unionAlign)
                                                        , debugFlags: debugFlags
                                                        , elements: memberTypes
                                                        );
            DebugInfoType = concreteType;
        }

        [SuppressMessage( "Style", "IDE0045:Convert to conditional expression", Justification = "'Simplification' not so simple - degrades to nested conditional operators" )]
        private DIDerivedType CreateMemberType( ref readonly DIBuilder diBuilder, DebugMemberInfo memberInfo )
        {
            ulong bitSize;
            if( memberInfo.ExplicitLayout is not null)
            {
                bitSize = memberInfo.ExplicitLayout.BitSize;
            }
            else if( diBuilder.OwningModule.Layout is not null)
            {
                bitSize = diBuilder.OwningModule.Layout.BitSizeOf( memberInfo.DebugType );
            }
            else
            {
                throw new ArgumentException( "Cannot determine size of member", nameof( memberInfo ) );
            }

            return diBuilder.CreateMemberType( scope: DebugInfoType
                                             , name: memberInfo.Name
                                             , file: memberInfo.File
                                             , line: memberInfo.Line
                                             , bitSize: bitSize
                                             , bitAlign: memberInfo.ExplicitLayout?.BitAlignment ?? 0
                                             , bitOffset: 0
                                             , debugFlags: memberInfo.DebugInfoFlags
                                             , type: memberInfo.DebugType.DebugInfoType
                                             );
        }
    }
}

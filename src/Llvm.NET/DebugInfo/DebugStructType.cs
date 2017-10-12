// <copyright file="DebugStructType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Llvm.NET.Types;
using Ubiquity.ArgValidators;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a structure type</summary>
    /// <seealso href="xref:llvm_langref#dicompositetype">LLVM DICompositeType</seealso>
    public class DebugStructType
        : DebugType<IStructType, DICompositeType>
        , IStructType
    {
        public DebugStructType( NativeModule module
                              , string nativeName
                              , DIScope scope
                              , string name
                              , DIFile diFile
                              , uint line
                              , DebugInfoFlags debugFlags
                              , IEnumerable<DebugMemberInfo> debugElements
                              , DIType derivedFrom = null
                              , bool packed = false
                              , uint? bitSize = null
                              , uint bitAlignment = 0
                              )
        {
            module.ValidateNotNull( nameof( module ) );
            DebugMembers = new ReadOnlyCollection<DebugMemberInfo>( debugElements as IList<DebugMemberInfo> ?? debugElements.ToList( ) );

            NativeType = module.Context.CreateStructType( nativeName, packed, debugElements.Select( e => e.DebugType ).ToArray( ) );
            DIType = module.DIBuilder.CreateReplaceableCompositeType( Tag.StructureType
                                                                    , name
                                                                    , scope
                                                                    , diFile
                                                                    , line
                                                                    );

            var memberTypes = from memberInfo in DebugMembers
                              select CreateMemberType( module, memberInfo );

            var concreteType = module.DIBuilder.CreateStructType( scope: scope
                                                                , name: name
                                                                , file: diFile
                                                                , line: line
                                                                , bitSize: bitSize ?? module.Layout.BitSizeOf( NativeType )
                                                                , bitAlign: bitAlignment
                                                                , debugFlags: debugFlags
                                                                , derivedFrom: derivedFrom
                                                                , elements: memberTypes
                                                                );

            // assignment performs RAUW
            DIType = concreteType;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "ValidateNotNull" )]
        public DebugStructType( IStructType llvmType
                              , NativeModule module
                              , DIScope scope
                              , string name
                              , DIFile file
                              , uint line
                              , DebugInfoFlags debugFlags
                              , DIType derivedFrom
                              , IEnumerable<DIType> elements
                              , uint alignment = 0
                              )
            : base( llvmType )
        {
            module.ValidateNotNull( nameof( module ) );
            DIType = module.DIBuilder
                           .CreateStructType( scope
                                            , name
                                            , file
                                            , line
                                            , module.Layout.BitSizeOf( llvmType )
                                            , alignment
                                            , debugFlags
                                            , derivedFrom
                                            , elements
                                            );
        }

        public DebugStructType( IStructType llvmType
                              , NativeModule module
                              , DIScope scope
                              , string name
                              , DIFile file
                              , uint line
                              )
            : base( llvmType )
        {
            DIType = module.ValidateNotNull( nameof( module ) )
                           .DIBuilder
                           .CreateReplaceableCompositeType( Tag.StructureType
                                                          , name
                                                          , scope
                                                          , file
                                                          , line
                                                          );
        }

        public DebugStructType( NativeModule module
                              , string nativeName
                              , DIScope scope
                              , string name
                              , DIFile file = null
                              , uint line = 0
                              )
            : this( module.ValidateNotNull( nameof( module ) ).Context.CreateStructType( nativeName )
                  , module
                  , scope
                  , name
                  , file
                  , line
                  )
        {
        }

        public bool IsOpaque => NativeType.IsOpaque;

        public bool IsPacked => NativeType.IsPacked;

        public IReadOnlyList<ITypeRef> Members => NativeType.Members;

        public string Name => NativeType.Name;

        public void SetBody( bool packed, params ITypeRef[ ] elements )
        {
            NativeType.SetBody( packed, elements );
        }

        public void SetBody( bool packed
                           , NativeModule module
                           , DIScope scope
                           , DIFile diFile
                           , uint line
                           , DebugInfoFlags debugFlags
                           , IEnumerable<DebugMemberInfo> debugElements
                           )
        {
            var debugMembersArray = debugElements as IList<DebugMemberInfo> ?? debugElements.ToList();
            var nativeElements = debugMembersArray.Select( e => e.DebugType.NativeType );
            SetBody( packed, module, scope, diFile, line, debugFlags, nativeElements, debugMembersArray );
        }

        public void SetBody( bool packed
                           , NativeModule module
                           , DIScope scope
                           , DIFile diFile
                           , uint line
                           , DebugInfoFlags debugFlags
                           , IEnumerable<ITypeRef> nativeElements
                           , IEnumerable<DebugMemberInfo> debugElements
                           , DIType derivedFrom = null
                           , uint? bitSize = null
                           , uint bitAlignment = 0
                           )
        {
            DebugMembers = new ReadOnlyCollection<DebugMemberInfo>( debugElements as IList<DebugMemberInfo> ?? debugElements.ToList( ) );
            SetBody( packed, nativeElements.ToArray() );
            var memberTypes = from memberInfo in DebugMembers
                              select CreateMemberType( module, memberInfo );

            var concreteType = module.DIBuilder.CreateStructType( scope: scope
                                                                , name: DIType.Name
                                                                , file: diFile
                                                                , line: line
                                                                , bitSize: bitSize ?? module.Layout.BitSizeOf( NativeType )
                                                                , bitAlign: bitAlignment
                                                                , debugFlags: debugFlags
                                                                , derivedFrom: derivedFrom
                                                                , elements: memberTypes
                                                                );
            DIType = concreteType;
        }

        public IReadOnlyList<DebugMemberInfo> DebugMembers { get; private set; }

        private DIDerivedType CreateMemberType( NativeModule module, DebugMemberInfo memberInfo )
        {
            UInt64 bitSize;
            UInt32 bitAlign;
            UInt64 bitOffset;

            // if explicit layout info provided, use it;
            // otherwise use module.Layout as the default
            if( memberInfo.ExplicitLayout != null )
            {
                bitSize = memberInfo.ExplicitLayout.BitSize;
                bitAlign = memberInfo.ExplicitLayout.BitAlignment;
                bitOffset = memberInfo.ExplicitLayout.BitOffset;
            }
            else
            {
                bitSize = module.Layout.BitSizeOf( memberInfo.DebugType.NativeType );
                bitAlign = 0;
                bitOffset = module.Layout.BitOffsetOfElement( NativeType, memberInfo.Index );
            }

            return module.DIBuilder.CreateMemberType( scope: DIType
                                                    , name: memberInfo.Name
                                                    , file: memberInfo.File
                                                    , line: memberInfo.Line
                                                    , bitSize: bitSize
                                                    , bitAlign: bitAlign
                                                    , bitOffset: bitOffset
                                                    , debugFlags: memberInfo.DebugInfoFlags
                                                    , type: memberInfo.DebugType.DIType
                                                    );
        }
    }
}

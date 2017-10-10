// <copyright file="DebugUnionType.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Llvm.NET.Types;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug representation of a union type</summary>
    /// <remarks>The underlying native LLVM type is a structure with a single member</remarks>
    /// <seealso href="xref:llvm_langref#dicompositetype">LLVM DICompositeType</seealso>
    public class DebugUnionType
        : DebugType<INamedStructuralType, DICompositeType>
        , INamedStructuralType
    {
        public DebugUnionType( IStructType llvmType
                             , NativeModule module
                             , DIScope scope
                             , string name
                             , DIFile file
                             , uint line
                             , DebugInfoFlags debugFlags
                             , IEnumerable<DebugMemberInfo> elements
                             )
            : base( llvmType )
        {
            if( llvmType == null )
            {
                throw new ArgumentNullException( nameof( llvmType ) );
            }

            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( file == null )
            {
                throw new ArgumentNullException( nameof( file ) );
            }

            if( !llvmType.IsOpaque )
            {
                throw new ArgumentException( "Struct type used as basis for a union must not have a body", nameof( llvmType ) );
            }

            DIType = module.DIBuilder
                           .CreateReplaceableCompositeType( Tag.UnionType
                                                          , name
                                                          , scope
                                                          , file
                                                          , line
                                                          );
            SetBody( module, scope, file, line, debugFlags, elements );
        }

        public DebugUnionType( NativeModule module
                             , string nativeName
                             , DIScope scope
                             , string name
                             , DIFile file
                             , uint line = 0
                             )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( file == null )
            {
                throw new ArgumentNullException( nameof( file ) );
            }

            NativeType = module.Context.CreateStructType( nativeName );
            DIType = module.DIBuilder
                           .CreateReplaceableCompositeType( Tag.UnionType
                                                          , name
                                                          , scope
                                                          , file
                                                          , line
                                                          );
        }

        public bool IsOpaque => NativeType.IsOpaque;

        public IReadOnlyList<ITypeRef> Members => NativeType.Members;

        public string Name => NativeType.Name;

        public IReadOnlyList<DebugMemberInfo> DebugMembers { get; private set; }

        public void SetBody( NativeModule module
                           , DIScope scope
                           , DIFile diFile
                           , uint line
                           , DebugInfoFlags debugFlags
                           , IEnumerable<DebugMemberInfo> debugElements
                           )
        {
            if( module == null )
            {
                throw new ArgumentNullException( nameof( module ) );
            }

            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( debugElements == null )
            {
                throw new ArgumentNullException( nameof( debugElements ) );
            }

            if( module.Layout == null )
            {
                throw new ArgumentException( "Module needs Layout to build basic types", nameof( module ) );
            }

            // Native body is a single element of a type with the largest size
            ulong maxSize = 0UL;
            ITypeRef[ ] nativeMembers = { null };
            foreach( var elem in debugElements )
            {
                var bitSize = elem.ExplicitLayout?.BitSize ?? module.Layout?.BitSizeOf( elem.DebugType );
                if( !bitSize.HasValue )
                {
                    throw new ArgumentException( "Cannot determine layout for element; The element must have an explicit layout or the module has a layout to use", nameof( debugElements ) );
                }

                if( maxSize < bitSize.Value )
                {
                    maxSize = bitSize.Value;
                    nativeMembers[ 0 ] = elem.DebugType;
                }
            }

            var nativeType = ( IStructType )NativeType;
            nativeType.SetBody( false, nativeMembers );

            // Debug info contains details of each member of the union
            DebugMembers = new ReadOnlyCollection<DebugMemberInfo>( debugElements as IList<DebugMemberInfo> ?? debugElements.ToList( ) );
            var memberTypes = from memberInfo in DebugMembers
                              select module.DIBuilder.CreateMemberType( scope: DIType
                                                                      , name: memberInfo.Name
                                                                      , file: memberInfo.File
                                                                      , line: memberInfo.Line
                                                                      , bitSize: ( memberInfo.ExplicitLayout?.BitSize ?? module.Layout?.BitSizeOf( memberInfo.DebugType ) ).Value
                                                                      , bitAlign: memberInfo.ExplicitLayout?.BitAlignment ?? 0
                                                                      , bitOffset: 0
                                                                      , debugFlags: memberInfo.DebugInfoFlags
                                                                      , type: memberInfo.DebugType.DIType
                                                                      );

            var concreteType = module.DIBuilder.CreateUnionType( scope: scope
                                                               , name: DIType.Name
                                                               , file: diFile
                                                               , line: line
                                                               , bitSize: 0 // TODO: find largest sized member
                                                               , bitAlign: 0 // TODO: Find most restrictive alignment
                                                               , debugFlags: debugFlags
                                                               , elements: memberTypes
                                                               );
            DIType = concreteType;
        }
    }
}

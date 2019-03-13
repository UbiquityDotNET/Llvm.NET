// <copyright file="DebugInfoBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugInfoBuilder is a factory class for creating DebugInformation for an LLVM <see cref="BitcodeModule"/></summary>
    public sealed partial class DebugInfoBuilder
    {
#pragma warning disable CA1008 // Enums should have zero value.
        internal enum LLVMDwarfTag : ushort
        {
            ArrayType = 0x01,
            ClassType = 0x02,
            EntryPoint = 0x03,
            EnumerationType = 0x04,
            FormalParameter = 0x05,
            ImportedDeclaration = 0x08,
            Label = 0x0a,
            LexicalBlock = 0x0b,
            Member = 0x0d,
            PointerType = 0x0f,
            ReferenceType = 0x10,
            CompileUnit = 0x11,
            StringType = 0x12,
            StructureType = 0x13,
            SubroutineType = 0x15,
            TypeDef = 0x16,
            UnionType = 0x17,
            UnspecifiedParameters = 0x18,
            Variant = 0x19,
            CommonBlock = 0x1a,
            CommonInclusion = 0x1b,
            Inheritance = 0x1c,
            InlinedSubroutine = 0x1d,
            Module = 0x1e,
            PtrToMemberType = 0x1f,
            SetType = 0x20,
            SubrangeType = 0x21,
            WithStatement = 0x22,
            AccessDeclaration = 0x23,
            BaseType = 0x24,
            CatchBlock = 0x25,
            ConstType = 0x26,
            Constant = 0x27,
            Enumerator = 0x28,
            FileType = 0x29,
            Friend = 0x2a,
            NameList = 0x2b,
            NameListItem = 0x2c,
            PackedType = 0x2d,
            SubProgram = 0x2e,
            TemplateTypeParameter = 0x2f,
            TemplateValueParameter = 0x30,
            ThrownType = 0x31,
            TryBlock = 0x32,
            VariantPart = 0x33,
            Variable = 0x34,
            VolatileType = 0x35,
            DwarfProcedure = 0x36,
            RestrictType = 0x37,
            InterfaceType = 0x38,
            Namespace = 0x39,
            ImportedModule = 0x3a,
            UnspecifiedType = 0x3b,
            PartialUnit = 0x3c,
            ImportedUnit = 0x3d,
            Condition = 0x3f,
            SharedType = 0x40,
            TypeUnit = 0x41,
            RValueReferenceType = 0x42,
            TemplateAlias = 0x43,

            // New in DWARF 5:
            CoArrayType = 0x44,
            GenericSubrange = 0x45,
            DynamicType = 0x46,

            MipsLoop = 0x4081,
            FormatLabel = 0x4101,
            FunctionTemplate = 0x4102,
            ClassTemplate = 0x4103,
            GnuTemplateTemplateParam = 0x4106,
            GnuTemplateParameterPack = 0x4107,
            GnuFormalParameterPack = 0x4108,
            LoUser = 0x4080,
            AppleProperty = 0x4200,
            HiUser = 0xffff
        }
#pragma warning restore CA1008 // Enums should have zero value.

        internal static class NativeMethods
        {
            // ReSharper disable IdentifierTypo
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMDIBuilderRef LLVMNewDIBuilder( LLVMModuleRef m, [MarshalAs( UnmanagedType.Bool )]bool allowUnresolved );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMDIBuilderFinalize( LLVMDIBuilderRef d );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateCompileUnit2( LLVMDIBuilderRef D
                                                                                 , UInt32 Language
                                                                                 , LLVMMetadataRef /*DIFile*/ fileRef
                                                                                 , [MarshalAs( UnmanagedType.LPStr )] string Producer
                                                                                 , IntPtr ProduceLen
                                                                                 , [MarshalAs( UnmanagedType.Bool )] bool Optimized
                                                                                 , [MarshalAs( UnmanagedType.LPStr )] string Flags
                                                                                 , IntPtr FlagsLen
                                                                                 , UInt32 RuntimeVersion
                                                                                 , [MarshalAs( UnmanagedType.LPStr )] string splitName
                                                                                 , IntPtr splitNameLen
                                                                                 , DwarfEmissionKind kind
                                                                                 , UInt32 oid
                                                                                 , [MarshalAs( UnmanagedType.Bool )] bool splitDebugInlining
                                                                                 , [MarshalAs( UnmanagedType.Bool )] bool debugInfoForProfiling
                                                                                 );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateFile( LLVMDIBuilderRef D
                                                                         , [MarshalAs( UnmanagedType.LPStr )] string File
                                                                         , IntPtr fileLen
                                                                         , [MarshalAs( UnmanagedType.LPStr )] string Dir
                                                                         , IntPtr dirLen
                                                                         );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock( LLVMDIBuilderRef D, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Line, UInt32 Column );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile( LLVMDIBuilderRef D, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Discriminator );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateFunction( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 Line, LLVMMetadataRef CompositeType, int IsLocalToUnit, int IsDefinition, UInt32 ScopeLine, UInt32 Flags, int IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 Line, LLVMMetadataRef CompositeType, int IsLocalToUnit, int IsDefinition, UInt32 ScopeLine, UInt32 Flags, int IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateAutoVariable( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 Line, LLVMMetadataRef Ty, int AlwaysPreserve, UInt32 Flags );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateParameterVariable( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, UInt32 ArgNo, LLVMMetadataRef File, UInt32 Line, LLVMMetadataRef Ty, int AlwaysPreserve, UInt32 Flags );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateBasicType( LLVMDIBuilderRef D, [MarshalAs( UnmanagedType.LPStr )] string Name, UInt64 SizeInBits, UInt32 Encoding );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreatePointerType( LLVMDIBuilderRef D, LLVMMetadataRef PointeeType, UInt64 SizeInBits, UInt32 AlignInBits, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateQualifiedType( LLVMDIBuilderRef Dref, UInt32 Tag, LLVMMetadataRef BaseType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateSubroutineType( LLVMDIBuilderRef D, LLVMMetadataRef ParameterTypes, UInt32 Flags );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateStructType( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 Line, UInt64 SizeInBits, UInt32 AlignInBits, UInt32 Flags, LLVMMetadataRef DerivedFrom, LLVMMetadataRef ElementTypes );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateUnionType( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 Line, UInt64 SizeInBits, UInt32 AlignInBits, UInt32 Flags, LLVMMetadataRef ElementTypes );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateMemberType( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 Line, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, UInt32 Flags, LLVMMetadataRef Ty );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateArrayType( LLVMDIBuilderRef D, UInt64 SizeInBits, UInt32 AlignInBits, LLVMMetadataRef ElementType, LLVMMetadataRef Subscripts );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateVectorType( LLVMDIBuilderRef D, UInt64 SizeInBits, UInt32 AlignInBits, LLVMMetadataRef ElementType, LLVMMetadataRef Subscripts );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateTypedef( LLVMDIBuilderRef D, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 Line, LLVMMetadataRef Context );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange( LLVMDIBuilderRef D, Int64 Lo, Int64 Count );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateArray( LLVMDIBuilderRef D, out LLVMMetadataRef Data, UInt64 Length );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray( LLVMDIBuilderRef D, out LLVMMetadataRef Data, UInt64 Length );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRef Dref, out Int64 Addr, UInt64 Length );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMDIBuilderInsertDeclareAtEnd( LLVMDIBuilderRef D, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMBasicBlockRef Block );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMDIBuilderInsertValueAtEnd( LLVMDIBuilderRef D, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMBasicBlockRef Block );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMDIBuilderInsertValueBefore( LLVMDIBuilderRef Dref, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DL, LLVMValueRef InsertBefore );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateEnumerationType( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef File, UInt32 LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, LLVMMetadataRef Elements, LLVMMetadataRef UnderlyingType, [MarshalAs( UnmanagedType.LPStr )]string UniqueId );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef D, [MarshalAs( UnmanagedType.LPStr )]string Name, Int64 Val );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression( LLVMDIBuilderRef Dref, LLVMMetadataRef Context, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )]bool isLocalToUnit, LLVMMetadataRef expression, LLVMMetadataRef Decl, UInt32 AlignInBits );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMDIBuilderInsertDeclareBefore( LLVMDIBuilderRef Dref, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMValueRef InsertBefore );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType( LLVMDIBuilderRef Dref, UInt32 Tag, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Line, UInt32 RuntimeLang, UInt64 SizeInBits, UInt64 AlignInBits, UInt32 Flags );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIBuilderCreateNamespace( LLVMDIBuilderRef Dref, LLVMMetadataRef scope, [MarshalAs( UnmanagedType.LPStr )] string name, [MarshalAs( UnmanagedType.Bool )]bool exportSymbols );
        }
    }
}

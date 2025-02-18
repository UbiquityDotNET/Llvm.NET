// -----------------------------------------------------------------------
// <copyright file="MetadataBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LibLLVMDwarfAttributeEncoding
        : Int32
    {
        Invalid = 0,
        address = 1,
        boolean = 2,
        complex_float = 3,
        @float = 4,
        signed = 5,
        signed_char = 6,
        unsigned = 7,
        unsigned_char = 8,
        imaginary_float = 9,
        packed_decimal = 10,
        numeric_string = 11,
        edited = 12,
        signed_fixed = 13,
        unsigned_fixed = 14,
        decimal_float = 15,
        UTF = 16,
        UCS = 17,
        ASCII = 18,
        HP_complex_float = 129,
        HP_float128 = 130,
        HP_complex_float128 = 131,
        HP_floathpintel = 132,
        HP_imaginary_float90 = 133,
        HP_imaginary_float128 = 134,
        lo_user = 128,
        hi_user = 255,
    }

    public enum LibLLVMDwarfTag
        : Int32
    {
        @null = 0,
        array_type = 1,
        class_type = 2,
        entry_point = 3,
        enumeration_type = 4,
        formal_parameter = 5,
        imported_declaration = 8,
        label = 10,
        lexical_block = 11,
        member = 13,
        pointer_type = 15,
        reference_type = 16,
        compile_unit = 17,
        string_type = 18,
        structure_type = 19,
        subroutine_type = 21,
        typedef = 22,
        union_type = 23,
        unspecified_parameters = 24,
        variant = 25,
        common_block = 26,
        common_inclusion = 27,
        inheritance = 28,
        inlined_subroutine = 29,
        module = 30,
        ptr_to_member_type = 31,
        set_type = 32,
        subrange_type = 33,
        with_stmt = 34,
        access_declaration = 35,
        base_type = 36,
        catch_block = 37,
        const_type = 38,
        constant = 39,
        enumerator = 40,
        file_type = 41,
        friend = 42,
        namelist = 43,
        namelist_item = 44,
        packed_type = 45,
        subprogram = 46,
        template_type_parameter = 47,
        template_value_parameter = 48,
        thrown_type = 49,
        try_block = 50,
        variant_part = 51,
        variable = 52,
        volatile_type = 53,
        dwarf_procedure = 54,
        restrict_type = 55,
        interface_type = 56,
        @namespace = 57,
        imported_module = 58,
        unspecified_type = 59,
        partial_unit = 60,
        imported_unit = 61,
        condition = 63,
        shared_type = 64,
        type_unit = 65,
        rvalue_reference_type = 66,
        template_alias = 67,
        coarray_type = 68,
        generic_subrange = 69,
        dynamic_type = 70,
        atomic_type = 71,
        call_site = 72,
        call_site_parameter = 73,
        skeleton_unit = 74,
        immutable_type = 75,
        MIPS_loop = 16513,
        format_label = 16641,
        function_template = 16642,
        class_template = 16643,
        GNU_BINCL = 16644,
        GNU_EINCL = 16645,
        GNU_template_template_param = 16646,
        GNU_template_parameter_pack = 16647,
        GNU_formal_parameter_pack = 16648,
        GNU_call_site = 16649,
        GNU_call_site_parameter = 16650,
        APPLE_property = 16896,
        SUN_function_template = 16897,
        SUN_class_template = 16898,
        SUN_struct_template = 16899,
        SUN_union_template = 16900,
        SUN_indirect_inheritance = 16901,
        SUN_codeflags = 16902,
        SUN_memop_info = 16903,
        SUN_omp_child_func = 16904,
        SUN_rtti_descriptor = 16905,
        SUN_dtor_info = 16906,
        SUN_dtor = 16907,
        SUN_f90_interface = 16908,
        SUN_fortran_vax_structure = 16909,
        SUN_hi = 17151,
        LLVM_ptrauth_type = 17152,
        ALTIUM_circ_type = 20737,
        ALTIUM_mwa_circ_type = 20738,
        ALTIUM_rev_carry_type = 20739,
        ALTIUM_rom = 20753,
        LLVM_annotation = 24576,
        GHS_namespace = 32772,
        GHS_using_namespace = 32773,
        GHS_using_declaration = 32774,
        GHS_template_templ_param = 32775,
        UPC_shared_type = 34661,
        UPC_strict_type = 34662,
        UPC_relaxed = 34663,
        PGI_kanji_type = 40960,
        PGI_interface_block = 40992,
        BORLAND_property = 45056,
        BORLAND_Delphi_string = 45057,
        BORLAND_Delphi_dynamic_array = 45058,
        BORLAND_Delphi_set = 45059,
        BORLAND_Delphi_variant = 45060,
    }

    public enum LibLLVMMetadataKind
        : Int32
    {
        MDString = 0,
        ConstantAsMetadata = 1,
        LocalAsMetadata = 2,
        DistinctMDOperandPlaceholder = 3,
        DIArgList = 4,
        MDTuple = 5,
        DILocation = 6,
        DIExpression = 7,
        DIGlobalVariableExpression = 8,
        GenericDINode = 9,
        DISubrange = 10,
        DIEnumerator = 11,
        DIBasicType = 12,
        DIDerivedType = 13,
        DICompositeType = 14,
        DISubroutineType = 15,
        DIFile = 16,
        DICompileUnit = 17,
        DISubprogram = 18,
        DILexicalBlock = 19,
        DILexicalBlockFile = 20,
        DINamespace = 21,
        DIModule = 22,
        DITemplateTypeParameter = 23,
        DITemplateValueParameter = 24,
        DIGlobalVariable = 25,
        DILocalVariable = 26,
        DILabel = 27,
        DIObjCProperty = 28,
        DIImportedEntity = 29,
        DIAssignID = 30,
        DIMacro = 31,
        DIMacroFile = 32,
        DICommonBlock = 33,
        DIStringType = 34,
        DIGenericSubrange = 35,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LibLLVMDwarfAttributeEncoding LibLLVMDIBasicTypeGetEncoding( LLVMMetadataRef basicType );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef F );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMContextRefAlias LibLLVMGetNodeContext( LLVMMetadataRef node );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataRef LibLLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef D, LLVMMetadataRef Scope, [MarshalUsing( typeof(AnsiStringMarshaller) )]string Name, size_t NameLen, [MarshalUsing( typeof(AnsiStringMarshaller) )]string LinkageName, size_t LinakgeNameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )]bool isLocalToUnit, [MarshalAs( UnmanagedType.Bool )]bool isDefinition, uint ScopeLine, LLVMDIOption Flags, [MarshalAs( UnmanagedType.Bool )]bool isOptimized );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef subProgram );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LibLLVMDwarfTag LibLLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataRef LibLLVMDILocationGetInlinedAtScope( LLVMMetadataRef location );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMMetadataAsString( LLVMMetadataRef descriptor );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial UInt32 LibLLVMMDNodeGetNumOperands( LLVMMetadataRef node );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LibLLVMMDOperandRef LibLLVMMDNodeGetOperand( LLVMMetadataRef node, UInt32 index );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMMDNodeReplaceOperand( LLVMMetadataRef node, UInt32 index, LLVMMetadataRef operand );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataRef LibLLVMGetOperandNode( LibLLVMMDOperandRef operand );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMModuleRefAlias LibLLVMNamedMetadataGetParentModule( LLVMNamedMDNodeRef namedMDNode );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMNamedMetadataEraseFromParent( LLVMNamedMDNodeRef namedMDNode );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataKind LibLLVMGetMetadataID( LLVMMetadataRef md );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial uint LibLLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataRef LibLLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, uint index );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, uint index, LLVMMetadataRef node );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef node );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial LLVMMetadataRef LibLLVMConstantAsMetadata( LLVMValueRef Val );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LibLLVMGetMDStringText( LLVMMetadataRef mdstring, out uint len );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMAddNamedMetadataOperand2( LLVMModuleRef M, [MarshalUsing( typeof(AnsiStringMarshaller) )]string name, LLVMMetadataRef Val );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LibLLVMSetMetadata2( LLVMValueRef Inst, uint KindID, LLVMMetadataRef MD );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsTemporary( LLVMMetadataRef M );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsResolved( LLVMMetadataRef M );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsUniqued( LLVMMetadataRef M );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsDistinct( LLVMMetadataRef M );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial Int64 LibLLVMDISubRangeGetLowerBounds( LLVMMetadataRef sr, Int64 defaultLowerBound );
    }
}

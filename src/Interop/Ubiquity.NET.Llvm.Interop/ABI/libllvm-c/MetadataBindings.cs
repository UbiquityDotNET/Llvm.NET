// -----------------------------------------------------------------------
// <copyright file="MetadataBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LibLLVMDwarfTag
        : Int32
    {
        DW_TAG_null = 0,
        DW_TAG_array_type = 1,
        DW_TAG_class_type = 2,
        DW_TAG_entry_point = 3,
        DW_TAG_enumeration_type = 4,
        DW_TAG_formal_parameter = 5,
        DW_TAG_imported_declaration = 8,
        DW_TAG_label = 10,
        DW_TAG_lexical_block = 11,
        DW_TAG_member = 13,
        DW_TAG_pointer_type = 15,
        DW_TAG_reference_type = 16,
        DW_TAG_compile_unit = 17,
        DW_TAG_string_type = 18,
        DW_TAG_structure_type = 19,
        DW_TAG_subroutine_type = 21,
        DW_TAG_typedef = 22,
        DW_TAG_union_type = 23,
        DW_TAG_unspecified_parameters = 24,
        DW_TAG_variant = 25,
        DW_TAG_common_block = 26,
        DW_TAG_common_inclusion = 27,
        DW_TAG_inheritance = 28,
        DW_TAG_inlined_subroutine = 29,
        DW_TAG_module = 30,
        DW_TAG_ptr_to_member_type = 31,
        DW_TAG_set_type = 32,
        DW_TAG_subrange_type = 33,
        DW_TAG_with_stmt = 34,
        DW_TAG_access_declaration = 35,
        DW_TAG_base_type = 36,
        DW_TAG_catch_block = 37,
        DW_TAG_const_type = 38,
        DW_TAG_constant = 39,
        DW_TAG_enumerator = 40,
        DW_TAG_file_type = 41,
        DW_TAG_friend = 42,
        DW_TAG_namelist = 43,
        DW_TAG_namelist_item = 44,
        DW_TAG_packed_type = 45,
        DW_TAG_subprogram = 46,
        DW_TAG_template_type_parameter = 47,
        DW_TAG_template_value_parameter = 48,
        DW_TAG_thrown_type = 49,
        DW_TAG_try_block = 50,
        DW_TAG_variant_part = 51,
        DW_TAG_variable = 52,
        DW_TAG_volatile_type = 53,
        DW_TAG_dwarf_procedure = 54,
        DW_TAG_restrict_type = 55,
        DW_TAG_interface_type = 56,
        DW_TAG_namespace = 57,
        DW_TAG_imported_module = 58,
        DW_TAG_unspecified_type = 59,
        DW_TAG_partial_unit = 60,
        DW_TAG_imported_unit = 61,
        DW_TAG_condition = 63,
        DW_TAG_shared_type = 64,
        DW_TAG_type_unit = 65,
        DW_TAG_rvalue_reference_type = 66,
        DW_TAG_template_alias = 67,
        DW_TAG_coarray_type = 68,
        DW_TAG_generic_subrange = 69,
        DW_TAG_dynamic_type = 70,
        DW_TAG_atomic_type = 71,
        DW_TAG_call_site = 72,
        DW_TAG_call_site_parameter = 73,
        DW_TAG_skeleton_unit = 74,
        DW_TAG_immutable_type = 75,
        DW_TAG_MIPS_loop = 16513,
        DW_TAG_format_label = 16641,
        DW_TAG_function_template = 16642,
        DW_TAG_class_template = 16643,
        DW_TAG_GNU_BINCL = 16644,
        DW_TAG_GNU_EINCL = 16645,
        DW_TAG_GNU_template_template_param = 16646,
        DW_TAG_GNU_template_parameter_pack = 16647,
        DW_TAG_GNU_formal_parameter_pack = 16648,
        DW_TAG_GNU_call_site = 16649,
        DW_TAG_GNU_call_site_parameter = 16650,
        DW_TAG_APPLE_property = 16896,
        DW_TAG_SUN_function_template = 16897,
        DW_TAG_SUN_class_template = 16898,
        DW_TAG_SUN_struct_template = 16899,
        DW_TAG_SUN_union_template = 16900,
        DW_TAG_SUN_indirect_inheritance = 16901,
        DW_TAG_SUN_codeflags = 16902,
        DW_TAG_SUN_memop_info = 16903,
        DW_TAG_SUN_omp_child_func = 16904,
        DW_TAG_SUN_rtti_descriptor = 16905,
        DW_TAG_SUN_dtor_info = 16906,
        DW_TAG_SUN_dtor = 16907,
        DW_TAG_SUN_f90_interface = 16908,
        DW_TAG_SUN_fortran_vax_structure = 16909,
        DW_TAG_SUN_hi = 17151,
        DW_TAG_LLVM_ptrauth_type = 17152,
        DW_TAG_ALTIUM_circ_type = 20737,
        DW_TAG_ALTIUM_mwa_circ_type = 20738,
        DW_TAG_ALTIUM_rev_carry_type = 20739,
        DW_TAG_ALTIUM_rom = 20753,
        DW_TAG_LLVM_annotation = 24576,
        DW_TAG_GHS_namespace = 32772,
        DW_TAG_GHS_using_namespace = 32773,
        DW_TAG_GHS_using_declaration = 32774,
        DW_TAG_GHS_template_templ_param = 32775,
        DW_TAG_UPC_shared_type = 34661,
        DW_TAG_UPC_strict_type = 34662,
        DW_TAG_UPC_relaxed = 34663,
        DW_TAG_PGI_kanji_type = 40960,
        DW_TAG_PGI_interface_block = 40992,
        DW_TAG_BORLAND_property = 45056,
        DW_TAG_BORLAND_Delphi_string = 45057,
        DW_TAG_BORLAND_Delphi_dynamic_array = 45058,
        DW_TAG_BORLAND_Delphi_set = 45059,
        DW_TAG_BORLAND_Delphi_variant = 45060,
    }

    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI" )]
    public enum LibLLVMDwarfAttributeEncoding
        : byte
    {
        DW_ATE_address = 0x01,
        DW_ATE_boolean = 0x02,
        DW_ATE_complex_float = 0x03,
        DW_ATE_float = 0x04,
        DW_ATE_signed = 0x05,
        DW_ATE_signed_char = 0x06,
        DW_ATE_unsigned = 0x07,
        DW_ATE_unsigned_char = 0x08,
        DW_ATE_imaginary_float = 0x09,
        DW_ATE_packed_decimal = 0x0a,
        DW_ATE_numeric_string = 0x0b,
        DW_ATE_edited = 0x0c,
        DW_ATE_signed_fixed = 0x0d,
        DW_ATE_unsigned_fixed = 0x0e,
        DW_ATE_decimal_float = 0x0f,
        DW_ATE_UTF = 0x10,
        DW_ATE_UCS = 0x11,
        DW_ATE_ASCII = 0x12,
        DW_ATE_HP_complex_float = 0x81,
        DW_ATE_HP_float128 = 0x82,
        DW_ATE_HP_complex_float128 = 0x83,
        DW_ATE_HP_floathpintel = 0x84,
        DW_ATE_HP_imaginary_float90 = 0x85,
        DW_ATE_HP_imaginary_float128 = 0x86,
        DW_ATE_lo_user = 0x80,
        DW_ATE_hi_user = 0xff
    }

    public enum LibLLVMMetadataKind
        : Int32
    {
        LibLLVMMetadataKind_MDString = 0,
        LibLLVMMetadataKind_ConstantAsMetadata = 1,
        LibLLVMMetadataKind_LocalAsMetadata = 2,
        LibLLVMMetadataKind_DistinctMDOperandPlaceholder = 3,
        LibLLVMMetadataKind_DIArgList = 4,
        LibLLVMMetadataKind_MDTuple = 5,
        LibLLVMMetadataKind_DILocation = 6,
        LibLLVMMetadataKind_DIExpression = 7,
        LibLLVMMetadataKind_DIGlobalVariableExpression = 8,
        LibLLVMMetadataKind_GenericDINode = 9,
        LibLLVMMetadataKind_DISubrange = 10,
        LibLLVMMetadataKind_DIEnumerator = 11,
        LibLLVMMetadataKind_DIBasicType = 12,
        LibLLVMMetadataKind_DIDerivedType = 13,
        LibLLVMMetadataKind_DICompositeType = 14,
        LibLLVMMetadataKind_DISubroutineType = 15,
        LibLLVMMetadataKind_DIFile = 16,
        LibLLVMMetadataKind_DICompileUnit = 17,
        LibLLVMMetadataKind_DISubprogram = 18,
        LibLLVMMetadataKind_DILexicalBlock = 19,
        LibLLVMMetadataKind_DILexicalBlockFile = 20,
        LibLLVMMetadataKind_DINamespace = 21,
        LibLLVMMetadataKind_DIModule = 22,
        LibLLVMMetadataKind_DITemplateTypeParameter = 23,
        LibLLVMMetadataKind_DITemplateValueParameter = 24,
        LibLLVMMetadataKind_DIGlobalVariable = 25,
        LibLLVMMetadataKind_DILocalVariable = 26,
        LibLLVMMetadataKind_DILabel = 27,
        LibLLVMMetadataKind_DIObjCProperty = 28,
        LibLLVMMetadataKind_DIImportedEntity = 29,
        LibLLVMMetadataKind_DIAssignID = 30,
        LibLLVMMetadataKind_DIMacro = 31,
        LibLLVMMetadataKind_DIMacroFile = 32,
        LibLLVMMetadataKind_DICommonBlock = 33,
        LibLLVMMetadataKind_DIStringType = 34,
        LibLLVMMetadataKind_DIGenericSubrange = 35,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMDwarfAttributeEncoding LibLLVMDIBasicTypeGetEncoding(LLVMMetadataRef basicType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMSubProgramDescribes(LLVMMetadataRef subProgram, LLVMValueRef F);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMContextRefAlias LibLLVMGetNodeContext(LLVMMetadataRef node);

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LibLLVMDIBuilderCreateTempFunctionFwdDecl(
            LLVMDIBuilderRef D,
            LLVMMetadataRef Scope,
            string Name,
            size_t NameLen,
            string LinkageName,
            size_t LinkageNameLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool isLocalToUnit,
            [MarshalAs( UnmanagedType.Bool )] bool isDefinition,
            uint ScopeLine,
            LLVMDIFlags Flags,
            [MarshalAs( UnmanagedType.Bool )] bool isOptimized
            );

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMDIBuilderFinalizeSubProgram(LLVMDIBuilderRef dref, LLVMMetadataRef subProgram);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMDwarfTag LibLLVMDIDescriptorGetTag(LLVMMetadataRef descriptor);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LibLLVMDILocationGetInlinedAtScope(LLVMMetadataRef location);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMMetadataAsString(LLVMMetadataRef descriptor);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LibLLVMMDNodeGetNumOperands(LLVMMetadataRef node);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMMDOperandRef LibLLVMMDNodeGetOperand(LLVMMetadataRef node, UInt32 index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMMDNodeReplaceOperand(LLVMMetadataRef node, UInt32 index, LLVMMetadataRef operand);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LibLLVMGetOperandNode(LibLLVMMDOperandRef operand);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMModuleRefAlias LibLLVMNamedMetadataGetParentModule(LLVMNamedMDNodeRef namedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMNamedMetadataEraseFromParent(LLVMNamedMDNodeRef namedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataKind LibLLVMGetMetadataID(LLVMMetadataRef md);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LibLLVMNamedMDNodeGetNumOperands(LLVMNamedMDNodeRef namedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LibLLVMNamedMDNodeGetOperand(LLVMNamedMDNodeRef namedMDNode, uint index);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMNamedMDNodeSetOperand(LLVMNamedMDNodeRef namedMDNode, uint index, LLVMMetadataRef node);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMNamedMDNodeAddOperand(LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef node);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMNamedMDNodeClearOperands(LLVMNamedMDNodeRef namedMDNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LibLLVMConstantAsMetadata(LLVMValueRef Val);

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LibLLVMGetMDStringText(LLVMMetadataRef mdstring, out uint len);

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMAddNamedMetadataOperand2(LLVMModuleRef M, string name, LLVMMetadataRef Val);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMSetMetadata2(LLVMValueRef Inst, uint KindID, LLVMMetadataRef MD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsTemporary(LLVMMetadataRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsResolved(LLVMMetadataRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsUniqued(LLVMMetadataRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsDistinct(LLVMMetadataRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial Int64 LibLLVMDISubRangeGetLowerBounds(LLVMMetadataRef sr, Int64 defaultLowerBound);
    }
}

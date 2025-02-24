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
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI" )]
    public enum LibLLVMDwarfAttributeEncoding
        : Int32
    {
        DW_ATE_address = 1,
        DW_ATE_boolean = 2,
        DW_ATE_complex_float = 3,
        DW_ATE_float = 4,
        DW_ATE_signed = 5,
        DW_ATE_signed_char = 6,
        DW_ATE_unsigned = 7,
        DW_ATE_unsigned_char = 8,
        DW_ATE_imaginary_float = 9,
        DW_ATE_packed_decimal = 10,
        DW_ATE_numeric_string = 11,
        DW_ATE_edited = 12,
        DW_ATE_signed_fixed = 13,
        DW_ATE_unsigned_fixed = 14,
        DW_ATE_decimal_float = 15,
        DW_ATE_UTF = 16,
        DW_ATE_UCS = 17,
        DW_ATE_ASCII = 18,
        DW_ATE_HP_complex_float = 129,
        DW_ATE_HP_float128 = 130,
        DW_ATE_HP_complex_float128 = 131,
        DW_ATE_HP_floathpintel = 132,
        DW_ATE_HP_imaginary_float90 = 133,
        DW_ATE_HP_imaginary_float128 = 134,
        DW_ATE_lo_user = 128,
        DW_ATE_hi_user = 255,
    }

    public enum LibLLVMDwarfTag
        : Int32
    {
        LibLLVMDwarfTagnull = 0,
        LibLLVMDwarfTagarray_type = 1,
        LibLLVMDwarfTagclass_type = 2,
        LibLLVMDwarfTagentry_point = 3,
        LibLLVMDwarfTagenumeration_type = 4,
        LibLLVMDwarfTagformal_parameter = 5,
        LibLLVMDwarfTagimported_declaration = 8,
        LibLLVMDwarfTaglabel = 10,
        LibLLVMDwarfTaglexical_block = 11,
        LibLLVMDwarfTagmember = 13,
        LibLLVMDwarfTagpointer_type = 15,
        LibLLVMDwarfTagreference_type = 16,
        LibLLVMDwarfTagcompile_unit = 17,
        LibLLVMDwarfTagstring_type = 18,
        LibLLVMDwarfTagstructure_type = 19,
        LibLLVMDwarfTagsubroutine_type = 21,
        LibLLVMDwarfTagtypedef = 22,
        LibLLVMDwarfTagunion_type = 23,
        LibLLVMDwarfTagunspecified_parameters = 24,
        LibLLVMDwarfTagvariant = 25,
        LibLLVMDwarfTagcommon_block = 26,
        LibLLVMDwarfTagcommon_inclusion = 27,
        LibLLVMDwarfTaginheritance = 28,
        LibLLVMDwarfTaginlined_subroutine = 29,
        LibLLVMDwarfTagmodule = 30,
        LibLLVMDwarfTagptr_to_member_type = 31,
        LibLLVMDwarfTagset_type = 32,
        LibLLVMDwarfTagsubrange_type = 33,
        LibLLVMDwarfTagwith_stmt = 34,
        LibLLVMDwarfTagaccess_declaration = 35,
        LibLLVMDwarfTagbase_type = 36,
        LibLLVMDwarfTagcatch_block = 37,
        LibLLVMDwarfTagconst_type = 38,
        LibLLVMDwarfTagconstant = 39,
        LibLLVMDwarfTagenumerator = 40,
        LibLLVMDwarfTagfile_type = 41,
        LibLLVMDwarfTagfriend = 42,
        LibLLVMDwarfTagnamelist = 43,
        LibLLVMDwarfTagnamelist_item = 44,
        LibLLVMDwarfTagpacked_type = 45,
        LibLLVMDwarfTagsubprogram = 46,
        LibLLVMDwarfTagtemplate_type_parameter = 47,
        LibLLVMDwarfTagtemplate_value_parameter = 48,
        LibLLVMDwarfTagthrown_type = 49,
        LibLLVMDwarfTagtry_block = 50,
        LibLLVMDwarfTagvariant_part = 51,
        LibLLVMDwarfTagvariable = 52,
        LibLLVMDwarfTagvolatile_type = 53,
        LibLLVMDwarfTagdwarf_procedure = 54,
        LibLLVMDwarfTagrestrict_type = 55,
        LibLLVMDwarfTaginterface_type = 56,
        LibLLVMDwarfTagnamespace = 57,
        LibLLVMDwarfTagimported_module = 58,
        LibLLVMDwarfTagunspecified_type = 59,
        LibLLVMDwarfTagpartial_unit = 60,
        LibLLVMDwarfTagimported_unit = 61,
        LibLLVMDwarfTagcondition = 63,
        LibLLVMDwarfTagshared_type = 64,
        LibLLVMDwarfTagtype_unit = 65,
        LibLLVMDwarfTagrvalue_reference_type = 66,
        LibLLVMDwarfTagtemplate_alias = 67,
        LibLLVMDwarfTagcoarray_type = 68,
        LibLLVMDwarfTaggeneric_subrange = 69,
        LibLLVMDwarfTagdynamic_type = 70,
        LibLLVMDwarfTagatomic_type = 71,
        LibLLVMDwarfTagcall_site = 72,
        LibLLVMDwarfTagcall_site_parameter = 73,
        LibLLVMDwarfTagskeleton_unit = 74,
        LibLLVMDwarfTagimmutable_type = 75,
        LibLLVMDwarfTagMIPS_loop = 16513,
        LibLLVMDwarfTagformat_label = 16641,
        LibLLVMDwarfTagfunction_template = 16642,
        LibLLVMDwarfTagclass_template = 16643,
        LibLLVMDwarfTagGNU_BINCL = 16644,
        LibLLVMDwarfTagGNU_EINCL = 16645,
        LibLLVMDwarfTagGNU_template_template_param = 16646,
        LibLLVMDwarfTagGNU_template_parameter_pack = 16647,
        LibLLVMDwarfTagGNU_formal_parameter_pack = 16648,
        LibLLVMDwarfTagGNU_call_site = 16649,
        LibLLVMDwarfTagGNU_call_site_parameter = 16650,
        LibLLVMDwarfTagAPPLE_property = 16896,
        LibLLVMDwarfTagSUN_function_template = 16897,
        LibLLVMDwarfTagSUN_class_template = 16898,
        LibLLVMDwarfTagSUN_struct_template = 16899,
        LibLLVMDwarfTagSUN_union_template = 16900,
        LibLLVMDwarfTagSUN_indirect_inheritance = 16901,
        LibLLVMDwarfTagSUN_codeflags = 16902,
        LibLLVMDwarfTagSUN_memop_info = 16903,
        LibLLVMDwarfTagSUN_omp_child_func = 16904,
        LibLLVMDwarfTagSUN_rtti_descriptor = 16905,
        LibLLVMDwarfTagSUN_dtor_info = 16906,
        LibLLVMDwarfTagSUN_dtor = 16907,
        LibLLVMDwarfTagSUN_f90_interface = 16908,
        LibLLVMDwarfTagSUN_fortran_vax_structure = 16909,
        LibLLVMDwarfTagSUN_hi = 17151,
        LibLLVMDwarfTagLLVM_ptrauth_type = 17152,
        LibLLVMDwarfTagALTIUM_circ_type = 20737,
        LibLLVMDwarfTagALTIUM_mwa_circ_type = 20738,
        LibLLVMDwarfTagALTIUM_rev_carry_type = 20739,
        LibLLVMDwarfTagALTIUM_rom = 20753,
        LibLLVMDwarfTagLLVM_annotation = 24576,
        LibLLVMDwarfTagGHS_namespace = 32772,
        LibLLVMDwarfTagGHS_using_namespace = 32773,
        LibLLVMDwarfTagGHS_using_declaration = 32774,
        LibLLVMDwarfTagGHS_template_templ_param = 32775,
        LibLLVMDwarfTagUPC_shared_type = 34661,
        LibLLVMDwarfTagUPC_strict_type = 34662,
        LibLLVMDwarfTagUPC_relaxed = 34663,
        LibLLVMDwarfTagPGI_kanji_type = 40960,
        LibLLVMDwarfTagPGI_interface_block = 40992,
        LibLLVMDwarfTagBORLAND_property = 45056,
        LibLLVMDwarfTagBORLAND_Delphi_string = 45057,
        LibLLVMDwarfTagBORLAND_Delphi_dynamic_array = 45058,
        LibLLVMDwarfTagBORLAND_Delphi_set = 45059,
        LibLLVMDwarfTagBORLAND_Delphi_variant = 45060,
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
            size_t LinakgeNameLen,
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

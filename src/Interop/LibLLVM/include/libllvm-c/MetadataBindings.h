#ifndef LLVM_BINDINGS_LLVM_METADATABINDINGS_H
#define LLVM_BINDINGS_LLVM_METADATABINDINGS_H

#include <stdint.h>
#include "llvm-c/Core.h"
#include "llvm-c/DebugInfo.h"

#ifdef __cplusplus
extern "C" {
#endif

    enum LLVMDwarfTag
    {
        LLVMDwarfTagArrayType = 0x01,
        LLVMDwarfTagClassType = 0x02,
        LLVMDwarfTagEntryPoint = 0x03,
        LLVMDwarfTagEnumerationType = 0x04,
        LLVMDwarfTagFormalParameter = 0x05,
        LLVMDwarfTagImportedDeclaration = 0x08,
        LLVMDwarfTagLabel = 0x0a,
        LLVMDwarfTagLexicalBlock = 0x0b,
        LLVMDwarfTagMember = 0x0d,
        LLVMDwarfTagPointerType = 0x0f,
        LLVMDwarfTagReferenceType = 0x10,
        LLVMDwarfTagCompileUnit = 0x11,
        LLVMDwarfTagStringType = 0x12,
        LLVMDwarfTagStructureType = 0x13,
        LLVMDwarfTagSubroutineType = 0x15,
        LLVMDwarfTagTypeDef = 0x16,
        LLVMDwarfTagUnionType = 0x17,
        LLVMDwarfTagUnspecifiedParameters = 0x18,
        LLVMDwarfTagVariant = 0x19,
        LLVMDwarfTagCommonBlock = 0x1a,
        LLVMDwarfTagCommonInclusion = 0x1b,
        LLVMDwarfTagInheritance = 0x1c,
        LLVMDwarfTagInlinedSubroutine = 0x1d,
        LLVMDwarfTagModule = 0x1e,
        LLVMDwarfTagPtrToMemberType = 0x1f,
        LLVMDwarfTagSetType = 0x20,
        LLVMDwarfTagSubrangeType = 0x21,
        LLVMDwarfTagWithStatement = 0x22,
        LLVMDwarfTagAccessDeclaration = 0x23,
        LLVMDwarfTagBaseType = 0x24,
        LLVMDwarfTagCatchBlock = 0x25,
        LLVMDwarfTagConstType = 0x26,
        LLVMDwarfTagConstant = 0x27,
        LLVMDwarfTagEnumerator = 0x28,
        LLVMDwarfTagFileType = 0x29,
        LLVMDwarfTagFriend = 0x2a,
        LLVMDwarfTagNameList = 0x2b,
        LLVMDwarfTagNameListItem = 0x2c,
        LLVMDwarfTagPackedType = 0x2d,
        LLVMDwarfTagSubProgram = 0x2e,
        LLVMDwarfTagTemplateTypeParameter = 0x2f,
        LLVMDwarfTagTemplateValueParameter = 0x30,
        LLVMDwarfTagThrownType = 0x31,
        LLVMDwarfTagTryBlock = 0x32,
        LLVMDwarfTagVariantPart = 0x33,
        LLVMDwarfTagVariable = 0x34,
        LLVMDwarfTagVolatileType = 0x35,
        LLVMDwarfTagDwarfProcedure = 0x36,
        LLVMDwarfTagRestrictType = 0x37,
        LLVMDwarfTagInterfaceType = 0x38,
        LLVMDwarfTagNamespace = 0x39,
        LLVMDwarfTagImportedModule = 0x3a,
        LLVMDwarfTagUnspecifiedType = 0x3b,
        LLVMDwarfTagPartialUnit = 0x3c,
        LLVMDwarfTagImportedUnit = 0x3d,
        LLVMDwarfTagCondition = 0x3f,
        LLVMDwarfTagSharedType = 0x40,
        LLVMDwarfTagTypeUnit = 0x41,
        LLVMDwarfTagRValueReferenceType = 0x42,
        LLVMDwarfTagTemplateAlias = 0x43,

        // New in DWARF 5:
        LLVMDwarfTagCoArrayType = 0x44,
        LLVMDwarfTagGenericSubrange = 0x45,
        LLVMDwarfTagDynamicType = 0x46,

        LLVMDwarfTagMipsLoop = 0x4081,
        LLVMDwarfTagFormatLabel = 0x4101,
        LLVMDwarfTagFunctionTemplate = 0x4102,
        LLVMDwarfTagClassTemplate = 0x4103,
        LLVMDwarfTagGnuTemplateTemplateParam = 0x4106,
        LLVMDwarfTagGnuTemplateParameterPack = 0x4107,
        LLVMDwarfTagGnuFormalParameterPack = 0x4108,
        LLVMDwarfTagLoUser = 0x4080,
        LLVMDwarfTagAppleProperty = 0x4200,
        LLVMDwarfTagHiUser = 0xffff
    };

    typedef struct LLVMOpaqueMDOperand* LLVMMDOperandRef;

    unsigned int LLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType );
    LLVMBool LLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef /*const Function **/F );
    LLVMContextRef LLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node );

    LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef D
                                                            , LLVMMetadataRef /*DIScope* */Scope
                                                            , char const* Name
                                                            , size_t NameLen
                                                            , char const* LinkageName
                                                            , size_t LinakgeNameLen
                                                            , LLVMMetadataRef /*DIFile* */ File
                                                            , unsigned LineNo
                                                            , LLVMMetadataRef /*DISubroutineType* */ Ty
                                                            , LLVMBool isLocalToUnit
                                                            , LLVMBool isDefinition
                                                            , unsigned ScopeLine
                                                            , LLVMDIFlags Flags /*= 0*/
                                                            , LLVMBool isOptimized /*= false*/
    );

    void LLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram );
    LLVMMetadataRef LLVMDILocation( LLVMContextRef context, unsigned Line, unsigned Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt );
    LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );
    LLVMMetadataRef /*DILocation*/ LLVMDILocationGetInlinedAt( LLVMMetadataRef /*DILocation*/ location );
    LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location );

    // caller must call LLVMDisposeMessage() on the returned string
    char const* LLVMMetadataAsString( LLVMMetadataRef descriptor );

    uint32_t LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );
    LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, uint32_t index );
    void LLVMMDNodeReplaceOperand( LLVMMetadataRef /* MDNode */ node, uint32_t index, LLVMMetadataRef operand );
    LLVMMetadataRef LLVMGetOperandNode( LLVMMDOperandRef operand );

    LLVMModuleRef LLVMNamedMetadataGetParentModule( LLVMNamedMDNodeRef namedMDNode );
    void LLVMNamedMetadataEraseFromParent( LLVMNamedMDNodeRef namedMDNode );
    LLVMMetadataKind LLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md );

    unsigned LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );
    /*MDNode*/ LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index );
    void LLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index, LLVMMetadataRef /*MDNode*/ node );
    void LLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node );
    void LLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode );

#ifdef __cplusplus
} // extern "C"
#endif

#endif

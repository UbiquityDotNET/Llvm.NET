#include <llvm-c/Core.h>
#include <llvm/IR/DIBuilder.h>
#include <llvm/IR/Module.h>
#include <llvm/Support/CBindingWrapping.h>

#include "libllvm-c/MetadataBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( MDOperand, LLVMMDOperandRef )

template <typename DIT> DIT* unwrapDI( LLVMMetadataRef Ref )
{
    return ( DIT* )( Ref ? unwrap<MDNode>( Ref ) : nullptr );
}

LLVMContextRef LLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node )
{
    MDNode* pNode = unwrap<MDNode>( node );
    return wrap( &pNode->getContext( ) );
}

static DINode::DIFlags map_from_llvmDIFlags( LLVMDIFlags Flags )
{
    return static_cast< DINode::DIFlags >( Flags );
}

static LLVMDIFlags map_to_llvmDIFlags( DINode::DIFlags Flags )
{
    return static_cast< LLVMDIFlags >( Flags );
}

static DISubprogram::DISPFlags
pack_into_DISPFlags( bool IsLocalToUnit, bool IsDefinition, bool IsOptimized )
{
    return DISubprogram::toSPFlags( IsLocalToUnit, IsDefinition, IsOptimized );
}

extern "C"
{
    LLVMBool LLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef /*const Function **/F )
    {
        DISubprogram* pSub = unwrap<DISubprogram>( subProgram );
        return pSub->describes( unwrap<Function>( F ) );
    }

    unsigned int LLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType )
    {
        return unwrap<DIBasicType>( basicType )->getEncoding( );
    }

    void LLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram )
    {
        unwrap( dref )->finalizeSubprogram( unwrap<DISubprogram>( subProgram ) );
    }

    LLVMMetadataRef LLVMDILocation( LLVMContextRef context, unsigned Line, unsigned Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt )
    {
        DILocation* pLoc = DILocation::get( *unwrap( context )
                                           , Line
                                           , Column
                                           , unwrap<DILocalScope>( scope )
                                           , InlinedAt ? unwrap<DILocation>( InlinedAt ) : nullptr
        );
        return wrap( pLoc );
    }

    LLVMMetadataRef /*DILocation*/ LLVMDILocationGetInlinedAt( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAt( ) );
    }

    LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAtScope( ) );
    }

    LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor )
    {
        DINode* desc = unwrap<DINode>( descriptor );
        return ( LLVMDwarfTag )desc->getTag( );
    }

    LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef Builder
                                                            , LLVMMetadataRef /*DIScope* */Scope
                                                            , char const* Name
                                                            , size_t NameLen
                                                            , char const* LinkageName
                                                            , size_t LinkageNameLen
                                                            , LLVMMetadataRef /*DIFile* */ File
                                                            , unsigned LineNo
                                                            , LLVMMetadataRef /*DISubroutineType* */ Ty
                                                            , LLVMBool isLocalToUnit
                                                            , LLVMBool isDefinition
                                                            , unsigned ScopeLine
                                                            , LLVMDIFlags Flags /*= 0*/
                                                            , LLVMBool isOptimized /*= false*/
    )
    {
        return wrap( unwrap( Builder )->createTempFunctionFwdDecl(
            unwrapDI<DIScope>( Scope ),
            { Name, NameLen },
            { LinkageName, LinkageNameLen },
            unwrapDI<DIFile>( File ),
            LineNo,
            unwrapDI<DISubroutineType>( Ty ),
            ScopeLine,
            map_from_llvmDIFlags( Flags ),
            pack_into_DISPFlags( isLocalToUnit, isDefinition, isOptimized ),
            nullptr,
            nullptr,
            nullptr ) );
    }

    char const* LLVMMetadataAsString( LLVMMetadataRef descriptor )
    {
        std::string Messages;
        raw_string_ostream Msg( Messages );
        Metadata* d = unwrap<Metadata>( descriptor );
        d->print( Msg );
        return LLVMCreateMessage( Msg.str( ).c_str( ) );
    }

    uint32_t LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return pNode->getNumOperands( );
    }

    LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, uint32_t index )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return wrap( &pNode->getOperand( index ) );
    }

    void LLVMMDNodeReplaceOperand( LLVMMetadataRef /* MDNode */ node, uint32_t index, LLVMMetadataRef operand )
    {
        unwrap<MDNode>( node )->replaceOperandWith( index, unwrap( operand ) );
    }

    LLVMMetadataRef LLVMGetOperandNode( LLVMMDOperandRef operand )
    {
        MDOperand const* pOperand = unwrap( operand );
        return wrap( pOperand->get( ) );
    }

    LLVMModuleRef LLVMNamedMetadataGetParentModule( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return wrap( pMDNode->getParent( ) );
    }

    void LLVMNamedMetadataEraseFromParent( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->eraseFromParent( );
    }

    LLVMMetadataKind LLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md )
    {
        Metadata* pMetadata = unwrap( md );
        return (LLVMMetadataKind )pMetadata->getMetadataID( );
    }

    unsigned LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return pMDNode->getNumOperands( );
    }

    LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index )
    {
        auto pMDNode = unwrap( namedMDNode );
        if ( index >= pMDNode->getNumOperands( ) )
            return nullptr;

        return wrap( pMDNode->getOperand( index ) );
    }

    void LLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        if ( index >= pMDNode->getNumOperands( ) )
            return;

        pMDNode->setOperand( index, unwrap<MDNode>( node ) );
    }

    void LLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        pMDNode->addOperand( unwrap<MDNode>( node ) );
    }

    void LLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->clearOperands( );
    }
}

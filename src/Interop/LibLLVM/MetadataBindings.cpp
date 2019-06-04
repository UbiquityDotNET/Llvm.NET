#include <llvm-c/Core.h>
#include <llvm/IR/DIBuilder.h>
#include <llvm/IR/Module.h>
#include <llvm/Support/CBindingWrapping.h>

#include "libllvm-c/MetadataBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( MDOperand, LibLLVMMDOperandRef )

template <typename DIT> DIT* unwrapDI( LLVMMetadataRef Ref )
{
    return (DIT* )( Ref ? unwrap<MDNode>( Ref ) : nullptr );
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
    LLVMContextRef LibLLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return wrap( &pNode->getContext( ) );
    }

    LLVMBool LibLLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef /*const Function **/F )
    {
        DISubprogram* pSub = unwrap<DISubprogram>( subProgram );
        return pSub->describes( unwrap<Function>( F ) );
    }

    LibLLVMDwarfAttributeEncoding LibLLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType )
    {
        return static_cast< LibLLVMDwarfAttributeEncoding >( unwrap<DIBasicType>( basicType )->getEncoding( ) );
    }

    void LibLLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram )
    {
        unwrap( dref )->finalizeSubprogram( unwrap<DISubprogram>( subProgram ) );
    }

    LLVMMetadataRef LibLLVMDILocation( LLVMContextRef context, unsigned Line, unsigned Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt )
    {
        DILocation* pLoc = DILocation::get( *unwrap( context )
                                            , Line
                                            , Column
                                            , unwrap<DILocalScope>( scope )
                                            , InlinedAt ? unwrap<DILocation>( InlinedAt ) : nullptr
        );
        return wrap( pLoc );
    }

    LLVMMetadataRef /*DILocation*/ LibLLVMDILocationGetInlinedAt( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAt( ) );
    }

    LLVMMetadataRef /*DILocalScope*/ LibLLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAtScope( ) );
    }

    LibLLVMDwarfTag LibLLVMDIDescriptorGetTag( LLVMMetadataRef descriptor )
    {
        DINode* desc = unwrap<DINode>( descriptor );
        return (LibLLVMDwarfTag )desc->getTag( );
    }

    LLVMMetadataRef LibLLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef Builder
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

    char const* LibLLVMMetadataAsString( LLVMMetadataRef descriptor )
    {
        std::string Messages;
        raw_string_ostream Msg( Messages );
        Metadata* d = unwrap<Metadata>( descriptor );
        d->print( Msg );
        return LLVMCreateMessage( Msg.str( ).c_str( ) );
    }

    uint32_t LibLLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return pNode->getNumOperands( );
    }

    LibLLVMMDOperandRef LibLLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, uint32_t index )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return wrap( &pNode->getOperand( index ) );
    }

    void LibLLVMMDNodeReplaceOperand( LLVMMetadataRef /* MDNode */ node, uint32_t index, LLVMMetadataRef operand )
    {
        unwrap<MDNode>( node )->replaceOperandWith( index, unwrap( operand ) );
    }

    LLVMMetadataRef LibLLVMGetOperandNode( LibLLVMMDOperandRef operand )
    {
        MDOperand const* pOperand = unwrap( operand );
        return wrap( pOperand->get( ) );
    }

    LLVMModuleRef LibLLVMNamedMetadataGetParentModule( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return wrap( pMDNode->getParent( ) );
    }

    void LibLLVMNamedMetadataEraseFromParent( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->eraseFromParent( );
    }

    LLVMMetadataKind LibLLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md )
    {
        Metadata* pMetadata = unwrap( md );
        return (LLVMMetadataKind )pMetadata->getMetadataID( );
    }

    unsigned LibLLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return pMDNode->getNumOperands( );
    }

    LLVMMetadataRef LibLLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index )
    {
        auto pMDNode = unwrap( namedMDNode );
        if ( index >= pMDNode->getNumOperands( ) )
            return nullptr;

        return wrap( pMDNode->getOperand( index ) );
    }

    void LibLLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        if ( index >= pMDNode->getNumOperands( ) )
            return;

        pMDNode->setOperand( index, unwrap<MDNode>( node ) );
    }

    void LibLLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        pMDNode->addOperand( unwrap<MDNode>( node ) );
    }

    void LibLLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->clearOperands( );
    }

    LLVMMetadataRef LibLLVMConstantAsMetadata( LLVMValueRef C )
    {
        return wrap( ConstantAsMetadata::get( unwrap<Constant>( C ) ) );
    }

    LLVMMetadataRef LibLLVMMDString2( LLVMContextRef C, char const* Str, unsigned SLen )
    {
        return wrap( MDString::get( *unwrap( C ), StringRef( Str, SLen ) ) );
    }

    LLVMMetadataRef LibLLVMMDNode2( LLVMContextRef C
                                    , LLVMMetadataRef* MDs
                                    , unsigned Count
    )
    {
        auto node = MDNode::get( *unwrap( C )
                                 , ArrayRef<Metadata*>( unwrap( MDs ), Count )
        );
        return wrap( node );
    }

    void LibLLVMAddNamedMetadataOperand2( LLVMModuleRef M
                                          , char const* name
                                          , LLVMMetadataRef Val
    )
    {
        NamedMDNode* N = unwrap( M )->getOrInsertNamedMetadata( name );
        if ( !N )
            return;

        if ( !Val )
            return;

        N->addOperand( unwrap<MDNode>( Val ) );
    }

    void LibLLVMSetMetadata2( LLVMValueRef Inst, unsigned KindID, LLVMMetadataRef MD )
    {
        MDNode* N = MD ? unwrap<MDNode>( MD ) : nullptr;
        unwrap<Instruction>( Inst )->setMetadata( KindID, N );
    }

    LLVMBool LibLLVMIsTemporary( LLVMMetadataRef M )
    {
        auto pMetadata = unwrap<MDNode>( M );
        return pMetadata->isTemporary( );
    }

    LLVMBool LibLLVMIsResolved( LLVMMetadataRef M )
    {
        auto pMetadata = unwrap<MDNode>( M );
        return pMetadata->isResolved( );
    }

    LLVMBool LibLLVMIsUniqued( LLVMMetadataRef M )
    {
        auto pMetadata = unwrap<MDNode>( M );
        return pMetadata->isUniqued( );
    }

    LLVMBool LibLLVMIsDistinct( LLVMMetadataRef M )
    {
        auto pMetadata = unwrap<MDNode>( M );
        return pMetadata->isDistinct( );
    }

    char const* LibLLVMGetMDStringText( LLVMMetadataRef mdstring, unsigned* len )
    {
        MDString const* S = unwrap<MDString>( mdstring );
        *len = S->getString( ).size( );
        return S->getString( ).data( );
    }


    LLVMMetadataRef LibLLVMDIGlobalVarExpGetVariable( LLVMMetadataRef metadataHandle )
    {
        auto pExp = unwrap<DIGlobalVariableExpression>( metadataHandle );
        return wrap( pExp->getVariable( ) );
    }
}

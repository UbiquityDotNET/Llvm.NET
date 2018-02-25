//===- DIBuilderBindings.cpp - Bindings for DIBuilder ---------------------===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines C bindings for the DIBuilder class.
//
//===----------------------------------------------------------------------===//

#include "DIBuilderBindings.h"

#include "IRBindings.h"
#include "llvm/IR/DIBuilder.h"
#include "llvm/IR/IRBuilder.h"
#include "llvm/IR/Module.h"
#include "llvm/IR/Constant.h"
#include "llvm/Support/raw_ostream.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( MDOperand, LLVMMDOperandRef )

extern "C"
{
    LLVMContextRef LLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node )
    {
        MDNode* pNode = unwrap<MDNode>( node );
        return wrap( &pNode->getContext( ) );
    }

    LLVMMetadataRef /*DILocalScope*/ LLVMGetDILocationScope( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getScope( ) );
    }

    unsigned LLVMGetDILocationLine( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return loc->getLine( );
    }

    unsigned LLVMGetDILocationColumn( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return loc->getColumn( );
    }

    LLVMMetadataRef /*DILocation*/ LLVMGetDILocationInlinedAt( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAt( ) );
    }

    LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        return wrap( loc->getInlinedAtScope( ) );
    }

    char const* LLVMGetDIFileName( LLVMMetadataRef /*DIFile*/ file )
    {
        DIFile* pFile = unwrap<DIFile>( file );
        return pFile->getFilename( ).data( );
    }

    char const* LLVMGetDIFileDirectory( LLVMMetadataRef /*DIFile*/ file )
    {
        DIFile* pFile = unwrap<DIFile>( file );
        return pFile->getDirectory( ).data( );
    }

    void LLVMSetDILocation( LLVMValueRef inst, LLVMMetadataRef location )
    {
        DILocation* loc = unwrap<DILocation>( location );
        unwrap<Instruction>( inst )->setDebugLoc( loc );
    }

    void LLVMSetDebugLoc( LLVMValueRef inst, unsigned line, unsigned column, LLVMMetadataRef scope )
    {
        unwrap<Instruction>( inst )->setDebugLoc( DebugLoc::get( line, column, unwrap<MDNode>( scope ) ) );
    }

    LLVMDIBuilderRef LLVMNewDIBuilder( LLVMModuleRef mref, LLVMBool allowUnresolved )
    {
        Module *m = unwrap( mref );
        return wrap( new DIBuilder( *m, allowUnresolved != 0 ) );
    }

    void LLVMDIBuilderDestroy( LLVMDIBuilderRef dref )
    {
        DIBuilder *d = unwrap( dref );
        delete d;
    }

    LLVMMetadataRef LLVMDIBuilderCreateCompileUnit2( LLVMDIBuilderRef Builder
                                                   , LLVMDWARFSourceLanguage Lang
                                                   , LLVMMetadataRef FileRef
                                                   , const char *Producer
                                                   , size_t ProducerLen
                                                   , LLVMBool isOptimized
                                                   , const char *Flags
                                                   , size_t FlagsLen
                                                   , unsigned RuntimeVer
                                                   , const char *SplitName
                                                   , size_t SplitNameLen
                                                   , LLVMDWARFEmissionKind Kind
                                                   , unsigned DWOId
                                                   , LLVMBool SplitDebugInlining
                                                   , LLVMBool DebugInfoForProfiling
                                                   )
    {
        auto File = unwrap<DIFile>( FileRef );

        return wrap( unwrap( Builder )->createCompileUnit( Lang
                                                         , File
                                                         , StringRef( Producer, ProducerLen )
                                                         , isOptimized
                                                         , StringRef( Flags, FlagsLen )
                                                         , RuntimeVer
                                                         , StringRef( SplitName, SplitNameLen )
                                                         , static_cast<DICompileUnit::DebugEmissionKind>( Kind )
                                                         , DWOId
                                                         , SplitDebugInlining
                                                         , DebugInfoForProfiling
                                                         )
                   );
    }

    void LLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram )
    {
        unwrap( dref )->finalizeSubprogram( unwrap<DISubprogram>( subProgram ) );
    }

    LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock( LLVMDIBuilderRef Dref
                                                     , LLVMMetadataRef Scope
                                                     , LLVMMetadataRef File
                                                     , unsigned Line
                                                     , unsigned Column
                                                     )
    {
        DIBuilder *D = unwrap( Dref );
        DILexicalBlock* LB = D->createLexicalBlock( unwrap<DILocalScope>( Scope )
                                                    , File ? unwrap<DIFile>( File ) : nullptr
                                                    , Line
                                                    , Column
                                                    );
        return wrap( LB );
    }

    LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile( LLVMDIBuilderRef Dref
                                                         , LLVMMetadataRef Scope
                                                         , LLVMMetadataRef File
                                                         , unsigned Discriminator
                                                         )
    {
        DIBuilder *D = unwrap( Dref );
        DILexicalBlockFile* LBF = D->createLexicalBlockFile( unwrap<DILocalScope>( Scope )
                                                             , unwrap<DIFile>( File )
                                                             , Discriminator
                                                             );
        return wrap( LBF );
    }

    LLVMMetadataRef LLVMDIBuilderCreateFunction( LLVMDIBuilderRef Dref
                                                 , LLVMMetadataRef Scope
                                                 , const char *Name
                                                 , const char *LinkageName
                                                 , LLVMMetadataRef File
                                                 , unsigned Line
                                                 , LLVMMetadataRef CompositeType
                                                 , int IsLocalToUnit
                                                 , int IsDefinition
                                                 , unsigned ScopeLine
                                                 , unsigned Flags
                                                 , int IsOptimized
                                                 , LLVMMetadataRef /*DITemplateParameterArray*/ TParams /*= nullptr*/
                                                 , LLVMMetadataRef /*DISubProgram */ Decl /*= nullptr*/
                                                 )
    {
        DIBuilder *D = unwrap( Dref );
        DISubprogram* SP = D->createFunction( unwrap<DIScope>( Scope )
                                              , Name
                                              , LinkageName
                                              , File ? unwrap<DIFile>( File ) : nullptr
                                              , Line
                                              , unwrap<DISubroutineType>( CompositeType )
                                              , IsLocalToUnit
                                              , IsDefinition
                                              , ScopeLine
                                              , static_cast<DINode::DIFlags>( Flags )
                                              , IsOptimized
                                              , TParams ? DITemplateParameterArray( unwrap<MDTuple>( TParams ) ) : nullptr
                                              , Decl ? unwrap<DISubprogram>( Decl ) : nullptr
                                              );
        return wrap( SP );
    }

    LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef Dref
                                                            , LLVMMetadataRef /*DIScope* */Scope
                                                            , char const* Name
                                                            , char const* LinkageName
                                                            , LLVMMetadataRef /*DIFile* */ File
                                                            , unsigned LineNo
                                                            , LLVMMetadataRef /*DISubroutineType* */ Ty
                                                            , bool isLocalToUnit
                                                            , bool isDefinition
                                                            , unsigned ScopeLine
                                                            , unsigned Flags /*= 0*/
                                                            , bool isOptimized /*= false*/
                                                            , LLVMMetadataRef /*DITemplateParameterArray*/ TParams /*= nullptr*/
                                                            , LLVMMetadataRef /*DISubProgram */ Decl /*= nullptr*/
                                                            )
    {
        DIBuilder *D = unwrap( Dref );
        DISubprogram* SP = D->createTempFunctionFwdDecl( unwrap<DIScope>( Scope )
                                                         , Name
                                                         , LinkageName
                                                         , File ? unwrap<DIFile>( File ) : nullptr
                                                         , LineNo
                                                         , unwrap<DISubroutineType>( Ty )
                                                         , isLocalToUnit
                                                         , isDefinition
                                                         , ScopeLine
                                                         , static_cast<DINode::DIFlags>( Flags )
                                                         , isOptimized
                                                         , TParams ? DITemplateParameterArray( unwrap<MDTuple>( TParams ) ) : nullptr
                                                         , Decl ? unwrap<DISubprogram>( Decl ) : nullptr
                                                         );
        return wrap( SP );
    }

    LLVMMetadataRef LLVMDIBuilderCreateAutoVariable( LLVMDIBuilderRef Dref
                                                     , LLVMMetadataRef Scope
                                                     , const char *Name
                                                     , LLVMMetadataRef File
                                                     , unsigned Line
                                                     , LLVMMetadataRef Ty
                                                     , int AlwaysPreserve
                                                     , unsigned Flags
                                                     )
    {
        DIBuilder *D = unwrap( Dref );
        DIVariable* V = D->createAutoVariable( unwrap<DIScope>( Scope )
                                               , Name
                                               , File ? unwrap<DIFile>( File ) : nullptr
                                               , Line
                                               , unwrap<DIType>( Ty )
                                               , AlwaysPreserve
                                               , static_cast<DINode::DIFlags>( Flags )
                                               );
        return wrap( V );
    }

    LLVMMetadataRef LLVMDIBuilderCreateParameterVariable( LLVMDIBuilderRef Dref
                                                          , LLVMMetadataRef Scope
                                                          , const char *Name
                                                          , unsigned ArgNo
                                                          , LLVMMetadataRef File
                                                          , unsigned Line
                                                          , LLVMMetadataRef Ty
                                                          , int AlwaysPreserve
                                                          , unsigned Flags
                                                          )
    {
        DIBuilder *D = unwrap( Dref );
        DILocalVariable* V = D->createParameterVariable( unwrap<DIScope>( Scope )
                                                        , Name
                                                        , ArgNo
                                                        , File ? unwrap<DIFile>( File ) : nullptr
                                                        , Line
                                                        , unwrap<DIType>( Ty )
                                                        , AlwaysPreserve
                                                        , static_cast<DINode::DIFlags>( Flags )
                                                        );
        return wrap( V );
    }

    LLVMMetadataRef LLVMDIBuilderCreateBasicType( LLVMDIBuilderRef Dref
                                                  , const char *Name
                                                  , uint64_t SizeInBits
                                                  , unsigned Encoding
                                                  )
    {
        DIBuilder *D = unwrap( Dref );
        DIBasicType* T = D->createBasicType( Name, SizeInBits, Encoding );
        return wrap( T );
    }

    LLVMMetadataRef LLVMDIBuilderCreatePointerType( LLVMDIBuilderRef Dref
                                                    , LLVMMetadataRef PointeeType
                                                    , uint64_t SizeInBits
                                                    , uint32_t AlignInBits
                                                    , const char *Name
                                                    )
    {
        DIBuilder *D = unwrap( Dref );
        DIDerivedType* T = D->createPointerType( PointeeType ? unwrap<DIType>( PointeeType ) : nullptr // nullptr == void
                                                 , SizeInBits
                                                 , AlignInBits
                                                 , None
                                                 , Name
                                                 );
        return wrap( T );
    }

    LLVMMetadataRef LLVMDIBuilderCreateQualifiedType( LLVMDIBuilderRef Dref
                                                      , uint32_t Tag
                                                      , LLVMMetadataRef BaseType
                                                      )
    {
        DIBuilder* D = unwrap( Dref );
        DIDerivedType* T = D->createQualifiedType( Tag, unwrap<DIType>( BaseType ) );
        return wrap( T );
    }


    LLVMMetadataRef LLVMDIBuilderCreateSubroutineType( LLVMDIBuilderRef Dref
                                                       , LLVMMetadataRef ParameterTypes
                                                       , unsigned Flags
                                                       )
    {
        DIBuilder *D = unwrap( Dref );
        DISubroutineType* sub = D->createSubroutineType( DITypeRefArray( unwrap<MDTuple>( ParameterTypes ) )
                                                         , static_cast<DINode::DIFlags>( Flags )
                                                         );
        return wrap( sub );
    }

    LLVMMetadataRef LLVMDIBuilderCreateStructType( LLVMDIBuilderRef Dref
                                                   , LLVMMetadataRef Scope
                                                   , const char *Name
                                                   , LLVMMetadataRef File
                                                   , unsigned Line
                                                   , uint64_t SizeInBits
                                                   , uint32_t AlignInBits
                                                   , unsigned Flags
                                                   , LLVMMetadataRef DerivedFrom
                                                   , LLVMMetadataRef ElementTypes
                                                   )
    {
        DIBuilder *D = unwrap( Dref );
        DICompositeType* CT = D->createStructType( unwrap<DIScope>( Scope )
                                                   , Name
                                                   , File ? unwrap<DIFile>( File ) : nullptr
                                                   , Line
                                                   , SizeInBits
                                                   , AlignInBits
                                                   , static_cast<DINode::DIFlags>( Flags )
                                                   , DerivedFrom ? unwrap<DIType>( DerivedFrom ) : nullptr
                                                   , ElementTypes ? DINodeArray( unwrap<MDTuple>( ElementTypes ) ) : nullptr
                                                   );
        return wrap( CT );
    }

    LLVMMetadataRef LLVMDIBuilderCreateUnionType( LLVMDIBuilderRef Dref
                                                  , LLVMMetadataRef Scope
                                                  , const char *Name
                                                  , LLVMMetadataRef File
                                                  , unsigned Line
                                                  , uint64_t SizeInBits
                                                  , uint32_t AlignInBits
                                                  , unsigned Flags
                                                  , LLVMMetadataRef ElementTypes
                                                  )
    {
        DIBuilder *D = unwrap( Dref );
        DICompositeType* CT = D->createUnionType( unwrap<DIScope>( Scope )
                                                  , Name
                                                  , File ? unwrap<DIFile>( File ) : nullptr
                                                  , Line
                                                  , SizeInBits
                                                  , AlignInBits
                                                  , static_cast<DINode::DIFlags>( Flags )
                                                  , ElementTypes ? DINodeArray( unwrap<MDTuple>( ElementTypes ) ) : nullptr
                                                  );
        return wrap( CT );
    }

    LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType( LLVMDIBuilderRef Dref
                                                                 , unsigned Tag
                                                                 , const char *Name
                                                                 , LLVMMetadataRef Scope
                                                                 , LLVMMetadataRef File
                                                                 , unsigned Line
                                                                 , unsigned RuntimeLang
                                                                 , uint64_t SizeInBits
                                                                 , uint32_t AlignInBits
                                                                 , unsigned Flags
                                                                 )
    {
        DIBuilder *D = unwrap( Dref );
        DICompositeType* type = D->createReplaceableCompositeType( Tag
                                                                   , Name
                                                                   , Scope ? unwrap<DIScope>( Scope ) : nullptr
                                                                   , File ? unwrap<DIFile>( File ) : nullptr
                                                                   , Line
                                                                   , RuntimeLang
                                                                   , SizeInBits
                                                                   , AlignInBits
                                                                   , static_cast<DINode::DIFlags>( Flags )
                                                                   );
        return wrap( type );
    }

    LLVMMetadataRef LLVMDIBuilderCreateMemberType( LLVMDIBuilderRef Dref
                                                   , LLVMMetadataRef Scope
                                                   , const char *Name
                                                   , LLVMMetadataRef File
                                                   , unsigned Line
                                                   , uint64_t SizeInBits
                                                   , uint32_t AlignInBits
                                                   , uint64_t OffsetInBits
                                                   , unsigned Flags
                                                   , LLVMMetadataRef Ty
                                                   )
    {
        DIBuilder *D = unwrap( Dref );
        DIDerivedType* DT = D->createMemberType( unwrap<DIScope>( Scope )
                                                 , Name
                                                 , File ? unwrap<DIFile>( File ) : nullptr
                                                 , Line
                                                 , SizeInBits
                                                 , AlignInBits
                                                 , OffsetInBits
                                                 , static_cast<DINode::DIFlags>( Flags )
                                                 , unwrap<DIType>( Ty )
                                                 );
        return wrap( DT );
    }

    LLVMMetadataRef LLVMDIBuilderCreateArrayType( LLVMDIBuilderRef Dref
                                                  , uint64_t SizeInBits
                                                  , uint32_t AlignInBits
                                                  , LLVMMetadataRef ElementType
                                                  , LLVMMetadataRef Subscripts
                                                  )
    {
        DIBuilder *D = unwrap( Dref );
        DICompositeType* CT = D->createArrayType( SizeInBits
                                                  , AlignInBits
                                                  , unwrap<DIType>( ElementType )
                                                  , DINodeArray( unwrap<MDTuple>( Subscripts ) )
                                                  );
        return wrap( CT );
    }

    LLVMMetadataRef LLVMDIBuilderCreateVectorType( LLVMDIBuilderRef Dref
                                                   , uint64_t SizeInBits
                                                   , uint32_t AlignInBits
                                                   , LLVMMetadataRef ElementType
                                                   , LLVMMetadataRef Subscripts
                                                   )
    {
        DIBuilder *D = unwrap( Dref );
        DICompositeType* CT = D->createVectorType( SizeInBits
                                                   , AlignInBits
                                                   , unwrap<DIType>( ElementType )
                                                   , DINodeArray( unwrap<MDTuple>( Subscripts ) )
                                                   );
        return wrap( CT );
    }

    LLVMMetadataRef LLVMDIBuilderCreateTypedef( LLVMDIBuilderRef Dref
                                                , LLVMMetadataRef Ty
                                                , const char *Name
                                                , LLVMMetadataRef File
                                                , unsigned Line
                                                , LLVMMetadataRef Context
                                                )
    {
        DIBuilder *D = unwrap( Dref );
        DIDerivedType* DT = D->createTypedef( Ty ? unwrap<DIType>( Ty ) : nullptr
                                              , Name
                                              , File ? unwrap<DIFile>( File ) : nullptr
                                              , Line
                                              , Context ? unwrap<DIScope>( Context ) : nullptr
                                              );
        return wrap( DT );
    }

    LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange( LLVMDIBuilderRef Dref
                                                      , int64_t Lo
                                                      , int64_t Count
                                                      )
    {
        DIBuilder *D = unwrap( Dref );
        DISubrange* S = D->getOrCreateSubrange( Lo, Count );
        return wrap( S );
    }

    LLVMMetadataRef LLVMDIBuilderGetOrCreateArray( LLVMDIBuilderRef Dref
                                                   , LLVMMetadataRef *Data
                                                   , size_t Length
                                                   )
    {
        DIBuilder *D = unwrap( Dref );
        Metadata **DataValue = unwrap( Data );
        ArrayRef<Metadata *> Elements( DataValue, Length );
        DINodeArray A = D->getOrCreateArray( Elements );
        return wrap( A.get( ) );
    }

    LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray( LLVMDIBuilderRef Dref
                                                       , LLVMMetadataRef *Data
                                                       , size_t Length
                                                       )
    {
        DIBuilder *D = unwrap( Dref );
        Metadata **DataValue = unwrap( Data );
        ArrayRef<Metadata *> Elements( DataValue, Length );
        DITypeRefArray A = D->getOrCreateTypeArray( Elements );
        return wrap( A.get( ) );
    }

    LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRef Dref
                                                   , int64_t *Addr
                                                   , size_t Length
                                                   )
    {
        DIBuilder *D = unwrap( Dref );
        DIExpression* Expr = D->createExpression( ArrayRef<int64_t>( Addr, Length ) );
        return wrap( Expr );
    }

    LLVMValueRef LLVMDIBuilderInsertDeclareAtEnd( LLVMDIBuilderRef Dref
                                                  , LLVMValueRef Storage
                                                  , LLVMMetadataRef VarInfo
                                                  , LLVMMetadataRef Expr
                                                  , LLVMMetadataRef diLocation
                                                  , LLVMBasicBlockRef Block
                                                  )
    {
        DIBuilder *D = unwrap( Dref );
        Instruction *Instr = D->insertDeclare( unwrap( Storage )
                                               , unwrap<DILocalVariable>( VarInfo )
                                               , unwrap<DIExpression>( Expr )
                                               , unwrap<DILocation>( diLocation )
                                               , unwrap( Block )
                                               );
        return wrap( Instr );
    }

    LLVMValueRef LLVMDIBuilderInsertValueAtEnd( LLVMDIBuilderRef Dref
                                                , LLVMValueRef Val
                                                , LLVMMetadataRef VarInfo
                                                , LLVMMetadataRef Expr
                                                , LLVMMetadataRef diLocation
                                                , LLVMBasicBlockRef Block
                                                )
    {
        DIBuilder *D = unwrap( Dref );
        Instruction *Instr = D->insertDbgValueIntrinsic( unwrap( Val )
                                                         , unwrap<DILocalVariable>( VarInfo )
                                                         , unwrap<DIExpression>( Expr )
                                                         , unwrap<DILocation>( diLocation )
                                                         , unwrap( Block )
                                                         );
        return wrap( Instr );
    }

    LLVMMetadataRef LLVMDIBuilderCreateEnumerationType( LLVMDIBuilderRef Dref
                                                        , LLVMMetadataRef Scope          // DIScope
                                                        , char const* Name
                                                        , LLVMMetadataRef File           // DIFile
                                                        , unsigned LineNumber
                                                        , uint64_t SizeInBits
                                                        , uint32_t AlignInBits
                                                        , LLVMMetadataRef Elements       // DINodeArray
                                                        , LLVMMetadataRef UnderlyingType // DIType
                                                        , char const* UniqueId
                                                        )
    {
        DIBuilder* D = unwrap( Dref );
        DICompositeType* type = D->createEnumerationType( unwrap<DIScope>( Scope )
                                                          , Name
                                                          , File ? unwrap<DIFile>( File ) : nullptr
                                                          , LineNumber
                                                          , SizeInBits
                                                          , AlignInBits
                                                          , DINodeArray( unwrap<MDTuple>( Elements ) )
                                                          , unwrap<DIType>( UnderlyingType )
                                                          , UniqueId
                                                          );
        return wrap( type );
    }

    /// createEnumerator - Create a single enumerator value.
    //DIEnumerator createEnumerator( StringRef Name, int64_t Val );
    LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef Dref, char const* Name, int64_t Val )
    {
        DIBuilder* D = unwrap( Dref );
        DIEnumerator* enumerator = D->createEnumerator( Name, Val );
        return wrap( enumerator );
    }

    LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor )
    {
        DINode* desc = unwrap<DINode>( descriptor );
        return ( LLVMDwarfTag )desc->getTag( );
    }

    LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression( LLVMDIBuilderRef Dref
                                                               , LLVMMetadataRef Context //DIScope
                                                               , char const* Name
                                                               , char const* LinkageName
                                                               , LLVMMetadataRef File  // DIFile
                                                               , unsigned LineNo
                                                               , LLVMMetadataRef Ty    //DIType
                                                               , LLVMBool isLocalToUnit
                                                               , LLVMMetadataRef expression // DIExpression
                                                               , LLVMMetadataRef Decl // MDNode = nullptr
                                                               , uint32_t AlignInBits
                                                               )
    {
        DIBuilder* D = unwrap( Dref );
        auto globalVar = D->createGlobalVariableExpression( unwrap<DIScope>( Context )
                                                               , Name
                                                               , LinkageName
                                                               , File ? unwrap<DIFile>( File ) : nullptr
                                                               , LineNo
                                                               , unwrap<DIType>( Ty )
                                                               , isLocalToUnit
                                                               , expression ? unwrap<DIExpression>( expression ) : nullptr
                                                               , Decl ? unwrap<MDNode>( Decl ) : nullptr
                                                               , AlignInBits
                                                               );
        return wrap( globalVar );
    }

    LLVMValueRef LLVMDIBuilderInsertDeclareBefore( LLVMDIBuilderRef Dref
                                                   , LLVMValueRef Storage      // Value
                                                   , LLVMMetadataRef VarInfo   // DIVariable
                                                   , LLVMMetadataRef Expr      // DIExpression
                                                   , LLVMMetadataRef diLocation // DILocation
                                                   , LLVMValueRef InsertBefore // Instruction
                                                   )
    {
        DIBuilder* D = unwrap( Dref );
        Instruction* pInstruction = D->insertDeclare( unwrap( Storage )
                                                      , unwrap<DILocalVariable>( VarInfo )
                                                      , unwrap<DIExpression>( Expr )
                                                      , unwrap<DILocation>( diLocation )
                                                      , unwrap<Instruction>( InsertBefore )
                                                      );
        return wrap( pInstruction );
    }

    LLVMValueRef LLVMDIBuilderInsertValueBefore( LLVMDIBuilderRef Dref
                                                 , /*llvm::Value **/LLVMValueRef Val
                                                 , /*DILocalVariable **/ LLVMMetadataRef VarInfo
                                                 , /*DIExpression **/ LLVMMetadataRef Expr
                                                 , /*const DILocation **/ LLVMMetadataRef DL
                                                 , /*Instruction **/ LLVMValueRef InsertBefore
                                                 )
    {
        DIBuilder* D = unwrap( Dref );
        Instruction* pInstruction = D->insertDbgValueIntrinsic( unwrap( Val )
                                                                , unwrap<DILocalVariable>( VarInfo )
                                                                , unwrap<DIExpression>( Expr )
                                                                , unwrap<DILocation>( DL )
                                                                , unwrap<Instruction>( InsertBefore )
                                                                );
        return wrap( pInstruction );
    }

    char const* LLVMMetadataAsString( LLVMMetadataRef descriptor )
    {
        std::string Messages;
        raw_string_ostream Msg( Messages );
        Metadata* d = unwrap<Metadata>( descriptor );
        d->print( Msg );
        return LLVMCreateMessage( Msg.str( ).c_str( ) );
    }

    void LLVMMDNodeReplaceAllUsesWith( LLVMMetadataRef oldDescriptor, LLVMMetadataRef newDescriptor )
    {
        MDNode* o = unwrap<MDNode>( oldDescriptor );
        Metadata* n = unwrap<Metadata>( newDescriptor );
        o->replaceAllUsesWith( n );
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

    LLVMBool LLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef /*const Function **/F )
    {
        DISubprogram* pSub = unwrap<DISubprogram>( subProgram );
        return pSub->describes( unwrap<Function>( F ) );
    }

    LLVMMetadataRef LLVMDIBuilderCreateNamespace( LLVMDIBuilderRef Dref, LLVMMetadataRef scope, char const* name, LLVMBool exportSymbols)
    {
        DIBuilder* D = unwrap( Dref );
        DINamespace* pNamespace = D->createNameSpace( scope ? unwrap<DIScope>( scope ) : nullptr
                                                      , name
                                                      , exportSymbols
                                                      );
        return wrap( pNamespace );
    }

    LLVMMetadataKind LLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md )
    {
        Metadata* pMetadata = unwrap( md );
        return ( LLVMMetadataKind )pMetadata->getMetadataID( );
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

    /*DISubProgram*/ LLVMMetadataRef LLVMDILocalScopeGetSubProgram( LLVMMetadataRef /*DILocalScope*/ localScope )
    {
        DILocalScope* pScope = unwrap<DILocalScope>( localScope );
        return wrap( pScope->getSubprogram( ) );
    }

    unsigned int LLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType )
    {
        return unwrap<DIBasicType>( basicType )->getEncoding( );
    }
}

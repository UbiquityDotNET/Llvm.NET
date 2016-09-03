//===- AttributeBindings.h - Additional bindings for IR ----------------*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines additional C bindings for LLVM Attributes IR Attributes.
//
//===----------------------------------------------------------------------===//

#ifndef LLVM_BINDINGS_LLVM_ATTRIBUTEBINDINGS_H
#define LLVM_BINDINGS_LLVM_ATTRIBUTEBINDINGS_H

#include "llvm-c/Core.h"
#include "llvm-c/Types.h"

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

    enum LLVMAttrKind
    {
        // IR-Level Attributes
        LLVMAttrKindNone,
        LLVMAttrKindAlignment,
        LLVMAttrKindAllocSize,
        LLVMAttrKindAlwaysInline,
        LLVMAttrKindArgMemOnly,
        LLVMAttrKindBuiltin,
        LLVMAttrKindByVal,
        LLVMAttrKindCold,
        LLVMAttrKindConvergent,
        LLVMAttrKindDereferenceable,
        LLVMAttrKindDereferenceableOrNull,
        LLVMAttrKindInAlloca,
        LLVMAttrKindInReg,
        LLVMAttrKindInaccessibleMemOnly,
        LLVMAttrKindInaccessibleMemOrArgMemOnly,
        LLVMAttrKindInlineHint,
        LLVMAttrKindJumpTable,
        LLVMAttrKindMinSize,
        LLVMAttrKindNaked,
        LLVMAttrKindNest,
        LLVMAttrKindNoAlias,
        LLVMAttrKindNoBuiltin,
        LLVMAttrKindNoCapture,
        LLVMAttrKindNoDuplicate,
        LLVMAttrKindNoImplicitFloat,
        LLVMAttrKindNoInline,
        LLVMAttrKindNoRecurse,
        LLVMAttrKindNoRedZone,
        LLVMAttrKindNoReturn,
        LLVMAttrKindNoUnwind,
        LLVMAttrKindNonLazyBind,
        LLVMAttrKindNonNull,
        LLVMAttrKindOptimizeForSize,
        LLVMAttrKindOptimizeNone,
        LLVMAttrKindReadNone,
        LLVMAttrKindReadOnly,
        LLVMAttrKindReturned,
        LLVMAttrKindReturnsTwice,
        LLVMAttrKindSExt,
        LLVMAttrKindSafeStack,
        LLVMAttrKindSanitizeAddress,
        LLVMAttrKindSanitizeMemory,
        LLVMAttrKindSanitizeThread,
        LLVMAttrKindStackAlignment,
        LLVMAttrKindStackProtect,
        LLVMAttrKindStackProtectReq,
        LLVMAttrKindStackProtectStrong,
        LLVMAttrKindStructRet,
        LLVMAttrKindSwiftError,
        LLVMAttrKindSwiftSelf,
        LLVMAttrKindUWTable,
        LLVMAttrKindZExt,
        EndAttrKinds           /// Sentinal value useful for loops
    };

    // These functions replace the LLVM*FunctionAttr functions in the stable C API
    // since the underlying attribute support has changed dramatically in 3.7.0

    /**
    * @defgroup LibLLVM Attributes
    *
    * Attributes are available for functions, function return types and parameters.
    * In previous releases of LLVM attributes were essentially a bit field. However,
    * as more attributes were added the number of bits had to grow. Furthermore, some
    * of the attributes such as stack alignment require a parameter value. Thus, the
    * attribute system evolved. Unfortunately the C API did not. Moving towards version
    * 4.0 of LLVM the entire bitfield approach is being deprecated. Thus these APIs
    * make the functionality of Attribute, AttributeSet and, AttrBuilder available
    * to language bindings based on a C-API
    *
    * The Attribute and AttributeSet classes are somewhat problematic to expose in
    * "C" as they are not officially POD types and therefore have limited defined
    * portable usage in "C". However, they are officially standard layout and trivially
    * copy constructible. The reason they are not POD is that they have a custom default
    * constructor. Thus construction cannot be assumed as trivially zero initializing
    * the memory for the type. To manage that with projections to "C" and other languages
    * this API exposes the underlying class constructors as "C" functions returning an
    * instance of the constructed type. Furthermore this API assumes that llvm::Attribute
    * and llvm::AttributeSet are defined using the Pointer to Implementation (PIMPL)
    * pattern and are entirely representable in a LLVMAttributeSet. (This is statically checked
    * in the AttributeBindings.cpp file along with the trivial copy construction and
    * standard layout type traits. Any changes in LLVM that would invalidate these assumptions
    * will trigger a compilation error, rather than failing in inexplicable ways at runtime.)
    * @{
    */
    typedef uintptr_t LLVMAttributeSet;
    typedef struct LLVMOpaqueAttributeBuilder* LLVMAttributeBuilderRef;

    /** @name AttributeSet 
    * @{
    */
    LLVMAttributeSet LLVMCreateEmptyAttributeSet( );
    LLVMAttributeSet LLVMCreateAttributeSetFromKindArray( LLVMContextRef context, unsigned index, LLVMAttrKind* pKinds, uint64_t len );
    LLVMAttributeSet LLVMCreateAttributeSetFromAttributeSetArray( LLVMContextRef context, LLVMAttributeSet* pAttributes, uint64_t len );
    LLVMAttributeSet LLVMCreateAttributeSetFromBuilder( LLVMContextRef context, unsigned index, LLVMAttributeBuilderRef bldr );

    LLVMAttributeSet LLVMAttributeSetAddKind( LLVMAttributeSet attributeSet, LLVMContextRef context, unsigned index, LLVMAttrKind kind );
    LLVMAttributeSet LLVMAttributeSetAddString( LLVMAttributeSet attributeSet, LLVMContextRef context, unsigned index, char const*name );
    LLVMAttributeSet LLVMAttributeSetAddStringValue( LLVMAttributeSet attributeSet, LLVMContextRef context, unsigned index, char const* name, char const* value );
    LLVMAttributeSet LLVMAttributeSetAddAttributes( LLVMAttributeSet attributeSet, LLVMContextRef context, unsigned index, LLVMAttributeSet otherAttributeSet );

    LLVMAttributeSet LLVMAttributeSetRemoveAttributeKind( LLVMAttributeSet attributeSet, unsigned index, LLVMAttrKind kind );
    LLVMAttributeSet LLVMAttributeSetRemoveAttributeSet( LLVMAttributeSet attributeSet, unsigned index, LLVMAttributeSet attributes );
    LLVMAttributeSet LLVMAttributeSetRemoveAttributeBuilder( LLVMAttributeSet attributeSet, LLVMContextRef context, unsigned index, LLVMAttributeBuilderRef bldr );

    LLVMContextRef LLVMAttributeSetGetContext( LLVMAttributeSet attributeSet );
    LLVMAttributeSet LLVMAttributeGetAttributes( LLVMAttributeSet attributeSet, unsigned index );
    LLVMBool LLVMAttributeSetHasAttributeKind( LLVMAttributeSet attributeSet, unsigned index, LLVMAttrKind kind );
    LLVMBool LLVMAttributeSetHasStringAttribute( LLVMAttributeSet attributeSet, unsigned index, char const* name );
    LLVMBool LLVMAttributeSetHasAttributes( LLVMAttributeSet attributeSet, unsigned index );
    LLVMBool LLVMAttributeSetHasAttributeSomewhere( LLVMAttributeSet attributeSet, LLVMAttrKind kind );

    LLVMAttributeRef LLVMAttributeSetGetAttributeByKind( LLVMAttributeSet attributeSet, unsigned index, LLVMAttrKind kind );
    LLVMAttributeRef LLVMAttributeSetGetAttributeByName( LLVMAttributeSet attributeSet, unsigned index, char const* name );
    char const* LLVMAttributeSetToString( LLVMAttributeSet attributeSet, unsigned index, LLVMBool inGroup );

    unsigned LLVMAttributeSetGetNumSlots( LLVMAttributeSet attributeSet );
    LLVMAttributeSet LLVMAttributeSetGetSlotAttributes( LLVMAttributeSet attributeSet, unsigned slot );
    unsigned LLVMAttributeSetGetSlotIndex( LLVMAttributeSet attributeSet, unsigned slot );

    LLVMAttributeSet LLVMGetFunctionAttributeSet( LLVMValueRef /*Function*/ function );
    void LLVMSetFunctionAttributeSet( LLVMValueRef /*Function*/ function, LLVMAttributeSet attributeSet );
    LLVMAttributeSet LLVMGetCallSiteAttributeSet( LLVMValueRef /*Instruction*/ instruction );
    void LLVMSetCallSiteAttributeSet( LLVMValueRef /*Instruction*/ instruction, LLVMAttributeSet attributeSet );

    uintptr_t LLVMAttributeSetGetIteratorStartToken( LLVMAttributeSet attributeSet, unsigned slot );
    LLVMAttributeRef LLVMAttributeSetIteratorGetNext( LLVMAttributeSet attributeSet, unsigned slot, uintptr_t* pToken );

    /**
    * @}
    */

    /** @name Attribute
    * @{
    */
    LLVMBool LLVMIsEnumAttribute( LLVMAttributeRef attribute );
    LLVMBool LLVMIsIntAttribute( LLVMAttributeRef attribute );
    LLVMBool LLVMIsStringAttribute( LLVMAttributeRef attribute );
    LLVMBool LLVMHasAttributeKind( LLVMAttributeRef attribute, LLVMAttrKind kind );
    LLVMBool LLVMHasAttributeString( LLVMAttributeRef attribute, char const* name );
    LLVMAttrKind LLVMGetAttributeKind( LLVMAttributeRef attribute );
    uint64_t LLVMGetAttributeValue( LLVMAttributeRef attribute );
    char const* LLVMGetAttributeName( LLVMAttributeRef attribute );
    char const* LLVMGetAttributeStringValue( LLVMAttributeRef attribute );
    char const* LLVMAttributeToString( LLVMAttributeRef attribute );

    LLVMAttributeRef LLVMCreateAttribute( LLVMContextRef ctx, LLVMAttrKind kind, uint64_t value );
    LLVMAttributeRef LVMCreateTargetDependentAttribute( LLVMContextRef ctx, char const* name, char const* value );
    /**
    * @}
    */

    /** @name AttrBuilder
    * @{
    */
    LLVMAttributeBuilderRef LLVMCreateAttributeBuilder( );
    LLVMAttributeBuilderRef LLVMCreateAttributeBuilder2( LLVMAttributeRef value );
    LLVMAttributeBuilderRef LLVMCreateAttributeBuilder3( LLVMAttributeSet attributeSet, unsigned index );
    void LLVMAttributeBuilderDispose( LLVMAttributeBuilderRef bldr );

    void LLVMAttributeBuilderClear( LLVMAttributeBuilderRef bldr );

    void LLVMAttributeBuilderAddEnum( LLVMAttributeBuilderRef bldr, LLVMAttrKind kind );
    void LLVMAttributeBuilderAddAttribute( LLVMAttributeBuilderRef bldr, LLVMAttributeRef value );
    void LLVMAttributeBuilderAddStringAttribute( LLVMAttributeBuilderRef bldr, char const* name, char const* value );
    void LLVMAttributeBuilderRemoveEnum( LLVMAttributeBuilderRef bldr, LLVMAttrKind kind );
    void LLVMAttributeBuilderRemoveAttributes( LLVMAttributeBuilderRef bldr, LLVMAttributeSet attributeSet, unsigned index );
    void LLVMAttributeBuilderRemoveAttribute( LLVMAttributeBuilderRef bldr, char const* name );
    void LLVMAttributeBuilderRemoveBldr( LLVMAttributeBuilderRef bldr, LLVMAttributeBuilderRef ohter );

    void LLVMAttributeBuilderMerge( LLVMAttributeBuilderRef bldr, LLVMAttributeBuilderRef ohter );
    LLVMBool LLVMAttributeBuilderOverlaps( LLVMAttributeBuilderRef bldr, LLVMAttributeBuilderRef other );
    LLVMBool LLVMAttributeBuilderContainsEnum( LLVMAttributeBuilderRef bldr, LLVMAttrKind kind );
    LLVMBool LLVMAttributeBuilderContainsName( LLVMAttributeBuilderRef bldr, char const* name );
    LLVMBool LLVMAttributeBuilderHasAnyAttributes( LLVMAttributeBuilderRef bldr );
    LLVMBool LLVMAttributeBuilderHasAttributes( LLVMAttributeBuilderRef bldr, LLVMAttributeSet attributeset, unsigned index );
    LLVMBool LLVMAttributeBuilderHasTargetIndependentAttrs( LLVMAttributeBuilderRef bldr );
    LLVMBool LLVMAttributeBuilderHasTargetDependentAttrs( LLVMAttributeBuilderRef bldr );

    // TODO: Define support for iterators over target dependent attributes
    /**
    * @}
    */

    /**
    * @}
    */
#ifdef __cplusplus
}
#endif

#endif

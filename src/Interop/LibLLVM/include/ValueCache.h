#ifndef _VALUECACHE_H_
#define _VALUECACHE_H_

#include "llvm/IR/ValueMap.h"
#include "libllvm-c/ValueBindings.h"

namespace llvm
{
    class ValueCache;

    // configuration of the ValueMap template for handling RAUW and deletion
    // while generating notifications to bindings
    struct ValueCacheConfig
        : ValueMapConfig<Value*>
    {
        enum
        {
            FollowRAUW = false
        };

        struct ExtraData
        {
            explicit ExtraData( ValueCache* owningCache
                              , LibLLVMValueCacheItemDeletedCallback deletedCallback = nullptr
                              , LibLLVMValueCacheItemReplacedCallback replacedCallback = nullptr
                              )
                : Cache( owningCache )
                , ItemDeletedCallback( deletedCallback )
                , ItemReplacedCallback( replacedCallback )
            {
            }

            ExtraData( ExtraData const& other ) = default;

            ValueCache* Cache;
            /*MaybeNull*/ LibLLVMValueCacheItemDeletedCallback ItemDeletedCallback;
            /*MaybeNull*/ LibLLVMValueCacheItemReplacedCallback ItemReplacedCallback;

        };

        static void onRAUW( const ExtraData& data, Value* pOldValue, Value* /*New*/ );
        static void onDelete( const ExtraData& data, Value* pOldValue /*Old*/ );
    };

    class ValueCache
        : public ValueMap<Value*, intptr_t, ValueCacheConfig>
    {
        typedef ValueMap<Value*, intptr_t, ValueCacheConfig> base_t;

    public:
        ValueCache( LibLLVMValueCacheItemDeletedCallback deletedCallback, LibLLVMValueCacheItemReplacedCallback replacedCallback )
            : base_t( ValueCacheConfig::ExtraData( this, deletedCallback, replacedCallback ) )
        {
        }
    };

    void ValueCacheConfig::onDelete( const ExtraData& data, Value* pOldValue )
    {
        // ValueCache (actually ValueMap base) will remove the item from the map
        // Notify binding if a callback is provided.
        if ( data.ItemDeletedCallback != nullptr )
        {
            data.ItemDeletedCallback( wrap( pOldValue ), data.Cache->lookup( pOldValue ) );
        }
    }

    void ValueCacheConfig::onRAUW( const ExtraData& data, Value* pOldValue, Value* pNewValue )
    {
        intptr_t& currentHandle = data.Cache->operator[]( pOldValue );

        // ValueCache (actually ValueMap base) does NOT remove the item from the cache on RAUW,
        // so erase it here. Projection must provide a new mapping as ValueMap doesn't have an
        // option for re-setting the handle the value is mapped to.
        data.Cache->erase( pOldValue );

        // Notify binding if a callback is provided.
        if ( data.ItemReplacedCallback != nullptr )
        {
            currentHandle = data.ItemReplacedCallback( wrap( pOldValue ), currentHandle, wrap( pNewValue ) );
        }
    }
}
#endif


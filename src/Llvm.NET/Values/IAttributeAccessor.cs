using System.Collections.Generic;

namespace Llvm.NET.Values
{
    // As of v3.9x and later Functions and call sites use distinct LLVM-C API sets for
    // manipulating attributes. Fortunately they have consistent signatures so these
    // are used to abstract the difference via derived types specialized for each case.
    // going forward this is the simplest and most direct way to manipulate attributes
    // on a value as everything ultimately comes down to this interface.
    internal interface IAttributeAccessor
        : IAttributeContainer
    {
        uint GetAttributeCountAtIndex( FunctionAttributeIndex index );

        IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index );

        AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind );

        AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, string name );

        void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib );

        void RemoveAttributeAtIndex( FunctionAttributeIndex index, AttributeKind kind );

        void RemoveAttributeAtIndex( FunctionAttributeIndex index, string name );
    }
}

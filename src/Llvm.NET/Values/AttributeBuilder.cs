using System;

namespace Llvm.NET.Values
{
    public sealed class AttributeBuilder
        : IDisposable
    {
        public AttributeBuilder( AttributeSet attributes )
        {
        }

        public void Dispose( )
        {
            // de-allocate the previously allocated builder
        }

        public AttributeBuilder Add( FunctionAttributeIndex index, AttributeKind[ ] value )
        {
            return this;
        }
        
        public AttributeBuilder Add( FunctionAttributeIndex index, AttributeValue value )
        {
            return this;
        }

        public AttributeBuilder Add( string name )
        {
            return Add( name, string.Empty );
        }

        public AttributeBuilder Add( string name, string value )
        {
            return this;
        }

        public AttributeSet ToAttributeSet()
        {
            return new AttributeSet( );
        }

        internal LLVMAttributeBuilderRef BuilderHandle;
    }
}

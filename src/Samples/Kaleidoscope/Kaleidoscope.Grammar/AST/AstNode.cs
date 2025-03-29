using System.Collections.Generic;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public abstract class AstNode
        : IAstNode
    {
        protected AstNode(SourceLocation location)
        {
            Location = location;
        }

        public SourceLocation Location { get; }

        public abstract IEnumerable<IAstNode> Children { get; }

        // NOTE: Accept dispatching is NOT implemented here to allow type specific handling
        //       Dispatch to the correct IKaleidoscopeAstVisitor.Visit(...) method requires
        //       type specific knowledge of the thing being visited. So this is an abstract
        //       method that implementation will need to provide, even though the implementation
        //       looks the same, it isn't as it includes direct calls to the correct overload
        //       of the Visit() method.
        public abstract TResult? Accept<TResult>( IAstVisitor<TResult> visitor );

        public abstract TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct;
    }
}

using System.Collections.Generic;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Common abstract base implementation of <see cref="IAstNode"/></summary>
    public abstract class AstNode
        : IAstNode
    {
        /// <summary>Initializes a new instance of an <see cref="AstNode"/> class</summary>
        /// <param name="location">Location in the source this node represents</param>
        protected AstNode(SourceLocation location)
        {
            Location = location;
        }

        /// <inheritdoc/>
        public SourceLocation Location { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<IAstNode> Children { get; }

        // NOTE: Accept() dispatching is NOT implemented here to allow type specific handling
        //       dispatch to the correct Visit(...) method requires type specific knowledge of
        //       the thing being visited. So this is an abstract method that implementation
        //       will need to provide, even though the implementation looks the same, it isn't
        //       as it includes direct calls to the correct overload of the Visit() method.
        /// <inheritdoc/>
        public abstract TResult? Accept<TResult>( IAstVisitor<TResult> visitor );

        /// <inheritdoc/>
        public abstract TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct;
    }
}

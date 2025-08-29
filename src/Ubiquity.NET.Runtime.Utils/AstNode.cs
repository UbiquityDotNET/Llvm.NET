// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Common abstract base implementation of <see cref="IAstNode"/></summary>
    public abstract class AstNode
        : IAstNode
    {
        /// <inheritdoc/>
        public SourceRange Location { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<IAstNode> Children { get; }

        // NOTE: Accept() dispatching is NOT implemented here to allow type specific handling
        //       dispatch to the correct Visit(...). Implementation of that method requires
        //       type specific knowledge of the thing being visited. So this is an abstract
        //       method that an implementation will need to provide, even though the implementation
        //       looks the same, it isn't as it includes direct calls to the correct overload
        //       of the Visit() method. It is plausible that a source generator could create
        //       the implementation of such mundane and error prone code duplication though...

        /// <inheritdoc/>
        public abstract TResult? Accept<TResult>( IAstVisitor<TResult> visitor );

        /// <inheritdoc/>
        public abstract TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct;

        /// <summary>Initializes a new instance of the <see cref="AstNode"/> class</summary>
        /// <param name="location">Location in the source this node represents</param>
        protected AstNode( SourceRange location )
        {
            Location = location;
        }
    }
}

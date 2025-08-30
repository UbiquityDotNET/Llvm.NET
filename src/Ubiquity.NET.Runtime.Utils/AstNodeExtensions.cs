// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Extensions for IAstNode</summary>
    /// <remarks>
    /// While default interface methods seems like a great idea, it's not yet complete enough to be useful.
    /// In particular there's pretty much no debugger support for evaluating such things, leaving you
    /// with no way to see what they produce when used as a property. Hopefully, that will be resolved in
    /// the future - but for now it is more a hindrance than it is a help.
    /// </remarks>
    public static class AstNodeExtensions
    {
        /// <summary>Gets the complete collection of errors for this node and children</summary>
        /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
        /// <param name="node">Node to traverse for errors</param>
        /// <remarks>Traverses the node hierarchy to find all error nodes at any depth</remarks>
        /// <returns>Collection of errors found</returns>
        public static ImmutableArray<ErrorNode<T>> CollectErrors<T>( this IAstNode node )
            where T : struct, Enum
        {
            ArgumentNullException.ThrowIfNull( node );

            var collector = new ErrorNodeCollector<T>();
            return node.Accept( collector );
        }
    }
}

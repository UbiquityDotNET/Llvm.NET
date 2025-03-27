// -----------------------------------------------------------------------
// <copyright file="IAstNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
        /// <param name="node">Node to traverse for errors</param>
        /// <remarks>Traverses the node hierarchy to find all error node at any depth</remarks>
        /// <returns>Collection of errors found</returns>
        public static IReadOnlyCollection<ErrorNode> CollectErrors( this IAstNode node )
        {
            ArgumentNullException.ThrowIfNull(node);

            var collector = new ErrorNodeCollector();
            node.Accept( collector );
            return collector.Errors;
        }
    }
}

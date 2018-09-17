// <copyright file="AssignmentExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Representation of a variable assignment</summary>
    /// <remarks>
    /// This is a distinct node type to distinguish between a variable reference, which has "load" semantics
    /// and an assignment, which has "store" semantics.
    /// </remarks>
    public class AssignmentExpression
        : IExpression
    {
        public AssignmentExpression( SourceSpan location, VariableReferenceExpression target, IExpression value )
        {
            Location = location;
            Target = target;
            Value = value;
        }

        public SourceSpan Location { get; }

        public VariableReferenceExpression Target { get; }

        public IExpression Value { get; }

        public TResult Accept<TResult>( IAstVisitor<TResult> visitor ) => visitor.Visit( this );

        public IEnumerable<IAstNode> Children
        {
            get
            {
                yield return Target;
                yield return Value;
            }
        }
    }
}

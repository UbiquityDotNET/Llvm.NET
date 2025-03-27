using System;

namespace Ubiquity.NET.Runtime.Utils
{
    public class AstVisitorBase<TResult>
        : IAstVisitor<TResult>
    {
        public virtual TResult? Visit( IAstNode node )
        {
            return VisitChildren(node);
        }

        public virtual TResult? VisitChildren( IAstNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach( var child in node.Children )
            {
                aggregate = AggregateResult( aggregate, child.Accept( this ) );
            }

            return aggregate;
        }

        protected AstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        protected TResult? DefaultResult { get; }
    }

    public class AstVisitorBase<TResult, TArg>
        : IAstVisitor<TResult, TArg>
        where TArg : struct, allows ref struct
    {
        public virtual TResult? Visit( IAstNode node, ref readonly TArg arg )
        {
            return VisitChildren(node, in arg);
        }

        public virtual TResult? VisitChildren( IAstNode node, ref readonly TArg arg)
        {
            ArgumentNullException.ThrowIfNull( node );
            TResult? aggregate = DefaultResult;
            foreach( var child in node.Children )
            {
                aggregate = AggregateResult( aggregate, child.Accept( this, in arg ) );
            }

            return aggregate;
        }

        protected AstVisitorBase( TResult? defaultResult )
        {
            DefaultResult = defaultResult;
        }

        protected virtual TResult? AggregateResult( TResult? aggregate, TResult? newResult )
        {
            return newResult;
        }

        protected TResult? DefaultResult { get; }
    }
}

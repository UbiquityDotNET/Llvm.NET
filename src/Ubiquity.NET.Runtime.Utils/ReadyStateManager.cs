// -----------------------------------------------------------------------
// <copyright file="ReadyStateManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Defines state for the UI for REPL parsing to show a prompt to the user</summary>
    public enum ReadyState
    {
        /// <summary>Start of an expression</summary>
        /// <remarks>
        /// Usually this state provides a "Ready" or other similar prompt that indicates ready to accept
        /// any valid language expression.
        /// </remarks>
        StartExpression,

        /// <summary>In the middle of an expression continuation</summary>
        /// <remarks>
        /// Usually this state provides a simpler short prompt to indicate the continuation
        /// of an expression from the previous line.
        /// </remarks>
        ContinueExpression
    }

    /// <summary>Tracks the ready state for a REPL interpreter</summary>
    /// <remarks>
    /// The ready state is used to handle partial input lines where the prompt
    /// presented to the user indicates that input is a continuation of the previous.
    /// </remarks>
    internal class ReadyStateManager
    {
        public ReadyStateManager( Action<ReadyState>? prompt = null )
        {
            PromptAction = prompt;
        }

        public ReadyState State { get; private set; }

        public void Prompt( )
        {
            PromptAction?.Invoke( State );
        }

        public void UpdateState( string txt, bool isPartial )
        {
            /*
                 Current Ready | IsPartial | blank | new value for Ready
            -------------------|-----------|-------|--------------------
              StartExpression  | false     | false | StartExpression
              StartExpression  | false     | true  | <INVALID>
              StartExpression  | true      | false | ContinueExpression
              StartExpression  | true      | true  | StartExpression
            ContinueExpression | false     | x     | StartExpression
            ContinueExpression | true      | x     | ContinueExpression
            */
            if(State == ReadyState.StartExpression)
            {
                bool isBlank = string.IsNullOrWhiteSpace( txt );

                if(!isPartial && isBlank)
                {
                    throw new InvalidOperationException();
                }

                State = isPartial == isBlank ? ReadyState.StartExpression : ReadyState.ContinueExpression;
            }
            else
            {
                State = isPartial ? ReadyState.ContinueExpression : ReadyState.StartExpression;
            }
        }

        private readonly Action<ReadyState>? PromptAction;
    }
}

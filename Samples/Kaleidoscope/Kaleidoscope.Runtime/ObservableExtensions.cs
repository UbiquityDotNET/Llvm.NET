// -----------------------------------------------------------------------
// <copyright file="ObservableExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Linq;

using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Runtime
{
    /// <summary>Extension class to provide support for the IObservable Pattern and System.Reactive.Linq</summary>
    public static class ObservableExtensions
    {
        /// <summary>Parses a sequence of Kaleidoscope source text into AST nodes</summary>
        /// <param name="source">source sequence of expressions</param>
        /// <param name="parser">Kaleidoscope parser stack to parse the input</param>
        /// <param name="errorHandler">Handler for AST Generation errors</param>
        /// <returns>Observable sequence of AST nodes</returns>
        /// <remarks>
        /// This explicitly swallows <see cref="CodeGeneratorException"/> that occur during parse by
        /// calling the <paramref name="errorHandler"/> to report the error and
        /// producing nothing. This allows for a more robust sequence as user input errors are
        /// expected to occur and shouldn't stop the complete sequence.
        /// </remarks>
        public static IObservable<IAstNode> ParseWith( this IObservable<string> source
                                                     , IKaleidoscopeParser parser
                                                     , Action<CodeGeneratorException> errorHandler
                                                     )
        {
            return from expression in source
                   let node = ParseAndReportErrors( expression, parser, errorHandler )
                   where node != null
                   select node;
        }

        /// <summary>Runs the provided generator to transform a sequence of AST nodes to a sequence of the result type <typeparamref name="T"/></summary>
        /// <typeparam name="T">Result type of the generator</typeparam>
        /// <param name="nodes">Sequence of nodes to transform</param>
        /// <param name="generator">generator to use for the transformation</param>
        /// <param name="codeGeneratorExceptionHandler">handler for any code generation errors</param>
        /// <returns>Observable sequence of result type <typeparamref name="T"/></returns>
        /// <remarks>
        /// Code generation exceptions do not halt the sequence, instead they are reported
        /// through the provided <paramref name="codeGeneratorExceptionHandler"/> delegate
        /// and no output for the failing input node is produced.
        /// </remarks>
        #region GenerateResults
        public static IObservable<T> GenerateResults<T>( this IObservable<IAstNode> nodes
                                                       , IKaleidoscopeCodeGenerator<T> generator
                                                       , Action<CodeGeneratorException> codeGeneratorExceptionHandler
                                                       )
            where T : class
        {
            return from n in nodes
                   let v = generator.Generate( n, codeGeneratorExceptionHandler )
                   where !v.Equals(default( T )!)
                   select v;
        }
        #endregion

        private static IAstNode? ParseAndReportErrors( string expression
                                                     , IKaleidoscopeParser parser
                                                     , Action<CodeGeneratorException> codeGeneratorExceptionHandler
                                                     )
        {
            try
            {
                return parser.TryParse( expression, out IAstNode? retVal ) ? retVal : null;
            }
            catch(CodeGeneratorException ex)
            {
                codeGeneratorExceptionHandler( ex );
                return null;
            }
        }
    }
}

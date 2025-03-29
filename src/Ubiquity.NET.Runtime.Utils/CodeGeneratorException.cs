// -----------------------------------------------------------------------
// <copyright file="CodeGeneratorException.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Exception to represent an error in the code generation of a DSL</summary>
    /// <remarks>
    /// This is used to indicate exceptions in the process of transforming an AST into
    /// an intermediate representation or some other form of native code. Errors in parsing
    /// are mostly handled as instances of the <see cref="ErrorNode"/> class or a parse
    /// technology specific exception.
    /// </remarks>
    [Serializable]
    public class CodeGeneratorException
        : Exception
    {
        /// <inheritdoc cref="CodeGeneratorException.CodeGeneratorException(string, Exception)"/>
        public CodeGeneratorException( )
        {
        }

        /// <inheritdoc cref="CodeGeneratorException.CodeGeneratorException(string, Exception)"/>
        public CodeGeneratorException( string message )
            : base( message )
        {
        }

        /// <summary>Initializes a new instance of <see cref="CodeGeneratorException"/></summary>
        /// <param name="message">Message for the exception</param>
        /// <param name="inner">Inner exception</param>
        public CodeGeneratorException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="CodeGeneratorException.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    [Serializable]
    public class CodeGeneratorException
        : Exception
    {
        public CodeGeneratorException( )
        {
        }

        public CodeGeneratorException( string message )
            : base( message )
        {
        }

        public CodeGeneratorException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}

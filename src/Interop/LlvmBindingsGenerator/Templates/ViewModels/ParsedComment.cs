// -----------------------------------------------------------------------
// <copyright file="ParsedComment.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal class ParsedComment
        : IEnumerable<string>
    {
        public ParsedComment( Declaration decl )
        {
            if( decl.Comment != null )
            {
                Lines.AddRange( decl.Comment.Text.Split( AnyNewLine, StringSplitOptions.None ).Select( l => l.Trim( '/' ) ) );
            }
        }

        public IEnumerator<string> GetEnumerator( )
        {
            return Lines.GetEnumerator( );
        }

        IEnumerator IEnumerable.GetEnumerator( )
        {
            return GetEnumerator( );
        }

        private readonly List<string> Lines = new List<string>();

        private static readonly string[ ] AnyNewLine = { "\r\n", "\n" };
    }
}

// -----------------------------------------------------------------------
// <copyright file="ParsedComment.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal class ParsedComment
        : IEnumerable<string>
    {
        public ParsedComment( RawComment c )
        {
            if( c != null )
            {
                Lines.AddRange( c.Text.Split( new[ ] { "\r\n", "\n" }, StringSplitOptions.None ) );
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
    }
}

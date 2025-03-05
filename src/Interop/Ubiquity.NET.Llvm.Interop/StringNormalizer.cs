// -----------------------------------------------------------------------
// <copyright file="StringNormalizer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Ubiquity.NET.Llvm.Interop
{
    public static partial class StringNormalizer
    {
        // LLVM doesn't use environment/OS specific line endings, so this will
        // normalize the line endings from strings provided by LLVM into the current
        // environment's normal format.
        public static unsafe string? NormalizeLineEndings(byte* llvmString)
        {
            return NormalizeLineEndings( ConstStringMarshaller.ConvertToManaged(llvmString) );
        }

        public static string? NormalizeLineEndings(string? txt)
        {
            // shortcut optimization for environments that match the LLVM assumption
            // as well as any null or empty strings
            return string.IsNullOrEmpty( txt ) || IsLineFeedOnlyEnv()
                ? txt
                : LineEndingNormalizingRegEx.Replace( txt, Environment.NewLine );
        }

        private static bool IsLineFeedOnlyEnv()
            => Environment.NewLine.Length == 1
            && Environment.NewLine[ 0 ] == LineFeed;

        private static readonly Regex LineEndingNormalizingRegEx = GeneratedRegExForLineEndings();

        [GeneratedRegex( "(\r\n|\n\r|\r|\n)" )]
        private static partial Regex GeneratedRegExForLineEndings();

        private const char LineFeed = '\n';
    }
}

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
    internal static partial class StringNormalizer
    {
        // LLVM doesn't use environment/OS specific line endings, so this will
        // normalize the line endings from strings provided by LLVM into the current
        // environment's normal format.
        internal static string NormalizeLineEndings(IntPtr llvmString)
        {
            if(llvmString == IntPtr.Zero)
            {
                return string.Empty;
            }

            // PtrToStringAnsi only returns null if input is null, but that is already tested above.
            string str = Marshal.PtrToStringAnsi( llvmString )!;
            return NormalizeLineEndings( str );
        }

        internal static string NormalizeLineEndings(string txt)
        {
            // shortcut optimization for environments that match the LLVM assumption
            return Environment.NewLine.Length == 1 && Environment.NewLine[ 0 ] == '\n'
                ? txt
                : LineEndingNormalizingRegEx.Replace( txt, Environment.NewLine );
        }

        private static readonly Regex LineEndingNormalizingRegEx = GeneratedRegExForLineEndings();

        [GeneratedRegex( "(\r\n|\n\r|\r|\n)" )]
        private static partial Regex GeneratedRegExForLineEndings();
    }
}

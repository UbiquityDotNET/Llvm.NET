// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace LlvmBindingsGenerator
{
    internal static class StringExtensions
    {
        // Runtime agnostic path separator normalization
        internal static string NormalizePathSep( this string path )
        {
            return path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );
        }
    }
}

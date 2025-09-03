// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.IO;

namespace Kaleidoscope.Chapter8
{
    /// <summary>Command line options for this application</summary>
    internal partial class Options
    {
        /// <summary>Gets the source file to use as the reference for debug information</summary>
        /// <remarks>The file is parsed as Kaleidoscope syntax to generate the output files for native code.</remarks>
        public required FileInfo SourcePath { get; init; }
    }
}

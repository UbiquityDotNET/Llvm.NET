// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;

namespace Ubiquity.NET.TextUX
{
    /// <summary>Process related extensions/support</summary>
    public static class ProcessInfo
    {
        /// <summary>Gets the active assembly as of the first use of this property</summary>
        /// <remarks>
        /// The active assembly is the entry assembly which may be null if called from native
        /// code as no such assembly exists for that scenario.
        /// </remarks>
        public static Assembly? ActiveAssembly => field ??= Assembly.GetEntryAssembly();

        /// <summary>Gets the executable path for this instance of an application</summary>
        /// <remarks>This is a short hand for <see cref="Environment.GetCommandLineArgs()"/>[ 0 ]</remarks>
        public static string ExecutablePath => Environment.GetCommandLineArgs()[ 0 ];

        /// <summary>Gets the name of the executable for this instance of an application</summary>
        /// <remarks>This is a short hand for Path.GetFileNameWithoutExtension( <see cref="ExecutablePath"/> )</remarks>
        public static string ExecutableName => Path.GetFileNameWithoutExtension( ExecutablePath );
    }
}

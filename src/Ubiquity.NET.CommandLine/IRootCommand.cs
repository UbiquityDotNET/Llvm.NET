// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.CommandLine;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Interface for a root command in command line parsing</summary>
    /// <typeparam name="T">Type of the command (CRTP)</typeparam>
    /// <remarks>
    /// This only contains a static interface and thus requires that feature of the runtime.
    /// It is used to constrain methods to work with only types that are specifically designed
    /// for command line parsing.
    /// </remarks>
    public interface IRootCommand<T>
        where T : IRootCommand<T>
    {
        /// <summary>Binds the results of the parse to a new instance of <typeparamref name="T"/></summary>
        /// <param name="parseResult">Results of the parse to bind</param>
        /// <returns>Newly constructed instance of <typeparamref name="T"/> with properties bound from <paramref name="parseResult"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when required argument or option was not parsed or has no default value configured.</exception>
        public static abstract T Bind( ParseResult parseResult );

        /// <summary>Builds a new <see cref="AppControlledDefaultsRootCommand"/> for parsing the command line</summary>
        /// <param name="settings">Settings to use for parsing</param>
        /// <returns>New <see cref="AppControlledDefaultsRootCommand"/> instance for <typeparamref name="T"/></returns>
        /// <remarks>
        /// Normally, this just creates an instance of <see cref="AppControlledDefaultsRootCommand"/> and initializes
        /// it with all of the <see cref="Symbol"/>s for a given command line
        /// </remarks>
        public static abstract AppControlledDefaultsRootCommand BuildRootCommand( CmdLineSettings? settings );
    }
}

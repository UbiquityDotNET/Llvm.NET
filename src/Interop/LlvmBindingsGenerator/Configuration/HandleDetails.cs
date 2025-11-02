// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace LlvmBindingsGenerator.Configuration
{
    /// <summary>Details for a handle</summary>
    internal readonly record struct HandleDetails
    {
        /// <summary>Initializes a new instance of the <see cref="HandleDetails"/> struct.</summary>
        /// <param name="name">Name of the handle</param>
        /// <param name="disposer">name of the method for disposal</param>
        /// <param name="alias">Indicates whether this handle may have an unowned alias</param>
        [SetsRequiredMembers]
        public HandleDetails(string name, string? disposer = null, bool alias = false)
        {
            Name = name;
            Disposer = disposer;
            Alias = alias;
        }

        /// <summary>Gets the name of the handle</summary>
        public required string Name { get; init; }

        /// <summary>Gets the name of the disposer for the handle (if any)</summary>
        public string? Disposer { get; init; }

        /// <summary>Gets a value indicating whether this handle has an alias type</summary>
        /// <remarks>
        /// If <see cref="Disposer"/> is <see langword="null"/> or all whitespace then the return is always <see langword="false"/> [Ignoring
        /// any value it is initialized with]
        /// </remarks>
        public bool Alias
        {
            get => !string.IsNullOrWhiteSpace(Disposer) && (field);
            init;
        }
    }
}

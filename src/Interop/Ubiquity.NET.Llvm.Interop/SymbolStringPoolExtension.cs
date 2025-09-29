// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Linq;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.OrcJITv2Bindings;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Representation of information represented in a symbol table Entry</summary>
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "record" )]
    public readonly record struct SymbolEntryInfo
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolEntryInfo"/> struct.</summary>
        /// <param name="name">Name of the symbol</param>
        /// <param name="refCount">Reference count for the symbol</param>
        public SymbolEntryInfo( string name, int refCount )
        {
            Name = name;
            RefCount = refCount;
        }

        /// <summary>Gets the Name of the symbol</summary>
        public string Name { get; }

        public int RefCount { get; }
    }

    /// <summary>Extension APIs for an <see cref="LLVMOrcSymbolStringPoolRef"/></summary>
    public static partial class SymbolStringPoolExtension
    {
        /// <summary>Gets the information for symbols in a pool</summary>
        /// <param name="h">Handle for the pool</param>
        /// <returns>Array of information for the symbols currently in the pool</returns>
        /// <remarks>
        /// <note type="important">
        /// This is intended as a diagnostic utility and is NOT in any way considered performant
        /// or even stable. This implementation depends on an officially undocumented string
        /// formatting of a pool and parsing that. Direct access to the reference count of symbols
        /// is NOT available via any API. The count is completely buried in private implementation
        /// details. This is an unfortunate state of affairs as knowing the count is an important
        /// diagnostic for detection of when the count is off (extra decrement or dangling references)</note>
        /// </remarks>
        public static ImmutableArray<SymbolEntryInfo> GetSymbolsInPool(this LLVMOrcSymbolStringPoolRef h)
        {
            // CAUTION:
            // This is a bit dodgy in that it depends on undocumented behavior.
            // The format of the result string is NOT guaranteed stable.
            string fullString = LibLLVMOrcSymbolStringPoolGetDiagnosticRepresentation(h);
            var q = from entry in fullString.Split('\n')
                    where !string.IsNullOrWhiteSpace(entry)
                    let x = entry.Split(':')
                    select new SymbolEntryInfo(x[0], int.Parse(x[1], CultureInfo.InvariantCulture));

            return [ .. q ];
        }
    }
}

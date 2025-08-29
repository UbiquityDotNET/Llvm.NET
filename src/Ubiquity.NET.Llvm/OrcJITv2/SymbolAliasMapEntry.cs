// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Entry in a symbol alias collection</summary>
    [SuppressMessage( "Design", "CA1001:Types that own disposable fields should be disposable", Justification = "False positive; see: https://github.com/dotnet/roslyn-analyzers/issues/6151" )]
    public readonly record struct SymbolAliasMapEntry
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolAliasMapEntry"/> struct.</summary>
        /// <param name="name">NameField of the symbol</param>
        /// <param name="flags">FlagsField for this symbol</param>
        public SymbolAliasMapEntry( SymbolStringPoolEntry name, SymbolFlags flags )
        {
            ArgumentNullException.ThrowIfNull( name );

            NameField = new( name ); // Force a native AddRef
            Flags = flags;
        }

        /// <summary>Gets the name of the alias</summary>
        /// <returns><see cref="SymbolStringPoolEntry"/> for the name.</returns>
        /// <remarks>
        /// The returned entry has it's own ref count and callers must use the <see cref="SymbolStringPoolEntry.Dispose"/>
        /// method to release it.
        /// </remarks>
        [SuppressMessage( "Design", "CA1024:Use properties where appropriate", Justification = "Rule conflict with IDISP012; Ownership of return rests with caller" )]
        public SymbolStringPoolEntry GetName( )
        {
            return new( NameField );
        }

        /// <summary>Gets the flags for this instance</summary>
        public SymbolFlags Flags { get; init; }

        /// <inheritdoc/>
        public void Dispose( )
        {
            NameField.Dispose();
        }

        internal LLVMOrcCSymbolAliasMapEntry ToABI( ) => new( NameField.ToABI(), Flags.ToABI() );

        private readonly SymbolStringPoolEntry NameField;
    }
}

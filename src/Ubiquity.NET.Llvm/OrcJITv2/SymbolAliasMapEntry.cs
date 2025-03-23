// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Entry in a symbol alias collection</summary>
    [SuppressMessage( "Design", "CA1001:Types that own disposable fields should be disposable", Justification = "False positive; see: https://github.com/dotnet/roslyn-analyzers/issues/6151" )]
    public readonly record struct SymbolAliasMapEntry
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolAliasMapEntry"/> class.</summary>
        /// <param name="name">NameField of the symbol</param>
        /// <param name="flags">FlagsField for this symbol</param>
        public SymbolAliasMapEntry(SymbolStringPoolEntry name, SymbolFlags flags)
        {
            ArgumentNullException.ThrowIfNull( name );

            NameField = new(name); // Force a native AddRef
            Flags = flags;
        }

        /// <summary>Gets the name of the alias</summary>
        /// <remarks>
        /// The returned entry has it's own ref count and callers must use the <see cref="SymbolStringPoolEntry.Dispose"/>
        /// method to release it.
        /// </remarks>
#pragma warning disable IDISP012 // Property should not return created disposable
        public SymbolStringPoolEntry Name => new( NameField );
#pragma warning restore IDISP012 // Property should not return created disposable

        /// <summary>Gets the flags for this instance</summary>
        public SymbolFlags Flags { get; init; }

        /// <inheritdoc/>
        public void Dispose()
        {
            NameField.Dispose();
        }

        internal LLVMOrcCSymbolAliasMapEntry ToABI() => new( NameField.ToABI(), Flags.ToABI() );

        private readonly SymbolStringPoolEntry NameField;
    }
}

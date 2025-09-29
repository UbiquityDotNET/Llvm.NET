// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Entry in a symbol alias collection</summary>
    [SuppressMessage( "Design", "CA1001:Types that own disposable fields should be disposable", Justification = "False positive; it IS disposable. see: https://github.com/dotnet/roslyn-analyzers/issues/6151" )]
    public readonly record struct SymbolAliasMapEntry
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolAliasMapEntry"/> struct.</summary>
        /// <param name="name">Name of the symbol</param>
        /// <param name="flags">Flags for this symbol</param>
        public SymbolAliasMapEntry( SymbolStringPoolEntry name, SymbolFlags flags )
        {
            ArgumentNullException.ThrowIfNull( name );

            NameField = name.AddRef();
            Flags = flags;
        }

        /// <summary>Gets the flags for this instance</summary>
        public SymbolFlags Flags { get; init; }

        /// <inheritdoc/>
        public void Dispose( )
        {
            NameField.Dispose();
        }

#pragma warning disable IDISP004 // Don't ignore created IDisposable; Unless addRef is specified it's an "alias"
        internal LLVMOrcCSymbolAliasMapEntry DangerousGetHandle( bool addRef = false ) => new( NameField.DangerousGetHandle(addRef), Flags.ToABI() );
#pragma warning restore IDISP004 // Don't ignore created IDisposable

        private readonly SymbolStringPoolEntry NameField;
    }
}

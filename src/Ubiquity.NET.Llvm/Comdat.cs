// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ModuleBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Comdat;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Comdat kind/behavior</summary>
    public enum ComdatKind
    {
        /// <summary>Linker may choose any COMDAT key, the choice is arbitrary</summary>
        Any = LLVMComdatSelectionKind.LLVMAnyComdatSelectionKind,

        /// <summary>Linker may choose any COMDAT key but sections must contain the same data</summary>
        ExactMatch = LLVMComdatSelectionKind.LLVMExactMatchComdatSelectionKind,

        /// <summary>The linker will choose the largest section containing the targets COMDAT key</summary>
        Largest = LLVMComdatSelectionKind.LLVMLargestComdatSelectionKind,

        /// <summary>The linker requires that only one section with this COMDAT key exists</summary>
        NoDuplicates = LLVMComdatSelectionKind.LLVMNoDeduplicateComdatSelectionKind,

        /// <summary>Linker may choose any COMDAT key but sections must contain the same amount of data</summary>
        SameSize = LLVMComdatSelectionKind.LLVMSameSizeComdatSelectionKind
    }

    /// <summary>Comdat entry for a module</summary>
    /// <remarks>
    /// A COMDAT is a named kind pair to ensure that, within a given module there are no two named COMDATs
    /// with different kinds. Ultimately, Comdat is 'owned' by the module, if the module is disposed the
    /// Comdats it owns are invalidated. Using a Comdat instance after the module is disposed results in
    /// an effective NOP.
    /// </remarks>
    [SuppressMessage( "Style", "IDE0250:Make struct 'readonly'", Justification = "NO, it can't be, the Kind setter is NOT readonly" )]
#if NET9_0_OR_GREATER
    public readonly ref struct Comdat // IEquatabe<T> only supports ref struct in .NET 9.0 or greater
#else
    public readonly struct Comdat
#endif
        : IEquatable<Comdat>
    {
        /// <summary>Gets the name of the <see cref="Comdat"/></summary>
        public string Name => Handle.IsNull ? string.Empty : LibLLVMComdatGetName( Handle );

        #region IEquatable<Comdat>

        /// <inheritdoc/>
        public bool Equals( Comdat other ) => Handle.Equals( other.Handle );

        /// <inheritdoc/>
        public override int GetHashCode( ) => Handle.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals( object? obj )
        {
            return obj is Comdat comdat && Equals( comdat );
        }

        /// <summary>Compare two instances for equality</summary>
        /// <param name="left">Left side of comparison</param>
        /// <param name="right">Right side of comparison</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/></returns>
        public static bool operator ==( Comdat left, Comdat right ) => left.Equals( right );

        /// <summary>Compare two instances for equality</summary>
        /// <param name="left">Left side of comparison</param>
        /// <param name="right">Right side of comparison</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/></returns>
        public static bool operator !=( Comdat left, Comdat right ) => !(left == right);
        #endregion

        /// <summary>Gets a value indicating whether this instance represents a NULL/Invalid <see cref="Comdat"/></summary>
        public bool IsNull => Handle.IsNull;

        /// <summary>Gets or sets the <see cref="ComdatKind"/> for this <see cref="Comdat"/></summary>
        [SuppressMessage( "Style", "IDE0251:Make member 'readonly'", Justification = "Setter method is 'by definition' NOT readonly! DUH!" )]
        public ComdatKind Kind
        {
            get => Handle.IsNull ? ComdatKind.Any : (ComdatKind)LLVMGetComdatSelectionKind( Handle );

            set
            {
                Handle.ThrowIfInvalid();
                LLVMSetComdatSelectionKind( Handle, (LLVMComdatSelectionKind)value );
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Comdat"/> struct from an LLVM module and reference</summary>
        /// <param name="comdatRef">LLVM-C API handle for the comdat</param>
        [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1642:Constructor summary documentation should begin with standard text", Justification = "Tooling is too stupid to see 'record struct'" )]
        internal Comdat( LLVMComdatRef comdatRef )
        {
            comdatRef.ThrowIfInvalid();
            Handle = comdatRef;
        }

        /// <summary>Gets the wrapped <see cref="LLVMComdatRef"/></summary>
        internal LLVMComdatRef Handle { get; }
    }
}

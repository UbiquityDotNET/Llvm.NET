// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Interface+internal type matches file name
#pragma warning disable SA1649

namespace Ubiquity.NET.Llvm.Types
{
    /// <summary>Interface for a pointer type in LLVM</summary>
    public interface IPointerType
        : ITypeRef
    {
        /// <summary>Gets the address space the pointer refers to</summary>
        uint AddressSpace { get; }

        /// <summary>Gets the element type of this pointer (if available)</summary>
        /// <remarks>LLVM dropped type specific pointers in favor of Opaque pointers.
        /// Unfortunately that means that IR generators need to track the type of
        /// elements and that this property may be <see langword="null"/> if not
        /// known. In practice, this is true when the pointer was retrieved directly
        /// from the lower level LLVM APIs and no Debugging type wrapper exists
        /// with the type. This, at least, will provide it if it is available.
        /// </remarks>
        ITypeRef? ElementType { get; init; }
    }

    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be suppressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx don't support the new syntax yet)
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

    /// <summary>Utility class to provide extensions for <see cref="IPointerType"/></summary>
    /// <remarks>
    /// These are useful even in the presence of default property implementations as the properties
    /// are not accessible accept when the type is explicitly the interface. To access them would require
    /// casting to the interface, just to get at the default implemented property. [Ugly!]
    /// </remarks>
    public static class PointerTypeExtensions
    {
        /// <summary>Gets a value indicating whether <paramref name="ptr"/> is opaque</summary>
        /// <param name="ptr">Pointer type to test</param>
        /// <returns><see langword="true"/> if this pointer is opaque <see langword="false"/> if it is not</returns>
        /// <remarks>
        /// An opaque pointer is one where the <see cref="IPointerType.ElementType"/> is <see langword="null"/>.
        /// This is normal for LLVM now. However, it makes for more complicated code generation. Thus, the
        /// types in this wrapper try to keep track of the type of data a pointer is pointing to. This isn't
        /// always plausible so it might be opaque. This is normally true for anything retrieved from LLVM IR
        /// but is not normal for anything that creates or clones the IR. Since the pointer type creation is
        /// done as a method of the thing being pointed to this information is "attached" to the pointer so
        /// that the <see cref="IPointerType.ElementType"/> is not <see langword="null"/>.
        /// </remarks>
        public static bool IsOpaque( this IPointerType ptr )
        {
            ArgumentNullException.ThrowIfNull( ptr );
            return ptr.ElementType is null;
        }
    }
}

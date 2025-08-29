// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
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
        /// <note Type="note">
        /// until C# 14 [.Net 10] is supported this is an extension method. Once C#14 is available
        /// then this can become a property. Default methods on the interface have too many restrictions
        /// (mostly egregious is the need to "box"/cast to the explicit interface for the lookup to find
        /// the method).
        /// </note>
        /// </remarks>
        public static bool IsOpaque( this IPointerType ptr )
        {
            ArgumentNullException.ThrowIfNull( ptr );
            return ptr.ElementType is null;
        }
    }
}

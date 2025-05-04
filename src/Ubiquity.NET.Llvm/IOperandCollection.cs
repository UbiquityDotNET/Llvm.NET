// -----------------------------------------------------------------------
// <copyright file="IFixedShapeCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Interface for a fixed shape collection of operands</summary>
    /// <typeparam name="T">Type of elements in the container</typeparam>
    /// <remarks>
    /// This interface describes a subset of the behavior of <see cref="ICollection{T}"/>
    /// and <see cref="IList{T}"/> along with an extension of the behavior of <see cref="IReadOnlyList{T}"/>.
    /// The semantics are a collection where the size/shape is not mutable, however the
    /// individual members are. That is the container does not support adding or removing
    /// elements, but does allow replacing existing elements.
    /// </remarks>
    public interface IOperandCollection<T>
        : IReadOnlyCollection<T>
    {
        /// <summary>Gets or sets the specified element in the collection</summary>
        /// <param name="index">index of the element in the collection</param>
        /// <returns>The element in the collection</returns>
        T this[int index] { get; set; }

        /// <summary>Gets a value indicating whether the collection contains the specified item or not</summary>
        /// <param name="item">Item to look for</param>
        /// <returns><see langword="true"/> if the item is found</returns>
        bool Contains( T item );

        /// <summary>Creates a slice of the collection</summary>
        /// <param name="start">Inclusive start index for the slice</param>
        /// <param name="end">Exclusive end index for the slice</param>
        /// <returns>Slice of the collection</returns>
        [SuppressMessage( "Naming", "CA1716:Identifiers should not match keywords", Justification = "Naming is consistent with System.Range parameters" )]
        IOperandCollection<T> Slice( int start, int end )
        {
            return new OperandCollectionSlice<T>( this, new Range( start, end ) );
        }
    }
}

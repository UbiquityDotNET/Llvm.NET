// -----------------------------------------------------------------------
// <copyright file="IAttributeAccessor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Interface for raw attribute access</summary>
    /// <remarks>
    /// As of LLVM v3.9x and later, Functions and call sites use distinct LLVM-C API sets for
    /// manipulating attributes. Fortunately, they have consistent signatures so this interface
    /// is used to abstract the difference via derived types specialized for each case. This
    /// interface abstracts the differences so that access from callers is the same.
    /// </remarks>
    /// <ImplementationNaote>
    /// The CallSiteAttributeAccessors static class has all the functionality for call sites.
    /// The FunctionAttributeAccessors static class handles functions the other cases.
    /// </ImplementationNaote>
    public interface IFunctionAttributeAccessor
    {
        /// <summary>Gets the context for the container, used for creating attributes</summary>
        IContext Context { get; }

        /// <summary>Gets the count of attributes on a given index</summary>
        /// <param name="index">Index to get the count for</param>
        /// <returns>Number of attributes on the specified index</returns>
        uint GetAttributeCountAtIndex( FunctionAttributeIndex index );

        /// <summary>Gets the attributes on a given index</summary>
        /// <param name="index">index to get the attributes for</param>
        /// <returns>Attributes for the index</returns>
        IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index );

        /// <summary>Gets a specific attribute at a given index</summary>
        /// <param name="index">Index to get the attribute from</param>
        /// <param name="id">ID of the attribute to get</param>
        /// <returns>The specified attribute or the default <see cref="AttributeValue"/></returns>
        AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, UInt32 id );

        /// <summary>Gets a named attribute at a given index</summary>
        /// <param name="index">Index to get the attribute from</param>
        /// <param name="name">name of the attribute to get</param>
        /// <returns>The specified attribute or the default <see cref="AttributeValue"/></returns>
        AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name );

        /// <summary>Adds an <see cref="AttributeValue"/> at a specified index</summary>
        /// <param name="index">Index to add the attribute to</param>
        /// <param name="attrib">Attribute to add</param>
        void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib );

        /// <summary>Removes an <see cref="AttributeInfo"/> at a specified index</summary>
        /// <param name="index">Index to add the attribute to</param>
        /// <param name="id">ID of the attribute to Remove</param>
        void RemoveAttributeAtIndex( FunctionAttributeIndex index, UInt32 id );

        /// <summary>Removes a named attribute at a specified index</summary>
        /// <param name="index">Index to add the attribute to</param>
        /// <param name="name">Name of the attribute to remove</param>
        void RemoveAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name );
    }
}

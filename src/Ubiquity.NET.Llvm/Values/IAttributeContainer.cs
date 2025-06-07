// -----------------------------------------------------------------------
// <copyright file="AttributeContainerMixins.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Interface for an attribute container</summary>
    /// <remarks>
    /// Attribute containers have a set of attributes that are NOT indexed
    /// by a <see cref="FunctionAttributeIndex"/>. (For example, an <see cref="Argument"/>
    /// has attributes specific to the one parameter that it is for.)
    /// </remarks>
    public interface IAttributeContainer
    {
        /// <summary>Gets the context used for creation of attributes</summary>
        IContext Context { get; }

        /// <summary>Gets a collection of attributes for this container</summary>
        ICollection<AttributeValue> Attributes { get; }
    }
}

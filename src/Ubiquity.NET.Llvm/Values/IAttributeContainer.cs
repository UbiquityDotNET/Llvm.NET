// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

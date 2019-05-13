// -----------------------------------------------------------------------
// <copyright file="IOperandContainer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Llvm.NET
{
    /// <summary>Internal interface to provide raw access to operands of a container type</summary>
    /// <typeparam name="T">type of operands</typeparam>
    /// <remarks>
    /// This is used to build an operand list for multiple container types that is ultimately
    /// exposed as a public property on the container.
    /// </remarks>
    internal interface IOperandContainer<T>
    {
        /// <summary>Gets the count of operands in the container</summary>
        long Count { get; }

        /// <summary>Gets or sets an operand</summary>
        /// <param name="index">Raw index of the operand in the container</param>
        /// <returns>Operand from the container</returns>
        T this[ int index ] { get; set; }

        /// <summary>Adds an item to the container</summary>
        /// <param name="item">item to add</param>
        /// <exception cref="NotSupportedException">If the container doesn't support adding items</exception>
        void Add( T item );
    }
}

// -----------------------------------------------------------------------
// <copyright file="IOperandContainer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
using System;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Internal interface to provide raw access to operands of a container type</summary>
    /// <typeparam name="T">type of operands</typeparam>
    /// <remarks>
    /// This is used to build an operand list for multiple container types that is ultimately
    /// exposed as a public property on the container.
    /// </remarks>
    internal interface IOperandContainer<T>
        where T : class
    {
        /// <summary>Gets the count of operands in the container</summary>
        long Count { get; }

        /// <summary>Gets an operand from the container</summary>
        /// <param name="index">Raw index of the operand in the container</param>
        /// <returns>Operand from the container</returns>
        TItem? GetRawOperandAt<TItem>( int index )
            where TItem : class, T;

        /// <summary>Sets an operand in the container</summary>
        /// <param name="index">Raw index of the operand in the container</param>
        /// <param name="value">Value to set at the index</param>
        void SetRawOperandAt( int index, T? value );

        /// <summary>Adds an item to the end of the container (i.e. append)</summary>
        /// <param name="item">item to add</param>
        /// <exception cref="NotSupportedException">If the container doesn't support adding items</exception>
        /// <exception cref="ArgumentNullException">If the container doesn't support adding null items</exception>
        void Add( T? item );
    }
}
*/

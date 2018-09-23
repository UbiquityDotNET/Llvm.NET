﻿// <copyright file="WriteOnce.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET
{
    /// <summary>Wrapper class to provide Write-Once semantics to a value</summary>
    /// <typeparam name="T">TYpe of value to store</typeparam>
    /// <remarks>
    /// This provides write once semantics for fields that may require initialization
    /// outside the context of a constructor, but once set should never be set again.
    /// Allowing a sort of lazy <see langword="readonly"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay( "{ValueOrDefault}" )]
    internal sealed class WriteOnce<T>
    {
        /// <inheritdoc/>
        public override string ToString( )
        {
            return HasValue ? Convert.ToString( ActualValue ) : string.Empty;
        }

        /// <summary>Gets or sets the value for this instance</summary>
        /// <exception cref="InvalidOperationException">Getting the value when no value is set</exception>
        /// <exception cref="InvalidOperationException">Setting the value when a value is already set</exception>
        public T Value
        {
            get
            {
                if( !HasValue )
                {
                    throw new InvalidOperationException( "Value not set" );
                }

                return ActualValue;
            }

            set
            {
                if( HasValue )
                {
                    throw new InvalidOperationException( "Value already set" );
                }

                ActualValue = value;
                HasValue = true;
            }
        }

        /// <summary>Initializer</summary>
        /// <param name="value">value to initialize</param>
        public delegate void Initializer( out T value );

        /// <summary>Initializes the value via a delegate using an out parameter</summary>
        /// <param name="initializer">delegate to initialize the value from</param>
        /// <returns>This instance for fluent style use</returns>
        public WriteOnce<T> InitializeWith( Initializer initializer )
        {
            initializer( out ActualValue );
            HasValue = true;
            return this;
        }

        /// <summary>Gets a value indicating whether the <see cref="Value"/> was set or not</summary>
        public bool HasValue { get; private set; }

        /// <summary>Gets the current value or the default value for <typeparamref name="T"/> if not yet set</summary>
        public T ValueOrDefault => ActualValue;

        /// <summary>Convenience implicit cast as a wrapper around the <see cref="Value"/> parameter</summary>
        /// <param name="value"> <see cref="WriteOnce{T}"/> instance to extract a value from</param>
        public static implicit operator T( WriteOnce<T> value ) => value.Value;

        private T ActualValue;
    }
}

// <copyright file="HandleValidation.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Native.Handles
{
    // TODO: Move this to ArgValidators package
    internal static class HandleValidation
    {
        /// <summary>Verifies a that a value is not equal to the default for the type</summary>
        /// <typeparam name="T">Type of value to test for</typeparam>
        /// <param name="value">Value to test</param>
        /// <param name="paramName">Name of the parameter for the argument exception generated</param>
        /// <returns><paramref name="value"/> for fluent design usage</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is the default value for value types or <see lang="null"/> for reference types</exception>
        [DebuggerStepThrough]
        public static T ValidateNotDefault<T>( [ValidatedNotNull] this T value, [InvokerParameterName] string paramName )
        {
            if( EqualityComparer<T>.Default.Equals( value, default ) )
            {
                throw new ArgumentNullException( paramName );
            }

            return value;
        }
    }
}

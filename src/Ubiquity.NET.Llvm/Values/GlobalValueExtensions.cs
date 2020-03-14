// -----------------------------------------------------------------------
// <copyright file="GlobalValueExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.ArgValidators;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Fluent style extensions for modifying properties of a <see cref="GlobalValue"/></summary>
    /// <remarks>
    /// These are not members of <see cref="GlobalValue"/> to allow the generic return type so that the return
    /// is for the specific type provided as input instead of the base type of GlobalValue.
    /// </remarks>
    public static class GlobalValueExtensions
    {
        /// <summary>Visibility of this global value</summary>
        /// <typeparam name="T">Type of the value (Must be <see cref="GlobalValue"/> or a type derived from it)</typeparam>
        /// <param name="self">Value to modify</param>
        /// <param name="value">New value to set</param>
        /// <returns><paramref name="self"/> for fluent usage patterns</returns>
        /// <seealso cref="GlobalValue.Visibility"/>
        public static T Visibility<T>( [ValidatedNotNull] this T self, Visibility value )
            where T : GlobalValue
        {
            self.ValidateNotNull( nameof( self ) );
            self.Visibility = value;
            return self;
        }

        /// <summary>Linkage specification for this symbol</summary>
        /// <typeparam name="T">Type of the value (Must be <see cref="GlobalValue"/> or a type derived from it)</typeparam>
        /// <param name="self">Value to modify</param>
        /// <param name="value">New value to set</param>
        /// <returns><paramref name="self"/> for fluent usage patterns</returns>
        /// <seealso cref="GlobalValue.Linkage"/>
        public static T Linkage<T>( [ValidatedNotNull] this T self, Linkage value )
            where T : GlobalValue
        {
            self.ValidateNotNull( nameof( self ) );
            self.Linkage = value;
            return self;
        }

        /// <summary>Sets the UnnamedAddress property of a value</summary>
        /// <typeparam name="T">Type of the value (Must be <see cref="GlobalValue"/> or a type derived from it)</typeparam>
        /// <param name="self">Value to modify</param>
        /// <param name="value">New value to set</param>
        /// <returns><paramref name="self"/> for fluent usage patterns</returns>
        /// <seealso cref="GlobalValue.UnnamedAddress"/>
        public static T UnnamedAddress<T>( [ValidatedNotNull] this T self, UnnamedAddressKind value )
            where T : GlobalValue
        {
            self.ValidateNotNull( nameof( self ) );
            self.UnnamedAddress = value;
            return self;
        }
    }
}

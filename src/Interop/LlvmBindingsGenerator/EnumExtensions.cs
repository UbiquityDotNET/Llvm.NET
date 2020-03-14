// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace LlvmBindingsGenerator
{
    internal static class EnumExtensions
    {
        public static IEnumerable<T> GetEnumValues<T>( )
            where T : Enum
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>( );
        }
    }
}

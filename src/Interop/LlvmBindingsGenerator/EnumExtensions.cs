// -----------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace LlvmBindingsGenerator
{
    internal static class EnumExtensions
    {
        public static IEnumerable<T> GetEnumValues<T>()
            where T : Enum
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>( );
        }
    }
}

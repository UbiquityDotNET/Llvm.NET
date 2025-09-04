// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.IO;

namespace Ubiquity.NET.CommandLine.UT
{
    internal static class SettingsTestExtensions
    {
        public static ImmutableArray<string> GetOutput( this StringWriter self )
        {
            ArgumentNullException.ThrowIfNull( self );
            string underlyingString = self.ToString();
            return string.IsNullOrWhiteSpace( underlyingString )
                ? []
                : [ .. underlyingString.Split( self.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries ) ];
        }
    }
}

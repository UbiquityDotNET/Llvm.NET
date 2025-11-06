// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Extensions.FluentValidation;

namespace Ubiquity.NET.Extensions.Properties
{
    internal static class StringResourceExtensions
    {
        // RESX file generator does not use `partial` so C# 14 extension is the only option...
        extension(Ubiquity.NET.Extensions.Properties.Resources)
        {
            internal static string Format<TArg0>( [NotNull][StringSyntax(StringSyntaxAttribute.CompositeFormat)] string fmt, TArg0 arg0 )
            {
                fmt.ThrowIfNull();
                return string.Format( CultureInfo.CurrentCulture, fmt, arg0 );
            }

            internal static string Format<TArg0, TArg1>( [NotNull][StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt, TArg0 arg0, TArg1 arg1 )
            {
                fmt.ThrowIfNull();
                return string.Format( CultureInfo.CurrentCulture, fmt, arg0, arg1 );
            }

            internal static string Format<TArg0, TArg1, TArg3>( [NotNull][StringSyntax( StringSyntaxAttribute.CompositeFormat )] string fmt, TArg0 arg0, TArg1 arg1, TArg3 arg3 )
            {
                fmt.ThrowIfNull();
                return string.Format( CultureInfo.CurrentCulture, fmt, arg0, arg1, arg3 );
            }
        }

#if NET7_0_OR_GREATER
        // This is just a utility method, not an extension
        internal static CompositeFormat ParseAsFormat( [NotNull][StringSyntax( StringSyntaxAttribute.CompositeFormat )] this string? self )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( self );
            return CompositeFormat.Parse( self );
        }
#endif
    }
}

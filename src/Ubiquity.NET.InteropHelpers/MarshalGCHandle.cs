// -----------------------------------------------------------------------
// <copyright file="MarshalGCHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Utility class for supporting use of a GCHandle (as a raw native <see cref="void"/> pointer)</summary>
    /// <remarks>
    /// Many native APIs use a callback with a "Context" as a raw native <see cref="void"/> pointer. It is common in
    /// such cases to use a <see cref="GCHandle"/> as the context.
    /// </remarks>
    public static class MarshalGCHandle
    {
        /// <summary>Try pattern to convert a <see cref="GCHandle"/> as a raw native <see cref="void"/> pointer</summary>
        /// <typeparam name="T">Type of object the context should hold</typeparam>
        /// <param name="ctx">Native pointer for the context</param>
        /// <param name="value">Value of the context or default if return is <see langword="false"/></param>
        /// <returns><see langword="true"/> if the <paramref name="ctx"/> is valid and converted to <paramref name="value"/></returns>
        /// <remarks>
        /// <para>This assumes that <paramref name="ctx"/> is a <see cref="GCHandle"/> that was allocated and provided to native code via
        /// a call to <see cref="GCHandle.ToIntPtr(GCHandle)"/>. This will follow the try pattern to resolve <paramref name="ctx"/>
        /// back to the original instance the handle is referencing. This allows managed code callbacks to use managed objects as
        /// an opaque "context" value for native APIs.</para>
        ///
        /// <note type="important">
        /// In order to satisfy nullability code analysis, call sites must declare <typeparamref name="T"/> explicitly. Otherwise,
        /// it is deduced as the type used for <paramref name="value"/>, which will cause analysis to complain if it isn't a nullable type.
        /// Thus, without explicit declaration of the type without nullability it is assumed nullable and the <see cref="MaybeNullWhenAttribute"/>
        /// is effectively moot. So call sites should always specify the generic type parameter <typeparamref name="T"/> explicitly.
        /// </note>
        /// </remarks>
        public static unsafe bool TryGet<T>( void* ctx, [MaybeNullWhen( false )] out T value )
        {
            if(ctx is not null && GCHandle.FromIntPtr( (nint)ctx ).Target is T self)
            {
                value = self;
                return true;
            }

            value = default;
            return false;
        }
    }
}

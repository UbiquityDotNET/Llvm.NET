// -----------------------------------------------------------------------
// <copyright file="LazyInitializedString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

using Ubiquity.NET.Llvm.Interop.Properties;

namespace Ubiquity.NET.Llvm.Interop.ABI.StringMarshaling
{
    /// <summary>This class provides the core functionality for a lazily initialized string</summary>
    /// <remarks>
    /// When doing interoperability with native code it is common to find strings that are used with
    /// the native code but rarely, if ever, used as a string in managed code. Normal "blind" marshalling
    /// would incur the costs of marshalling the string into  native code, and then back again to native
    /// EVERY time it was retrieved or need to call a native API. This class helps avoid all of that
    /// extra overhead. It encapsulates all of the logic and data associated with lazy initialization
    /// to ensure that a managed string, if used is ONLY marshalled once and ONLY if it is actually needed.
    /// <note type="important">
    /// This class does NOT own the native string and does not perform any cleanup of that data. The APIs
    /// defined here all accept a native string pointer but capture it and throw an exception if a mismatch
    /// is detected. They ALL handle an input of <see langword="null"/> and return a default value for the
    /// return. This default return occurs even if the string was initialized properly once to help prevent
    /// misuse while allowing the containing type to release it's native resources to provide a
    /// <see langword="null"/> for the raw pointer.
    /// </note>
    /// </remarks>
    public sealed class LazyInitializedString
        : IDisposable
    {
        public unsafe ReadOnlySpan<byte> GetReadOnlySpan(byte* nativeString)
        {
            if (nativeString is null)
            {
                return default;
            }

            LazyCaptureNativeString(nativeString);

            // At the very least, this needs to know the native byte length of the string - including the null terminator
            // so make sure it is computed/scanned only once.
            LazyInitializer.EnsureInitialized<int>( ref LazyNativeByteLen, ref LengthInitialized, ref LazySyncLock, GetNativeStringByteLen );
            return new ReadOnlySpan<byte>( CapturedNativeString, LazyNativeByteLen );

            unsafe int GetNativeStringByteLen() => ExecutionEncodingStringMarshaller.ReadOnlySpanFromNullTerminated( CapturedNativeString ).Length + 1;
        }

        public unsafe string? ToString(byte* nativeString)
        {
            if( nativeString is null)
            {
                return null;
            }

            LazyCaptureNativeString(nativeString);
            LazyInitializer.EnsureInitialized(ref LazyString, ref LazySyncLock, MakeLazyString);
            return LazyString;

            // local function to perform one-time initialization of the string
            string MakeLazyString() => ExecutionEncodingStringMarshaller.ConvertToManaged( CapturedNativeString ) ?? string.Empty;
        }

        public void Dispose()
        {
            lock(LazySyncLock)
            {
                LazyString = null;
                LazyNativeByteLen = 0;
                LengthInitialized = false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void LazyCaptureNativeString(byte* nativeString)
        {
            // NOTE: Null protection of nativeString provided by callers
            Debug.Assert(nativeString is not null, "Internal error - nativeString param should NEVER be null!");

            lock(LazySyncLock)
            {
                if (nativeString != CapturedNativeString)
                {
                    CapturedNativeString = CapturedNativeString is null
                                         ? nativeString
                                         : throw new ArgumentException(Resources.Already_captured_different_native_string, nameof(nativeString));
                }
            }
        }

        private unsafe byte* CapturedNativeString;
        private object LazySyncLock = new();
        private string? LazyString;
        private bool LengthInitialized;
        private int LazyNativeByteLen;
    }
}

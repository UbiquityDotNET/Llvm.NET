using System;

namespace Llvm.NET.Native
{
    internal static class IntPtrExtensions
    {
        public static bool IsNull( this IntPtr self ) => self == IntPtr.Zero;

        public static bool IsNull( this UIntPtr self ) => self == UIntPtr.Zero;
    }
}

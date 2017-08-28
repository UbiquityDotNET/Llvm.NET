using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    internal struct LLVMVersionInfo
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;
        private readonly IntPtr VersionStringPtr;

        public override string ToString( )
        {
            return Marshal.PtrToStringAnsi( VersionStringPtr );
        }

        public static implicit operator Version( LLVMVersionInfo versionInfo )
            => new Version( versionInfo.Major, versionInfo.Minor, versionInfo.Patch );
    }
}

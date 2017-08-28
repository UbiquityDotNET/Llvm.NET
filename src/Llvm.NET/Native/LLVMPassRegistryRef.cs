using System;
using System.Security;

namespace Llvm.NET.Native
{
    [SecurityCritical]
    internal class LLVMPassRegistryRef
        : SafeHandleNullIsInvalid
    {
        internal LLVMPassRegistryRef( )
            : base( true )
        {
        }

        internal LLVMPassRegistryRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal LLVMPassRegistryRef( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.PassRegistryDispose( handle );
            return true;
        }
    }
}

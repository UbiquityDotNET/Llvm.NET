using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Llvm.NET.Native
{
    internal static class IntPtrExtensions
    {
        public static bool IsNull( this IntPtr self ) => self == IntPtr.Zero;
        public static bool IsNull( this UIntPtr self ) => self == UIntPtr.Zero;
    }

    /// <summary>Base class for LLVM disposable types that are instantiated outside of an LLVM <see cref="Context"/> and therefore won't be disposed by the context</summary>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    internal abstract class SafeHandleNullIsInvalid
        : SafeHandle
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected SafeHandleNullIsInvalid( bool ownsHandle)
            : base( IntPtr.Zero, ownsHandle )
        {
        }

        public bool IsNull => handle.IsNull();

        public override bool IsInvalid
        {
            [SecurityCritical]
            get
            {
                return IsNull;
            }
        }
    }

    [SecurityCritical]
    internal class AttributeBuilderHandle
        : SafeHandleNullIsInvalid
    {
        internal AttributeBuilderHandle()
            : base( true )
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal AttributeBuilderHandle( IntPtr handle )
            : base( true )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.AttributeBuilderDispose( handle );
            return true;
        }
    }

    [SecurityCritical]
    internal class PassRegistryHandle
        : SafeHandleNullIsInvalid
    {
        internal PassRegistryHandle( )
            : base( true )
        {
        }

        internal PassRegistryHandle( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal PassRegistryHandle( IntPtr handle )
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

    // typedef struct LLVMOpaqueTriple* LLVMTripleRef;
    [SecurityCritical]
    internal class TripleHandle
        : SafeHandleNullIsInvalid
    {
        internal TripleHandle( )
            : base( true )
        {
        }

        internal TripleHandle( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal TripleHandle( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.DisposeTriple( handle );
            return true;
        }
    }

    [SecurityCritical]
    internal class InstructionBuilderHandle
        : SafeHandleNullIsInvalid
    {
        internal InstructionBuilderHandle( )
            : base( true )
        {
        }

        internal InstructionBuilderHandle( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for marshaling support (used via reflection)" )]
        internal InstructionBuilderHandle( IntPtr handle )
            : this( handle, false )
        {
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            NativeMethods.DisposeBuilder( handle );
            return true;
        }
    }
}

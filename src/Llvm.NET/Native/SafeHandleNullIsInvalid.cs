// <copyright file="SafeHandleNullIsInvalid.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Llvm.NET.Native
{
    /// <summary>Base class for LLVM disposable types that are instantiated outside of an LLVM <see cref="Llvm.NET.Context"/> and therefore won't be disposed by the context</summary>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    internal abstract class SafeHandleNullIsInvalid
        : SafeHandle
    {
        public bool IsNull => handle.IsNull();

        public override bool IsInvalid
        {
            [SecurityCritical]
            get => IsNull;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected SafeHandleNullIsInvalid( bool ownsHandle)
            : base( IntPtr.Zero, ownsHandle )
        {
        }
    }
}

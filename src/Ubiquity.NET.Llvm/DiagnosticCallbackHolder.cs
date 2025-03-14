// -----------------------------------------------------------------------
// <copyright file="Context.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Delegate for a diagnostic info callback</summary>
    /// <param name="info">Diagnostic information to process</param>
    /// <example>
    /// <code><![CDATA[
    /// if( info.Severity <= DiagnosticSeverity.Warning)
    /// {
    ///     using var msg = info.Description
    ///     Debug.WriteLine( "{0}: {1}", level, msg );
    /// }
    /// ]]></code>
    /// </example>
    public delegate void DiagnosticInfoCallbackAction(DiagnosticInfo info);

    internal sealed class DiagnosticCallbackHolder
        : IDisposable
    {
        public DiagnosticCallbackHolder(DiagnosticInfoCallbackAction diagnosticHandler)
        {
            Delegate = diagnosticHandler;
            AllocatedSelf = new( this );
        }

        public void Dispose()
        {
            if(!AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                // Decrements the ref count on the handle
                // might not actually destroy anything
                AllocatedSelf.Dispose();
            }
        }

        internal unsafe nint AddRefAndGetNativeContext()
        {
            return AllocatedSelf.AddRefAndGetNativeContext();
        }

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        internal static unsafe void DiagnosticHandler(nint abiInfo, void* context)
        {
            try
            {
                if(context is not null && GCHandle.FromIntPtr( (nint)context ).Target is DiagnosticCallbackHolder self)
                {
                    self.Delegate( new( abiInfo ) );
                }
            }
            catch
            {
                Debugger.Break();
            }
        }

        private readonly DiagnosticInfoCallbackAction Delegate;
        private readonly SafeGCHandle AllocatedSelf;
    }
}

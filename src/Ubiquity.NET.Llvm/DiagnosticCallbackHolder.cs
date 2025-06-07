// -----------------------------------------------------------------------
// <copyright file="ContextAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Delegate for a diagnostic info callback</summary>
    /// <param name="info">Diagnostic information to process</param>
    /// <example>
    /// <code><![CDATA[
    /// void MyDiagnosticCallback(DiagnosticInfo info)
    /// {
    ///     if( info.Severity <= DiagnosticSeverity.Warning)
    ///     {
    ///         Debug.WriteLine( "{0}: {1}", level, info.Description );
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public delegate void DiagnosticInfoCallbackAction( DiagnosticInfo info );

    internal sealed class DiagnosticCallbackHolder
        : IDisposable
    {
        public DiagnosticCallbackHolder( DiagnosticInfoCallbackAction diagnosticHandler )
        {
            Delegate = diagnosticHandler;
            AllocatedSelf = new( this );
        }

        public void Dispose( )
        {
            if(!AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                // Decrements the ref count on the handle
                // might not actually destroy anything
                AllocatedSelf.Dispose();
            }
        }

        internal unsafe nint AddRefAndGetNativeContext( )
        {
            return AllocatedSelf.AddRefAndGetNativeContext();
        }

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        internal static unsafe void DiagnosticHandler( nint abiInfo, void* context )
        {
            try
            {
                if(MarshalGCHandle.TryGet<DiagnosticCallbackHolder>( context, out DiagnosticCallbackHolder? self ))
                {
                    self.Delegate( new( abiInfo ) );
                }
            }
            catch
            {
                // stop in debugger as this is a detected app error.
                // Test for attached debugger directly to avoid prompts, WER cruft etc...
                // End user should NOT be prompted to attach a debugger!
                if(Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }

        private readonly DiagnosticInfoCallbackAction Delegate;
        private readonly SafeGCHandle AllocatedSelf;
    }
}

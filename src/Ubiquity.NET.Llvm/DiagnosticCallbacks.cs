// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

    // native callbacks for an LLVM context Diagnostic messages.
    internal static class DiagnosticCallbacks
    {
        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        internal static unsafe void DiagnosticHandler( nint abiInfo, void* context )
        {
            try
            {
                if(NativeContext.TryFrom<DiagnosticInfoCallbackAction>(context, out var self ))
                {
                    self( new( abiInfo ) );
                }
            }
            catch
            {
                // SAFETY: stop in debugger as this is a detected app error.
                // Test for attached debugger directly to avoid prompts, WER cruft etc...
                // End user should NOT be prompted to attach a debugger, if one is already
                // attached then stop as the resulting exception is likely an app crash!
                if(Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }
    }
}

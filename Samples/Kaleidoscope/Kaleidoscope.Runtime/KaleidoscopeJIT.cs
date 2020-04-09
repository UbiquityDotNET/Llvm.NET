// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.JIT;

namespace Kaleidoscope.Runtime
{
    /// <summary>JIT engine for Kaleidoscope language</summary>
    /// <remarks>
    /// This engine uses the <see cref="Ubiquity.NET.Llvm.JIT.OrcJit"/> engine to support lazy
    /// compilation of LLVM IR modules added to the JIT.
    /// </remarks>
    public sealed class KaleidoscopeJIT
        : OrcJit
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        public KaleidoscopeJIT( )
            : base( BuildTargetMachine( ) )
        {
            AddInteropCallback( "putchard", new CallbackHandler1( PutChard ) );
            AddInteropCallback( "printd", new CallbackHandler1( Printd ) );
        }

        /// <summary>Gets or sets the output writer for output from the program.</summary>
        /// <remarks>The default writer is <see cref="Console.Out"/>.</remarks>
        public TextWriter OutputWriter { get; set; } = Console.Out;

        /// <summary>Delegate for an interop callback taking no parameters</summary>
        /// <returns>value for the function</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler0( );

        /// <summary>Delegate for an interop callback taking one parameters</summary>
        /// <param name="arg1">First parameter</param>
        /// <returns>value for the function</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler1( double arg1 );

        /// <summary>Delegate for an interop callback taking two parameters</summary>
        /// <param name="arg1">First parameter</param>
        /// <param name="arg2">Second parameter</param>
        /// <returns>value for the function</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler2( double arg1, double arg2 );

        /// <summary>Delegate for an interop callback taking three parameters</summary>
        /// <param name="arg1">First parameter</param>
        /// <param name="arg2">Second parameter</param>
        /// <param name="arg3">Third parameter</param>
        /// <returns>value for the function</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler3( double arg1, double arg2, double arg3 );

        /// <summary>Delegate for an interop callback taking four parameters</summary>
        /// <param name="arg1">First parameter</param>
        /// <param name="arg2">Second parameter</param>
        /// <param name="arg3">Third parameter</param>
        /// <param name="arg4">Fourth parameter</param>
        /// <returns>value for the function</returns>
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler4( double arg1, double arg2, double arg3, double arg4 );

        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "Native callback *MUST NOT* surface managed exceptions" )]
        private double Printd( double x )
        {
            // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
            try
            {
                OutputWriter.WriteLine( x );
                return 0.0F;
            }
            catch
            {
                return 0.0;
            }
        }

        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "Native callback *MUST NOT* surface managed exceptions" )]
        private double PutChard( double x )
        {
            // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
            try
            {
                OutputWriter.Write( ( char )x );
                return 0.0F;
            }
            catch
            {
                return 0.0;
            }
        }

        private static TargetMachine BuildTargetMachine( )
        {
            string hostTriple = Triple.HostTriple.ToString( );
            return Target.FromTriple( hostTriple )
                         .CreateTargetMachine( hostTriple
                                             , /*cpu*/null
                                             , /*features*/null
                                             , CodeGenOpt.Default
                                             , RelocationMode.Default
                                             , CodeModel.JitDefault
                                             );
        }
    }
}

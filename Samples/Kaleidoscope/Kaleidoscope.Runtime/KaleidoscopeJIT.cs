// <copyright file="KaleidoscopeJIT.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Runtime.InteropServices;
using Llvm.NET;
using Llvm.NET.JIT;

namespace Kaleidoscope.Runtime
{
    /// <summary>JIT engine for Kaleidoscope language</summary>
    /// <remarks>
    /// This engine uses the <see cref="Llvm.NET.JIT.OrcJit"/> engine to support lazy
    /// compilation of LLVM IR modules added to the JIT.
    /// </remarks>
    public sealed class KaleidoscopeJIT
        : OrcJit
    {
        /// <summary>Initializes a new instance of the <see cref="KaleidoscopeJIT"/> class.</summary>
        public KaleidoscopeJIT( )
            : base( BuildTargetMachine() )
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

        private static TargetMachine BuildTargetMachine()
        {
            return Target.FromTriple( Triple.HostTriple.ToString( ) )
                         .CreateTargetMachine( Triple.HostTriple.ToString( ), null, null, CodeGenOpt.Default, RelocationMode.Default, CodeModel.JitDefault );
        }
    }
}

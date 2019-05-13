// -----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Ubiquity.ArgValidators;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

namespace Kaleidoscope.Runtime
{
    public static class Utilities
    {
        /// <summary>Waits for a debugger attach</summary>
        /// <param name="condition">Condition to determine when to wait for the debugger</param>
        /// <remarks>
        /// This is useful for Mixed mode debugging the new SDK projects as they don't directly
        /// support debug launch with mixed debugging. Instead you launch without debugging and
        /// call this at the start of Main() then use the Attach option to attach using both
        /// managed and native. This is a regression in the project system for the new "SDK" type
        /// .NET projects that will hopefully be resolved in the not too distant future, until it
        /// is this hack will have to do.
        /// <note type="note">
        /// In release builds this is a NOP due to the use of the <see cref="ConditionalAttribute"/>
        /// </note>
        /// </remarks>
        public static void WaitForDebugger( bool condition )
        {
            if(!condition || Debugger.IsAttached )
            {
                return;
            }

            if( !Debugger.IsAttached)
            {
                Console.WriteLine( "Waiting for Debugger attach... PID: {0}", Process.GetCurrentProcess().Id );
            }

            while( !Debugger.IsAttached )
            {
                System.Threading.Thread.Sleep( 5000 );
            }
        }

        /// <summary>replaces any characters in a name that are invalid for a file name</summary>
        /// <param name="name">name to convert</param>
        /// <returns>name with invalid characters replaced with '_'</returns>
        public static string GetSafeFileName( string name )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            var bldr = new StringBuilder( name.Length );
            char[ ] invalidChars = Path.GetInvalidFileNameChars( );
            foreach( char c in name )
            {
                bldr.Append( invalidChars.Contains( c ) ? '_' : c );
            }

            return bldr.ToString( );
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="TestContextTextWriter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaleidoscope.Tests
{
    internal class TestContextTextWriter
        : TextWriter
    {
        public TestContextTextWriter( TestContext context )
        {
            Context = context;
        }

        public override Encoding Encoding => Encoding.Unicode;

        public override void Write( char value )
        {
            if( value == '\n' )
            {
                // remove any preceding \r
                if( Builder.Length > 0 && Builder[ 0 ] == '\r' )
                {
                    Builder.Remove( Builder.Length - 1, 1 );
                }

                WriteLine( );
            }
            else
            {
                Builder.Append( value );
            }
        }

        public override void WriteLine( )
        {
            Context.WriteLine( Builder.ToString( ) );
            Builder.Clear( );
        }

        private readonly TestContext Context;
        private readonly StringBuilder Builder = new StringBuilder();
    }
}

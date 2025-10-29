// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kaleidoscope.Tests
{
    // Adapter - Used as a target to output text of the runs to the test context
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
            if(value == '\n')
            {
                // remove any preceding \r
                if(Builder.Length > 0 && Builder[ 0 ] == '\r')
                {
                    Builder.Remove( Builder.Length - 1, 1 );
                }

                WriteLine();
            }
            else
            {
                Builder.Append( value );
            }
        }

        public override void WriteLine( )
        {
            Context.WriteLine( Builder.ToString() );
            Builder.Clear();
        }

        private readonly TestContext Context;
        private readonly StringBuilder Builder = new();
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

namespace Ubiquity.NET.SrcGeneration.UT
{
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Record" )]
    [ExcludeFromCodeCoverage]
    internal readonly record struct LineData( string Input, ImmutableArray<string> Expected );

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringExtensionTests
    {
        public TestContext TestContext { get; set; }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        // tests are intended to VALIDATE nullability checks in implementation
        [TestMethod]
        public void Extensions_throw_if_null_self( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                StringExtensions.GetCommentLines( null );
            } );
            Assert.AreEqual("self", ex.ParamName);

            ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                StringExtensions.EscapeComment( null );
            } );
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                StringExtensions.SplitLines( null );
            } );
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                StringExtensions.MakeXmlSafe( null );
            } );
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                StringExtensions.EscapeForComment( null );
            } );
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                var strings = StringExtensions.SkipDuplicates( null );
            } );

            Assert.AreEqual( "self", ex.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void GetCommentLines_splits_string_correctly( )
        {
            // default behavior is StringSplitOptions2.TrimEntries - verify that
            var testData = GetCommentLinesTestData( StringSplitOptions2.TrimEntries );
            var actual = StringExtensions.GetCommentLines(testData.Input).ToImmutableArray();
            VerifyLineData( testData.Expected, actual );
        }

        [TestMethod]
        [DataRow( StringSplitOptions2.None )]
        [DataRow( StringSplitOptions2.RemoveEmptyEntries )]
        [DataRow( StringSplitOptions2.TrimEntries )]
        [DataRow( StringSplitOptions2.TrimEntries | StringSplitOptions2.RemoveEmptyEntries )]
        public void GetCommentLines_with_options( StringSplitOptions2 options )
        {
            var testData = GetCommentLinesTestData( options );
            var actual = StringExtensions.GetCommentLines(testData.Input, options).ToImmutableArray();
            VerifyLineData( testData.Expected, actual );
        }

        [TestMethod]
        public void EscapeComment_handles_newline_escapes( )
        {
            const string input = "line0\\nline1\\nline2";
            string expected = "line0" + Environment.NewLine + "line1" + Environment.NewLine + "line2";
            string actual = StringExtensions.EscapeComment(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SplitLines_default_option_is_none( )
        {
            var testData = GetSplitLinesTestData(StringSplitOptions2.None);
            var actual = StringExtensions.SplitLines(testData.Input).ToImmutableArray();
            VerifyLineData(testData.Expected, actual);
        }

        [TestMethod]
        [DataRow( StringSplitOptions2.None )]
        [DataRow( StringSplitOptions2.RemoveEmptyEntries )]
        [DataRow( StringSplitOptions2.TrimEntries )]
        [DataRow( StringSplitOptions2.TrimEntries | StringSplitOptions2.RemoveEmptyEntries )]
        public void SplitLines_with_options( StringSplitOptions2 options )
        {
            var testData = GetSplitLinesTestData( options );
            var actual = StringExtensions.SplitLines(testData.Input, options).ToImmutableArray();
            VerifyLineData( testData.Expected, actual );
        }

        [TestMethod]
        public void MakeXmlSafe_handles_XML_Conversion( )
        {
            Assert.AreEqual("&amp;", StringExtensions.MakeXmlSafe("&"));
            Assert.AreEqual( "&lt;text2&gt;", StringExtensions.MakeXmlSafe("<text2>"));

            // additional escapes are testable but it comes down to
            // testing XText(...).ToString() at that point...
        }

        [TestMethod]
        public void SkipDuplicates_skips_any_duplicate_line( )
        {
            ImmutableArray<string> input = [
                "text",
                "text2",
                "text2", // duplicate of previous line - expect removed,
                "text",  // duplicate of first line, but not previous - expect remains
            ];

            ImmutableArray<string> expected = [
                "text",
                "text2",
                /* "text2", // duplicate of previous line - expect removed, */
                "text",  // duplicate of first line, but not previous - expect remains
            ];

            var actual = StringExtensions.SkipDuplicates(input).ToImmutableArray();
            VerifyLineData(expected, actual);
        }

        [TestMethod]
        public void EscapeForComment_escapes_each_string( )
        {
            ImmutableArray<string> input = [
                "text\\npart2",
                "text2",
                "text2",
                "text\\n",
                "\\ntext"
            ];

            ImmutableArray<string> expected = [
                "text" + Environment.NewLine + "part2",
                "text2",
                "text2",
                "text" + Environment.NewLine,
                Environment.NewLine + "text",
            ];

            var actual = StringExtensions.EscapeForComment(input).ToImmutableArray();
            VerifyLineData( expected, actual );
        }

        [TestMethod]
        public void EscapeForXML_escapes_each_string( )
        {
            ImmutableArray<string> input = [
                "&this or that",
                "<text2>",
                "this or &that",
            ];

            ImmutableArray<string> expected = [
                "&amp;this or that",
                "&lt;text2&gt;",
                "this or &amp;that",
            ];

            var actual = StringExtensions.EscapeForXML(input).ToImmutableArray();
            VerifyLineData( expected, actual );
        }

        private void VerifyLineData( ImmutableArray<string> expected, ImmutableArray<string> actual )
        {
            TestContext.Report( "expected", expected);
            TestContext.Report( "actual", actual);
            Assert.HasCount( expected.Length, actual );
            for(int i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual( expected[ i ], actual[ i ], $"Mismatch on element {i}" );
            }
        }

        private static LineData GetCommentLinesTestData( StringSplitOptions2 options )
        {
            // determine expected results based on options
            string line0 = "Comment line0";
            string line1 = "   s   e   ";
            string line2 = "   ";
            string line3 = string.Empty;
            string line4 = "Comment line4";

            string testInput = string.Join(Environment.NewLine, line0, line1, line2, line3, line4);

            var bldr = ImmutableArray.CreateBuilder<string>();

            bldr.Add(line0); // never altered, always included.

            // if Trim entries is specified, then trim lines 1 & 2
            // These are the only input lines that can be trimmed.
            if(options.HasFlag( StringSplitOptions2.TrimEntries ))
            {
                line1 = line1.Trim();
                line2 = line2.Trim();
            }

            // never empty, but might be trimmed
            bldr.Add(line1);

            // line 2 & 3 might be empty strings and might be removed.
            if(options.HasFlag( StringSplitOptions2.RemoveEmptyEntries ))
            {
                if(!string.IsNullOrEmpty( line2 ))
                {
                    bldr.Add( line2 );
                }

                // IFF line3 is not empty AND not the same as previous line add it
                // duplicates always removed.
                if(!string.IsNullOrEmpty( line3 ) && (bldr[ ^1 ] != line3))
                {
                    bldr.Add( line3 );
                }
            }
            else
            {
                bldr.Add( line2 );

                // IFF line3 is not the same as previous line add it; duplicates always removed.
                if(bldr[ ^1 ] != line3)
                {
                    bldr.Add( line3 );
                }
            }

            bldr.Add( line4 );

            return new(testInput, bldr.ToImmutable());
        }

        private static LineData GetSplitLinesTestData( StringSplitOptions2 options )
        {
            // determine expected results based on options
            string line0 = "Comment line0";
            string line1 = "   s   e   ";
            string line2 = "   ";
            string line3 = string.Empty;
            string line4 = "Comment line4";

            string testInput = string.Join(Environment.NewLine, line0, line1, line2, line3, line4);

            var bldr = ImmutableArray.CreateBuilder<string>();

            bldr.Add( line0 ); // never altered, always included.

            // if Trim entries is specified, then trim lines 1 & 2
            // These are the only input lines that can be trimmed.
            if(options.HasFlag( StringSplitOptions2.TrimEntries ))
            {
                line1 = line1.Trim();
                line2 = line2.Trim();
            }

            // never empty, but might be trimmed
            bldr.Add( line1 );

            // line 2 & 3 might be empty strings and might be removed.
            if(options.HasFlag( StringSplitOptions2.RemoveEmptyEntries ))
            {
                if(!string.IsNullOrEmpty( line2 ))
                {
                    bldr.Add( line2 );
                }

                if(!string.IsNullOrEmpty( line3 ))
                {
                    bldr.Add( line3 );
                }
            }
            else
            {
                bldr.Add( line2 );
                bldr.Add( line3 );
            }

            bldr.Add( line4 );

            return new( testInput, bldr.ToImmutable() );
        }
    }
}

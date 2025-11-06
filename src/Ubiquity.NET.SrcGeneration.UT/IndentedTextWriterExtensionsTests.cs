// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.SrcGeneration.UT
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class IndentedTextWriterExtensionsTests
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        // nullability checks is the point of this test...
        [TestMethod]
        public void WriteEmptyLine_throws_if_null( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>IndentedTextWriterExtensions.WriteEmptyLine(null));
            Assert.AreEqual( "self", ex.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void WriteEmptyLine_writes_a_blank_line_without_indentation( )
        {
            using var writer = new OwningIndentedStringWriter();
            writer.WriteLine("line1");
            IndentedTextWriterExtensions.WriteEmptyLine(writer);
            writer.Write("line2 [no trailing newline]");
            Assert.AreEqual( TestEmptyLine, writer.ToString() );
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        // nullability checks is the point of this test...
        [TestMethod]
        public void PushIndent_throws_if_null( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
                {
                    using var x = IndentedTextWriterExtensions.PushIndent(null);
                });
            Assert.AreEqual( "self", ex.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void PushIndent_correctly_pushes_indentation_level( )
        {
            using var writer = new OwningIndentedStringWriter();
            Assert.AreEqual( 0, writer.Indent, "SANITY: Writer should start at level 0" );
            using(var scope1 = IndentedTextWriterExtensions.PushIndent( writer ))
            {
                Assert.AreEqual( 1, writer.Indent, "Indentation level should be +1 while indented" );
                using(var scope2 = IndentedTextWriterExtensions.PushIndent( writer ))
                {
                    Assert.AreEqual( 2, writer.Indent, "Indentation level should be +1 while indented" );
                }

                Assert.AreEqual( 1, writer.Indent, "Indentation level should be -1 after indentation" );
            }

            Assert.AreEqual( 0, writer.Indent, "Disposal of scope should result in indentation level of 0" );
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        // nullability checks is the point of this test...
        [TestMethod]
        public void Block_throws_if_null( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                using var x = IndentedTextWriterExtensions.Block(null, "{","}");
            });
            Assert.AreEqual( "self", ex.ParamName );

            using var writer = new OwningIndentedStringWriter();
            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                using var x = IndentedTextWriterExtensions.Block(writer, null,"}");
            } );

            Assert.AreEqual( "open", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) =>
            {
                using var x = IndentedTextWriterExtensions.Block(writer, "{",null);
            } );

            Assert.AreEqual( "close", ex.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void Block_produces_correct_results_for_defaults( )
        {
            using var writer = new OwningIndentedStringWriter();
            using(var scope = IndentedTextWriterExtensions.Block( writer, "begin", "end" ))
            {
                writer.WriteLine( "test line" );
            }

            Assert.AreEqual( 0, writer.Indent );
            Assert.AreEqual( TestBlock, writer.ToString());
        }

        [TestMethod]
        public void Block_produces_correct_results_with_leading_line( )
        {
            using var writer = new OwningIndentedStringWriter();
            using(var scope = IndentedTextWriterExtensions.Block( writer, "begin", "end", "<leading line>" ))
            {
                writer.WriteLine( "test line" );
            }

            Assert.AreEqual( 0, writer.Indent );
            Assert.AreEqual( TestBlockWithLeadingLine, writer.ToString() );
        }

        [TestMethod]
        public void Block_produces_correct_results_with_leading_line_no_indentation( )
        {
            using var writer = new OwningIndentedStringWriter();
            using(var scope = IndentedTextWriterExtensions.Block( writer, "begin", "end", "<leading line>", indented: false ))
            {
                writer.WriteLine( "test line" );
            }

            Assert.AreEqual( 0, writer.Indent );
            Assert.AreEqual( TestBlockWithLeadingLineNoIndentation, writer.ToString() );
        }

        [TestMethod]
        public void Block_produces_correct_results_with_no_indentation( )
        {
            using var writer = new OwningIndentedStringWriter();
            using(var scope = IndentedTextWriterExtensions.Block( writer, "begin", "end", indented: false ))
            {
                writer.WriteLine( "test line" );
            }

            Assert.AreEqual( 0, writer.Indent );
            Assert.AreEqual( TestBlockWithNoIndentation, writer.ToString() );
        }

        private const string TestBlock = """
        begin
            test line
        end
        """;

        private const string TestBlockWithLeadingLine = """
        <leading line>
        begin
            test line
        end
        """;

        private const string TestBlockWithLeadingLineNoIndentation = """
        <leading line>
        begin
        test line
        end
        """;

        private const string TestBlockWithNoIndentation = """
        begin
        test line
        end
        """;

        private const string TestEmptyLine = """
        line1

        line2 [no trailing newline]
        """;
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#if !NET6_0_OR_GREATER
// need to import the namespace to allow implicit access to extensions
using System.Text;
#endif

namespace Ubiquity.NET.Extensions.UT
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class PolyFillStringExtensionsTests
    {
        // since these are ONLY an extension with runtime's prior to .NET 6
        // test the handling. For .NET6 or later, that's up to .NET itself to test
#if !NET6_0_OR_GREATER
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [TestMethod]
        public void Methods_throw_on_invalid_input( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillStringExtensions.ReplaceLineEndings( null ));
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillStringExtensions.ReplaceLineEndings( null, "replacement" ));
            Assert.AreEqual( "self", ex.ParamName );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillStringExtensions.ReplaceLineEndings( "source text", null ) );
            Assert.AreEqual( "replacementText", ex.ParamName );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#endif

        // Technically this tests both poly filled and official implementations, but validates the
        // assumptions that exist betweeen them. If both pass then the poly fill is replicating the
        // tested behavior of the official runtime implementation. (If, perhaps, less performant...)

        [TestMethod]
        public void ReplaceLineEndings_uses_Environment_newlines( )
        {
            const string inputMixedLines = "line0\r\nline1\rline2\nline3\fline4\u0085line5\u2028line6\u2029";
            string expected = "line0" + Environment.NewLine
                            + "line1" + Environment.NewLine
                            + "line2" + Environment.NewLine
                            + "line3" + Environment.NewLine
                            + "line4" + Environment.NewLine
                            + "line5" + Environment.NewLine
                            + "line6" + Environment.NewLine;

            string actual = inputMixedLines.ReplaceLineEndings();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReplaceLineEndings_uses_provided_string( )
        {
            const string inputMixedLines = "line0\r\nline1\rline2\nline3\fline4\u0085line5\u2028line6\u2029";
            const string expected = "line0<NL>line1<NL>line2<NL>line3<NL>line4<NL>line5<NL>line6<NL>";

            string actual = inputMixedLines.ReplaceLineEndings("<NL>");
            Assert.AreEqual( expected, actual );
        }
    }
}

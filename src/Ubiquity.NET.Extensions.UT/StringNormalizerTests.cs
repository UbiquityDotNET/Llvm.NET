// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Extensions.UT
{
    // NOTE: Test output will include special characters for CR and LF characters that makes it easy to visualize
    // Example:
    //     Assert.AreEqual failed. Expected string length 51 but was 52. 'expected' expression: 'expectedOutput', 'actual' expression: 'systemNormalizedInput'.
    // Expected: "This is a line␊This is anotherline␊And..."
    // But was:  "This is a line␍␊This is anotherline␍An..."
    // -------------------------^

    // NOTE: In C# the string "line1\nLine2" has exactly what was put in the string
    //       That is, it contains a SINGLE LF '\n' character. NOT an environment
    //       specific "newline". Thus, if a string needs to represent an platform specific
    //       newline sequence it can use `Environment.NewLine` or `string.ReplaceLineEndings()`.
    //       The docs on `ReplaceLineEndings()` are silent on the point of input forms
    //       replaced. However, seplunking the code indicates it follows Unicode standard §5.8,
    //       Recommendation R4 and Table 5-2 (CR, LF, CRLF, NEL, LS, FF, PS). Explicitly excluded
    //       is VT. Thus, that will normalize ANY newline sequence to the form expected by the
    //       environment.

    [TestClass]
    public sealed class StringNormalizerTests
    {
        [TestMethod]
        public void System_line_ending_detected_correctly( )
        {
            Assert.AreEqual( OsDefaultLineEnding, StringNormalizer.SystemLineEndings);
        }

        [TestMethod]
        public void Normalize_with_default_endings_does_nothing( )
        {
            string testInput = "This is a line\nAnd so is this".ReplaceLineEndings(); // Platform sepecific
            string normalizedOutput = testInput.NormalizeLineEndings(StringNormalizer.SystemLineEndings);
            Assert.AreSame(testInput, normalizedOutput, "Should return same instance (zero copy)");
        }

        [TestMethod]
        public void Normalize_with_alternate_endings_produces_new_string( )
        {
            string testInput = "This is a line\nAnd so is this".ReplaceLineEndings(); // Platform sepecific
            const string expectedOutput = "This is a line\rAnd so is this";

            // CR Only is not the default for any currently supported runtinme for .NET so this
            // remains a platform neutral test - verify that assumption!
            // See also: System_line_ending_detected_correctly()
            Assert.AreNotEqual(LineEndingKind.CarriageReturn, StringNormalizer.SystemLineEndings, "TEST ERROR: CR is default line ending for this runtime!");

            string normalizedOutput = testInput.NormalizeLineEndings(LineEndingKind.CarriageReturn);
            Assert.AreEqual(expectedOutput, normalizedOutput);
        }

        [TestMethod]
        public void Normalize_with_mixed_input_succeeds()
        {
            const string mixedInput = "This is a line\r\nThis is anotherline\rAnd aonther line";
            string expectedOutput = "This is a line\nThis is anotherline\nAnd aonther line".ReplaceLineEndings(); // Platform sepecific
            string systemNormalizedInput = mixedInput.NormalizeLineEndings(LineEndingKind.MixedOrUnknownEndings, StringNormalizer.SystemLineEndings);
            Assert.AreEqual(expectedOutput, systemNormalizedInput);
        }

        // Technincally Mac OS prior to OS X (Lion) use CR, but .NET does not
        // support those older versions. Thus, this only treats Windows as the
        // "odd man out", everything else uses LF.
        private static LineEndingKind OsDefaultLineEnding
            => OperatingSystem.IsWindows()
                ? LineEndingKind.CarriageReturnLineFeed
                : LineEndingKind.LineFeed;
    }
}

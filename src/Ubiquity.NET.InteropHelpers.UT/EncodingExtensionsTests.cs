// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.InteropHelpers.UT
{
    [TestClass]
    public class EncodingExtensionsTests
    {
        [TestMethod]
        public void MarshalStringTest( )
        {
            string? emptyString = EncodingExtensions.MarshalString(Encoding.UTF8, []);
            Assert.IsNotNull( emptyString );
            Assert.AreEqual( string.Empty, emptyString );

            // NOTE: u8 places a null terminator in the allocated space for the string!
            // [See: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/utf8-string-literals]
            var testMsg="Testing 1,2,3"u8;
            string expectedManagedString = "Testing 1,2,3";
            unsafe
            {
                string? nullString = Encoding.UTF8.MarshalString((byte*)null);
                Assert.IsNull( nullString );

                fixed(byte* pMsg = &MemoryMarshal.GetReference( testMsg ))
                {
                    string? managedString = EncodingExtensions.MarshalString(Encoding.UTF8, pMsg);
                    Assert.IsNotNull( managedString );
                    Assert.AreEqual( expectedManagedString, managedString );
                }
            }
        }
    }
}

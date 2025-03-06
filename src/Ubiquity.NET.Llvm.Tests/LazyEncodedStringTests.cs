// -----------------------------------------------------------------------
// <copyright file="LazyEncodedStringTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class LazyEncodedStringTests
    {
        [TestMethod]
        public void TestManagedToNative()
        {
            const string initString = "123";
            byte[] expectedNativeBytes = [0x31, 0x32, 0x33, 0];

            var lazyString = new LazyEncodedString(initString);
            Assert.IsNotNull(lazyString);
            Assert.AreEqual(initString, lazyString.ToString());
            Assert.AreEqual(initString, lazyString); // tests implicit cast operator
            Assert.AreEqual(expectedNativeBytes.Length, lazyString.ToReadOnlySpan().Length);
            ReadOnlySpan<byte> span = lazyString; // tests implicit cast operator
            Assert.IsTrue(((ReadOnlySpan<byte>)expectedNativeBytes).SequenceEqual(span));
            var span2 = lazyString.ToReadOnlySpan();
            Assert.IsTrue(((ReadOnlySpan<byte>)expectedNativeBytes).SequenceEqual(span2));
        }

        [TestMethod]
        public void TestNativeToManaged()
        {
            const string expectedString = "123";
            ReadOnlySpan<byte> initNativeBytes = [0x31, 0x32, 0x33, 0];

            var lazyString = new LazyEncodedString(initNativeBytes);
            Assert.IsNotNull(lazyString);
            Assert.AreEqual(expectedString, lazyString.ToString());
            Assert.AreEqual(expectedString, lazyString); // tests implicit cast operator
            Assert.AreEqual(initNativeBytes.Length, lazyString.ToReadOnlySpan().Length);
            ReadOnlySpan<byte> span = lazyString; // tests implicit cast operator
            Assert.IsTrue(initNativeBytes.SequenceEqual(span));
            var span2 = lazyString.ToReadOnlySpan();
            Assert.IsTrue(initNativeBytes.SequenceEqual(span2));
        }

        [TestMethod]
        public void TestNativeToManagedWithUnterminatedNative()
        {
            // Test verifies a terminator is added to the native span, even if one was left off.
            // That is, it just deals with it if you throw it a span that isn't accounting for
            // one (It has to copy the data anyway, so why not?)
            const string expectedString = "123";
            ReadOnlySpan<byte> initNativeBytes = [0x31, 0x32, 0x33];
            ReadOnlySpan<byte> expectedNativeBytes = [0x31, 0x32, 0x33, 0];

            var lazyString = new LazyEncodedString(initNativeBytes);
            Assert.IsNotNull(lazyString);
            Assert.AreEqual(expectedString, lazyString.ToString());
            Assert.AreEqual(expectedString, lazyString); // tests implicit cast operator
            Assert.AreEqual(expectedNativeBytes.Length, lazyString.ToReadOnlySpan().Length);
            ReadOnlySpan<byte> span = lazyString; // tests implicit cast operator
            Assert.IsTrue(expectedNativeBytes.SequenceEqual(span));
            var span2 = lazyString.ToReadOnlySpan();
            Assert.IsTrue(expectedNativeBytes.SequenceEqual(span2));
        }
    }
}

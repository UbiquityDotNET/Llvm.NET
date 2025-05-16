using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.NET.InteropHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers.Tests
{
    [TestClass()]
    public class LazyEncodedStringTests
    {
        [TestMethod()]
        public void LazyEncodedStringTest( )
        {
            var lazyString = new LazyEncodedString("managed");
            Assert.IsNotNull( lazyString );
            Assert.IsFalse( lazyString.IsEmpty );
            Assert.AreEqual( Encoding.UTF8, lazyString.Encoding );
        }

        [TestMethod()]
        public void LazyEncodedStringTest1( )
        {
            var lazyString = new LazyEncodedString("utf8 Text"u8);
            Assert.IsNotNull( lazyString );
            Assert.IsFalse( lazyString.IsEmpty );
            Assert.AreEqual( Encoding.UTF8, lazyString.Encoding );
        }

        [TestMethod()]
        public void ToStringTest( )
        {
            var lazyString = new LazyEncodedString("utf8 Text"u8);
            string managed = lazyString.ToString();
            Assert.IsNotNull( managed );
            Assert.AreEqual( "utf8 Text", managed );

            const string managedText = "managed Text";
            var lazyString2 = new LazyEncodedString(managedText);
            string managed2 = lazyString2.ToString();
            Assert.IsNotNull( managed2 );
            Assert.AreEqual( managedText, managed2 );
        }

        [TestMethod()]
        public void ToReadOnlySpanTest( )
        {
            const string managedText = "Some Text";
            var utf8Span = "Some Text"u8;

            var lazyString = new LazyEncodedString(utf8Span);
            var containedSpan = lazyString.ToReadOnlySpan();
            Assert.IsTrue( utf8Span.SequenceEqual( containedSpan ) );

            var lazyString2 = new LazyEncodedString(managedText);
            containedSpan = lazyString2.ToReadOnlySpan();
            Assert.IsTrue( utf8Span.SequenceEqual( containedSpan ) );
        }

        [TestMethod()]
        public void PinTest( )
        {
            const string managedText = "Some Text";
            var utf8Span = "Some Text"u8;

            var lazyString = new LazyEncodedString(utf8Span);
            using(var memHandle = lazyString.Pin())
            {
                unsafe
                {
                    var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)memHandle.Pointer);
                    Assert.IsTrue( utf8Span.SequenceEqual( span ) );
                }
            }

            lazyString = new LazyEncodedString( managedText );
            using(var memHandle = lazyString.Pin())
            {
                unsafe
                {
                    var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)memHandle.Pointer);
                    Assert.IsTrue( utf8Span.SequenceEqual( span ) );
                }
            }
        }

        /*
        Test matrix for this:
        # | lhs     | rhs     | Result
        --|---------|---------|-----------
        1 | null    | null    | Always equal
        2 | null    | managed | Never equal
        3 | null    | native  | Never equal
        4 | managed | null    | Never equal
        5 | managed | managed | Equal if contents of string are equal (ordinal compare)
        6 | managed | native  | Equal if contents of string are equal (ordinal compare)
        7 | native  | null    | Never equal
        8 | native  | managed | Equal if contents of string are equal (ordinal compare)
        9 | native  | native  | Equal if contents of native array are equal
        */
        [TestMethod()]
        public void EqualsTest( )
        {
            LazyEncodedString? nullString1 = null;
            LazyEncodedString? nullString2 = null;
            LazyEncodedString managed1 = "tEst one";
            LazyEncodedString native1 = "tEst one"u8;
            LazyEncodedString managed1Rhs = "tEst one";
            LazyEncodedString native1Rhs = "tEst one"u8;

            LazyEncodedString managed2 = "tEst two";
            LazyEncodedString native2 = "tEst two"u8;
            LazyEncodedString managed2Rhs = "tEst two";
            LazyEncodedString native2Rhs = "tEst two"u8;

            // case 1:
            Assert.IsTrue( nullString1 == nullString2 );
            Assert.IsTrue( Equals( nullString1, nullString1 ) );
            Assert.IsTrue( null == nullString1 );
            Assert.IsTrue( Equals( null, nullString1 ) );
            Assert.IsTrue( nullString1 == null );
            Assert.IsTrue( Equals( nullString1, null ) );

            // case 2:
            Assert.IsFalse( nullString1 == managed1 );
            Assert.IsFalse( Equals( nullString1, managed1 ) );

            // case 3:
            Assert.IsFalse( nullString1 == managed1 );
            Assert.IsFalse( Equals( nullString1, native1 ) );

            // case 4:
            Assert.IsFalse( managed1 == nullString1 );
            Assert.IsFalse( Equals( managed1, nullString1 ) );

            // case 5:
            Assert.IsFalse( managed1 == managed2 );
            Assert.IsFalse( managed1.Equals( managed2 ) );
            Assert.IsTrue( managed1 == managed1Rhs );
            Assert.IsTrue( managed1.Equals( managed1Rhs ) );

            // case 6:
            Assert.IsFalse( managed1 == native2 );
            Assert.IsFalse( managed1.Equals( native2 ) );
            Assert.IsTrue( managed1 == native1 );
            Assert.IsTrue( managed1.Equals( native1 ) );

            // case 7:
            Assert.IsFalse( native1 == nullString1 );
            Assert.IsFalse( Equals( native1, nullString1 ) );

            // case 8:
            Assert.IsFalse( native2 == managed1 );
            Assert.IsFalse( native2.Equals( managed1 ) );
            Assert.IsTrue( native1 == managed1 );
            Assert.IsTrue( native1.Equals( managed1 ) );

            // case 9:
            Assert.IsFalse( native1 == native2 );
            Assert.IsFalse( native1.Equals( native2 ) );
            Assert.IsTrue( native1 == native1Rhs );
            Assert.IsTrue( native1.Equals( native1Rhs ) );
        }

        [TestMethod()]
        public void GetHashCodeTest( )
        {
            LazyEncodedString managed1 = "tEst one";
            LazyEncodedString native1 = "tEst one"u8;
            Assert.AreNotEqual(0, managed1.GetHashCode());
            Assert.AreNotEqual(0, native1.GetHashCode());
            Assert.AreEqual(managed1.GetHashCode(), native1.GetHashCode());
        }
    }
}

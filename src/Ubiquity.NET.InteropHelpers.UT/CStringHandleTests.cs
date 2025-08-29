// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.InteropHelpers.UT
{
    [TestClass]
    public class CStringHandleTests
    {
        [TestMethod]
        public void EqualityTests( )
        {
            unsafe
            {
                // need to use a real string as the length is computed for it (Lazily)
                var testMsg = "testing1,2,3"u8;
                var testMsg2 = "testing1,2,3"u8;
                fixed(byte* pMsg = &MemoryMarshal.GetReference( testMsg ))
                {
                    var handle1 = new TestStringHandle();
                    var handle2 = new TestStringHandle();
                    var handle3 = new TestStringHandle(pMsg);
                    Assert.IsFalse( handle1.IsClosed );
                    Assert.IsFalse( handle2.IsClosed );
                    Assert.IsFalse( handle3.IsClosed );

                    Assert.IsTrue( handle1.IsInvalid );
                    Assert.IsTrue( handle2.IsInvalid );
                    Assert.IsFalse( handle3.IsInvalid );

                    using(handle1)
                    using(handle2)
                    using(handle3)
                    {
                        Assert.AreEqual( handle1, handle2, "Different instances but NULL value should report value equal" );
                        Assert.AreNotEqual( handle1, handle3, "Different instances with different value should NOT report as equal" );
                        Assert.IsTrue( handle3.Equals( testMsg2 ), "Should compare equal with same content" );
                    }

                    // Verify dispose handling is correct.
                    // Actual pointer is not supposed to change, but it should change state
                    // and should have called the release method once (for any valid handle)
                    Assert.IsTrue( handle1.IsClosed );
                    Assert.AreEqual( 0ul, handle1.ReleaseCount );
                    Assert.AreEqual( nint.Zero, handle1.DangerousGetHandle() );

                    Assert.IsTrue( handle2.IsClosed );
                    Assert.AreEqual( 0ul, handle2.ReleaseCount );
                    Assert.AreEqual( nint.Zero, handle2.DangerousGetHandle() );

                    Assert.IsTrue( handle3.IsClosed );
                    Assert.AreEqual( 1ul, handle3.ReleaseCount );
                    Assert.AreEqual( (nint)pMsg, handle3.DangerousGetHandle() );
                }
            }
        }

        [TestMethod]
        public void ReadOnlySpanPropertyTests( )
        {
            // need to use a real string as the length is computed for it (Lazily)
            var testMsg = "testing1,2,3"u8;
            var testMsg2 = "testing1,2,3"u8;
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference( testMsg ))
                {
                    using var testHandle = new TestStringHandle(pMsg);
                    Assert.IsTrue( testHandle.ReadOnlySpan.SequenceEqual( testMsg2 ), "span should evaluate as equal to identical content span" );
                }
            }
        }

        [TestMethod]
        public void ToStringTests( )
        {
            using var handle1 = new TestStringHandle();
            Assert.IsNull( handle1.ToString(), "invalid handle should produce a null string" );

            var testMsg = "testing1,2,3"u8;
            string testMsgManaged = "testing1,2,3";
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference( testMsg ))
                {
                    using var testHandle = new TestStringHandle(pMsg);
                    Assert.AreEqual( testMsgManaged, testHandle.ToString() );
                }
            }
        }

        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Testing API" )]
        public void GetHashCodeTests( )
        {
            using var invalidHandle = new TestStringHandle();
            Assert.AreEqual( 0, invalidHandle.GetHashCode() );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.CurrentCulture ) );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.CurrentCultureIgnoreCase ) );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.InvariantCulture ) );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.InvariantCultureIgnoreCase ) );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.Ordinal ) );
            Assert.AreEqual( 0, invalidHandle.GetHashCode( StringComparison.OrdinalIgnoreCase ) );

            var testMsg = "Testing1,2,3"u8;
            string testMsgManaged = "Testing1,2,3";
            unsafe
            {
                fixed(byte* pMsg = &MemoryMarshal.GetReference( testMsg ))
                {
                    using var testHandle = new TestStringHandle(pMsg);
                    Assert.AreEqual( testMsgManaged.GetHashCode(), testHandle.GetHashCode() );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.CurrentCulture ), testHandle.GetHashCode( StringComparison.CurrentCulture ) );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.CurrentCultureIgnoreCase ), testHandle.GetHashCode( StringComparison.CurrentCultureIgnoreCase ) );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.InvariantCulture ), testHandle.GetHashCode( StringComparison.InvariantCulture ) );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.InvariantCultureIgnoreCase ), testHandle.GetHashCode( StringComparison.InvariantCultureIgnoreCase ) );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.Ordinal ), testHandle.GetHashCode( StringComparison.Ordinal ) );
                    Assert.AreEqual( testMsgManaged.GetHashCode( StringComparison.OrdinalIgnoreCase ), testHandle.GetHashCode( StringComparison.OrdinalIgnoreCase ) );
                }
            }
        }
    }

    // Internal type to test abstract base type
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "It's file scoped - https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3803" )]
    file class TestStringHandle
        : CStringHandle
    {
        public TestStringHandle( )
            : base()
        {
        }

        public unsafe TestStringHandle( byte* p )
            : base( p )
        {
        }

        public TestStringHandle( nint p )
            : base( p )
        {
        }

        public UInt64 ReleaseCount => ReleaseCountField;

        protected override bool ReleaseHandle( )
        {
            Interlocked.Increment( ref ReleaseCountField );
            return true;
        }

        private UInt64 ReleaseCountField;
    }
}

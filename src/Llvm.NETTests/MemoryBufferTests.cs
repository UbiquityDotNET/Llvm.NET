// -----------------------------------------------------------------------
// <copyright file="MemoryBufferTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class MemoryBufferTests
    {
        private const string TestData = "This is a test, it is only a test...";
        private const string TestFileName = "test.txt";

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText( TestFileName, TestData, Encoding.ASCII );
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete( TestFileName );
        }

        [TestMethod]
        public void Size_Provides_Correct_length( )
        {
            var buffer = new MemoryBuffer( TestFileName );
            Assert.IsNotNull( buffer );

            byte[ ] result = buffer.ToArray( );
            Assert.IsNotNull( result );

            byte[ ] expected = Encoding.ASCII.GetBytes( TestData );
            Assert.AreEqual( expected.Length, result.Length );
        }

        [TestMethod]
        public void ToArray_Provides_ArrayOfBuffer_Data( )
        {
            var buffer = new MemoryBuffer( TestFileName );
            Assert.IsNotNull( buffer );

            byte[ ] result = buffer.ToArray( );
            Assert.IsNotNull( result );

            byte[ ] expected = Encoding.ASCII.GetBytes( TestData );
            Assert.AreEqual( expected.Length, result.Length );

            for( int i = 0; i < result.Length; ++i )
            {
                Assert.AreEqual( expected[ i ], result[ i ] );
            }
        }

        [TestMethod]
        public void Slice_Provides_a_FullView_of_the_Data( )
        {
            var buffer = new MemoryBuffer( TestFileName );
            Assert.IsNotNull( buffer );

            ReadOnlySpan<byte> result = buffer.Slice( );

            byte[ ] expected = Encoding.ASCII.GetBytes( TestData );
            Assert.IsTrue( expected.AsSpan( ).SequenceEqual( result ) );
        }

        [TestMethod]
        public void Slice_Provides_a_Partial_View_of_the_Data( )
        {
            var buffer = new MemoryBuffer( TestFileName );
            Assert.IsNotNull( buffer );

            ReadOnlySpan<byte> result = buffer.Slice( 5, 2 );

            byte[ ] expected = Encoding.ASCII.GetBytes( TestData );
            Assert.IsTrue( expected.AsSpan( 5, 2 ).SequenceEqual( result ) );
        }
    }
}

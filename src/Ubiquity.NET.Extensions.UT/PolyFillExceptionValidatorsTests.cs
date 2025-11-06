// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// .NET 7 added the various exception static methods for parameter validation
// This will back fill them for earlier versions.
//
// NOTE: C #14 extension keyword support is required to make this work.
#if !NET7_0_OR_GREATER

namespace Ubiquity.NET.Extensions.UT
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class PolyFillExceptionValidatorsTests
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_throws_if_null_or_whitespace( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace( null ));
            Assert.AreEqual( "null", ex.ParamName, "Compiler should provide expression as name" );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace( null, "self" ) );
            Assert.AreEqual( "self", ex.ParamName, "explicit name should override compiler" );

            var argEx = Assert.ThrowsExactly<ArgumentException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace(" \t "));
            Assert.AreEqual( "\" \\t \"", argEx.ParamName, "Compiler should provide expression as name" );

            argEx = Assert.ThrowsExactly<ArgumentException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace( " \t ", "self" ) );
            Assert.AreEqual( "self", argEx.ParamName, "explicit name should override compiler" );

            argEx = Assert.ThrowsExactly<ArgumentException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace( string.Empty ) );
            Assert.AreEqual( "string.Empty", argEx.ParamName, "Compiler should provide expression as name" );

            argEx = Assert.ThrowsExactly<ArgumentException>( ( ) => PolyFillExceptionValidators.ThrowIfNullOrWhiteSpace( string.Empty, "self" ) );
            Assert.AreEqual( "self", argEx.ParamName, "explicit name should override compiler" );
        }

        [TestMethod]
        public void ThrowIfNull_throws_if_null( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillExceptionValidators.ThrowIfNull( null ));
            Assert.AreEqual( "null", ex.ParamName, "Compiler should provide expression as name" );

            ex = Assert.ThrowsExactly<ArgumentNullException>( ( ) => PolyFillExceptionValidators.ThrowIfNull( null, "self" ) );
            Assert.AreEqual( "self", ex.ParamName, "explicit name should override compiler" );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void ThrowIf_throws_as_expected( )
        {
            object instance = new();

            var ex = Assert.ThrowsExactly<ObjectDisposedException>( ( ) => PolyFillExceptionValidators.ThrowIf(true, instance));
            Assert.AreEqual( "System.Object", ex.ObjectName );

            // should not throw
            PolyFillExceptionValidators.ThrowIf( false, instance );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfEqual_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfEqual(1, 1));
            Assert.AreEqual( "1", ex.ParamName );
            Assert.AreEqual( 1, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfEqual( 1, 0 );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfNotEqual_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfNotEqual(1, 0));
            Assert.AreEqual( "1", ex.ParamName );
            Assert.AreEqual( 1, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfNotEqual( 1, 1 );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfGreaterThan_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfGreaterThan(1, 0));
            Assert.AreEqual( "1", ex.ParamName );
            Assert.AreEqual( 1, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfGreaterThan( 0, 1 );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfGreaterThanOrEqual_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfGreaterThanOrEqual(1, 1));
            Assert.AreEqual( "1", ex.ParamName );
            Assert.AreEqual( 1, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfGreaterThanOrEqual( 0, 1 );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfLessThan_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfLessThan(0, 1));
            Assert.AreEqual( "0", ex.ParamName );
            Assert.AreEqual( 0, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfLessThan( 1, 0 );
        }

        [TestMethod]
        public void ArgumentOutOfRangeExcpetion_ThrowIfLessThanOrEqual_operates_as_expected( )
        {
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>( ( ) => PolyFillExceptionValidators.ThrowIfLessThanOrEqual(1, 1));
            Assert.AreEqual( "1", ex.ParamName );
            Assert.AreEqual( 1, ex.ActualValue );

            // should not throw
            PolyFillExceptionValidators.ThrowIfLessThanOrEqual( 1, 0 );
        }
    }
}
#endif

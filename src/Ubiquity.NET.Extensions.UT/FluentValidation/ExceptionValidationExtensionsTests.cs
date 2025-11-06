// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Extensions.FluentValidation;

namespace Ubiquity.NET.Extensions.UT.FluentValidation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class ExceptionValidationExtensionsTests
    {
        [TestMethod]
        public void ThrowIfNull_throws_expected_exception_when_null( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                ExceptionValidationExtensions.ThrowIfNull<string>( null );
            } );
            Assert.AreEqual("null", ex.ParamName, "parameter name should match input expression");
        }

        [TestMethod]
        public void ThrowIfNull_does_not_throw_on_non_null_input()
        {
            const string input = "This is a test";

            Assert.AreSame(input, ExceptionValidationExtensions.ThrowIfNull(input), "Fluent API should return input value on success" );
        }

        [TestMethod]
        public void ThrowIfNull_reports_exception_whith_provided_expression( )
        {
            const string exp = "My-Expression";
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                ExceptionValidationExtensions.ThrowIfNull<string>( null, exp );
            } );
            Assert.AreEqual( exp, ex.ParamName, "parameter name should match input expression" );
        }

        [TestMethod]
        public void ThrowIfNotDefined_does_not_throw_for_defined_value()
        {
            Assert.AreEqual(TestEnum.Max, ExceptionValidationExtensions.ThrowIfNotDefined(TestEnum.Max), "Fluent API should return input value on success" );
        }

        [TestMethod]
        public void ThrowIfOutOfRange_does_not_throw_for_inrange_values( )
        {
            double value = 1.0;
            double min = 0.0;
            double max = 2.0;

            Assert.AreEqual(value, ExceptionValidationExtensions.ThrowIfOutOfRange(value, min, max), "Fluent API should return input value on success");
        }

        [TestMethod]
        public void ThrowIfOutOfRange_throws_for_out_of_range_values( )
        {
            double value = 2.0;
            double min = 1.0;
            double max = 1.5;

            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(()=>
            {
                _ = ExceptionValidationExtensions.ThrowIfOutOfRange( value, min, max );
            } );
            Assert.AreEqual(value, ex.ActualValue);
            Assert.AreEqual(nameof(value), ex.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_throws_with_custom_expression_for_out_of_range_values( )
        {
            double value = 2.0;
            double min = 1.0;
            double max = 1.5;

            const string exp = "My Expression";
            var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(()=>
            {
                _ = ExceptionValidationExtensions.ThrowIfOutOfRange( value, min, max, exp );
            } );
            Assert.AreEqual( value, ex.ActualValue );
            Assert.AreEqual( exp, ex.ParamName );
        }

        [TestMethod]
        public void ThrowIfNotDefined_throws_for_undefined_values( )
        {
            var temp = (TestEnum)4;
            var ex = Assert.ThrowsExactly<InvalidEnumArgumentException>( ( ) =>
            {
                ExceptionValidationExtensions.ThrowIfNotDefined(temp);
            } );
            Assert.AreEqual(nameof(temp), ex.ParamName, "parameter name should match input expression" );

            var temp2 = (TestByteEnum)4;
            var ex2 = Assert.ThrowsExactly<InvalidEnumArgumentException>( ( ) =>
            {
                ExceptionValidationExtensions.ThrowIfNotDefined(temp2);
            } );
            Assert.AreEqual( nameof( temp2 ), ex2.ParamName, "parameter name should match input expression" );

            // This still fits an int so the normal constructor that sets paramName is available
            var temp3 = (TestU64Enum)int.MaxValue;
            var ex3 = Assert.ThrowsExactly<InvalidEnumArgumentException>( ( ) =>
            {
                ExceptionValidationExtensions.ThrowIfNotDefined(temp3);
            } );
            Assert.AreEqual( nameof( temp3 ), ex3.ParamName, "parameter name should match input expression" );

            // This can't fit into an int so, the exception constructor that does not provide paramName is
            // the only option :( [But at least this scenario is VERY rare in the real world]
            var temp4 = (TestU64Enum)(UInt64.MaxValue - 1);
            var ex4 = Assert.ThrowsExactly<InvalidEnumArgumentException>( ( ) =>
            {
                ExceptionValidationExtensions.ThrowIfNotDefined(temp4);
            } );
            Assert.IsNull( ex4.ParamName, "parameter name not available for non-int formattable enums" );
        }

        private enum TestEnum // default underling type is Int32
        {
            Zero,
            One,
            Two,
            Max = int.MaxValue
        }

        private enum TestByteEnum
            : byte
        {
            Zero,
            One,
            Two,
            Max = byte.MaxValue
        }

        private enum TestU64Enum
            : UInt64
        {
            Zero,
            One,
            Two,
            Max = UInt64.MaxValue
        }
    }
}

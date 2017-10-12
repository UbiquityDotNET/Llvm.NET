// <copyright file="ExpectedArgumentException.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    /// <summary>Attribute to mark a method as expecting an argument exception with optional parameter name and exception message validation</summary>
    public sealed class ExpectedArgumentExceptionAttribute
        : ExpectedExceptionBaseAttribute
    {
        public ExpectedArgumentExceptionAttribute( string expectedName )
            : this( expectedName, string.Empty )
        {
        }

        public ExpectedArgumentExceptionAttribute( string expectedName, string noExceptionMessage )
            : base( noExceptionMessage )
        {
            ExpectedName = expectedName;
            WrongExceptionMessage = DefautWrongExceptionMessage;
        }

        public string WrongExceptionMessage { get; set; }

        public string ExpectedExceptionMessage { get; set; }

        protected override void Verify( Exception exception )
        {
            Assert.IsNotNull(exception);

            // Handle assertion exceptions from assertion failures in the test method, since we are not interested in verifying those
            RethrowIfAssertException(exception);

            Assert.IsInstanceOfType(exception, typeof(ArgumentException), WrongExceptionMessage );
            Assert.AreEqual( ExpectedName, ( ( ArgumentException )exception ).ParamName );
            if( !string.IsNullOrWhiteSpace( ExpectedExceptionMessage ) )
            {
                Assert.AreEqual( $"{ExpectedExceptionMessage}\r\nParameter name: {ExpectedName}"
                               , exception.Message
                               , "Could not verify the exception message."
                               );
            }
        }

       private const string DefautWrongExceptionMessage = "The exception that was thrown does not derive from System.ArgumentException.";
       private readonly string ExpectedName;
    }
}

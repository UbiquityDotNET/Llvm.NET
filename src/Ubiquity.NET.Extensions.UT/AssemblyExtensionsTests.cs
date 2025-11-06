// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Reflection;

namespace Ubiquity.NET.Extensions.UT
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class AssemblyExtensionsTests
    {
        [TestMethod]
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public void GetInformationalVersion_with_null_throws( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>( ()=> _ = AssemblyExtensions.GetInformationalVersion( null ) );
            Assert.AreEqual("null", ex.ParamName, "CallerExpressionArgumentAttribute should provide the expression used");

            ex = Assert.ThrowsExactly<ArgumentNullException>( ()=> _ = AssemblyExtensions.GetInformationalVersion( null, "self" ) );
            Assert.AreEqual( "self", ex.ParamName, "explicit value for CallerExpressionArgumentAttribute should override it" );
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        public void GetInformationalVersion_succeeds( )
        {
            var thisAsm = typeof( AssemblyExtensionsTests ).Assembly;

            var assemblyVersionAttribute = thisAsm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            string expected = assemblyVersionAttribute is not null
                            ? assemblyVersionAttribute.InformationalVersion
                            : thisAsm.GetName().Version?.ToString() ?? string.Empty;

            string actual = thisAsm.GetInformationalVersion();
            Assert.AreEqual( expected, actual );
        }
    }
}

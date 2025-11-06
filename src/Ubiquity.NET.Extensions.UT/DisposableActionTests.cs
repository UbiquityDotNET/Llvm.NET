// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Extensions.UT
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class DisposableActionTests
    {
        [TestMethod]
        public void DisposableAction_CreateNOP_succeeds( )
        {
            using var disp = DisposableAction.CreateNOP();
            Assert.IsNotNull(disp);
        }

        [TestMethod]
        public void DisposableAction_with_null_Action_throws( )
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(static ()=>
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // Testing explicit case of null param.
                using var x = new DisposableAction(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            } );
            Assert.IsNotNull( ex );
            Assert.AreEqual("null", ex.ParamName);
        }

        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP017:Prefer using", Justification = "Explicit testing" )]
        public void DisposableAction_called_correctly()
        {
            bool actionCalled = false;

            var raiiAction = new DisposableAction( ()=> actionCalled = true );
            raiiAction.Dispose(); // Should trigger call to action

            Assert.IsTrue(actionCalled);
        }
    }
}

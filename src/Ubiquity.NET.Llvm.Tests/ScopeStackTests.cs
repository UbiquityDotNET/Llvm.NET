// -----------------------------------------------------------------------
// <copyright file="ScopeStackTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Runtime.Utils;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class ScopeStackTests
    {
        [TestMethod]
        public void New_Stack_has_Depth_of_1( )
        {
            var stack = new ScopeStack<int>( );
            Assert.AreEqual( 1, stack.Depth );
        }

        [TestMethod]
        public void EnterScope_On_New_Stack_has_Depth_of_2( )
        {
            var stack = new ScopeStack<int>( );
            var result = stack.EnterScope( );

            Assert.IsNotNull( result );
            using( result )
            {
                Assert.AreEqual( 2, stack.Depth );
            }

            Assert.AreEqual( 1, stack.Depth );
        }

        [TestMethod]
        public void TryGetValue_Nested_Scope_Value_Overrides_Parent( )
        {
            const string symbolName = "Global1";

            var stack = new ScopeStack<int> { [ symbolName ] = 1 };

            using( stack.EnterScope( ) )
            {
                stack[ symbolName ] = 2;
                Assert.IsTrue( stack.TryGetValue( symbolName, out int value ) );
                Assert.AreEqual( 2, value );

                using( stack.EnterScope( ) )
                {
                    stack[ symbolName ] = 3;
                    Assert.IsTrue( stack.TryGetValue( symbolName, out int nestedValue ) );
                    Assert.AreEqual( 3, nestedValue );
                }

                Assert.IsTrue( stack.TryGetValue( symbolName, out int valueAfterExitNested ) );
                Assert.AreEqual( 2, valueAfterExitNested );
            }

            Assert.IsTrue( stack.TryGetValue( symbolName, out int globalValue ) );
            Assert.AreEqual( 1, globalValue );
        }
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Extensions.UT
{
    [TestClass]
    public sealed class KvpArrayBuilderTests
    {
        [TestMethod]
        public void Inintialization_of_KvpArray_is_successfull( )
        {
            ImmutableArray<KeyValuePair<string, int>> testArray = new KvpArrayBuilder<string, int>()
            {
                ["one"] = 1,
                ["two"] = 2,
            }.ToImmutable();

            Assert.AreEqual( "one", testArray[ 0 ].Key);
            Assert.AreEqual( 1, testArray[ 0 ].Value );

            Assert.AreEqual( "two", testArray[ 1 ].Key );
            Assert.AreEqual( 2, testArray[ 1 ].Value );
        }

        [TestMethod]
        public void Getting_enumerator_from_KvpArrayBuilder_throws( )
        {
            var testBuilder = new KvpArrayBuilder<string, int>()
            {
                ["one"] = 1,
                ["two"] = 2,
            };

            Assert.ThrowsExactly<NotImplementedException>(()=>
            {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                // NOT disposable, no idea where the analyzer gets this from but System.Collections.IEnumerator
                // does not implement IDisposable. [Methods is supposed to throw anyway]
                _ = testBuilder.GetEnumerator();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
            }
            , "Syntactic sugar only for initialization");
        }
    }
}

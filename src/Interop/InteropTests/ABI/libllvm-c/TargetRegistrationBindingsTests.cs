// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Versioning;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetRegistrationBindings;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class TargetRegistrationBindingsTests
    {
        [TestMethod]
        [Ignore("Not yet implemented")]
        public void LibLLVMRegisterTargetTest( )
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void LibLLVMGetNumTargetsTest( )
        {
            Int32 numTargets = LibLLVMGetNumTargets();

            // -3 to account for declared "None, Native, and ALL" values
            // not included in native methods.
            int expectedTargets = Enum.GetValues<LibLLVMCodeGenTarget>().Length - 3;
            Assert.AreEqual( expectedTargets, numTargets );
        }

        [TestMethod]
        public void LibLLVMGetRuntimeTargetsTest( )
        {
            var targets = new LibLLVMCodeGenTarget[LibLLVMGetNumTargets()];
            using var errorRef = LibLLVMGetRuntimeTargets( targets );
            errorRef.ThrowIfFailed();

            // Test supported targets; This is an extension as at one point the
            // set of supported targets was ONLY the native target and that of one
            // additional target. That's not true anymore, so this should verify
            // that ALL (non-experimental) targets are an option.
            var nativeTargets = from id in Enum.GetValues<LibLLVMCodeGenTarget>()
                                where id != LibLLVMCodeGenTarget.CodeGenTarget_None
                                   && id != LibLLVMCodeGenTarget.CodeGenTarget_Native
                                   && id != LibLLVMCodeGenTarget.CodeGenTarget_All
                                select id;

            foreach(var v in nativeTargets)
            {
                Assert.IsTrue( targets.Contains( v ), $"Target: {v} should be supported" );
            }
        }

        [TestMethod]
        public void LibLLVMGetVersionTest( )
        {
            var ver = SemVer.Parse(LibLLVMGetVersion()?.ToString() ?? string.Empty, SemVerFormatProvider.CaseInsensitive);
            Assert.IsTrue( ver is SemVer or CSemVer);
            Assert.AreEqual( 20, ver.Major );
            Assert.AreEqual( 1, ver.Minor );

            // Testing for an exact match of the patch level (or anything finer grained than that)
            // in an automated test is dubious as it would require updating the tests on effectively
            // EVERY build of the native library... So, this tests for a minimum value that was valid
            // at the time of creation. The above major/minor values should be updated on changes to
            // those assumptions as any number of things may have changed.
            Assert.IsTrue( ver.Patch >= 7);
        }
    }
}

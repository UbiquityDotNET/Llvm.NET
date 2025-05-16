using System;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop.UT;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetRegistrationBindings;

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT
{
    [TestClass]
    public class TargetRegistrationBindingsTests
    {
        [SkipTestMethod]
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
            Assert.AreEqual(expectedTargets, numTargets);
        }

        [TestMethod]
        public void LibLLVMGetRuntimeTargetsTest( )
        {
            var targets = new LibLLVMCodeGenTarget[LibLLVMGetNumTargets()];
            LibLLVMGetRuntimeTargets( targets ).ThrowIfFailed();

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
                Assert.IsTrue(targets.Contains(v), $"Target: {v} should be supported");
            }
        }

        [TestMethod]
        public void LibLLVMGetVersionTest( )
        {
            UInt64 ver = LibLLVMGetVersion();
            Assert.IsTrue( ver > 0);
            var csemVer  = CSemVer.FromUInt64(ver);
            Assert.AreEqual(20u, csemVer.Major);
            Assert.AreEqual(1u, csemVer.Minor);

            // Testing for an exact match of the patch level (or anything finer grained than that)
            // in an automated test is dubious as it would require updating the tests on effectively
            // EVERY build of the native library... So, this tests for a minimum value that was valid
            // at the time of creation. The above major/minor values should be updated on changes to
            // those assumptions as any number of things may have changed.
            Assert.IsTrue(4u <= csemVer.Patch);
        }
    }
}

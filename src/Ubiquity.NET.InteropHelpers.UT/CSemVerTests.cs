using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.InteropHelpers.UT
{
    internal readonly record struct Version64Details(UInt64 FileVersion, UInt64 OrderedVersion, bool IsCIBuild);

    [TestClass]
    public class CSemVerTests
    {
        [TestMethod]
        public void ConversionFromFileVersionTests( )
        {
            // From https://csemver.org/playground/site/#/
            // parts for v20.1.5.alpha
            var x = MakeVersion64(5, 44854, 3878, 23340);
            // Sanity check the assumptions made for this test
            // against what is known from official site.
            Assert.AreEqual(800010800410006ul, x.OrderedVersion);
            Assert.IsFalse(x.IsCIBuild);

            var semVer = CSemVer.FromUInt64(x.FileVersion);
            Assert.IsFalse(semVer.IsCIBuild);
            Assert.IsTrue(semVer.PreRelease.HasValue, "Expected a pre-release");
            Assert.AreEqual(20ul, semVer.Major);
            Assert.AreEqual(1ul, semVer.Minor);
            Assert.AreEqual(5ul, semVer.Patch);
            Assert.AreEqual(0, semVer.PreRelease.Value.Index); // alpha
            Assert.AreEqual(0, semVer.PreRelease.Value.Number);
            Assert.AreEqual(0, semVer.PreRelease.Value.Fix);

            // Revision is odd (+1), all other values the same
            var xCI = MakeVersion64(5, 44854, 3878, 23341);
            // Sanity check the assumptions made for this test
            // against what is known from official site.
            Assert.AreEqual(800010800410006ul, xCI.OrderedVersion);
            Assert.IsTrue(xCI.IsCIBuild);

            var semVerCI = CSemVer.FromUInt64(xCI.FileVersion);
            Assert.IsTrue(semVerCI.IsCIBuild);
            Assert.IsTrue(semVerCI.PreRelease.HasValue, "Expected a pre-release");
            Assert.AreEqual(20ul, semVerCI.Major);
            Assert.AreEqual(1ul, semVerCI.Minor);
            Assert.AreEqual(5ul, semVerCI.Patch);
            Assert.AreEqual(0, semVerCI.PreRelease.Value.Index); // alpha
            Assert.AreEqual(0, semVerCI.PreRelease.Value.Number);
            Assert.AreEqual(0, semVerCI.PreRelease.Value.Fix);
        }

        private static Version64Details MakeVersion64(UInt16 major, UInt16 minor, UInt16 build, UInt16 revision)
        {
            bool isCiBuild = ( revision & 1 ) == 1;

            UInt64 fileVersion = ((UInt64)major << 48)
                               + ((UInt64)minor << 32)
                               + ((UInt64)build << 16)
                               + ((UInt64)revision << 0);
            UInt64 orderedVersion = fileVersion >> 1;
            return new(fileVersion, orderedVersion, isCiBuild);
        }
    }
}

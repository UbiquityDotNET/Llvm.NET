// -----------------------------------------------------------------------
// <copyright file="TripleTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class TripleTests
    {
        [TestMethod]
        public void TripleTest( )
        {
            // nonsensical, but syntactically valid triple
            using var triple = new Triple( "thumbv7m-pc-cuda-eabicoff" );
            Assert.AreEqual( ArchKind.Thumb, triple.ArchitectureType );
            Assert.AreEqual( SubArchKind.ARMSubArch_v7m, triple.SubArchitecture );
            Assert.AreEqual( VendorKind.PC, triple.Vendor );
            Assert.AreEqual( OSKind.CUDA, triple.OS );
            Assert.AreEqual( EnvironmentKind.EABI, triple.Environment );
            Assert.AreEqual( ObjectFormatKind.COFF, triple.ObjectFormat );
        }

        [TestMethod]
        public void ToStringTest( )
        {
            // constructor should parse and normalize the triple
            // so that ToString() retrieves the full normalized form
            using var triple = new Triple( "thumbv7m-none-eabi" );
            string? str = triple.ToString( );
            Assert.IsNotNull( str );
            Assert.AreEqual( "thumbv7m-unknown-none-eabi", str );
        }

        [TestMethod]
        public void OpEqualsTest( )
        {
            // constructor should parse and normalize the triple
            // so that ToString() retrieves the normalized form
            using var triple = new Triple( "thumbv7m-eabi" );
            using var otherTriple = new Triple( "thumbv7m---eabi" );
            Assert.AreEqual( otherTriple, triple );
            using var notEqualTriple = new Triple( "thumbv7m-eabicoff" );
            Assert.AreNotEqual( triple, notEqualTriple );
        }

        [TestMethod]
        public void GetArchTypeNameTest( )
        {
            var values = new Dictionary<ArchKind, string>
                {
                  { ArchKind.UnknownArch,    "unknown" },
                  { ArchKind.Aarch64,        "aarch64" },
                  { ArchKind.Aarch64BE,      "aarch64_be" },
                  { ArchKind.Arm,            "arm" },
                  { ArchKind.Armeb,          "armeb" },
                  { ArchKind.Arc,            "arc" },
                  { ArchKind.Avr,            "avr" },
                  { ArchKind.BPFel,          "bpfel" },
                  { ArchKind.BPFeb,          "bpfeb" },
                  { ArchKind.Hexagon,        "hexagon" },
                  { ArchKind.MIPS,           "mips" },
                  { ArchKind.MIPSel,         "mipsel" },
                  { ArchKind.MIPS64,         "mips64" },
                  { ArchKind.MIPS64el,       "mips64el" },
                  { ArchKind.MSP430,         "msp430" },
                  { ArchKind.PPC64,          "powerpc64" },
                  { ArchKind.PPC64le,        "powerpc64le" },
                  { ArchKind.PPC,            "powerpc" },
                  { ArchKind.R600,           "r600" },
                  { ArchKind.AMDGCN,         "amdgcn" },
                  { ArchKind.RiscV32,        "riscv32" },
                  { ArchKind.RiscV64,        "riscv64" },
                  { ArchKind.Sparc,          "sparc" },
                  { ArchKind.Sparcv9,        "sparcv9" },
                  { ArchKind.Sparcel,        "sparcel" },
                  { ArchKind.SystemZ,        "s390x" },
                  { ArchKind.TCE,            "tce" },
                  { ArchKind.Thumb,          "thumb" },
                  { ArchKind.Thumbeb,        "thumbeb" },
                  { ArchKind.X86,            "i386" },
                  { ArchKind.Amd64,          "x86_64" },
                  { ArchKind.Xcore,          "xcore" },
                  { ArchKind.Nvptx,          "nvptx" },
                  { ArchKind.Nvptx64,        "nvptx64" },
                  { ArchKind.Amdil,          "amdil" },
                  { ArchKind.Amdil64,        "amdil64" },
                  { ArchKind.Hsail,          "hsail" },
                  { ArchKind.Hsail64,        "hsail64" },
                  { ArchKind.Spir,           "spir" },
                  { ArchKind.Spir64,         "spir64" },
                  { ArchKind.Kalimba,        "kalimba" },
                  { ArchKind.Lanai,          "lanai" },
                  { ArchKind.Shave,          "shave" },
                  { ArchKind.Wasm32,         "wasm32" },
                  { ArchKind.Wasm64,         "wasm64" },
                  { ArchKind.RenderScript32, "renderscript32" },
                  { ArchKind.RenderScript64, "renderscript64" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ ArchKind.UnknownArch ], Triple.GetCanonicalName( ( ArchKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetVendorTypeNameTest( )
        {
            var values = new Dictionary<VendorKind, string>
            {
                { VendorKind.Unknown,                 "unknown" },
                { VendorKind.Apple,                   "apple" },
                { VendorKind.PC,                      "pc" },
                { VendorKind.SCEI,                    "scei" },
                { VendorKind.Freescale,               "fsl" },
                { VendorKind.IBM,                     "ibm" },
                { VendorKind.ImaginationTechnologies, "img" },
                { VendorKind.MipsTechnologies,        "mti" },
                { VendorKind.NVIDIA,                  "nvidia" },
                { VendorKind.CSR,                     "csr" },
                { VendorKind.AMD,                     "amd" },
                { VendorKind.Mesa,                    "mesa" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ VendorKind.Unknown ], Triple.GetCanonicalName( ( VendorKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetOsTypeNameTest( )
        {
            var values = new Dictionary<OSKind, string>
            {
                { OSKind.UnknownOS, "unknown" },
                { OSKind.Darwin,    "darwin" },
                { OSKind.DragonFly, "dragonfly" },
                { OSKind.FreeBSD,   "freebsd" },
                { OSKind.Fuchsia,   "fuchsia" },
                { OSKind.IOS,       "ios" },
                { OSKind.KFreeBSD,  "kfreebsd" },
                { OSKind.Linux,     "linux" },
                { OSKind.Lv2,       "lv2" },
                { OSKind.MacOSX,    "macosx" },
                { OSKind.NetBSD,    "netbsd" },
                { OSKind.OpenBSD,   "openbsd" },
                { OSKind.Solaris,   "solaris" },
                { OSKind.Win32,     "windows" },
                { OSKind.Haiku,     "haiku" },
                { OSKind.RTEMS,     "rtems" },
                { OSKind.NaCl,      "nacl" },
                { OSKind.AIX,       "aix" },
                { OSKind.CUDA,      "cuda" },
                { OSKind.NVCL,      "nvcl" },
                { OSKind.AMDHSA,    "amdhsa" },
                { OSKind.PS4,       "ps4" },
                { OSKind.ELFIAMCU,  "elfiamcu" },
                { OSKind.TvOS,      "tvos" },
                { OSKind.WatchOS,   "watchos" },
                { OSKind.Mesa3D,    "mesa3d" },
                { OSKind.AmdPAL,    "amdpal" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ OSKind.UnknownOS ], Triple.GetCanonicalName( ( OSKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetEnvironmentTypeNameTest( )
        {
            var values = new Dictionary<EnvironmentKind, string>
            {
                { EnvironmentKind.Unknown,    "unknown" },
                { EnvironmentKind.GNU,        "gnu" },
                { EnvironmentKind.GNUABIN32,  "gnuabin32" },
                { EnvironmentKind.GNUABI64,   "gnuabi64" },
                { EnvironmentKind.GNUEABIHF,  "gnueabihf" },
                { EnvironmentKind.GNUEABI,    "gnueabi" },
                { EnvironmentKind.GNUX32,     "gnux32" },
                { EnvironmentKind.CODE16,     "code16" },
                { EnvironmentKind.EABI,       "eabi" },
                { EnvironmentKind.EABIHF,     "eabihf" },
                { EnvironmentKind.Android,    "android" },
                { EnvironmentKind.Musl,       "musl" },
                { EnvironmentKind.MuslEABI,   "musleabi" },
                { EnvironmentKind.MuslEABIHF, "musleabihf" },
                { EnvironmentKind.MSVC,       "msvc" },
                { EnvironmentKind.Itanium,    "itanium" },
                { EnvironmentKind.Cygnus,     "cygnus" },
                { EnvironmentKind.CoreCLR,    "coreclr" },
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ EnvironmentKind.Unknown ], Triple.GetCanonicalName( ( EnvironmentKind )0x12345678 ) );
        }
    }
}

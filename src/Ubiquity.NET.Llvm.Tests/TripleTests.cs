// -----------------------------------------------------------------------
// <copyright file="TripleTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop;

/* ReSharper disable StringLiteralTypo */
namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class TripleTests
    {
        [TestMethod]
        public void TripleTest( )
        {
            // nonsensical, but syntactically valid triple
            var triple = new Triple( "thumbv7m-pc-cuda-eabicoff" );
            Assert.AreEqual( Triple.ArchKind.Thumb, triple.ArchitectureType );
            Assert.AreEqual( Triple.SubArchKind.ARMSubArch_v7m, triple.SubArchitecture );
            Assert.AreEqual( Triple.VendorKind.PC, triple.Vendor );
            Assert.AreEqual( Triple.OSKind.CUDA, triple.OS );
            Assert.AreEqual( Triple.EnvironmentKind.EABI, triple.Environment );
            Assert.AreEqual( Triple.ObjectFormatKind.COFF, triple.ObjectFormat );
        }

        [TestMethod]
        public void ToStringTest( )
        {
            // constructor should parse and normalize the triple
            // so that ToString() retrieves the full normalized form
            var triple = new Triple( "thumbv7m-none-eabi" );
            string str = triple.ToString( );
            Assert.AreEqual( "thumbv7m-unknown-none-eabi", str );
        }

        [TestMethod]
        public void OpEqualsTest( )
        {
            // constructor should parse and normalize the triple
            // so that ToString() retrieves the normalized form
            var triple = new Triple( "thumbv7m-eabi" );
            var otherTriple = new Triple( "thumbv7m---eabi" );
            Assert.AreEqual( otherTriple, triple );
            var notEqualTriple = new Triple( "thumbv7m-eabicoff" );
            Assert.AreNotEqual( triple, notEqualTriple );
        }

        [TestMethod]
        public void GetArchTypeNameTest( )
        {
            var values = new Dictionary<Triple.ArchKind, string>
                {
                  { Triple.ArchKind.UnknownArch,    "unknown" },
                  { Triple.ArchKind.Aarch64,        "aarch64" },
                  { Triple.ArchKind.Aarch64BE,      "aarch64_be" },
                  { Triple.ArchKind.Arm,            "arm" },
                  { Triple.ArchKind.Armeb,          "armeb" },
                  { Triple.ArchKind.Arc,            "arc" },
                  { Triple.ArchKind.Avr,            "avr" },
                  { Triple.ArchKind.BPFel,          "bpfel" },
                  { Triple.ArchKind.BPFeb,          "bpfeb" },
                  { Triple.ArchKind.Hexagon,        "hexagon" },
                  { Triple.ArchKind.MIPS,           "mips" },
                  { Triple.ArchKind.MIPSel,         "mipsel" },
                  { Triple.ArchKind.MIPS64,         "mips64" },
                  { Triple.ArchKind.MIPS64el,       "mips64el" },
                  { Triple.ArchKind.MSP430,         "msp430" },
                  { Triple.ArchKind.PPC64,          "powerpc64" },
                  { Triple.ArchKind.PPC64le,        "powerpc64le" },
                  { Triple.ArchKind.PPC,            "powerpc" },
                  { Triple.ArchKind.R600,           "r600" },
                  { Triple.ArchKind.AMDGCN,         "amdgcn" },
                  { Triple.ArchKind.RiscV32,        "riscv32" },
                  { Triple.ArchKind.RiscV64,        "riscv64" },
                  { Triple.ArchKind.Sparc,          "sparc" },
                  { Triple.ArchKind.Sparcv9,        "sparcv9" },
                  { Triple.ArchKind.Sparcel,        "sparcel" },
                  { Triple.ArchKind.SystemZ,        "s390x" },
                  { Triple.ArchKind.TCE,            "tce" },
                  { Triple.ArchKind.Thumb,          "thumb" },
                  { Triple.ArchKind.Thumbeb,        "thumbeb" },
                  { Triple.ArchKind.X86,            "i386" },
                  { Triple.ArchKind.Amd64,          "x86_64" },
                  { Triple.ArchKind.Xcore,          "xcore" },
                  { Triple.ArchKind.Nvptx,          "nvptx" },
                  { Triple.ArchKind.Nvptx64,        "nvptx64" },
                  { Triple.ArchKind.Amdil,          "amdil" },
                  { Triple.ArchKind.Amdil64,        "amdil64" },
                  { Triple.ArchKind.Hsail,          "hsail" },
                  { Triple.ArchKind.Hsail64,        "hsail64" },
                  { Triple.ArchKind.Spir,           "spir" },
                  { Triple.ArchKind.Spir64,         "spir64" },
                  { Triple.ArchKind.Kalimba,        "kalimba" },
                  { Triple.ArchKind.Lanai,          "lanai" },
                  { Triple.ArchKind.Shave,          "shave" },
                  { Triple.ArchKind.Wasm32,         "wasm32" },
                  { Triple.ArchKind.Wasm64,         "wasm64" },
                  { Triple.ArchKind.RenderScript32, "renderscript32" },
                  { Triple.ArchKind.RenderScript64, "renderscript64" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.ArchKind.UnknownArch ], Triple.GetCanonicalName( ( Triple.ArchKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetVendorTypeNameTest( )
        {
            var values = new Dictionary<Triple.VendorKind, string>
            {
                { Triple.VendorKind.Unknown,                 "unknown" },
                { Triple.VendorKind.Apple,                   "apple" },
                { Triple.VendorKind.PC,                      "pc" },
                { Triple.VendorKind.SCEI,                    "scei" },
                { Triple.VendorKind.Freescale,               "fsl" },
                { Triple.VendorKind.IBM,                     "ibm" },
                { Triple.VendorKind.ImaginationTechnologies, "img" },
                { Triple.VendorKind.MipsTechnologies,        "mti" },
                { Triple.VendorKind.NVIDIA,                  "nvidia" },
                { Triple.VendorKind.CSR,                     "csr" },
                { Triple.VendorKind.AMD,                     "amd" },
                { Triple.VendorKind.Mesa,                    "mesa" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.VendorKind.Unknown ], Triple.GetCanonicalName( ( Triple.VendorKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetOsTypeNameTest( )
        {
            var values = new Dictionary<Triple.OSKind, string>
            {
                { Triple.OSKind.UnknownOS, "unknown" },
                { Triple.OSKind.Darwin,    "darwin" },
                { Triple.OSKind.DragonFly, "dragonfly" },
                { Triple.OSKind.FreeBSD,   "freebsd" },
                { Triple.OSKind.Fuchsia,   "fuchsia" },
                { Triple.OSKind.IOS,       "ios" },
                { Triple.OSKind.KFreeBSD,  "kfreebsd" },
                { Triple.OSKind.Linux,     "linux" },
                { Triple.OSKind.Lv2,       "lv2" },
                { Triple.OSKind.MacOSX,    "macosx" },
                { Triple.OSKind.NetBSD,    "netbsd" },
                { Triple.OSKind.OpenBSD,   "openbsd" },
                { Triple.OSKind.Solaris,   "solaris" },
                { Triple.OSKind.Win32,     "windows" },
                { Triple.OSKind.Haiku,     "haiku" },
                { Triple.OSKind.RTEMS,     "rtems" },
                { Triple.OSKind.NaCl,      "nacl" },
                { Triple.OSKind.AIX,       "aix" },
                { Triple.OSKind.CUDA,      "cuda" },
                { Triple.OSKind.NVCL,      "nvcl" },
                { Triple.OSKind.AMDHSA,    "amdhsa" },
                { Triple.OSKind.PS4,       "ps4" },
                { Triple.OSKind.ELFIAMCU,  "elfiamcu" },
                { Triple.OSKind.TvOS,      "tvos" },
                { Triple.OSKind.WatchOS,   "watchos" },
                { Triple.OSKind.Mesa3D,    "mesa3d" },
                { Triple.OSKind.AmdPAL,    "amdpal" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.OSKind.UnknownOS ], Triple.GetCanonicalName( ( Triple.OSKind )0x12345678 ) );
        }

        [TestMethod]
        public void GetEnvironmentTypeNameTest( )
        {
            var values = new Dictionary<Triple.EnvironmentKind, string>
            {
                { Triple.EnvironmentKind.Unknown,    "unknown" },
                { Triple.EnvironmentKind.GNU,        "gnu" },
                { Triple.EnvironmentKind.GNUABIN32,  "gnuabin32" },
                { Triple.EnvironmentKind.GNUABI64,   "gnuabi64" },
                { Triple.EnvironmentKind.GNUEABIHF,  "gnueabihf" },
                { Triple.EnvironmentKind.GNUEABI,    "gnueabi" },
                { Triple.EnvironmentKind.GNUX32,     "gnux32" },
                { Triple.EnvironmentKind.CODE16,     "code16" },
                { Triple.EnvironmentKind.EABI,       "eabi" },
                { Triple.EnvironmentKind.EABIHF,     "eabihf" },
                { Triple.EnvironmentKind.Android,    "android" },
                { Triple.EnvironmentKind.Musl,       "musl" },
                { Triple.EnvironmentKind.MuslEABI,   "musleabi" },
                { Triple.EnvironmentKind.MuslEABIHF, "musleabihf" },
                { Triple.EnvironmentKind.MSVC,       "msvc" },
                { Triple.EnvironmentKind.Itanium,    "itanium" },
                { Triple.EnvironmentKind.Cygnus,     "cygnus" },
                { Triple.EnvironmentKind.CoreCLR,    "coreclr" },
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.EnvironmentKind.Unknown ], Triple.GetCanonicalName( ( Triple.EnvironmentKind )0x12345678 ) );
        }
    }
}

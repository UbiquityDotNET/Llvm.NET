// <copyright file="TripleTests.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class TripleTests
    {
        [TestMethod]
        public void TripleTest( )
        {
            // nonsensical, but syntactically valid triple
            var triple = new Triple( "thumbv7m-pc-cuda-eabicoff" );
            Assert.AreEqual( Triple.ArchType.Thumb, triple.ArchitectureType );
            Assert.AreEqual( Triple.SubArchType.ARMSubArch_v7m, triple.SubArchitecture );
            Assert.AreEqual( Triple.VendorType.PC, triple.Vendor );
            Assert.AreEqual( Triple.OSType.CUDA, triple.OS );
            Assert.AreEqual( Triple.EnvironmentType.EABI, triple.Environment );
            Assert.AreEqual( Triple.ObjectFormatType.COFF, triple.ObjectFormat );
        }

        [TestMethod]
        public void ToStringTest( )
        {
            // constructor should parse and normalize the triple
            // so that ToString() retrieves the normalized form
            var triple = new Triple( "thumbv7m-none-eabi" );
            string str = triple.ToString( );
            Assert.AreEqual( "thumbv7m-none--eabi", str );
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
            var values = new Dictionary<Triple.ArchType, string>
                {
                  { Triple.ArchType.UnknownArch,    "unknown" },
                  { Triple.ArchType.Aarch64,        "aarch64" },
                  { Triple.ArchType.Aarch64_be,     "aarch64_be" },
                  { Triple.ArchType.Arm,            "arm" },
                  { Triple.ArchType.Armeb,          "armeb" },
                  { Triple.ArchType.Arc,            "arc" },
                  { Triple.ArchType.Avr,            "avr" },
                  { Triple.ArchType.BPFel,          "bpfel" },
                  { Triple.ArchType.BPFeb,          "bpfeb" },
                  { Triple.ArchType.Hexagon,        "hexagon" },
                  { Triple.ArchType.MIPS,           "mips" },
                  { Triple.ArchType.MIPSel,         "mipsel" },
                  { Triple.ArchType.MIPS64,         "mips64" },
                  { Triple.ArchType.MIPS64el,       "mips64el" },
                  { Triple.ArchType.MSP430,         "msp430" },
                  { Triple.ArchType.PPC64,          "powerpc64" },
                  { Triple.ArchType.PPC64le,        "powerpc64le" },
                  { Triple.ArchType.PPC,            "powerpc" },
                  { Triple.ArchType.R600,           "r600" },
                  { Triple.ArchType.AMDGCN,         "amdgcn" },
                  { Triple.ArchType.Sparc,          "sparc" },
                  { Triple.ArchType.Sparcv9,        "sparcv9" },
                  { Triple.ArchType.Sparcel,        "sparcel" },
                  { Triple.ArchType.SystemZ,        "s390x" },
                  { Triple.ArchType.TCE,            "tce" },
                  { Triple.ArchType.Thumb,          "thumb" },
                  { Triple.ArchType.Thumbeb,        "thumbeb" },
                  { Triple.ArchType.X86,            "i386" },
                  { Triple.ArchType.X86_64,         "x86_64" },
                  { Triple.ArchType.Xcore,          "xcore" },
                  { Triple.ArchType.Nvptx,          "nvptx" },
                  { Triple.ArchType.Nvptx64,        "nvptx64" },
                  { Triple.ArchType.Le32,           "le32" },
                  { Triple.ArchType.Le64,           "le64" },
                  { Triple.ArchType.Amdil,          "amdil" },
                  { Triple.ArchType.Amdil64,        "amdil64" },
                  { Triple.ArchType.Hsail,          "hsail" },
                  { Triple.ArchType.Hsail64,        "hsail64" },
                  { Triple.ArchType.Spir,           "spir" },
                  { Triple.ArchType.Spir64,         "spir64" },
                  { Triple.ArchType.Kalimba,        "kalimba" },
                  { Triple.ArchType.Lanai,          "lanai" },
                  { Triple.ArchType.Shave,          "shave" },
                  { Triple.ArchType.Wasm32,         "wasm32" },
                  { Triple.ArchType.Wasm64,         "wasm64" },
                  { Triple.ArchType.Renderscript32, "renderscript32" },
                  { Triple.ArchType.Renderscript64, "renderscript64" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.ArchType.UnknownArch ], Triple.GetCanonicalName( ( Triple.ArchType )0x12345678 ) );
        }

        [TestMethod]
        public void GetSubArchTypeNameTest( )
        {
            var values = new Dictionary<Triple.SubArchType, string>
                {
                    { Triple.SubArchType.NoSubArch,               string.Empty },
                    { Triple.SubArchType.ARMSubArch_v8_3a,        "v8.3a" },
                    { Triple.SubArchType.ARMSubArch_v8_2a,        "v8.2a" },
                    { Triple.SubArchType.ARMSubArch_v8_1a,        "v8.1a" },
                    { Triple.SubArchType.ARMSubArch_v8,           "v8" },
                    { Triple.SubArchType.ARMSubArch_v8r,          "v8r" },
                    { Triple.SubArchType.ARMSubArch_v8m_baseline, "v8m.base" },
                    { Triple.SubArchType.ARMSubArch_v8m_mainline, "v8m.main" },
                    { Triple.SubArchType.ARMSubArch_v7,           "v7" },
                    { Triple.SubArchType.ARMSubArch_v7em,         "v7em" },
                    { Triple.SubArchType.ARMSubArch_v7m,          "v7m" },
                    { Triple.SubArchType.ARMSubArch_v7s,          "v7s" },
                    { Triple.SubArchType.ARMSubArch_v7k,          "v7k" },
                    { Triple.SubArchType.ARMSubArch_v6,           "v6" },
                    { Triple.SubArchType.ARMSubArch_v6m,          "v6m" },
                    { Triple.SubArchType.ARMSubArch_v6k,          "v6k" },
                    { Triple.SubArchType.ARMSubArch_v6t2,         "v6t2" },
                    { Triple.SubArchType.ARMSubArch_v5,           "v5" },
                    { Triple.SubArchType.ARMSubArch_v5te,         "v5e" },
                    { Triple.SubArchType.ARMSubArch_v4t,          "v4t" },
                    { Triple.SubArchType.KalimbaSubArch_v3,       "kalimba3" },
                    { Triple.SubArchType.KalimbaSubArch_v4,       "kalimba4" },
                    { Triple.SubArchType.KalimbaSubArch_v5,       "kalimba5" }
                };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.SubArchType.NoSubArch ], Triple.GetCanonicalName( ( Triple.SubArchType )0x12345678 ) );
        }

        [TestMethod]
        public void GetVendorTypeNameTest( )
        {
            var values = new Dictionary<Triple.VendorType, string>
            {
                { Triple.VendorType.UnknownVendor,           "unknown" },
                { Triple.VendorType.Apple,                   "apple" },
                { Triple.VendorType.PC,                      "pc" },
                { Triple.VendorType.SCEI,                    "scei" },
                { Triple.VendorType.BGP,                     "bgp" },
                { Triple.VendorType.BGQ,                     "bgq" },
                { Triple.VendorType.Freescale,               "fsl" },
                { Triple.VendorType.IBM,                     "ibm" },
                { Triple.VendorType.ImaginationTechnologies, "img" },
                { Triple.VendorType.MipsTechnologies,        "mti" },
                { Triple.VendorType.NVIDIA,                  "nvidia" },
                { Triple.VendorType.CSR,                     "csr" },
                { Triple.VendorType.Myriad,                  "myriad" },
                { Triple.VendorType.AMD,                     "amd" },
                { Triple.VendorType.Mesa,                    "mesa" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.VendorType.UnknownVendor ], Triple.GetCanonicalName( ( Triple.VendorType )0x12345678 ) );
        }

        [TestMethod]
        public void GetOsTypeNameTest( )
        {
            var values = new Dictionary<Triple.OSType, string>
            {
                { Triple.OSType.UnknownOS, "unknown" },
                { Triple.OSType.Ananas,    "ananas" },
                { Triple.OSType.CloudABI,  "cloudabi" },
                { Triple.OSType.Darwin,    "darwin" },
                { Triple.OSType.DragonFly, "dragonfly" },
                { Triple.OSType.FreeBSD,   "freebsd" },
                { Triple.OSType.Fuchsia,   "fuchsia" },
                { Triple.OSType.IOS,       "ios" },
                { Triple.OSType.KFreeBSD,  "kfreebsd" },
                { Triple.OSType.Linux,     "linux" },
                { Triple.OSType.Lv2,       "lv2" },
                { Triple.OSType.MacOSX,    "macosx" },
                { Triple.OSType.NetBSD,    "netbsd" },
                { Triple.OSType.OpenBSD,   "openbsd" },
                { Triple.OSType.Solaris,   "solaris" },
                { Triple.OSType.Win32,     "windows" },
                { Triple.OSType.Haiku,     "haiku" },
                { Triple.OSType.Minix,     "minix" },
                { Triple.OSType.RTEMS,     "rtems" },
                { Triple.OSType.NaCl,      "nacl" },
                { Triple.OSType.CNK,       "cnk" },
                { Triple.OSType.AIX,       "aix" },
                { Triple.OSType.CUDA,      "cuda" },
                { Triple.OSType.NVCL,      "nvcl" },
                { Triple.OSType.AMDHSA,    "amdhsa" },
                { Triple.OSType.PS4,       "ps4" },
                { Triple.OSType.ELFIAMCU,  "elfiamcu" },
                { Triple.OSType.TvOS,      "tvos" },
                { Triple.OSType.WatchOS,   "watchos" },
                { Triple.OSType.Mesa3D,    "mesa3d" },
                { Triple.OSType.Contiki,   "contiki" },
                { Triple.OSType.AmdPAL,    "amdpal" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.OSType.UnknownOS ], Triple.GetCanonicalName( ( Triple.OSType )0x12345678 ) );
        }

        [TestMethod]
        public void GetEnvironmentTypeNameTest( )
        {
            var values = new Dictionary<Triple.EnvironmentType, string>
            {
                { Triple.EnvironmentType.UnknownEnvironment, "unknown" },
                { Triple.EnvironmentType.GNU,                "gnu" },
                { Triple.EnvironmentType.GNUABIN32,          "gnuabin32" },
                { Triple.EnvironmentType.GNUABI64,           "gnuabi64" },
                { Triple.EnvironmentType.GNUEABIHF,          "gnueabihf" },
                { Triple.EnvironmentType.GNUEABI,            "gnueabi" },
                { Triple.EnvironmentType.GNUX32,             "gnux32" },
                { Triple.EnvironmentType.CODE16,             "code16" },
                { Triple.EnvironmentType.EABI,               "eabi" },
                { Triple.EnvironmentType.EABIHF,             "eabihf" },
                { Triple.EnvironmentType.Android,            "android" },
                { Triple.EnvironmentType.Musl,               "musl" },
                { Triple.EnvironmentType.MuslEABI,           "musleabi" },
                { Triple.EnvironmentType.MuslEABIHF,         "musleabihf" },
                { Triple.EnvironmentType.MSVC,               "msvc" },
                { Triple.EnvironmentType.Itanium,            "itanium" },
                { Triple.EnvironmentType.Cygnus,             "cygnus" },
                { Triple.EnvironmentType.AMDOpenCL,          "amdopencl" },
                { Triple.EnvironmentType.CoreCLR,            "coreclr" },
                { Triple.EnvironmentType.OpenCL,             "opencl" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ Triple.EnvironmentType.UnknownEnvironment ], Triple.GetCanonicalName( ( Triple.EnvironmentType )0x12345678 ) );
        }

        public void GetObjFormatTypeNameTest( )
        {
            var values = new Dictionary<Triple.ObjectFormatType, string>
            {
                { Triple.ObjectFormatType.COFF, "coff" },
                { Triple.ObjectFormatType.ELF,  "elf" },
                { Triple.ObjectFormatType.MachO, "macho" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreSame( string.Empty, Triple.GetCanonicalName( ( Triple.ObjectFormatType )0x12345678 ) );
        }
    }
}

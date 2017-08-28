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
            Assert.AreEqual( TripleArchType.Thumb, triple.ArchitectureType );
            Assert.AreEqual( TripleSubArchType.ARMSubArch_v7m, triple.SubArchitecture );
            Assert.AreEqual( TripleVendorType.PC, triple.VendorType );
            Assert.AreEqual( TripleOSType.CUDA, triple.OSType );
            Assert.AreEqual( TripleEnvironmentType.EABI, triple.EnvironmentType );
            Assert.AreEqual( TripleObjectFormatType.COFF, triple.ObjectFormatType );
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
            var values = new Dictionary<TripleArchType, string>
                {
                  { TripleArchType.UnknownArch,    "unknown" },
                  { TripleArchType.Aarch64,        "aarch64" },
                  { TripleArchType.Aarch64_be,     "aarch64_be" },
                  { TripleArchType.Arm,            "arm" },
                  { TripleArchType.Armeb,          "armeb" },
                  { TripleArchType.Avr,            "avr" },
                  { TripleArchType.BPFel,          "bpfel" },
                  { TripleArchType.BPFeb,          "bpfeb" },
                  { TripleArchType.Hexagon,        "hexagon" },
                  { TripleArchType.MIPS,           "mips" },
                  { TripleArchType.MIPSel,         "mipsel" },
                  { TripleArchType.MIPS64,         "mips64" },
                  { TripleArchType.MIPS64el,       "mips64el" },
                  { TripleArchType.MSP430,         "msp430" },
                  { TripleArchType.PPC64,          "powerpc64" },
                  { TripleArchType.PPC64le,        "powerpc64le" },
                  { TripleArchType.PPC,            "powerpc" },
                  { TripleArchType.R600,           "r600" },
                  { TripleArchType.AMDGCN,         "amdgcn" },
                  { TripleArchType.Sparc,          "sparc" },
                  { TripleArchType.Sparcv9,        "sparcv9" },
                  { TripleArchType.Sparcel,        "sparcel" },
                  { TripleArchType.SystemZ,        "s390x" },
                  { TripleArchType.TCE,            "tce" },
                  { TripleArchType.Thumb,          "thumb" },
                  { TripleArchType.Thumbeb,        "thumbeb" },
                  { TripleArchType.X86,            "i386" },
                  { TripleArchType.X86_64,         "x86_64" },
                  { TripleArchType.Xcore,          "xcore" },
                  { TripleArchType.Nvptx,          "nvptx" },
                  { TripleArchType.Nvptx64,        "nvptx64" },
                  { TripleArchType.Le32,           "le32" },
                  { TripleArchType.Le64,           "le64" },
                  { TripleArchType.Amdil,          "amdil" },
                  { TripleArchType.Amdil64,        "amdil64" },
                  { TripleArchType.Hsail,          "hsail" },
                  { TripleArchType.Hsail64,        "hsail64" },
                  { TripleArchType.Spir,           "spir" },
                  { TripleArchType.Spir64,         "spir64" },
                  { TripleArchType.Kalimba,        "kalimba" },
                  { TripleArchType.Lanai,          "lanai" },
                  { TripleArchType.Shave,          "shave" },
                  { TripleArchType.Wasm32,         "wasm32" },
                  { TripleArchType.Wasm64,         "wasm64" },
                  { TripleArchType.Renderscript32, "renderscript32" },
                  { TripleArchType.Renderscript64, "renderscript64" },
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ TripleArchType.UnknownArch ], Triple.GetCanonicalName( ( TripleArchType )0x12345678 ) );
        }

        [TestMethod]
        public void GetSubArchTypeNameTest( )
        {
            var values = new Dictionary<TripleSubArchType, string>
                {
                    { TripleSubArchType.NoSubArch,               string.Empty },
                    { TripleSubArchType.ARMSubArch_v8_2a,        "v8.2a" },
                    { TripleSubArchType.ARMSubArch_v8_1a,        "v8.1a" },
                    { TripleSubArchType.ARMSubArch_v8,           "v8" },
                    { TripleSubArchType.ARMSubArch_v8r,          "v8r" },
                    { TripleSubArchType.ARMSubArch_v8m_baseline, "v8m.base" },
                    { TripleSubArchType.ARMSubArch_v8m_mainline, "v8m.main" },
                    { TripleSubArchType.ARMSubArch_v7,           "v7" },
                    { TripleSubArchType.ARMSubArch_v7em,         "v7em" },
                    { TripleSubArchType.ARMSubArch_v7m,          "v7m" },
                    { TripleSubArchType.ARMSubArch_v7s,          "v7s" },
                    { TripleSubArchType.ARMSubArch_v7k,          "v7k" },
                    { TripleSubArchType.ARMSubArch_v6,           "v6" },
                    { TripleSubArchType.ARMSubArch_v6m,          "v6m" },
                    { TripleSubArchType.ARMSubArch_v6k,          "v6k" },
                    { TripleSubArchType.ARMSubArch_v6t2,         "v6t2" },
                    { TripleSubArchType.ARMSubArch_v5,           "v5" },
                    { TripleSubArchType.ARMSubArch_v5te,         "v5e" },
                    { TripleSubArchType.ARMSubArch_v4t,          "v4t" },
                    { TripleSubArchType.KalimbaSubArch_v3,       "kalimba3" },
                    { TripleSubArchType.KalimbaSubArch_v4,       "kalimba4" },
                    { TripleSubArchType.KalimbaSubArch_v5,       "kalimba5" }
                };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ TripleSubArchType.NoSubArch ], Triple.GetCanonicalName( ( TripleSubArchType )0x12345678 ) );
        }

        [TestMethod]
        public void GetVendorTypeNameTest( )
        {
            var values = new Dictionary<TripleVendorType, string>
            {
                { TripleVendorType.UnknownVendor,           "unknown" },
                { TripleVendorType.Apple,                   "apple" },
                { TripleVendorType.PC,                      "pc" },
                { TripleVendorType.SCEI,                    "scei" },
                { TripleVendorType.BGP,                     "bgp" },
                { TripleVendorType.BGQ,                     "bgq" },
                { TripleVendorType.Freescale,               "fsl" },
                { TripleVendorType.IBM,                     "ibm" },
                { TripleVendorType.ImaginationTechnologies, "img" },
                { TripleVendorType.MipsTechnologies,        "mti" },
                { TripleVendorType.NVIDIA,                  "nvidia" },
                { TripleVendorType.CSR,                     "csr" },
                { TripleVendorType.Myriad,                  "myriad" },
                { TripleVendorType.AMD,                     "amd" },
                { TripleVendorType.Mesa,                    "mesa" },
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ TripleVendorType.UnknownVendor ], Triple.GetCanonicalName( ( TripleVendorType )0x12345678 ) );
        }

        [TestMethod]
        public void GetOsTypeNameTest( )
        {
            var values = new Dictionary<TripleOSType, string>
            {
                { TripleOSType.UnknownOS, "unknown" },
                { TripleOSType.CloudABI,  "cloudabi" },
                { TripleOSType.Darwin,    "darwin" },
                { TripleOSType.DragonFly, "dragonfly" },
                { TripleOSType.FreeBSD,   "freebsd" },
                { TripleOSType.IOS,       "ios" },
                { TripleOSType.KFreeBSD,  "kfreebsd" },
                { TripleOSType.Linux,     "linux" },
                { TripleOSType.Lv2,       "lv2" },
                { TripleOSType.MacOSX,    "macosx" },
                { TripleOSType.NetBSD,    "netbsd" },
                { TripleOSType.OpenBSD,   "openbsd" },
                { TripleOSType.Solaris,   "solaris" },
                { TripleOSType.Win32,     "windows" },
                { TripleOSType.Haiku,     "haiku" },
                { TripleOSType.Minix,     "minix" },
                { TripleOSType.RTEMS,     "rtems" },
                { TripleOSType.NaCl,      "nacl" },
                { TripleOSType.CNK,       "cnk" },
                { TripleOSType.Bitrig,    "bitrig" },
                { TripleOSType.AIX,       "aix" },
                { TripleOSType.CUDA,      "cuda" },
                { TripleOSType.NVCL,      "nvcl" },
                { TripleOSType.AMDHSA,    "amdhsa" },
                { TripleOSType.PS4,       "ps4" },
                { TripleOSType.ELFIAMCU,  "elfiamcu" },
                { TripleOSType.TvOS,      "tvos" },
                { TripleOSType.WatchOS,   "watchos" },
                { TripleOSType.Mesa3D,    "mesa3d" },
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ TripleOSType.UnknownOS ], Triple.GetCanonicalName( ( TripleOSType )0x12345678 ) );
        }

        [TestMethod]
        public void GetEnvironmentTypeNameTest( )
        {
            var values = new Dictionary<TripleEnvironmentType, string>
            {
                { TripleEnvironmentType.UnknownEnvironment, "unknown" },
                { TripleEnvironmentType.GNU,                "gnu" },
                { TripleEnvironmentType.GNUABI64,           "gnuabi64" },
                { TripleEnvironmentType.GNUEABIHF,          "gnueabihf" },
                { TripleEnvironmentType.GNUEABI,            "gnueabi" },
                { TripleEnvironmentType.GNUX32,             "gnux32" },
                { TripleEnvironmentType.CODE16,             "code16" },
                { TripleEnvironmentType.EABI,               "eabi" },
                { TripleEnvironmentType.EABIHF,             "eabihf" },
                { TripleEnvironmentType.Android,            "android" },
                { TripleEnvironmentType.Musl,               "musl" },
                { TripleEnvironmentType.MuslEABI,           "musleabi" },
                { TripleEnvironmentType.MuslEABIHF,         "musleabihf" },
                { TripleEnvironmentType.MSVC,               "msvc" },
                { TripleEnvironmentType.Itanium,            "itanium" },
                { TripleEnvironmentType.Cygnus,             "cygnus" },
                { TripleEnvironmentType.AMDOpenCL,          "amdopencl" },
                { TripleEnvironmentType.CoreCLR,            "coreclr" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreEqual( values[ TripleEnvironmentType.UnknownEnvironment ], Triple.GetCanonicalName( ( TripleEnvironmentType )0x12345678 ) );
        }

        public void GetObjFormatTypeNameTest( )
        {
            var values = new Dictionary<TripleObjectFormatType, string>
            {
                { TripleObjectFormatType.COFF, "coff" },
                { TripleObjectFormatType.ELF,  "elf"  },
                { TripleObjectFormatType.MachO, "macho" }
            };

            foreach( var kvp in values )
            {
                Assert.AreEqual( kvp.Value, Triple.GetCanonicalName( kvp.Key ) );
            }

            Assert.AreSame( string.Empty, Triple.GetCanonicalName( ( TripleObjectFormatType )0x12345678 ) );
        }
    }
}
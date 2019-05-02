// <copyright file="TargetTests.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class TargetTests
    {
        [TestMethod]
        public void CreateTargetMachineTest( )
        {
            var target = Target.FromTriple( DefaultTargetTriple );
            var machine = GetTargetMachine( target );
            Assert.IsNotNull( machine );
            Assert.AreSame( target, machine.Target );
            Assert.AreEqual( DefaultTargetTriple, machine.Triple );
            Assert.AreEqual( DefaultTargetCpu, machine.Cpu );
            Assert.AreEqual( DefaultTargetFeatures, machine.Features );
            Assert.IsNotNull( machine.TargetData );
        }

        [TestMethod]
        public void FromTripleTest( )
        {
            var target = Target.FromTriple( DefaultTargetTriple );
            Assert.IsNotNull( target );
            Assert.AreEqual( "thumb", target.Name );
            Assert.AreEqual( "Thumb", target.Description );
            Assert.IsTrue( target.HasAsmBackEnd );
            Assert.IsTrue( target.HasJIT );
            Assert.IsTrue( target.HasTargetMachine );
        }

        [TestMethod]
        public void AvailableTargetsTest()
        {
            /* string expTargets = GenerateExpectedTargets( ); */

            Assert.IsNotNull( Target.AvailableTargets );
            int foundTargets = 0;
            foreach( var target in Target.AvailableTargets )
            {
                Assert.IsNotNull( target );
                TargetInfo info = TargetInfo.ExpectedTargets[ target.Name ];
                Assert.AreEqual( info.Name, target.Name );
                Assert.AreEqual( info.Description, target.Description );
                Assert.AreEqual( info.HasAsmBackEnd, target.HasAsmBackEnd );
                Assert.AreEqual( info.HasJit, target.HasJIT );
                Assert.AreEqual( info.HasTargetMachine, target.HasTargetMachine );
                ++foundTargets;
            }

            Assert.AreEqual( TargetInfo.ExpectedTargets.Count, foundTargets );
        }

        internal static TargetMachine GetTargetMachine( )
        {
            var target = Target.FromTriple( DefaultTargetTriple );
            return GetTargetMachine( target );
        }

        internal static TargetMachine GetTargetMachine( Target target )
            => target.CreateTargetMachine( DefaultTargetTriple
                                         , DefaultTargetCpu
                                         , string.Empty
                                         , CodeGenOpt.Aggressive
                                         , RelocationMode.Default
                                         , CodeModel.Small
                                         );

        internal const string DefaultTargetTriple = "thumbv7m-none--eabi";
        internal const string DefaultTargetCpu = "cortex-m3";
        internal const string DefaultTargetFeatures = "";

        internal class TargetInfoCollection
            : KeyedCollection<string, TargetInfo>
        {
            protected override string GetKeyForItem( TargetInfo item ) => item.Name;
        }

        internal class TargetInfo
        {
            public TargetInfo( string name, string description, bool hasAsmBackend, bool hasJit, bool hasTargetMachine )
            {
                Name = name;
                Description = description;
                HasAsmBackEnd = hasAsmBackend;
                HasJit = hasJit;
                HasTargetMachine = hasTargetMachine;
            }

            public string Name { get; }

            public string Description { get; }

            public bool HasAsmBackEnd { get; }

            public bool HasJit { get; }

            public bool HasTargetMachine { get; }

            public static TargetInfoCollection ExpectedTargets { get; } = new TargetInfoCollection
            {
                /* ReSharper disable StringLiteralTypo */
                new TargetInfo( "xcore", "XCore", false, false, true ),
                new TargetInfo( "x86-64", "64-bit X86: EM64T and AMD64", true, true, true ),
                new TargetInfo( "x86", "32-bit X86: Pentium-Pro and above", true, true, true ),
                new TargetInfo( "wasm64", "WebAssembly 64-bit", true, false, true ),
                new TargetInfo( "wasm32", "WebAssembly 32-bit", true, false, true ),
                new TargetInfo( "systemz", "SystemZ", true, true, true ),
                new TargetInfo( "sparcel", "Sparc LE", true, true, true ),
                new TargetInfo( "sparcv9", "Sparc V9", true, true, true ),
                new TargetInfo( "sparc", "Sparc", true, true, true ),
                new TargetInfo( "ppc64le", "PowerPC 64 LE", true, true, true ),
                new TargetInfo( "ppc64", "PowerPC 64", true, true, true ),
                new TargetInfo( "ppc32", "PowerPC 32", true, true, true ),
                new TargetInfo( "nvptx64", "NVIDIA PTX 64-bit", false, false, true ),
                new TargetInfo( "nvptx", "NVIDIA PTX 32-bit", false, false, true ),
                new TargetInfo( "msp430", "MSP430 [experimental]", true, false, true ),
                new TargetInfo( "mips64el", "MIPS (64-bit little endian)", true, true, true ),
                new TargetInfo( "mips64", "MIPS (64-bit big endian)", true, true, true ),
                new TargetInfo( "mipsel", "MIPS (32-bit little endian)", true, true, true ),
                new TargetInfo( "mips", "MIPS (32-bit big endian)", true, true, true ),
                new TargetInfo( "lanai", "Lanai", true, false, true ),
                new TargetInfo( "hexagon", "Hexagon", true, true, true ),
                new TargetInfo( "bpfeb", "BPF (big endian)", true, true, true ),
                new TargetInfo( "bpfel", "BPF (little endian)", true, true, true ),
                new TargetInfo( "bpf", "BPF (host endian)", true, true, true ),
                new TargetInfo( "thumbeb", "Thumb (big endian)", true, true, true ),
                new TargetInfo( "thumb", "Thumb", true, true, true ),
                new TargetInfo( "armeb", "ARM (big endian)", true, true, true ),
                new TargetInfo( "arm", "ARM", true, true, true ),
                new TargetInfo( "amdgcn", "AMD GCN GPUs", true, false, true ),
                new TargetInfo( "r600", "AMD GPUs HD2XXX-HD6XXX", true, false, true ),
                new TargetInfo( "aarch64_be", "AArch64 (big endian)", true, true, true ),
                new TargetInfo( "aarch64", "AArch64 (little endian)", true, true, true ),
                new TargetInfo( "arm64", "ARM64 (little endian)", true, true, true )
                /* ReSharper enable StringLiteralTypo */
            };
        }

        /*
        // This is useful for generating the list of expected targets
        // Obviously, since it uses the API being tested, the results require verification
        // before updating the list of ExpectedTargets above, but it helps eliminate tedious
        // typing
        internal string GenerateExpectedTargets( )
        {
            var bldr = new System.Text.StringBuilder( "private static readonly TargetInfoCollection ExpectedTargets = new TargetInfoCollection {" );
            bldr.AppendLine( );
            var targets = System.Linq.Enumerable.ToList( Target.AvailableTargets );
            for( int i = 0; i < targets.Count; ++i )
            {
                var target = targets[ i ];
                bldr.AppendFormat( "    new TargetInfo( \"{0}\", \"{1}\", {2}, {3}, {4} )"
                                 , target.Name
                                 , target.Description
                                 , target.HasAsmBackEnd.ToString( ).ToLowerInvariant( )
                                 , target.HasJIT.ToString( ).ToLowerInvariant( )
                                 , target.HasTargetMachine.ToString( ).ToLowerInvariant( )
                                 );

                bldr.AppendLine( i == targets.Count - 1 ? string.Empty : "," );
            }

            bldr.AppendLine( "};" );
            return bldr.ToString( );
        }
        */
    }
}

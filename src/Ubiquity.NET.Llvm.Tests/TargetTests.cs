// -----------------------------------------------------------------------
// <copyright file="TargetTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class TargetTests
    {
        [TestMethod]
        public void CreateTargetMachineTest( )
        {
            var target = Target.FromTriple( DefaultTargetTriple );
            using var machine = GetTargetMachine( target );

            Assert.IsNotNull( machine );
            Assert.AreSame( target, machine.Target );
            Assert.AreEqual( DefaultTargetTriple, machine.Triple );
            Assert.AreEqual( DefaultTargetCpu, machine.Cpu );
            Assert.AreEqual( DefaultTargetFeatures, machine.Features );
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

#if TEST_AVAILABLE_TARGETS
        // This test is broken as it assumes ALL targets are available. But reality is the API
        // ONLY takes into account the registered targets, which is based on the native library
        // used. Thus, the list changes based on what is registered. And once it is loaded, it
        // cannot be changed so testing the x-plat variants is EXTREMELY difficult.
        // see: https://stackoverflow.com/questions/8189657/can-i-force-mstest-to-use-a-new-process-for-each-test-run
        // Additionally the set of targets depends on the registered CodeGenTarget and is NOT
        // strictly 1:1, so there needs to be an additional means of mapping the target info
        // to a code gen target.
        [TestMethod]
        public void AvailableTargetsTest( )
        {
            // for debug builds show the full list to aid in updating test code
            GenerateExpectedTargets( );

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
#endif

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

            internal static readonly TargetInfoCollection ExpectedTargets = [
                new( "xcore", "XCore", false, false, true ),
                new( "x86-64", "64-bit X86: EM64T and AMD64", true, true, true ),
                new( "x86", "32-bit X86: Pentium-Pro and above", true, true, true ),
                new( "wasm64", "WebAssembly 64-bit", true, false, true ),
                new( "wasm32", "WebAssembly 32-bit", true, false, true ),
                new( "ve", "VE", true, false, true ),
                new( "systemz", "SystemZ", true, true, true ),
                new( "spirv", "SPIR-V Logical", true, false, true ),
                new( "spirv64", "SPIR-V 64-bit", true, false, true ),
                new( "spirv32", "SPIR-V 32-bit", true, false, true ),
                new( "sparcel", "Sparc LE", true, false, true ),
                new( "sparcv9", "Sparc V9", true, false, true ),
                new( "sparc", "Sparc", true, false, true ),
                new( "riscv64", "64-bit RISC-V", true, true, true ),
                new( "riscv32", "32-bit RISC-V", true, true, true ),
                new( "ppc64le", "PowerPC 64 LE", true, true, true ),
                new( "ppc64", "PowerPC 64", true, true, true ),
                new( "ppc32le", "PowerPC 32 LE", true, true, true ),
                new( "ppc32", "PowerPC 32", true, true, true ),
                new( "nvptx64", "NVIDIA PTX 64-bit", false, false, true ),
                new( "nvptx", "NVIDIA PTX 32-bit", false, false, true ),
                new( "msp430", "MSP430 [experimental]", true, false, true ),
                new( "mips64el", "MIPS (64-bit little endian)", true, true, true ),
                new( "mips64", "MIPS (64-bit big endian)", true, true, true ),
                new( "mipsel", "MIPS (32-bit little endian)", true, true, true ),
                new( "mips", "MIPS (32-bit big endian)", true, true, true ),
                new( "loongarch64", "64-bit LoongArch", true, true, true ),
                new( "loongarch32", "32-bit LoongArch", true, false, true ),
                new( "lanai", "Lanai", true, false, true ),
                new( "hexagon", "Hexagon", true, true, true ),
                new( "bpfeb", "BPF (big endian)", true, true, true ),
                new( "bpfel", "BPF (little endian)", true, true, true ),
                new( "bpf", "BPF (host endian)", true, true, true ),
                new( "avr", "Atmel AVR Microcontroller", true, false, true ),
                new( "thumbeb", "Thumb (big endian)", true, true, true ),
                new( "thumb", "Thumb", true, true, true ),
                new( "armeb", "ARM (big endian)", true, true, true ),
                new( "arm", "ARM", true, true, true ),
                new( "amdgcn", "AMD GCN GPUs", true, false, true ),
                new( "r600", "AMD GPUs HD2XXX-HD6XXX", true, false, true ),
                new( "aarch64_32", "AArch64 (little endian ILP32)", true, true, true ),
                new( "aarch64_be", "AArch64 (big endian)", true, true, true ),
                new( "aarch64", "AArch64 (little endian)", true, true, true ),
                new( "arm64_32", "ARM64 (little endian ILP32)", true, true, true ),
                new( "arm64", "ARM64 (little endian)", true, true, true )
            ];
        }

        // This is useful for generating the list of expected targets
        // Obviously, since it uses the API being tested, the results require verification
        // before updating the list of ExpectedTargets above, but it helps eliminate tedious
        // typing
        [SuppressMessage( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Uppercase is WRONG for this" )]
        [Conditional( "DEBUG" )]
        internal static void GenerateExpectedTargets( )
        {
            var bldr = new System.Text.StringBuilder( "internal static readonly TargetInfoCollection ExpectedTargets = new TargetInfoCollection {" );
            bldr.AppendLine( );
            var targets = System.Linq.Enumerable.ToList( Target.AvailableTargets );
            for( int i = 0; i < targets.Count; ++i )
            {
                var target = targets[ i ];
                bldr.AppendFormat( CultureInfo.InvariantCulture
                                 , "    new TargetInfo( \"{0}\", \"{1}\", {2}, {3}, {4} )"
                                 , target.Name
                                 , target.Description
                                 , target.HasAsmBackEnd.ToString( CultureInfo.InvariantCulture ).ToLowerInvariant( )
                                 , target.HasJIT.ToString( CultureInfo.InvariantCulture ).ToLowerInvariant( )
                                 , target.HasTargetMachine.ToString( CultureInfo.InvariantCulture ).ToLowerInvariant( )
                                 );

                bldr.AppendLine( i == targets.Count - 1 ? string.Empty : "," );
            }

            bldr.AppendLine( "};" );
            Debug.WriteLine( bldr.ToString( ) );
        }
    }
}

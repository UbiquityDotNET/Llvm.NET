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
            using( var context = new Context( ) )
            {
                var target = Target.FromTriple( DefaultTargetTriple );
                var machine = GetTargetMachine( context, target );
                Assert.IsNotNull( machine );
                Assert.AreSame( context, machine.Context );
                Assert.AreSame( target, machine.Target );
                Assert.AreEqual( DefaultTargetTriple, machine.Triple );
                Assert.AreEqual( DefaultTargetCpu, machine.Cpu );
                Assert.AreEqual( DefaultTargetFeatures, machine.Features );
                Assert.IsNotNull( machine.TargetData );
            }
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

        internal static TargetMachine GetTargetMachine( Context context )
        {
            var target = Target.FromTriple( DefaultTargetTriple );
            return GetTargetMachine( context, target );
        }

        internal static TargetMachine GetTargetMachine( Context context, Target target )
        {
            return target.CreateTargetMachine( context
                                             , DefaultTargetTriple
                                             , DefaultTargetCpu
                                             , string.Empty
                                             , CodeGenOpt.Aggressive
                                             , Reloc.Default
                                             , CodeModel.Small
                                             );
        }

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

            public static readonly TargetInfoCollection ExpectedTargets = new TargetInfoCollection
            {
                new TargetInfo( "xcore", "XCore", false, false, true ),
                new TargetInfo( "x86-64", "64-bit X86: EM64T and AMD64", true, true, true ),
                new TargetInfo( "x86", "32-bit X86: Pentium-Pro and above", true, true, true ),
                new TargetInfo( "systemz", "SystemZ", true, true, true ),
                new TargetInfo( "sparcel", "Sparc LE", true, true, true ),
                new TargetInfo( "sparcv9", "Sparc V9", true, true, true ),
                new TargetInfo( "sparc", "Sparc", true, true, true ),
                new TargetInfo( "riscv64", "64-bit RISC-V", true, false, true ),
                new TargetInfo( "riscv32", "32-bit RISC-V", true, false, true ),
                new TargetInfo( "ppc64le", "PowerPC 64 LE", true, true, true ),
                new TargetInfo( "ppc64", "PowerPC 64", true, true, true ),
                new TargetInfo( "ppc32", "PowerPC 32", true, true, true ),
                new TargetInfo( "nvptx64", "NVIDIA PTX 64-bit", false, false, true ),
                new TargetInfo( "nvptx", "NVIDIA PTX 32-bit", false, false, true ),
                new TargetInfo( "msp430", "MSP430 [experimental]", false, false, true ),
                new TargetInfo( "mips64el", "Mips64el [experimental]", true, true, true ),
                new TargetInfo( "mips64", "Mips64 [experimental]", true, true, true ),
                new TargetInfo( "mipsel", "Mipsel", true, true, true ),
                new TargetInfo( "mips", "Mips", true, true, true ),
                new TargetInfo( "lanai", "Lanai", true, false, true ),
                new TargetInfo( "hexagon", "Hexagon", true, false, true ),
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
            };
        }

        /*internal string GenerateExpectedTargets( )
        {
            var bldr = new System.Text.StringBuilder( "public static TargetInfoCollection ExpectedTargets = new TargetInfoCollection {" );
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
                var lastEntry = i == targets.Count - 1;
                bldr.AppendLine( lastEntry ? string.Empty : "," );
            }

            bldr.AppendLine( "};" );
            return bldr.ToString( );
        }
        */
    }
}
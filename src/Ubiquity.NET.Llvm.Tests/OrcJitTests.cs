// -----------------------------------------------------------------------
// <copyright file="OrcJitTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.JIT;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class OrcJitTests
    {
        [Ignore("Orc JIT unreliable")]
        [TestMethod]
        public void TestEagerIRCompilation( )
        {
            using var ctx = new Context( );
            var nativeTriple = Triple.HostTriple;
            var target = Target.FromTriple( nativeTriple );
            var machine = target.CreateTargetMachine( nativeTriple );
            using var orcJit = new OrcJit( machine );
            using( var lazyModule = CreateModule( ctx, machine, 10101010, "lazy1", "lazy1" ) )
            {
                orcJit.AddLazyCompiledModule( lazyModule );
            }

            using( var lazyModule = CreateModule( ctx, machine, 20202020, "lazy2", "lazy2" ) )
            {
                orcJit.AddLazyCompiledModule( lazyModule );
            }

            // try several different modules with the same function name replacing the previous
            using( var module = CreateModule( ctx, orcJit.TargetMachine, 42 ) )
            {
                AddAndExecuteTestModule( orcJit, module, 42 );
            }

            using( var module = CreateModule( ctx, orcJit.TargetMachine, 12345678 ) )
            {
                AddAndExecuteTestModule( orcJit, module, 12345678 );
            }

            using( var module = CreateModule( ctx, orcJit.TargetMachine, 87654321 ) )
            {
                AddAndExecuteTestModule( orcJit, module, 87654321 );
            }
        }

        private void AddAndExecuteTestModule( OrcJit orcJit, BitcodeModule module, int magicNumber )
        {
            ulong orcHandle = orcJit.AddEagerlyCompiledModule( module );
            var main = orcJit.GetFunctionDelegate<TestMain>("main");
            Assert.IsNotNull( main );
            Assert.AreEqual( magicNumber, main( ) );
            orcJit.RemoveModule( orcHandle );
        }

        private static BitcodeModule CreateModule( Context ctx, TargetMachine machine, int magicNumber, string modulename = "test", string functionname = "main" )
        {
            var module = ctx.CreateBitcodeModule(modulename);
            module.Layout = machine.TargetData;
            IrFunction main = module.CreateFunction( functionname, ctx.GetFunctionType( ctx.Int32Type ) );
            BasicBlock entryBlock = main.AppendBasicBlock("entry");
            var bldr = new InstructionBuilder(entryBlock);
            bldr.Return( ctx.CreateConstant( magicNumber ) );
            return module;
        }

        private delegate int TestMain( );
    }
}

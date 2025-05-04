﻿// -----------------------------------------------------------------------
// <copyright file="LibraryInitTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class LibraryInitTests
    {
        // Since these test validates the initialize and dispose of the the native library the
        // native code is no longer usable in the process that runs this test. Therefore this
        // is set to use a distinct process. (Sadly, this doesn't work in the VS test explorer
        // so it is simply marked as "skipped" there. Command line dotnet test via the scripts
        // does correctly run the test however)
        [DistinctProcessTestMethod]
        [SuppressMessage( "Style", "IDE0063:Use simple 'using' statement", Justification = "Explicit scoping helps make usage more clear" )]
        public void TestLibraryReInit( )
        {
            // Module test fixture MUST NOT run in distinct process case to init LLVM
            // or that will result in an exception trying to "re-init" it again here.
            Assert.IsNull(ModuleFixtures.LibLLVM);

            using(ILibLlvm lib = Library.InitializeLLVM())
            {
                Assert.IsNotNull(lib);
                // Test supported targets; This is a LibLLVM extension as at one point the
                // set of supported targets was ONLY the native target and that of one
                // additional targetNames-plat target. That's not true anymore, so this should verify
                // that ALL (non-experimental) targets are an option.
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_AArch64));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_AMDGPU));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_ARM));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_AVR));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_BPF));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_Hexagon));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_Lanai));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_LoongArch));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_MIPS));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_MSP430));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_NVPTX));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_PowerPC));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_RISCV));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_Sparc));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_SPIRV));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_SystemZ));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_VE));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_WebAssembly));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_X86));
                Assert.IsTrue(lib.SupportedTargets.Contains(LibLLVMCodeGenTarget.CodeGenTarget_XCore));

                // Now test registered targets (subset of supported)
                lib.RegisterTarget(LibLLVMCodeGenTarget.CodeGenTarget_ARM);
                var targets = GetRegisteredTargets().ToArray();
                // NOTE: There are multiple actual targets under the one registered target
                //       The registration is more along the lines of a "family" of targets...
                Assert.AreEqual(4, targets.Length);
                string?[] targetNames = [ .. targets.Select(h=>LLVMGetTargetName(h)) ];
                // order of names/targets is not guaranteed, so just test for presence via Contains().
                Assert.IsTrue(targetNames.Contains("thumbeb"));
                Assert.IsTrue(targetNames.Contains("thumb"));
                Assert.IsTrue(targetNames.Contains("armeb"));
                Assert.IsTrue(targetNames.Contains("arm"));
            }

            // After dispose - "that's all she wrote", LLVM native libraries
            // Do not support re-init, once shutdown is called that's it.
            Assert.ThrowsExactly<InvalidOperationException>(()=>
            {
                using var lib2 = Library.InitializeLLVM();
            });
        }

        public static IEnumerable<LLVMTargetRef> GetRegisteredTargets()
        {
            var current = LLVMGetFirstTarget( );
            while( current != default )
            {
                yield return current;
                current = LLVMGetNextTarget( current );
            }
        }
    }
}

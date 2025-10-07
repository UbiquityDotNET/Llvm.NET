// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.OrcJITv2;

namespace Ubiquity.NET.Llvm.JIT.Tests
{
    [TestClass]
    public class OrcJitTests
    {
        private static readonly LazyEncodedString FooBodySymbolName = "foo_body"u8;
        private static readonly LazyEncodedString BarBodySymbolName = "bar_body"u8;

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using", Justification = "Unit test, explicit control intended")]
        public void TestStringPool_Dispose_leaves_0_refcount( )
        {
#if !DEBUG
            Assert.Inconclusive("Not applicable to release builds");
#else
            using var jit = new LLJit();

            // This is a bit dodgy in that it depends on undocumented behavior
            // the format of the result string is NOT guaranteed stable.
            var initSyms = jit.Session.SymbolStringPool.GetSymbolsInPool();

#pragma warning disable IDE0063 // Use simple 'using' statement; Explicit scoping helps clarify tests and make debugging it easier
            using(SymbolStringPoolEntry entry = jit.MangleAndIntern(FooBodySymbolName))
            {
                using(SymbolStringPoolEntry entry2 = entry.AddRef())
                {
                    var activeSyms = jit.Session.SymbolStringPool.GetSymbolsInPool();
                    Assert.AreEqual(initSyms.Length + 1, activeSyms.Length);
                } // end of using scope should decrement ref count (once)
            } // end of using scope should decrement ref count (once)
#pragma warning restore IDE0063 // Use simple 'using' statement

            // clear entries with 0 ref count; Should include the two just destroyed
            jit.Session.SymbolStringPool.ClearDeadEntries();
            var postSyms = jit.Session.SymbolStringPool.GetSymbolsInPool();
            Assert.IsTrue(postSyms.SequenceEqual(initSyms), "Should have initial symbol count");
#endif
        }

        [TestMethod]
        public void TestVeryLazyJIT( )
        {
            using var jit = new LLJit();

            var triple = jit.TripleString;
            using ThreadSafeModule mainMod = ParseTestModule(MainModuleSource.NormalizeLineEndings(LineEndingKind.LineFeed)!, "main-mod"u8);

            // Place the generated module in the JIT
            jit.Add( jit.MainLib, mainMod );
            SymbolFlags flags = new(SymbolGenericOption.Exported | SymbolGenericOption.Callable);

            using SymbolStringPoolEntry mangledFooBodySymName = jit.MangleAndIntern(FooBodySymbolName);

            var fooSym = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolFlags>
            {
                [mangledFooBodySymName] = flags,
            }.ToImmutable();

            using var fooMu = new CustomMaterializationUnit("FooMU"u8, Materialize, fooSym);
            jit.MainLib.Define( fooMu );

            using var mangledBarBodySymName = jit.MangleAndIntern(BarBodySymbolName);

            var barSym = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolFlags>
            {
                [mangledBarBodySymName] = flags,
            }.ToImmutable();

            using var barMu = new CustomMaterializationUnit("BarMU"u8, Materialize, barSym);
            jit.MainLib.Define( barMu );

            using var callThruMgr = jit.Session.CreateLazyCallThroughManager(triple);
            using var mangledFoo = jit.MangleAndIntern("foo"u8);
            using var mangledBar = jit.MangleAndIntern("bar"u8);

            var reexports = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolAliasMapEntry>
            {
                [mangledFoo] = new(mangledFooBodySymName, flags),
                [mangledBar] = new(mangledBarBodySymName, flags),
            }.ToImmutable();

            using var ism = new LocalIndirectStubsManager(triple);
            using var lazyReExports = new LazyReExportsMaterializationUnit(callThruMgr, ism, jit.MainLib, reexports);
            jit.MainLib.Define( lazyReExports );

            ulong address = jit.Lookup("entry"u8);

            unsafe
            {
                var entry = (delegate* unmanaged[Cdecl]<int, int>)address;
                int result = entry(1); // Conditionally calls "foo" with lazy materialization
                Assert.AreEqual( 1, result );

                result = entry( 0 ); // Conditionally calls "bar" with lazy materialization
                Assert.AreEqual( 2, result );
            }

            // Local function to handle materializing the very lazy symbols (foo|bar)
            // This function captures "jit" and therefore the materializer instance
            // above must remain valid until the JIT is destroyed or the materialization
            // occurs. This is guaranteed by language use of `using`.
            void Materialize( MaterializationResponsibility r )
            {
                // symbol strings returned are NOT owned by this function so Dispose() isn't needed (Though it is an allowed NOP)
                using var symbols = r.GetRequestedSymbols();
                Debug.Assert( symbols.Count == 1, "Unexpected number of symbols!" );

                using var fooBodyName = jit.MangleAndIntern(FooBodySymbolName);
                using var barBodyName = jit.MangleAndIntern(BarBodySymbolName);

                ThreadSafeModule module;
                if(symbols[ 0 ].Equals( fooBodyName ))
                {
                    Debug.WriteLine( "Parsing module for 'Foo'" );
                    module = ParseTestModule( FooModuleSource.NormalizeLineEndings( LineEndingKind.LineFeed )!, "foo-mod" );
                }
                else if(symbols[ 0 ].Equals( barBodyName ))
                {
                    Debug.WriteLine( "Parsing module for 'Bar'" );
                    module = ParseTestModule( BarModuleSource.NormalizeLineEndings( LineEndingKind.LineFeed )!, "bar-mod" );
                }
                else
                {
                    Debug.WriteLine( "Unknown symbol" );

                    // Not a known symbol - fail the materialization request.
                    r.Fail();
#pragma warning disable IDISP007 // Don't dispose injected
                    // ABI requires disposal in this case
                    r.Dispose();
#pragma warning restore IDISP007 // Don't dispose injected
                    return;
                }

                // ownership of the module is transferred on success
                // this protects it in the face of an exception
                using(module)
                {
                    // apply the data Layout
                    module.WithPerThreadModule( ApplyDataLayout );

                    // Finally emit the module to the JIT.
                    // This transfers ownership of both the responsibility AND the module
                    // to the native LLVM JIT.
                    jit.TransformLayer.Emit( r, module );
                }

                ErrorInfo ApplyDataLayout( IModule module )
                {
                    module.DataLayoutString = jit.DataLayoutString;

                    // no API calls in this function provide an ErrorInfo instance, so just return
                    // the default (success)
                    //
                    // Note: The invoking callback will catch any exceptions thrown from this function
                    //       AND convert them to an ErrorInfo to return to the native code.
                    return default;
                }
            }
        }

        private static ThreadSafeModule ParseTestModule( LazyEncodedString src, LazyEncodedString name )
        {
            using var threadSafeContext = new ThreadSafeContext();
            var ctx = threadSafeContext.PerThreadContext;

            // Ownership is transferred, this protects in the event of an exception
            using var module = ctx.ParseModule(src, name);
            return new ThreadSafeModule( threadSafeContext, module );
        }

        private const string MainModuleSource = """
        define i32 @entry(i32 %argc)
        {
        entry:
            %tobool = icmp ne i32 %argc, 0
            br i1 %tobool, label %if.foo, label %if.bar

        if.foo:
            %call = tail call i32 @foo()
            br label %return

        if.bar:
            %call1 = tail call i32 @bar()
            br label %return

        return:
            %retval.0 = phi i32 [ %call, %if.foo ], [ %call1, %if.bar ]
            ret i32 %retval.0
        }

        declare i32 @foo()
        declare i32 @bar()
        """;

        private const string FooModuleSource = """
        define i32 @foo_body() {
          entry:
            ret i32 1
        }
        """;

        private const string BarModuleSource = """
        define i32 @bar_body()
        {
          entry:
            ret i32 2
        }
        """;
    }
}

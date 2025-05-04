using System;
using System.Diagnostics;

using Ubiquity.NET.Extensions;
using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.OrcJITv2;

internal class Program
{
    private static void Main(/*string[] args*/)
    {
        using var libLLVM = Library.InitializeLLVM( );

        // Using JIT so the only target of relevance is the native machine
        libLLVM.RegisterTarget( CodeGenTarget.Native );

        using var jit = new LlJIT();

        var triple = jit.TripleString;
        using ThreadSafeModule mainMod = ParseTestModule(MainModuleSource.NormalizeLineEndings(LineEndingKind.LineFeed)!, "main-mod");

        // Place the generated module in the JIT
        jit.AddModule(jit.MainLib, mainMod);
        SymbolFlags flags = new(SymbolGenericOption.Exported | SymbolGenericOption.Callable);

        using var internedFooBodyName = jit.MangleAndIntern(FooBodySymbolName);
        var fooSym = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolFlags> {
            [internedFooBodyName] = flags,
        }.ToImmutable();

        // ownership of this Materialization Unit (MU) is "moved" to the JITDyLib in the
        // call to Define. Applying a "using" ensures it is released even if an exception
        // occurs that prevents completion of the transfer. When transfer completes the
        // MU is marked as disposed but a call to Dispose() is a safe NOP. Thus, this handles
        // all conditions consistently
        using var fooMu = new CustomMaterializationUnit("FooMU", Materialize, fooSym);
        jit.MainLib.Define(fooMu);

        using var internedBarBodyName = jit.MangleAndIntern(BarBodySymbolName);
        var barSym = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolFlags> {
            [internedBarBodyName] = flags,
        }.ToImmutable();

        using var barMu = new CustomMaterializationUnit("BarMU", Materialize, barSym);
        jit.MainLib.Define(barMu);

        using var ism = new LocalIndirectStubsManager(triple);
        using var callThruMgr = jit.Session.CreateLazyCallThroughManager(triple);

        using var internedFoo = jit.MangleAndIntern("foo");
        using var internedBar = jit.MangleAndIntern("bar");

        var reexports = new KvpArrayBuilder<SymbolStringPoolEntry, SymbolAliasMapEntry> {
            [internedFoo] = new(internedFooBodyName, flags),
            [internedBar] = new(internedBarBodyName, flags),
        }.ToImmutable();

        using var lazyReExports = new LazyReExportsMaterializationUnit(callThruMgr, ism, jit.MainLib, reexports);
        jit.MainLib.Define(lazyReExports);

        UInt64 address = jit.Lookup("entry");

        unsafe
        {
            var entry = (delegate* unmanaged[Cdecl]<Int32, Int32>)address;
            int result = entry(1); // Conditionally calls "foo" with lazy materialization
            Debug.Assert(1 == result);

            result = entry(0); // Conditionally calls "bar" with lazy materialization
            Debug.Assert(2 == result);
        }

        // Local function to handle materializing the very lazy symbols
        // This function captures "jit" and therefore the materializer instance
        // above must remain valid until the JIT is destroyed or the materialization
        // occurs.
        void Materialize(MaterializationResponsibility r)
        {
            // symbol strings returned are NOT owned by this function so Dispose() isn't needed (Though it is an allowed NOP)
            using var symbols = r.GetRequestedSymbols();
            Debug.Assert(symbols.Count == 1, "Unexpected number of symbols!");

            using var fooBodyName = jit.MangleAndIntern(FooBodySymbolName);
            using var barBodyName = jit.MangleAndIntern(BarBodySymbolName);

            ThreadSafeModule module;
            if(symbols[0].Equals(fooBodyName))
            {
                Debug.WriteLine("Parsing module for 'Foo'");
                module = ParseTestModule(FooModuleSource.NormalizeLineEndings(LineEndingKind.LineFeed)!, "foo-mod");
            }
            else if (symbols[0].Equals(barBodyName))
            {
                Debug.WriteLine("Parsing module for 'Bar'");
                module = ParseTestModule(BarModuleSource.NormalizeLineEndings(LineEndingKind.LineFeed)!, "bar-mod");
            }
            else
            {
                Debug.WriteLine("Unknown symbol");

                // Not a known symbol - fail the materialization request.
                r.Fail();
                r.Dispose();
                return;
            }

            using(module)
            {
                // apply the data Layout
                module.WithPerThreadModule(ApplyDataLayout);

                // Finally emit the module to the JIT.
                // This transfers ownership of both the responsibility AND the module
                // to the native LLVM JIT.
                jit.TransformLayer.Emit(r, module);
            }

            ErrorInfo ApplyDataLayout(IModule module)
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

    static ThreadSafeModule ParseTestModule(LazyEncodedString src, LazyEncodedString name)
    {
        using var threadSafeContext = new ThreadSafeContext();
        var ctx = threadSafeContext.PerThreadContext;
        using var module = ctx.ParseModule( src, name );
        return new ThreadSafeModule( threadSafeContext, module);
    }

    const string FooBodySymbolName = "foo_body";
    const string BarBodySymbolName = "bar_body";

    const string MainModuleSource = """
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

    const string FooModuleSource = """
    define i32 @foo_body() {
    entry:
        ret i32 1
    }
    """;

    const string BarModuleSource = """
    define i32 @bar_body()
    {
    entry:
        ret i32 2
    }
    """;
}

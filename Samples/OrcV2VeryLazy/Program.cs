using System;
using System.Collections.Generic;
using System.Diagnostics;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.JIT.OrcJITv2;

internal class Program
{
    private static void Main(/*string[] args*/)
    {
        using var libLLVM = Library.InitializeLLVM( );

        // Testing JIT so the only target of relevance is the native machine
        libLLVM.RegisterTarget( CodeGenTarget.Native );

        using var jit = new LlJIT();

        // Keep the callback alive and valid until not needed. (End of scope)
        // NOTE: There is an inherent conflict of dependency in that the
        //       Materialize local function will capture the JIT instance
        //       and therefore cannot exist before it and must be disposed
        //       before it. The JIT owns the callback to the materializer
        //       so it can call the materializer at any point while code is
        //       executing in the JIT. However, that is resolved by understanding
        //       that the materializer is not relevant unless the JIT is running
        //       code that has not yet materialized the symbols it controls or,
        //       more likely in shutdown, not running any code any more.
        //       Thus, once all execution is done Dispose() on this before
        //       Dispose() on the JIT is safe. Additionally, the internal implementation
        //       of the native callbacks will dispose of the materializer AFTER
        //       either a call to Materialize or Destroy.
        using var materializer = new CustomMaterializer(Materialize);

        var triple = jit.TripleString;
        using ThreadSafeModule mainMod = ParseTestModule(MainModuleSource.NormalizeLineEndings(LineEndingKind.LineFeed)!, "main-mod");

        // Place the generated module in the JIT
        jit.AddModule(jit.MainLib, mainMod);
        SymbolFlags flags = new(SymbolGenericOption.Exported | SymbolGenericOption.Callable);

        List<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> fooSym = [
            new(jit.MangleAndIntern(FooBodySymbolName), flags),
        ];

        List<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> barSym = [
            new(jit.MangleAndIntern(BarBodySymbolName), flags),
        ];

        using var fooMu = new CustomMaterializationUnit("FooMU", materializer, fooSym);
        using var barMu = new CustomMaterializationUnit("BarMU", materializer, barSym);
        jit.MainLib.Define(fooMu);
        jit.MainLib.Define(barMu);

        using var ism = new LocalIndirectStubsManager(triple);
        using var callThruMgr = jit.Session.CreateLazyCallThroughManager(triple);

        List<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> reexports =[
            new(jit.MangleAndIntern("foo"), new(jit.MangleAndIntern(FooBodySymbolName), flags)),
            new(jit.MangleAndIntern("bar"), new(jit.MangleAndIntern(BarBodySymbolName), flags)),
        ];

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

            // apply the data Layout
            module.WithPerThreadModule(ApplyDataLayout);

            // Finally emit the module to the JIT.
            // This transfers ownership of both the responsibility AND the module
            // to the native LLVM JIT.
            jit.TransformLayer.Emit(r, module);

            ErrorInfo ApplyDataLayout(BitcodeModule module)
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
        return new ThreadSafeModule( threadSafeContext, ctx.ParseModule( src, name ) );
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

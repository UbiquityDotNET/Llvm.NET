# LibLLVMBindingsGenerator
As the name implies this is a code generator for use in building the LLVM libraries.
Historically it did more than it does now but that use proved to be difficult to maintain (It
generated all the P/Invoke code from a custom representation in YAML. In the end that was just
extra work as not much of generating interop for the LLVM-C was automatable. (A LOT required
reading the docs, and often the LLVM source code to determine if it was an ownership transfer
(move) and, if so, how to dispose of the resource once done with it. All of that is expressible
IN C# already as part of P/Invoke generation so there wasn't much point in continuing with
that. (Though there is something to be said for use as a starting point...)

## Split implementation
This app was subsequently split into to implementations that now exist in distinct repositories
1) Generates the EXPORTS.g.def for the Windows DLL generation from the LLVM + LIBLLVM headers
    1) This version lives in the [LibllVM repository](https://github.com/UbiquityDotNET/Llvm.Libs)
2) Generates the "safe handle" C# code from the LLVM + LIBLLVM headers
    1) This version is the one in this repository this document discusses

### Common implementation 
While there is a common implementation between the implementations (They started as simply the
same code and commenting out the functionality not desired) they have and will diverge over
time, though anything in the core parsing of headers and general code generation from templates
is likely to remain. (It may be viable to support a common library for this scenario but this
is ONLY necessary when the native side of the interop library changes)

## Usage
> [!IMPORTANT]
> This project has a dependency on the `CppSharp` library which ONLY supports the `X64`
> architecture but the generated wrappers are NOT dependent on a particular architecture.
> This limits the environments that can be used to generate the sources. To simplify that,
> the generated sources are placed into source control but generated off-line by a developer.
> A developer machine doing this ***MUST*** be X64 or this tool can't run. This is a limitation
> defined by a dependent library.

`LlvmBindingsGenerator -l <llvmRoot> -e <extensionsRoot> -h <HandleOutputPath> [-Diagnostics <Diagnostic>]`

| Options Property   | Usage |
|--------------------|-------|
| LlvmRoot           | This is the root of the LLVM directory in the repository containing the llvm headers |
| ExtensionsRoot     | This is the root of the directory containing the extended LLVM-C headers from the LibLLVM project |
| HandleOutputPath   | [Optional] Path to the root folder where the handle files are generated |
| Diagnostics        | Diagnostics output level for the app |

This tool is generally only required once per Major LLVM release. (Though a Minor release
that adds new APIs would also warrant a new run) However, to ensure the code generation tool
itself isn't altered with a breaking change, the PowerShell script takes care of running the
generator to update the Generated code base on each run, even if nothing changes in the end.
This is run on every automated build before building the res of the project so that
the generator is tested on every full automated build. 

### Generated code
This library will generate the handle file directly. Therefore ROSLYN source generators are
not used. 

>[!IMPORTANT]
>The generated files are not usable on their own. They depend on the additional types found in
> Ubiquity.NET.Llvm.Interop assembly. The design of this app assumes that is where the results
> are used. ANY USE outside of that context is ***STRONGLY*** discouraged and explicitly NOT
> supported.

#### Roslyn Source Generators - 'There be dragons there!'
Roslyn allows source generators directly in the compiler making for a feature similar to C++
template code generation AT compile time. However, there's a couple of BIG issues with that for
this particular code base.
1) Non-deterministic ordering, or more specifically for this app, no way to declare the
   dependency on ***outputs*** of one generator as the ***input*** for another.
2) Dependencies for project references
    - As a generator for this is not general purpose it would not be published or produced
      as a NUGET package. It would only work as a project reference. But that creates a TON
      of problems for the binary runtime dependencies of source generators, which don't flow
      with them as project references...

Specifically, in this code, the built-in generator that otherwise knows noting about the
handle generation, needs to see and use the **OUTPUT** of the handle source generation. (It's
not just a run ordering problem as ALL generators see the same input text!)  
[See: [Discussion on ordering and what a generator "sees"](https://github.com/dotnet/roslyn/discussions/57912#discussioncomment-1682779)
[See: [Roslyn issue #57239](https://github.com/dotnet/roslyn/issues/57239)]]

The interop code uses the LibraryImportAttribute for AOT support of ALL of the interop APIs
declared. Thus, at compile time the interop source generator **MUST** be able to see the used,
specifically, it must have access to the `NativeMarshalling` attribute for all the handle
types. Otherwise, it doesn't know how to marshal the type and bails out. It is possible to
"overcome" this with an explicit `MarshalUsingAttribute` on every parameter or return type
but that's tedious. Tedious, typing is what source generators and templates are supposed to
remove. Thus, this library will host the source generator (like a unit test would) and
generates the handle sources **BEFORE** they are compiled in the project. Thus, the generated
source files will contain the marshaling attributes so that the interop source generator knows
how to generate the correct code.

>To be crystal clear - The problem is **NOT** one of generator run ordering, but on the
> ***dependency of outputs***. By design, Roslyn source generators can only see the original
> source input, never the output of another generator. Most don't, and never will, care. The
> handle generation, in this case, does. Solving that generically in a performant fashion is a
> ***HARD*** problem indeed... Not guaranteed impossible, but so far no-one has come up with a
> good answer to the problem. Even C++ has this issue with templates+concepts+CRTP; and that
> language has had source generating templates as a direct part of the language for several
> decades now.  
[See also: [Using the CRTP and C++20 Concepts to Enforce Contracts for Static Polymorphism](https://medium.com/@rogerbooth/using-the-crtp-and-c-20-concepts-to-enforce-contracts-for-static-polymorphism-a27d93111a75) ]  
[See also: [Rules for Roslyn source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)]

#### Alternate solutions considered and rejected
1) Running the source generator directly in the project
    1) This is where the problem on non-deterministic ordering and visibility of the generated
       code was discovered. Obviously (now anyway!) this won't work.
2) Use a source generator in a separate assembly
    1) This solves the generator output dependency problem but introduces a new problem of how
       the build infrastructure for these types manage NuGet versions
    2) Additionally, this adds complexity of a second native dependency on the library
       exporting the native functionality. (Should there be two copies? How does code in each
       refer to the one instance?...)
3) Call the source generator from within this app to control the ordering
    1) This at least could get around the ordering/dependency problem as it would guarantee the
       custom generator runs before the built-in one.
    2) However, this runs afoul of the binary dependency problem... Not 100% insurmountable but
       the number of caveats on the Roslyn Source Generator side of things grows to a
       significant factor.

#### The final choice
Keep using this app as a generator for the handle types. This used to work, and still does.
However, this doesn't solve the problem of expressing managed code things in a custom language
(YAML) but it's at least a rather simplistic expression for the handles. And arguably less
complicated then all the subtleties of using a Roslyn Source generator for this sort of one off
specialized code generation.

Solving the problem of expressing P/Invokes is simply to just manage that directly. It seemed
like a good idea to automate the tedium of generating those. Sadly, there are so many
subtleties of "special cases" that involve reading the docs (or source code) before you can
correctly implement it. In the end, there's no value in expressing all that subtlety in anything
other than C#.

This also keeps the door open to use the native AST from within the source generator or an
analyzer to perform additional checks and ensure the hand written code matches the actual
native code... (Though this would involve more direct use of the Roslyn parser/analyzer and
may be best to generate an input to a proper analyzer)


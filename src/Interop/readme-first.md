# Interop Support
This folder contains the low level LLVM direct interop support. It requires specialized build
ordering and processing, which is handled by the [Build-Interop.ps1](../../Build-Interop.ps1)
PowerShell script.

The nature of the .NET SDK projects and VCX projects drives the need for the script,instead of
VS solution dependencies or even MSBuild project to project references. Unfortunately, due to
the way multi-targeting is done in the newer C# SDK projects the project to project references
don't work with C++. The VCXproj files don't have targets for all the .NET targets. Making that
all work seamlessly in VS is just plain hard work that has, thus far, not worked particularly
well. Thus, the design here uses a simpler PowerShell script that takes care of building the
correct platform+configuration+target framework combinations of each and finally builds the NuGet
package from the resulting binaries.

# OBSOLECENSE NOTE
Most of the functionality of this tool for generating the interop API signatures was gutted and
removed. It fell into the "good idea at the time" category. But in reality turned out to be a
greater PITA than worth. The source code committed to the repo now includes the C# interop code
so this generator is not needed for most of that. It **IS** still used for the following scenarios:
1) To check the APIs exported from the extensions header(s)
    1) Specifcally to ensure they are all named with "LibLLVM" as a leading prefix to identify them
       as distinct from any official LLVM exported APIs.
2) To generate the `EXPORTS.g.def` file used by the native code DLL
    1) This includes all of the extended APIS AND the official native APIs
3) To generate the source for the handle types.
    1) These types are very tedious and repetitive which is the perfect use of some form of template
    2) Global handles are mostly covered in the `GlobalHandleBase` class
    3) Context handles are all `readonly record` types that wrap a native integer so have more to
       generate.
        1) In the future when .NET 10 is available as Long Term Support (LTS) these will change to
          a `ref` struct so that the compiler can validate usage as never stored. They are not
          currently using that as they also need to support the `IContextHandle{T}` interface for
          marshalling. `allows ref` is ONLY available in .NET9 which has only Short Term Support (STS)

## Roslyn Source Generators - 'There be dragons there!'
Roslyn allows source generators directly in the compiler making for a feature similar to C++ template
code generation AT compile time. However, there's a copule of BIG issue with that for this particular
code base.
1) Non-deterministic ordering, or more specifically no way to declare the dependency on outputs of
   one generator as the input for another.
2) Dependencies for project references
    - As the generators are not general purpose they are not published or produced as a NUGET package.
      They only would work as a project reference. But that creates a TON of problems for the binary
      runtime dependencies of source generators, which don't flow with them as project references...

Specifically, in this code, the built-in generator that otherwise knows noting about the handle generation,
needs to see and use the **OUTPUT** of the handle source generation. (It's not just a run ordering problem
as ALL generators see the same input text!)  
[See: [Discussion on ordering and what a generator "sees"](https://github.com/dotnet/roslyn/discussions/57912#discussioncomment-1682779)
[See: [Roslyn issue #57239](https://github.com/dotnet/roslyn/issues/57239)]]

The interop code uses the the LibraryImportAttribute for AOT support of ALL of the interop APIs
declared. Thus, at compile time the interop source generator **MUST** be able to see the used,
specifically, it must have access to the `NativeMarshalling` attribute for all the handle types.
Otherwise, it doesn't know how to marshal the type and bails out. It is possible to "overcome"
this with an explicit `MarshalUsingAttribute` on every parameter or return type but that's tedious.
Tedious, typing is what soure generators and templates are supposed to remove. Thus, this library
will host the source generator (like a unit test would) and generates the handle sources **BEFORE**
they are compiled in the project. Thus, the generated source files will contain the marshalling
attributes so that the interop source generator knows how to generate the correct code.

>To be crystal clear - The problem is **NOT** one of generator run ordering, but on the ***dependency
of outputs***. By design, Roslyn source generators can only see the original source input, never
the output of another generator. Most don't, and never will, care. The handle generation, in this case
does. Solving that generically in a performant fashion is a ***HARD*** problem indeed... Not guaranteed
impossible, but so far no-one has come up with a good answer to the problem. Even C++ has this issue with
templates+concepts+CRTP; and that language has had source generating templates as a direct part of the
language for several decades now.  
[See also: [Using the CRTP and C++20 Concepts to Enforce Contracts for Static Polymorphism](https://medium.com/@rogerbooth/using-the-crtp-and-c-20-concepts-to-enforce-contracts-for-static-polymorphism-a27d93111a75) ]  
[See also: [Rules for Roslyn source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)]

### Alternate solutions considered and rejected
1) Running the source generator directly in the project
    1) This is where the problem on non-deterministic ordering and visibility of the generated code
       was discovered. Obviously (now anyway!) this won't work.
2) Use a source generator in a seperate assembly
    1) This solves the generator output dependency problem but introduces a new problem of how
       the build infrastructore for these types manage nuget versions
    2) Additionally, this adds complexity of a second native dependency on the dll exporting
       the native functionality. (Should there be two copies? How does code in each refer to
       the one instance?...)
3) Call the source generator from within this app to control the ordering
    1) This at least could get around the ordering/dpendency problem as it would gurantee the custom
       generator runs before the built-in one.
    2) However, this runs afoul of the binary dependency problem... Not 100% insurmountable but the
       number of caveats on the Roslyn Source Generator side of things grows to a significant factor

### The final choice
Keep using this library as a generator for the handle types. This used to work, and still does. However,
this doesn't solve the problem of expressing managed code things in a custom language (YAML) but it's
at least a rather simplistic expression for the handles. And arguably less complicated then all the
subtleties of using a Roslyn Source generator for this sort of one off specialized code generation.

This also keeps the door open to use the native AST from within the source generator or an analyzer to
perform additional checks and ensure the hand written code matches the actual native code... (Though
this would involve more direct use of the roslyn parser/analyzer and may be best to generate an input
to a proper anaylzer)

## Projects
### LlvmBindingsGenerator
This is the P/Invoke generator for the generated interop code in Ubiquity.NET.Llvm.Interop. It uses
CppSharp to parse the C or C++ headers and generate the C# P/Invoke API declarations, enums and value
types required to interop with the native code. The generator has a number of automatic marshaling
rules, but there are ambiguous cases that it needs a configuration file to resolve. The configuration
file provides resolution of the ambiguities and also helps in detection of missing or removed APIs when
moving to a newer version of LLVM.

#### Usage
`LlvmBindingsGenerator <llvmRoot> <extensionsRoot> [OutputPath]`

| Parameter | Usage |
|------------|-------|
| llvmroot   | This is the root of the LLVM directory in the repository containing the llvm headers |
| extensionsRoot | this is the root of the directory containing the extended LLVM-C headers from the LibLLVM project |
| InputPath  | Root directory containg the "GlobalHandles.cs" and "ContextHandles.cs" files to generate the backing implementation for
| OutputPath | This is the root of the location to generate the output into, normally this is the "GeneratedCode" sub folder of the Ubiquity.NET.Llvm.Interop project so the files are generated into the project |

This tool is generally only required once per Major LLVM release. (Though a Minor release that adds new APIs
would also warrant a new run) However, to ensure the code generation tool itself isn't altered with a breaking
change, the PowerShell script takes care of running the generator to update the Ubiquity.NET.Llvm.Interop
code base on each run, even if nothing changes in the end. This is run on every automated build before building
the Ubiquity.NET.Llvm.Interop project so that the generator is tested on every full automated build. 

### LibLLVM
This is the native project that creates the extended LLVM-C API as an actual DLL. Currently
only Windows 64 bit is supported, though other configurations are plausible with additional
build steps in the PowerShell script to build for other platforms. The extensions are configured
to build with high C++ conformance mode, so they should readily build without much modification
for other platforms given the appropriate build infrastructure is set up.

### Ubiquity.NET.Llvm.Interop
This is the .NET P/Invoke layer that provides the raw API projection to .NET. The, majority
of the code is generated P/Invokes from the LlvmBindingsGenerator tool. There are a few
additional support classes that are consistent across variations in LLVM. While this library
has a runtime dependency on the native LibLLVM binary there is no compile time dependency.

## Building the Interop libraries
### General requirements
There are some general steps that are required to successfully build the interop NuGet package and a couple
of different ways to go about completing them.
 1. Build LlvmBindingsGenerator
 2. Run LlvmBindingsGenerator to parse the llvm headers and the extended headers from the native LibLLVM
    1. This generates the C# interop code AND the linker DEF file used by the native library and therefore
       needs to run before the other projects are built. Generating the exports file ensures that it is always
       accurate and any functions declared in the headers are exported so that the linker generate an error
       for any missing implementation(s).
 3. Build the native libraries for all supported runtimes (OS+arch)
 4. Build Ubiquity.NET.Llvm.Interop to create the interop assembly and, ultimately create the final NuGet package with
the native and manged code bundled together.

### Automated build
The interop libraries are built using the Build-Interop.ps1 PowerShell script. This script is required
to correctly build the projects in an automated build as it isn't possible to accomplish all the required
steps in a standard project/solution. (OK, impossible is a bit strong as creating custom targets and tasks
could probably cover it but at the expense of greater complexity). The script is pretty simple though
understanding why it is needed is a more complex matter this document is aimed towards.

>[!NOTE]
> Building the interop tests via the solution is required as building the CSproj doesn't 'publish' the test.
> If you build via a solution file, then the publish automatically occurs. (You can run the publish target via
> `dotnet publish`, but that's both an extra step AND ends up doing things differently then when you build the
> solution! (There's a publish folder with binaries in it and things are generally in different places - Go Figure!)






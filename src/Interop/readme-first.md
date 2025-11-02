# Interop Support
This folder contains the low level LLVM direct interop support. It requires some specialized
build ordering in particular the generated handle wrappers are needed to build the interop
library, which is, currently^1^, a dependency of the LLVM wrapper library.

# OBSOLECENSE NOTE
Most of the functionality of this tool for generating the interop API signatures was gutted
and removed. It fell into the "good idea at the time" category. In reality it turned out to
be a greater PITA than worth. The source code committed to the repo now includes the C#
interop code so this generator is not needed for most of that. It **IS** still used for the
following scenarios:
* To generate the source for the handle types.
    - These types are very tedious and repetitive which is the perfect use of some form of
      template
    - All Handle kinds structs that are simple type safe wrappers around an `nint`
        - Global handles all implement `IDisposable` to make disposal consistent.
            - They are still immutable types and the use of IDisposable is simply to unify
              the disposal and leverage built-in patterns for ownership. Since these are
              immutable types the `Dispose()` is NOT idempotent.
            - This is normally not an issue as this is NOT inteded for exposire to end users.
              It is expected that the handles are wrapped in an OO type that can replace the
              handle held with a default value on Dispose() to allow for an idempotent
              dispose.
                - Idempotent dispose is useful for APIs that use `move` semantics where the
                  native API takes over ownership, but only on success. Thus, a caller might
                  still own the resource on error/exception. By allowing idempotent `Dispose`
                  the caller need not care about such subtlties and ALWAYS calls `Dispose`
                  which is normally a NOP, but if an error occured actually releases the
                  resource.

## Roslyn Source Generators - 'There be dragons there!'
Roslyn allows source generators directly in the compiler making for a feature similar to C++
template code generation AT compile time. However, there's a couple of BIG issues with that
for this particular code base.
1) Non-deterministic ordering, or more specifically no way to declare the dependency on
   outputs of one generator as the input for another.
2) Dependencies for project references
    - The generators are not general purpose so they are not published or produced as a
      NUGET package. They only would be of use as a project reference. But that creates a
      TON of problems for the binary runtime dependencies of source generators, which don't
      flow with them as project references...

Specifically, in this code, the built-in generator, which otherwise knows nothing about the
handle generation, needs to see and use the **OUTPUT** of the handle source generation. 
(It's not just a run ordering problem as ALL generators see the same input text!)  
[See: [Discussion on ordering and what a generator "sees"](https://github.com/dotnet/roslyn/discussions/57912#discussioncomment-1682779)
[See: [Roslyn issue #57239](https://github.com/dotnet/roslyn/issues/57239)]]

The interop code uses the LibraryImportAttribute for AOT support of ALL of the interop APIs
declared. Thus, at compile time the interop source generator **MUST** be able to see the
used, specifically, it must have access to the `NativeMarshalling` attribute for all the
handle types. Otherwise, it doesn't know how to marshal the type and bails out. It is
possible to "overcome" this with an explicit `MarshalUsingAttribute` on every parameter or
return type but that's tedious. Tedious, typing is what source generators and templates are
supposed to remove. Thus, this library includes the generated sources as part of the
repository. A developer must run the `Generate-HandleWrappers.ps1` script whenever the
native library is updated to ensure the wrappers are up to date with the native headers
**BEFORE** they are compiled in the project. Thus, the generated source files will contain
the marshalling attributes so that the interop source generator knows how to generate the
correct code.

>To be crystal clear - The problem is **NOT** one of generator run ordering, but on the
> ***dependency of outputs***. By design, Roslyn source generators can only see the original
> source input, never the output of another generator. Most don't, and never will, care. The
> handle generation, in this case, does. Solving that generically in a performant fashion is
> a ***HARD*** problem indeed... Not guaranteed impossible, but so far no-one has come up
> with a good answer to the problem. Even C++ has this issue with templates+concepts+CRTP;
> and that language has had source generating templates as a direct part of the language for
> several decades now.  
[See also: [Using the CRTP and C++20 Concepts to Enforce Contracts for Static Polymorphism](https://medium.com/@rogerbooth/using-the-crtp-and-c-20-concepts-to-enforce-contracts-for-static-polymorphism-a27d93111a75) ]  
[See also: [Rules for Roslyn source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)]

### Alternate solutions considered and rejected
1) Running the source generator directly in the project
    1) This is where the problem on non-deterministic ordering and visibility of the
       generated code was discovered. Obviously, (now anyway!) this won't work.
2) Use a source generator in a separate assembly
    1) This solves the generator output dependency problem but introduces a new problem of
       how the build infrastructure for these types manage NuGet versions.
    2) Additionally, this adds complexity of a second native dependency on the DLL exporting
       the native functionality. (Should there be two copies? How does code in each refer to
       the one instance?...)
3) Call the source generator from within this app to control the ordering
    1) This at least could get around the ordering/dependency problem as it would guarantee
       the custom generator runs before the built-in one.
    2) However, this runs afoul of the binary dependency problem... Not 100% insurmountable
       but the number of caveats on the Roslyn Source Generator side of things grows to a
       significant factor. This makes it more trouble than it's worth.
4) Mark each call site with `MarhalUsing` so that it is explicit at the call site instead
   of needing a source generator.
    1) This can work, however it means a great deal of tedious attribution that the
       compilation should do for us.
        1) Elimination of tedious typing and repetive code is WHY templates/generics exist.

### The final choice
Keep using `LlvmBindingsGenerator` as a generator for the handle types. This used to work,
and still does. However, this doesn't solve the problem of expressing managed code things in
a custom language (YAML) but it's at least a rather simplistic expression for the handles.
And, arguably, less complicated then all the subtleties of using a Roslyn Source generator
for this sort of one off specialized code generation.

This also keeps the door open to use the native AST from within the source generator or an
analyzer to perform additional checks and ensure the hand written code matches the actual
native code... (Though this would involve more direct use of the Roslyn parser/analyzer and
may be best to generate an input to a proper analyzer)

## Projects
### LlvmBindingsGenerator
This is the P/Invoke generator for the handle wrappers in Ubiquity.NET.Llvm.Interop. It uses
`CppSharp` to parse the C consumable headers and generate wrappers for all the handles in
the native library (Global, Context, and global aliased).

#### Usage
`LlvmBindingsGenerator -l <llvmRoot> -e <extensionsRoot> [-o <OutputPath>]`

| Parameter | Usage |
|------------|-------|
| llvmroot   | This is the root of the LLVM directory in the repository containing the llvm headers |
| extensionsRoot | this is the root of the directory containing the extended LLVM-C headers from the LibLLVM project |
| InputPath  | Root directory containing the "GlobalHandles.cs" and "ContextHandles.cs" files to generate the backing implementation for
| OutputPath | This is the root of the location to generate the output into, normally this is the "GeneratedCode" sub folder of the Ubiquity.NET.Llvm.Interop project so the files are generated into the project |

This tool is generally only required once per Major LLVM release. It is run locally and the
resulting generated code is checked in. This is due to a limitation of the `CppSharp`
dependency. It only supports `x64` targets and this assembly is intended for use with any
RID supported by .NET (and the underlying LLVM) ^2^

### Ubiquity.NET.Llvm.Interop
This is the .NET P/Invoke layer that provides the raw API projection to .NET. The, majority
of the code is simply P/Invokes to the native library. There are a few additional support
classes that are consistent across variations in LLVM. While this library has a runtime
dependency on the native LibLLVM binary there is no compile time dependency.

## Building the Interop libraries
### General requirements
There are some general steps that are required to successfully build the interop NuGet
package and a couple of different ways to go about completing them.
1. Build LlvmBindingsGenerator
2. Run LlvmBindingsGenerator to parse the llvm headers and the extended headers from the
   native LibLLVM.
    1. This generates the C# interop code AND the linker DEF file used by the native library
       and therefore needs to run before the other projects are built.
        1. Generating the exports file ensures that it is always accurate and any functions
           declared in the headers are exported. This also ensures that the linker generates
           an error for any missing implementation(s).
 3. Build Ubiquity.NET.Llvm.Interop to create the interop assembly and, ultimately create
    the final NuGet package with the native and manged code bundled together.

Steps 1-2 are only needed once per build of the native libraries and are wrapped into the
`Generate-WrappedHandles.ps1` for developer convenience. (It is otherwise very difficult to
specify how the generator must build, and then run the generator ***Before*** building the
interop. Since it only needs to be done on an update to the native libraries, this is
simplified by making it a developer local action. (This also resolves the problem of `x64`
only target support)

>[!NOTE]
> Keeping generation as a local developer action resolves the problem of binary dependencies
> of the generator itself. The generator depends on `CppSharp` which depends on a native
> `libclang` and ONLY supports X64 architectures. Thus, the generator itself cannot run on
> any target that is not X64.

---
^1^ There is some consideration/thinking to eliminate the interop library entirely and move
all of it's functionality to the main LLVM.NET assembly itself. Therefore, no production app
or library should release with that as a dependency (except transitively from the wrapper)
as it may not exist in the future  
:warning: You have been warned! :warning:

^2^ Currently ONLY win-x64 is supported but the foundational work is present to allow
building for other platorms.




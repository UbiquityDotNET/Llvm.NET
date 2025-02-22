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
Most of the functionality described here is now obsolete. The source code committed to the repo
includes the C# interop code so this generator is not needed for that. It **IS** still used to
check the APIs exported from the extensions AND to generate the `EXPORTS.g.def` file used by the
native code DLL. But that's where it ends.

The marshalling support and SourceGenerators in C# and .NET has progressed to the point where
it is easier to express the marshalling needs of an API, in the C# language itself instead of
a foreign language custom for this case (the YAML config file). For simplicity sake, the config
file format and parsing remains intact [Though that may change in the future once everything
else settles down]

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
| OutputPath | This is the root of the location to generate the output into, normally this is the root of the Ubiquity.NET.Llvm.Interop project so the files are generated into the project |

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






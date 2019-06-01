# Interop Support
This folder contains the low level LLVM direct interop support. It requires specialized build
ordering and processing, which is handled by the [Build-Interop.ps1](../../Build-Interop.ps1)
PowerShell script.

The nature of the .NET SDK projects and VCX projects drives the need for the script,instead of
VS solution dependencies or even MSBuild project to project references. Unfortunately, due to
the way multi-targeting is done in the newer C# SDK projects project to project references
don't work. The VCXproj files don't have targets for all the .NET targets. Making that all work
seamlessly in VS is just plain hard work that has, thus far, not worked particularly well. Thus,
the design here uses a simpler PowerShell script that takes care of building the correct
platform+configuration+target framework combinations of each and finally builds the NuGet
package from the resulting binaries.

## Projects
### LibLLVM
This is the native project that creates the extended LLVM-C API as an actual DLL. Currently
only Windows 64 bit is supported, though other configurations are plausible with additional
build steps in the PowerShell script to build for other platforms. The extensions are configured
to build with high C++ conformance mode, so they should readily build without much modification
for other platforms given the appropriate build infrastructure is set up.

### Llvm.NET.Interop
This is the .NET P/Invoke layer that provides the raw API projection to .NET. The, majority
of the code is generated P/Invokes from the LlvmBindingsGenerator tool. There are a few
additional support classes that are consistent across variations in LLVM. While this library
has a runtime dependency on the native LibLLVM binary there is no compile time dependency.

### LlvmBindingsGenerator
This is the P/Invoke generator for the generated interop code in Llvm.NET.Interop. It uses
CppSharp to parse the C headers and generate the C# P/Invoke API declarations, enums and value
types required to interop with the native code.

#### Usage
`LlvmBindingsGenerator <llvmRoot> <extensionsRoot> [OutputPath]`

| Parameter | Usage |
|------------|-------|
| llvmroot   | This is the root of the LLVM directory in the repository containing the llvm headers |
| extensionsRoot | this is the root of the directory containing the extended LLVM-C headers from the LibLLVM project |
| OutputPath | This is the root of the location to generate the output into, normally this is the root of the Llvm.NET.Interop project so the files are generated into the project |

This tool is generally only required once per Major LLVM release. (Though a Minor release that adds new APIs
would also warrant a new run) However, to ensure the code generation tool itself isn't altered with a breaking
change, the PowerShell script takes care of running the generator to update the Llvm.NET.Interop
code base on each run, even if nothing changes in the end. This is run on every automated build before building
the Llvm.NET.Interop project so that the generator is tested on every full automated build. 

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
 4. Build Llvm.NET.Interop to create the interop assembly and, ultimately create the final NuGet package with
the native and manged code bundled together.

### Automated build
The interop libraries are built using the Build-Interop.ps1 PowerShell script. This script is required
to correctly build the projects in an automated build as it isn't possible to accomplish all the required
steps in a standard project/solution. (OK, impossible is a bit strong as creating custom targets and tasks
could probably cover it but at the expense of greater complexity). The script is pretty simple though
understanding it is a more complex matter this document is aimed towards.

### Manually (developer inner loop)
While it is possible to use the PowerShell script as part of the development of the interop libraries themselves,
it is generally easier to use the Interop.sln. The solution contains projects for the native libraries, the
bindings generator and the managed interop. Using the solution requires that you manually build/run the projects.

1. Build LlvmBindingsGenerator project
2. Run LlvmBindingsGenerator (via command line or debugger launch) with the location of the LLVM headers, the
LibLLVM headers, and the output location of generated code for the Llvm.NET.Interop project.
     1. This, generates C# interop source files AND also generates the native C++ EXPORTS.DEF for the LibLLVM library
and therefore, must run before building either of the other libraries.
3. Build LibLLVM project for all architectures and configurations.
   1. At present the only supported runtime and architecture is Windows 64bit so batch building, etc.. isn't required.
      Other runtimes and architectures are possible in the future, however.
4. Build the Llvm.NET.Interop project.






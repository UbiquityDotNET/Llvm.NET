// -----------------------------------------------------------------------
// <copyright file="GlobalNamespaceImports.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
NOTE:
While the MSBuild `ImplicitUsings` property is banned from this repo, the C# language feature of global usings is NOT.
The build property will auto include an invisible and undiscoverable (without looking up obscure documentation)
set of namespaces that is NOT consistent or controlled by the developer. THAT is what is BAD/BROKEN about that feature.
By banning it's use and then providing a `GlobalNamespaceImports.cs` source file with ONLY global using statements ALL of
that is eliminated. Such use of the language feature restores FULL control and visibility of the namespaces to the developer,
where it belongs. For a good explanation of this problem see: https://rehansaeed.com/the-problem-with-csharp-10-implicit-usings/.
For an explanation of the benefits of the language feature see: https://www.hanselman.com/blog/implicit-usings-in-net-6
*/

global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.IO;
global using System.Linq;

global using Ubiquity.NET.InteropHelpers;
global using Ubiquity.NET.Llvm.DebugInfo;
global using Ubiquity.NET.Llvm.Instructions;
global using Ubiquity.NET.Llvm.Interop;
global using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;
global using Ubiquity.NET.Llvm.Interop.ABI.llvm_c;
global using Ubiquity.NET.Llvm.Properties;
global using Ubiquity.NET.Llvm.Types;
global using Ubiquity.NET.Llvm.Values;

// Extended C API
global using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;
global using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ModuleBindings;

// Official LLVM-C API
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Analysis;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.BitReader;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.BitWriter;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Blake3;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Comdat;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Disassembler;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.IrReader;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Linker;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Object;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.PassBuilder;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Target;
global using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

/*
NOTE:
While the MsBuild `ImplicitUsings` property is banned from this repo, the C# language feature of global usings is NOT.
The build property will auto include an invisible and undiscoverable (without looking up obscure documentation)
set of namespaces that is NOT consistent or controlled by the developer. THAT is what is BAD/BROKEN about that feature.
By banning it's use and then providing a `GlobalNamespaceImports.cs` source file with ONLY global using statements ALL of
that is eliminated. Such use of the language feature restores FULL control and visibility of the namespaces to the developer,
where it belongs. For a good explanation of this problem see: https://rehansaeed.com/the-problem-with-csharp-10-implicit-usings/.
For an explanation of the benefits of the language feature see: https://www.hanselman.com/blog/implicit-usings-in-net-6
*/

global using System;
global using System.Buffers;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Numerics;
global using System.Reflection;
global using System.Resources;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Runtime.InteropServices.Marshalling;
global using System.Text;
global using System.Threading;

global using Ubiquity.NET.Extensions;

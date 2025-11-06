// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "826adb1c-4bdc-4a89-836b-87088e511d6a" )]

[assembly: CLSCompliant( false )]

[assembly: Parallelize( Scope = ExecutionScope.ClassLevel )]
[assembly: ExcludeFromCodeCoverage]

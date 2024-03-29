﻿// -----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliant( false )]

// This assembly does not expose COM types
[assembly: ComVisible( false )]

[assembly: NeutralResourcesLanguage( "en" )]

// allow use of internal types in the JIT support built on top of this library
[assembly: InternalsVisibleTo("Ubiquity.NET.Llvm.JIT")]

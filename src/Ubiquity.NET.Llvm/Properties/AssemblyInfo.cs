// -----------------------------------------------------------------------
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

// use of code generation does NOT require use of the JIT, but the JIT library is effectively
// considered a "part" of this library so allow internal access.
//
// NOTE: This is NOT in any way a security matter or "whole" (anyone could us reflection to
// get to the internals anyway or just use the raw interop library directly) This is a convenience
// to help people to do the right thing by making it MUCH harder to do the wrong thing. It is
// impossible to claim (or at least easy to refute such a claim...) that one is ignorant of the
// "wrongness" of such misuse. The only legit answer is an equivalent of "tough noogies".
[assembly: InternalsVisibleTo("Ubiquity.NET.llvm.JIT")]

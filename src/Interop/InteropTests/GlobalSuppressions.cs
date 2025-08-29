// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Unit Tests" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Unit Tests" )]
[assembly: SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "unit tests use namespaces that match API", Scope = "namespace", Target = "~N:Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.UT" )]

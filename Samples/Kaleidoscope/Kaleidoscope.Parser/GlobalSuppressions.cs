﻿// <copyright file="GlobalSuppressions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

/* This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
*/

using System.Diagnostics.CodeAnalysis;

// until full docs generation for the sample support libraries is enabled, these are just annoying noise
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Sample Application" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Sample Application" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Sample Application" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample Application" )]

[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope = "type", Target = "Kaleidoscope.Grammar.KaleidoscopeBaseVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1715", Justification = "Generated code", MessageId = "Result", Scope = "type", Target = "Kaleidoscope.Grammar.IKaleidoscopeVisitor`1" )]
[assembly: SuppressMessage( "Microsoft.Naming", "CA1708", Justification = "Generated code" )]

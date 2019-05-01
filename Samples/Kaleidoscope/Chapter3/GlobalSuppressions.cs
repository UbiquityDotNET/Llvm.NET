// <copyright file="GlobalSuppressions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

/* This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
*/

using System.Diagnostics.CodeAnalysis;

// until full docs generation for the sample support libraries is enabled, these are just annoying noise
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample Application" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Sample Application" )]
[assembly: SuppressMessage( "StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions", Justification = "Regions used to extract code snippets for documentation" )]
[assembly: SuppressMessage( "StyleCop.CSharp.ReadabilityRules", "SA1123:Do not place regions within elements", Justification = "Regions used to extract code snippets for documentation" )]
[assembly: SuppressMessage( "ReSharper", "LocalizableElement", Justification = "Sample Application")]
[assembly: SuppressMessage( "Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Sample Application" )]

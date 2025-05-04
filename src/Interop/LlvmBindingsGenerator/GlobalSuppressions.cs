// -----------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/* attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
*/

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage( "Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Use is Controlled by preprocessor define 'GENERATE_CS_INTEROP'" )]

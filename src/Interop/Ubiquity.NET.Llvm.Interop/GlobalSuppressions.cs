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

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Low level ABI interop; Read C ABI headers/docs for details" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Low level ABI interop; Read C ABI headers/docs for details" )]
[assembly: SuppressMessage( "Interoperability", "CA1401:P/Invokes should not be visible", Justification = "The point of this DLL is to present the native interop ABI" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Low level ABI interop; Read C ABI headers/docs for details" )]
[assembly: SuppressMessage( "Redundancies in Symbol Declarations", "RECS0007:The default underlying type of enums is int, so defining it explicitly is redundant.", Justification = "ABI interop; intent is to be explicit for clarity (even if redundant)" )]
[assembly: SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Interop ABI structs and methods using them are defined in same file; deal with it." )]
[assembly: SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Interop ABI naming matches source for most structures and types" )]
[assembly: SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "Interop ABI naming matches source for most structures and types" )]
[assembly: SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Interop ABI naming matches source for most structures and types" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Filenames match that of the native ABI header file" )]
[assembly: SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Interop ABI naming matches source for most structures and types" )]
[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1642:Constructor summary documentation should begin with standard text", Justification = "Dumb rule - if the summary is really **supposed** to be a fixed format then tooling should just generate it..." )]
[assembly: SuppressMessage( "Maintainability", "CA1506:Avoid excessive class coupling", Justification = "The whole point of this library is to expose the native ABI methods and types" )]
[assembly: SuppressMessage( "Naming", "CA1712:Do not prefix enum values with type name", Justification = "Matches ABI. Wrapping OO library provides .NET 'style' names" )]
[assembly: SuppressMessage( "Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Matches ABI. Wrapping OO library provides .NET 'style' names" )]

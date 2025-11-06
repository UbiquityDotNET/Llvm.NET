// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties

// Setting ComVisible to false makes the types in this assembly not visible to COM
// components. If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("312c3354-ea8c-4819-8823-ac78064c645c")]

// Tests are so trivial they perform better when not individually parallelized.
// Unfortunately this is an assembly wide choice and not class or method level
// see: https://github.com/microsoft/testfx/issues/5555#issuecomment-3448956323
[assembly: Parallelize( Scope = ExecutionScope.ClassLevel )]

// can't use this at assembly level as it isn't supported there for downlevel... [Sigh...]
//[assembly: ExcludeFromCodeCoverage]

// NOTE: use of this and `internal` test classes results in a flurry of
// error CA1812: '<class name>' is an internal class that is apparently never instantiated. If so, remove the code from the assembly.
//       If this class is intended to contain only static members, make it 'static' (Module in Visual Basic).
//       (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1812)
// In other words, not worth the bother...
// [assembly: DiscoverInternals]

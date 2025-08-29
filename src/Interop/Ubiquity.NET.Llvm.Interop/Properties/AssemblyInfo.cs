// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Resources;

[assembly: CLSCompliant( false )]

// This assembly does not expose COM types
[assembly: ComVisible( false )]

[assembly: NeutralResourcesLanguage( "en" )]

// FULLY AOT compatible, NO runtime marshaling support enabled
[assembly: DisableRuntimeMarshalling]

﻿// <copyright file="IKaleidoscopeCodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    /// <summary>Enumeration to define the kinds of diagnostic intermediate data to generate for each function definition</summary>
    [Flags]
    public enum DiagnosticRepresentations
    {
        /// <summary>No diagnostics</summary>
        None,

        /// <summary>Generate an XML representation of the parse tree</summary>
        Xml,

        /// <summary>Generate a DGML representation of the parse tree</summary>
        Dgml,

        /// <summary>Generates a BlockDiag representation of the parse tree</summary>
        BlockDiag,

        /// <summary>Emits debug tracing during the parse to an attached debugger</summary>
        DebugTraceParser,
    }
}

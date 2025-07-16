// -----------------------------------------------------------------------
// <copyright file="VisualizationKind.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Enumeration to define the kinds of diagnostic intermediate data to generate from a runtime/language AST</summary>
    [Flags]
    public enum VisualizationKind
    {
        /// <summary>No Visualizations</summary>
        None,

        /// <summary>Generate an XML representation of the parse tree</summary>
        Xml,

        /// <summary>Generate a DGML representation of the parse tree</summary>
        Dgml,

        /// <summary>Generates a BlockDiag representation of the parse tree</summary>
        BlockDiag,

        /// <summary>Emits debug tracing during the parse to an attached debugger</summary>
        /// <remarsk>This is a NOP if no debugger is attached</remarsk>
        DebugTraceParser,

        /// <summary>Emit all visualizations</summary>
        All = Xml | Dgml | BlockDiag | DebugTraceParser
    }
}

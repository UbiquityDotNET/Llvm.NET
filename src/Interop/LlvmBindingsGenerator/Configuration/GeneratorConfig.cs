// -----------------------------------------------------------------------
// <copyright file="GeneratorConfig.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration
{
    /// <summary>Configuration  root type</summary>
    internal class GeneratorConfig
    {
        /// <summary>Gets or sets the set of names of LLVM-C functions that are declared as returning LLVMBool but have semantics of SUCCESS == 0</summary>
        /// <remarks>
        /// The inconsistency on the semantics of LLVMBool returns is confusing, so these are used to set LLVMStatus as the return for
        /// the generated code.
        /// </remarks>
        public SortedSet<string> StatusReturningFunctions { get; set; } = new SortedSet<string>( );

        // mapping of function + parameter/Return marshaling info that cannot be inferred from the declarations
        public List<IMarshalInfo> MarshalingInfo { get; set; } = new List<IMarshalInfo>( );

        // Set of deprecated functions and optional message, depending on options generation may filter
        // these out completely (default)
        public Dictionary<string, string> DeprecatedFunctionToMessageMap { get; set; } = new Dictionary<string, string>( );

        // Functions that have manually placed P/Invoke in the templates should be ignored
        // to prevent confusion. These are normally the string disposal types.
        public IDictionary<string, bool> InternalFunctions { get; set; } = new Dictionary<string, bool>( );

        // maps a handle type name to a template for generating the interop for the handle
        public HandleTemplateMap HandleToTemplateMap { get; set; } = new HandleTemplateMap( );

        public Dictionary<string, string> AnonymousEnumNames { get; set; } = new Dictionary<string, string>( );

        public SortedSet<string> IgnoredHeaders { get; set; } = new SortedSet<string>( );

        public SortedSet<string> AliasReturningFunctions { get; set; } = new SortedSet<string>( );
    }
}

// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Configuration
{
    /// <summary>Interface for a generator configuration</summary>
    /// <remarks>
    /// In earlier versions of this app, the configuration was read in from an external YAML file.
    /// However, over time that became quite a bit simpler and mostly excessive overhead. When the
    /// handle generation was split from the exports.def generation use of a YAML file was removed.
    /// To maintain the minimum of changes and allow for easier comparisons this interface is still
    /// used, though the implementation is now entirely expressed in code.
    /// </remarks>
    internal interface IGeneratorConfig
    {
        /// <summary>Gets the Headers to ignore when parsing the input</summary>
        ImmutableArray<string> IgnoredHeaders { get; }

        /// <summary>Gets an array of the details for a handle in the input</summary>
        ImmutableArray<HandleDetails> HandleMap { get; }
    }

    internal static class GeneratorConfigExtensions
    {
        /// <summary>Builds the template map (as a lookup) for all the handles.</summary>
        /// <returns>Lookup of the handle name to the template that generates code for it.</returns>
        /// <remarks>
        /// It is possible, for global aliased handles, that there are multiple templates for a given
        /// handle. Thus, a lookup is used to map a handle name to all of the templates for that
        /// handle.
        /// </remarks>
        public static ILookup<string, IHandleCodeTemplate> BuildTemplateMap( this IGeneratorConfig self )
        {
            // get all the templates to use for generating the output code
            var handleTemplates = from h in self.HandleMap
                                  from template in Transforms( h )
                                  select (h.Name, template);

            return handleTemplates.ToLookup((p)=>p.Name, (p)=>p.template);
        }

        private static IEnumerable<IHandleCodeTemplate> Transforms( HandleDetails h )
        {
            if(string.IsNullOrWhiteSpace(h.Disposer))
            {
                yield return new ContextHandleTemplate( h.Name );
            }
            else
            {
                yield return new GlobalHandleTemplate( h.Name, h.Disposer, h.Alias );

                // for aliases treat them like a context handle as they are
                // not owned by the managed code and only reference the native
                // handle via a simple nint. Context Handle template creates
                // a type safe wrapper around the raw 'nint' (as a value type) that
                // does NOT implement IDisposable. (Unlike a SafeHandle)
                if(h.Alias)
                {
                    yield return new ContextHandleTemplate( $"{h.Name}Alias" );
                }
            }
        }
    }
}

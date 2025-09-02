// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Help;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Extension of <see cref="RootCommand"/> that allows app control of defaults that are otherwise forced</summary>
    [SuppressMessage( "Design", "CA1010:Generic interface should also be implemented", Justification = "Collection initialization" )]
    public class AppControlledDefaultsRootCommand
        : RootCommand
    {
        /// <summary>Initializes a new instance of the <see cref="AppControlledDefaultsRootCommand"/> class.</summary>
        /// <param name="description">Description of this root command</param>
        /// <param name="settings">Settings to apply for the command parsing</param>
        public AppControlledDefaultsRootCommand( string description = "", CmdLineSettings? settings = null )
            : base( description )
        {
            settings ??= new CmdLineSettings();

            // RootCommand constructor already adds HelpOption and VersionOption so remove them
            // unless specified by caller.
            var removeableOptions = from o in Options
                                    where (o is HelpOption && !settings.DefaultOptions.HasFlag(DefaultOption.Help))
                                       || (o is VersionOption && !settings.DefaultOptions.HasFlag(DefaultOption.Version))
                                    select o;

            // .ToArray forces duplication of the enumeration to prevent exception from modifying
            // the underlying list while enumerating.
            foreach(var o in removeableOptions.ToArray())
            {
                Options.Remove( o );
            }

            // RootCommand constructor adds the "SuggestDirective" directive.
            if(!settings.DefaultDirectives.HasFlag( DefaultDirective.Suggest ))
            {
                // Remove default added and start clean.
                Directives.Clear();
            }

            // Add additional directives based on app controlled settings
            if(settings.DefaultDirectives.HasFlag( DefaultDirective.Diagram ))
            {
                Add( new DiagramDirective() );
            }

            if(settings.DefaultDirectives.HasFlag( DefaultDirective.EnvironmentVariables ))
            {
                Add( new EnvironmentVariablesDirective() );
            }
        }
    }
}

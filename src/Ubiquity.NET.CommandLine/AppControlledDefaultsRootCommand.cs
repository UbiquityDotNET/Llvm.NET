// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Extension of <see cref="RootCommand"/> that allows app control of defaults that are otherwise forced</summary>
    /// <remarks>
    /// This type is derived from <see cref="RootCommand"/> and offers no additional behavior beyond the construction.
    /// The constructor will adapt the command based on the <see cref="CmdLineSettings"/> provided. This moves the
    /// hard coded defaults into an app controlled domain. The default constructed settings matches the behavior of
    /// <see cref="RootCommand"/> so there's no distinction. This allows an application to explicitly decide the behavior
    /// and support of various defaults that could otherwise surprise the author/user. This is especially important when
    /// replacing the internal command line handling of a published app or otherwise creating a "drop-in" replacement. In
    /// such cases, strict adherence to back-compat is of paramount importance and the addition of default behavior is
    /// potentially a breaking change.
    /// </remarks>
    [SuppressMessage( "Design", "CA1010:Generic interface should also be implemented", Justification = "Collection initialization" )]
    public class AppControlledDefaultsRootCommand
        : RootCommand
    {
        /// <summary>Initializes a new instance of the <see cref="AppControlledDefaultsRootCommand"/> class.</summary>
        /// <param name="description">Description of this root command</param>
        /// <param name="settings">Settings to apply for the command parsing</param>
        public AppControlledDefaultsRootCommand( CmdLineSettings settings, string description = "" )
            : base( description )
        {
            ArgumentNullException.ThrowIfNull(settings);

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

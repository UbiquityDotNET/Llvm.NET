// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Parsing;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Flags to determine the default Options for an <see cref="AppControlledDefaultsRootCommand"/></summary>
    [Flags]
    public enum DefaultOption
    {
        /// <summary>No default options used</summary>
        None = 0,

        /// <summary>Include the default help option</summary>
        Help,

        /// <summary>Include the default version option</summary>
        Version,
    }

    /// <summary>Flags to determine the default directives supported for an <see cref="AppControlledDefaultsRootCommand"/></summary>
    [Flags]
    public enum DefaultDirective
    {
        /// <summary>No default directives included</summary>
        None = 0,

        /// <summary>Include support for <see cref="SuggestDirective"/></summary>
        Suggest,

        /// <summary>Include support for <see cref="DiagramDirective"/></summary>
        Diagram,

        /// <summary>Include support for <see cref="EnvironmentVariablesDirective"</summary>
        EnvironmentVariables,
    }

    /// <summary>Contains settings for parsing a command line</summary>
    /// <remarks>
    /// <para>This is effectively an extension to <see cref="ParserConfiguration"/> adding control
    /// of the default options and directives. This is used with <see cref="AppControlledDefaultsRootCommand"/>
    /// to adapt the defaults otherwise forced by <see cref="RootCommand"/>. Of particular interest is the
    /// <see cref="ResponseFileTokenReplacer"/> which, if not set uses the default from
    /// <see cref="ParserConfiguration.ResponseFileTokenReplacer"/> which is not publicly available. If the value
    /// is set to <see langword="null"/> then no replacer is used or supported. If it is set to a non-null value
    /// then that replacer is used.</para>
    /// <para>The default values follows the default behaviors of the underlying library. This ensures the
    /// principle of least surprise while allowing for explicit overrides</para>
    /// </remarks>
    public class CmdLineSettings
    {
        /// <inheritdoc cref="ParserConfiguration.EnablePosixBundling"/>
        public bool EnablePosixBundling { get; set; } = true;

        /// <summary>Gets a value that indicates the default options to include</summary>
        /// <remarks>
        /// Default handling includes <see cref="DefaultOption.Help"/> and <see cref="DefaultOption.Version"/>.
        /// This allows overriding that to specify behavior as needed.
        /// </remarks>
        public DefaultOption DefaultOptions { get; init; } = DefaultOption.Help | DefaultOption.Version;

        /// <summary>Gets a value that indicates the default Directives to include</summary>
        /// <remarks>
        /// Default handling includes <see cref="DefaultDirective.Suggest"/>.
        /// This allows overriding that to specify behavior as needed.
        /// </remarks>
        public DefaultDirective DefaultDirectives { get; init; } = DefaultDirective.Suggest;

        /// <summary>Gets a response file token replacer. [Default: internal common token replacement]</summary>
        /// <remarks>
        /// <para>Unless explicitly set the default behavior, which is not otherwise accessible, is used.</para>
        /// <para>
        /// Any option preceded by a '@' will trigger a call to this delegate if specified. The `tokenToReplace`
        /// parameter to the delegate is the characters following, but not including, the leading '@'. The default
        /// assumes that is a file name containing the command in a `Response` file. The default is normally what
        /// is used but, since it isn't directly accessible, this class helps in expressing the behavior.
        /// </para>
        /// </remarks>
        public TryReplaceToken? ResponseFileTokenReplacer
        {
            get;
            init
            {
                field = value;
                HasCustomeResponseFileBehavior = true;
            }
        }

        /// <summary>Constructs a new <see cref="ParserConfiguration"/> based on this instance</summary>
        /// <returns>new <see cref="ParserConfiguration"/> from this instance</returns>
        public ParserConfiguration ToParserConfiguration( )
        {
            ParserConfiguration retVal = new()
            {
                EnablePosixBundling = EnablePosixBundling,
            };

            // Don't set the behavior unless explicitly specified as the default
            // is not publicly accessible (and therefore cannot be expressed as
            // a value for this type).
            if(HasCustomeResponseFileBehavior)
            {
                retVal.ResponseFileTokenReplacer = ResponseFileTokenReplacer;
            }

            return retVal;
        }

        private bool HasCustomeResponseFileBehavior = false;

        /// <summary>Implicitly constructs a new <see cref="ParserConfiguration"/> based on an instance of <see cref="CmdLineSettings"/></summary>
        /// <param name="self">The settings to build the configuration from</param>
        public static implicit operator ParserConfiguration( CmdLineSettings self )
        {
            return self.ToParserConfiguration();
        }

        /// <summary>Gets a settings with defaults for options and directives removed</summary>
        /// <remarks>
        /// The "default" support for help and version is rather inflexible and an "all or nothing"
        /// approach. (with late bug fix hacks [https://github.com/dotnet/command-line-api/issues/2659]
        /// to resolve issues). Thus, this simply removes them and leaves it to the calling app to
        /// specify them explicitly as a custom option. Then validation is customized to handle
        /// behavior as desired by the app.
        /// </remarks>
        public static CmdLineSettings NoDefaults { get; }
            = new()
            {
                DefaultDirectives = DefaultDirective.None,
                DefaultOptions = DefaultOption.None,
                ResponseFileTokenReplacer = null,
                EnablePosixBundling = false,
            };
    }
}

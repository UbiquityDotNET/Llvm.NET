// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

namespace Ubiquity.NET.CommandLine
{
    /// <summary>Utility class to provide extensions for validation of options and arguments</summary>
    public static class SymbolValidationExtensions
    {
        /// <summary>Fluent function to add a validator to an <see cref="Option"/></summary>
        /// <typeparam name="T">Type of Option to add the validator to</typeparam>
        /// <param name="self">The option to add the validator to</param>
        /// <param name="validator">validator to add</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddValidator<T>( this T self, Action<OptionResult> validator )
            where T : Option
        {
            ArgumentNullException.ThrowIfNull( self );
            self.Validators.Add( validator );
            return self;
        }

        /// <summary>Fluent function to add a validator to an <see cref="Argument"/></summary>
        /// <typeparam name="T">Type of Option to add the validator to</typeparam>
        /// <param name="self">The option to add the validator to</param>
        /// <param name="validator">validator to add</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddValidator<T>( this T self, Action<ArgumentResult> validator )
            where T : Argument
        {
            ArgumentNullException.ThrowIfNull( self );
            self.Validators.Add( validator );
            return self;
        }

        /// <summary>Add validation for an existing file only</summary>
        /// <param name="self">Option to add the validation to</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This is similar to <see cref="OptionValidation.AcceptExistingOnly{T}(Option{T})"/>
        /// but it is constrained to specifically files AND enhances the error message to
        /// provide more details to aid user in correcting the problem.
        /// </remarks>
        public static Option<FileInfo> AcceptExistingFileOnly( this Option<FileInfo> self )
        {
            self.AddValidator( ValidateFileExists );
            return self;
        }

        /// <inheritdoc cref="AcceptExistingFileOnly{T}(Argument{T})"/>
        public static Argument<FileInfo> AcceptExistingFileOnly( this Argument<FileInfo> self )
        {
            self.AddValidator( ValidateFileExists );
            return self;
        }

        /// <summary>Add validation for an existing file only</summary>
        /// <typeparam name="T">Type of value for the argument</typeparam>
        /// <param name="self">Option to add the validation to</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This is similar to <see cref="ArgumentValidation.AcceptExistingOnly{T}(Argument{T})"/>
        /// but it is constrained to specifically files AND enhances the error message to
        /// provide more details to aid user in correcting the problem.
        /// </remarks>
        public static Argument<T> AcceptExistingFileOnly<T>( this Argument<T> self )
            where T : IEnumerable<FileInfo>
        {
            self.AddValidator( ValidateFileExists );
            return self;
        }

        /// <summary>Option validator for an Option's tokens that ensures each represents an existing file</summary>
        /// <param name="result">result to validate</param>
        /// <remarks>
        /// This is simply a parse validation sanity check for common usage errors. It is explicitly NOT
        /// a security check/test!. Apps MUST NOT assume anything about the file beyond the fact that it
        /// existed AT THE TIME this validation ran. It might not exist when needed, might have been replaced
        /// since, or may exist as or been replaced by a link to something else. None of those scenarios is
        /// verified or prevented by this.
        /// </remarks>
        public static void ValidateFileExists( OptionResult result )
        {
            string optionName = result.Option.HelpName ?? result.Option.Name;
            foreach(var token in result.Tokens)
            {
                if(!File.Exists( token.Value ))
                {
                    result.AddError( $"File '{token.Value}' specified for '{optionName}' does not exist" );
                    return;
                }
            }
        }

        /// <summary>Argument validator for an Arguments's tokens that ensures each represents an existing file</summary>
        /// <param name="result">result to validate</param>
        /// <remarks>
        /// This is simply a parse validation sanity check for common usage errors. It is explicitly NOT
        /// a security check/test!. Apps MUST NOT assume anything about the file beyond the fact that it
        /// existed AT THE TIME this validation ran. It might not exist when needed, might have been replaced
        /// since, or may exist as or been replaced by a link to something else. None of those scenarios is
        /// verified or prevented by this.
        /// </remarks>
        public static void ValidateFileExists( ArgumentResult result )
        {
            string optionName = result.Argument.HelpName ?? result.Argument.Name;
            foreach(var token in result.Tokens)
            {
                if(!File.Exists( token.Value ))
                {
                    result.AddError( $"File '{token.Value}' specified for '{optionName}' does not exist" );
                    return;
                }
            }
        }

        /// <summary>Extension method to add the <see cref="ValidateFolderExists(OptionResult)"/></summary>
        /// <param name="self">Option to add the validator for</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static Option<DirectoryInfo> AcceptExistingFolderOnly( this Option<DirectoryInfo> self )
        {
            self.AddValidator( ValidateFolderExists );
            return self;
        }

        /// <summary>Option validator for an Option's tokens that ensures each represents an existing Folder</summary>
        /// <param name="result">result to validate</param>
        /// <remarks>
        /// This is simply a parse validation sanity check for common usage errors. It is explicitly NOT
        /// a security check/test!. Apps MUST NOT assume anything about the folder beyond the fact that it
        /// existed AT THE TIME this validation ran. It might not exist when needed, might have been replaced
        /// since, or may exist as or been replaced by a link to something else later. None of those scenarios
        /// is verified or prevented by this.
        /// </remarks>
        public static void ValidateFolderExists( OptionResult result )
        {
            string optionName = result.Option.HelpName ?? result.Option.Name;
            foreach(var token in result.Tokens)
            {
                if(!Directory.Exists( token.Value ))
                {
                    result.AddError( $"File '{token.Value}' specified for '{optionName}' does not exist" );
                    return;
                }
            }
        }
    }
}

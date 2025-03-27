// -----------------------------------------------------------------------
// <copyright file="IParseErrorLogger.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace Ubiquity.NET.Runtime.Utils
{
    public interface IParseErrorLogger
    {
        void ShowError( ErrorNode node );

        void ShowError( string msg );
    }

    public static class ParseErrorLoggerExtensions
    {
        public static bool CheckAndShowParseErrors(this IParseErrorLogger self, IAstNode node )
        {
            ArgumentNullException.ThrowIfNull(self);

            var errors = node.CollectErrors( );
            if( errors.Count == 0 )
            {
                return false;
            }

            foreach( var err in errors )
            {
                self.ShowError( err );
            }

            return true;
        }
    }
}

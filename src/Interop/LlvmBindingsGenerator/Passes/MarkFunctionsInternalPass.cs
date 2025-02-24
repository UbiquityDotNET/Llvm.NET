// -----------------------------------------------------------------------
// <copyright file="MarkFunctionsIgnoredPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Mark functions as internal or ignored</summary>
    /// <remarks>
    /// This pass will mark all Implicit functions from the headers ignored (e.g. an
    /// implicit constructor/destructor for a struct).
    /// </remarks>
    internal class MarkFunctionsInternalPass
        : TranslationUnitPass
    {
        public override bool VisitFunctionDecl( Function function )
        {
            if( function.Ignore )
            {
                return true;
            }

            if( function.IsImplicit )
            {
                function.Ignore = true;
                return true;
            }

            return false;
        }
    }
}

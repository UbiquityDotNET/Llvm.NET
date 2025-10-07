// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

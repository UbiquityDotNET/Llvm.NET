// -----------------------------------------------------------------------
// <copyright file="PODToValueTypePass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to mark all POD types as a ValueType</summary>
    internal class PODToValueTypePass
        : TranslationUnitPass
    {
        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitClassDecl( Class @class )
        {
            if( base.VisitClassDecl( @class ) )
            {
                return false;
            }

            if( @class.IsPOD )
            {
                @class.Type = ClassType.ValueType;
            }

            return true;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="MapHandleTypeDefsPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator.Passes
{
    // Add TypeMaps for the typedef handles to use the generated SafeHandle derived types
    internal class MapHandleTypeDefsPass
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

        public override bool VisitTypedefDecl( TypedefDecl typedef )
        {
            if( AlreadyVisited( typedef ) )
            {
                return false;
            }

            if( !typedef.IsHandleTypeDef( ) )
            {
                return false;
            }

            TypeMaps.TypeMaps.Add( typedef.Name, new HandleRefTypeMap( typedef, Context, TypeMaps ) );
            return true;
        }
    }
}

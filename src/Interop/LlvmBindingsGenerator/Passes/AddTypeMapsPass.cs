// -----------------------------------------------------------------------
// <copyright file="MapHandleTypeDefsPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    // Add TypeMaps for the typedef handles to use the generated SafeHandle derived types and other well known mappings.
    internal class AddTypeMapsPass
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

            if( typedef.IsHandleTypeDef( ) )
            {
                TypeMaps.TypeMaps.Add( typedef.Name, new HandleRefTypeMap( typedef, Context, TypeMaps ) );
            }
            else if( typedef.Name == "intptr_t" )
            {
                TypeMaps.TypeMaps.Add( typedef.Name, new IntPtrTypeMap( typedef, Context, TypeMaps ) );
            }

            return true;
        }
    }
}

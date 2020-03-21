// -----------------------------------------------------------------------
// <copyright file="IdentifyReduntantConfigurationEntriesPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    internal class IdentifyReduntantConfigurationEntriesPass
        : TranslationUnitPass
    {
        public IdentifyReduntantConfigurationEntriesPass( IGeneratorConfig configuration )
        {
            Configuration = configuration;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            if( !base.VisitASTContext( context ) )
            {
                return false;
            }

            // roll these out so that they ALL run and short circuiting doesn't cut any out
            // that way all errors are identified in a single pass
            var results = new List<bool>( 4 )
            {
                VerifyFunctionBindings( context ),
                VerifyIgnoredHeader( context ),
                VerifyHandleMapEntries( context ),
                VerifyAnonymousEnums( context )
            };

            return results.Aggregate( ( a, b ) => a && b );
        }

        private bool VerifyAnonymousEnums( ASTContext context )
        {
            bool retVal = true;

            foreach( var anonEnum in Configuration.AnonymousEnums )
            {
                var typeDefs = context.FindTypedef( anonEnum.Value ).ToList();
                if( typeDefs.Count == 0 )
                {
                    retVal = false;
                    Diagnostics.Warning( "typedef {0} for anonymous enum not found in source"
                                       , anonEnum.Value
                                       );
                }
                else if( typeDefs.Count > 1)
                {
                    retVal = false;
                    Diagnostics.Warning( "Multiple type definitions for Anonymous enum ({0},{1}) found in source"
                                       , anonEnum.Key
                                       , anonEnum.Value
                                       );
                    foreach(var td in typeDefs)
                    {
                        Diagnostics.Warning( "\t{0} in {1}@{2}", td.Name, td.TranslationUnit.FileRelativePath, td.LineNumberStart );
                    }
                }

                /*At this point: typdefs.Count == 1 && typedefs[0] is the matching type - so good to go on that front */

                var sourceEnums = ( from unit in context.TranslationUnits
                                    let @enum = unit.FindEnumWithItem( anonEnum.Key )
                                    where @enum != null
                                    select @enum
                                  ).ToList();

                if(sourceEnums.Count == 0 )
                {
                    retVal = false;
                    Diagnostics.Warning( "no enum found in source with first item matching {0}"
                                       , anonEnum.Key
                                       );
                }
                else if (sourceEnums.Count > 1)
                {
                    retVal = false;
                    Diagnostics.Warning( "Multiple enums found in source with first item matching {0}"
                                       , anonEnum.Key
                                       );
                    foreach( var e in sourceEnums )
                    {
                        Diagnostics.Warning( "\t{1}@{2}", e.TranslationUnit.FileRelativePath, e.LineNumberStart );
                    }
                }
                else if( !string.IsNullOrEmpty(sourceEnums[0].Name) )
                {
                    retVal = false;
                    var wrongEnum = sourceEnums[0];
                    Diagnostics.Warning( "Found enum {0} in {1}@{2} with value {3} but expected an anonymous enum"
                                       , wrongEnum.Name
                                       , wrongEnum.TranslationUnit.FileRelativePath
                                       , wrongEnum.LineNumberStart
                                       , anonEnum.Key
                                       );
                }
            }

            return retVal;
        }

        private bool VerifyHandleMapEntries( ASTContext context )
        {
            var sourceHandleTypes = new SortedSet<string>( context.GetHandleTypeDefs().Select( td=>td.Name ) );

            var redundantMapEntries = ( from h in Configuration.HandleMap
                                        where !sourceHandleTypes.Contains( h.HandleName )
                                        select h
                                      ).ToList();
            if( redundantMapEntries.Count == 0 )
            {
                return true;
            }

            foreach( var entry in redundantMapEntries )
            {
                Diagnostics.Warning( "Handle type {0} declared at line {1} does not exist in source", entry.HandleName, entry.Start.Line );
            }

            return false;
        }

        private bool VerifyIgnoredHeader( ASTContext context )
        {
            var ignoredHeaders = Configuration.IgnoredHeaders.ToDictionary( n=>n.Path, n=>n.Start );
            var allTranslationUnits = context.ValidUnits().ToDictionary( tu => NormalizePath( tu.FileRelativePath ) );

            var missingHeaders = ( from ignoredHeader in ignoredHeaders
                                   where !allTranslationUnits.ContainsKey( ignoredHeader.Key )
                                   select ignoredHeader
                                 ).ToList();

            if( missingHeaders.Count == 0 )
            {
                return true;
            }

            foreach( var missingHeader in missingHeaders )
            {
                Diagnostics.Warning( "Ignored header {0} not found; It was declared at Line {1} in the configuration but was not found in the source"
                                   , missingHeader.Key
                                   , missingHeader.Value.Line
                                   );
            }

            return false;
        }

        private static string NormalizePath( string path )
        {
            return path.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );
        }

        private bool VerifyFunctionBindings( ASTContext context )
        {
            var configFunctions = Configuration.FunctionBindings.Values.ToDictionary( f=>f.Name, f=>f );

            var allFunctions = ( from unit in context.GeneratedUnits( )
                                 from f in unit.Functions
                                 where !f.Ignore
                                 select f
                               ).Distinct(new FunctionNameComparer())
                                .ToDictionary( f => f.Name );

            var missingFunctions = ( from declaredFunc in configFunctions
                                     where !allFunctions.ContainsKey( declaredFunc.Key )
                                     select declaredFunc
                                   ).ToList();

            if( missingFunctions.Count == 0 )
            {
                return true;
            }

            foreach( var missingFunc in missingFunctions )
            {
                Diagnostics.Warning( "Function {0} not found; It was declared in the configuration at line {1} but was not found in the source"
                                   , missingFunc.Key
                                   , missingFunc.Value.Start.Line
                                   );
            }

            return false;
        }

        private class FunctionNameComparer
            : IEqualityComparer<Function>
        {
            public bool Equals( Function x, Function y ) => string.CompareOrdinal( x.Name, y.Name ) == 0;

            public int GetHashCode( Function obj ) => obj.Name.GetHashCode( );
        }

        private readonly IGeneratorConfig Configuration;
    }
}

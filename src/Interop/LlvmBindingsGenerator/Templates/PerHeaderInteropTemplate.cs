// -----------------------------------------------------------------------
// <copyright file="PerHeaderInteropTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class PerHeaderInteropTemplate
        : ICodeGenTemplate
    {
        public PerHeaderInteropTemplate( TranslationUnit tu )
        {
            Unit = tu;
        }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string FileExtension => "cs";

        public string Generate( )
        {
            return TransformText( );
        }

        public ISet<string> Imports
        {
            get
            {
                var results = new SortedSet<string>( ) { "System.CodeDom.Compiler" };

                foreach(var f in Unit.Functions )
                {
                    foreach( var a in f.Attributes )
                    {
                        results.Add( a.Type.Namespace );
                    }

                    foreach( var p in f.Parameters )
                    {
                        foreach( var pa in p.Attributes )
                        {
                            results.Add( pa.Type.Namespace );
                        }
                    }
                }

                foreach( var d in Delegates )
                {
                    foreach( var da in d.Attributes )
                    {
                        results.Add( da.Type.Namespace );
                    }
                }

                foreach( var e in Unit.Enums )
                {
                    foreach( var ea in e.Attributes )
                    {
                        results.Add( ea.Type.Namespace );
                    }
                }

                return results;
            }
        }

        public IEnumerable<ParsedEnum> Enums
            => from e in Unit.Enums
               select new ParsedEnum( e );

        public IEnumerable<ParsedFunctionSignature> Functions
            => from f in Unit.Functions
               where f.IsGenerated
               select new ParsedFunctionSignature( f );

        public IEnumerable<ParsedFunctionSignature> Delegates
            => from td in Unit.Typedefs
               where td.IsDelegateTypeDef( )
               select new ParsedFunctionSignature( td );

        public IEnumerable<Class> ValueTypes
            => from cls in Unit.Classes
               where cls.IsValueType
               select cls;

        public TranslationUnit Unit { get; }

        private static string GetTypeName( CppSharp.AST.Type type ) => ParsedFunctionSignature.GetTypeName( type );
    }
}

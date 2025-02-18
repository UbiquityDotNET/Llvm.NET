// -----------------------------------------------------------------------
// <copyright file="PerHeaderInteropTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class PerHeaderInteropTemplate
        : ICodeGenTemplate
    {
        public PerHeaderInteropTemplate( TranslationUnit tu, ITypePrinter2 typePrinter )
        {
            Unit = tu;
            TypePrinter = typePrinter;
        }

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( )
        {
            return TransformText( );
        }

        public ISet<string> Imports
        {
            get
            {
                var results = new SortedSet<string>( ) { "System", "System.CodeDom.Compiler" };

                foreach( var f in Unit.Functions )
                {
                    foreach( var a in f.Attributes )
                    {
                        results.Add( a.Type.Namespace );
                    }

                    foreach( var p in f.Parameters )
                    {
                       AddNamespacesForAttributes( results, p.Attributes );
                    }
                }

                foreach( var d in Delegates )
                {
                    AddNamespacesForAttributes( results, d.Attributes );
                }

                foreach( var e in Unit.Enums )
                {
                    AddNamespacesForAttributes( results, e.Attributes );
                }

                foreach(var st in Unit.Classes.Where(cls=>cls.IsValueType))
                {
                    AddNamespacesForAttributes( results, st.Attributes);
                    foreach(var fld in st.Fields)
                    {
                        AddNamespacesForAttributes( results, fld.Attributes);
                    }
                }

                return results;
            }
        }

        public string GetManagedName(Type t)
        {
            return TypePrinter.GetName(t, TypeNameKind.Managed);
        }

        public IEnumerable<ParsedEnum> Enums
            => from e in Unit.Enums
               select new ParsedEnum( e );

        public IEnumerable<ParsedFunctionSignature> Functions
            => from f in Unit.Functions
               where f.IsGenerated
               select new ParsedFunctionSignature( f, TypePrinter );

        public IEnumerable<ParsedFunctionSignature> Delegates
            => from td in Unit.Typedefs
               where td.IsDelegateTypeDef( )
               select new ParsedFunctionSignature( td, TypePrinter );

        public IEnumerable<Class> ValueTypes
            => from cls in Unit.Classes
               where cls.IsValueType
               select cls;

        public TranslationUnit Unit { get; }

        public bool HasNativeMethods => Unit.Functions.Any(f=>f.IsGenerated);

        private static void AddNamespacesForAttributes(SortedSet<string> results, IEnumerable<Attribute> attribs)
        {
            foreach(var attrib in attribs)
            {
                results.Add( attrib.Type.Namespace );
            }
        }

        private readonly ITypePrinter2 TypePrinter;
    }
}

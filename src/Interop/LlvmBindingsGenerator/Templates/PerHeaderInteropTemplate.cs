// -----------------------------------------------------------------------
// <copyright file="PerHeaderInteropTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

using CppSharp.AST;
using CppSharp.AST.Extensions;

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
                var results = new SortedSet<string>( ) { "System" };

                foreach( var f in Unit.Functions )
                {
                    AddNamespacesForAttributes(results, f.Attributes);

                    foreach( var p in f.Parameters )
                    {
                       AddNamespacesForAttributes( results, p.Attributes );
                    }
                }

                foreach( var e in Unit.Enums )
                {
                    results.Add("System.CodeDom.Compiler"); // needed for the "GeneratedCode" attribute om the template
                    AddNamespacesForAttributes( results, e.Attributes );
                }

                foreach(var st in Unit.Classes.Where(cls=>cls.IsValueType))
                {
                    results.Add("System.CodeDom.Compiler"); // needed for the "GeneratedCode" attribute om the template
                    AddNamespacesForAttributes( results, st.Attributes);
                    foreach(var fld in st.Fields)
                    {
                        AddNamespacesForAttributes( results, fld.Attributes);
                    }
                }

                return results;
            }
        }

        public void EOL() => Write(Environment.NewLine);

        public static Int64 InlinedArraySize(Field f)
        {
            return f.Type is ArrayType at && at.SizeType == ArrayType.ArraySize.Constant
                ? at.Size
                : throw new ArgumentException("Expected a field with a fixed size array", nameof(f));
        }

        public string GetManagedTypeName(Field f)
        {
            return f.IsInlinedArray()
                ? $"{f.Class.Name}_{f.Name}_t"
                : GetManagedName(f.Type);
        }

        public string GetInlinedArrayElementType(Field f)
        {
            return f.Type is ArrayType at && at.SizeType == ArrayType.ArraySize.Constant
                 ? GetManagedName(at.Type)
                 : throw new ArgumentException("Expected a field with a fixed size array", nameof(f));
        }

        public string GetManagedName(CppSharp.AST.Type t)
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

        public IEnumerable<ParsedFunctionSignature> FunctionPointers
            => (from f in Unit.Functions
                where f.IsGenerated
                from param in f.Parameters
                where param.Type is TypedefType tdt && IsFunctionPointer( tdt )
                select GetFunctionPointerType( (TypedefType)param.Type )
               ).Distinct( new TypeDefNameComparer() )
                .Select( (f) => new ParsedFunctionSignature( f, TypePrinter ) );

        public IEnumerable<Class> ValueTypes
            => from cls in Unit.Classes
               where cls.IsValueType
               select cls;

        public IEnumerable<Field> InlinedArrayFields
            => from st in ValueTypes
               from field in st.Fields
               where field.IsInlinedArray()
               select field;

        public TranslationUnit Unit { get; }

        public bool HasNativeMethods => Unit.Functions.Any(f=>f.IsGenerated);

        private static bool IsFunctionPointer(TypedefType tdt)
        {
            return tdt.Declaration.Type.IsPointerTo<FunctionType>(out FunctionType _);
        }

        private static TypedefNameDecl GetFunctionPointerType(TypedefType tdt)
        {
            return !tdt.Declaration.Type.IsPointerTo<FunctionType>(out FunctionType _)
                ? throw new ArgumentException("Can't find function type!", nameof(tdt))
                : tdt.Declaration;
        }

        private static void AddNamespacesForAttributes(SortedSet<string> results, IEnumerable<CppSharp.AST.Attribute> attribs)
        {
            foreach(var attrib in attribs)
            {
                results.Add( attrib.Type.Namespace );
                if (attrib.Type.Name == nameof(UnmanagedCallConvAttribute))
                {
                    results.Add("System.Runtime.CompilerServices");
                }
            }
        }

        private readonly ITypePrinter2 TypePrinter;

        private class TypeDefNameComparer
            : IEqualityComparer<TypedefNameDecl>
        {
            public bool Equals(TypedefNameDecl x, TypedefNameDecl y) => x.Name.Equals(y.Name, StringComparison.Ordinal);

            public int GetHashCode([DisallowNull] TypedefNameDecl obj) => obj.Name.GetHashCode(StringComparison.Ordinal);
        }
    }
}

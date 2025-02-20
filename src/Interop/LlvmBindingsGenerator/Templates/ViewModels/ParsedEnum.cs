// -----------------------------------------------------------------------
// <copyright file="ParsedEnum.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal class ParsedEnum
    {
        public ParsedEnum( Enumeration e )
        {
            Name = !string.IsNullOrWhiteSpace( e.Name ) ? e.Name : throw new ArgumentException( "Enum name cannot be blank" );
            BaseType = GetBaseTypeName( e.BuiltinType );
            Members = from i in e.Items
                      select (i.Name, e.GetItemValueAsString( i ));
        }

        public string Name { get; }

        public string BaseType { get; }

        public IEnumerable<(string Name, string Value)> Members { get; }

        private static string GetBaseTypeName( BuiltinType t )
        {
            return t.Type switch
            {
                PrimitiveType.Short => "Int16",
                PrimitiveType.UShort => "UInt16",
                PrimitiveType.Int or PrimitiveType.Long => "Int32",
                PrimitiveType.ULong or PrimitiveType.UInt => "UInt32",
                PrimitiveType.LongLong => "Int64",
                PrimitiveType.ULongLong => "UInt64",

                // PrimitiveType.Int128:
                // PrimitiveType.UInt128:
                // PrimitiveType.Half:
                // PrimitiveType.Float:
                // PrimitiveType.Double:
                // PrimitiveType.LongDouble:
                // PrimitiveType.Float128:
                // PrimitiveType.IntPtr:
                // PrimitiveType.UIntPtr:
                // PrimitiveType.String:
                // PrimitiveType.Decimal:
                // PrimitiveType.Null:
                // PrimitiveType.Void:
                // PrimitiveType.Bool:
                // PrimitiveType.WideChar:
                // PrimitiveType.Char:
                // PrimitiveType.SChar:
                // PrimitiveType.UChar:
                // PrimitiveType.Char16:
                // PrimitiveType.Char32:
                _ => throw new ArgumentException( "Unsupported enum base type", nameof( t ) ),
            };
        }
    }
}

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
            Comments = new ParsedComment( e );
            Name = !string.IsNullOrWhiteSpace( e.Name ) ? e.Name : throw new ArgumentException("Enum name cannot be blank");
            BaseType = GetBaseTypeName( e.BuiltinType );
            Members = from i in e.Items
                      select (i.Name, e.GetItemValueAsString( i ), new ParsedComment( i ));
        }

        public ParsedComment Comments { get; }

        public string Name { get; }

        public string BaseType { get; }

        public IEnumerable<(string Name, string Value, ParsedComment Comment)> Members { get; }

        private string GetBaseTypeName( BuiltinType t )
        {
            switch( t.Type )
            {
            case PrimitiveType.Short:
                return "global::System.Int16";

            case PrimitiveType.UShort:
                return "global::System.UInt16";

            case PrimitiveType.Int:
            case PrimitiveType.Long:
                return "global::System.Int32";

            case PrimitiveType.ULong:
            case PrimitiveType.UInt:
                return "global::System.UInt32";

            case PrimitiveType.LongLong:
                return "global::System.Int64";

            case PrimitiveType.ULongLong:
                return "global::System.UInt64";

            // case PrimitiveType.Int128:
            // case PrimitiveType.UInt128:
            // case PrimitiveType.Half:
            // case PrimitiveType.Float:
            // case PrimitiveType.Double:
            // case PrimitiveType.LongDouble:
            // case PrimitiveType.Float128:
            // case PrimitiveType.IntPtr:
            // case PrimitiveType.UIntPtr:
            // case PrimitiveType.String:
            // case PrimitiveType.Decimal:
            // case PrimitiveType.Null:
            // case PrimitiveType.Void:
            // case PrimitiveType.Bool:
            // case PrimitiveType.WideChar:
            // case PrimitiveType.Char:
            // case PrimitiveType.SChar:
            // case PrimitiveType.UChar:
            // case PrimitiveType.Char16:
            // case PrimitiveType.Char32:
            default:
                throw new ArgumentException("Unsupported enum base type", nameof(t) );
            }
        }
    }
}

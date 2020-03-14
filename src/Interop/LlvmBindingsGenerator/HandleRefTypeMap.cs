// -----------------------------------------------------------------------
// <copyright file="HandleRefTypeMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Types;

namespace LlvmBindingsGenerator
{
    // DO NOT apply the TypeMap attribute here as that triggers auto inclusion in the
    // TypeMapDataBase and these must be constructed at run time to provide the correct name
    internal class HandleRefTypeMap
        : TypeMap
    {
        public HandleRefTypeMap( TypedefNameDecl td, BindingContext ctx, TypeMapDatabase dataBase )
        {
            Context = ctx;
            Type = td.Type;
            TypeMapDatabase = dataBase;
            InjectedType = new TagType( new Class( )
            {
                Name = td.Name,
                Namespace = td.Namespace
            } );
        }

        public override bool DoesMarshalling => false;

        public override Type CLISignatureType( TypePrinterContext ctx )
        {
            return InjectedType;
        }

        private readonly Type InjectedType;
    }
}

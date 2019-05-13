// -----------------------------------------------------------------------
// <copyright file="IntPtrTypeMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Types;

namespace LlvmBindingsGenerator
{
    internal class IntPtrTypeMap
        : TypeMap
    {
        public IntPtrTypeMap( TypedefNameDecl td, BindingContext ctx, TypeMapDatabase dataBase )
        {
            Type = td.Type;
            Context = ctx;
            TypeMapDatabase = dataBase;
            InjectedType = new CILType( typeof( IntPtr ) );
        }

        public override bool DoesMarshalling => false;

        public override CppSharp.AST.Type CLISignatureType( TypePrinterContext ctx )
        {
            return InjectedType;
        }

        private readonly CppSharp.AST.Type InjectedType;
    }
}

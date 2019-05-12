// -----------------------------------------------------------------------
// <copyright file="MarshalingInfoMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace LlvmBindingsGenerator
{
    internal interface IMarshalInfo
    {
        string FunctionName { get; }

        string ParameterName { get; }

        uint ParameterIndex { get; set; }

        ParamSemantics Semantics { get; }

        IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        CppSharp.AST.QualifiedType TransformType( CppSharp.AST.QualifiedType type );
    }
}

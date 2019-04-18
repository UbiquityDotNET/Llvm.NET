// -----------------------------------------------------------------------
// <copyright file="MarshalingInfoMap.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace LlvmBindingsGenerator
{
    internal interface IMarshalInfo
    {
        string FunctionName { get; }

        string ParameterName { get; }

        uint ParameterIndex { get; set;  }

        ParamSemantics Semantics { get; }

        CppSharp.AST.Type Type { get; }

        IEnumerable<CppSharp.AST.Attribute> Attributes { get; }
    }
}

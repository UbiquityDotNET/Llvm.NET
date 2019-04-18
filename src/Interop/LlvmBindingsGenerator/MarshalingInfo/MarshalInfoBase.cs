// -----------------------------------------------------------------------
// <copyright file="MarshalInfoBase.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;

namespace LlvmBindingsGenerator
{
    [DebuggerDisplay("{FunctionName}({Semantics}: {ParameterName}[{ParameterIndex}])")]
    internal abstract class MarshalInfoBase
        : IMarshalInfo
    {
        public string FunctionName { get; }

        public string ParameterName { get; }

        public uint ParameterIndex { get; set; }

        public ParamSemantics Semantics { get; }

        public abstract CppSharp.AST.Type Type { get; }

        public abstract IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        protected MarshalInfoBase( string functionName, string paramName, ParamSemantics semantics )
        {
            FunctionName = functionName;
            ParameterName = paramName;
            ParameterIndex = semantics == ParamSemantics.Return ? MarshalingInfoMap.ReturnParamIndex : MarshalingInfoMap.UnresolvedParamIndex;
            Semantics = semantics;
        }
    }
}

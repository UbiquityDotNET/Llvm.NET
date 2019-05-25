// -----------------------------------------------------------------------
// <copyright file="MarshalingInfoMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace LlvmBindingsGenerator
{
    /// <summary>Interface for marshaling information for function parameters and return</summary>
    internal interface IMarshalInfo
    {
        /// <summary>Gets the name of the function this info applies to</summary>
        string FunctionName { get; }

        /// <summary>Gets the parameter name this info applies to</summary>
        string ParameterName { get; }

        /// <summary>Gets or sets the parameter index this info applies to</summary>
        /// <remarks>
        /// This is ordinarily <see cref="MarshalingInfoMap.UnresolvedParamIndex"/> at
        /// time of construction. Later, during the AddMarshalingAttributesPass the
        /// index is resolved from the function signature.
        /// </remarks>
        uint ParameterIndex { get; set; }

        /// <summary>Gets the semantics for the parameter</summary>
        ParamSemantics Semantics { get; }

        /// <summary>Gets a collection of the attributes required for marshaling the parameter or return</summary>
        IEnumerable<CppSharp.AST.Attribute> Attributes { get; }

        /// <summary>Transforms the declared type of a parameter or return into the required interop type</summary>
        /// <param name="type">QUalified type of the parameter or function return</param>
        /// <returns>Transformed type for subsequent passes and code generation to use</returns>
        CppSharp.AST.QualifiedType TransformType( CppSharp.AST.QualifiedType type );
    }
}

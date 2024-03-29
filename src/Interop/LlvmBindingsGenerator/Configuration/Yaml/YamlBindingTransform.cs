﻿// -----------------------------------------------------------------------
// <copyright file="MarshalInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;

using CppSharp.AST;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LlvmBindingsGenerator.Configuration
{
    internal enum ParamSemantics
    {
        Return, // indicates a function return rather than an actual parameter
        In,
        Out,
        InOut
    }

    internal abstract class YamlBindingTransform
        : IYamlNodeLocation
    {
        public const uint ReturnParamIndex = uint.MaxValue;

        public ParamSemantics Semantics { get; set; } = ParamSemantics.In; // Ignored for return marshaling

        public string Name { get; set; } // ignored for return marshaling

        public bool IsAlias { get; set; } // only applies to return marshaling

        public bool IsUnsafe { get; set; } // only applies to return marshaling

        public uint? ParameterIndex { get; set; }

        [YamlIgnore]
        public abstract IEnumerable<Attribute> Attributes { get; }

        [YamlIgnore]
        public Mark Start { get; set; }

        public virtual QualifiedType TransformType( CppSharp.AST.QualifiedType type ) => type;

        public static readonly CppSharp.AST.Attribute InAttribute = new TargetedAttribute( typeof( InAttribute ) );
        public static readonly CppSharp.AST.Attribute OutAttribute = new TargetedAttribute( typeof( OutAttribute ) );
    }
}

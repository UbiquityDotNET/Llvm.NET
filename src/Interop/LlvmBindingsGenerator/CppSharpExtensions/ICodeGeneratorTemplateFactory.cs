// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using CppSharp.Generators;

namespace LlvmBindingsGenerator
{
    internal interface ICodeGeneratorTemplateFactory
    {
        IEnumerable<ICodeGenerator> CreateTemplates( BindingContext bindingContext, Options options );

        void SetupPasses( BindingContext bindingContext );
    }
}

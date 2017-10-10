// <copyright file="X64Details.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET;
using Llvm.NET.Types;
using Llvm.NET.Values;

namespace TestDebugInfo
{
    internal class X64Details
        : ITargetDependentDetails
    {
        public string Cpu => "x86-64";

        public string Features => "+sse,+sse2";

        public string ShortName => "x86";

        public string Triple => "x86_64-pc-windows-msvc18.0.0";

        public void AddABIAttributesForByValueStructure( Function function, int paramIndex )
        {
            var argType = function.Parameters[ paramIndex ].NativeType as IPointerType;
            if( argType == null || !argType.ElementType.IsStruct )
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
            }
        }

        public void AddModuleFlags( NativeModule module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
        }

        public IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx )
            => new List<AttributeValue>
            {
                ctx.CreateAttribute("disable-tail-calls", "false" ),
                ctx.CreateAttribute( "less-precise-fpmad", "false" ),
                ctx.CreateAttribute( "no-frame-pointer-elim", "false" ),
                ctx.CreateAttribute( "no-infs-fp-math", "false" ),
                ctx.CreateAttribute( "no-nans-fp-math", "false" ),
                ctx.CreateAttribute( "stack-protector-buffer-size", "8" ),
                ctx.CreateAttribute( "target-cpu", Cpu ),
                ctx.CreateAttribute( "target-features", Features ),
                ctx.CreateAttribute( "unsafe-fp-math", "false" ),
                ctx.CreateAttribute( "use-soft-float", "false" ),
                ctx.CreateAttribute( AttributeKind.UWTable ),
            };
    }
}

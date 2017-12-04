// <copyright file="X64Details.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET;
using Llvm.NET.Types;
using Llvm.NET.Values;

using static Llvm.NET.StaticState;

namespace TestDebugInfo
{
    internal class X64Details
        : ITargetDependentDetails
    {
        public X64Details()
        {
            RegisterX86( );
        }

        public string ShortName => "x86";

        public TargetMachine TargetMachine => TargetMachine.FromTriple( new Triple( TripleName )
                                                                      , Cpu
                                                                      , Features
                                                                      , CodeGenOpt.Aggressive
                                                                      , Reloc.Default
                                                                      , CodeModel.Small
                                                                      );

        public void AddABIAttributesForByValueStructure( Function function, int paramIndex )
        {
            var argType = function.Parameters[ paramIndex ].NativeType as IPointerType;
            if( argType == null || !argType.ElementType.IsStruct )
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
            }
        }

        public void AddModuleFlags( BitcodeModule module )
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

        private static string Cpu = "x86-64";
        private static string Features = "+sse,+sse2";
        private static string TripleName = "x86_64-pc-windows-msvc18.0.0";
    }
}

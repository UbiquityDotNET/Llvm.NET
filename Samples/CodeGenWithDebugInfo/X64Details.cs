// -----------------------------------------------------------------------
// <copyright file="X64Details.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

namespace CodeGenWithDebugInfo
{
    internal class X64Details
        : ITargetDependentDetails
    {
        public X64Details( ILibLlvm libLLVM )
        {
            libLLVM.RegisterTarget( CodeGenTarget.X86 );
        }

        public string ShortName => "x86";

        public TargetMachine CreateTargetMachine( )
        {
            using var triple = new Triple( TripleName );
            return TargetMachine.FromTriple( triple
                                           , Cpu
                                           , Features
                                           , CodeGenOpt.Aggressive
                                           , RelocationMode.Default
                                           , CodeModel.Small
                                           );
        }

        public void AddABIAttributesForByValueStructure( Function function, int paramIndex )
        {
            if(function.Parameters[ paramIndex ].NativeType is not IPointerType ptrType || ptrType.IsOpaque || !ptrType.ElementType!.IsStruct)
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
            }
        }

        public void AddModuleFlags( Module module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
        }

        public IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( IContext ctx )
            => [
                ctx.CreateAttribute( "disable-tail-calls", "false" ),
                ctx.CreateAttribute( "less-precise-fpmad", "false" ),
                ctx.CreateAttribute( "no-frame-pointer-elim", "false" ),
                ctx.CreateAttribute( "no-infs-fp-math", "false" ),
                ctx.CreateAttribute( "no-nans-fp-math", "false" ),
                ctx.CreateAttribute( "stack-protector-buffer-size", "8" ),
                ctx.CreateAttribute( "target-cpu", Cpu ),
                ctx.CreateAttribute( "target-features", Features ),
                ctx.CreateAttribute( "unsafe-fp-math", "false" ),
                ctx.CreateAttribute( "use-soft-float", "false" ),
                ctx.CreateAttribute( AttributeKind.UWTable )
            ];

        private const string Cpu = "x86-64";
        private const string Features = "+sse,+sse2";
        private const string TripleName = "x86_64-pc-windows-msvc18.0.0";
    }
}

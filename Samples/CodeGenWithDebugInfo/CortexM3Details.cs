// -----------------------------------------------------------------------
// <copyright file="CortexM3Details.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

namespace CodeGenWithDebugInfo
{
    internal class CortexM3Details
        : ITargetDependentDetails
    {
        public CortexM3Details( ILibLlvm libLLVM )
        {
            libLLVM.RegisterTarget( CodeGenTarget.ARM );
        }

        public string ShortName => "M3";

        public TargetMachine TargetMachine => TargetMachine.FromTriple( new Triple( TripleName )
                                                                      , Cpu
                                                                      , Features
                                                                      , CodeGenOpt.Aggressive
                                                                      , RelocationMode.Default
                                                                      , CodeModel.Small
                                                                      );

        public void AddABIAttributesForByValueStructure( IrFunction function, int paramIndex )
        {
            // ByVal pointers indicate by value semantics. The actual LLVM semantics are along the lines of
            // "pass the arg as copy on the arguments stack and set parameter implicitly to that copy's address"
            // (src: https://github.com/ldc-developers/ldc/issues/937 ) [e.g. caller copies byval args]
            //
            // LLVM recognizes this pattern and has a pass to map to an efficient register usage whenever plausible.
            // Though it seems Clang doesn't apply the attribute in all cases, for x86 it doesn't appear to ever use
            // it, for Cortex-Mx it seems to use it only for larger structs, otherwise it uses an [ n x i32]. (Max
            // value of n is not known) and performs casts. Thus, on cortex-m the function parameters are handled
            // quite differently by clang, which seems odd to put such target dependent differences into the front-end.
            // Sadly, the ABI calling convention details are left to the source generator so each one needs to know
            // ALL the gory details of the ABI. [There is some work to generalize what Clang does and pull that down
            // to LLVM proper, but that hasn't materialized yet...]
            if( function.Parameters[ paramIndex ].NativeType is not IPointerType ptrType || ptrType.IsOpaque || !ptrType.ElementType!.IsStruct )
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure that is NOT opaque" );
            }

            var layout = function.ParentModule.Layout;
            function.AddAttributes( FunctionAttributeIndex.Parameter0 + paramIndex
                                  , function.Context.CreateAttribute( AttributeKind.ByVal )
                                  , function.Context.CreateAttribute( AttributeKind.Alignment, layout.AbiAlignmentOf( ptrType.ElementType! ) )
                                  );
        }

        public void AddModuleFlags( BitcodeModule module )
        {
            // Specify ABI const sizes so linker can detect mismatches
            module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size", 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size", 4 );
        }

        public IEnumerable<AttributeValue> BuildTargetDependentFunctionAttributes( Context ctx )
            =>
            [
                ctx.CreateAttribute( "correctly-rounded-divide-sqrt-fp-math", "false" ),
                ctx.CreateAttribute( "disable-tail-calls", "false" ),
                ctx.CreateAttribute( "less-precise-fpmad", "false" ),
                ctx.CreateAttribute( "no-frame-pointer-elim", "true" ),
                ctx.CreateAttribute( "no-frame-pointer-elim-non-leaf" ),
                ctx.CreateAttribute( "no-infs-fp-math", "false" ),
                ctx.CreateAttribute( "no-jump-tables", "false" ),
                ctx.CreateAttribute( "no-nans-fp-math", "false" ),
                ctx.CreateAttribute( "no-signed-zeros-fp-math", "false" ),
                ctx.CreateAttribute( "no-trapping-math", "false" ),
                ctx.CreateAttribute( "stack-protector-buffer-size", "8" ),
                ctx.CreateAttribute( "target-cpu", Cpu ),
                ctx.CreateAttribute( "target-features", Features ),
                ctx.CreateAttribute( "unsafe-fp-math", "false" ),
                ctx.CreateAttribute( "use-soft-float", "false" )
            ];

        private const string Cpu = "cortex-m3";
        private const string Features = "+hwdiv,+strict-align,+thumb-mode";
        private const string TripleName = "thumbv7m-none--eabi";
    }
}

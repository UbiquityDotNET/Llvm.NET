// -----------------------------------------------------------------------
// <copyright file="CortexM3ABI.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

namespace CodeGenWithDebugInfo
{
    internal sealed class CortexM3ABI
        : ITargetABI
    {
        public CortexM3ABI( ILibLlvm library)
        {
            library.RegisterTarget( CodeGenTarget.ARM );
        }

        public string ShortName => "M3";

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

        public void AddAttributesForByValueStructure( Function function, DebugFunctionType debugSig, int paramIndex )
        {
            ArgumentNullException.ThrowIfNull( function );
            ArgumentNullException.ThrowIfNull( debugSig );
            ArgumentOutOfRangeException.ThrowIfNotEqual( debugSig.ParameterTypes.Count, function.Parameters.Count );
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual( paramIndex, function.Parameters.Count );

            // ByVal pointers indicate by value semantics. The actual LLVM semantics are along the lines of
            // "pass the arg as copy on the arguments stack and set parameter implicitly to that copy's address"
            // (src: https://github.com/ldc-developers/ldc/issues/937 ) [e.g. caller copies byval args]
            //
            // LLVM recognizes this pattern and has a pass to map to an efficient register usage whenever plausible.
            // Though it seems Clang doesn't apply the attribute in all cases, it's ABI dependent, for x86 it doesn't
            // appear to ever use it, for Cortex-Mx it seems to use it only for larger structs, otherwise it uses an
            // [ n x i32]. (Max value of n is, again, ABI dependent) and performs casts. Thus, on cortex-m the function
            // parameters are handled quite differently by Clang, which seems odd to put such target dependent differences
            // into the front-end. Sadly, the ABI calling convention details are left to the source generator so each one
            // needs to know ALL the gory details of the ABI. [There is some work to generalize what Clang does and pull
            // that down to LLVM proper, but that hasn't materialized yet...]
            DebugPointerType? ptrType = debugSig.ParameterTypes[ paramIndex ] is DebugType<DebugPointerType, DIDerivedType> debugType
                                        ? debugType.NativeType
                                        : null;

            if(ptrType is null || ptrType.IsOpaque() || !ptrType.ElementType!.IsStruct)
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
            }

            var layout = function.ParentModule.Layout;
            function.AddAttributes( FunctionAttributeIndex.Parameter0 + paramIndex
                                  , function.Context.CreateAttribute( "byval"u8, ptrType.ElementType )
                                  , function.Context.CreateAttribute( "align"u8, layout.AbiAlignmentOf( ptrType.ElementType! ) )
                                  );
        }

        public void AddModuleFlags( Module module )
        {
            // Specify ABI const sizes so linker can detect mismatches
            module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size"u8, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size"u8, 4 );
        }

        public ImmutableArray<AttributeValue> BuildTargetDependentFunctionAttributes( IContext ctx )
            => [
                ctx.CreateAttribute( "correctly-rounded-divide-sqrt-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "disable-tail-calls"u8, "false"u8 ),
                ctx.CreateAttribute( "less-precise-fpmad"u8, "false"u8 ),
                ctx.CreateAttribute( "no-frame-pointer-elim"u8, "true"u8 ),
                ctx.CreateAttribute( "no-frame-pointer-elim-non-leaf"u8 ),
                ctx.CreateAttribute( "no-infs-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "no-jump-tables"u8, "false"u8 ),
                ctx.CreateAttribute( "no-nans-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "no-signed-zeros-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "no-trapping-math"u8, "false"u8 ),
                ctx.CreateAttribute( "stack-protector-buffer-size"u8, "8"u8 ),
                ctx.CreateAttribute( "target-cpu"u8, Cpu ),
                ctx.CreateAttribute( "target-features"u8, Features ),
                ctx.CreateAttribute( "unsafe-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "use-soft-float"u8, "false"u8 )
            ];

        // Sadly, these can't be utf8 literals, but, they can be static readonly LazyEncodedString!
        private static readonly LazyEncodedString Cpu = "cortex-m3"u8;
        private static readonly LazyEncodedString Features = "+hwdiv,+strict-align,+thumb-mode"u8;
        private static readonly LazyEncodedString TripleName = "thumbv7m-none--eabi"u8;
    }
}

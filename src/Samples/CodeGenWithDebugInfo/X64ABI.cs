// -----------------------------------------------------------------------
// <copyright file="X64ABI.cs" company="Ubiquity.NET Contributors">
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
    internal sealed class X64ABI
        : ITargetABI
    {
        public X64ABI( )
        {
            LlvmLib = Library.InitializeLLVM();
            LlvmLib.RegisterTarget( CodeGenTarget.X86 );
        }

        public string ShortName => "x86";

        public void Dispose( )
        {
            LlvmLib.Dispose();
        }

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
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual( paramIndex, function.Parameters.Count );

            DebugPointerType? ptrType = debugSig.ParameterTypes[ paramIndex ] is DebugType<DebugPointerType, DIDerivedType> debugType
                                        ? debugType.NativeType
                                        : null;

            if(ptrType is null || ptrType.IsOpaque() || !ptrType.ElementType!.IsStruct)
            {
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
            }

            // nothing to do for this ABI, validation is still useful though!
        }

        public void AddModuleFlags( Module module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
        }

        public ImmutableArray<AttributeValue> BuildTargetDependentFunctionAttributes( IContext ctx )
            => [
                ctx.CreateAttribute( "disable-tail-calls"u8, "false"u8 ),
                ctx.CreateAttribute( "less-precise-fpmad"u8, "false"u8 ),
                ctx.CreateAttribute( "no-frame-pointer-elim"u8, "false"u8 ),
                ctx.CreateAttribute( "no-infs-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "no-nans-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "stack-protector-buffer-size"u8, "8"u8 ),
                ctx.CreateAttribute( "target-cpu"u8, Cpu ),
                ctx.CreateAttribute( "target-features"u8, Features ),
                ctx.CreateAttribute( "unsafe-fp-math"u8, "false"u8 ),
                ctx.CreateAttribute( "use-soft-float"u8, "false"u8 ),
                ctx.CreateAttribute( "uwtable"u8, (ulong)UWTableKind.Async)
            ];

        private readonly ILibLlvm LlvmLib;

        // Sadly, these can't be utf8 literals, but, they can be static readonly LazyEncodedString!
        private static readonly LazyEncodedString Cpu = "x86-64"u8;
        private static readonly LazyEncodedString Features = "+sse,+sse2"u8;
        private static readonly LazyEncodedString TripleName = "x86_64-pc-windows-msvc18.0.0"u8;
    }
}

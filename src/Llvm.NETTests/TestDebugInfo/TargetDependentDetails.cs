using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Llvm.NET;
using Llvm.NET.Types;
using Llvm.NET.Values;

namespace TestDebugInfo
{
    interface ITargetDependentDetails
    {
        string ShortName { get; }
        string Triple { get; }
        string Cpu { get; }
        string Features { get; }
        AttributeSet BuildTargetDependentFunctionAttributes( Context ctx );
        void AddABIAttributesForByValueStructure( Function function, int paramIndex );
        void AddModuleFlags( NativeModule module );
    }

    class CortexM3Details
        : ITargetDependentDetails
    {
        public string Cpu => "cortex-m3";
        public string Features => "+hwdiv";
        public string ShortName => "M3";
        public string Triple => "thumbv7m-none--eabi";

        public void AddABIAttributesForByValueStructure( Function function, int paramIndex )
        {
            // ByVal pointers indicate by value semantics. The actual semantics are along the lines of
            // "pass the arg as copy on the arguments stack and set parameter implicitly to that copy's address"
            // (src: https://github.com/ldc-developers/ldc/issues/937 ) [e.g. caller copies byval args]
            //
            // LLVM recognizes this pattern and has a pass to map to an efficient register usage whenever plausible.
            // Though it seems Clang doesn't apply the attribute in all cases, for x86 it doesn't appear to ever use
            // it, for Cortex-Mx it seems to use it only for larger structs, otherwise it uses an [ n x i32]. (Max 
            // value of n is not known) and performs casts. Thus, on cortex-m the function parameters are handled
            // quite differently by clang, which seems odd to put such target dependent differences into the front-end.

            var argType = function.Parameters[ paramIndex ].NativeType as IPointerType;
            if( argType == null || !argType.ElementType.IsStruct )
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );

            var layout = function.ParentModule.Layout;
            function.AddAttributes( FunctionAttributeIndex.Parameter0 + paramIndex
                                  , AttributeKind.ByVal
                                  , new AttributeValue( AttributeKind.Alignment, layout.AbiAlignmentOf( argType.ElementType ) )
                                  );
        }

        public void AddModuleFlags( NativeModule module )
        {
            // Specify ABI const sizes so linker can detect mismatches
            module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size", 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size", 4 );
        }

        public AttributeSet BuildTargetDependentFunctionAttributes( Context ctx )
        {
            var bldr = new AttributeBuilder( );

            bldr.Add( "disable-tail-calls", "false" );
            bldr.Add( "less-precise-fpmad", "false" );
            bldr.Add( "no-frame-pointer-elim", "true" );
            bldr.Add( "no-frame-pointer-elim-non-leaf" );
            bldr.Add( "no-infs-fp-math", "false" );
            bldr.Add( "no-nans-fp-math", "false" );
            bldr.Add( "stack-protector-buffer-size", "8" );
            bldr.Add( "target-cpu", Cpu );
            bldr.Add( "target-features", Features );
            bldr.Add( "unsafe-fp-math", "false" );
            bldr.Add( "use-soft-float", "false" );
            return bldr.ToAttributeSet( FunctionAttributeIndex.Function, ctx );
        }
    }

    class X64Details 
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
                throw new ArgumentException( "Signature for specified parameter must be a pointer to a structure" );
        }

        public void AddModuleFlags( NativeModule module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
        }

        public AttributeSet BuildTargetDependentFunctionAttributes( Context ctx )
        {
            var bldr = new AttributeBuilder( );

            bldr.Add( "disable-tail-calls", "false" );
            bldr.Add( "less-precise-fpmad", "false" );
            bldr.Add( "no-frame-pointer-elim", "false" );
            bldr.Add( "no-infs-fp-math", "false" );
            bldr.Add( "no-nans-fp-math", "false" );
            bldr.Add( "stack-protector-buffer-size", "8" );
            bldr.Add( "target-cpu", Cpu );
            bldr.Add( "target-features", Features );
            bldr.Add( "unsafe-fp-math", "false" );
            bldr.Add( "use-soft-float", "false" );
            bldr.Add( AttributeKind.UWTable );
            return bldr.ToAttributeSet( FunctionAttributeIndex.Function, ctx );
        }
    }
}

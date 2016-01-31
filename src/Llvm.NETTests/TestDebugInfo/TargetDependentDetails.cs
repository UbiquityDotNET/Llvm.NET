using Llvm.NET;
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
}

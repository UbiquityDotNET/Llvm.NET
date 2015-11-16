using Llvm.NET;

namespace ReloadModule
{
    class Program
    {
        static void Main( string[ ] args )
        {
            using( var context = new Context())
            using( var module = NativeModule.LoadFrom( args[ 0 ], context ) )
            using( var module2 = NativeModule.LoadFrom( args[ 1 ], context))
            {
                var doCopyFunc = module.GetFunction( "DoCopy" );
                var call = doCopyFunc.EntryBlock.FirstInstruction.ToString( );
                var doCopyFunc2 = module2.GetFunction( "DoCopy" );
            }
        }
    }
}

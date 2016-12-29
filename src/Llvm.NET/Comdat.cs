using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Comdat entry for a module</summary>
    /// <remarks>
    /// A COMDAT is a named kind pair to ensure that, within
    /// a given module there are no two named COMDATs with a
    /// different kinds. Ultimately, Comdat is 'owned' by the
    /// module, if the module is disposed the Comdats it owns
    /// are invalidated. Using a Comdat instance after the
    /// module is disposed results in an effective NOP.
    /// </remarks>
    public class Comdat
    {
        internal Comdat( NativeModule module, LLVMComdatRef comdatRef )
        {
            Module = module;
            ComdatHandle = comdatRef;
        }
        
        public string Name
        {
            get
            {
                if( Module.IsDisposed )
                    return string.Empty;
                else
                    return NativeMethods.MarshalMsg( NativeMethods.ComdatGetName( ComdatHandle ) );
            }
        }

        public ComdatKind Kind
        {
            get
            {
                if( Module.IsDisposed )
                    return default( ComdatKind );
                else
                    return (ComdatKind)NativeMethods.ComdatGetKind( ComdatHandle );
            }

            set
            {
                if( Module.IsDisposed )
                    return;

                NativeMethods.ComdatSetKind( ComdatHandle, ( LLVMComdatSelectionKind )value );
            }
        }

        internal readonly LLVMComdatRef ComdatHandle;
        internal readonly NativeModule Module;
    }
}

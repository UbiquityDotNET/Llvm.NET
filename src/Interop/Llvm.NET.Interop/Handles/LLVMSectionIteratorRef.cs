// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 2.17941.31104.49410
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Security;

namespace Llvm.NET.Interop
{
    [SecurityCritical]
    [GeneratedCode("LlvmBindingsGenerator","2.17941.31104.49410")]
    public class LLVMSectionIteratorRef
        : LlvmObjectRef
    {
        public LLVMSectionIteratorRef( IntPtr handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        [SecurityCritical]
        protected override bool ReleaseHandle( )
        {
            LLVMDisposeSectionIterator( handle );
            return true;
        }

        private LLVMSectionIteratorRef( )
            : base( true )
        {
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeSectionIterator( IntPtr p );
    }

    [GeneratedCode("LlvmBindingsGenerator","2.17941.31104.49410")]
    internal class LLVMSectionIteratorRefAlias
        : LLVMSectionIteratorRef
    {
        private LLVMSectionIteratorRefAlias()
            : base( IntPtr.Zero, false )
        {
        }
    }
}

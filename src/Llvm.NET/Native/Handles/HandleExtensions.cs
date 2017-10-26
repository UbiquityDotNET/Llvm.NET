// <copyright file="HandleExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Native
{
    internal static class HandleExtensions
    {
        internal static bool IsNull(this ILlvmHandle h)
        {
            return h.Handle == IntPtr.Zero;
        }

        internal static Context GetContextFor( this LLVMContextRef contextRef )
        {
            if( contextRef.IsNull( ) )
            {
                return null;
            }

            if( ContextCache.TryGetValue( contextRef, out Context retVal ) )
            {
                return retVal;
            }

            return new Context( contextRef );
        }

        internal static Context GetContextFor( this LLVMModuleRef moduleRef )
        {
            if( moduleRef.Handle == IntPtr.Zero )
            {
                return null;
            }

            var hContext = LLVMGetModuleContext( moduleRef );
            Debug.Assert( hContext.Handle != IntPtr.Zero, "Should not get a null pointer from LLVM" );
            return GetContextFor( hContext );
        }

        internal static Context GetContextFor( this LLVMValueRef valueRef )
        {
            if( valueRef.Handle == IntPtr.Zero )
            {
                return null;
            }

            var hType = LLVMTypeOf( valueRef );
            Debug.Assert( hType.Handle != IntPtr.Zero, "Should not get a null pointer from LLVM" );
            return GetContextFor( hType );
        }

        internal static Context GetContextFor( this LLVMTypeRef typeRef )
        {
            if( typeRef.Handle == IntPtr.Zero )
            {
                return null;
            }

            var hContext = LLVMGetTypeContext( typeRef );
            Debug.Assert( hContext.Handle != IntPtr.Zero, "Should not get a null pointer from LLVM" );
            return GetContextFor( hContext );
        }

        internal static Context GetContextFor( this LLVMMetadataRef handle )
        {
            if( handle == LLVMMetadataRef.Zero )
            {
                return null;
            }

            var hContext = LLVMGetNodeContext( handle );
            Debug.Assert( hContext.Handle != IntPtr.Zero, "Should not get a null pointer from LLVM" );
            return GetContextFor( hContext );
        }
    }
}

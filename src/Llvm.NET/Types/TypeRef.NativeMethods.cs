// <copyright file="TypeRef.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Types
{
    internal partial class TypeRef
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMGetElementType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMGetElementType( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeKind", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeKind LLVMGetTypeKind( LLVMTypeRef Ty );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMContextAlias LLVMGetTypeContext( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIntTypeWidth", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetIntTypeWidth( LLVMTypeRef IntegerTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMTypeIsSized", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTypeIsSized( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMDumpType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDumpType( LLVMTypeRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMPrintTypeToString", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMPrintTypeToString( LLVMTypeRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetStructName", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetStructName( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMStructSetBody", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMStructSetBody( LLVMTypeRef StructTy, out LLVMTypeRef ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )]bool Packed );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMCountStructElementTypes( LLVMTypeRef StructTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetStructElementTypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMGetStructElementTypes( LLVMTypeRef StructTy, out LLVMTypeRef Dest );

            [DllImport( LibraryPath, EntryPoint = "LLVMStructGetTypeAtIndex", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMStructGetTypeAtIndex( LLVMTypeRef StructTy, uint i );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsPackedStruct", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsPackedStruct( LLVMTypeRef StructTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsOpaqueStruct", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsOpaqueStruct( LLVMTypeRef StructTy );

            // Added to LLVM-C APIs in 5.0.0
            [DllImport( LibraryPath, EntryPoint = "LLVMGetSubtypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMGetSubtypes( LLVMTypeRef Tp, out LLVMTypeRef Arr );

            // Added to LLVM-C APIs in 5.0.0
            [DllImport( LibraryPath, EntryPoint = "LLVMGetNumContainedTypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetNumContainedTypes( LLVMTypeRef Tp );

            [DllImport( LibraryPath, EntryPoint = "LLVMArrayType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMArrayType( LLVMTypeRef ElementType, uint ElementCount );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetArrayLength", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetArrayLength( LLVMTypeRef ArrayTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMPointerType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMPointerType( LLVMTypeRef ElementType, uint AddressSpace );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPointerAddressSpace", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetPointerAddressSpace( LLVMTypeRef PointerTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstArray", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstArray( LLVMTypeRef ElementTy, out LLVMValueRef ConstantVals, uint Length );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNamedStruct", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNamedStruct( LLVMTypeRef StructTy, out LLVMValueRef ConstantVals, uint Count );

            [DllImport( LibraryPath, EntryPoint = "LLVMAlignOf", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMAlignOf( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMSizeOf", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMSizeOf( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerNull", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstPointerNull( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstInt", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstInt( LLVMTypeRef IntTy, ulong N, [MarshalAs( UnmanagedType.Bool )]bool SignExtend );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfArbitraryPrecision", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstIntOfArbitraryPrecision( LLVMTypeRef IntTy, uint NumWords, int[ ] Words );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstIntOfString( LLVMTypeRef IntTy, [MarshalAs( UnmanagedType.LPStr )] string Text, byte Radix );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstIntOfStringAndSize( LLVMTypeRef IntTy, [MarshalAs( UnmanagedType.LPStr )] string Text, uint SLen, byte Radix );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstReal", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstReal( LLVMTypeRef RealTy, double N );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstRealOfString( LLVMTypeRef RealTy, [MarshalAs( UnmanagedType.LPStr )] string Text );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstRealOfStringAndSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstRealOfStringAndSize( LLVMTypeRef RealTy, [MarshalAs( UnmanagedType.LPStr )] string Text, uint SLen );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNull", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNull( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstAllOnes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstAllOnes( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetUndef", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetUndef( LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMVectorType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMVectorType( LLVMTypeRef ElementType, uint ElementCount );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetVectorSize", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMGetVectorSize( LLVMTypeRef VectorTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsFunctionVarArg", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsFunctionVarArg( LLVMTypeRef FunctionTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetReturnType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMGetReturnType( LLVMTypeRef FunctionTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountParamTypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMCountParamTypes( LLVMTypeRef FunctionTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParamTypes", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMGetParamTypes( LLVMTypeRef FunctionTy, out LLVMTypeRef Dest );
        }
    }
}

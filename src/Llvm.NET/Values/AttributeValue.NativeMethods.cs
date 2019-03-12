// <copyright file="AttributeValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CC = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Values
{
    /// <summary>Single attribute for functions, function returns and function parameters</summary>
    public partial struct AttributeValue
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKindForName", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern uint LLVMGetEnumAttributeKindForName( [MarshalAs( UnmanagedType.LPStr )] string Name, size_t SLen );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastEnumAttributeKind", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMGetLastEnumAttributeKind( );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeKind", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMGetEnumAttributeKind( LLVMAttributeRef A );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeValue", CallingConvention = CC.Cdecl )]
            internal static extern ulong LLVMGetEnumAttributeValue( LLVMAttributeRef A );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeKind", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetStringAttributeKind( LLVMAttributeRef A, out uint Length );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeValue", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetStringAttributeValue( LLVMAttributeRef A, out uint Length );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsEnumAttribute", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsEnumAttribute( LLVMAttributeRef A );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsStringAttribute", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsStringAttribute( LLVMAttributeRef A );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMAttributeToString( LLVMAttributeRef attribute );
        }
    }
}

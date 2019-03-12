// <copyright file="InstructionBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using static Llvm.NET.Instructions.Instruction.NativeMethods;
using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Instructions
{
    /// <summary>LLVM Instruction builder allowing managed code to generate IR instructions</summary>
    public sealed partial class InstructionBuilder
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetZExtValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern ulong LLVMConstIntGetZExtValue( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntGetSExtValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern long LLVMConstIntGetSExtValue( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstRealGetDouble", CallingConvention = CallingConvention.Cdecl )]
            internal static extern double LLVMConstRealGetDouble( LLVMValueRef ConstantVal, [MarshalAs( UnmanagedType.Bool )]out bool losesInfo );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstString( [MarshalAs( UnmanagedType.LPStr )] string Str, uint Length, [MarshalAs( UnmanagedType.Bool )]bool DontNullTerminate );

            [DllImport( LibraryPath, EntryPoint = "LLVMIsConstantString", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsConstantString( LLVMValueRef c );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstStruct", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstStruct( out LLVMValueRef ConstantVals, uint Count, [MarshalAs( UnmanagedType.Bool )]bool Packed );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetElementAsConstant", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetElementAsConstant( LLVMValueRef C, uint idx );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstVector", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstVector( out LLVMValueRef ScalarConstantVals, uint Size );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetConstOpcode", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMOpcode LLVMGetConstOpcode( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNeg", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNeg( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWNeg", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNSWNeg( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWNeg", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNUWNeg( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFNeg", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFNeg( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNot", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNot( LLVMValueRef ConstantVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstAdd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWAdd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNSWAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWAdd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNUWAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFAdd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFAdd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSub", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWSub", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNSWSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWSub", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNUWSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFSub", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFSub( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstMul", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNSWMul", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNSWMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstNUWMul", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstNUWMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFMul", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFMul( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstUDiv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstUDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            // Added to LLVM-C APIs in LLVM 4.0.0
            [DllImport( LibraryPath, EntryPoint = "LLVMConstExactUDiv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstExactUDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSDiv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstExactSDiv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstExactSDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFDiv", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFDiv( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstURem", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstURem( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSRem", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSRem( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFRem", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFRem( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstAnd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstAnd( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstOr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstOr( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstXor", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstXor( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstICmp", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstICmp( LLVMIntPredicate Predicate, LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFCmp", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFCmp( LLVMRealPredicate Predicate, LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstShl", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstShl( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstLShr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstLShr( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstAShr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstAShr( LLVMValueRef LHSConstant, LLVMValueRef RHSConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstGEP", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstGEP( LLVMValueRef ConstantVal, out LLVMValueRef ConstantIndices, uint NumIndices );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstInBoundsGEP", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstInBoundsGEP( LLVMValueRef ConstantVal, out LLVMValueRef ConstantIndices, uint NumIndices );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstTrunc", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstTrunc( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSExt", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSExt( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstZExt", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstZExt( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFPTrunc", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFPTrunc( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFPExt", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFPExt( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstUIToFP", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstUIToFP( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSIToFP", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSIToFP( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToUI", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFPToUI( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFPToSI", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFPToSI( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstPtrToInt", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstPtrToInt( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntToPtr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstIntToPtr( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstBitCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstAddrSpaceCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstAddrSpaceCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstZExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstZExtOrBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSExtOrBitCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSExtOrBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstTruncOrBitCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstTruncOrBitCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstPointerCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstPointerCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstIntCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstIntCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType, [MarshalAs( UnmanagedType.Bool )]bool isSigned );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstFPCast", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstFPCast( LLVMValueRef ConstantVal, LLVMTypeRef ToType );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstSelect", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstSelect( LLVMValueRef ConstantCondition, LLVMValueRef ConstantIfTrue, LLVMValueRef ConstantIfFalse );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractElement", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstExtractElement( LLVMValueRef VectorConstant, LLVMValueRef IndexConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertElement", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstInsertElement( LLVMValueRef VectorConstant, LLVMValueRef ElementValueConstant, LLVMValueRef IndexConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstShuffleVector", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstShuffleVector( LLVMValueRef VectorAConstant, LLVMValueRef VectorBConstant, LLVMValueRef MaskConstant );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstExtractValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstExtractValue( LLVMValueRef AggConstant, out uint IdxList, uint NumIdx );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstInsertValue", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstInsertValue( LLVMValueRef AggConstant, LLVMValueRef ElementValueConstant, out uint IdxList, uint NumIdx );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstInlineAsm( LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string AsmString, [MarshalAs( UnmanagedType.LPStr )] string Constraints, [MarshalAs( UnmanagedType.Bool )]bool HasSideEffects, [MarshalAs( UnmanagedType.Bool )]bool IsAlignStack );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilderInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBuilderRef LLVMCreateBuilderInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateBuilder", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBuilderRef LLVMCreateBuilder( );

            [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilder", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMPositionBuilder( LLVMBuilderRef Builder, LLVMBasicBlockRef Block, LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderBefore", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMPositionBuilderBefore( LLVMBuilderRef Builder, LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMPositionBuilderAtEnd", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMPositionBuilderAtEnd( LLVMBuilderRef Builder, LLVMBasicBlockRef Block );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetInsertBlock", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetInsertBlock( LLVMBuilderRef Builder );

            [DllImport( LibraryPath, EntryPoint = "LLVMClearInsertionPosition", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMClearInsertionPosition( LLVMBuilderRef Builder );

            [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilder", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInsertIntoBuilder( LLVMBuilderRef Builder, LLVMValueRef Instr );

            [DllImport( LibraryPath, EntryPoint = "LLVMInsertIntoBuilderWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMInsertIntoBuilderWithName( LLVMBuilderRef Builder, LLVMValueRef Instr, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetCurrentDebugLocation( LLVMBuilderRef Builder, LLVMValueRef L );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetCurrentDebugLocation", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetCurrentDebugLocation( LLVMBuilderRef Builder );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetInstDebugLocation", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetInstDebugLocation( LLVMBuilderRef Builder, LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildRetVoid", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildRetVoid( LLVMBuilderRef param0 );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildRet", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildRet( LLVMBuilderRef param0, LLVMValueRef V );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAggregateRet", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildAggregateRet( LLVMBuilderRef param0, out LLVMValueRef RetVals, uint N );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildBr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildBr( LLVMBuilderRef param0, LLVMBasicBlockRef Dest );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildCondBr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildCondBr( LLVMBuilderRef param0, LLVMValueRef If, LLVMBasicBlockRef Then, LLVMBasicBlockRef Else );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSwitch", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildSwitch( LLVMBuilderRef param0, LLVMValueRef V, LLVMBasicBlockRef Else, uint NumCases );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildIndirectBr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildIndirectBr( LLVMBuilderRef B, LLVMValueRef Addr, uint NumDests );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildInvoke", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildInvoke( LLVMBuilderRef param0, LLVMValueRef Fn, out LLVMValueRef Args, uint NumArgs, LLVMBasicBlockRef Then, LLVMBasicBlockRef Catch, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildLandingPad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildLandingPad( LLVMBuilderRef B, LLVMTypeRef Ty, LLVMValueRef PersFn, uint NumClauses, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildResume", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildResume( LLVMBuilderRef B, LLVMValueRef Exn );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildUnreachable", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildUnreachable( LLVMBuilderRef param0 );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntCast2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMBuildIntCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.Bool )]bool isSigned, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetCurrentDebugLocation2( LLVMBuilderRef Bref, UInt32 Line, UInt32 Col, LLVMMetadataRef Scope, LLVMMetadataRef InlinedAt );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildAdd( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNSWAdd( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNUWAdd( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFAdd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFAdd( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSub( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNSWSub( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNUWSub( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFSub", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFSub( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildMul( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNSWMul( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNUWMul( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFMul", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFMul( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildUDiv( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            // Added to LLVM-C API in LLVM 4.0.0
            [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactUDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildExactUDiv( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSDiv( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildExactSDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildExactSDiv( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFDiv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFDiv( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildURem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildURem( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSRem( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFRem", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFRem( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildShl", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildShl( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildLShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildLShr( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAShr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildAShr( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildAnd( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildOr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildOr( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildXor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildXor( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildBinOp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildBinOp( LLVMBuilderRef B, LLVMOpcode Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNeg( LLVMBuilderRef param0, LLVMValueRef V, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNSWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNSWNeg( LLVMBuilderRef B, LLVMValueRef V, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNUWNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNUWNeg( LLVMBuilderRef B, LLVMValueRef V, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFNeg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFNeg( LLVMBuilderRef param0, LLVMValueRef V, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildNot", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildNot( LLVMBuilderRef param0, LLVMValueRef V, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildMalloc( LLVMBuilderRef param0, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayMalloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildArrayMalloc( LLVMBuilderRef param0, LLVMTypeRef Ty, LLVMValueRef Val, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildAlloca( LLVMBuilderRef param0, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildArrayAlloca", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildArrayAlloca( LLVMBuilderRef param0, LLVMTypeRef Ty, LLVMValueRef Val, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFree", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildFree( LLVMBuilderRef param0, LLVMValueRef PointerVal );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildLoad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildLoad( LLVMBuilderRef param0, LLVMValueRef PointerVal, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildStore", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildStore( LLVMBuilderRef param0, LLVMValueRef Val, LLVMValueRef Ptr );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildGEP( LLVMBuilderRef B, LLVMValueRef Pointer, out LLVMValueRef Indices, uint NumIndices, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildInBoundsGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildInBoundsGEP( LLVMBuilderRef B, LLVMValueRef Pointer, out LLVMValueRef Indices, uint NumIndices, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildStructGEP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildStructGEP( LLVMBuilderRef B, LLVMValueRef Pointer, uint Idx, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalString", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildGlobalString( LLVMBuilderRef B, [MarshalAs( UnmanagedType.LPStr )] string Str, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildGlobalStringPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildGlobalStringPtr( LLVMBuilderRef B, [MarshalAs( UnmanagedType.LPStr )] string Str, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildTrunc( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildZExt( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSExt( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToUI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFPToUI( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPToSI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFPToSI( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildUIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildUIToFP( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSIToFP", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSIToFP( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPTrunc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFPTrunc( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPExt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFPExt( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrToInt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildPtrToInt( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntToPtr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildIntToPtr( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildBitCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAddrSpaceCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildAddrSpaceCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildZExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildZExtOrBitCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSExtOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSExtOrBitCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildTruncOrBitCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildTruncOrBitCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildCast( LLVMBuilderRef B, LLVMOpcode Op, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildPointerCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildPointerCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFPCast", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFPCast( LLVMBuilderRef param0, LLVMValueRef Val, LLVMTypeRef DestTy, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildICmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildICmp( LLVMBuilderRef param0, LLVMIntPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFCmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFCmp( LLVMBuilderRef param0, LLVMRealPredicate Op, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildPhi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildPhi( LLVMBuilderRef param0, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildCall", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildCall( LLVMBuilderRef param0, LLVMValueRef Fn, out LLVMValueRef Args, uint NumArgs, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildSelect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildSelect( LLVMBuilderRef param0, LLVMValueRef If, LLVMValueRef Then, LLVMValueRef Else, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildVAArg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildVAArg( LLVMBuilderRef param0, LLVMValueRef List, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildExtractElement( LLVMBuilderRef param0, LLVMValueRef VecVal, LLVMValueRef Index, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertElement", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildInsertElement( LLVMBuilderRef param0, LLVMValueRef VecVal, LLVMValueRef EltVal, LLVMValueRef Index, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildShuffleVector", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildShuffleVector( LLVMBuilderRef param0, LLVMValueRef V1, LLVMValueRef V2, LLVMValueRef Mask, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildExtractValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildExtractValue( LLVMBuilderRef param0, LLVMValueRef AggVal, uint Index, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildInsertValue", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildInsertValue( LLVMBuilderRef param0, LLVMValueRef AggVal, LLVMValueRef EltVal, uint Index, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildIsNull( LLVMBuilderRef param0, LLVMValueRef Val, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildIsNotNull", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildIsNotNull( LLVMBuilderRef param0, LLVMValueRef Val, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildPtrDiff", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildPtrDiff( LLVMBuilderRef param0, LLVMValueRef LHS, LLVMValueRef RHS, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildFence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMBuildFence( LLVMBuilderRef B, LLVMAtomicOrdering ordering, [MarshalAs( UnmanagedType.Bool )]bool singleThread, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicRMW", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildAtomicRMW( LLVMBuilderRef B, LLVMAtomicRMWBinOp op, LLVMValueRef PTR, LLVMValueRef Val, LLVMAtomicOrdering ordering, [MarshalAs( UnmanagedType.Bool )]bool singleThread );

            [DllImport( LibraryPath, EntryPoint = "LLVMBuildAtomicCmpXchg", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBuildAtomicCmpXchg( LLVMBuilderRef B, LLVMValueRef Ptr, LLVMValueRef Cmp, LLVMValueRef New, LLVMAtomicOrdering SuccessOrdering, LLVMAtomicOrdering FailureOrdering, [MarshalAs( UnmanagedType.Bool )]bool SingleThread );
        }
    }
}

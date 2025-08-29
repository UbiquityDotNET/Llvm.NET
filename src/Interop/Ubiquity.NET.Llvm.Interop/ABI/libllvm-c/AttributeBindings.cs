// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public enum LibLLVMAttributeArgKind
    {
        LibLLVMAttributeArgKind_None,
        LibLLVMAttributeArgKind_Int,
        LibLLVMAttributeArgKind_Type,
        LibLLVMAttributeArgKind_ConstantRange,
        LibLLVMAttributeArgKind_ConstantRangeList,
        LibLLVMAttributeArgKind_String, // NOTE: if argkind is string then ID is 0; String args MAY be BOOL 'true' 'false' but that is a constraint on the value as a string!
    }

    [Flags]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI naming" )]
    public enum LibLLVMAttributeAllowedOn
    {
        LibLLVMAttributeAllowedOn_None,
        LibLLVMAttributeAllowedOn_Return = 0x0001,
        LibLLVMAttributeAllowedOn_Parameter = 0x0002,
        LibLLVMAttributeAllowedOn_Function = 0x0004,
        LibLLVMAttributeAllowedOn_CallSite = 0x0008,
        LibLLVMAttributeAllowedOn_Global = 0x0010,

        LibLLVMAttributeAllowedOn_All = LibLLVMAttributeAllowedOn_Return
                                      | LibLLVMAttributeAllowedOn_Parameter
                                      | LibLLVMAttributeAllowedOn_Function
                                      | LibLLVMAttributeAllowedOn_CallSite
                                      | LibLLVMAttributeAllowedOn_Global
    }

    [StructLayout( LayoutKind.Sequential )]
    public readonly record struct LibLLVMAttributeInfo( UInt32 ID, LibLLVMAttributeArgKind ArgKind, LibLLVMAttributeAllowedOn AllowedOn );

    public static partial class AttributeBindings
    {
        // Gets the number of attributes known by this implementation/runtime
        // The value this returns is used to allocate an array of `char const*`
        // to use with the LibLLVMGetKnownAttributeNames() API.
        // NOTE: This includes string only attributes where there is no enum
        // so the returned value is > than LLVMGetLastEnumAttributeKind()
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial nuint LibLLVMGetNumKnownAttribs( );

        // Fills in an array of const string pointers. No deallocation is needed for each as
        // they are global static constants.

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMGetKnownAttributeNames(
            byte** names,
            nuint namesLen
        );

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( DisposeMessageMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LibLLVMAttributeToString( LLVMAttributeRef attribute );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsConstantRangeAttribute( LLVMAttributeRef attribute );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMIsConstantRangeListAttribute( LLVMAttributeRef attribute );

        // The concept of AttributeInfo is entirely unique to LibLLVM (The extended C based API). This consolidates
        // information about an attribute based on it's name. It is implemented in the native code to gather the
        // required information from LLVM and report it as a simple data structure. These are cacheable in that the
        // same name will produce the same attribute info data.
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static unsafe LLVMErrorRef LibLLVMGetAttributeInfo( LazyEncodedString attribName, out LibLLVMAttributeInfo info )
        {
            return LibLLVMGetAttributeInfo( attribName, attribName.NativeStrLen, out info );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]

        private static unsafe partial LLVMErrorRef LibLLVMGetAttributeInfo( LazyEncodedString attribName, nuint nameLen, out LibLLVMAttributeInfo info );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LibLLVMGetAttributeNameFromID( UInt32 id )
        {
            unsafe
            {
                byte* p = LibLLVMGetAttributeNameFromID(id, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LibLLVMGetAttributeNameFromID( UInt32 id, out uint len );
    }
}

// -----------------------------------------------------------------------
// <copyright file="AttributeBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{
    public enum LibLLVMAttributeArgKind
    {
        LibLLVMAttributeArgKind_None,
        LibLLVMAttributeArgKind_Int,
        LibLLVMAttributeArgKind_Type,
        LibLLVMAttributeArgKind_ConstantRange,
        LibLLVMAttributeArgKind_ConstantRangeList,
        LibLLVMAttributeArgKind_String, // NOTE: if argkind is string then ID is 0
                                        // String args MAY be BOOL 'true' 'false' but that is a constraint on the value as a string!
    };

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
    };

    [StructLayout(LayoutKind.Sequential)]
    public readonly record struct LibLLVMAttributeInfo(UInt32 ID, LibLLVMAttributeArgKind ArgKind, LibLLVMAttributeAllowedOn AllowedOn);

    public static partial class AttributeBindings
    {
        // Gets the number of attributes known by this implementation/runtime
        // The value this returns is used to allocate and array of `char const*`
        // to use with the LibLLVMGetKnownAttributeNames() API.
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial size_t LibLLVMGetNumKnownAttribs();

        // Fills in an array of const string pointers. No deallocation is needed for each as
        // they are global static constants.

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMGetKnownAttributeNames(size_t namesLen, /*[Out]*/ byte** names);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof(DisposeMessageMarshaller) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial string LibLLVMAttributeToString(LLVMAttributeRef attribute);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static unsafe partial bool LibLLVMIsConstantRangeAttribute(LLVMAttributeRef attribute);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static unsafe partial bool LibLLVMIsConstantRangeListAttribute(LLVMAttributeRef attribute);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]

        public static unsafe partial LLVMErrorRef LibLLVMGetAttributeInfo(byte* attribName, size_t nameLen, /*[out, byref]*/ LibLLVMAttributeInfo* pInfo);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial byte* LibLLVMGetAttributeNameFromID(UInt32 id, out uint len);
    }
}

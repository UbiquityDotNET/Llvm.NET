
using System;

namespace Llvm.NET.DebugInfo
{
    public enum SourceLanguage
    {
        // Language names
        C89 = 0x0001,
        C = 0x0002,
        Ada83 = 0x0003,
        CPlusPlus = 0x0004,
        Cobol74 = 0x0005,
        Cobol85 = 0x0006,
        Fortran77 = 0x0007,
        Fortran90 = 0x0008,
        Pascal83 = 0x0009,
        Modula2 = 0x000a,
        Java = 0x000b,
        C99 = 0x000c,
        Ada95 = 0x000d,
        Fortran95 = 0x000e,
        PLI = 0x000f,
        ObjC = 0x0010,
        ObjCPlusPlus = 0x0011,
        UPC = 0x0012,
        D = 0x0013,

        // New in DWARF 5:
        Python = 0x0014,
        OpenCL = 0x0015,
        Go = 0x0016,
        Modula3 = 0x0017,
        Haskell = 0x0018,
        CPlusPlus_03 = 0x0019,
        CPlusPlus_11 = 0x001a,
        OCaml = 0x001b,

        UserMin = 0x8000,
        LLvmMipsAssembler = UserMin + 1,
        CSharp = UserMin + 0x0100,
        ILAsm = UserMin + 0x01001,
        UserMax = 0xffff
    }

    public enum Tag : ushort
    {
        ArrayType= LLVMDwarfTag.LLVMDwarfTagArrayType,
        ClassType= LLVMDwarfTag.LLVMDwarfTagClassType,
        EntryPoint= LLVMDwarfTag.LLVMDwarfTagEntryPoint,
        EnumerationType= LLVMDwarfTag.LLVMDwarfTagEnumerationType,
        FormalParameter= LLVMDwarfTag.LLVMDwarfTagFormalParameter,
        ImportedDeclaration= LLVMDwarfTag.LLVMDwarfTagImportedDeclaration,
        Label= LLVMDwarfTag.LLVMDwarfTagLabel,
        LexicalBlock= LLVMDwarfTag.LLVMDwarfTagLexicalBlock,
        Member= LLVMDwarfTag.LLVMDwarfTagMember,
        PointerType= LLVMDwarfTag.LLVMDwarfTagPointerType,
        ReferenceType= LLVMDwarfTag.LLVMDwarfTagReferenceType,
        CompileUnit= LLVMDwarfTag.LLVMDwarfTagCompileUnit,
        StringType= LLVMDwarfTag.LLVMDwarfTagStringType,
        StructureType= LLVMDwarfTag.LLVMDwarfTagStructureType,
        SubroutineType= LLVMDwarfTag.LLVMDwarfTagSubroutineType,
        TypeDef= LLVMDwarfTag.LLVMDwarfTagTypeDef,
        UnionType= LLVMDwarfTag.LLVMDwarfTagUnionType,
        UnspecifiedParameters= LLVMDwarfTag.LLVMDwarfTagUnspecifiedParameters,
        Variant= LLVMDwarfTag.LLVMDwarfTagVariant,
        CommonBlock= LLVMDwarfTag.LLVMDwarfTagCommonBlock,
        CommonInclusion= LLVMDwarfTag.LLVMDwarfTagCommonInclusion,
        Inheritance= LLVMDwarfTag.LLVMDwarfTagInheritance,
        InlinedSubroutine= LLVMDwarfTag.LLVMDwarfTagInlinedSubroutine,
        Module= LLVMDwarfTag.LLVMDwarfTagModule,
        PtrToMemberType= LLVMDwarfTag.LLVMDwarfTagPtrToMemberType,
        SetType= LLVMDwarfTag.LLVMDwarfTagSetType,
        SubrangeType= LLVMDwarfTag.LLVMDwarfTagSubrangeType,
        WithStatement= LLVMDwarfTag.LLVMDwarfTagWithStatement,
        AccessDeclaration= LLVMDwarfTag.LLVMDwarfTagAccessDeclaration,
        BaseType= LLVMDwarfTag.LLVMDwarfTagBaseType,
        CatchBlock= LLVMDwarfTag.LLVMDwarfTagCatchBlock,
        ConstType= LLVMDwarfTag.LLVMDwarfTagConstType,
        Constant= LLVMDwarfTag.LLVMDwarfTagConstant,
        Enumerator= LLVMDwarfTag.LLVMDwarfTagEnumerator,
        FileType= LLVMDwarfTag.LLVMDwarfTagFileType,
        Friend= LLVMDwarfTag.LLVMDwarfTagFriend,
        NameList= LLVMDwarfTag.LLVMDwarfTagNameList,
        NameListItem= LLVMDwarfTag.LLVMDwarfTagNameListItem,
        PackedType= LLVMDwarfTag.LLVMDwarfTagPackedType,
        SubProgram= LLVMDwarfTag.LLVMDwarfTagSubProgram,
        TemplateTypeParameter= LLVMDwarfTag.LLVMDwarfTagTemplateTypeParameter,
        TemplateValueParameter= LLVMDwarfTag.LLVMDwarfTagTemplateValueParameter,
        ThrownType= LLVMDwarfTag.LLVMDwarfTagThrownType,
        TryBlock= LLVMDwarfTag.LLVMDwarfTagTryBlock,
        VariantPart= LLVMDwarfTag.LLVMDwarfTagVariantPart,
        Variable= LLVMDwarfTag.LLVMDwarfTagVariable,
        VolatileType= LLVMDwarfTag.LLVMDwarfTagVolatileType,
        DwarfProcedure= LLVMDwarfTag.LLVMDwarfTagDwarfProcedure,
        RestrictType= LLVMDwarfTag.LLVMDwarfTagRestrictType,
        InterfaceType= LLVMDwarfTag.LLVMDwarfTagInterfaceType,
        Namespace= LLVMDwarfTag.LLVMDwarfTagNamespace,
        ImportedModule= LLVMDwarfTag.LLVMDwarfTagImportedModule,
        UnspecifiedType= LLVMDwarfTag.LLVMDwarfTagUnspecifiedType,
        PartialUnit= LLVMDwarfTag.LLVMDwarfTagPartialUnit,
        InportedUnit= LLVMDwarfTag.LLVMDwarfTagInportedUnit,
        Condition= LLVMDwarfTag.LLVMDwarfTagCondition,
        SharedType= LLVMDwarfTag.LLVMDwarfTagSharedType,
        TypeUnit= LLVMDwarfTag.LLVMDwarfTagTypeUnit,
        RValueReferenceType= LLVMDwarfTag.LLVMDwarfTagRValueReferenceType,
        TemplateAlias= LLVMDwarfTag.LLVMDwarfTagTemplateAlias,
         
        // New in DWARF 5: // New in DWARF 5:
        CoArrayType= LLVMDwarfTag.LLVMDwarfTagCoArrayType,
        GenericSubrange= LLVMDwarfTag.LLVMDwarfTagGenericSubrange,
        DynamicType= LLVMDwarfTag.LLVMDwarfTagDynamicType,

        // LLVM Custom constants
        AutoVariable = 0x100, // Tag for local (auto) variables.
        ArgVariable = 0x101,  // Tag for argument variables.
        Expression = 0x102,    // Tag for complex address expressions.

        UserBase = 0x1000, // Recommended base for user tags.

        MipsLoop = LLVMDwarfTag.LLVMDwarfTagMipsLoop,
        FormatLabel= LLVMDwarfTag.LLVMDwarfTagFormatLabel,
        FunctionTemplate= LLVMDwarfTag.LLVMDwarfTagFunctionTemplate,
        ClassTemplate= LLVMDwarfTag.LLVMDwarfTagClassTemplate,
        GnuTemplateTemplateParam= LLVMDwarfTag.LLVMDwarfTagGnuTemplateTemplateParam,
        GnuTemplateParameterPack= LLVMDwarfTag.LLVMDwarfTagGnuTemplateParameterPack,
        GnuFormalParameterPack= LLVMDwarfTag.LLVMDwarfTagGnuFormalParameterPack,
        LoUser= LLVMDwarfTag.LLVMDwarfTagLoUser,
        AppleProperty= LLVMDwarfTag.LLVMDwarfTagAppleProperty,
        HiUser = LLVMDwarfTag.LLVMDwarfTagHiUser
    }

    public enum TypeKind : uint
    {
        // Encoding attribute values
        Address = 0x01,
        Boolean = 0x02,
        Complex_float = 0x03,
        Float = 0x04,
        Signed = 0x05,
        SignedChar = 0x06,
        Unsigned = 0x07,
        UnsignedChar = 0x08,
        ImaginaryFloat = 0x09,
        PackedDecimal = 0x0a,
        NumericString = 0x0b,
        Edited = 0x0c,
        SignedFixed = 0x0d,
        UnsignedFixed = 0x0e,
        DecimalFloat = 0x0f,
        UTF = 0x10,
        LoUser = 0x80,
        HiUser = 0xff
    }

    /// <summary>Accessibility flags</summary>
    /// <remarks>
    /// The three accessibility flags are mutually exclusive and rolled together
    /// in the first two bits.
    /// </remarks>
    [Flags]
    public enum DebugInfoFlags
    {
        Private = 1,
        Protected = 2,
        Public = 3,
        AccessibilityMask = 1 << 0 | 1 << 1,

        FwdDecl = 1 << 2,
        AppleBlock = 1 << 3,
        BlockByrefStruct = 1 << 4,
        Virtual = 1 << 5,
        Artificial = 1 << 6,
        Explicit = 1 << 7,
        Prototyped = 1 << 8,
        ObjcClassComplete = 1 << 9,
        ObjectPointer = 1 << 10,
        Vector = 1 << 11,
        StaticMember = 1 << 12,
        IndirectVariable = 1 << 13,
        LValueReference = 1 << 14,
        RValueReference = 1 << 15
    }

    public enum ExpressionOp : long
    {
    }
}

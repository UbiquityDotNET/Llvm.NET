// -----------------------------------------------------------------------
// <copyright file="IContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.ObjectFile;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Interface for an unowned LLVM Context</summary>
    /// <remarks>
    /// This interface is intended to distinguish between an unowned reference to
    /// a context and something that is owned and requires a call to
    /// <see cref="IDisposable.Dispose"/>. A <see cref="Context"/> is an owned
    /// type that callers ***MUST** dispose, an <see cref="IContext"/> interface
    /// is not.
    /// </remarks>
    public interface IContext
        : IEquatable<IContext>
    {
        /// <summary>Gets the LLVM void type for this context</summary>
        public ITypeRef VoidType { get; }

        /// <summary>Gets the LLVM boolean type for this context</summary>
        public ITypeRef BoolType { get; }

        /// <summary>Gets the LLVM 8 bit integer type for this context</summary>
        public ITypeRef Int8Type { get; }

        /// <summary>Gets the LLVM 16 bit integer type for this context</summary>
        public ITypeRef Int16Type { get; }

        /// <summary>Gets the LLVM 32 bit integer type for this context</summary>
        public ITypeRef Int32Type { get; }

        /// <summary>Gets the LLVM 64 bit integer type for this context</summary>
        public ITypeRef Int64Type { get; }

        /// <summary>Gets the LLVM 128 bit integer type for this context</summary>
        public ITypeRef Int128Type { get; }

        /// <summary>Gets the LLVM half precision floating point type for this context</summary>
        public ITypeRef HalfFloatType { get; }

        /// <summary>Gets the LLVM single precision floating point type for this context</summary>
        public ITypeRef FloatType { get; }

        /// <summary>Gets the LLVM double precision floating point type for this context</summary>
        public ITypeRef DoubleType { get; }

        /// <summary>Gets the LLVM token type for this context</summary>
        public ITypeRef TokenType { get; }

        /// <summary>Gets the LLVM IrMetadata type for this context</summary>
        public ITypeRef MetadataType { get; }

        /// <summary>Gets the LLVM X86 80-bit floating point type for this context</summary>
        public ITypeRef X86Float80Type { get; }

        /// <summary>Gets the LLVM 128-Bit floating point type</summary>
        public ITypeRef Float128Type { get; }

        /// <summary>Gets the LLVM PPC 128-bit floating point type</summary>
        public ITypeRef PpcFloat128Type { get; }

        /// <summary>Gets or sets a value indicating whether the context keeps a map for uniqueing debug info identifiers across the context</summary>
        public bool OdrUniqueDebugTypes { get; set; }

        /// <summary>Gets or sets a value indicating whether this context is configured to discard value names</summary>
        public bool DiscardValueName { get; set; }

#if HAVE_API_TO_ENUMERATE_METADATA
        /*
        public IEnumerable<string> MDKindNames { get; }
        /*TODO: Create interop calls to support additional properties/methods

        public unsigned GetOperandBundleTagId(string name) {...}
        public IEnumerable<string> OperandBundleTagIds { get; }
        */

        // Underlying LLVM has no mechanism to access the metadata in a context.

        /// <summary>Gets an enumerable collection of all the metadata created in this context</summary>
        public IEnumerable<LlvmMetadata> Metadata => MetadataCache;
#endif

        /// <summary>Set a custom diagnostic handler</summary>
        /// <param name="handler">handler</param>
        public void SetDiagnosticHandler(DiagnosticInfoCallbackAction handler);

        /// <summary>Get a type that is a pointer to a value of a given type</summary>
        /// <param name="elementType">Type of value the pointer points to</param>
        /// <returns><see cref="IPointerType"/> for a pointer that references a value of type <paramref name="elementType"/></returns>
        public IPointerType GetPointerTypeFor(ITypeRef elementType);

        /// <summary>Get's an LLVM integer type of arbitrary bit width</summary>
        /// <param name="bitWidth">Width of the integer type in bits</param>
        /// <remarks>
        /// For standard integer bit widths (e.g. 1,8,16,32,64) this will return
        /// the same type as the corresponding specialized property.
        /// (e.g. GetIntType(1) is the same as <see cref="BoolType"/>,
        ///  GetIntType(16) is the same as <see cref="Int16Type"/>, etc... )
        /// </remarks>
        /// <returns>Integer <see cref="ITypeRef"/> for the specified width</returns>
        public ITypeRef GetIntType(uint bitWidth);

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public IFunctionType GetFunctionType(ITypeRef returnType, params IEnumerable<ITypeRef> args);

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="isVarArgs">Flag to indicate if the method supports C/C++ style VarArgs</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, params IEnumerable<ITypeRef> args );

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( ref readonly DIBuilder diBuilder
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   );

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="isVarArg">Flag to indicate if this function is variadic</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( ref readonly DIBuilder diBuilder
                                                   , bool isVarArg
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   );

        /// <summary>Creates a constant structure from a set of values</summary>
        /// <param name="packed">Flag to indicate if the structure is packed and no alignment should be applied to the members</param>
        /// <param name="values">Set of values to use in forming the structure</param>
        /// <returns>Newly created <see cref="Constant"/></returns>
        /// <remarks>
        /// <note type="note">The actual concrete return type depends on the parameters provided and will be one of the following:
        /// <list type="table">
        /// <listheader>
        /// <term><see cref="Constant"/> derived type</term><description>Description</description>
        /// </listheader>
        /// <item><term>ConstantAggregateZero</term><description>If all the member values are zero constants</description></item>
        /// <item><term>UndefValue</term><description>If all the member values are UndefValue</description></item>
        /// <item><term>ConstantStruct</term><description>All other cases</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        public Constant CreateConstantStruct(bool packed, params IEnumerable<Constant> values);

        /// <summary>Creates a constant instance of a specified structure type from a set of values</summary>
        /// <param name="type">Type of the structure to create</param>
        /// <param name="values">Set of values to use in forming the structure</param>
        /// <returns>Newly created <see cref="Constant"/></returns>
        /// <remarks>
        /// <note type="note">The actual concrete return type depends on the parameters provided and will be one of the following:
        /// <list type="table">
        /// <listheader>
        /// <term><see cref="Constant"/> derived type</term><description>Description</description>
        /// </listheader>
        /// <item><term>ConstantAggregateZero</term><description>If all the member values are zero constants</description></item>
        /// <item><term>UndefValue</term><description>If all the member values are UndefValue</description></item>
        /// <item><term>ConstantStruct</term><description>All other cases</description></item>
        /// </list>
        /// </note>
        /// </remarks>
        public Constant CreateNamedConstantStruct(IStructType type, params IEnumerable<Constant> values);

        /// <summary>Create an opaque structure type (e.g. a forward reference)</summary>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <remarks>
        /// This method creates an opaque type. The <see cref="IStructType.SetBody"/> method provides a means
        /// to add a body, including indication of packed status, to an opaque type at a later time if the
        /// details of the body are required. (If only pointers to the type are required then the body isn't
        /// required)
        /// </remarks>
        /// <returns>New type</returns>
        public IStructType CreateStructType(string name);

        /// <summary>Create an anonymous structure type (e.g. Tuple)</summary>
        /// <param name="packed">Flag to indicate if the structure is "packed"</param>
        /// <param name="elements">Types of the fields of the structure</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        public IStructType CreateStructType(bool packed, params IEnumerable<ITypeRef> elements);

        /// <summary>Creates a new structure type in this <see cref="ContextAlias"/></summary>
        /// <param name="name">Name of the structure (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="packed">Flag indicating if the structure is packed</param>
        /// <param name="elements">Types for the structures elements in layout order</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        /// <remarks>
        /// If the elements argument list is empty then a complete 0 sized struct is created
        /// </remarks>
        public IStructType CreateStructType(string name, bool packed, params IEnumerable<ITypeRef> elements);

        /// <summary>Creates a metadata string from the given string</summary>
        /// <param name="value">string to create as metadata</param>
        /// <returns>new metadata string</returns>
        /// <remarks>
        /// if <paramref name="value"/> is <see langword="null"/> then the result
        /// represents an empty string.
        /// </remarks>
        public MDString CreateMetadataString(string? value);

        /// <summary>Create an <see cref="MDNode"/> from a string</summary>
        /// <param name="value">String value</param>
        /// <returns>New node with the string as the first element of the <see cref="MDNode.Operands"/> property (as an MDString)</returns>
        public MDNode CreateMDNode(string value);

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data with a terminating null character. To control the enforcement
        /// of a terminating null character, use the <see cref="CreateConstantString(string, bool)"/>
        /// overload to specify the intended behavior.
        /// </remarks>
        public ConstantDataArray CreateConstantString(string value);

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <param name="nullTerminate">flag to indicate if the string should include a null terminator</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data. Enforcement of a null terminator depends on the value of <paramref name="nullTerminate"/>
        /// </remarks>
        public ConstantDataArray CreateConstantString(string value, bool nullTerminate);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 1</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(bool constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(byte constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant(sbyte constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(Int16 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(UInt16 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(Int32 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(UInt32 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(Int64 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(UInt64 constValue);

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="bitWidth">Bit width of the integer</param>
        /// <param name="constValue">Value for the constant</param>
        /// <param name="signExtend">flag to indicate if the constant value should be sign extended</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public ConstantInt CreateConstant(uint bitWidth, UInt64 constValue, bool signExtend);

        /// <summary>Create a constant value of the specified integer type</summary>
        /// <param name="intType">Integer type</param>
        /// <param name="constValue">value</param>
        /// <param name="signExtend">flag to indicate if <paramref name="constValue"/> is sign extended</param>
        /// <returns>Constant for the specified value</returns>
        public ConstantInt CreateConstant(ITypeRef intType, UInt64 constValue, bool signExtend);

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant(float constValue);

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant(double constValue);

        /// <summary>Creates a simple boolean attribute</summary>
        /// <param name="kind">Id of attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified Id set</returns>
        public AttributeValue CreateAttribute(AttributeKind kind);

        /// <summary>Creates an attribute with an integer value parameter</summary>
        /// <param name="kind">The kind of attribute</param>
        /// <param name="value">Value for the attribute</param>
        /// <remarks>
        /// <para>Not all attributes support a value and those that do don't all support
        /// a full 64bit value. The following table provides the kinds of attributes
        /// accepting a value and the allowed size of the values.</para>
        /// <list type="table">
        /// <listheader><term><see cref="AttributeKind"/></term><term>Bit Length</term></listheader>
        /// <item><term><see cref="AttributeKind.Alignment"/></term><term>32</term></item>
        /// <item><term><see cref="AttributeKind.StackAlignment"/></term><term>32</term></item>
        /// <item><term><see cref="AttributeKind.Dereferenceable"/></term><term>64</term></item>
        /// <item><term><see cref="AttributeKind.DereferenceableOrNull"/></term><term>64</term></item>
        /// </list>
        /// </remarks>
        /// <returns><see cref="AttributeValue"/> with the specified kind and value</returns>
        public AttributeValue CreateAttribute(AttributeKind kind, UInt64 value);

        /// <summary>Adds a valueless named attribute</summary>
        /// <param name="name">Attribute name</param>
        /// <returns><see cref="AttributeValue"/> with the specified name</returns>
        public AttributeValue CreateAttribute(string name);

        /// <summary>Adds a Target specific named attribute with value</summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified name and value</returns>
        public AttributeValue CreateAttribute(string name, string value);

        /// <summary>Create a named <see cref="BasicBlock"/> without inserting it into a function</summary>
        /// <param name="name">Name of the block to create</param>
        /// <returns><see cref="BasicBlock"/> created</returns>
        public BasicBlock CreateBasicBlock(string name);

        /// <summary>Creates a new instance of the <see cref="Module"/> class in this context</summary>
        /// <returns><see cref="Module"/></returns>
        public Module CreateBitcodeModule();

        /// <summary>Creates a new instance of the <see cref="Module"/> class in a given context</summary>
        /// <param name="moduleId">ModuleHandle's ID</param>
        /// <returns><see cref="Module"/></returns>
        public Module CreateBitcodeModule(string moduleId);

        /// <summary>Parse LLVM IR source for a module, into this context</summary>
        /// <param name="src">LLVM IR Source code of the module</param>
        /// <param name="name">Name of the module buffer</param>
        /// <returns>Newly created module parsed from the IR</returns>
        /// <exception cref="LlvmException">Any errors parsing the IR</exception>
        public Module ParseModule(LazyEncodedString src, LazyEncodedString name);

        /// <summary>Gets non-zero IrMetadata kind ID for a given name</summary>
        /// <param name="name">name of the metadata kind</param>
        /// <returns>integral constant for the ID</returns>
        /// <remarks>
        /// These IDs are uniqued across all modules in this context.
        /// </remarks>
        public uint GetMDKindId(string name);

        /// <summary>Opens a <see cref="TargetBinary"/> from a path</summary>
        /// <param name="path">path to the object file binary</param>
        /// <returns>new object file</returns>
        /// <exception cref="System.IO.IOException">File IO failures</exception>
        public TargetBinary OpenBinary(string path);
    }

    internal static class ContextExtensions
    {
        internal static LLVMContextRefAlias GetUnownedHandle(this IContext self)
        {
            if(self is IHandleWrapper<LLVMContextRefAlias> wrapper)
            {
                return wrapper.Handle;
            }
            else if (self is IGlobalHandleOwner<LLVMContextRef> owner)
            {
                // implicitly cast to the alias handle
                return owner.OwnedHandle;
            }
            else
            {
                throw new ArgumentException("Internal Error - Unknown context type!", nameof(self));
            }
        }
    }
}

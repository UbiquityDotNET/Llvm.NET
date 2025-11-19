// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
        ITypeRef VoidType { get; }

        /// <summary>Gets the LLVM boolean type for this context</summary>
        ITypeRef BoolType { get; }

        /// <summary>Gets the LLVM 8 bit integer type for this context</summary>
        ITypeRef Int8Type { get; }

        /// <summary>Gets the LLVM 16 bit integer type for this context</summary>
        ITypeRef Int16Type { get; }

        /// <summary>Gets the LLVM 32 bit integer type for this context</summary>
        ITypeRef Int32Type { get; }

        /// <summary>Gets the LLVM 64 bit integer type for this context</summary>
        ITypeRef Int64Type { get; }

        /// <summary>Gets the LLVM 128 bit integer type for this context</summary>
        ITypeRef Int128Type { get; }

        /// <summary>Gets the LLVM half precision floating point type for this context</summary>
        ITypeRef HalfFloatType { get; }

        /// <summary>Gets the LLVM single precision floating point type for this context</summary>
        ITypeRef FloatType { get; }

        /// <summary>Gets the LLVM double precision floating point type for this context</summary>
        ITypeRef DoubleType { get; }

        /// <summary>Gets the LLVM token type for this context</summary>
        ITypeRef TokenType { get; }

        /// <summary>Gets the LLVM IrMetadata type for this context</summary>
        ITypeRef MetadataType { get; }

        /// <summary>Gets the LLVM X86 80-bit floating point type for this context</summary>
        ITypeRef X86Float80Type { get; }

        /// <summary>Gets the LLVM 128-Bit floating point type</summary>
        ITypeRef Float128Type { get; }

        /// <summary>Gets the LLVM PPC 128-bit floating point type</summary>
        ITypeRef PpcFloat128Type { get; }

        /// <summary>Gets or sets a value indicating whether the context keeps a map for uniqueing debug info identifiers across the context</summary>
        bool OdrUniqueDebugTypes { get; set; }

        /// <summary>Gets or sets a value indicating whether this context is configured to discard value names</summary>
        bool DiscardValueNames { get; set; }

#if HAVE_API_TO_ENUMERATE_METADATA
        /*
        IEnumerable<LazyEncodedString> MDKindNames { get; }
        /*TODO: Create interop calls to support additional properties/methods

        unsigned GetOperandBundleTagId(LazyEncodedString name) {...}
        IEnumerable<LazyEncodedString> OperandBundleTagIds { get; }
        */

        // Underlying LLVM has no mechanism to access the metadata in a context.

        /// <summary>Gets an enumerable collection of all the metadata created in this context</summary>
        IEnumerable<LlvmMetadata> Metadata => MetadataCache;
#endif

        /// <summary>Creates a simple attribute without arguments</summary>
        /// <param name="name">name of the attribute</param>
        /// <returns><see cref="AttributeValue"/> of the attribute</returns>
        AttributeValue CreateAttribute( LazyEncodedString name );

        /// <summary>Creates an attribute with an integer value parameter</summary>
        /// <param name="name">name of the attribute</param>
        /// <param name="value">Value for the attribute</param>
        /// <remarks>
        /// <para>Not all attributes support a value and those that do don't all support
        /// a full 64bit value. Se the LLVM docs for details.</para>
        /// </remarks>
        /// <returns><see cref="AttributeValue"/> with the specified kind and value</returns>
        AttributeValue CreateAttribute( LazyEncodedString name, UInt64 value );

        /// <summary>Create an attribute that accepts a type value</summary>
        /// <param name="name">name of the attribute</param>
        /// <param name="value">Type value to create the attribute with</param>
        /// <returns>Attribute value</returns>
        /// <exception cref="ArgumentException">The <paramref name="name"/> attribute does not accept a type argument</exception>
        AttributeValue CreateAttribute( LazyEncodedString name, ITypeRef value );

        // TODO: Implement support for APInt with customized BigInteger.
        //       BigInteger mostly fits the bill except it doesn't constrain the max value by a specified number of bits
        //       Additionally, it favors representation as an array using the least number of elements possible, which
        //       is NOT the same as how LLVM treats an APInt...
        //
        //       Such a type should have implicit casting to/from integral types if the size matches (signed extending
        //       as appropriate but never a downcast without error...)
        //
        //       Then Creation of a Const Range attribute becomes simpler.

        /// <summary>Create an attribute that accepts a constant range value</summary>
        /// <param name="name">name of the attribute</param>
        /// <param name="numBits">Number of valid bits in the range values</param>
        /// <param name="lowWords">Array of bits composing the low end value of the range</param>
        /// <param name="upperWords">Array of bits composing the upper end value of the range</param>
        /// <returns>Attribute value</returns>
        /// <remarks>
        /// A constant range, as the name implies, contains a range of constant values that are a lower bound
        /// and upper bound for a range. The values of each end are an arbitrary precision integer (APInt)
        /// and thus <paramref name="numBits"/> indicates the total bit width of the values in the range.
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="name"/> does not accept a constant range value</exception>
        AttributeValue CreateAttribute( LazyEncodedString name, UInt32 numBits, UInt64[] lowWords, UInt64[] upperWords );

        // TODO: Figure out how to create a constant range list attribute as LLVM-C API doesn't provide any means to do that.

        /// <summary>Creates a string attribute with a value</summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Value of the property</param>
        /// <returns><see cref="AttributeValue"/> with the specified name</returns>
        AttributeValue CreateAttribute( LazyEncodedString name, LazyEncodedString value );

        /// <summary>Set a custom diagnostic handler</summary>
        /// <param name="handler">handler</param>
        void SetDiagnosticHandler( DiagnosticInfoCallbackAction handler );

        /// <summary>Get's an LLVM integer type of arbitrary bit width</summary>
        /// <param name="bitWidth">Width of the integer type in bits</param>
        /// <remarks>
        /// For standard integer bit widths (e.g. 1,8,16,32,64) this will return
        /// the same type as the corresponding specialized property.
        /// (e.g. GetIntType(1) is the same as <see cref="BoolType"/>,
        ///  GetIntType(16) is the same as <see cref="Int16Type"/>, etc... )
        /// </remarks>
        /// <returns>Integer <see cref="ITypeRef"/> for the specified width</returns>
        ITypeRef GetIntType( uint bitWidth );

#if NET9_0_OR_GREATER
        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        IFunctionType GetFunctionType( ITypeRef returnType, params IEnumerable<ITypeRef> args );
#else
        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        IFunctionType GetFunctionType( ITypeRef returnType, IEnumerable<ITypeRef> args );
#endif

#if NET9_0_OR_GREATER
        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="isVarArgs">Flag to indicate if the method supports C/C++ style VarArgs</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, params IEnumerable<ITypeRef> args );
#else
        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="isVarArgs">Flag to indicate if the method supports C/C++ style VarArgs</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, IEnumerable<ITypeRef> args );
#endif

#if NET9_0_OR_GREATER
        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                            , IDebugType<ITypeRef, DIType> retType
                                            , params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                            );
#else
        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                            , IDebugType<ITypeRef, DIType> retType
                                            , IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                            );
#endif

#if NET9_0_OR_GREATER
        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="isVarArg">Flag to indicate if this function is variadic</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                            , bool isVarArg
                                            , IDebugType<ITypeRef, DIType> retType
                                            , params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                            );
#else
        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="isVarArg">Flag to indicate if this function is variadic</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                            , bool isVarArg
                                            , IDebugType<ITypeRef, DIType> retType
                                            , IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                            );
#endif

#if NET9_0_OR_GREATER
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
        Constant CreateConstantStruct( bool packed, params IEnumerable<Constant> values );
#else
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
        Constant CreateConstantStruct( bool packed, IEnumerable<Constant> values );
#endif

#if NET9_0_OR_GREATER
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
        Constant CreateNamedConstantStruct( IStructType type, params IEnumerable<Constant> values );
#else
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
        Constant CreateNamedConstantStruct( IStructType type, IEnumerable<Constant> values );
#endif

        /// <summary>Create an opaque structure type (e.g. a forward reference)</summary>
        /// <param name="name">Name of the type (use <see cref="LazyEncodedString.Empty"/> for anonymous types)</param>
        /// <remarks>
        /// This method creates an opaque type. The <see cref="IStructType.SetBody"/> method provides a means
        /// to add a body, including indication of packed status, to an opaque type at a later time if the
        /// details of the body are required. (If only pointers to the type are required then the body isn't
        /// required)
        /// </remarks>
        /// <returns>New type</returns>
        IStructType CreateStructType( LazyEncodedString name );

#if NET9_0_OR_GREATER
        /// <summary>Create an anonymous structure type (e.g. Tuple)</summary>
        /// <param name="packed">Flag to indicate if the structure is "packed"</param>
        /// <param name="elements">Types of the fields of the structure</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        IStructType CreateStructType( bool packed, params IEnumerable<ITypeRef> elements );
#else
        /// <summary>Create an anonymous structure type (e.g. Tuple)</summary>
        /// <param name="packed">Flag to indicate if the structure is "packed"</param>
        /// <param name="elements">Types of the fields of the structure</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        IStructType CreateStructType( bool packed, params ITypeRef[] elements );
#endif

#if NET9_0_OR_GREATER
        /// <summary>Creates a new structure type in this <see cref="ContextAlias"/></summary>
        /// <param name="name">Name of the structure (use <see cref="LazyEncodedString.Empty"/> for anonymous types)</param>
        /// <param name="packed">Flag indicating if the structure is packed</param>
        /// <param name="elements">Types for the structures elements in layout order</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        /// <remarks>
        /// If the elements argument list is empty then a complete 0 sized struct is created
        /// </remarks>
        IStructType CreateStructType( LazyEncodedString name, bool packed, params IEnumerable<ITypeRef> elements );
#else
        /// <summary>Creates a new structure type in this <see cref="ContextAlias"/></summary>
        /// <param name="name">Name of the structure (use <see cref="LazyEncodedString.Empty"/> for anonymous types)</param>
        /// <param name="packed">Flag indicating if the structure is packed</param>
        /// <param name="elements">Types for the structures elements in layout order</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        /// <remarks>
        /// If the elements argument list is empty then a complete 0 sized struct is created
        /// </remarks>
        IStructType CreateStructType( LazyEncodedString name, bool packed, params ITypeRef[] elements );
#endif

        /// <summary>Creates a metadata string from the given string</summary>
        /// <param name="value">string to create as metadata</param>
        /// <returns>new metadata string</returns>
        /// <remarks>
        /// if <paramref name="value"/> is <see langword="null"/> then the result
        /// represents an empty string.
        /// </remarks>
        MDString CreateMetadataString( LazyEncodedString? value );

        /// <summary>Create an <see cref="MDNode"/> from a string</summary>
        /// <param name="value">String value</param>
        /// <returns>New node with the string as the first element of the <see cref="MDNode.Operands"/> property (as an MDString)</returns>
        MDNode CreateMDNode( LazyEncodedString value );

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data with a terminating null character. To control the enforcement
        /// of a terminating null character, use the <see cref="CreateConstantString(LazyEncodedString, bool)"/>
        /// overload to specify the intended behavior.
        /// </remarks>
        ConstantDataArray CreateConstantString( LazyEncodedString value );

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <param name="nullTerminate">flag to indicate if the string should include a null terminator</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data. Enforcement of a null terminator depends on the value of <paramref name="nullTerminate"/>
        /// </remarks>
        ConstantDataArray CreateConstantString( LazyEncodedString value, bool nullTerminate );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 1</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( bool constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( byte constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        Constant CreateConstant( sbyte constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( Int16 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( UInt16 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( Int32 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( UInt32 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( Int64 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( UInt64 constValue );

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="bitWidth">Bit width of the integer</param>
        /// <param name="constValue">Value for the constant</param>
        /// <param name="signExtend">flag to indicate if the constant value should be sign extended</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        ConstantInt CreateConstant( uint bitWidth, UInt64 constValue, bool signExtend );

        /// <summary>Create a constant value of the specified integer type</summary>
        /// <param name="intType">Integer type</param>
        /// <param name="constValue">value</param>
        /// <param name="signExtend">flag to indicate if <paramref name="constValue"/> is sign extended</param>
        /// <returns>Constant for the specified value</returns>
        ConstantInt CreateConstant( ITypeRef intType, UInt64 constValue, bool signExtend );

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        ConstantFP CreateConstant( float constValue );

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        ConstantFP CreateConstant( double constValue );

        /// <summary>Create a named <see cref="BasicBlock"/> without inserting it into a function</summary>
        /// <param name="name">Name of the block to create</param>
        /// <returns><see cref="BasicBlock"/> created</returns>
        BasicBlock CreateBasicBlock( LazyEncodedString name );

        /// <summary>Creates a new instance of the <see cref="Module"/> class in this context</summary>
        /// <returns><see cref="Module"/></returns>
        Module CreateBitcodeModule( );

        /// <summary>Creates a new instance of the <see cref="Module"/> class in a given context</summary>
        /// <param name="moduleId">ModuleHandle's ID</param>
        /// <returns><see cref="Module"/></returns>
        Module CreateBitcodeModule( LazyEncodedString moduleId );

        /// <summary>Parse LLVM IR source for a module, into this context</summary>
        /// <param name="src">LLVM IR Source code of the module</param>
        /// <param name="name">Name of the module buffer</param>
        /// <returns>Newly created module parsed from the IR</returns>
        /// <exception cref="LlvmException">Any errors parsing the IR</exception>
        Module ParseModule( LazyEncodedString src, LazyEncodedString name );

        /// <summary>Gets non-zero IrMetadata kind ID for a given name</summary>
        /// <param name="name">name of the metadata kind</param>
        /// <returns>integral constant for the ID</returns>
        /// <remarks>
        /// These IDs are uniqued across all modules in this context.
        /// </remarks>
        uint GetMDKindId( LazyEncodedString name );

        /// <summary>Opens a <see cref="TargetBinary"/> from a path</summary>
        /// <param name="path">path to the object file binary</param>
        /// <returns>new object file</returns>
        /// <exception cref="System.IO.IOException">File IO failures</exception>
        TargetBinary OpenBinary( LazyEncodedString path );
    }

    /// <summary>Utility class to provide extensions to <see cref="IContext"/></summary>
    /// <remarks>
    /// This is used mostly to provide support for legacy runtimes that don't have the `params IEnumerable` language feature.
    /// In such a case, this provides extensions that allow use of a params array and then re-directs to the enumerable
    /// form of the function. This allows source compat of consumers while not trying to leverage support beyond what
    /// is supported by the runtime.
    /// </remarks>
    public static class ContextExtensions
    {
#if !NET9_0_OR_GREATER
        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="self">Instance of context to get a function type</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public static IFunctionType GetFunctionType(this IContext self, ITypeRef returnType, params ITypeRef[] args )
        {
            // this seems redundant but for older runtimes it does an implicit cast to IEnumerable
            return self.GetFunctionType( returnType, args );
        }

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="self">Context instance to get the function from</param>
        /// <param name="isVarArgs">Flag to indicate if the method supports C/C++ style VarArgs</param>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public static IFunctionType GetFunctionType( this IContext self, bool isVarArgs, ITypeRef returnType, params ITypeRef[] args )
        {
            // this seems redundant but for older runtimes it does an implicit cast to IEnumerable
            return self.GetFunctionType( isVarArgs, returnType, args );
        }

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="self">Context instance to get the function from</param>
        /// <param name="diBuilder"><see cref="DIBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public static DebugFunctionType CreateFunctionType( this IContext self
                                                          , IDIBuilder diBuilder
                                                          , IDebugType<ITypeRef, DIType> retType
                                                          , params IDebugType<ITypeRef, DIType>[] argTypes
                                                          )
        {
            return self.CreateFunctionType(diBuilder, retType, argTypes);
        }

        /// <summary>Creates a constant structure from a set of values</summary>
        /// <param name="self">Context instance to get the function from</param>
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
        public static Constant CreateConstantStruct(this IContext self, bool packed, params Constant[] values )
        {
            return self.CreateConstantStruct(packed, values);
        }

        /// <summary>Creates a constant instance of a specified structure type from a set of values</summary>
        /// <param name="self">Context instance to get the function from</param>
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
        public static Constant CreateNamedConstantStruct( this IContext self, IStructType type, params Constant[] values )
        {
            return self.CreateNamedConstantStruct(type, values);
        }
#endif

        // TODO: Is this needed? Owned handles are implicitly castable to the unowned forms
        internal static LLVMContextRefAlias GetUnownedHandle( this IContext self )
        {
            if(self is ContextAlias alias)
            {
                return alias.Handle;
            }
            else if(self is Context ctx)
            {
                // implicitly cast to the alias handle
                return ctx.Handle;
            }
            else
            {
                throw new ArgumentException( "Internal Error - Unknown context type!", nameof( self ) );
            }
        }
    }
}

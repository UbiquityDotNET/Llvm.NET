// -----------------------------------------------------------------------
// <copyright file="Context.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.ObjectFile;
using Ubiquity.NET.Llvm.Properties;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Encapsulates an LLVM context</summary>
    /// <remarks>
    /// <para>A context in LLVM is a container for interning (LLVM refers to this as "uniqueing") various types
    /// and values in the system. This allows running multiple LLVM tool transforms etc.. on different threads
    /// without causing them to collide namespaces and types even if they use the same name (e.g. module one
    /// may have a type Foo, and so does module two but they are completely distinct from each other)</para>
    ///
    /// <para>LLVM Debug information is ultimately all parented to a top level <see cref="DICompileUnit"/> as
    /// the scope, and a compilation unit is bound to a <see cref="BitcodeModule"/>, even though, technically
    /// the types are owned by a Context. Thus to keep things simpler and help make working with debug information
    /// easier. Ubiquity.NET.Llvm encapsulates the native type and the debug type in separate classes that are instances
    /// of the <see cref="IDebugType{NativeT, DebugT}"/> interface </para>
    ///
    /// <note type="note">It is important to be aware of the fact that a Context is not thread safe. The context
    /// itself and the object instances it owns are intended for use by a single thread only. Accessing and
    /// manipulating LLVM objects from multiple threads may lead to race conditions corrupted state and any number
    /// of other undefined issues.</note>
    /// </remarks>
    public sealed class Context
        : DisposableObject
        , IBitcodeModuleFactory
    {
        /// <summary>Initializes a new instance of the <see cref="Context"/> class.Creates a new context</summary>
        public Context( )
            : this( LLVMContextCreate( ) )
        {
        }

        /// <summary>Gets the LLVM void type for this context</summary>
        public ITypeRef VoidType => TypeRef.FromHandle( LLVMVoidTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM boolean type for this context</summary>
        public ITypeRef BoolType => TypeRef.FromHandle( LLVMInt1TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM 8 bit integer type for this context</summary>
        public ITypeRef Int8Type => TypeRef.FromHandle( LLVMInt8TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM 16 bit integer type for this context</summary>
        public ITypeRef Int16Type => TypeRef.FromHandle( LLVMInt16TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM 32 bit integer type for this context</summary>
        public ITypeRef Int32Type => TypeRef.FromHandle( LLVMInt32TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM 64 bit integer type for this context</summary>
        public ITypeRef Int64Type => TypeRef.FromHandle( LLVMInt64TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM half precision floating point type for this context</summary>
        public ITypeRef HalfFloatType => TypeRef.FromHandle( LLVMHalfTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM single precision floating point type for this context</summary>
        public ITypeRef FloatType => TypeRef.FromHandle( LLVMFloatTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM double precision floating point type for this context</summary>
        public ITypeRef DoubleType => TypeRef.FromHandle( LLVMDoubleTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM token type for this context</summary>
        public ITypeRef TokenType => TypeRef.FromHandle( LLVMTokenTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM Metadata type for this context</summary>
        public ITypeRef MetadataType => TypeRef.FromHandle( LLVMMetadataTypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM X86 80-bit floating point type for this context</summary>
        public ITypeRef X86Float80Type => TypeRef.FromHandle( LLVMX86FP80TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM 128-Bit floating point type</summary>
        public ITypeRef Float128Type => TypeRef.FromHandle( LLVMFP128TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets the LLVM PPC 128-bit floating point type</summary>
        public ITypeRef PpcFloat128Type => TypeRef.FromHandle( LLVMPPCFP128TypeInContext( ContextHandle ).ThrowIfInvalid( ) )!;

        /// <summary>Gets an enumerable collection of all the metadata created in this context</summary>
        public IEnumerable<LlvmMetadata> Metadata => MetadataCache;

        /// <summary>Get a type that is a pointer to a value of a given type</summary>
        /// <param name="elementType">Type of value the pointer points to</param>
        /// <returns><see cref="IPointerType"/> for a pointer that references a value of type <paramref name="elementType"/></returns>
        public IPointerType GetPointerTypeFor( ITypeRef elementType )
        {
            elementType.ValidateNotNull( nameof( elementType ) );

            if( elementType.Context != this )
            {
                throw new ArgumentException( Resources.Cannot_mix_types_from_different_contexts, nameof( elementType ) );
            }

            return TypeRef.FromHandle<IPointerType>( LLVMPointerType( elementType.GetTypeRef( ), 0 ).ThrowIfInvalid( ) )!;
        }

        /// <summary>Get's an LLVM integer type of arbitrary bit width</summary>
        /// <param name="bitWidth">Width of the integer type in bits</param>
        /// <remarks>
        /// For standard integer bit widths (e.g. 1,8,16,32,64) this will return
        /// the same type as the corresponding specialized property.
        /// (e.g. GetIntType(1) is the same as <see cref="BoolType"/>,
        ///  GetIntType(16) is the same as <see cref="Int16Type"/>, etc... )
        /// </remarks>
        /// <returns>Integer <see cref="ITypeRef"/> for the specified width</returns>
        public ITypeRef GetIntType( uint bitWidth )
        {
            if( bitWidth == 0 )
            {
                throw new ArgumentException( Resources.Integer_bit_width_must_be_greater_than_0 );
            }

            return bitWidth switch
            {
                1 => BoolType,
                8 => Int8Type,
                16 => Int16Type,
                32 => Int32Type,
                64 => Int64Type,
                _ => TypeRef.FromHandle( LLVMIntTypeInContext( ContextHandle, bitWidth ).ThrowIfInvalid( ) )!,
            };
        }

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Optional set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public IFunctionType GetFunctionType( ITypeRef returnType, params ITypeRef[ ] args )
            => GetFunctionType( returnType, args, false );

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <returns>Signature type for the specified signature</returns>
        public IFunctionType GetFunctionType( ITypeRef returnType, IEnumerable<ITypeRef> args )
            => GetFunctionType( returnType, args, false );

        /// <summary>Get an LLVM Function type (e.g. signature)</summary>
        /// <param name="returnType">Return type of the function</param>
        /// <param name="args">Potentially empty set of function argument types</param>
        /// <param name="isVarArgs">Flag to indicate if the method supports C/C++ style VarArgs</param>
        /// <returns>Signature type for the specified signature</returns>
        public IFunctionType GetFunctionType( ITypeRef returnType, IEnumerable<ITypeRef> args, bool isVarArgs )
        {
            returnType.ValidateNotNull( nameof( returnType ) );
            args.ValidateNotNull( nameof( args ) );

            if( ContextHandle != returnType.Context.ContextHandle )
            {
                throw new ArgumentException( Resources.Mismatched_context, nameof( returnType ) );
            }

            LLVMTypeRef[ ] llvmArgs = args.Select( a => a.GetTypeRef( ) ).ToArray( );
            var signature = LLVMFunctionType( returnType.GetTypeRef( ), llvmArgs, ( uint )llvmArgs.Length, isVarArgs );
            return TypeRef.FromHandle<IFunctionType>( signature.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DebugInfoBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( DebugInfoBuilder diBuilder
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IDebugType<ITypeRef, DIType>[ ] argTypes
                                                   )
            => CreateFunctionType( diBuilder, false, retType, argTypes );

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DebugInfoBuilder"/>to use to create the debug information</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( DebugInfoBuilder diBuilder
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   )
            => CreateFunctionType( diBuilder, false, retType, argTypes );

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DebugInfoBuilder"/>to use to create the debug information</param>
        /// <param name="isVarArg">Flag to indicate if this function is variadic</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( DebugInfoBuilder diBuilder
                                                   , bool isVarArg
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IDebugType<ITypeRef, DIType>[ ] argTypes
                                                   )
            => CreateFunctionType( diBuilder, isVarArg, retType, ( IEnumerable<IDebugType<ITypeRef, DIType>> )argTypes );

        /// <summary>Creates a FunctionType with Debug information</summary>
        /// <param name="diBuilder"><see cref="DebugInfoBuilder"/>to use to create the debug information</param>
        /// <param name="isVarArg">Flag to indicate if this function is variadic</param>
        /// <param name="retType">Return type of the function</param>
        /// <param name="argTypes">Argument types of the function</param>
        /// <returns>Function signature</returns>
        public DebugFunctionType CreateFunctionType( DebugInfoBuilder diBuilder
                                                   , bool isVarArg
                                                   , [ValidatedNotNull] IDebugType<ITypeRef, DIType> retType
                                                   , [ValidatedNotNull] IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   )
        {
            diBuilder.ValidateNotNull( nameof( diBuilder ) );
            retType.ValidateNotNull( nameof( retType ) );
            argTypes.ValidateNotNull( nameof( retType ) );

            if( !retType.HasDebugInfo( ) )
            {
                throw new ArgumentNullException( nameof( retType ), Resources.Return_type_does_not_have_debug_information );
            }

            var nativeArgTypes = new List<ITypeRef>( );
            var debugArgTypes = new List<DIType?>( );
            var msg = new StringBuilder( Resources.One_or_more_parameter_types_are_not_valid );
            bool hasParamErrors = false;

            foreach( var indexedPair in argTypes.Select( ( t, i ) => new { Type = t, Index = i } ) )
            {
                if( indexedPair.Type == null )
                {
                    msg.AppendFormat( CultureInfo.CurrentCulture, Resources.Argument_0_is_null, indexedPair.Index );
                    hasParamErrors = true;
                }
                else
                {
                    nativeArgTypes.Add( indexedPair.Type.NativeType );
                    debugArgTypes.Add( indexedPair.Type.DIType );
                    if( indexedPair.Type.HasDebugInfo( ) )
                    {
                        continue;
                    }

                    msg.AppendFormat( CultureInfo.CurrentCulture, Resources.Argument_0_does_not_contain_debug_type_information, indexedPair.Index );
                    hasParamErrors = true;
                }
            }

            // if any parameters have errors, then provide a hopefully helpful message indicating which one(s)
            if( hasParamErrors )
            {
                throw new ArgumentException( msg.ToString( ), nameof( argTypes ) );
            }

            var llvmType = GetFunctionType( retType.NativeType, nativeArgTypes, isVarArg );

            var diType = diBuilder.CreateSubroutineType( 0, retType.DIType, debugArgTypes );
            Debug.Assert( !diType.IsTemporary, Resources.Assert_Should_have_a_valid_non_temp_type_by_now );

            return new DebugFunctionType( llvmType, diType );
        }

        /// <summary>Creates a constant structure from a set of values</summary>
        /// <param name="packed">Flag to indicate if the structure is packed and no alignment should be applied to the members</param>
        /// <param name="values">Set of values to use in forming the structure</param>
        /// <returns>Newly created <see cref="Constant"/></returns>
        /// <remarks>
        /// The actual concrete return type depends on the parameters provided and will be one of the following:
        /// <list type="table">
        /// <listheader>
        /// <term><see cref="Constant"/> derived type</term><description>Description</description>
        /// </listheader>
        /// <item><term>ConstantAggregateZero</term><description>If all the member values are zero constants</description></item>
        /// <item><term>UndefValue</term><description>If all the member values are UndefValue</description></item>
        /// <item><term>ConstantStruct</term><description>All other cases</description></item>
        /// </list>
        /// </remarks>
        public Constant CreateConstantStruct( bool packed, params Constant[ ] values )
            => CreateConstantStruct( packed, ( IEnumerable<Constant> )values );

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
        public Constant CreateConstantStruct( bool packed, IEnumerable<Constant> values )
        {
            values.ValidateNotNull( nameof( values ) );

            var valueHandles = values.Select( v => v.ValueHandle ).ToArray( );
            var handle = LLVMConstStructInContext( ContextHandle, valueHandles, ( uint )valueHandles.Length, packed );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

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
        public Constant CreateNamedConstantStruct( IStructType type, params Constant[ ] values )
        {
            return CreateNamedConstantStruct( type, ( IEnumerable<Constant> )values );
        }

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
        public Constant CreateNamedConstantStruct( IStructType type, IEnumerable<Constant> values )
        {
            type.ValidateNotNull( nameof( type ) );
            values.ValidateNotNull( nameof( values ) );

            if( type.Context != this )
            {
                throw new ArgumentException( Resources.Cannot_create_named_constant_struct_with_type_from_another_context, nameof( type ) );
            }

            var valueList = values as IList<Constant> ?? values.ToList( );
            var valueHandles = valueList.Select( v => v.ValueHandle ).ToArray( );
            if( type.Members.Count != valueHandles.Length )
            {
                throw new ArgumentException( Resources.Number_of_values_provided_must_match_the_number_of_elements_in_the_specified_type );
            }

            var mismatchedTypes = from indexedVal in valueList.Select( ( v, i ) => new { Value = v, Index = i } )
                                  where indexedVal.Value.NativeType != type.Members[ indexedVal.Index ]
                                  select indexedVal;

            if( mismatchedTypes.Any( ) )
            {
                var msg = new StringBuilder( Resources.One_or_more_values_provided_do_not_match_the_corresponding_member_type_ );
                msg.AppendLine( );
                foreach( var mismatch in mismatchedTypes )
                {
                    msg.AppendFormat( CultureInfo.CurrentCulture
                                    , Resources.MismatchedType_0_member_type_equals_1_value_type_equals_2
                                    , mismatch.Index
                                    , type.Members[ mismatch.Index ]
                                    , valueList[ mismatch.Index ].NativeType
                                    );
                }

                throw new ArgumentException( msg.ToString( ) );
            }

            var handle = LLVMConstNamedStruct( type.GetTypeRef( ), valueHandles, (uint)valueHandles.Length );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Create an empty structure type</summary>
        /// <param name="name">Name of the type</param>
        /// <returns>New type</returns>
        public IStructType CreateStructType( string name )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            var handle = LLVMStructCreateNamed( ContextHandle, name );
            return TypeRef.FromHandle<IStructType>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Create an anonymous structure type (e.g. Tuple)</summary>
        /// <param name="packed">Flag to indicate if the structure is "packed"</param>
        /// <param name="element0">Type of the first field of the structure</param>
        /// <param name="elements">Types of any additional fields of the structure</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        public IStructType CreateStructType( bool packed, [ValidatedNotNull] ITypeRef element0, params ITypeRef[ ] elements )
        {
            element0.ValidateNotNull( nameof( element0 ) );
            elements.ValidateNotNull( nameof( elements ) );

            var llvmArgs = new LLVMTypeRef[ elements.Length + 1 ];
            llvmArgs[ 0 ] = element0.GetTypeRef( );
            for( int i = 1; i < llvmArgs.Length; ++i )
            {
                llvmArgs[ i ] = elements[ i - 1 ].GetTypeRef( );
            }

            var handle = LLVMStructTypeInContext( ContextHandle, llvmArgs, ( uint )llvmArgs.Length, packed );
            return TypeRef.FromHandle<IStructType>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new structure type in this <see cref="Context"/></summary>
        /// <param name="name">Name of the structure</param>
        /// <param name="packed">Flag indicating if the structure is packed</param>
        /// <param name="elements">Types for the structures elements in layout order</param>
        /// <returns>
        /// <see cref="IStructType"/> with the specified body defined.
        /// </returns>
        /// <remarks>
        /// If the elements argument list is empty then an opaque type is created (e.g. a forward reference)
        /// The <see cref="IStructType.SetBody(bool, ITypeRef[])"/> method provides a means to add a body to
        /// an opaque type at a later time if the details of the body are required. (If only pointers to
        /// to the type are required the body isn't required)
        /// </remarks>
        public IStructType CreateStructType( string name, bool packed, params ITypeRef[ ] elements )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            elements.ValidateNotNull( nameof( elements ) );

            var retVal = TypeRef.FromHandle<IStructType>( LLVMStructCreateNamed( ContextHandle, name ).ThrowIfInvalid( ) )!;
            if( elements.Length > 0 )
            {
                retVal.SetBody( packed, elements );
            }

            return retVal;
        }

        /// <summary>Creates a metadata string from the given string</summary>
        /// <param name="value">string to create as metadata</param>
        /// <returns>new metadata string</returns>
        public MDString CreateMetadataString( string? value )
        {
            var handle = LLVMMDStringInContext2( ContextHandle, value, ( uint )( value?.Length ?? 0 ) );
            return new MDString( handle );
        }

        /// <summary>Create an <see cref="MDNode"/> from a string</summary>
        /// <param name="value">String value</param>
        /// <returns>New node with the string as the first element of the <see cref="MDNode.Operands"/> property (as an MDString)</returns>
        public MDNode CreateMDNode( string value )
        {
            value.ValidateNotNullOrWhiteSpace( nameof( value ) );
            var elements = new[ ] { CreateMetadataString( value ).MetadataHandle };
            var hNode = LLVMMDNodeInContext2( ContextHandle, elements, ( uint )elements.Length );
            return MDNode.FromHandle<MDNode>( hNode.ThrowIfInvalid( ) )!;
        }

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data without any terminating null character.
        /// </remarks>
        public ConstantDataArray CreateConstantString( string value )
        {
            value.ValidateNotNull( nameof( value ) );

            var handle = LLVMConstStringInContext( ContextHandle, value, ( uint )value.Length, true );
            return Value.FromHandle<ConstantDataArray>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <param name="nullTerminate">flag to indicate if the string should include a null terminator</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts the string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data without any terminating null character.
        /// </remarks>
        public ConstantDataArray CreateConstantString( string value, bool nullTerminate )
        {
            value.ValidateNotNull( nameof( value ) );
            var handle = LLVMConstStringInContext( ContextHandle, value, ( uint )value.Length, !nullTerminate );
            return Value.FromHandle<ConstantDataArray>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 1</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( bool constValue )
        {
            var handle = LLVMConstInt( BoolType.GetTypeRef( )
                                     , ( ulong )( constValue ? 1 : 0 )
                                     , false
                                     );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( byte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( sbyte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="bitWidth">Bit width of the integer</param>
        /// <param name="constValue">Value for the constant</param>
        /// <param name="signExtend">flag to indicate if the constant value should be sign extended</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( uint bitWidth, UInt64 constValue, bool signExtend )
        {
            var intType = GetIntType( bitWidth );
            return CreateConstant( intType, constValue, signExtend );
        }

        /// <summary>Create a constant value of the specified integer type</summary>
        /// <param name="intType">Integer type</param>
        /// <param name="constValue">value</param>
        /// <param name="signExtend">flag to indicate if <paramref name="constValue"/> is sign extended</param>
        /// <returns>Constant for the specified value</returns>
        public Constant CreateConstant( ITypeRef intType, UInt64 constValue, bool signExtend )
        {
            intType.ValidateNotNull( nameof( intType ) );

            if( intType.Context != this )
            {
                throw new ArgumentException( Resources.Cannot_mix_types_from_different_contexts, nameof( intType ) );
            }

            if( intType.Kind != TypeKind.Integer )
            {
                throw new ArgumentException( Resources.Integer_type_required, nameof( intType ) );
            }

            LLVMValueRef valueRef = LLVMConstInt( intType.GetTypeRef( ), constValue, signExtend );
            return Value.FromHandle<Constant>( valueRef.ThrowIfInvalid( ) )!;
        }

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant( float constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( FloatType.GetTypeRef( ), constValue ).ThrowIfInvalid( ) )!;

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant( double constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( DoubleType.GetTypeRef( ), constValue ).ThrowIfInvalid( ) )!;

        /// <summary>Creates a simple boolean attribute</summary>
        /// <param name="kind">Kind of attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified Kind set</returns>
        public AttributeValue CreateAttribute( AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( kind.RequiresIntValue( ) )
            {
                throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Attribute_0_requires_a_value, kind ), nameof( kind ) );
            }

            var handle = LLVMCreateEnumAttribute( ContextHandle
                                                , kind.GetEnumAttributeId( )
                                                , 0ul
                                                );
            return AttributeValue.FromHandle( this, handle );
        }

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
        public AttributeValue CreateAttribute( AttributeKind kind, UInt64 value )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( !kind.RequiresIntValue( ) )
            {
                throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Attribute_0_does_not_support_a_value, kind ), nameof( kind ) );
            }

            var handle = LLVMCreateEnumAttribute( ContextHandle
                                                , kind.GetEnumAttributeId( )
                                                , value
                                                );
            return AttributeValue.FromHandle( this, handle );
        }

        /// <summary>Adds a valueless named attribute</summary>
        /// <param name="name">Attribute name</param>
        /// <returns><see cref="AttributeValue"/> with the specified name</returns>
        public AttributeValue CreateAttribute( string name ) => CreateAttribute( name, string.Empty );

        /// <summary>Adds a Target specific named attribute with value</summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified name and value</returns>
        public AttributeValue CreateAttribute( string name, string value )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            value.ValidateNotNull( nameof( value ) );

            var handle = LLVMCreateStringAttribute( ContextHandle, name, ( uint )name.Length, value, ( uint )value.Length );
            return AttributeValue.FromHandle( this, handle );
        }

        /// <summary>Create a named <see cref="BasicBlock"/> without inserting it into a function</summary>
        /// <param name="name">Name of the block to create</param>
        /// <returns><see cref="BasicBlock"/> created</returns>
        public BasicBlock CreateBasicBlock( string name )
        {
            return BasicBlock.FromHandle( LLVMCreateBasicBlockInContext( ContextHandle, name ).ThrowIfInvalid( ) )!;
        }

        /// <inheritdoc/>
        public BitcodeModule CreateBitcodeModule( )
        {
            return ModuleCache.CreateBitcodeModule( );
        }

        /// <inheritdoc/>
        public BitcodeModule CreateBitcodeModule( string moduleId )
        {
            return ModuleCache.CreateBitcodeModule( moduleId );
        }

        /// <inheritdoc/>
        public BitcodeModule CreateBitcodeModule( string moduleId
                                                , SourceLanguage language
                                                , string srcFilePath
                                                , string producer
                                                , bool optimized = false
                                                , string compilationFlags = ""
                                                , uint runtimeVersion = 0
                                                )
        {
            return ModuleCache.CreateBitcodeModule( moduleId, language, srcFilePath, producer, optimized, compilationFlags, runtimeVersion );
        }

        /// <summary>Gets the modules created in this context</summary>
        public IEnumerable<BitcodeModule> Modules => ModuleCache;

        /// <summary>Gets non-zero Metadata kind ID for a given name</summary>
        /// <param name="name">name of the metadata kind</param>
        /// <returns>integral constant for the ID</returns>
        /// <remarks>
        /// These IDs are uniqued across all modules in this context.
        /// </remarks>
        public uint GetMDKindId( string name )
            => LLVMGetMDKindIDInContext( ContextHandle, name, name == null ? 0u : ( uint )name.Length );

        /// <summary>Gets or sets a value indicating whether the context keeps a map for uniqueing debug info identifiers across the context</summary>
        public bool OdrUniqueDebugTypes
        {
            get => LibLLVMContextGetIsODRUniquingDebugTypes( ContextHandle );
            set => LibLLVMContextSetIsODRUniquingDebugTypes( ContextHandle, value );
        }

        /// <summary>Opens a <see cref="TargetBinary"/> from a path</summary>
        /// <param name="path">path to the object file binary</param>
        /// <returns>new object file</returns>
        /// <exception cref="System.IO.IOException">File IO failures</exception>
        public TargetBinary OpenBinary( string path )
        {
            return new TargetBinary( new MemoryBuffer( path ), this );
        }

        /*TODO: Create interop calls to support additional properties/methods
        public bool DiscardValueName { get; set; }

        public IEnumerable<string> MDKindNames { get; }

        public unsigned GetOperandBundleTagId(string name) {...}
        public IEnumerable<string> OperandBundleTagIds { get; }
        */

        internal LLVMContextRef ContextHandle { get; }

        /* These interning methods provide unique mapping between the .NET wrappers and the underlying LLVM instances
        // The mapping ensures that any LibLLVM handle is always re-mappable to a exactly one wrapper instance.
        // This helps reduce the number of wrapper instances created and also allows reference equality to work
        // as expected for managed types.
        */

        // looks up an attribute by it's handle, if none is found a new managed wrapper is created
        // The factory contains a reference to this Context to ensure that the proper ownership is
        // maintained. (LLVM has no method of retrieving the context that owns an attribute)
        internal AttributeValue GetAttributeFor( LLVMAttributeRef handle )
        {
            handle.ValidateNotDefault( nameof( handle ) );
            return AttributeValueCache.GetOrCreateItem( handle );
        }

        internal void RemoveModule( BitcodeModule module )
        {
            module.ValidateNotNull( nameof( module ) );
            if( !( module.ModuleHandle is null ) )
            {
                ModuleCache.Remove( module.ModuleHandle );
            }
        }

        internal BitcodeModule GetModuleFor( LLVMModuleRef moduleRef )
        {
            moduleRef.ValidateNotDefault( nameof( moduleRef ) );

            var hModuleContext = LLVMGetModuleContext( moduleRef );
            if( hModuleContext != ContextHandle )
            {
                throw new ArgumentException( Resources.Incorrect_context_for_module );
            }

            // make sure handle to existing module isn't auto disposed.
            return ModuleCache.GetOrCreateItem( moduleRef, h => h.SetHandleAsInvalid( ) );
        }

        internal void RemoveDeletedNode( MDNode node )
        {
            MetadataCache.Remove( node.MetadataHandle );
        }

        internal Value GetValueFor( LLVMValueRef valueRef )
        {
            valueRef.ValidateNotDefault( nameof( valueRef ) );

            return ValueCache.GetOrCreateItem( valueRef );
        }

        internal LlvmMetadata GetNodeFor( LLVMMetadataRef handle )
        {
            handle.ValidateNotDefault( nameof( handle ) );
            return MetadataCache.GetOrCreateItem( handle );
        }

        internal ITypeRef GetTypeFor( LLVMTypeRef typeRef )
        {
            typeRef.ValidateNotDefault( nameof( typeRef ) );
            return TypeCache.GetOrCreateItem( typeRef );
        }

        internal Context( LLVMContextRef contextRef )
        {
            if( contextRef == LLVMContextRef.Zero )
            {
                throw new ArgumentNullException( nameof( contextRef ) );
            }

            ContextHandle = contextRef;
            ContextCache.Add( this );
            ActiveHandler = new WrappedNativeCallback<LLVMDiagnosticHandler>( DiagnosticHandler );
            LLVMContextSetDiagnosticHandler( ContextHandle, ActiveHandler, IntPtr.Zero );
            ValueCache = new ValueCache( this );
            ModuleCache = new BitcodeModule.InterningFactory( this );
            TypeCache = new TypeRef.InterningFactory( this );
            AttributeValueCache = new AttributeValue.InterningFactory( this );
            MetadataCache = new LlvmMetadata.InterningFactory( this );
        }

        /// <inheritdoc />
        protected override void Dispose( bool disposing )
        {
            // disconnect all modules so that any future critical finalization has no impact
            var handles = from m in Modules
                          where !(m.ModuleHandle is null) && !m.ModuleHandle.IsClosed && !m.ModuleHandle.IsInvalid
                          select m.ModuleHandle;

            foreach( var handle in handles )
            {
                handle.SetHandleAsInvalid( );
            }

            ValueCache.Dispose( );
            LLVMContextSetDiagnosticHandler( ContextHandle, null, IntPtr.Zero );
            ActiveHandler.Dispose( );

            ContextCache.Remove( ContextHandle );

            ContextHandle.Dispose( );
        }

        private static void DiagnosticHandler( LLVMDiagnosticInfoRef param0, IntPtr param1 )
        {
            string msg = LLVMGetDiagInfoDescription( param0 );
            var level = LLVMGetDiagInfoSeverity( param0 );
            Debug.WriteLine( "{0}: {1}", level, msg );
            Debug.Assert( level != LLVMDiagnosticSeverity.LLVMDSError, Resources.Assert_Unexpected_Debug_state );
        }

        private readonly WrappedNativeCallback<LLVMDiagnosticHandler> ActiveHandler;

        // child item wrapper factories
        private readonly ValueCache ValueCache;
        private readonly BitcodeModule.InterningFactory ModuleCache;
        private readonly TypeRef.InterningFactory TypeCache;
        private readonly AttributeValue.InterningFactory AttributeValueCache;
        private readonly LlvmMetadata.InterningFactory MetadataCache;
    }
}

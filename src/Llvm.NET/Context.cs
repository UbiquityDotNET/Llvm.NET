// <copyright file="Context.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using Llvm.NET.DebugInfo;
using Llvm.NET.JIT;
using Llvm.NET.Native;
using Llvm.NET.Native.Handles;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Encapsulates an LLVM context</summary>
    /// <remarks>
    /// <para>A context in LLVM is a container for interning (LLVM refers
    /// to this as "uniqueing") various types and values in the system. This
    /// allows running multiple LLVM tool transforms etc.. on different threads
    /// without causing them to collide namespaces and types even if they use
    /// the same name (e.g. module one may have a type Foo, and so does module
    /// two but they are completely distinct from each other)
    /// </para>
    /// <para>LLVM Debug information is ultimately all parented to a top level
    /// <see cref="DICompileUnit"/> as the scope, and a compilation
    /// unit is bound to a <see cref="BitcodeModule"/>, even though, technically
    /// the types are owned by a Context. Thus to keep things simpler and help
    /// make working with debug information easier. Lllvm.NET encapsulates the
    /// native type and the debug type in separate classes that are instances
    /// of the <see cref="IDebugType{NativeT, DebugT}"/> interface </para>
    /// <note type="note">It is important to be aware of the fact that a Context
    /// is not thread safe. The context itself and the object instances it owns
    /// are intended for use by a single thread only. Accessing and manipulating
    /// LLVM objects from multiple threads may lead to race conditions corrupted
    /// state and any number of other undefined issues.</note>
    /// </remarks>
    public sealed class Context
        : DisposableObject
    {
        /// <summary>Initializes a new instance of the <see cref="Context"/> class.Creates a new context</summary>
        public Context( )
            : this( LLVMContextCreate( ) )
        {
        }

        /// <inheritdoc/>
        public override bool IsDisposed => ContextHandle.IsClosed;

        /// <summary>Gets the LLVM void type for this context</summary>
        public ITypeRef VoidType => TypeRef.FromHandle( LLVMVoidTypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM boolean type for this context</summary>
        public ITypeRef BoolType => TypeRef.FromHandle( LLVMInt1TypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM 8 bit integer type for this context</summary>
        public ITypeRef Int8Type => TypeRef.FromHandle( LLVMInt8TypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM 16 bit integer type for this context</summary>
        public ITypeRef Int16Type => TypeRef.FromHandle( LLVMInt16TypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM 32 bit integer type for this context</summary>
        public ITypeRef Int32Type => TypeRef.FromHandle( LLVMInt32TypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM 64 bit integer type for this context</summary>
        public ITypeRef Int64Type => TypeRef.FromHandle( LLVMInt64TypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM half precision floating point type for this context</summary>
        public ITypeRef HalfFloatType => TypeRef.FromHandle( LLVMHalfTypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM single precision floating point type for this context</summary>
        public ITypeRef FloatType => TypeRef.FromHandle( LLVMFloatTypeInContext( ContextHandle ) );

        /// <summary>Gets the LLVM double precision floating point type for this context</summary>
        public ITypeRef DoubleType => TypeRef.FromHandle( LLVMDoubleTypeInContext( ContextHandle ) );

        /// <summary>Gets an enumerable collection of all the metadata created in this context</summary>
        public IEnumerable<LlvmMetadata> Metadata => MetadataCache.Values;

        /// <summary>Get a type that is a pointer to a value of a given type</summary>
        /// <param name="elementType">Type of value the pointer points to</param>
        /// <returns><see cref="IPointerType"/> for a pointer that references a value of type <paramref name="elementType"/></returns>
        public IPointerType GetPointerTypeFor( ITypeRef elementType )
        {
            elementType.ValidateNotNull( nameof( elementType ) );

            if( elementType.Context != this )
            {
                throw new ArgumentException( "Cannot mix types from different contexts", nameof( elementType ) );
            }

            return TypeRef.FromHandle<IPointerType>( LLVMPointerType( elementType.GetTypeRef( ), 0 ) );
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
                throw new ArgumentException( "integer bit width must be greater than 0" );
            }

            switch( bitWidth )
            {
            case 1:
                return BoolType;
            case 8:
                return Int8Type;
            case 16:
                return Int16Type;
            case 32:
                return Int32Type;
            case 64:
                return Int64Type;
            default:
                return TypeRef.FromHandle( LLVMIntTypeInContext( ContextHandle, bitWidth ) );
            }
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
                throw new ArgumentException( "Mismatched context", nameof( returnType ) );
            }

            LLVMTypeRef[ ] llvmArgs = args.Select( a => a.GetTypeRef( ) ).ToArray( );
            int argCount = llvmArgs.Length;

            // have to pass a valid addressable object to native interop
            // so allocate space for a single value but tell LLVM the length is 0
            if( llvmArgs.Length == 0 )
            {
                llvmArgs = new LLVMTypeRef[ 1 ];
            }

            var signature = LLVMFunctionType( returnType.GetTypeRef( ), out llvmArgs[ 0 ], ( uint )argCount, isVarArgs );
            return TypeRef.FromHandle<IFunctionType>( signature );
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
                throw new ArgumentNullException( nameof( retType ), "Return type does not have debug information" );
            }

            var nativeArgTypes = new List<ITypeRef>( );
            var debugArgTypes = new List<DIType>( );
            var msg = new StringBuilder( "One or more parameter types are not valid:\n" );
            bool hasParamErrors = false;

            foreach( var indexedPair in argTypes.Select( ( t, i ) => new { Type = t, Index = i } ) )
            {
                if( indexedPair.Type == null )
                {
                    msg.AppendFormat( "\tArgument {0} is null", indexedPair.Index );
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

                    msg.AppendFormat( "\tArgument {0} does not contain debug type information", indexedPair.Index );
                    hasParamErrors = true;
                }
            }

            // if any parameters don't have errors, then provide a hopefully helpful message indicating which one(s)
            if( hasParamErrors )
            {
                throw new ArgumentException( msg.ToString( ), nameof( argTypes ) );
            }

            var llvmType = GetFunctionType( retType.NativeType, nativeArgTypes, isVarArg );

            var diType = diBuilder.CreateSubroutineType( 0, retType.DIType, debugArgTypes );
            Debug.Assert( diType != null && !diType.IsTemporary, "Should have a valid non temp type by now");

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
            if( valueHandles.Length == 0 )
            {
                throw new ArgumentException( "structure must have at least one element", nameof( values ) );
            }

            var handle = LLVMConstStructInContext( ContextHandle, out valueHandles[ 0 ], ( uint )valueHandles.Length, packed );
            return Value.FromHandle<Constant>( handle );
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
                throw new ArgumentException( "Cannot create named constant struct with type from another context", nameof( type ) );
            }

            var valueList = values as IList<Constant> ?? values.ToList( );
            var valueHandles = valueList.Select( v => v.ValueHandle ).ToArray( );
            if( type.Members.Count != valueHandles.Length )
            {
                throw new ArgumentException( "Number of values provided must match the number of elements in the specified type" );
            }

            var mismatchedTypes = from indexedVal in valueList.Select( ( v, i ) => new { Value = v, Index = i } )
                                  where indexedVal.Value.NativeType != type.Members[ indexedVal.Index ]
                                  select indexedVal;

            if( mismatchedTypes.Any( ) )
            {
                var msg = new StringBuilder( "One or more values provided do not match the corresponding member type:" );
                msg.AppendLine( );
                foreach( var mismatch in mismatchedTypes )
                {
                    msg.AppendFormat( "\t[{0}]: member type={1}; value type={2}"
                                    , mismatch.Index
                                    , type.Members[ mismatch.Index ]
                                    , valueList[ mismatch.Index ].NativeType
                                    );
                }

                throw new ArgumentException( msg.ToString( ) );
            }

            // To interop correctly, we need to have an array of at least size one.
            uint valuesLength = (uint)valueHandles.Length;
            if( valuesLength == 0 )
            {
                valueHandles = new LLVMValueRef[1];
            }

            var handle = LLVMConstNamedStruct( type.GetTypeRef( ), out valueHandles[ 0 ], valuesLength );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Create an empty structure type</summary>
        /// <param name="name">Name of the type</param>
        /// <returns>New type</returns>
        public IStructType CreateStructType( string name )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            var handle = LLVMStructCreateNamed( ContextHandle, name );
            return TypeRef.FromHandle<IStructType>( handle );
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

            LLVMTypeRef[ ] llvmArgs = new LLVMTypeRef[ elements.Length + 1 ];
            llvmArgs[ 0 ] = element0.GetTypeRef( );
            for( int i = 1; i < llvmArgs.Length; ++i )
            {
                llvmArgs[ i ] = elements[ i - 1 ].GetTypeRef( );
            }

            var handle = LLVMStructTypeInContext( ContextHandle, out llvmArgs[ 0 ], ( uint )llvmArgs.Length, packed );
            return TypeRef.FromHandle<IStructType>( handle );
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

            var retVal = TypeRef.FromHandle<IStructType>( LLVMStructCreateNamed( ContextHandle, name ) );
            if( elements.Length > 0 )
            {
                retVal.SetBody( packed, elements );
            }

            return retVal;
        }

        /// <summary>Creates a metadata string from the given string</summary>
        /// <param name="value">string to create as metadata</param>
        /// <returns>new metadata string</returns>
        public MDString CreateMetadataString( [CanBeNull] string value )
        {
            value = value ?? string.Empty;
            var handle = LLVMMDString2( ContextHandle, value, ( uint )value.Length );
            return new MDString( handle );
        }

        /// <summary>Create a constant data string value</summary>
        /// <param name="value">string to convert into an LLVM constant value</param>
        /// <returns>new <see cref="ConstantDataArray"/></returns>
        /// <remarks>
        /// This converts th string to ANSI form and creates an LLVM constant array of i8
        /// characters for the data without any terminating null character.
        /// </remarks>
        public ConstantDataArray CreateConstantString( string value )
        {
            value.ValidateNotNull( nameof( value ) );

            var handle = LLVMConstStringInContext( ContextHandle, value, ( uint )value.Length, true );
            return Value.FromHandle<ConstantDataArray>( handle );
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
            return Value.FromHandle<ConstantDataArray>( handle );
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
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( byte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 8</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( sbyte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 16</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 32</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( Int64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="constValue">Value for the constant</param>
        /// <returns><see cref="ConstantInt"/> representing the value</returns>
        public Constant CreateConstant( UInt64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<Constant>( handle );
        }

        /// <summary>Creates a new <see cref="ConstantInt"/> with a bit length of 64</summary>
        /// <param name="bitWidth">Bit width of the integer</param>
        /// <param name="constValue">Value for the constant</param>
        /// <param name="signExtend">flag to indicate if the const value should be sign extended</param>
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
                throw new ArgumentException( "Cannot mix types from different contexts", nameof( intType ) );
            }

            if( intType.Kind != TypeKind.Integer )
            {
                throw new ArgumentException( "Integer type required", nameof( intType ) );
            }

            return Value.FromHandle<Constant>( LLVMConstInt( intType.GetTypeRef( ), constValue, signExtend ) );
        }

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant( float constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( FloatType.GetTypeRef( ), constValue ) );

        /// <summary>Creates a constant floating point value for a given value</summary>
        /// <param name="constValue">Value to make into a <see cref="ConstantFP"/></param>
        /// <returns>Constant value</returns>
        public ConstantFP CreateConstant( double constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( DoubleType.GetTypeRef( ), constValue ) );

        /// <summary>Creates a simple boolean attribute</summary>
        /// <param name="kind">Kind of attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified Kind set</returns>
        public AttributeValue CreateAttribute( AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( kind.RequiresIntValue( ) )
            {
                throw new ArgumentException( $"Attribute {kind} requires a value", nameof( kind ) );
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
                throw new ArgumentException( $"Attribute {kind} does not support a value", nameof( kind ) );
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
        public AttributeValue CreateAttribute( string name ) => CreateAttribute( name, string.Empty);

        /// <summary>Adds a Target specific named attribute with value</summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <returns><see cref="AttributeValue"/> with the specified name and value</returns>
        public AttributeValue CreateAttribute( string name, string value )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );
            value.ValidateNotNull( nameof( value ) );

            var handle = LLVMCreateStringAttribute( ContextHandle, name, ( uint )name.Length, value, ( uint )value.Length );
            return AttributeValue.FromHandle(this, handle );
        }

        /// <summary>Create a named <see cref="BasicBlock"/> in a given context</summary>
        /// <param name="name">Name of the block to create</param>
        /// <param name="parentFuntion">Parent function (or <see lang="null"/> if no parent)</param>
        /// <param name="insertBefore">Optional block to insert the new block in front of</param>
        /// <returns><see cref="BasicBlock"/> created</returns>
        public BasicBlock CreateBasicBlock( string name, [CanBeNull] Function parentFuntion = null, [CanBeNull] BasicBlock insertBefore = null )
        {
            return BasicBlock.FromHandle( LLVMContextCreateBasicBlock( ContextHandle
                                                                     , name
                                                                     , parentFuntion?.ValueHandle ?? default
                                                                     , insertBefore?.BlockHandle ?? default
                                                                     )
                                        );
        }

        /*TODO:
        public unsigned GetMDKindId(string name) {...}
        public IEnumerable<string> MDKindNames { get; }

        public unsigned GetOperandBundleTagId(string name) {...}
        public IEnumerable<string> OperandBundleTagIds { get; }

        public bool ODRUniqueDebugTypes { get; set; }

        public OptBisect OptBisec { get; set; }
        */

        // looks up an attribute by it's handle, if none is found a new managed wrapper is created
        // The factory as a Func<> allows for the constructor to remain private so that the only
        // way to create an AttributeValue is via the containing context. This ensures that the
        // proper ownership is maintained. (LLVM has no method of retrieving the context that owns an attribute)
        // TODO: move this to an attribute interning factory bound to the context
        internal AttributeValue GetAttributeFor( LLVMAttributeRef handle, Func<Context, LLVMAttributeRef, AttributeValue> factory )
        {
            factory.ValidateNotNull( nameof( factory ) );
            if( handle == default )
            {
                return default;
            }

            if( AttributeValueCache.TryGetValue( handle, out AttributeValue retVal ) )
            {
                return retVal;
            }

            retVal = factory( this, handle );
            AttributeValueCache.Add( handle, retVal );
            return retVal;
        }

        internal LLVMContextRef ContextHandle { get; private set; }

        /* These interning methods provide unique mapping between the .NET wrappers and the underlying LLVM instances
        // The mapping ensures that any LibLLVM handle is always re-mappable to a exactly one wrapper instance.
        // This helps reduce the number of wrapper instances created and also allows reference equality to work
        // as expected for managed types.
        // TODO: Refactor the interning to class dedicated to managing the mappings, this can allow looking up the
        // context from handles where there isn't any APIs to retrieve the Context.
        */

        internal void AddModule( BitcodeModule module )
        {
            ModuleCache.Add( module.ModuleHandle, module );
        }

        internal void RemoveModule( BitcodeModule module )
        {
            ModuleCache.Remove( module.ModuleHandle );
        }

        internal BitcodeModule GetModuleFor( LLVMModuleRef moduleRef )
        {
            if( moduleRef == default )
            {
                throw new ArgumentNullException( nameof( moduleRef ) );
            }

            var hModuleContext = LLVMGetModuleContext( moduleRef );
            if( hModuleContext != ContextHandle )
            {
                throw new ArgumentException( "Incorrect context for module" );
            }

            if( !ModuleCache.TryGetValue( moduleRef, out BitcodeModule retVal ) )
            {
                retVal = new BitcodeModule( moduleRef );
            }

            return retVal;
        }

        internal void RemoveDeletedNode( MDNode node )
        {
            MetadataCache.Remove( node.MetadataHandle );
        }

        internal Value GetValueFor( LLVMValueRef valueRef )
        {
            valueRef.ValidateNotDefault( nameof( valueRef ) );

            return ValueCache.GetItemFor( valueRef, this );
        }

        internal LegacyExecutionEngine GetEngineFor( LLVMExecutionEngineRef h )
        {
            h.ValidateNotDefault( nameof( h ) );
            return EngineCache.GetItemFor( h, this );
        }

        internal LlvmMetadata GetNodeFor( LLVMMetadataRef handle, Func<LLVMMetadataRef, LlvmMetadata> staticFactory )
        {
            if( handle == default )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if( MetadataCache.TryGetValue( handle, out LlvmMetadata retVal ) )
            {
                return retVal;
            }

            retVal = staticFactory( handle );
            MetadataCache.Add( handle, retVal );
            return retVal;
        }

        internal MDOperand GetOperandFor( MDNode owningNode, LLVMMDOperandRef handle )
        {
            if( owningNode.Context != this )
            {
                throw new ArgumentException( "Cannot get operand for a node from a different context", nameof( owningNode ) );
            }

            if( handle == default )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if( MDOperandCache.TryGetValue( handle, out MDOperand retVal ) )
            {
                return retVal;
            }

            retVal = new MDOperand( owningNode, handle );
            MDOperandCache.Add( handle, retVal );
            return retVal;
        }

        internal ITypeRef GetTypeFor( LLVMTypeRef valueRef, Func<LLVMTypeRef, ITypeRef> constructor )
        {
            if( valueRef == default )
            {
                throw new ArgumentNullException( nameof( valueRef ) );
            }

            if( TypeCache.TryGetValue( valueRef, out ITypeRef retVal ) )
            {
                return retVal;
            }

            retVal = constructor( valueRef );
            TypeCache.Add( valueRef, retVal );
            return retVal;
        }

        internal Context( LLVMContextRef contextRef )
        {
            ContextHandle = contextRef;
            ContextCache.Add( this );
            ActiveHandler = new WrappedNativeCallback( new LLVMDiagnosticHandler( DiagnosticHandler ) );
            LLVMContextSetDiagnosticHandler( ContextHandle, ActiveHandler.GetFuncPointer( ), IntPtr.Zero );
        }

        protected override void InternalDispose( bool disposing )
        {
            // make sure engines are disposed before disposing the context
            // as they hold on to all owned Modules, which are ultimately destroyed
            // when the context is, so when the GC finalizes the execution engine
            // the modules are already destroyed triggering a double free.
            EngineCache.Clear( );
            LLVMContextSetDiagnosticHandler( ContextHandle, IntPtr.Zero, IntPtr.Zero );
            ActiveHandler.Dispose( );

            ContextCache.Remove( ContextHandle );

            ContextHandle.Dispose( );
        }

        private void DiagnosticHandler( LLVMDiagnosticInfoRef param0, IntPtr param1 )
        {
            string msg = LLVMGetDiagInfoDescription( param0 );
            var level = LLVMGetDiagInfoSeverity( param0 );
            Debug.WriteLine( "{0}: {1}", level, msg );
            Debug.Assert( level != LLVMDiagnosticSeverity.LLVMDSError, "Unexpected Debug state" );
        }

        private WrappedNativeCallback ActiveHandler;

        private readonly IHandleInterning<LLVMValueRef, Value> ValueCache = Value.CreateInterningFactory();

        private readonly IHandleInterning<LLVMExecutionEngineRef, LegacyExecutionEngine> EngineCache = LegacyExecutionEngine.CreateInterningFactory();

        private readonly Dictionary< LLVMTypeRef, ITypeRef > TypeCache = new Dictionary< LLVMTypeRef, ITypeRef >( );

        private readonly Dictionary< LLVMModuleRef, BitcodeModule > ModuleCache = new Dictionary< LLVMModuleRef, BitcodeModule >( );

        private readonly Dictionary< LLVMAttributeRef, AttributeValue> AttributeValueCache = new Dictionary<LLVMAttributeRef, AttributeValue>( );

        private readonly Dictionary< LLVMMetadataRef, LlvmMetadata > MetadataCache = new Dictionary< LLVMMetadataRef, LlvmMetadata >( );

        private readonly Dictionary< LLVMMDOperandRef, MDOperand > MDOperandCache = new Dictionary< LLVMMDOperandRef, MDOperand >( );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern LLVMBasicBlockRef LLVMContextCreateBasicBlock( LLVMContextRef context, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMValueRef /*Function*/ function, LLVMBasicBlockRef insertBefore );
    }
}

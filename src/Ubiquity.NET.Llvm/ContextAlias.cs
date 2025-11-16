// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Llvm.ObjectFile;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ContextBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.IrReader;

namespace Ubiquity.NET.Llvm
{
    // internal type for an unowned (alias) context
    internal sealed class ContextAlias
        : IContext
        , IEquatable<ContextAlias>
    {
        #region IEquatable<T>

        /// <inheritdoc/>
        public bool Equals( IContext? other ) => other is not null && Handle.Equals( other.GetUnownedHandle() );

        /// <inheritdoc/>
        public bool Equals( ContextAlias? other ) => other is not null && Handle.Equals( other.Handle );

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is ContextAlias alias
                                                  ? Equals( alias )
                                                  : Equals( obj as IContext );

        /// <inheritdoc/>
        public override int GetHashCode( ) => Handle.GetHashCode();

        #endregion

        public ITypeRef VoidType => LLVMVoidTypeInContext( Handle ).CreateType();

        public ITypeRef BoolType => LLVMInt1TypeInContext( Handle ).CreateType();

        public ITypeRef Int8Type => LLVMInt8TypeInContext( Handle ).CreateType();

        public ITypeRef Int16Type => LLVMInt16TypeInContext( Handle ).CreateType();

        public ITypeRef Int32Type => LLVMInt32TypeInContext( Handle ).CreateType();

        public ITypeRef Int64Type => LLVMInt64TypeInContext( Handle ).CreateType();

        public ITypeRef Int128Type => LLVMInt128TypeInContext( Handle ).CreateType();

        public ITypeRef HalfFloatType => LLVMHalfTypeInContext( Handle ).CreateType();

        public ITypeRef FloatType => LLVMFloatTypeInContext( Handle ).CreateType();

        public ITypeRef DoubleType => LLVMDoubleTypeInContext( Handle ).CreateType();

        public ITypeRef TokenType => LLVMTokenTypeInContext( Handle ).CreateType();

        public ITypeRef MetadataType => LLVMMetadataTypeInContext( Handle ).CreateType();

        public ITypeRef X86Float80Type => LLVMX86FP80TypeInContext( Handle ).CreateType();

        public ITypeRef Float128Type => LLVMFP128TypeInContext( Handle ).CreateType();

        public ITypeRef PpcFloat128Type => LLVMPPCFP128TypeInContext( Handle ).CreateType();

#if HAVE_API_TO_ENUMERATE_METADATA
        // Underlying LLVM has no mechanism to access the metadata in a context.

        public IEnumerable<LlvmMetadata> Metadata => MetadataCache;
#endif

        public AttributeValue CreateAttribute( LazyEncodedString name )
        {
            name.ThrowIfNullOrEmpty();
            var attribInfo = AttributeInfo.From(name);

            // There is no validation of string attributes
            // and all unknown names are considered custom string
            // attributes. So just treat it as one with an empty
            // string arg value.
            if(attribInfo.ArgKind == AttributeArgKind.String)
            {
                return InternalCreateStringAttribute( name );
            }

            if(attribInfo.HasArg)
            {
                ThrowAttributeRequiresArg( attribInfo, name );
            }

            return new( LLVMCreateEnumAttribute( Handle, attribInfo.ID, 0u ) );
        }

        public AttributeValue CreateAttribute( LazyEncodedString name, ulong value )
        {
            name.ThrowIfNullOrEmpty();
            var attribInfo = AttributeInfo.From(name);

            if(attribInfo.ArgKind != AttributeArgKind.Int)
            {
                ThrowAttributeRequiresArg( attribInfo, name );
            }

            return new( LLVMCreateEnumAttribute( Handle, attribInfo.ID, value ) );
        }

        public AttributeValue CreateAttribute( LazyEncodedString name, ITypeRef value )
        {
            name.ThrowIfNullOrEmpty();
            var attribInfo = AttributeInfo.From(name);

            if(attribInfo.ArgKind != AttributeArgKind.Type)
            {
                ThrowAttributeRequiresArg( attribInfo, name );
            }

            return new( LLVMCreateTypeAttribute( Handle, attribInfo.ID, value.GetTypeRef() ) );
        }

        public AttributeValue CreateAttribute( LazyEncodedString name, uint numBits, ulong[] lowWords, ulong[] upperWords )
        {
            name.ThrowIfNullOrEmpty();
            var attribInfo = AttributeInfo.From(name);

            if(attribInfo.ArgKind != AttributeArgKind.ConstantRange)
            {
                ThrowAttributeRequiresArg( attribInfo, name );
            }

            return new( LLVMCreateConstantRangeAttribute( Handle, attribInfo.ID, numBits, lowWords, upperWords ) );
        }

        public AttributeValue CreateAttribute( LazyEncodedString name, LazyEncodedString? value = null )
        {
            name.ThrowIfNullOrEmpty();

            var attribInfo = AttributeInfo.From(name);
            if(attribInfo.ArgKind != AttributeArgKind.String)
            {
                ThrowAttributeRequiresArg( attribInfo, name );
            }

            return InternalCreateStringAttribute( name, value );
        }

        public void SetDiagnosticHandler( DiagnosticInfoCallbackAction handler )
        {
            unsafe
            {
                void* ctx = null;
                try
                {
                    ctx = handler.AsNativeContext();
                    LLVMContextSetDiagnosticHandler( Handle, &DiagnosticCallbacks.DiagnosticHandler, ctx );
                }
                catch when (ctx is not null)
                {
                    NativeContext.Release(ctx);
                    throw;
                }
            }
        }

        public ITypeRef GetIntType( uint bitWidth )
        {
            ArgumentOutOfRangeException.ThrowIfZero( bitWidth );

            return bitWidth switch
            {
                1 => BoolType,
                8 => Int8Type,
                16 => Int16Type,
                32 => Int32Type,
                64 => Int64Type,
                128 => Int128Type,
                _ => LLVMIntTypeInContext( Handle, bitWidth ).CreateType(),
            };
        }

#if NET9_0_OR_GREATER
        public IFunctionType GetFunctionType( ITypeRef returnType, params IEnumerable<ITypeRef> args )
#else
        public IFunctionType GetFunctionType( ITypeRef returnType, IEnumerable<ITypeRef> args )
#endif
        {
            return GetFunctionType( isVarArgs: false, returnType, args );
        }

#if NET9_0_OR_GREATER
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, params IEnumerable<ITypeRef> args )
#else
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, IEnumerable<ITypeRef> args )
#endif
        {
            ArgumentNullException.ThrowIfNull( returnType );
            ArgumentNullException.ThrowIfNull( args );

            if(!Equals( returnType.Context ))
            {
                throw new ArgumentException( Resources.Mismatched_context, nameof( returnType ) );
            }

            LLVMTypeRef[ ] llvmArgs = [ .. args.Select( a => a.GetTypeRef( ) ) ];
            var signature = LLVMFunctionType( returnType.GetTypeRef( ), llvmArgs, ( uint )llvmArgs.Length, isVarArgs );
            return (IFunctionType)signature.CreateType();
        }

        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IDebugType<ITypeRef, DIType>[] argTypes
                                                   )
        {
            return CreateFunctionType( diBuilder, false, retType, argTypes );
        }

        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   )
        {
            return CreateFunctionType( diBuilder, false, retType, argTypes );
        }

        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                                   , bool isVarArg
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IDebugType<ITypeRef, DIType>[] argTypes
                                                   )
        {
            return CreateFunctionType( diBuilder, isVarArg, retType, (IEnumerable<IDebugType<ITypeRef, DIType>>)argTypes );
        }

#if NET9_0_OR_GREATER
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                                   , bool isVarArg
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   )
#else
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder
                                                   , bool isVarArg
                                                   , IDebugType<ITypeRef, DIType> retType
                                                   , IEnumerable<IDebugType<ITypeRef, DIType>> argTypes
                                                   )
#endif
        {
            ArgumentNullException.ThrowIfNull( retType );
            ArgumentNullException.ThrowIfNull( argTypes );

            if(!retType.HasDebugInfo())
            {
                throw new ArgumentNullException( nameof( retType ), Resources.Return_type_does_not_have_debug_information );
            }

            // Validate enumerable elements to ensure they are all valid and won't crash.
            var msg = new StringBuilder( Resources.One_or_more_parameter_types_are_not_valid );
            bool hasParamErrors = false;

            foreach(var indexedPair in argTypes.Select( ( t, i ) => new { Type = t, Index = i } ))
            {
                // If any element of the parameter types enumerable is null, that's an error
                if(indexedPair.Type == null)
                {
                    msg.AppendFormat( CultureInfo.CurrentCulture, Resources.Argument_0_is_null, indexedPair.Index );
                    hasParamErrors = true;
                }
                else
                {
                    // if any element is missing debug information - that's an error
                    if(!indexedPair.Type.HasDebugInfo())
                    {
                        msg.AppendFormat( CultureInfo.CurrentCulture, Resources.Argument_0_does_not_contain_debug_type_information, indexedPair.Index );
                        hasParamErrors = true;
                    }
                }
            }

            // if any parameters have errors, then provide a, hopefully, helpful message indicating which one(s)
            if(hasParamErrors)
            {
                throw new ArgumentException( msg.ToString(), nameof( argTypes ) );
            }

            // Input validation complete, now do the actual work...
            var llvmType = GetFunctionType( isVarArg , retType, argTypes);
            return new DebugFunctionType( llvmType, diBuilder, DebugInfoFlags.None, retType, argTypes );
        }

#if NET9_0_OR_GREATER
        public Constant CreateConstantStruct( bool packed, params IEnumerable<Constant> values )
#else
        public Constant CreateConstantStruct( bool packed, IEnumerable<Constant> values )
#endif
        {
            ArgumentNullException.ThrowIfNull( values );

            var valueHandles = values.Select( v => v.Handle ).ToArray( );
            var handle = LLVMConstStructInContext( Handle, valueHandles, ( uint )valueHandles.Length, packed );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid() )!;
        }

        public Constant CreateNamedConstantStruct( IStructType type, params Constant[] values )
        {
            return CreateNamedConstantStruct( type, (IEnumerable<Constant>)values );
        }

#if NET9_0_OR_GREATER
        public Constant CreateNamedConstantStruct( IStructType type, params IEnumerable<Constant> values )
#else
        public Constant CreateNamedConstantStruct( IStructType type, IEnumerable<Constant> values )
#endif
        {
            ArgumentNullException.ThrowIfNull( type );
            ArgumentNullException.ThrowIfNull( values );

            if(!Equals( type.Context ))
            {
                throw new ArgumentException( Resources.Cannot_create_named_constant_struct_with_type_from_another_context, nameof( type ) );
            }

            var valueList = values as IList<Constant> ?? [ .. values ];
            var valueHandles = valueList.Select( v => v.Handle ).ToArray( );
            if(type.Members.Count != valueHandles.Length)
            {
                throw new ArgumentException( Resources.Number_of_values_provided_must_match_the_number_of_elements_in_the_specified_type );
            }

            var mismatchedTypes = from indexedVal in valueList.Select( ( v, i ) => new { Value = v, Index = i } )
                                  where !indexedVal.Value.NativeType.Equals(type.Members[ indexedVal.Index ])
                                  select indexedVal;

            if(mismatchedTypes.Any())
            {
                var msg = new StringBuilder( Resources.One_or_more_values_provided_do_not_match_the_corresponding_member_type_ );
                msg.AppendLine();
                foreach(var mismatch in mismatchedTypes)
                {
                    msg.AppendFormat( CultureInfo.CurrentCulture
                                    , Resources.MismatchedType_0_member_type_equals_1_value_type_equals_2
                                    , mismatch.Index
                                    , type.Members[ mismatch.Index ]
                                    , valueList[ mismatch.Index ].NativeType
                                    );
                }

                throw new ArgumentException( msg.ToString() );
            }

            var handle = LLVMConstNamedStruct( type.GetTypeRef( ), valueHandles, (uint)valueHandles.Length );
            return Value.FromHandle<Constant>( handle.ThrowIfInvalid() )!;
        }

        public IStructType CreateStructType( LazyEncodedString name )
        {
            ArgumentNullException.ThrowIfNull( name );
            var handle = LLVMStructCreateNamed( Handle, name );
            return (IStructType)handle.CreateType();
        }

#if NET9_0_OR_GREATER
        public IStructType CreateStructType( bool packed, params IEnumerable<ITypeRef> elements )
#else
        public IStructType CreateStructType( bool packed, params ITypeRef[] elements )
#endif
        {
            ArgumentNullException.ThrowIfNull( elements );

            LLVMTypeRef[ ] llvmArgs = [ .. elements.Select( e => e.GetTypeRef() ) ];
            var handle = LLVMStructTypeInContext( Handle, llvmArgs, ( uint )llvmArgs.Length, packed );
            return (IStructType)handle.CreateType();
        }

#if NET9_0_OR_GREATER
        public IStructType CreateStructType( LazyEncodedString name, bool packed, params IEnumerable<ITypeRef> elements )
#else
        public IStructType CreateStructType( LazyEncodedString name, bool packed, params ITypeRef[] elements )
#endif
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( elements );

            var retVal = (IStructType)LLVMStructCreateNamed( Handle, name ).CreateType();
            retVal.SetBody( packed, elements );

            return retVal;
        }

        public MDString CreateMetadataString( LazyEncodedString? value )
        {
            var handle = LLVMMDStringInContext2( Handle, value ?? LazyEncodedString.Empty );
            return new MDString( handle );
        }

        public MDNode CreateMDNode( LazyEncodedString value )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( value );
            var elements = new[ ] { CreateMetadataString( value ).Handle };
            var hNode = LLVMMDNodeInContext2( Handle, elements, ( uint )elements.Length );
            return (MDNode)hNode.ThrowIfInvalid().CreateMetadata()!;
        }

        public ConstantDataArray CreateConstantString( LazyEncodedString value ) => CreateConstantString( value, true );

        public ConstantDataArray CreateConstantString( LazyEncodedString value, bool nullTerminate )
        {
            ArgumentNullException.ThrowIfNull( value );
            var handle = LLVMConstStringInContext( Handle, value, !nullTerminate );
            return Value.FromHandle<ConstantDataArray>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( bool constValue )
        {
            var handle = LLVMConstInt( BoolType.GetTypeRef( )
                                     , ( ulong )( constValue ? 1 : 0 )
                                     , false
                                     );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( byte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public Constant CreateConstant( sbyte constValue )
        {
            var handle = LLVMConstInt( Int8Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( Int16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( UInt16 constValue )
        {
            var handle = LLVMConstInt( Int16Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( Int32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( UInt32 constValue )
        {
            var handle = LLVMConstInt( Int32Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( Int64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), ( ulong )constValue, true );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( UInt64 constValue )
        {
            var handle = LLVMConstInt( Int64Type.GetTypeRef( ), constValue, false );
            return Value.FromHandle<ConstantInt>( handle.ThrowIfInvalid() )!;
        }

        public ConstantInt CreateConstant( uint bitWidth, UInt64 constValue, bool signExtend )
        {
            var intType = GetIntType( bitWidth );
            return CreateConstant( intType, constValue, signExtend );
        }

        public ConstantInt CreateConstant( ITypeRef intType, UInt64 constValue, bool signExtend )
        {
            ArgumentNullException.ThrowIfNull( intType );

            if(!intType.Context.Equals( this ))
            {
                throw new ArgumentException( Resources.Cannot_mix_types_from_different_contexts, nameof( intType ) );
            }

            if(intType.Kind != TypeKind.Integer)
            {
                throw new ArgumentException( Resources.Integer_type_required, nameof( intType ) );
            }

            LLVMValueRef valueRef = LLVMConstInt( intType.GetTypeRef( ), constValue, signExtend );
            return Value.FromHandle<ConstantInt>( valueRef.ThrowIfInvalid() )!;
        }

        public ConstantFP CreateConstant( float constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( FloatType.GetTypeRef(), constValue ).ThrowIfInvalid() )!;

        public ConstantFP CreateConstant( double constValue )
            => Value.FromHandle<ConstantFP>( LLVMConstReal( DoubleType.GetTypeRef(), constValue ).ThrowIfInvalid() )!;

        public BasicBlock CreateBasicBlock( LazyEncodedString name )
        {
            return BasicBlock.FromHandle( LLVMCreateBasicBlockInContext( Handle, name ).ThrowIfInvalid() )!;
        }

        public Module CreateBitcodeModule( )
        {
            return CreateBitcodeModule( LazyEncodedString.Empty );
        }

        public Module CreateBitcodeModule( LazyEncodedString moduleId )
        {
            // empty string is OK.
            ArgumentNullException.ThrowIfNull( moduleId );

            return new( LLVMModuleCreateWithNameInContext( moduleId, Handle ) );
        }

        public Module ParseModule( LazyEncodedString src, LazyEncodedString name )
        {
            ArgumentNullException.ThrowIfNull( src );
            ArgumentNullException.ThrowIfNull( name );

            unsafe
            {
                using var nativeSrcHandle = src.Pin();
                using var mb = new MemoryBuffer((byte*)nativeSrcHandle.Pointer, src.NativeLength, name, requiresNullTerminator: true);

                LLVMStatus result = LLVMParseIRInContext(Handle, mb.Handle, out LLVMModuleRef moduleRef, out string? errMsg);
                if(result.Failed)
                {
                    Debug.Assert( errMsg is not null, "Internal Error - Got a failed status but NULL message!" );
                    throw new LlvmException( errMsg );
                }

                Debug.Assert( errMsg is null, "Internal Error - Got a success status but a valid message!" );

                if(moduleRef.IsNull)
                {
                    throw new LlvmException( "Internal Error - Got a null or invalid moduleRef for a success!" );
                }

                try
                {
                    // successfully completed ownership transfer to/from native code
                    // NOTE: Ownership transfer of the buffer is NOT documented, but in reality - does happen!
                    mb.InvalidateAfterMove();
                    return new Module( moduleRef );
                }
                catch
                {
                    // cleanup in case of exceptions (Otherwise, moved to return)
                    moduleRef.Dispose();
                    throw;
                }
            }
        }

        public uint GetMDKindId( LazyEncodedString? name )
        {
            return LLVMGetMDKindIDInContext( Handle, name ?? LazyEncodedString.Empty );
        }

        public bool OdrUniqueDebugTypes
        {
            get => LibLLVMContextGetIsODRUniquingDebugTypes( Handle );
            set => LibLLVMContextSetIsODRUniquingDebugTypes( Handle, value );
        }

        public TargetBinary OpenBinary( LazyEncodedString path )
        {
            // ownership of this buffer transfers to the TargetBinary instance
            // unless an exception occurs. Dispose() is idempotent and a harmless
            // NOP if transfer occurs.
            using var buffer = new MemoryBuffer( path );
            return new TargetBinary( buffer, this );
        }

        public bool DiscardValueNames
        {
            get => LLVMContextShouldDiscardValueNames( Handle );
            set => LLVMContextSetDiscardValueNames( Handle, value );
        }

        internal LLVMContextRefAlias Handle { get; set; }

        internal ContextAlias( LLVMContextRefAlias nativeHandle )
        {
            if(nativeHandle.IsNull)
            {
                throw new ArgumentException( "Invalid handle value", nameof( nativeHandle ) );
            }

            Handle = nativeHandle;
        }

        private AttributeValue InternalCreateStringAttribute( LazyEncodedString name, LazyEncodedString? value = null )
        {
            // if no value provided, use an empty string so native API doesn't crash.
            value ??= LazyEncodedString.Empty;
            return new( LLVMCreateStringAttribute( Handle, name, value ) );
        }

        // TODO: Move this to the extensions support it is useful on it's own in other contexts
        [DoesNotReturn]
        private static void ThrowAttributeRequiresArg(
            AttributeInfo info,
            LazyEncodedString attribName,
            [CallerArgumentExpression( nameof( attribName ) )] string? exp = null
            )
        {
            throw new ArgumentException(
                string.Format( CultureInfo.CurrentCulture, Resources.Attribute_0_requires_a_value_of_1, attribName, info.ArgKind ),
                exp
            );
        }

        /*
        public IEnumerable<string> MDKindNames { get; }
        /*TODO: Create interop calls to support additional properties/methods

        public unsigned GetOperandBundleTagId(string name) {...}
        public IEnumerable<string> OperandBundleTagIds { get; }
        */
    }
}

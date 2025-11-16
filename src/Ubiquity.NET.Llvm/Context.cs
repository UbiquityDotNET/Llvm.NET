// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Llvm.ObjectFile;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

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
    /// the scope, and a compilation unit is bound to a <see cref="Module"/>, even though, technically
    /// the types are owned by a Context. Thus to keep things simpler and help make working with debug information
    /// easier. Ubiquity.NET.Llvm encapsulates the native type and the debug type in separate classes that are
    /// instances of the <see cref="IDebugType{NativeT, DebugT}"/> interface</para>
    ///
    /// <note type="note">It is important to be aware of the fact that a Context is not thread safe. The context
    /// itself and the object instances it owns are intended for use by a single thread only. Accessing and
    /// manipulating LLVM objects from multiple threads may lead to race conditions corrupted state and any number
    /// of other undefined issues.</note>
    /// </remarks>
    public sealed class Context
        : IContext
        , IDisposable
        , IEquatable<Context>
    {
        #region IEquatable<T>

        /// <inheritdoc/>
        public bool Equals( IContext? other ) => other is not null && ((LLVMContextRefAlias)Handle).Equals( other.GetUnownedHandle() );

        /// <inheritdoc/>
        public bool Equals( Context? other ) => other is not null && Handle.Equals( other.Handle );

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is Context owner
                                                      ? Equals( owner )
                                                      : Equals( obj as IContext );

        /// <inheritdoc/>
        public override int GetHashCode( ) => Handle.GetHashCode();

        #endregion

        /// <summary>Initializes a new instance of the <see cref="Context"/> class.</summary>
        /// <remarks>
        /// This creates an underlying native LLVM context handle and wraps it in this instance.
        /// Callers should call <see cref="Dispose"/> to release the native allocation as early
        /// as possible. Anything owned by this context is destroyed by the call to <see cref="Dispose"/>,
        /// thus any references to those items are invalid once the context is destroyed. This is
        /// particularly relevant for any IDisposable items (like a <see cref="Module"/>). If those
        /// are left to linger at the mercy of the garbage collector/finalization then problems
        /// can happen. Since .NET uses a non-deterministic finalization design there is no guarantee
        /// that an element is finalized **BEFORE** the container that owns it. Thus if the finalizer
        /// destroys the container and then attempts to destroy the element - BOOM access violation
        /// and app crash occur. The location of such a crash is well after the problem occurred.
        /// This is a VERY difficult situation to debug. (Though, in a debug build of this library,
        /// the call stack of the creator of any owned handle is captured to make it easier to find
        /// the cause of such things.)
        /// </remarks>
        public Context( )
            : this( LLVMContextCreate() )
        {
        }

        // To keep from needing to implement the same code twice this instance contains an
        // internal implementation of the "alias" wrapper to do all the real work, while
        // maintaining the ownership semantics.
        #region IContext (via Impl)

        /// <inheritdoc/>
        public void SetDiagnosticHandler( DiagnosticInfoCallbackAction handler ) => Impl.SetDiagnosticHandler( handler );

        /// <inheritdoc/>
        public ITypeRef GetIntType( uint bitWidth ) => Impl.GetIntType( bitWidth );

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public IFunctionType GetFunctionType( ITypeRef returnType, params IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( returnType, args );
#else

        /// <inheritdoc/>
        public IFunctionType GetFunctionType( ITypeRef returnType, IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( returnType, args );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, params IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( isVarArgs, returnType, args );
#else
        /// <inheritdoc/>
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( isVarArgs, returnType, args );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder, IDebugType<ITypeRef, DIType> retType, params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes )
            => Impl.CreateFunctionType( diBuilder, retType, argTypes );
#else
        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder, IDebugType<ITypeRef, DIType> retType, IEnumerable<IDebugType<ITypeRef, DIType>> argTypes )
            => Impl.CreateFunctionType( diBuilder, retType, argTypes );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder, bool isVarArg, IDebugType<ITypeRef, DIType> retType, params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes )
            => Impl.CreateFunctionType( diBuilder, isVarArg, retType, argTypes );
#else
        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( IDIBuilder diBuilder, bool isVarArg, IDebugType<ITypeRef, DIType> retType, IEnumerable<IDebugType<ITypeRef, DIType>> argTypes )
            => Impl.CreateFunctionType( diBuilder, isVarArg, retType, argTypes );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public Constant CreateConstantStruct( bool packed, params IEnumerable<Constant> values ) => Impl.CreateConstantStruct( packed, values );
#else
        /// <inheritdoc/>
        public Constant CreateConstantStruct( bool packed, IEnumerable<Constant> values ) => Impl.CreateConstantStruct( packed, values );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public Constant CreateNamedConstantStruct( IStructType type, params IEnumerable<Constant> values ) => Impl.CreateNamedConstantStruct( type, values );
#else
        /// <inheritdoc/>
        public Constant CreateNamedConstantStruct( IStructType type, IEnumerable<Constant> values ) => Impl.CreateNamedConstantStruct( type, values );
#endif

        /// <inheritdoc/>
        public IStructType CreateStructType( LazyEncodedString name ) => Impl.CreateStructType( name );

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public IStructType CreateStructType( bool packed, params IEnumerable<ITypeRef> elements ) => Impl.CreateStructType( packed, elements );
#else
        /// <inheritdoc/>
        public IStructType CreateStructType( bool packed, params ITypeRef[] elements ) => Impl.CreateStructType( packed, elements );
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc/>
        public IStructType CreateStructType( LazyEncodedString name, bool packed, params IEnumerable<ITypeRef> elements ) => Impl.CreateStructType( name, packed, elements );
#else
        /// <inheritdoc/>
        public IStructType CreateStructType( LazyEncodedString name, bool packed, params ITypeRef[] elements ) => Impl.CreateStructType( name, packed, elements );
#endif

        /// <inheritdoc/>
        public MDString CreateMetadataString( LazyEncodedString? value ) => Impl.CreateMetadataString( value );

        /// <inheritdoc/>
        public MDNode CreateMDNode( LazyEncodedString value ) => Impl.CreateMDNode( value );

        /// <inheritdoc/>
        public ConstantDataArray CreateConstantString( LazyEncodedString value ) => Impl.CreateConstantString( value );

        /// <inheritdoc/>
        public ConstantDataArray CreateConstantString( LazyEncodedString value, bool nullTerminate ) => Impl.CreateConstantString( value, nullTerminate );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( bool constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( byte constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public Constant CreateConstant( sbyte constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( short constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( ushort constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( int constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( uint constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( long constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( ulong constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( uint bitWidth, ulong constValue, bool signExtend ) => Impl.CreateConstant( bitWidth, constValue, signExtend );

        /// <inheritdoc/>
        public ConstantInt CreateConstant( ITypeRef intType, ulong constValue, bool signExtend ) => Impl.CreateConstant( intType, constValue, signExtend );

        /// <inheritdoc/>
        public ConstantFP CreateConstant( float constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public ConstantFP CreateConstant( double constValue ) => Impl.CreateConstant( constValue );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( LazyEncodedString name ) => Impl.CreateAttribute( name );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( LazyEncodedString name, UInt64 value ) => Impl.CreateAttribute( name, value );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( LazyEncodedString name, ITypeRef value ) => Impl.CreateAttribute( name, value );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( LazyEncodedString name, UInt32 numBits, UInt64[] lowWords, UInt64[] upperWords )
            => Impl.CreateAttribute( name, numBits, lowWords, upperWords );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( LazyEncodedString name, LazyEncodedString value ) => Impl.CreateAttribute( name, value );

        /// <inheritdoc/>
        public BasicBlock CreateBasicBlock( LazyEncodedString name ) => Impl.CreateBasicBlock( name );

        /// <inheritdoc/>
        public Module CreateBitcodeModule( ) => Impl.CreateBitcodeModule();

        /// <inheritdoc/>
        public Module CreateBitcodeModule( LazyEncodedString moduleId ) => Impl.CreateBitcodeModule( moduleId );

        /// <inheritdoc/>
        public Module ParseModule( LazyEncodedString src, LazyEncodedString name ) => Impl.ParseModule( src, name );

        /// <inheritdoc/>
        public uint GetMDKindId( LazyEncodedString name ) => Impl.GetMDKindId( name );

        /// <inheritdoc/>
        public TargetBinary OpenBinary( LazyEncodedString path ) => Impl.OpenBinary( path );

        /// <inheritdoc/>
        public ITypeRef VoidType => Impl.VoidType;

        /// <inheritdoc/>
        public ITypeRef BoolType => Impl.BoolType;

        /// <inheritdoc/>
        public ITypeRef Int8Type => Impl.Int8Type;

        /// <inheritdoc/>
        public ITypeRef Int16Type => Impl.Int16Type;

        /// <inheritdoc/>
        public ITypeRef Int32Type => Impl.Int32Type;

        /// <inheritdoc/>
        public ITypeRef Int64Type => Impl.Int64Type;

        /// <inheritdoc/>
        public ITypeRef Int128Type => Impl.Int128Type;

        /// <inheritdoc/>
        public ITypeRef HalfFloatType => Impl.HalfFloatType;

        /// <inheritdoc/>
        public ITypeRef FloatType => Impl.FloatType;

        /// <inheritdoc/>
        public ITypeRef DoubleType => Impl.DoubleType;

        /// <inheritdoc/>
        public ITypeRef TokenType => Impl.TokenType;

        /// <inheritdoc/>
        public ITypeRef MetadataType => Impl.MetadataType;

        /// <inheritdoc/>
        public ITypeRef X86Float80Type => Impl.X86Float80Type;

        /// <inheritdoc/>
        public ITypeRef Float128Type => Impl.Float128Type;

        /// <inheritdoc/>
        public ITypeRef PpcFloat128Type => Impl.PpcFloat128Type;

        /// <inheritdoc/>
        public bool OdrUniqueDebugTypes
        {
            get => Impl.OdrUniqueDebugTypes;
            set => Impl.OdrUniqueDebugTypes = value;
        }

        /// <inheritdoc/>
        public bool DiscardValueNames
        {
            get => Impl.DiscardValueNames;
            set => Impl.DiscardValueNames = value;
        }
#endregion

        /// <summary>Gets a value indicating whether this instance is already disposed</summary>
        public bool IsDisposed => Handle.IsNull;

        /// <inheritdoc/>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Ownership transferred in constructor")]
        public void Dispose( )
        {
            if(Handle != nint.Zero)
            {
                // remove any diagnostic handler so it the callback's context doesn't leak
                unsafe
                {
                    void* context = LLVMContextGetDiagnosticContext(Handle);
                    if(context is not null)
                    {
                        LLVMContextSetDiagnosticHandler(Handle, null, null);
                        NativeContext.Release(context);
                    }
                }

                Handle.Dispose();
                InvalidateAfterMove();
            }
        }

        internal Context( LLVMContextRef h )
        {
            Handle = h;

            // Create the implementation from this handle
#if NET10_0_OR_GREATER
            Impl = new( LLVMContextRef.FromABI(Handle ) );
#else
            ImplBackingField = new( LLVMContextRef.FromABI( Handle ) );
#endif
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "Constructor uses move semantics")]
        internal LLVMContextRef Handle { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvalidateAfterMove( )
        {
            Handle = default;
        }

#if NET10_0_OR_GREATER
        private ContextAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf( IsDisposed, this );
                return field;
            }
        }
#else
        private ContextAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf( IsDisposed, this );
                return ImplBackingField;
            }
        }

        private readonly ContextAlias ImplBackingField;
#endif
    }
}

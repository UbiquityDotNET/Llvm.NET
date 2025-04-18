// -----------------------------------------------------------------------
// <copyright file="Context.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.ObjectFile;

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
        , IGlobalHandleOwner<LLVMContextRef>
        , IEquatable<Context>
    {
        #region IEquatable<T>

        /// <inheritdoc/>
        public bool Equals(IContext? other) => other is not null && ((LLVMContextRefAlias)NativeHandle).Equals(other.GetUnownedHandle());

        /// <inheritdoc/>
        public bool Equals(Context? other) => other is not null && NativeHandle.Equals(other.NativeHandle);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Context owner
                                                  ? Equals(owner)
                                                  : Equals( obj as IContext );

        /// <inheritdoc/>
        public override int GetHashCode() => NativeHandle.GetHashCode();

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
        /// This is a VERY difficult situation to debug. (Though in a debug build the call stack of
        /// the creator of any owned handle is captured to make it easier to find the cause of such
        /// things.) <see cref="Ubiquity.NET.Llvm.Interop.GlobalHandleBase"/>
        /// </remarks>
        public Context()
            : this(LLVMContextCreate())
        {
        }

        // To keep from needing to implement the same code twice this instance contains an
        // internal implementation of the "alias" wrapper to do all the real work, while
        // maintaining the ownership semantics.
        #region IContext (via Impl)

        /// <inheritdoc/>
        public void SetDiagnosticHandler( DiagnosticInfoCallbackAction handler ) => Impl.SetDiagnosticHandler( handler );

        /// <inheritdoc/>
        public IPointerType GetPointerTypeFor( ITypeRef elementType ) => Impl.GetPointerTypeFor( elementType );

        /// <inheritdoc/>
        public ITypeRef GetIntType( uint bitWidth ) => Impl.GetIntType( bitWidth );

        /// <inheritdoc/>
        public IFunctionType GetFunctionType( ITypeRef returnType, params IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( returnType, args );

        /// <inheritdoc/>
        public IFunctionType GetFunctionType( bool isVarArgs, ITypeRef returnType, params IEnumerable<ITypeRef> args ) => Impl.GetFunctionType( isVarArgs, returnType, args );

        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( ref readonly DIBuilder diBuilder, IDebugType<ITypeRef, DIType> retType, params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes ) => Impl.CreateFunctionType( in diBuilder, retType, argTypes );

        /// <inheritdoc/>
        public DebugFunctionType CreateFunctionType( ref readonly DIBuilder diBuilder, bool isVarArg, IDebugType<ITypeRef, DIType> retType, params IEnumerable<IDebugType<ITypeRef, DIType>> argTypes ) => Impl.CreateFunctionType( in diBuilder, isVarArg, retType, argTypes );

        /// <inheritdoc/>
        public Constant CreateConstantStruct( bool packed, params IEnumerable<Constant> values ) => Impl.CreateConstantStruct( packed, values );

        /// <inheritdoc/>
        public Constant CreateNamedConstantStruct( IStructType type, params IEnumerable<Constant> values ) => Impl.CreateNamedConstantStruct( type, values );

        /// <inheritdoc/>
        public IStructType CreateStructType( string name ) => Impl.CreateStructType( name );

        /// <inheritdoc/>
        public IStructType CreateStructType(bool packed, params IEnumerable<ITypeRef> elements) => Impl.CreateStructType(packed, elements);

        /// <inheritdoc/>
        public IStructType CreateStructType( string name, bool packed, params IEnumerable<ITypeRef> elements ) => Impl.CreateStructType( name, packed, elements );

        /// <inheritdoc/>
        public MDString CreateMetadataString( string? value ) => Impl.CreateMetadataString( value );

        /// <inheritdoc/>
        public MDNode CreateMDNode( string value ) => Impl.CreateMDNode( value );

        /// <inheritdoc/>
        public ConstantDataArray CreateConstantString( string value ) => Impl.CreateConstantString( value );

        /// <inheritdoc/>
        public ConstantDataArray CreateConstantString( string value, bool nullTerminate ) => Impl.CreateConstantString( value, nullTerminate );

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
        public AttributeValue CreateAttribute( AttributeKind kind ) => Impl.CreateAttribute( kind );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( AttributeKind kind, ulong value ) => Impl.CreateAttribute( kind, value );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( string name ) => Impl.CreateAttribute( name );

        /// <inheritdoc/>
        public AttributeValue CreateAttribute( string name, string value ) => Impl.CreateAttribute( name, value );

        /// <inheritdoc/>
        public BasicBlock CreateBasicBlock( string name ) => Impl.CreateBasicBlock( name );

        /// <inheritdoc/>
        public Module CreateBitcodeModule( ) => Impl.CreateBitcodeModule();

        /// <inheritdoc/>
        public Module CreateBitcodeModule( string moduleId ) => Impl.CreateBitcodeModule( moduleId );

        /// <inheritdoc/>
        public Module ParseModule( LazyEncodedString src, LazyEncodedString name ) => Impl.ParseModule( src, name );

        /// <inheritdoc/>
        public uint GetMDKindId( string name ) => Impl.GetMDKindId( name );

        /// <inheritdoc/>
        public TargetBinary OpenBinary( string path ) => Impl.OpenBinary( path );

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
        public bool DiscardValueName
        {
            get => Impl.DiscardValueName;
            set => Impl.DiscardValueName = value;
        }
        #endregion

        /// <summary>Gets a value indicating whether this instance is already disposed</summary>
        public bool IsDisposed => NativeHandle is null || NativeHandle.IsInvalid || NativeHandle.IsClosed;

        /// <inheritdoc/>
        public void Dispose( )
        {
            NativeHandle.Dispose();
        }

        internal Context(LLVMContextRef h)
        {
            NativeHandle = h.Move();

            // Create the implementation from this handle
            AliasImpl = new(NativeHandle);
        }

        /// <inheritdoc/>
        [SuppressMessage( "StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "internal interface" )]
        LLVMContextRef IGlobalHandleOwner<LLVMContextRef>.OwnedHandle => NativeHandle;

        /// <inheritdoc/>
        void IGlobalHandleOwner<LLVMContextRef>.InvalidateFromMove( ) => NativeHandle.SetHandleAsInvalid();

        private ContextAlias Impl
        {
            get
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);
                return AliasImpl;
            }
        }

        private readonly ContextAlias AliasImpl;
        private readonly LLVMContextRef NativeHandle;
    }
}

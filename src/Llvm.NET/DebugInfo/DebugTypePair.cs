using System;
using Llvm.NET.Types;
using Llvm.NET.Values;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Provides pairing of a <see cref="ITypeRef"/> with a <see cref="DIType"/> for function signatures</summary>
    /// <remarks>
    /// <para>Ordinarily, <see cref="ITypeRef.DIType"/> serves to provide the correct debug information type.
    /// However, when using the pointer+alloca+memcpy pattern to pass by value the actual source and
    /// debug info type is different than the LLVM function signature. This class is used to construct
    /// signatures to allow for overriding the normal 1:1 type mapping with <see cref="ITypeRef.DIType"/>.
    /// </para>
    /// <para>Since DebugTypePair implements ITypeRef it can be implicitly passed where an ITypeRef is 
    /// expected. This allows for function signature factories to accept ITypeRef. Then, when explicitly
    /// needed, callers can pass in an appropriate DebugTypePair instance.</para>
    /// </remarks>
    public class DebugTypePair<T>
        : ITypeRef
        where T : DIType
    {
        public DebugTypePair( ITypeRef llvmType, T diType )
        {
            LlvmType = llvmType;
            DIType = diType;
        }

        public IntPtr TypeHandle => LlvmType.TypeHandle;

        public bool IsSized => LlvmType.IsSized;

        public TypeKind Kind => LlvmType.Kind;

        public Context Context => LlvmType.Context;

        public uint IntegerBitWidth => LlvmType.IntegerBitWidth;

        public DIType DIType
        {
            get { return DIType_; }
            set
            {
                var newValue = value as T;
                if( newValue == null )
                    throw new ArgumentException( $"DIType mismatch: Expected an instance of {typeof( T ).Name}" );

                DIType_ = newValue;
            }
        }

        public bool IsInteger => LlvmType.IsInteger;

        public bool IsFloat => LlvmType.IsFloat;

        public bool IsDouble => LlvmType.IsDouble;

        public bool IsVoid => LlvmType.IsVoid;

        public bool IsStruct => LlvmType.IsStruct;

        public bool IsPointer => LlvmType.IsPointer;

        public bool IsSequence => LlvmType.IsSequence;

        public bool IsFloatingPoint => LlvmType.IsFloatingPoint;

        public bool IsPointerPointer => LlvmType.IsPointerPointer;

        private T DIType_;

        public Constant GetNullValue( ) => LlvmType.GetNullValue( );

        public IArrayType CreateArrayType( uint count ) => LlvmType.CreateArrayType( count );

        public IPointerType CreatePointerType( ) => LlvmType.CreatePointerType( );

        public IPointerType CreatePointerType( uint addressSpace ) => LlvmType.CreatePointerType( addressSpace );

        public void ReplaceAllUsesOfDebugTypeWith( DICompositeType compositeType )
        {
            DIType.ReplaceAllUsesWith( compositeType );
            DIType = compositeType;
        }

        public IPointerType CreatePointerType( Module module, uint addressSpace ) => LlvmType.CreatePointerType( module, addressSpace );

        public IArrayType CreateArrayType( Module module, uint lowerBound, uint count ) => LlvmType.CreateArrayType( module, lowerBound, count );

        public bool TryGetExtendedPropertyValue<propT>( string id, out propT value )
        {
            if( PropertyContainer.TryGetExtendedPropertyValue( id, out value ) )
                return true;

            return LlvmType.TryGetExtendedPropertyValue( id, out value );
        }

        public void AddExtendedPropertyValue( string id, object value )
        {
            PropertyContainer.AddExtendedPropertyValue( id, value );
        }

        protected readonly ITypeRef LlvmType;
        private readonly ExtensiblePropertyContainer PropertyContainer = new ExtensiblePropertyContainer( );
    }
}

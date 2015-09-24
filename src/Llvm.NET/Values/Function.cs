using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Llvm.NET.Types;
using Llvm.NET.DebugInfo;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Function definition</summary>
    public class Function
        : GlobalObject
    {
        /// <summary>Signature type of the function</summary>
        public IFunctionType Signature => TypeRef.FromHandle<IFunctionType>( NativeMethods.GetElementType( NativeMethods.TypeOf( ValueHandle ) ) );

        /// <summary>Entry block for this function</summary>
        public BasicBlock EntryBlock
        {
            get
            {
                if( NativeMethods.CountBasicBlocks( ValueHandle ) == 0 )
                    return null;

                return BasicBlock.FromHandle( NativeMethods.GetEntryBasicBlock( ValueHandle ) );
            }
        }

        /// <summary>Basic Blocks for the function</summary>
        public IReadOnlyList<BasicBlock> BasicBlocks
        {
            get
            {
                uint count = NativeMethods.CountBasicBlocks( ValueHandle );
                var buf = new LLVMBasicBlockRef[ count ];
                if( count > 0 )
                    NativeMethods.GetBasicBlocks( ValueHandle, out buf[ 0 ] );

                return buf.Select( BasicBlock.FromHandle )
                          .ToList( )
                          .AsReadOnly( );
            }
        }

        /// <summary>Parameters for the function including any method definition specific attributes (i.e. ByVal)</summary>
        public IReadOnlyList<Argument> Parameters => new FunctionParameterList( this );

        /// <summary>Calling convention for the method</summary>
        public CallingConvention CallingConvention
        {
            get
            {
                return ( CallingConvention )NativeMethods.GetFunctionCallConv( ValueHandle );
            }
            set
            {
                NativeMethods.SetFunctionCallConv( ValueHandle, ( uint )value );
            }
        }

        /// <summary>LLVM instrinsicID for the method</summary>
        public uint IntrinsicId => NativeMethods.GetIntrinsicID( ValueHandle );

        /// <summary>Flag to indicate if the method signature accepts variable arguments</summary>
        public bool IsVarArg => Signature.IsVarArg;

        /// <summary>Return type of the function</summary>
        public ITypeRef ReturnType => Signature.ReturnType;

        /// <summary>Debug information for this function</summary>
        public DISubProgram DISubProgram { get; internal set; }

        /// <summary>Garbage collection engine name that this function is generated to work with</summary>
        /// <remarks>For details on GC support in LLVM see: http://llvm.org/docs/GarbageCollection.html </remarks>
        public string GcName
        {
            get
            {
                var nativePtr = NativeMethods.GetGC( ValueHandle );
                return Marshal.PtrToStringAnsi( nativePtr );
            }
            set
            {
                NativeMethods.SetGC( ValueHandle, value );
            }
        }

        /// <summary>Verifies the function is valid and all blocks properly terminated</summary>
        public void Verify()
        {
            IntPtr errMsgPtr;
            var status = NativeMethods.VerifyFunctionEx( ValueHandle, LLVMVerifierFailureAction.LLVMReturnStatusAction, out errMsgPtr );
            if( status )
                throw new InternalCodeGeneratorException( NativeMethods.MarshalMsg( errMsgPtr) );
        }

        /// <summary>Gets the <see cref="IAttributeSet"/> for this function itself</summary>
        public IAttributeSet Attributes { get; }

        // REVIEW: Should this be null if the return type is void? Is there any valid use of attributes
        // to a void return type?
        /// <summary>Gets the <see cref="IAttributeSet"/> for the return value of this function</summary>
        public IAttributeSet ReturnAttributes { get; }

        /// <summary>Add a new basic block to the beginning of a function</summary>
        /// <param name="name">Name (label) for the block</param>
        /// <returns><see cref="BasicBlock"/> created and insterted into the begining function</returns>
        public BasicBlock PrependBasicBlock( string name )
        {
            LLVMBasicBlockRef firstBlock = NativeMethods.GetFirstBasicBlock( ValueHandle );
            BasicBlock retVal;
            if( firstBlock.Pointer == IntPtr.Zero )
            {
                retVal = AppendBasicBlock( name );
            }
            else
            {
                var blockRef = NativeMethods.InsertBasicBlockInContext( Type.Context.ContextHandle, firstBlock, name );
                retVal = BasicBlock.FromHandle( firstBlock );
            }
            return retVal;
        }

        /// <summary>Appends a new basic block to a function</summary>
        /// <param name="name">Name (label) of the block</param>
        /// <returns><see cref="BasicBlock"/> created and insterted onto the end of the function</returns>
        public BasicBlock AppendBasicBlock( string name )
        {
            LLVMBasicBlockRef blockRef = NativeMethods.AppendBasicBlockInContext( Type.Context.ContextHandle, ValueHandle, name );
            return BasicBlock.FromHandle( blockRef );
        }

        /// <summary>Retrieves or creates  block by name</summary>
        /// <param name="name">Block name (label) to look for or create</param>
        /// <returns><see cref="BasicBlock"/> If the block was created it is appended to the end of function</returns>
        /// <remarks>
        /// This method tries to find a block by it's name and returns it if found, if not found a new block is 
        /// created and appended to the current function.
        /// </remarks>
        public BasicBlock FindOrCreateNamedBlock( string name )
        {
            var retVal = BasicBlocks.FirstOrDefault( b => b.Name == name );
            if( ReferenceEquals( retVal, null ) )
                retVal = AppendBasicBlock( name );

            Debug.Assert( retVal.ContainingFunction.ValueHandle.Pointer == ValueHandle.Pointer );
            return retVal;
        }

        /// <summary>Determines if a given attribute uses a parameter value</summary>
        /// <param name="kind">Attribute kind to test</param>
        /// <returns>true if the attribute has a parameter</returns>
        /// <remarks>
        /// Most of the attributes are simple boolean flags, however some, in particular
        /// those dealing with sizes or alignment require a parameter value. This method
        /// is used to determine which category the attribute falls into. Any attribute 
        /// returning true requires an integer parameter and has special handling so 
        /// cannot be used in the AddAttributes() methods.
        /// </remarks>
        public static bool AttributeHasValue( AttributeKind kind )
        {
            switch( kind )
            {
            case AttributeKind.Alignment:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
            case AttributeKind.StackAlignment:
                return true;

            default:
                return false;
            }
        }

        #region Attribute Support
        /// <summary>Adds a set of boolean attributes to the function index specified</summary>
        /// <param name="index">Function index to apply the attribute to</param>
        /// <param name="attributes">Attributes to add</param>
        /// <returns>This function for use in fluent style coding</returns>
        internal Function AddAttributes( FunctionAttributeIndex index, params AttributeKind[ ] attributes )
        {
            return AddAttributes( index, ( IEnumerable<AttributeKind> )attributes );
        }

        /// <summary>Adds a set of boolean attributes to the function index specified</summary>
        /// <param name="index">Function index to apply the attribute to</param>
        /// <param name="attributes">Attributes to add</param>
        /// <returns>This function for use in fluent style coding</returns>
        internal Function AddAttributes( FunctionAttributeIndex index, IEnumerable<AttributeKind> attributes )
        {
            foreach( var attribute in attributes )
                AddAttribute( index, attribute );

            return this;
        }

        /// <summary>Adds a single boolean attribute to the function index specified</summary>
        /// <param name="index">Function index to apply the attribute to</param>
        /// <param name="kind">Attribute kind to add</param>
        /// <returns>This function for use in fluent style coding</returns>
        internal void AddAttribute( FunctionAttributeIndex index, AttributeKind kind )
        {
            if( AttributeHasValue( kind ) )
                throw new ArgumentException( $"Attribute '{kind}' requires an argument", nameof( kind ) );

            NativeMethods.AddFunctionAttr2( ValueHandle, ( int )index, ( LLVMAttrKind )kind );
        }

        internal void AddAttribute( FunctionAttributeIndex index, AttributeKind kind, UInt64 value )
        {
            // To prevent native asserts or crashes - validate params before passing down to native code
            switch( kind )
            {
            case AttributeKind.Alignment:
                if( index > FunctionAttributeIndex.ReturnType )
                    throw new ArgumentException( "Alignment only supported on parameters", nameof( index ) );

                if( value > UInt32.MaxValue )
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for alignment" );

                break;

            case AttributeKind.StackAlignment:
                if( index != FunctionAttributeIndex.Function )
                    throw new ArgumentException( "Stack alignment only applicable to the function itself", nameof( index ) );

                if( value > UInt32.MaxValue )
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for alignment" );

                break;

            case AttributeKind.Dereferenceable:
                if( index == FunctionAttributeIndex.Function )
                    throw new ArgumentException( "Expected a return or param index", nameof( index ) );
                break;

            case AttributeKind.DereferenceableOrNull:
                if( index == FunctionAttributeIndex.Function )
                    throw new ArgumentException( "Expected a return or param index", nameof( index ) );
                break;

            default:
                throw new ArgumentException( $"Attribute '{kind}' does not support an argument", nameof( kind ) );
            }
            NativeMethods.SetFunctionAttributeValue( ValueHandle, ( int )index, ( LLVMAttrKind )kind, value );
        }

        internal UInt64 GetAttributeValue( FunctionAttributeIndex index, AttributeKind kind )
        {
            if( !AttributeHasValue( kind ) )
                throw new ArgumentException( $"Attribute '{kind}' does not support an argument", nameof( kind ) );

            return NativeMethods.GetFunctionAttributeValue( ValueHandle, (int)index, ( LLVMAttrKind )kind );
        }

        internal void RemoveAttribute( FunctionAttributeIndex index, AttributeKind kind )
        {
            NativeMethods.RemoveFunctionAttr2( ValueHandle, ( int )index, ( LLVMAttrKind )kind );
        }

        internal void AddAttribute( FunctionAttributeIndex index, string name, string value )
        {
            NativeMethods.AddTargetDependentFunctionAttr2( ValueHandle, ( int )index, name, value );
        }

        internal void RemoveAttribute( FunctionAttributeIndex index, string name )
        {
            NativeMethods.RemoveTargetDependentFunctionAttr2( ValueHandle, ( int )index, name );
        }

        internal bool HasAttribute( FunctionAttributeIndex index, AttributeKind kind )
        {
            return NativeMethods.HasFunctionAttr2( ValueHandle, ( int )index, ( LLVMAttrKind )kind );
        }
        #endregion

        internal Function( LLVMValueRef valueRef ) 
            : this( valueRef, false )
        {
        }

        internal Function( LLVMValueRef valueRef, bool preValidated ) 
            : base( preValidated ? valueRef : ValidateConversion( valueRef, NativeMethods.IsAFunction ) )
        {
            Attributes = new AttributeSetImpl( this, FunctionAttributeIndex.Function );
            ReturnAttributes = new AttributeSetImpl( this, FunctionAttributeIndex.ReturnType );
        }
    }
}

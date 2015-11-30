using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Llvm.NET.Values
{
    /// <summary>Enumeration flags to indicate which attribute set index an attribute may apply to</summary>
    [Flags]
    public enum FunctionIndexKind
    {
        /// <summary>Invalid attributes don't apply to any index</summary>
        None = 0,
        /// <summary>The attribute is applicable to a function</summary>
        Function = 1,
        /// <summary>The attribute is applicable to a function's return</summary>
        Return = 2,
        /// <summary>The aattribute is applicable to a function's parameter</summary>
        Parameter = 4
    }

    /// <summary>AttributeSet for a <see cref="Function"/>, <see cref="Instructions.CallInstruction"/>, or <see cref="Instructions.Invoke"/> instruction</summary>
    /// <remarks>
    /// The underlying LLVM AttributeSet class is an immutable value type, unfortunately it includes a non-trivial copy constructor
    /// and therefore isn't a blittable type. (e.g. memcpy is not a valid means of cloning or copying an AttributeSet ). Specifically,
    /// the llvm AttributeSet class uses a Pointer to IMPLementation (PIMPL) pattern. Ideally, this would be a .NET value type that
    /// simply contained an LLVM attribute set type instance. Unfortunately since the CLR effectively uses memcpy for copying value 
    /// types that won't work. Thus, this is a class in .NET and any modifications will create an new wrapped llvm attributeset,
    /// however the source of the attribute set will not reflect the changes until the attributes are re-assigned back to the source.
    /// This keeps the LLVM.NET programming model as close as possible to the underlying LLVM model while still providing a .NET
    /// developer experience.
    /// </remarks>
    public class AttributeSet
    {
        /// <summary>Retrieves an attributeSet filtered by the specified function index</summary>
        /// <param name="index">Index to filter on</param>
        /// <returns>A new <see cref="AttributeSet"/>with attributes from this set belonging to the specified index</returns>
        public AttributeSet this[ FunctionAttributeIndex index ]
        {
            get
            {
                return DoPinnedAction( ( pThis ) =>
                {
                    return new AttributeSet( TargetFunction, ( p ) => InternalGetIndexedAttributeSet( index, pThis, p ) );
                } );
            }
        }

        /// <summary>Gets the attributes for the function return</summary>
        public AttributeSet ReturnAttributes
        {
            get
            {
                return this[ FunctionAttributeIndex.ReturnType ];
            }
        }

        /// <summary>Gets the attributes for the function itself</summary>
        public AttributeSet FunctionAttributes
        {
            get
            {
                return this[ FunctionAttributeIndex.Function ];
            }
        }

        /// <summary>Function this attributeSet targets</summary>
        /// <remarks>
        /// It is important to not that, in LLVM, this attribute set is distinct
        /// from the attributes on the function itself. In particular attribute
        /// sets are applied to <see cref="Instructions.CallInstruction"/> and 
        /// <see cref="Instructions.Invoke"/> instructions. Thus allowing the
        /// call site to include a different set of attributes from the set for
        /// the function itself. (Though it is currently unclear what scenarios
        /// this is intended for).
        /// </remarks>
        public Function TargetFunction { get; }

        /// <summary>Gets the attributes for a function parameter</summary>
        /// <param name="paramIndex">Parameter index [ 0 based ]</param>
        /// <returns><see cref="AttributeSet"/>filtered for the specified parameter</returns>
        public AttributeSet ParameterAttributes( int paramIndex )
        {
            if( paramIndex > TargetFunction.Parameters.Count )
                throw new ArgumentOutOfRangeException( nameof( paramIndex ) );

            var index = FunctionAttributeIndex.Parameter0 + paramIndex;
            return this[ index ];
        }

        /// <summary>Get LLVM formatted string representation of this <see cref="AttributeSet"/> for a given index</summary>
        /// <param name="index">Index to get the string for</param>
        /// <returns>Formatted string for the specified attribute index</returns>
        public string AsString( FunctionAttributeIndex index )
        {
            IntPtr txtPtr = DoPinnedAction( ( p ) =>
            {
                return NativeMethods.AttributeSetGetAttributesAsString( p, ( int )index );
            } );
            return NativeMethods.MarshalMsg( txtPtr );
        }

        /// <summary>Creates a formatted string representation of the entire <see cref="AttributeSet"/> (e.g. all indeces)</summary>
        /// <returns>Formatted string representation of the <see cref="AttributeSet"/></returns>
        public override string ToString( )
        {
            var bldr = new StringBuilder( );
            bldr.AppendFormat( "[Return: {0}]", AsString( FunctionAttributeIndex.ReturnType ) ).AppendLine( )
                .AppendFormat( "[Function: {0}]", AsString( FunctionAttributeIndex.Function ) ).AppendLine( );
            
            var q = from param in TargetFunction.Parameters
                    where HasAny( FunctionAttributeIndex.Parameter0 + (int)param.Index )
                    select $"[Parameter({param.Name},{param.Index}): {AsString( FunctionAttributeIndex.Parameter0 + ( int )param.Index)}";

            foreach( var param in q )
                bldr.AppendLine( param );

            return bldr.ToString( );
        }

        /// <summary>Adds a set of attributes</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attributes">Attributes to add</param>
        public AttributeSet Add( FunctionAttributeIndex index, params AttributeValue[ ] attributes )
        {
           return Add( index, ( IEnumerable<AttributeValue> )attributes );
        }

        /// <summary>Add a collection of attributes</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attributes"></param>
        public AttributeSet Add( FunctionAttributeIndex index, IEnumerable<AttributeValue> attributes )
        {
            return DoPinnedAction( ( pThis ) =>
            {
                return new AttributeSet( TargetFunction
                                       , ( p ) =>
                                         {
                                             NativeMethods.CopyConstructAttributeSet( p, pThis );
                                             foreach( var attribute in attributes )
                                                 InternalAddOneAttribute( TargetFunction.Context, p, index, attribute );
                                         }
                                       );
            } );
        }

        /// <summary>Adds a single attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="attribute"><see cref="AttributeValue"/> kind to add</param>
        public AttributeSet Add( FunctionAttributeIndex index, AttributeValue attribute )
        {
            return DoPinnedAction( ( p ) =>
            {
                return Add( p, index, attribute );
            } );
        }

        /// <summary>Adds Attributes from another attribute set along a given index</summary>
        /// <param name="index">Index to add attributes to and from</param>
        /// <param name="attribute"><see cref="AttributeSet"/> to add the attributes from</param>
        /// <returns>New <see cref="AttributeSet"/>Containing all attributes of this set plus any
        ///  attributes from <paramref name="attribute"/> along the specified <paramref name="index"/></returns>
        public AttributeSet Add( FunctionAttributeIndex index, AttributeSet attribute )
        {
            return DoPinnedAction( ( pThis ) =>
            {
                    return new AttributeSet( TargetFunction, ( p ) =>
                    {
                        unsafe
                        {
                            fixed(byte* pOther = &attribute.NativeData[0] )
                            {
                                NativeMethods.AttributeSetAddAttributes( TargetFunction.Context.ContextHandle, pThis, (int)index, ( UIntPtr )pOther, p );
                            }
                        }
                    } );
            } );
        }
        
        /// <summary>Removes the specified attribute from the attribute set</summary>
        public AttributeSet Remove( FunctionAttributeIndex index, AttributeKind kind )
        {
            return DoPinnedAction( ( pThis ) =>
            {
                return new AttributeSet( TargetFunction
                                       , (p) =>
                                         {
                                             NativeMethods.CopyConstructAttributeSet( p, pThis );
                                             NativeMethods.AttributeSetRemoveAttribute( TargetFunction.Context.ContextHandle, p, ( int )index, ( LLVMAttrKind )kind );
                                         }
                                       );
            } );
        }

        /// <summary>Remove a target specific attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="name">Name of the attribute</param>
        public AttributeSet Remove( FunctionAttributeIndex index, string name )
        {
            return DoPinnedAction( ( pThis ) =>
            {
                return new AttributeSet( TargetFunction
                                       , ( p ) =>
                                         {
                                             NativeMethods.CopyConstructAttributeSet( p, pThis );
                                             NativeMethods.AttributeSetRemoveTargetDependentAttribute( TargetFunction.Context.ContextHandle
                                                                                                     , p
                                                                                                     , ( int )index
                                                                                                     , name
                                                                                                     );
                                         }
                                       );
            } );
        }

        /// <summary>Get an integer value for an index</summary>
        /// <param name="index">Index to get the value from</param>
        /// <param name="kind"><see cref="AttributeKind"/> to get the value of (see remarks for supported attributes)</param>
        /// <returns>Value of the attribute</returns>
        /// <remarks>
        /// The only attributes supporting an integer value are <see cref="AttributeKind.Alignment"/>,
        /// <see cref="AttributeKind.StackAlignment"/>, <see cref="AttributeKind.Dereferenceable"/>,
        /// <see cref="AttributeKind.DereferenceableOrNull"/>.
        /// </remarks>
        public UInt64 GetAttributeValue( FunctionAttributeIndex index, AttributeKind kind )
        {
            VerifyIntAttributeUsage( index, kind, 0ul );
            return DoPinnedAction( ( p ) =>
            {
                return NativeMethods.AttributeSetGetAttributeValue( p, (int)index, ( LLVMAttrKind )kind );
            } );
        }

        /// <summary>Tests if an <see cref="AttributeSet"/> has any attributes in the specified index</summary>
        /// <param name="index">Index for the attribute</param>
        public bool HasAny( FunctionAttributeIndex index )
        {
            return DoPinnedAction( ( p ) => 
            {
                return NativeMethods.AttributeSetHasAny( p, ( int )index );
            } );
        }

        /// <summary>Tests if this attribute set has a given AttributeValue kind</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="kind">Kind of AttributeValue to test for</param>
        /// <returns>true if the AttributeValue esists or false if not</returns>
        public bool Has( FunctionAttributeIndex index, AttributeKind kind )
        {
            return DoPinnedAction( ( p ) =>
            {
                return NativeMethods.AttributeSetHasAttribute( p, ( int )index, ( LLVMAttrKind )kind );
            } );
        }

        /// <summary>Tests if this attribute set has a given string attribute</summary>
        /// <param name="index">Index for the attribute</param>
        /// <param name="name">Name of the attribute to test for</param>
        /// <returns>true if the attribute exists or false if not</returns>
        public bool Has( FunctionAttributeIndex index, string name )
        {
            return DoPinnedAction( ( p ) =>
            {
                return NativeMethods.AttributeSetHasTargetDependentAttribute ( p, ( int )index, name);
            } );
        }

        /// <summary>Method used to get the pinned native data structure when storing the attribute set in native Methods</summary>
        /// <param name="storeFunc">function that performs the store with the native data pointer</param>
        internal void Store( Action<UIntPtr> storeFunc )
        {
            DoPinnedAction( storeFunc );
        }

        private AttributeSet Add( UIntPtr pThis, FunctionAttributeIndex index, AttributeValue attribute )
        {
            return new AttributeSet( TargetFunction, ( p )=>
            {
                NativeMethods.CopyConstructAttributeSet( p, pThis );
                InternalAddOneAttribute( TargetFunction.Context, p, index, attribute );
            } );
        }

        /// <summary>pins the internal data and runs the specified delegate with the pointer as an argument</summary>
        /// <param name="action">Action to perform with the pinned pointer</param>
        private void DoPinnedAction( Action<UIntPtr> action )
        {
            unsafe
            {
                // pin native data so it can be used safely by interop layer
                fixed ( byte* p = &NativeData[0])
                {
                    action( (UIntPtr)p );
                }
            }
        }

        /// <summary>pins the internal data and runs the specified delegate with the pointer as an argument</summary>
        /// <typeparam name="T">Return type for the action delegate</typeparam>
        /// <param name="action">Action to perform with the pinned pointer</param>
        private T DoPinnedAction<T>( Func<UIntPtr, T> action )
        {
            unsafe
            {
                // pin native data so it can be used safely by interop layer
                fixed ( byte* p = &NativeData[0])
                {
                    return action( (UIntPtr)p );
                }
            }
        }

        internal AttributeSet( AttributeSet other )
            : this( other.TargetFunction )
        {
            unsafe
            {
                fixed( byte* pThis = &NativeData[0])
                fixed( byte* pOther = &other.NativeData[0] )
                {
                    NativeMethods.CopyConstructAttributeSet( ( UIntPtr )pThis, (UIntPtr)pOther );
                }
            }
        }

        /// <summary>Constructs a new Attributeset initialized by a provided delegate</summary>
        /// <param name="targetFunction">Funtion this attribute set refers to</param>
        /// <param name="loadFunc">Action to initialize the allocated attribute set via it's pinned pointer</param>
        /// <remarks>
        /// This constructor allocates the internal data for the native attribute set, pins a pointer to 
        /// the native data then calls the <paramref name="loadFunc"/> with the pinned pointer to initialize
        /// the newly allocated data structure. The initialization, ordinarily calls various NativeMethods
        /// to perform initialization of the native AttributeSet.
        /// </remarks>
        internal AttributeSet( Function targetFunction, Action<UIntPtr> loadFunc )
            : this( targetFunction )
        {
            DoPinnedAction( loadFunc );
        }

        internal AttributeSet( Function targetFunction )
        {
            TargetFunction = targetFunction;
            NativeData = new byte[ NativeMethods.GetAttributeSetSize( ) ];
        }

        internal static void RangeCheckIntAttributeValue( AttributeKind kind, ulong value )
        {
            // To prevent native asserts or crashes - validate params before passing down to native code
            switch( kind )
            {
            case AttributeKind.Alignment:
                if( value > UInt32.MaxValue )
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for alignment" );

                break;

            case AttributeKind.StackAlignment:
                if( value > UInt32.MaxValue )
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for alignment" );
                break;

            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                break;

            default:
                throw new ArgumentException( $"Attribute '{kind}' does not support an argument", nameof( kind ) );
            }
        }

        public static FunctionIndexKind GetAllowedIndecesForAttribute( AttributeKind kind )
        {
            switch( kind )
            {
            default:
                return FunctionIndexKind.None;

            case AttributeKind.ZExt:
            case AttributeKind.SExt:
            case AttributeKind.InReg:
            case AttributeKind.ByVal:
            case AttributeKind.InAlloca:
            case AttributeKind.StructRet:
            case AttributeKind.Alignment:
            case AttributeKind.NoAlias:
            case AttributeKind.NoCapture:
            case AttributeKind.Nest:
            case AttributeKind.Returned:
            case AttributeKind.NonNull:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                return FunctionIndexKind.Parameter | FunctionIndexKind.Return;

            case AttributeKind.StackAlignment:
            case AttributeKind.AlwaysInline:
            case AttributeKind.Builtin:
            case AttributeKind.Cold:
            case AttributeKind.Convergent:
            case AttributeKind.InlineHint:
            case AttributeKind.JumpTable:
            case AttributeKind.MinSize:
            case AttributeKind.Naked:
            case AttributeKind.NoBuiltin:
            case AttributeKind.NoDuplicate:
            case AttributeKind.NoImplicitFloat:
            case AttributeKind.NoInline:
            case AttributeKind.NonLazyBind:
            case AttributeKind.NoRedZone:
            case AttributeKind.NoReturn:
            //case AttributeKind.NoRecurse: // ver > 3.7.0?
            case AttributeKind.NoUnwind:
            case AttributeKind.OptimizeForSize:
            case AttributeKind.OptimizeNone:
            case AttributeKind.ReadNone:
            case AttributeKind.ReadOnly:
            case AttributeKind.ArgMemOnly:
            case AttributeKind.ReturnsTwice:
            case AttributeKind.SafeStack:
            case AttributeKind.SanitizeAddress:
            case AttributeKind.SanitizeMemory:
            case AttributeKind.SanitizeThread:
            case AttributeKind.StackProtect:
            case AttributeKind.StackProtectReq:
            case AttributeKind.StackProtectStrong:
            case AttributeKind.UWTable:
                return FunctionIndexKind.Function;
            }
        }

        internal void VerifyAttributeUsage( FunctionAttributeIndex index, AttributeKind kind )
        {
            FunctionIndexKind allowedIndeces = GetAllowedIndecesForAttribute( kind );
            switch( index )
            {
            case FunctionAttributeIndex.Function:
                if( !allowedIndeces.HasFlag( FunctionIndexKind.Function ) )
                    throw new ArgumentException( "Attribute not allowed on functions", nameof( index ) );
                break;

            case FunctionAttributeIndex.ReturnType:
                if( !allowedIndeces.HasFlag( FunctionIndexKind.Return ) )
                    throw new ArgumentException( "Attribute not allowed on function Return", nameof( index ) );
                break;

            //case FunctionAttributeIndex.Parameter0:
            default:
                if( !allowedIndeces.HasFlag( FunctionIndexKind.Parameter ) )
                    throw new ArgumentException( "Attribute not allowed on function parameter", nameof( index ) );
                break;
            }

            if( index >= FunctionAttributeIndex.Parameter0 )
            {
                int paramIndex = index - FunctionAttributeIndex.Parameter0;
                if( paramIndex >= TargetFunction.Parameters.Count )
                    throw new ArgumentOutOfRangeException( nameof( index ), "Specified parameter index exceeds the number of parameters in the function" );
            }
        }

        // To prevent native asserts or crashes - validates params before passing down to native code
        internal void VerifyIntAttributeUsage( FunctionAttributeIndex index, AttributeKind kind, ulong value )
        {
            VerifyAttributeUsage( index, kind );
            RangeCheckIntAttributeValue( kind, value );
            switch( kind )
            {
            case AttributeKind.Alignment:
            case AttributeKind.StackAlignment:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                break;

            default:
                throw new ArgumentException( $"Attribute '{kind}' does not support an argument", nameof( kind ) );
            }
        }

        private void InternalAddOneAttribute( Context context, UIntPtr p, FunctionAttributeIndex index, AttributeValue attribute )
        {
            // int or enum?
            if( attribute.Kind.HasValue )
            {
                if( attribute.IntegerValue.HasValue )
                {
                    VerifyIntAttributeUsage( index, attribute.Kind.Value, attribute.IntegerValue.Value );
                    NativeMethods.AttributeSetSetAttributeValue( context.ContextHandle,  p, ( int )index, ( LLVMAttrKind )attribute.Kind.Value, attribute.IntegerValue.Value );
                }
                else
                    NativeMethods.AttributeSetAddAttribute( context.ContextHandle, p, ( int )index, ( LLVMAttrKind )attribute.Kind.Value );
            }
            else if( attribute.IsString )
            {
                NativeMethods.AttributeSetAddTargetDependentAttribute( context.ContextHandle, p, ( int )index, attribute.Name, attribute.StringValue );
            }
            else
                throw new ArgumentException( "Invalid Attribute", nameof( attribute ) );
        }

        // internal function to retrieve an attribute set filterd for a given index
        // this should only be called while the NativeData is pinned
        private static void InternalGetIndexedAttributeSet( FunctionAttributeIndex index, UIntPtr pThis, UIntPtr p )
        {
            switch( index )
            {
            case FunctionAttributeIndex.Function:
                NativeMethods.AttributeSetGetFunctionAttributes( pThis, p );
                break;
            case FunctionAttributeIndex.ReturnType:
                NativeMethods.AttributeSetGetReturnAttributes( pThis, p );
                break;
            case FunctionAttributeIndex.Parameter0:
            default:
                NativeMethods.AttributeSetGetParamAttributes( pThis, ( int )index, p );
                break;
            }
        }

        // The Attribute and AttributeSet classes are problematic to expose in "C" as
        // they are not POD types and therefore have no standard defined portable
        // representation in C or any other language binding. Most, if not all, modern
        // compilers will create those particular types in a way that we could probably
        // get away with treating them as if they were a POD. However, doing so is relying
        // on behavior that is officially specified as UNDEFINED. Thus, these APIs wrap
        // access to an AttributeSet via a pointer to this byte array. The array is set
        // to the size of an llvm::AttributeSet in the constructor. The interop layer
        // NativeMethods.CopyAssignAttributeSet() uses an explicit destructor call along
        // with a placement new copy constructor call to effectively re-assign the value
        // of the AttributeSet stored in this memory area.
        // 
        private readonly byte[ ] NativeData;

        private static FunctionAttributeIndex[ ] ParamAndReturnAttributes = { FunctionAttributeIndex.Parameter0, FunctionAttributeIndex.ReturnType };
    }

}

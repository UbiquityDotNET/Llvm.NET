// <copyright file="AttributeKind.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Enumeration for the known LLVM attributes</summary>
    /// <remarks>
    /// <para>It is important to note that the integer values of this enum
    /// do NOT necessarily correlate to the attribute IDs. LLVM has
    /// moved away from using an enum Flags model as the number of
    /// attributes reached the limit of available bits. Thus, the
    /// enum was dropped as of V5.0. Instead, strings are used to
    /// identify attributes. However, for maximum compatibility and
    /// ease of use for this library the enum is retained and the
    /// provided attribute manipulation classes will map the enum
    /// to the associated string.</para>
    /// <para>Also note that as a reult of the changes in LLVM this
    /// set of attributes is fluid and subject to change from version
    /// to version. Thus, code using any attributes that have changed
    /// or were removed will produce compile time errors. That is useful
    /// and by design so that any changes in LLVM naming will break at
    /// compile time instead of at runtime.</para>
    /// </remarks>
    public enum AttributeKind
    {
        None,
        Alignment,
        AllocSize,
        AlwaysInline,
        ArgMemOnly,
        Builtin,
        ByVal,
        Cold,
        Convergent,
        Dereferenceable,
        DereferenceableOrNull,
        InAlloca,
        InReg,
        InaccessibleMemOnly,
        InaccessibleMemOrArgMemOnly,
        InlineHint,
        JumpTable,
        MinSize,
        Naked,
        Nest,
        NoAlias,
        NoBuiltin,
        NoCapture,
        NoDuplicate,
        NoImplicitFloat,
        NoInline,
        NoRecurse,
        NoRedZone,
        NoReturn,
        NoUnwind,
        NonLazyBind,
        NonNull,
        OptimizeForSize,
        OptimizeNone,
        ReadNone,
        ReadOnly,
        Returned,
        ReturnsTwice,
        SExt,
        SafeStack,
        SanitizeAddress,
        SanitizeMemory,
        SanitizeThread,
        Speculatable,
        StackAlignment,
        StackProtect,
        StackProtectReq,
        StackProtectStrong,
        StructRet,
        SwiftError,
        SwiftSelf,
        UWTable,
        WriteOnly,
        ZExt,
        EndAttrKinds// always last
    }

    /// <summary>Enumeration flags to indicate which attribute set index an attribute may apply to</summary>
    [Flags]
    public enum FunctionIndexKinds
    {
        /// <summary>Invalid attributes don't apply to any index</summary>
        None = 0,

        /// <summary>The attribute is applicable to a function</summary>
        Function = 1,

        /// <summary>The attribute is applicable to a function's return</summary>
        Return = 2,

        /// <summary>The attribute is applicable to a function's parameter</summary>
        Parameter = 4
    }

    /// <summary>Utility class to provide extension methods for validating usage of attribute kinds</summary>
    public static class AttributeKindExtensions
    {
        public static string GetAttributeName( this AttributeKind kind )
        {
            return KnownAttributeNames[ ( int )kind ];
        }

        public static bool RequiresIntValue( this AttributeKind kind )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );
            switch( kind )
            {
            case AttributeKind.Alignment:
            case AttributeKind.StackAlignment:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                return true;

            default:
                return false;
            }
        }

        public static AttributeKind LookupId( uint id )
        {
            if( AttribIdToKindMap.Value.TryGetValue( id, out AttributeKind retValue ) )
            {
                return retValue;
            }

            return AttributeKind.None;
        }

        internal static uint GetEnumAttributeId( this AttributeKind kind )
        {
            if( KindToAttribIdMap.Value.TryGetValue( kind, out uint retVal ) )
            {
                return retVal;
            }

            return 0;
        }

        internal static bool CheckAttributeUsage( this AttributeKind kind, FunctionAttributeIndex index, Value value )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );
            FunctionIndexKinds allowedindices = kind.GetAllowedIndexes( );
            switch( index )
            {
            case FunctionAttributeIndex.Function:
                if( !allowedindices.HasFlag( FunctionIndexKinds.Function ) )
                {
                    return false;
                }

                break;

            case FunctionAttributeIndex.ReturnType:
                if( !allowedindices.HasFlag( FunctionIndexKinds.Return ) )
                {
                    return false;
                }

                break;

            // case FunctionAttributeIndex.Parameter0:
            default:
                {
                    if( value == null )
                    {
                        throw new ArgumentNullException( nameof( value ) );
                    }

                    if( !allowedindices.HasFlag( FunctionIndexKinds.Parameter ) )
                    {
                        return false;
                    }

                    Function function;
                    switch( value )
                    {
                    case Function f:
                        function = f;
                        break;

                    case Instructions.Invoke inv:
                        function = inv.TargetFunction;
                        break;

                    case Instructions.CallInstruction call:
                        function = call.TargetFunction;
                        break;

                    case Argument arg:
                        function = arg.ContainingFunction;
                        if( index != FunctionAttributeIndex.Parameter0 + ( int )arg.Index )
                        {
                            return false;
                        }

                        break;

                    default:
                        function = null;
                        break;
                    }

                    int paramIndex = index - FunctionAttributeIndex.Parameter0;
                    if( paramIndex >= function.Parameters.Count )
                    {
                        return false;
                    }
                }

                break;
            }

            return true;
        }

        internal static void VerifyAttributeUsage( this AttributeKind kind, FunctionAttributeIndex index, Value value )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );
            VerifyAttributeUsage( kind, index );

            if( index >= FunctionAttributeIndex.Parameter0 )
            {
                Function function;
                switch( value )
                {
                case Function f:
                    function = f;
                    break;

                case Instructions.Invoke inv:
                    function = inv.TargetFunction;
                    break;

                case Instructions.CallInstruction call:
                    function = call.TargetFunction;
                    break;

                case Argument arg:
                    function = arg.ContainingFunction;
                    if( index != FunctionAttributeIndex.Parameter0 + ( int )arg.Index )
                    {
                        throw new ArgumentException( "Index for paramters must be the actual position of the argument" );
                    }

                    break;

                default:
                    function = null;
                    break;
                }

                int paramIndex = index - FunctionAttributeIndex.Parameter0;
                if( paramIndex > ( function.Parameters.Count - 1 ) )
                {
                    throw new ArgumentException( "Specified parameter index exceeds the number of parameters in the function", nameof( index ) );
                }
            }
        }

        internal static void VerifyAttributeUsage( this AttributeKind kind, FunctionAttributeIndex index )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );
            FunctionIndexKinds allowedindices = kind.GetAllowedIndexes( );
            switch( index )
            {
            case FunctionAttributeIndex.Function:
                if( !allowedindices.HasFlag( FunctionIndexKinds.Function ) )
                {
                    throw new ArgumentException( "Attribute not allowed on functions", nameof( index ) );
                }

                break;

            case FunctionAttributeIndex.ReturnType:
                if( !allowedindices.HasFlag( FunctionIndexKinds.Return ) )
                {
                    throw new ArgumentException( "Attribute not allowed on function Return", nameof( index ) );
                }

                break;

            // case FunctionAttributeIndex.Parameter0:
            default:
                if( !allowedindices.HasFlag( FunctionIndexKinds.Parameter ) )
                {
                    throw new ArgumentException( "Attribute not allowed on function parameter", nameof( index ) );
                }

                break;
            }
        }

        // To prevent native asserts or crashes - validates parameters before passing down to native code
        internal static void VerifyAttributeUsage( this AttributeKind kind, FunctionAttributeIndex index, ulong value )
        {
            kind.VerifyAttributeUsage( index );
            kind.RangeCheckValue( value );
        }

        internal static void RangeCheckValue( this AttributeKind kind, ulong value )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );

            // To prevent native asserts or crashes - validate parameters before passing down to native code
            switch( kind )
            {
            case AttributeKind.Alignment:
                if( value > UInt32.MaxValue )
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for alignment" );
                }

                break;

            case AttributeKind.StackAlignment:
                if( value > UInt32.MaxValue )
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), "Expected a 32 bit value for stack alignment" );
                }

                if( value != 0 && !IsPowerOfTwo( value ) )
                {
                    throw new ArgumentException( "Stack alignment value must be a power of 2", nameof( value ) );
                }

                break;

            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                break;

            default:
                throw new ArgumentException( $"Attribute '{kind}' does not support an argument", nameof( kind ) );
            }
        }

        internal static FunctionIndexKinds GetAllowedIndexes( this AttributeKind kind )
        {
            Debug.Assert( kind >= AttributeKind.None && kind < AttributeKind.EndAttrKinds );
            switch( kind )
            {
            default:
                return FunctionIndexKinds.None;

            case AttributeKind.ReadOnly:
            case AttributeKind.WriteOnly:
            case AttributeKind.ReadNone:
                return FunctionIndexKinds.Function | FunctionIndexKinds.Parameter;

            case AttributeKind.ByVal:
            case AttributeKind.InAlloca:
            case AttributeKind.StructRet:
            case AttributeKind.Nest:
            case AttributeKind.NoCapture:
            case AttributeKind.Returned:
            case AttributeKind.SwiftSelf:
            case AttributeKind.SwiftError:
                return FunctionIndexKinds.Parameter;

            case AttributeKind.ZExt:
            case AttributeKind.SExt:
            case AttributeKind.InReg:
            case AttributeKind.Alignment:
            case AttributeKind.NoAlias:
            case AttributeKind.NonNull:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
                return FunctionIndexKinds.Parameter | FunctionIndexKinds.Return;

            case AttributeKind.NoReturn:
            case AttributeKind.NoUnwind:
            case AttributeKind.NoInline:
            case AttributeKind.AlwaysInline:
            case AttributeKind.OptimizeForSize:
            case AttributeKind.StackProtect:
            case AttributeKind.StackProtectReq:
            case AttributeKind.StackProtectStrong:
            case AttributeKind.SafeStack:
            case AttributeKind.NoRedZone:
            case AttributeKind.NoImplicitFloat:
            case AttributeKind.Naked:
            case AttributeKind.InlineHint:
            case AttributeKind.StackAlignment:
            case AttributeKind.UWTable:
            case AttributeKind.NonLazyBind:
            case AttributeKind.ReturnsTwice:
            case AttributeKind.SanitizeAddress:
            case AttributeKind.SanitizeThread:
            case AttributeKind.SanitizeMemory:
            case AttributeKind.MinSize:
            case AttributeKind.NoDuplicate:
            case AttributeKind.Builtin:
            case AttributeKind.NoBuiltin:
            case AttributeKind.Cold:
            case AttributeKind.OptimizeNone:
            case AttributeKind.JumpTable:
            case AttributeKind.Convergent:
            case AttributeKind.ArgMemOnly:
            case AttributeKind.NoRecurse:
            case AttributeKind.InaccessibleMemOnly:
            case AttributeKind.InaccessibleMemOrArgMemOnly:
            case AttributeKind.AllocSize:
            case AttributeKind.Speculatable:
                return FunctionIndexKinds.Function;
            }
        }

        private static Function GetFunctionForAttributes( Value value )
        {
            switch( value )
            {
            case Function f:
                return f;

            case Instructions.Invoke inv:
                return inv.TargetFunction;

            case Instructions.CallInstruction call:
                return call.TargetFunction;

            case Argument arg:
                return arg.ContainingFunction;

            default:
                return null;
            }
        }

        // use complement and compare technique for efficiency
        private static bool IsPowerOfTwo( ulong x ) => ( x != 0 ) && ( ( x & ( ~x + 1 ) ) == x );

        // Lazy initialized one time mapping of LLVM attribut Ids to AttributeKind
        private static Lazy<Dictionary<uint, AttributeKind>> AttribIdToKindMap = new Lazy<Dictionary<uint, AttributeKind>>( BuildAttribIdToKindMap );

        private static Dictionary<uint, AttributeKind> BuildAttribIdToKindMap( )
        {
            return ( from kind in Enum.GetValues( typeof( AttributeKind ) ).Cast<AttributeKind>( ).Skip( 1 )
                     where kind != AttributeKind.EndAttrKinds
                     let name = kind.GetAttributeName( )
                     select new KeyValuePair<uint, AttributeKind>( NativeMethods.GetEnumAttributeKindForName( name, ( size_t )name.Length ), kind )
                   ).ToDictionary( ( KeyValuePair<uint, AttributeKind> kvp ) => kvp.Key, ( KeyValuePair<uint, AttributeKind> kvp ) => kvp.Value );
        }

        private static Lazy<Dictionary<AttributeKind, uint>> KindToAttribIdMap = new Lazy<Dictionary<AttributeKind, uint>>( BuildKindToAttribIdMap );

        private static Dictionary<AttributeKind, uint> BuildKindToAttribIdMap( )
        {
            return AttribIdToKindMap.Value.ToDictionary( kvp => kvp.Value, kvp => kvp.Key );
        }

        private static string[ ] KnownAttributeNames =
        {
            string.Empty,
            "align",
            "allocsize",
            "alwaysinline",
            "argmemonly",
            "builtin",
            "byval",
            "cold",
            "convergent",
            "dereferenceable",
            "dereferenceable_or_null",
            "inalloca",
            "inreg",
            "inaccessiblememonly",
            "inaccessiblemem_or_argmemonly",
            "inlinehint",
            "jumptable",
            "minsize",
            "naked",
            "nest",
            "noalias",
            "nobuiltin",
            "nocapture",
            "noduplicate",
            "noimplicitfloat",
            "noinline",
            "norecurse",
            "noredzone",
            "noreturn",
            "nounwind",
            "nonlazybind",
            "nonnull",
            "optsize",
            "optnone",
            "readnone",
            "readonly",
            "returned",
            "returns_twice",
            "signext",
            "safestack",
            "sanitize_address",
            "sanitize_memory",
            "sanitize_thread",
            "speculatable",
            "alignstack",
            "ssp",
            "sspreq",
            "sspstrong",
            "sret",
            "swifterror",
            "swiftself",
            "uwtable",
            "writeonly",
            "zeroext",
        };
    }
}

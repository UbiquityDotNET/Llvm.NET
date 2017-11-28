// <copyright file="AttributeKind.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Instructions;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

// TEMP: Disable these until all values are properly doc'd
#pragma warning disable SA1600, SA1602 // Enumeration items must be documented

namespace Llvm.NET.Values
{
    /// <summary>Enumeration for the known LLVM attributes</summary>
    /// <remarks>
    /// <para>It is important to note that the integer values of this enum do NOT necessarily
    /// correlate to the attribute IDs. LLVM has moved away from using an enum Flags model
    /// as the number of attributes reached the limit of available bits. Thus, the enum was
    /// dropped. Instead, strings are used to identify attributes. However, for maximum
    /// compatibility and ease of use for this library the enum is retained and the provided
    /// attribute manipulation classes will map the enum to the associated string.</para>
    /// <note type="warning">As a result of the changes in LLVM this set of attributes is
    /// fluid and subject to change from version to version. Thus, code using any attributes
    /// that have changed or were removed will produce compile time errors. That is useful
    /// and by design so that any changes in LLVM naming will break at compile time instead
    /// of at runtime.</note>
    /// </remarks>
    /// <seealso href="xref:llvm_langref#function-attributes">LLVM Function Attributes</seealso>
    /// <seealso href="xref:llvm_langref#parameter-attributes">LLVM Parameter Attributes</seealso>
    public enum AttributeKind
    {
        /// <summary>No attributes</summary>
        None,

        /// <summary>This indicates that the pointer value may be assumed by the optimizer to
        /// have the specified alignment.</summary>
        /// <remarks>
        /// <note type="note">
        /// This attribute has additional semantics when combined with the byval attribute.
        /// </note>
        /// </remarks>
        Alignment,

        /// <summary>This attribute indicates that the annotated function will always return at
        /// least a given number of bytes (or null).</summary>
        /// <remarks>Its arguments are zero-indexed parameter numbers; if one argument is provided,
        /// then it’s assumed that at least CallSite.Args[EltSizeParam] bytes will be available at
        /// the returned pointer. If two are provided, then it’s assumed that CallSite.Args[EltSizeParam]
        /// * CallSite.Args[NumEltsParam] bytes are available. The referenced parameters must be integer
        /// types. No assumptions are made about the contents of the returned block of memory.
        /// </remarks>
        AllocSize,

        /// <summary>is attribute indicates that the inliner should attempt to inline this function
        /// into callers whenever possible, ignoring any active inlining size threshold for this caller.</summary>
        AlwaysInline,

        /// <summary>indicates that the only memory accesses inside function are loads and stores from
        /// objects pointed to by its pointer-typed arguments, with arbitrary offsets</summary>
        /// <remarks>This attribute indicates that the only memory accesses inside function are loads and
        /// stores from objects pointed to by its pointer-typed arguments, with arbitrary offsets. Or in
        /// other words, all memory operations in the function can refer to memory only using pointers
        /// based on its function arguments. Note that argmemonly can be used together with readonly
        /// attribute in order to specify that function reads only from its arguments.</remarks>
        ArgMemOnly,

        /// <summary>This indicates that the callee function at a call site should be recognized as a
        /// built-in function, even though the function’s declaration uses the nobuiltin attribute.</summary>
        /// <remarks>
        /// This is only valid at call sites for direct calls to functions that are declared with the
        /// nobuiltin attribute.</remarks>
        Builtin,

        /// <summary>This indicates that the pointer parameter should really be passed by value to the function.</summary>
        /// <remarks>
        /// <para>The attribute implies that a hidden copy of the pointee is made between the caller and
        /// the callee, so the callee is unable to modify the value in the caller. This attribute is only
        /// valid on LLVM pointer arguments. It is generally used to pass structs and arrays by value, but
        /// is also valid on pointers to scalars. The copy is considered to belong to the caller not the
        /// callee (for example, readonly functions should not write to byval parameters). This is not a
        /// valid attribute for return values.</para>
        /// <para>The byval attribute also supports specifying an alignment with the align attribute. It
        /// indicates the alignment of the stack slot to form and the known alignment of the pointer
        /// specified to the call site. If the alignment is not specified, then the code generator makes
        /// a target-specific assumption.</para>
        /// </remarks>
        ByVal,

        Cold,
        Convergent,
        Dereferenceable,
        DereferenceableOrNull,

        /// <summary>The inalloca argument attribute allows the caller to take the address of outgoing stack arguments.</summary>
        /// <remarks>
        /// <para>An inalloca argument must be a pointer to stack memory produced by an <see cref="Alloca"/>
        /// instruction. The alloca, or argument allocation, must also be tagged with the inalloca keyword.
        /// Only the last argument may have the inalloca attribute, and that argument is guaranteed to be
        /// passed in memory.</para>
        /// <para>An argument allocation may be used by a call at most once because the call may deallocate
        /// it. The inalloca attribute cannot be used in conjunction with other attributes that affect argument
        /// storage, like <see cref="InReg"/>, <see cref="Nest"/>, <see cref="StructRet"/>, or <see cref="ByVal"/>.
        /// The inalloca attribute also disables LLVM’s implicit lowering of large aggregate return values,
        /// which means that frontend authors must lower them with sret pointers.</para>
        /// <para>When the call site is reached, the argument allocation must have been the most recent stack
        /// allocation that is still live, or the results are undefined. It is possible to allocate additional
        /// stack space after an argument allocation and before its call site, but it must be cleared off with
        /// llvm.stackrestore.</para>
        /// </remarks>
        InAlloca,

        /// <summary>This indicates that this parameter or return value should be treated in a special target-dependent fashion
        /// while emitting code for a function call or return (usually, by putting it in a register as opposed to memory, though
        /// some targets use it to distinguish between two different kinds of registers). Use of this attribute is target-specific.
        /// </summary>
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

        /// <summary>This indicates to the code generator that the parameter or return value should be sign-extended to the extent
        /// required by the target’s ABI (which is usually 32-bits) by the caller (for a parameter) or the callee (for a return value).
        /// </summary>
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

        /// <summary>This indicates to the code generator that the parameter or return value should be zero-extended to the extent
        /// required by the target’s ABI by the caller (for a parameter) or the callee (for a return value).</summary>
        ZExt,
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
        /// <summary>Gets the symbolic name of the attribute</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to get the name of</param>
        /// <returns>Name of the attribute</returns>
        public static string GetAttributeName( this AttributeKind kind )
        {
            return KnownAttributeNames[ ( int )kind ];
        }

        /// <summary>Gets a value indicating whether the attribute requires an integer parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires an integer value</returns>
        public static bool RequiresIntValue( this AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );

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

        /// <summary>Looks up the <see cref="AttributeKind"/> for an LLVM attribute id</summary>
        /// <param name="id">LLVM attribute id</param>
        /// <returns><see cref="AttributeKind"/> that corresponds to the LLVM id</returns>
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
            kind.ValidateDefined( nameof( kind ) );

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
            kind.ValidateDefined( nameof( kind ) );

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
            kind.ValidateDefined( nameof( kind ) );

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
            kind.ValidateDefined( nameof( kind ) );

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
                     let name = kind.GetAttributeName( )
                     select new KeyValuePair<uint, AttributeKind>( NativeMethods.LLVMGetEnumAttributeKindForName( name, ( size_t )name.Length ), kind )
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

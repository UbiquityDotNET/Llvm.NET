// -----------------------------------------------------------------------
// <copyright file="AttributeKindExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.AttributeBindings;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Enumeration flags to indicate which attribute set index an attribute may apply to</summary>
    [Flags]
    public enum AttributeAllowedOn
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
        public static string GetAttributeName(this AttributeKind kind)
        {
            using var nativeRet = LibLLVMGetAttributeKindName((LibLLVMAttrKind)kind);
            return nativeRet.ToString() ?? string.Empty;
        }

        /// <summary>Gets a value indicating whether the attribute requires no parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires no parameter value</returns>
        public static bool IsEnumKind(this AttributeKind kind)
        {
            return kind >= AttributeKind.FirstEnumAttr && kind <= AttributeKind.LastEnumAttr;
        }

        /// <summary>Gets a value indicating whether the attribute requires an integer parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires an integer value</returns>
        public static bool IsIntKind(this AttributeKind kind)
        {
            return kind >= AttributeKind.FirstIntAttr && kind <= AttributeKind.LastIntAttr;
        }

        /// <summary>Gets a value indicating whether the attribute requires a type parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires a type value</returns>
        public static bool IsTypeKind(this AttributeKind kind)
        {
            return kind >= AttributeKind.FirstTypeAttr && kind <= AttributeKind.LastTypeAttr;
        }

        /// <summary>Gets a value indicating whether the attribute requires a constant range parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires an constant range value</returns>
        public static bool IsConstantRangeKind(this AttributeKind kind)
        {
            return kind >= AttributeKind.FirstConstantRangeAttr && kind <= AttributeKind.LastConstantRangeAttr;
        }

        /// <summary>Gets a value indicating whether the attribute requires a constant range list parameter value</summary>
        /// <param name="kind"><see cref="AttributeKind"/> to check</param>
        /// <returns><see langword="true"/> if the attribute requires an constant range list value</returns>
        public static bool IsConstantRangeListKind(this AttributeKind kind)
        {
            return !Enum.IsDefined( kind )
                ? throw new ArgumentOutOfRangeException( nameof( kind ) )
                : kind >= AttributeKind.FirstConstantRangeListAttr && kind <= AttributeKind.LastConstantRangeListAttr;
        }

#if SEEMS_UNUSED_BUT_USEFUL
        internal static bool CheckAttributeUsage(this AttributeKind kind, FunctionAttributeIndex index, Value? value)
        {
            AttributeAllowedOn allowedIndexes = kind.GetAllowedIndexes( );
            switch(index)
            {
            case FunctionAttributeIndex.Function:
                return allowedIndexes.HasFlag( AttributeAllowedOn.Function );

            case FunctionAttributeIndex.ReturnType:
                return allowedIndexes.HasFlag( AttributeAllowedOn.Return );

            // case FunctionAttributeIndex.Parameter0:
            default:
            {
                ArgumentNullException.ThrowIfNull( value );

                if(!allowedIndexes.HasFlag( AttributeAllowedOn.Parameter ))
                {
                    return false;
                }

                IrFunction? function;
                switch(value)
                {
                case IrFunction f:
                    function = f;
                    break;

                case Invoke inv:
                    function = inv.TargetFunction;
                    break;

                case CallInstruction call:
                    function = call.TargetFunction;
                    break;

                case Argument arg:
                    function = arg.ContainingFunction;
                    if(index != FunctionAttributeIndex.Parameter0 + (int)arg.Index)
                    {
                        return false;
                    }

                    break;

                default:
                    function = null;
                    break;
                }

                int paramIndex = index - FunctionAttributeIndex.Parameter0;
                if(paramIndex >= (function?.Parameters.Count ?? 0))
                {
                    return false;
                }
            }

            break;
            }

            return true;
        }
#endif
        internal static void VerifyAttributeUsage(this AttributeKind kind, FunctionAttributeIndex index, Value value)
        {
            VerifyAttributeUsage( kind, index );

            if(index >= FunctionAttributeIndex.Parameter0)
            {
                Function? function;
                switch(value)
                {
                case Function f:
                    function = f;
                    break;

                case Invoke inv:
                    function = inv.TargetFunction;
                    break;

                case CallInstruction call:
                    function = call.TargetFunction;
                    break;

                case Argument arg:
                    function = arg.ContainingFunction;
                    if(index != FunctionAttributeIndex.Parameter0 + (int)arg.Index)
                    {
                        throw new ArgumentException( Resources.Index_for_parameters_must_be_the_actual_position_of_the_argument );
                    }

                    break;

                default:
                    function = null;
                    break;
                }

                int paramIndex = index - FunctionAttributeIndex.Parameter0;
                if(paramIndex > ((function?.Parameters.Count ?? 0) - 1))
                {
                    throw new ArgumentException( Resources.Specified_parameter_index_exceeds_the_number_of_parameters_in_the_function, nameof( index ) );
                }
            }
        }

        internal static void VerifyAttributeUsage(this AttributeKind kind, FunctionAttributeIndex index)
        {
            AttributeAllowedOn allowedIndexes = kind.GetAllowedIndexes( );
            switch(index)
            {
            case FunctionAttributeIndex.Function:
                if(!allowedIndexes.HasFlag( AttributeAllowedOn.Function ))
                {
                    throw new ArgumentException( Resources.Attribute_not_allowed_on_functions, nameof( kind ) );
                }

                break;

            case FunctionAttributeIndex.ReturnType:
                if(!allowedIndexes.HasFlag( AttributeAllowedOn.Return ))
                {
                    throw new ArgumentException( Resources.Attribute_not_allowed_on_function_Return, nameof( kind ) );
                }

                break;

            // case FunctionAttributeIndex.Parameter0:
            default:
                if(!allowedIndexes.HasFlag( AttributeAllowedOn.Parameter ))
                {
                    throw new ArgumentException( Resources.Attribute_not_allowed_on_function_parameter, nameof( kind ) );
                }

                break;
            }
        }

        // To prevent native asserts or crashes - validates parameters before passing down to native code
        internal static void VerifyAttributeUsage(this AttributeKind kind, FunctionAttributeIndex index, ulong value)
        {
            if (!kind.IsIntKind())
            {
                throw new ArgumentOutOfRangeException(nameof(kind), Resources.Attribute_0_does_not_support_an_argument);
            }

            kind.VerifyAttributeUsage( index );
            kind.RangeCheckValue( value );
        }

        private static void RangeCheckValue(this AttributeKind kind, ulong value)
        {
            // To prevent native asserts or crashes - validate parameters before passing down to native code
            switch(kind)
            {
            case AttributeKind.Alignment:
                if(value > UInt32.MaxValue)
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), Resources.Expected_a_32_bit_value_for_alignment );
                }

                break;

            case AttributeKind.StackAlignment:
                if(value > UInt32.MaxValue)
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), Resources.Expected_a_32_bit_value_for_stack_alignment );
                }

                if(value != 0 && !IsPowerOfTwo( value ))
                {
                    throw new ArgumentException( Resources.Stack_alignment_value_must_be_a_power_of_2, nameof( value ) );
                }

                break;

            case AttributeKind.AllocKind:
            case AttributeKind.AllocSize:
            case AttributeKind.Captures:
            case AttributeKind.Dereferenceable:
            case AttributeKind.DereferenceableOrNull:
            case AttributeKind.Memory:
            case AttributeKind.NoFPClass:
            case AttributeKind.UWTable:
            case AttributeKind.VScaleRange:
                break;  // TODO: The set of attributes supporting an integer argument in LLVM 20 has grown considerably, check them here...

            default:
                throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Resources.Attribute_0_does_not_support_an_argument, kind ), nameof( kind ) );
            }
        }

        private static AttributeAllowedOn GetAllowedIndexes(this AttributeKind kind)
        {
            // TODO: update this based on latest attribute definitions for LLVM
            return kind switch
            {
                AttributeKind.ReadOnly or
                AttributeKind.WriteOnly => AttributeAllowedOn.Function | AttributeAllowedOn.Parameter,

                AttributeKind.ByVal or
                AttributeKind.InAlloca or
                AttributeKind.StructRet or
                AttributeKind.Nest or
                AttributeKind.NoCapture or
                AttributeKind.Returned or
                AttributeKind.ReadNone or
                AttributeKind.SwiftSelf or
                AttributeKind.SwiftError => AttributeAllowedOn.Parameter,

                AttributeKind.ZExt or
                AttributeKind.SExt or
                AttributeKind.InReg or
                AttributeKind.Alignment or
                AttributeKind.NoAlias or
                AttributeKind.NonNull or
                AttributeKind.Dereferenceable or
                AttributeKind.DereferenceableOrNull => AttributeAllowedOn.Parameter | AttributeAllowedOn.Return,

                AttributeKind.StackAlignment or
                AttributeKind.AllocKind or
                AttributeKind.NoReturn or
                AttributeKind.NoUnwind or
                AttributeKind.NoInline or
                AttributeKind.AlwaysInline or
                AttributeKind.OptimizeForSize or
                AttributeKind.StackProtect or
                AttributeKind.StackProtectReq or
                AttributeKind.StackProtectStrong or
                AttributeKind.SafeStack or
                AttributeKind.NoRedZone or
                AttributeKind.NoImplicitFloat or
                AttributeKind.Naked or
                AttributeKind.InlineHint or
                AttributeKind.StackAlignment or
                AttributeKind.UWTable or
                AttributeKind.NonLazyBind or
                AttributeKind.ReturnsTwice or
                AttributeKind.SanitizeAddress or
                AttributeKind.SanitizeThread or
                AttributeKind.SanitizeMemory or
                AttributeKind.MinSize or
                AttributeKind.NoDuplicate or
                AttributeKind.BuiltIn or
                AttributeKind.NoBuiltIn or
                AttributeKind.Cold or
                AttributeKind.OptimizeNone or
                AttributeKind.JumpTable or
                AttributeKind.Convergent or
                AttributeKind.NoRecurse or
                AttributeKind.AllocSize or
                AttributeKind.Memory or
                AttributeKind.Speculatable => AttributeAllowedOn.Function,

                _ => AttributeAllowedOn.None,
            };
        }

        // use complement and compare technique for efficiency
        private static bool IsPowerOfTwo(ulong x) => (x != 0) && ((x & (~x + 1)) == x);
    }
}

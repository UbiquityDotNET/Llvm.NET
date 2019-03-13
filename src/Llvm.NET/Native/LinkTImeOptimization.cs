// -----------------------------------------------------------------------
// <copyright file="LinkTImeOptimization.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Llvm.NET.Native
{
    internal class LinkTImeOptimization
    {
        /* LTO
        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal struct llvm_lto_t
        {
            internal llvm_lto_t( IntPtr pointer )
            {
                Pointer = pointer;
            }

            internal IntPtr Pointer { get; }
        }

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal struct lto_bool_t
        {
            internal lto_bool_t( bool value )
            {
                Value = value;
            }

            internal bool Value;
        }

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal struct lto_module_t
        {
            internal lto_module_t( IntPtr pointer )
            {
                Pointer = pointer;
            }

            internal IntPtr Pointer { get; }
        }

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal struct lto_code_gen_t
        {
            internal lto_code_gen_t( IntPtr pointer )
            {
                Pointer = pointer;
            }

            internal IntPtr Pointer { get; }
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal delegate void lto_diagnostic_handler_t( lto_codegen_diagnostic_severity_t severity, [MarshalAs( UnmanagedType.LPStr )] string diag, IntPtr ctxt );

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal enum llvm_lto_status
        {
            LLVM_LTO_UNKNOWN = 0,
            LLVM_LTO_OPT_SUCCESS = 1,
            LLVM_LTO_READ_SUCCESS = 2,
            LLVM_LTO_READ_FAILURE = 3,
            LLVM_LTO_WRITE_FAILURE = 4,
            LLVM_LTO_NO_TARGET = 5,
            LLVM_LTO_NO_WORK = 6,
            LLVM_LTO_MODULE_MERGE_FAILURE = 7,
            LLVM_LTO_ASM_FAILURE = 8,
            LLVM_LTO_NULL_OBJECT = 9
        }

    #pragma warning disable CA1008 // Enums should have zero value.
        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal enum lto_symbol_attributes
        {
            LTO_SYMBOL_ALIGNMENT_MASK = 31,
            LTO_SYMBOL_PERMISSIONS_MASK = 224,
            LTO_SYMBOL_PERMISSIONS_CODE = 160,
            LTO_SYMBOL_PERMISSIONS_DATA = 192,
            LTO_SYMBOL_PERMISSIONS_RODATA = 128,
            LTO_SYMBOL_DEFINITION_MASK = 1792,
            LTO_SYMBOL_DEFINITION_REGULAR = 256,
            LTO_SYMBOL_DEFINITION_TENTATIVE = 512,
            LTO_SYMBOL_DEFINITION_WEAK = 768,
            LTO_SYMBOL_DEFINITION_UNDEFINED = 1024,
            LTO_SYMBOL_DEFINITION_WEAKUNDEF = 1280,
            LTO_SYMBOL_SCOPE_MASK = 14336,
            LTO_SYMBOL_SCOPE_INTERNAL = 2048,
            LTO_SYMBOL_SCOPE_HIDDEN = 4096,
            LTO_SYMBOL_SCOPE_PROTECTED = 8192,
            LTO_SYMBOL_SCOPE_DEFAULT = 6144,
            LTO_SYMBOL_SCOPE_DEFAULT_CAN_BE_HIDDEN = 10240,
            LTO_SYMBOL_COMDAT = 16384,
            LTO_SYMBOL_ALIAS = 32768
        }
    #pragma warning restore CA1008 // Enums should have zero value.

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal enum lto_debug_model
        {
            LTO_DEBUG_MODEL_NONE = 0,
            LTO_DEBUG_MODEL_DWARF = 1
        }

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal enum lto_codegen_model
        {
            LTO_CODEGEN_PIC_MODEL_STATIC = 0,
            LTO_CODEGEN_PIC_MODEL_DYNAMIC = 1,
            LTO_CODEGEN_PIC_MODEL_DYNAMIC_NO_PIC = 2,
            LTO_CODEGEN_PIC_MODEL_DEFAULT = 3
        }

        [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Low level Interop API matching" )]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:Element must begin with upper-case letter", Justification = "Matches interop type" )]
        internal enum lto_codegen_diagnostic_severity_t
        {
            LTO_DS_ERROR = 0,
            LTO_DS_WARNING = 1,
            LTO_DS_REMARK = 3,
            LTO_DS_NOTE = 2
        }
        */
    }
}

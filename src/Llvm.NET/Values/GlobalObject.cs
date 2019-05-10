// <copyright file="GlobalObject.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Interop;
using Llvm.NET.Properties;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>Base class for Global objects in an LLVM Module</summary>
    public class GlobalObject
        : GlobalValue
    {
        /// <summary>Gets or sets the alignment requirements for this object</summary>
        public uint Alignment
        {
            get => LLVMGetAlignment( ValueHandle );
            set => LLVMSetAlignment( ValueHandle, value );
        }

        /// <summary>Gets or sets the linker section this object belongs to</summary>
        public string Section
        {
            get => LLVMGetSection( ValueHandle );
            set => LLVMSetSection( ValueHandle, value );
        }

        /// <summary>Gets or sets the comdat attached to this object, if any</summary>
        /// <remarks>
        /// Setting this property to <see langword="null"/> or an
        /// empty string will remove any comdat setting for the
        /// global object.
        /// </remarks>
        public Comdat Comdat
        {
            get
            {
                LLVMComdatRef comdatRef = LibLLVMGlobalObjectGetComdat( ValueHandle );
                return comdatRef == default ? null : new Comdat( ParentModule, comdatRef );
            }

            set
            {
                if( value != null && value.Module != ParentModule )
                {
                    throw new ArgumentException( Resources.Mismatched_modules_for_Comdat, nameof( value ) );
                }

                LibLLVMGlobalObjectSetComdat( ValueHandle, value?.ComdatHandle ?? LLVMComdatRef.Zero );
            }
        }

        /* TODO: Add GlobalObject metadata accessors
        public IEnumerable<MDNode> GetMetadata() {...}


        public void SetMetadata(unsigned kindID, MDNode node) {...}
        public void SetMetadata(string kind, MDNode node) {...}
        */
        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

// <copyright file="GlobalObject.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Properties;

namespace Llvm.NET.Values
{
    /// <summary>Base class for Global objects in an LLVM Module</summary>
    public class GlobalObject
        : GlobalValue
    {
        /// <summary>Gets or sets the alignment requirements for this object</summary>
        public uint Alignment
        {
            get => NativeMethods.LLVMGetAlignment( ValueHandle );
            set => NativeMethods.LLVMSetAlignment( ValueHandle, value );
        }

        /// <summary>Gets or sets the linker section this object belongs to</summary>
        public string Section
        {
            get => NativeMethods.LLVMGetSection( ValueHandle );
            set => NativeMethods.LLVMSetSection( ValueHandle, value );
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
                LLVMComdatRef comdatRef = NativeMethods.LLVMGlobalObjectGetComdat( ValueHandle );
                if( comdatRef == default )
                {
                    return null;
                }

                return new Comdat( ParentModule, comdatRef );
            }

            set
            {
                if( value != null && value.Module != ParentModule )
                {
                    throw new ArgumentException( Resources.Mismatched_modules_for_Comdat, nameof( value ) );
                }

                NativeMethods.LLVMGlobalObjectSetComdat( ValueHandle, value?.ComdatHandle?? new LLVMComdatRef( IntPtr.Zero ) );
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

// <copyright file="GlobalObject.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Base class for Global objects in an LLVM Module</summary>
    public class GlobalObject
        : GlobalValue
    {
        /// <summary>Gets or sets the alignment requirements for this object</summary>
        public uint Alignment
        {
            get => NativeMethods.GetAlignment( ValueHandle );
            set => NativeMethods.SetAlignment( ValueHandle, value );
        }

        /// <summary>Gets or sets the linker section this object belongs to</summary>
        public string Section
        {
            get => NativeMethods.GetSection( ValueHandle );
            set => NativeMethods.SetSection( ValueHandle, value );
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
                LLVMComdatRef comdatRef = NativeMethods.GlobalObjectGetComdat( ValueHandle );
                if( comdatRef.Pointer.IsNull( ) )
                {
                    return null;
                }

                return new Comdat( ParentModule, comdatRef );
            }

            set
            {
                if( value != null && value.Module != ParentModule )
                {
                    throw new ArgumentException( "Mismatched modules for Comdat", nameof( value ) );
                }

                NativeMethods.GlobalObjectSetComdat( ValueHandle, value?.ComdatHandle?? new LLVMComdatRef( IntPtr.Zero ) );
            }
        }

        /* TODO:
        public MDNode GetMetadata(unsigned kindID) {...}
        public MDNode GetMetadata(string kind) {...}

        public MDNode GetMetadata(unsigned kindID, IEnumerable<MDNode> nodes) {...}
        public MDNode GetMetadata(string kind, IEnumerable<MDNode> nodes) {...}

        public MDNode SetMetadata(unsigned kindID, MDNode node) {...}
        public MDNode SetMetadata(string kind, MDNode node) {...}
        */
        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}

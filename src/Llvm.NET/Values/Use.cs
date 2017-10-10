// <copyright file="Use.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Use, which is essentially a tuple of the <see cref="User"/> and the <see cref="Value"/> used</summary>
    /// <remarks>
    /// A Use in LLVM forms a link in a directed graph of dependencies for values.
    /// </remarks>
    public class Use
    {
        /// <summary>Gets the <see cref="User"/> of this <see cref="Use"/></summary>
        public User User => Value.FromHandle<User>( NativeMethods.GetUser( OpaqueHandle ) );

        /// <summary>Gets the <see cref="Value"/> used</summary>
        public Value Value => Value.FromHandle( NativeMethods.GetUsedValue( OpaqueHandle ) );

        /// <summary>Creates a new <see cref="Use"/> from  low level LLVM <see cref="LLVMUseRef"/></summary>
        /// <param name="useRef">LLVM raw reference</param>
        internal Use( LLVMUseRef useRef )
        {
            OpaqueHandle = useRef;
        }

        private LLVMUseRef OpaqueHandle;
    }
}

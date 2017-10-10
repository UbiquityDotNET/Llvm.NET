// <copyright file="InstructionExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.Instructions
{
    /// <summary>Provides extension methods to <see cref="Instruction"/> that cannot be achieved as members of the class</summary>
    /// <remarks>
    /// Using generic static extension methods allows for fluent coding while retaining the type of the "this" parameter.
    /// If these were members of the <see cref="Instruction"/> class then the only return type could be <see cref="Instruction"/>
    /// thus losing the original type and requiring a cast to get back to it, thereby defeating the purpose of the fluent style.
    /// </remarks>
    public static class InstructionExtensions
    {
        /// <summary>Fluent style extension method to set the <see cref="Instruction.Alignment"/> for an instruction</summary>
        /// <typeparam name="T">Type of the instruction (usually implicitly inferred from usage)</typeparam>
        /// <param name="self">Instruction to set the <see cref="Instruction.Alignment"/> for</param>
        /// <param name="value">New alignment for the instruction</param>
        /// <returns>To allow fluent style coding this returns the <paramref name="self"/> parameter</returns>
        public static T Alignment<T>( this T self, uint value )
            where T : Instruction
        {
            if( self.IsMemoryAccess )
            {
                self.Alignment = value;
            }

            return self;
        }

#pragma warning disable IDE0019 // Use Pattern matching - doesn't work for generics (Expected in C#7.X)

        /// <summary>Fluent style extension method to set the Volatile property of a <see cref="Load"/> or <see cref="Store"/> instruction</summary>
        /// <typeparam name="T">Type of the instruction (usually implicitly inferred from usage)</typeparam>
        /// <param name="self">Instruction to set the Volatile property for</param>
        /// <param name="value">Flag to indicate if the instruction's operation is volatile</param>
        /// <returns>To allow fluent style coding this returns the <paramref name="self"/> parameter</returns>
        public static T IsVolatile<T>( this T self, bool value )
            where T : Instruction
        {
            if( self.IsMemoryAccess )
            {
                // only load and store instructions have the volatile property
                var loadInst = self as Load;
                if( loadInst != null )
                {
                    loadInst.IsVolatile = value;
                }
                else
                {
                    var storeinst = self as Store;
                    if( storeinst != null )
                    {
                        storeinst.IsVolatile = value;
                    }
                }
            }

            return self;
        }
    }
#pragma warning restore IDE0019 // Use Pattern matching - doesn't work for generics (Expected in C#7.X)
}

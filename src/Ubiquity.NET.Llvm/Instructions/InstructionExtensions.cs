// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be suppressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx don't support the new syntax yet)
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

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
        public static T SetAlignment<T>( this T self, uint value )
            where T : Instruction
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsMemoryAccess)
            {
                self.Alignment = value;
            }

            return self;
        }

        /// <summary>Fluent style extension method to set the Volatile property of a <see cref="Load"/> or <see cref="Store"/> instruction</summary>
        /// <typeparam name="T">Type of the instruction (usually implicitly inferred from usage)</typeparam>
        /// <param name="self">Instruction to set the Volatile property for</param>
        /// <param name="value">Flag to indicate if the instruction's operation is volatile</param>
        /// <returns>To allow fluent style coding this returns the <paramref name="self"/> parameter</returns>
        public static T SetIsVolatile<T>( this T self, bool value )
            where T : Instruction
        {
            ArgumentNullException.ThrowIfNull( self );

            if(self.IsMemoryAccess)
            {
                // only load and store instructions have the volatile property
                if(self is Load loadInst)
                {
                    loadInst.IsVolatile = value;
                }
                else
                {
                    if(self is Store storeinst)
                    {
                        storeinst.IsVolatile = value;
                    }
                }
            }

            return self;
        }
    }
}

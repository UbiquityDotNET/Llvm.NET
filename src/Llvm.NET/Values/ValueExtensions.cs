// <copyright file="ValueExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.DebugInfo;
using Llvm.NET.Instructions;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Provides extension methods to <see cref="Value"/> that cannot be achieved as members of the class</summary>
    /// <remarks>
    /// Using generic static extension methods allows for fluent coding while retaining the type of the "this" parameter.
    /// If these were members of the <see cref="Value"/> class then the only return type could be <see cref="Value"/>,
    /// thus losing the original type and requiring a cast to get back to it.
    /// </remarks>
    public static class ValueExtensions
    {
        /// <summary>Sets the debugging location for a value</summary>
        /// <typeparam name="T"> Type of the value to tag</typeparam>
        /// <param name="value">Value to set debug location for</param>
        /// <param name="location">Debug location information</param>
        /// <remarks>
        /// <para>Technically speaking only an <see cref="Instructions.Instruction"/> can have debug location
        /// information. However, since LLVM will perform constant folding in the <see cref="InstructionBuilder"/>
        /// most of the methods in <see cref="InstructionBuilder"/> return a <see cref="Value"/> rather than a
        /// more specific <see cref="Instructions.Instruction"/>. Thus, without this extension method, calling code
        /// would need to know ahead of time that an actual instruction would be produced then cast the result to an
        /// <see cref="Instructions.Instruction"/> and then set the debug location. This makes the code rather ugly
        /// and tedious to manage. Placing this as a generic extension method ensures that the return type matches
        /// the original and no additional casting is needed, which would defeat the purpose of doing this. For
        /// <see cref="Value"/> types that are not instructions this does nothing. This allows for a simpler fluent
        /// style of programming where the actual type is retained even in cases where an <see cref="InstructionBuilder"/>
        /// method will always return an actual instruction.</para>
        /// <para>In order to help simplify code generation for cases where not all of the source information is
        /// available this is a NOP if <paramref name="location"/> is null. Thus, it is safe to call even when
        /// debugging information isn't actually available. This helps to avoid cluttering calling code with test
        /// for debug info before trying to add it.</para>
        /// </remarks>
        /// <returns><paramref name="value"/> for fluent usage</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static T SetDebugLocation<T>( this T value, DILocation location )
            where T : Value
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( location == null )
            {
                return value;
            }

            if( value is Instructions.Instruction instruction )
            {
                if( !location.Scope.SubProgram.Describes( instruction.ContainingBlock.ContainingFunction ) )
                {
                    throw new ArgumentException( "Location does not describe the function containing the provided instruction", nameof( location ) );
                }

                NativeMethods.LLVMSetDILocation( value.ValueHandle, location.MetadataHandle );
            }

            return value;
        }

        /// <summary>Sets the debugging location for a value</summary>
        /// <typeparam name="T"> Type of the value to tag</typeparam>
        /// <param name="value">Value to set debug location for</param>
        /// <param name="line">Line number</param>
        /// <param name="column">Column number</param>
        /// <param name="scope">Scope for the value</param>
        /// <remarks>
        /// <para>Technically speaking only an <see cref="Instructions.Instruction"/> can have debug location
        /// information. However, since LLVM will perform constant folding in the <see cref="InstructionBuilder"/>
        /// most of the methods in <see cref="InstructionBuilder"/> return a <see cref="Value"/> rather than a
        /// more specific <see cref="Instructions.Instruction"/>. Thus, without this extension method here,
        /// code would need to know ahead of time that an actual instruction would be produced then cast the result
        /// to an <see cref="Instructions.Instruction"/> and then set the debug location. This makes the code rather
        /// ugly and tedious to manage. Placing this as a generic extension method ensures that the return type matches
        /// the original and no additional casting is needed, which would defeat the purpose of doing this. For
        /// <see cref="Value"/> types that are not instructions this does nothing. This allows for a simpler fluent
        /// style of programming where the actual type is retained even in cases where an <see cref="InstructionBuilder"/>
        /// method will always return an actual instruction.</para>
        /// <para>In order to help simplify code generation for cases where not all of the source information is
        /// available this is a NOP if <paramref name="scope"/> is null. Thus, it is safe to call even when debugging
        /// information isn't actually available. This helps to avoid cluttering calling code with test for debug info
        /// before trying to add it.</para>
        /// </remarks>
        /// <returns><paramref name="value"/> for fluent usage</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public static T SetDebugLocation<T>( this T value, uint line, uint column, DebugInfo.DILocalScope scope )
            where T : Value
        {
            if( scope == null )
            {
                return value;
            }

            if( value is Instructions.Instruction instruction )
            {
                if( !scope.SubProgram.Describes( instruction.ContainingBlock.ContainingFunction ) )
                {
                    throw new ArgumentException( "scope does not describe the function containing the provided instruction", nameof( scope ) );
                }

                NativeMethods.LLVMSetDebugLoc( value.ValueHandle, line, column, scope.MetadataHandle );
            }

            return value;
        }

        /// <summary>Sets the virtual register name for a value</summary>
        /// <typeparam name="T"> Type of the value to set the name for</typeparam>
        /// <param name="value">Value to set register name for</param>
        /// <param name="name">Name for the virtual register the value represents</param>
        /// <remarks>
        /// <para>Technically speaking only an <see cref="Instructions.Instruction"/> can have register name
        /// information. However, since LLVM will perform constant folding in the <see cref="InstructionBuilder"/>
        /// it almost all of the methods in <see cref="InstructionBuilder"/> return a <see cref="Value"/> rather
        /// than an more specific <see cref="Instructions.Instruction"/>. Thus, without this extension method here,
        /// code would need to know ahead of time that an actual instruction would be produced then cast the result
        /// to an <see cref="Instructions.Instruction"/> and then set the debug location. This makes the code rather
        /// ugly and tedious to manage. Placing this as a generic extension method ensures that the return type matches
        /// the original and no additional casting is needed, which would defeat the purpose of doing this. For
        /// <see cref="Value"/> types that are not instructions this does nothing. This allows for a simpler fluent
        /// style of programming where the actual type is retained even in cases where an <see cref="InstructionBuilder"/>
        /// method will always return an actual instruction.</para>
        /// <para>Since the <see cref="Value.Name"/> property is available on all <see cref="Value"/>s this is slightly
        /// redundant. It is useful for maintaining the fluent style of coding along with expressing intent more clearly.
        /// (e.g. using this makes it expressly clear that the intent is to set the virtual register name and not the
        /// name of a local variable etc...) Using the fluent style allows a significant reduction in the number of
        /// overloaded methods in <see cref="InstructionBuilder"/> to account for all variations with or without a name.
        /// </para>
        /// </remarks>
        /// <returns><paramref name="value"/> for fluent usage</returns>
        public static T RegisterName<T>( this T value, string name )
            where T : Value
        {
            if( value is Instructions.Instruction inst )
            {
                value.Name = name;
            }

            return value;
        }
    }
}

// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ModuleBindings;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Fluent style extensions for properties of <see cref="GlobalObject"/></summary>
    public static class GlobalObjectExtensions
    {
        /// <summary>Sets a named <see cref="Ubiquity.NET.Llvm.Comdat"/> for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Global to get the Comdat for</param>
        /// <param name="name">name of the ComDat</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This finds a <see cref="Ubiquity.NET.Llvm.Comdat"/> in the <see cref="GlobalValue.ParentModule"/>
        /// of the object if it exists or creates a new one if it doesn't and assigns the <see cref="GlobalObject.Comdat"/>
        /// property with the <see cref="Ubiquity.NET.Llvm.Comdat"/>.
        /// </remarks>
        public static GlobalObject Comdat( this GlobalObject self, string name ) => Comdat( self, name, ComdatKind.Any );

        /// <summary>Sets a named <see cref="Ubiquity.NET.Llvm.Comdat"/> for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Global to get the Comdat for</param>
        /// <param name="name">name of the Comdat</param>
        /// <param name="kind">Id of Comdat to create if it doesn't exist already</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This finds a <see cref="Ubiquity.NET.Llvm.Comdat"/> in the <see cref="GlobalValue.ParentModule"/>
        /// of the object if it exists or creates a new one if it doesn't and assigns the <see cref="GlobalObject.Comdat"/>
        /// property with the <see cref="Ubiquity.NET.Llvm.Comdat"/>.
        /// </remarks>
        /// <seealso cref="GlobalObject.Comdat"/>
        public static GlobalObject Comdat( this GlobalObject self, string name, ComdatKind kind )
        {
            ArgumentNullException.ThrowIfNull( self );
            self.Comdat = new( LibLLVMModuleInsertOrUpdateComdat( self.ParentModule.GetUnownedHandle(), name, (LLVMComdatSelectionKind)kind ) );
            return self;
        }

        /// <summary>Sets the linker section name for a <see cref="GlobalObject"/></summary>
        /// <param name="self"><see cref="GlobalObject"/> to set the section for</param>
        /// <param name="name">Name of the section</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <seealso cref="GlobalObject.Section"/>
        public static GlobalObject SectionName( this GlobalObject self, string name )
        {
            ArgumentNullException.ThrowIfNull( self );

            self.Section = name;
            return self;
        }

        /// <summary>Sets the alignment for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Object to set the alignment of</param>
        /// <param name="value">Alignment value to set</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <seealso cref="GlobalObject.Alignment"/>
        public static GlobalObject Alignment( this GlobalObject self, uint value )
        {
            ArgumentNullException.ThrowIfNull( self );

            self.Alignment = value;
            return self;
        }
    }
}

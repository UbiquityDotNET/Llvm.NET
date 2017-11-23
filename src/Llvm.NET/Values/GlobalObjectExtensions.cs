// <copyright file="GlobalObjectExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Values
{
    /// <summary>Fluent style extensions for properties of <see cref="GlobalObject"/></summary>
    public static class GlobalObjectExtensions
    {
        /// <summary>Sets a named <see cref="Llvm.NET.Comdat"/> for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Global to get the Comdat for</param>
        /// <param name="name">name of the ComDat</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This finds a <see cref="Llvm.NET.Comdat"/> in the <see cref="GlobalValue.ParentModule"/>
        /// of the object if it exists or creates a new one if it doesn't and assigns the <see cref="GlobalObject.Comdat"/>
        /// property with the <see cref="Llvm.NET.Comdat"/>.
        /// </remarks>
        public static GlobalObject Comdat( this GlobalObject self, string name ) => Comdat( self, name, ComdatKind.Any );

        /// <summary>Sets a named <see cref="Llvm.NET.Comdat"/> for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Global to get the Comdat for</param>
        /// <param name="name">name of the Comdat</param>
        /// <param name="kind">Kind of Comdat to create if it doesn't exist already</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <remarks>
        /// This finds a <see cref="Llvm.NET.Comdat"/> in the <see cref="GlobalValue.ParentModule"/>
        /// of the object if it exists or creates a new one if it doesn't and assigns the <see cref="GlobalObject.Comdat"/>
        /// property with the <see cref="Llvm.NET.Comdat"/>.
        /// </remarks>
        /// <seealso cref="GlobalObject.Comdat"/>
        public static GlobalObject Comdat( this GlobalObject self, string name, ComdatKind kind )
        {
            if( self == null )
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            if(!self.ParentModule.Comdats.TryGetValue( name, out Comdat comdat ))
            {
                comdat = self.ParentModule.Comdats.InsertOrUpdate( name, kind );
            }
            else
            {
                comdat.Kind = kind;
            }

            self.Comdat = comdat;
            return self;
        }

        /// <summary>Sets the linker section name for a <see cref="GlobalObject"/></summary>
        /// <param name="self"><see cref="GlobalObject"/> to set the section for</param>
        /// <param name="name">Name of the section</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <seealso cref="GlobalObject.Section"/>
        public static GlobalObject SectionName( this GlobalObject self, string name )
        {
            if( self == null )
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            self.Section = name;
            return self;
        }

        /// <summary>Sets the alignment for a <see cref="GlobalObject"/></summary>
        /// <param name="self">Object to set the alignement of</param>
        /// <param name="value">Alignment value to set</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        /// <seealso cref="GlobalObject.Alignment"/>
        public static GlobalObject Alignment( this GlobalObject self, uint value )
        {
            if( self == null )
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            self.Alignment = value;
            return self;
        }
    }
}

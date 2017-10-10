// <copyright file="GlobalObjectExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Values
{
    /// <summary>Fluent style extensions for properties of <see cref="GlobalObject"/></summary>
    public static class GlobalObjectExtensions
    {
        public static GlobalObject Comdat( this GlobalObject self, string name ) => Comdat( self, name, ComdatKind.Any );

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

        public static void SectionName( this GlobalObject self, string name )
        {
            if( self == null )
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            self.Section = name;
        }

        public static void Alignment( this GlobalObject self, uint value )
        {
            if( self == null )
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            self.Alignment = value;
        }
    }
}

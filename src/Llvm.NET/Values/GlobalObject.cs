using System.Runtime.InteropServices;
using Llvm.NET.Native;
using System;

namespace Llvm.NET.Values
{
    public class GlobalObject 
        : GlobalValue
    {
        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        /// <summary>Alignment requirements for this object</summary>
        public uint Alignment
        {
            get
            {
                return NativeMethods.GetAlignment( ValueHandle );
            }
            set
            {
                NativeMethods.SetAlignment( ValueHandle, value );
            }
        }

        /// <summary>Linker section this object belongs to</summary>
        public string Section
        {
            get
            {
                var ptr = NativeMethods.GetSection( ValueHandle );
                return Marshal.PtrToStringAnsi( ptr );
            }
            set
            {
                NativeMethods.SetSection( ValueHandle, value );
            }
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
                    return null;

                return new Comdat( ParentModule, comdatRef );
            }

            set
            {

                if( value != null && value.Module != ParentModule )
                    throw new ArgumentException( "Mismatched modules for Comdat", nameof( value ) );

                NativeMethods.GlobalObjectSetComdat( ValueHandle, value?.ComdatHandle?? new LLVMComdatRef( IntPtr.Zero ) );
            }
        }
    }

    /// <summary>Fluent style extensions for properties of <see cref="GlobalObject"/></summary>
    public static class GlobalObjectExtensions
    {
        public static GlobalObject Comdat( this GlobalObject self, string name ) => Comdat( self, name, ComdatKind.Any );

        public static GlobalObject Comdat( this GlobalObject self, string name, ComdatKind kind )
        {
            if( self == null )
                throw new ArgumentNullException( nameof( self ) );

            Comdat comdat;
            if( !self.ParentModule.Comdats.TryGetValue( name, out comdat ) )
            {
                comdat = self.ParentModule.Comdats.Add( name, kind );
            }
            NativeMethods.GlobalObjectSetComdat( self.ValueHandle, comdat.ComdatHandle );
            return self;
        }

        public static void SectionName( this GlobalObject self, string name )
        {
            if( self == null )
                throw new ArgumentNullException( nameof( self ) );

            self.Section = name;
        }

        public static void Alignment( this GlobalObject self, uint alignment )
        {
            if( self == null )
                throw new ArgumentNullException( nameof( self ) );

            self.Alignment = alignment;
        }
    }
}

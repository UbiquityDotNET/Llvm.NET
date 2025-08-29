// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    // NOTE: these are intentionally, NOT record structs as those do NOT allow unsafe types as fields (CS8908)
    // Sadly, there is a bug where that error won't show in an IDE so it might look safe/OK until you build
    // (see: https://github.com/dotnet/roslyn/issues/58878)

    [StructLayout( LayoutKind.Sequential )]
    public unsafe readonly struct LLVMOpInfoSymbol1
        : IEquatable<LLVMOpInfoSymbol1>
    {
        public readonly UInt64 Present;
        public readonly byte* Name;
        public readonly UInt64 Value;

        public override bool Equals( object? obj )
        {
            return obj is LLVMOpInfoSymbol1 v && Equals( v );
        }

        public bool Equals( LLVMOpInfoSymbol1 other )
            => Present == other.Present
            && Name == other.Name
            && Value == other.Value;

        public override int GetHashCode( )
        {
            return HashCode.Combine( Present, (nint)Name, Value );
        }

        public static bool operator ==( LLVMOpInfoSymbol1 left, LLVMOpInfoSymbol1 right ) => left.Equals( right );

        public static bool operator !=( LLVMOpInfoSymbol1 left, LLVMOpInfoSymbol1 right ) => !(left == right);
    }

    [StructLayout( LayoutKind.Sequential )]
    public unsafe readonly struct LLVMOpInfo1
            : IEquatable<LLVMOpInfo1>
    {
        public readonly LLVMOpInfoSymbol1 AddSymbol;
        public readonly LLVMOpInfoSymbol1 SubtractSymbol;
        public readonly UInt64 Value;
        public readonly UInt64 VariantKind;

        public override bool Equals( object? obj )
        {
            return obj is LLVMOpInfo1 v && Equals( v );
        }

        public bool Equals( LLVMOpInfo1 other )
        {
            return AddSymbol.Equals( other.AddSymbol )
                && SubtractSymbol.Equals( other.SubtractSymbol )
                && Value == other.Value
                && VariantKind == other.VariantKind;
        }

        public override int GetHashCode( )
        {
            return HashCode.Combine( AddSymbol, SubtractSymbol, Value, VariantKind );
        }

        public static bool operator ==( LLVMOpInfo1 left, LLVMOpInfo1 right ) => left.Equals( right );

        public static bool operator !=( LLVMOpInfo1 left, LLVMOpInfo1 right ) => !(left == right);
    }
}

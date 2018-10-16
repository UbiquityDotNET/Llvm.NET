// <copyright file="IJitModuleHandle.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.JIT
{
    /// <summary>Opaque interface for JIT engine module 'handles'</summary>
    /// <remarks>
    /// <note type="warning">
    /// This is effectively a weak reference and operations on the engine are allowed to invalidate the handle.
    /// </note>
    /// </remarks>
    public interface IJitModuleHandle
        : IEquatable<IJitModuleHandle>
    {
    }

    /// <summary>Generic value type as a typedef for JIT module handles</summary>
    /// <typeparam name="T">Actual JIT engine handle type</typeparam>
    internal struct JitModuleHandle<T>
        : IJitModuleHandle
    {
        public static implicit operator T( JitModuleHandle<T> typeDef ) => typeDef.Value;

        public static implicit operator JitModuleHandle<T>( T value ) => new JitModuleHandle<T>( value );

        public override bool Equals( object obj ) => obj is JitModuleHandle<T> && Value.Equals( obj );

        public override int GetHashCode( ) => Value.GetHashCode( );

        public override string ToString( ) => Value.ToString( );

        public bool Equals( IJitModuleHandle other ) => other is JitModuleHandle<T> && Value.Equals( other );

        public static bool operator ==( JitModuleHandle<T> lhs, IJitModuleHandle rhs ) => lhs.Equals( rhs );

        public static bool operator !=( JitModuleHandle<T> lhs, IJitModuleHandle rhs ) => !lhs.Equals( rhs );

        private JitModuleHandle( T value )
        {
            Value = value;
        }

        private readonly T Value;
    }
}

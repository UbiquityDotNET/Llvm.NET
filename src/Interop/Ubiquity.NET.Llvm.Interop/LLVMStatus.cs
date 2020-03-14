// <copyright file="Generated.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>maps to LLVMBool in LLVM-C for methods that return 0 on success.</summary>
    /// <remarks>
    /// This was hand added to help clarify use when a return value is not really
    /// a bool but a status where (0==SUCCESS)
    /// </remarks>
    public struct LLVMStatus : System.IEquatable<LLVMStatus>
    {
        /// <summary>Gets a value indicating whether the operation was successful</summary>
        public bool Succeeded => ErrorCode == 0;

        /// <summary>Gets a value indicating whether the operation Failed</summary>
        public bool Failed => !Succeeded;

        /// <summary>Gets a the raw integer value of the status code</summary>
        public int ErrorCode { get; }

        /// <inheritdoc/>
        public override bool Equals( object obj )
        {
            return ErrorCode.Equals( obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return ErrorCode.GetHashCode( );
        }

        /// <summary>Compares two status values for equality</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are equal</returns>
        public static bool operator ==( LLVMStatus lhs, LLVMStatus rhs ) => lhs.ErrorCode.Equals( rhs.ErrorCode );

        /// <summary>Compares two status values for inequality</summary>
        /// <param name="lhs">Left side of comparison</param>
        /// <param name="rhs">Right side of comparison</param>
        /// <returns><see langword="true"/> if the handles are not equal</returns>
        public static bool operator !=( LLVMStatus lhs, LLVMStatus rhs ) => !( lhs == rhs );

        /// <inheritdoc/>
        public bool Equals( LLVMStatus other )
        {
            return ErrorCode == other.ErrorCode;
        }

        internal LLVMStatus( int value )
        {
            ErrorCode = value;
        }
    }
}

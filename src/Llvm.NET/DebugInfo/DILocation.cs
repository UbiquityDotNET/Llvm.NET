// <copyright file="DILocation.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug source location information</summary>
    public class DILocation
        : MDNode
    {
        /// <summary>Initializes a new instance of the <see cref="DILocation"/> class.</summary>
        /// <param name="context">Context that owns this location</param>
        /// <param name="line">line number for the location</param>
        /// <param name="column">Column number for the location</param>
        /// <param name="scope">Containing scope for the location</param>
        public DILocation( Context context, uint line, uint column, DILocalScope scope )
            : this( context, line, column, scope, null )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DILocation"/> class.</summary>
        /// <param name="context">Context that owns this location</param>
        /// <param name="line">line number for the location</param>
        /// <param name="column">Column number for the location</param>
        /// <param name="scope">Containing scope for the location</param>
        /// <param name="inlinedAt">Scope where this scope is inlined at/into</param>
        public DILocation( Context context, uint line, uint column, DILocalScope scope, DILocation inlinedAt )
            : base( LLVMDILocation( context.ValidateNotNull( nameof( context ) ).ContextHandle
                                  , line
                                  , column
                                  , scope.ValidateNotNull(nameof(scope)).MetadataHandle
                                  , inlinedAt?.MetadataHandle ?? default
                                  )
                  )
        {
        }

        /// <summary>Gets the scope for this location</summary>
        public DILocalScope Scope => FromHandle<DILocalScope>( Context, LLVMGetDILocationScope( MetadataHandle ) );

        /// <summary>Gets the line for this location</summary>
        public uint Line => LLVMGetDILocationLine( MetadataHandle );

        /// <summary>Gets the column for this location</summary>
        public uint Column => LLVMGetDILocationColumn( MetadataHandle );

        /// <summary>Gets the location this location is inlined at</summary>
        [property: CanBeNull]
        public DILocation InlinedAt => FromHandle<DILocation>( LLVMGetDILocationInlinedAt( MetadataHandle ) );

        /// <summary>Gets the scope where this is inlined.</summary>
        /// <remarks>This walks through the <see cref="InlinedAt"/> properties to return
        /// a <see cref="DILocalScope"/> from the deepest location.
        /// </remarks>
        public DILocalScope InlinedAtScope => FromHandle<DILocalScope>( LLVMDILocationGetInlinedAtScope( MetadataHandle ) );

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{Scope.File}({Line},{Column})";
        }

        internal DILocation( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef /*DILocalScope*/ LLVMGetDILocationScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern UInt32 LLVMGetDILocationLine( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern UInt32 LLVMGetDILocationColumn( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef /*DILocation*/ LLVMGetDILocationInlinedAt( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMMetadataRef LLVMDILocation( LLVMContextRef context, UInt32 Line, UInt32 Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt );
    }
}

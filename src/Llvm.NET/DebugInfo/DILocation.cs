// <copyright file="DILocation.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using JetBrains.Annotations;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

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
        public DILocalScope Scope => GetOperand<DILocalScope>( 0 );

        /// <summary>Gets the line for this location</summary>
        public uint Line => LLVMGetDILocationLine( MetadataHandle );

        /// <summary>Gets the column for this location</summary>
        public uint Column => LLVMGetDILocationColumn( MetadataHandle );

        /// <summary>Gets the location this location is inlined at</summary>
        [property: CanBeNull]
        public DILocation InlinedAt => Operands.Count < 2 ? null : GetOperand<DILocation>( 3 );

        /// <summary>Gets the scope where this is inlined.</summary>
        /// <remarks>This walks through the <see cref="InlinedAt"/> properties to return
        /// a <see cref="DILocalScope"/> from the deepest location.
        /// </remarks>
        public DILocalScope InlinedAtScope
        {
            get
            {
                var ia = InlinedAt;
                if( ia != null )
                {
                    return ia.InlinedAtScope;
                }

                return Scope;
            }
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return $"{Scope.File}({Line},{Column})";
        }

        internal DILocation( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

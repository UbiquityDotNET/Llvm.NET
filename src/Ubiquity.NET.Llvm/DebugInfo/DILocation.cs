// -----------------------------------------------------------------------
// <copyright file="DILocation.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug source location information</summary>
    public class DILocation
        : MDNode
    {
        /// <summary>Initializes a new instance of the <see cref="DILocation"/> class.</summary>
        /// <param name="context">ContextAlias that owns this location</param>
        /// <param name="line">line number for the location</param>
        /// <param name="column">Column number for the location</param>
        /// <param name="scope">Containing scope for the location</param>
        public DILocation( IContext context, uint line, uint column, DILocalScope scope )
            : this( context, line, column, scope, null )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DILocation"/> class.</summary>
        /// <param name="context">ContextAlias that owns this location</param>
        /// <param name="line">line number for the location</param>
        /// <param name="column">Column number for the location</param>
        /// <param name="scope">Containing scope for the location</param>
        /// <param name="inlinedAt">Scope where this scope is inlined at/into</param>
        public DILocation( IContext context, uint line, uint column, DILocalScope scope, DILocation? inlinedAt )
            : base( LLVMDIBuilderCreateDebugLocation( context.ThrowIfNull().GetUnownedHandle()
                                                    , line
                                                    , column
                                                    , scope.ThrowIfNull().Handle
                                                    , inlinedAt?.Handle ?? default
                                                    )
                  )
        {
        }

        /// <summary>Gets the scope for this location</summary>
        public DILocalScope Scope => (DILocalScope)LLVMDILocationGetScope( Handle ).CreateMetadata()!;

        /// <summary>Gets the line for this location</summary>
        public uint Line => LLVMDILocationGetLine( Handle );

        /// <summary>Gets the column for this location</summary>
        public uint Column => LLVMDILocationGetColumn( Handle );

        /// <summary>Gets the location this location is inlined at</summary>
        public DILocation? InlinedAt => (DILocation?)LLVMDILocationGetInlinedAt( Handle ).CreateMetadata( );

        /// <summary>Gets the scope where this is inlined.</summary>
        /// <remarks>
        /// This walks through the <see cref="InlinedAt"/> properties to return
        /// a <see cref="DILocalScope"/> from the deepest location.
        /// </remarks>
        public DILocalScope? InlinedAtScope => (DILocalScope?)LibLLVMDILocationGetInlinedAtScope( Handle ).CreateMetadata( );

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

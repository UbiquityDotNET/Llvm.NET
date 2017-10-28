// <copyright file="DILocation.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    public class DILocation : MDNode
    {
        public DILocation( Context context, uint line, uint column, DILocalScope scope )
            : this( context, line, column, scope, null )
        {
        }

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

        public DILocalScope Scope => FromHandle<DILocalScope>( LLVMGetDILocationScope( MetadataHandle ) );

        public uint Line => LLVMGetDILocationLine( MetadataHandle );

        public uint Column => LLVMGetDILocationColumn( MetadataHandle );

        public DILocation InlinedAt
        {
            get
            {
                var handle = LLVMGetDILocationInlinedAt( MetadataHandle );
                return FromHandle<DILocation>( handle );
            }
        }

        public DILocalScope InlinedAtScope
        {
            get
            {
                var handle = LLVMDILocationGetInlinedAtScope( MetadataHandle );
                return FromHandle<DILocalScope>( handle );
            }
        }

        public override string ToString( )
        {
            return $"{Scope.File}({Line},{Column})";
        }

        public bool Describes( Function function )
        {
            return Scope.SubProgram.Describes( function )
                || InlinedAtScope.SubProgram.Describes( function );
        }

        internal DILocation( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

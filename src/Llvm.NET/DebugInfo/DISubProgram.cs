// <copyright file="DISubProgram.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

namespace Llvm.NET.DebugInfo
{
    /// <summary>see <a href="http://llvm.org/docs/LangRef.html#disubprogram"/></summary>
    [SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "It is already correct 8^)" )]
    public class DISubProgram : DILocalScope
    {
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public bool Describes( Function function )
            => NativeMethods.SubProgramDescribes( MetadataHandle, function.ValidateNotNull( nameof( function )).ValueHandle );

        internal DISubProgram( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}

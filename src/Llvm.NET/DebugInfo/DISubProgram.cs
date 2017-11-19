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
            => NativeMethods.LLVMSubProgramDescribes( MetadataHandle, function.ValidateNotNull( nameof( function )).ValueHandle );

        internal DISubProgram( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        // TODO: Crack operands:
        //       File, Scope, Name,
        //       string LinagkeName (3),
        //       DISubroutineType Type( 4 )
        //       DICompileUnit Unit(5)
        //       DISubProgram Declaration(6)
        //       DILocalVariableArray Variables (7)
        //       DITypeRef ContainingType (8) // This and all remaiining ops are Optional and may not exist - must check operand count
        //       DITemplateParameterArray (9) TemplateParams
        //       DITypeArray ThrownTypes (10)
    }
}

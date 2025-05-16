// -----------------------------------------------------------------------
// <copyright file="DISubProgram.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a SubProgram</summary>
    /// <seealso href="xref:llvm_langref#disubprogram"/>
    [SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = "It is already correct 8^)" )]
    public sealed class DISubProgram
        : DILocalScope
    {
        /* TODO: Non-operand properties - need interop API to access these
            uint line{ get;}
            Virtuality Virtuality {get;}
            uint VirtualIndex {get;}
            int ThisAdjustment {get;}
            int ScopeLine {get;}
            DIFlags Flags {get;}
            bool IsLocalToUnit {get;}
        */

        /// <summary>Gets the source line associated with this <see cref="DISubProgram"/></summary>
        public uint Line => LLVMDISubprogramGetLine( Handle );

        /// <summary>Gets the name of this <see cref="DISubProgram"/></summary>
        public override LazyEncodedString Name => GetOperandString( 2 );

        /// <summary>Gets the linkage name of this <see cref="DISubProgram"/></summary>
        public string LinkageName => GetOperandString( 3 );

        /// <summary>Gets the signature of this <see cref="DISubProgram"/></summary>
        public DISubroutineType Signature => GetOperand<DISubroutineType>( 4 )!;

        /// <summary>Gets the <see cref="DICompileUnit"/> that contains this <see cref="DISubProgram"/></summary>
        public DICompileUnit CompileUnit => GetOperand<DICompileUnit>( 5 )!;

        /* TODO: CompileUnit set => LLVMDISubProgramReplaceUnit() - needs new interop API */

        /// <summary>Gets the <see cref="DISubProgram"/> that declares this <see cref="DISubProgram"/></summary>
        public DISubProgram Declaration => GetOperand<DISubProgram>( 6 )!;

        /// <summary>Gets the variables of this <see cref="DISubProgram"/></summary>
        public DILocalVariableArray Variables => new( GetOperand<MDTuple>( 7 )! );

        /// <summary>Gets the type that contains this <see cref="DISubProgram"/>, if any</summary>
        public DIType? ContainingType => Operands.Count < 9 ? null : GetOperand<DIType>( 8 );

        /// <summary>Gets the template parameters of this <see cref="DISubProgram"/>, if any</summary>
        public DITemplateParameterArray? TemplateParams => Operands.Count < 10 ? null : new DITemplateParameterArray( GetOperand<MDTuple>( 9 )! );

        /// <summary>Gets the exception types this <see cref="DISubProgram"/> can throw</summary>
        // Does the list include exceptions thrown by the complete call graph? or only those explicitly thrown by this function?
        public DITypeArray? ThrownTypes => Operands.Count < 11 ? null : new DITypeArray( GetOperand<MDTuple>( 10 ) );

        // TODO: Annotations (11) [Metadata? name suggests an array but likely not a type array...]
        // TODO: Target FuncName (12) [MDString]

        /// <summary>Determines if this instance describes a given <see cref="Function"/></summary>
        /// <param name="function"><see cref="Function"/> to test</param>
        /// <returns><see langword="true"/> if this <see cref="DISubProgram"/> describes <paramref name="function"/> </returns>
        public bool Describes( Function function )
        {
            return function.DISubProgram is not null && function.DISubProgram.Equals(this);
        }

        internal DISubProgram( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
